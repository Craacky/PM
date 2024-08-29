using PM.Devices;
using PM.Models;
using PM.Services;

using PM.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace PM.ViewModels
{
    public class HandleAggregationDeleteBoxViewModel : ViewModel
    {
        private DAL.Entities.Pallet CurrentPallet { get; set; }

        public ScannedCode PalletCode { get; set; }
        public ScannedCode BoxCode { get; set; }


        public ProcessingCodeService ProcessingCodeService { get; set; }
        public ReportTaskService ReportTaskService { get; set; }
        public LocalDBService LocalDBService { get; set; }
        public ErrorsService ErrorsService { get; set; }


        public HandleAggregationDeleteBoxViewModel(ProcessingCodeService processingCodeService,
                                                   ReportTaskService reportTaskService,
                                                   LocalDBService localDBService,
                                                   ErrorsService errorsService)
        {
            PalletCode = new ScannedCode();
            PalletCode.ProccessingScannedCode += ProccessingPalletCode;

            BoxCode = new ScannedCode();
            BoxCode.IsEnabledFieldScannedCode = false;
            BoxCode.ProccessingScannedCode += ProccessingBoxCode;

            ProcessingCodeService = processingCodeService;
            ReportTaskService = reportTaskService;
            LocalDBService = localDBService;
            ErrorsService = errorsService;
        }


        private void ProccessingPalletCode()
        {
            if (PalletCode.Code.Replace("\0", "") == string.Empty)
            {
                return;
            }


            CurrentPallet = null;

            string code = PalletCode.Code.Trim();
            if (code.Contains("\0"))
            {
                code = code.Replace("\0", "");

                bool isPalletCode = ProcessingCodeService.IsPalletCode(code);
                if (isPalletCode)
                {
                    bool isPalletCodeTheCurrentTask = ProcessingCodeService.IsPalletCodeTheCurrentTask(code);
                    if (isPalletCodeTheCurrentTask)
                    {
                        bool isRepeatPalletCode = ProcessingCodeService.IsRepeatPalletCode(code);
                        if (isRepeatPalletCode)
                        {
                            var pallets = LocalDBService.PalletDataService.GetAllWithInclude(p => p.MarkingCode.Replace("\u001d", "") == code.Replace("\u001d", "") && p.ReportTaskGuid == ReportTaskService.CurrentReportTask.Guid, p => p.Boxes, p => p.Products).ToList();
                            if (pallets.Count == 1)
                            {
                                PalletCode.StatusScannedCode = $"Найден код паллеты.\n" +
                                                            $"{code}.\n" +
                                                            $"Коробов в паллете {pallets[0].Boxes.Count}/{ReportTaskService.CurrentReportTask.CountBoxInPallet}.";
                                if(pallets[0].Boxes.Count == 0)
                                {
                                    MessageBox.Show("Паллета пустая");

                                    CurrentPallet = null;

                                    PalletCode.Code = string.Empty;
                                    PalletCode.StatusScannedCode = string.Empty;
                                    PalletCode.IsEnabledFieldScannedCode = true;
                                    PalletCode.IsErrorStatusScannedCode = true;

                                    BoxCode.Code = string.Empty;
                                    BoxCode.StatusScannedCode = string.Empty;
                                    BoxCode.IsEnabledFieldScannedCode = false;
                                }
                                else
                                {
                                    CurrentPallet = pallets[0];

                                    PalletCode.IsErrorStatusScannedCode = false;
                                    BoxCode.IsEnabledFieldScannedCode = true;
                                }
                            }
                            else
                            {
                                PalletCode.StatusScannedCode = $"Найденно несколько паллет с данным кодом." +
                                                               $"\n{code}.";

                                PalletCode.IsErrorStatusScannedCode = true;
                                BoxCode.StatusScannedCode = string.Empty;
                                BoxCode.IsEnabledFieldScannedCode = false;
                            }
                        }
                        else
                        {
                            PalletCode.StatusScannedCode = $"Введённый код паллеты не найден.\n{code}.";
                            PalletCode.IsErrorStatusScannedCode = true;
                            BoxCode.StatusScannedCode = string.Empty;
                            BoxCode.IsEnabledFieldScannedCode = false;
                        }
                    }
                    else
                    {
                        PalletCode.StatusScannedCode = $"Введённый код паллеты не соответствует коду паллеты текущего задания.\n{code}.";
                        PalletCode.IsErrorStatusScannedCode = true;
                        BoxCode.StatusScannedCode = string.Empty;
                        BoxCode.IsEnabledFieldScannedCode = false;
                    }
                }
                else
                {
                    PalletCode.StatusScannedCode = $"Введённый код не соответствует коду паллеты.\n{code}.";
                    PalletCode.IsErrorStatusScannedCode = true;
                    BoxCode.StatusScannedCode = string.Empty;
                    BoxCode.IsEnabledFieldScannedCode = false;
                }
            }

            PalletCode.Code = string.Empty;
        }
        private void ProccessingBoxCode()
        {
            if (BoxCode.Code.Replace("\0", "") == string.Empty)
            {
                return;
            }

            string code = BoxCode.Code.Trim();
            if (code.Contains("\0"))
            {
                code = code.Replace("\0", "");

                bool isBoxCode = ProcessingCodeService.IsBoxCode(code);
                if (isBoxCode)
                {
                    bool isBoxCodeTheCurrentTask = ProcessingCodeService.IsBoxCodeTheCurrentTask(code);
                    if (isBoxCodeTheCurrentTask)
                    {
                        bool isRepeatBoxCode = ProcessingCodeService.IsRepeatBoxCode(code);
                        if (isRepeatBoxCode)
                        {
                            var boxes = LocalDBService.BoxDataService.GetAllWithInclude(p => p.MarkingCode.Replace("\u001d", "") == code.Replace("\u001d", "") && p.ReportTaskGuid == ReportTaskService.CurrentReportTask.Guid, p => p.Products).ToList();
                            for (int i = 0; i < boxes.Count; i++)
                            {
                                boxes[i].Pallet = LocalDBService.PalletDataService.Get((int)boxes[i].PalletId);
                            }

                            if (boxes.Count == 1 || boxes.Where(b => b.PalletId == CurrentPallet.Id).Count() == 1)
                            {
                                PM.DAL.Entities.Box deleteBox = boxes.FirstOrDefault(b => b.PalletId == CurrentPallet.Id);

                                if (deleteBox != null)
                                {
                                    LocalDBService.BoxDataService.Delete(deleteBox.Id);

                                    //Очищаем ошибки связанные с коробом
                                    {
                                        var errors = ErrorsService.Errors.FindAll(e => e.BoxCodes.Contains(deleteBox.MarkingCode));
                                        foreach (var error in errors)
                                        {
                                            ErrorsService.Errors.Remove(error);
                                        }

                                        ErrorsService.Errors = new List<Error>(ErrorsService.Errors);
                                    }

                                    //Обновляем статистику
                                    {
                                        CurrentPallet.Boxes.RemoveAll(b => b.Id == deleteBox.Id);

                                        ReportTaskService.Statistic.BoxCodes.RemoveAll(p => p.Id == deleteBox.Id);
                                        ReportTaskService.Statistic.CountBoxes--;

                                        foreach (var product in deleteBox.Products)
                                        {
                                            ReportTaskService.Statistic.ProductCodes.RemoveAll(p => p.Id == product.Id);
                                        }
                                        ReportTaskService.Statistic.CountProducts = ReportTaskService.Statistic.ProductCodes.Count();

                                        if (CurrentPallet.Id == ReportTaskService.Statistic.PalletCodes[^1].Id)
                                        {
                                            ReportTaskService.Statistic.CountBoxInCurrentPallet--;

                                        }
                                    }

                                    //Обновляем информацию о состоянии короба и текущей паллеты
                                    {
                                        BoxCode.StatusScannedCode = $"Код короба удалён.\n" +
                                                                    $"{code}.";
                                        BoxCode.IsErrorStatusScannedCode = false;

                                        if (CurrentPallet.Boxes.Count == 0)
                                        {
                                            MessageBox.Show("Паллета пустая");

                                            CurrentPallet = null;

                                            PalletCode.Code = string.Empty;
                                            PalletCode.StatusScannedCode = string.Empty;
                                            PalletCode.IsEnabledFieldScannedCode = true;
                                            PalletCode.IsErrorStatusScannedCode = true;

                                            BoxCode.Code = string.Empty;
                                            BoxCode.StatusScannedCode = string.Empty;
                                            BoxCode.IsEnabledFieldScannedCode = false;
                                        }
                                        PalletCode.StatusScannedCode = $"Код паллеты.\n" +
                                                                        $"{CurrentPallet.MarkingCode}.\n" +
                                                                        $"Коробов в паллете {CurrentPallet.Boxes.Count}/{ReportTaskService.CurrentReportTask.CountBoxInPallet}.";
                                    }


                                }
                                else
                                {
                                    BoxCode.StatusScannedCode = $"Код короба найден в другой паллете.\n" +
                                                                $"{code}.\n" +
                                                                $"Код паллеты {boxes[0].Pallet.MarkingCode}.";
                                    BoxCode.IsErrorStatusScannedCode = true;
                                }
                            }
                            else 
                            {
                                var result = MessageBox.Show($"Найденно несколько коробов с данным кодом." +
                                                            $"\n{code}." +
                                                            $"Удалить последний найденный короб?", "", MessageBoxButton.YesNo);
                                if (result == MessageBoxResult.Yes)
                                {
                                    LocalDBService.BoxDataService.Delete(boxes[^1].Id);

                                    var errors = ErrorsService.Errors.FindAll(e => e.BoxCodes.Contains(boxes[^1].MarkingCode));
                                    foreach (var error in errors)
                                    {
                                        ErrorsService.Errors.Remove(error);
                                    }

                                    ErrorsService.Errors = new List<Error>(ErrorsService.Errors);

                                    CurrentPallet.Boxes.Remove(boxes[^1]);


                                    CurrentPallet.MarkingCode = CurrentPallet.MarkingCode.Split($"\u001d37")[0] + $"\u001d37" +$"{Convert.ToInt32(CurrentPallet.Boxes.Count) * Convert.ToInt32(ReportTaskService.CurrentReportTask.CountProductInBox)}";
                                    CurrentPallet = LocalDBService.PalletDataService.Update(CurrentPallet.Id, CurrentPallet);


                                    BoxCode.StatusScannedCode = $"Код короба удалён.\n" +
                                                                $"{code}.";

                                    BoxCode.IsErrorStatusScannedCode = false;
                                    ReportTaskService.Statistic.BoxCodes.Remove(boxes[^1]);
                                    ReportTaskService.Statistic.BoxCodes = new List<DAL.Entities.Box>(ReportTaskService.Statistic.BoxCodes);


                                    if (CurrentPallet == ReportTaskService.Statistic.PalletCodes[^1])
                                    {
                                        ReportTaskService.Statistic.CountBoxInCurrentPallet--;

                                    }

                                    if (CurrentPallet.Boxes.Count == 0)
                                    {
                                        MessageBox.Show("Паллета пустая");

                                        CurrentPallet = null;

                                        PalletCode.Code = string.Empty;
                                        PalletCode.StatusScannedCode = string.Empty;
                                        PalletCode.IsEnabledFieldScannedCode = true;
                                        PalletCode.IsErrorStatusScannedCode = true;

                                        BoxCode.Code = string.Empty;
                                        BoxCode.StatusScannedCode = string.Empty;
                                        BoxCode.IsEnabledFieldScannedCode = false;
                                    }

                                    foreach (var product in boxes[^1].Products)
                                    {
                                        ReportTaskService.Statistic.ProductCodes.Remove(product);
                                    }

                                    ReportTaskService.Statistic.ProductCodes = new List<DAL.Entities.Product>(ReportTaskService.Statistic.ProductCodes);

                                    PalletCode.StatusScannedCode = $"Код паллеты.\n" +
                                                                    $"{code}.\n" +
                                                                    $"Коробов в паллете {CurrentPallet.Boxes.Count}/{ReportTaskService.CurrentReportTask.CountBoxInPallet}.";
                                }
                                else
                                {
                                    BoxCode.IsErrorStatusScannedCode = true;

                                    BoxCode.StatusScannedCode = $"Найденно несколько коробов с данным кодом." +
                                                                $"\n{code}.";
                                    foreach (var box in boxes)
                                    {
                                        BoxCode.StatusScannedCode += $"\nКод паллеты {box.Pallet.MarkingCode}.";
                                    }
                                }
                            }
                        }
                        else
                        {
                            BoxCode.StatusScannedCode = $"Введённый код короба не существует.\n" +
                                                        $"{code}.";
                            BoxCode.IsErrorStatusScannedCode = true;
                        }
                    }
                    else
                    {
                        BoxCode.StatusScannedCode = $"Введённый код короба не соответствует коду короба текущего задания.\n{code}.";
                        BoxCode.IsErrorStatusScannedCode = true;
                    }
                }
                else
                {
                    BoxCode.StatusScannedCode = $"Введённый код не соответствует коду короба.\n{code}.";
                    BoxCode.IsErrorStatusScannedCode = true;
                }
            }

            BoxCode.Code = string.Empty;
        }
    }
}
