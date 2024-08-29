using PM.Devices;
using PM.Models;
using PM.Services;

using PM.ViewModels.Base;
using System.Collections.Generic;
using System.Linq;

namespace PM.ViewModels
{
    public class HandleAggregationDeletePalletViewModel : ViewModel
    {
        public ScannedCode PalletCode { get; set; }


        public ProcessingCodeService ProcessingCodeService { get; set; }
        public ReportTaskService ReportTaskService { get; set; }
        public LocalDBService LocalDBService { get; set; }
        public ErrorsService ErrorsService { get; set; }


        public HandleAggregationDeletePalletViewModel(ProcessingCodeService processingCodeService,
                                                      ReportTaskService reportTaskService,
                                                      LocalDBService localDBService,
                                                      ErrorsService errorsService)
        {
            PalletCode = new ScannedCode();
            PalletCode.ProccessingScannedCode += ProccessingPalletCode;

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

                                LocalDBService.PalletDataService.Delete(pallets[0].Id);
                                
                                //Очищаем ошибки связанные с паллетой
                                {
                                    var errors = ErrorsService.Errors.FindAll(e => e.PalletCodes.Contains(pallets[0].MarkingCode));
                                    foreach (var error in errors)
                                    {
                                        ErrorsService.Errors.Remove(error);
                                    }
                                    ErrorsService.Errors = new List<Error>(ErrorsService.Errors);
                                }

                                //Обновляем статистику
                                {
                                    ReportTaskService.Statistic.PalletCodes.RemoveAll(p => p.Id == pallets[0].Id);
                                    ReportTaskService.Statistic.PalletCodes = new List<DAL.Entities.Pallet>(ReportTaskService.Statistic.PalletCodes);

                                    foreach (var box in pallets[0].Boxes)
                                    {
                                       ReportTaskService.Statistic.BoxCodes.RemoveAll(b => b.PalletId == pallets[0].Id);

                                        foreach (var product in box.Products)
                                        {
                                            ReportTaskService.Statistic.ProductCodes.RemoveAll(b => b.BoxId == box.Id);
                                        }
                                    }

                                    ReportTaskService.Statistic.CountProducts = ReportTaskService.Statistic.ProductCodes.Count();
                                    ReportTaskService.Statistic.CountBoxes = ReportTaskService.Statistic.BoxCodes.Count();

                                    if (pallets[0] == ReportTaskService.Statistic.PalletCodes[^1])
                                    {
                                        if (ReportTaskService.Statistic.PalletCodes.Count > 1)
                                        {
                                            ReportTaskService.Statistic.CountBoxInCurrentPallet = ReportTaskService.Statistic.PalletCodes[^2].Boxes.Count;
                                        }
                                        else
                                        {
                                            ReportTaskService.Statistic.CountBoxInCurrentPallet = 0;
                                        }
                                    }

                                }


                                //Обновляем информацию о состоянии короба и текущей паллеты
                                {
                                    PalletCode.StatusScannedCode = $"Код паллеты удалён.\n" +
                                                               $"{code}.";

                                    PalletCode.IsErrorStatusScannedCode = false;
                                }

                            }
                            else
                            {
                                PalletCode.StatusScannedCode = $"Найденно несколько паллет с данным кодом." +
                                                                $"\n{code}.";
                                PalletCode.IsErrorStatusScannedCode = true;
                            }
                        }
                        else
                        {
                            PalletCode.StatusScannedCode = $"Введённый код паллеты не найден.\n{code}.";
                            PalletCode.IsErrorStatusScannedCode = true;
                        }
                    }
                    else
                    {
                        PalletCode.StatusScannedCode = $"Введённый код паллеты не соответствует коду паллеты текущего задания.\n{code}.";
                        PalletCode.IsErrorStatusScannedCode = true;
                    }
                }
                else
                {
                    PalletCode.StatusScannedCode = $"Введённый код не соответствует коду паллеты.\n{code}.";
                    PalletCode.IsErrorStatusScannedCode = true;
                }
            }

            PalletCode.Code = string.Empty;
        }
    }
}
