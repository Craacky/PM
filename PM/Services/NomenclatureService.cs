using PM.DAL.Entities;
using PM.Devices;
using PM.Formats.JSON.Services;
using PM.Models;
using PM.Models.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace PM.Services
{
    public class NomenclatureService : ObservableObject
    {
        private bool _isStartLodingNomenclatureAsync;


        private List<Nomenclature> nomenclatures;
        public List<Nomenclature> Nomenclatures
        {
            get => nomenclatures;
            set
            {
                nomenclatures = value;
                OnPropertyChanged(nameof(Nomenclatures));
            }
        }


        public JsonService JsonService { get; set; }
        public LocalDBService LocalDBService { get; set; }


        public NomenclatureService(LocalDBService localDBService)
        {
            LocalDBService = localDBService;

            _isStartLodingNomenclatureAsync = false;
            Nomenclatures = new List<Nomenclature>();

            //List<Nomenclature> dbNomenclatures = LocalDBService.NomenclatureDataService.GetAllWithInclude(n => n.Attributes).ToList();
            //foreach (Nomenclature dbNomenclature in dbNomenclatures)
            //{
            //    bool isNewNomenclatureGtin = !Nomenclatures.Any(n => n.Gtin == dbNomenclature.Gtin);
            //    if (isNewNomenclatureGtin)
            //    {
            //        Nomenclatures.Add(dbNomenclature);
            //    }
            //}
        }


        private IEnumerable<string> GetNewJsonFileName(DirectoryInfo directoryInfo)
        {
            FileInfo[] inFileNamesFromDirectory = directoryInfo.GetFiles("*.in");
            foreach (FileInfo inFileName in inFileNamesFromDirectory)
            {
                string guidFormInFileName = inFileName.Name.Split(".")[0];
                bool isContainInFileNameGuid = Guid.TryParse(guidFormInFileName, out Guid guid);
                if (isContainInFileNameGuid)
                {
                    //bool isNewOutFile = !Nomenclatures.Any(n => n.Guid == guid);
                   // if (isNewOutFile)
                    {
                        string jsonFileName = inFileName.FullName.Replace(".in", ".json");
                        bool isExistJsonFile = File.Exists(jsonFileName);
                        if (isExistJsonFile)
                        {
                            yield return jsonFileName;
                        }
                    }
                }
            }
        }
        private IEnumerable<Nomenclature> ConvertJsonMarkingTaskToDBNomenclature(Formats.JSON.Objects.Read.MarkingTask task, string guid)
        {
            IEnumerable<Nomenclature> nomenclatures = new List<Nomenclature>();

            foreach (var jsonNomenclature in task.Nomenclatures)
            {
                Nomenclature dbNomenclature = new()
                {
                    Guid = Guid.Parse(guid),
                    Code = jsonNomenclature.Code,
                    ExporterCode = jsonNomenclature.ExporterCode,
                    GrpCode = jsonNomenclature.GrpCode,
                    Name = jsonNomenclature.Name,
                    Gtin = jsonNomenclature.Gtin,
                    ArtCode = jsonNomenclature.ArtCode,
                    Description = jsonNomenclature.Description,
                    Attributes = new List<DAL.Entities.Attribute>()
                };

                foreach (var jsonAttribute in jsonNomenclature.Attributes)
                {
                    DAL.Entities.Attribute dbAttribute = new()
                    {
                        Code = (int)jsonAttribute.Code,
                        Value = jsonAttribute.Value,
                    };
                    dbNomenclature.Attributes.Add(dbAttribute);
                }
                nomenclatures = nomenclatures.Append(dbNomenclature);
            }

            return nomenclatures;

        }
        private async void CheckingOnDeleteNomenclature(string directoryPath)
        {
            await Task.Run(async () =>
            {
                string[] inFileNamesFromDirectory = Directory.GetFiles(directoryPath, "*.in");
                for (int i = 0; i < Nomenclatures.Count; i++)
                {
                    bool isExistMarkingTaskInDB = inFileNamesFromDirectory.Any(a => a.ToUpper().Contains(Nomenclatures[i].Guid.ToString().ToUpper()));
                    if (!isExistMarkingTaskInDB)
                    {
                        IEnumerable<Nomenclature> deleteDBNomenclatures = LocalDBService.NomenclatureDataService.GetAllWithInclude(mt => mt.Guid == Nomenclatures[i].Guid, n => n.Attributes);
                        if (deleteDBNomenclatures != null)
                        {
                            foreach (Nomenclature deleteDBNomenclature in deleteDBNomenclatures)
                            {
                                await LocalDBService.NomenclatureDataService.DeleteAsync(deleteDBNomenclature.Id);
                            }
                        }

                        Nomenclatures.Remove(Nomenclatures[i]);
                        Nomenclatures = new List<Nomenclature>(Nomenclatures);
                    }
                }
            });
        }


        public void StartLodingNomenclatureAsync(string directoryPath)
        {
            Task.Run(async () =>
            {
                if (_isStartLodingNomenclatureAsync)
                {
                    StopLodingNomenclatures();
                }

                _isStartLodingNomenclatureAsync = true;

                DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
                //if(!directoryInfo.Exists)
                //{
                //    MessageBox.Show($"При попытки подключиться к папке с номенклатурами произошла ошибка.\n" +
                //                    $"{directoryPath}"+
                //                    $"Указанная папка либо отсутсутвует либо отсутвует подключение к ней.\n" +
                //                    $"Новые номенклатуры не будут загружены");
                //}

                while (_isStartLodingNomenclatureAsync)
                {
                    directoryInfo = new DirectoryInfo(directoryPath);
                    if (directoryInfo.Exists)
                    {
                        foreach (string jsonFileName in GetNewJsonFileName(directoryInfo))
                        {
                            Formats.JSON.Objects.Read.MarkingTask jsonMarkingTask = JsonService.Read(jsonFileName);
                            bool isFullJsonMarkingTask = !JsonService.HasEmtyObjectOrFields(jsonMarkingTask);
                            if (isFullJsonMarkingTask)
                            {
                                string guidFormJsonFileName = jsonFileName.Split(".json")[0].Split("\\")[^1];
                                IEnumerable<Nomenclature> newDBNomenclatures = ConvertJsonMarkingTaskToDBNomenclature(jsonMarkingTask, guidFormJsonFileName);
                                foreach (Nomenclature newDBNomenclature in newDBNomenclatures)
                                {
                                    Nomenclature oldObject = LocalDBService.NomenclatureDataService.GetAllWithInclude(n => n.Code == newDBNomenclature.Code, n => n.Attributes).FirstOrDefault();
                                    if(oldObject == null)
                                    {
                                        await LocalDBService.NomenclatureDataService.CreateAsync(newDBNomenclature);
                                    }
                                    else
                                    {
                                        await LocalDBService.NomenclatureDataService.UpdateAsync(oldObject.Id, newDBNomenclature);
                                    }

                                    bool isNewNomenclatureGtin = !Nomenclatures.Any(n => n.Gtin == newDBNomenclature.Gtin);
                                    if (isNewNomenclatureGtin)
                                    {
                                        Nomenclatures.Add(newDBNomenclature);
                                        Nomenclatures = new List<Nomenclature>(Nomenclatures);
                                    }
                                }
                            }
                        }

                        CheckingOnDeleteNomenclature(directoryPath);
                    }

                    Thread.Sleep(100000);

                    StopLodingNomenclatures();
                }
            });
        }
        public void StopLodingNomenclatures()
        {
            _isStartLodingNomenclatureAsync = false;
        }
    }
}
