using PM.Devices;
using PM.Models;
using PM.Services;
using PM.ViewModels.Base;
using System.Collections.Generic;
using System.Linq;

namespace PM.ViewModels
{
    public class HandleAggregationCheckCodeViewModel : ViewModel
    {
        public ScannedCode ScannedCode { get; set; }


        public ProcessingCodeService ProcessingCodeService { get; set; }
        public ReportTaskService ReportTaskService { get; set; }
        public LocalDBService LocalDBService { get; set; }


        public HandleAggregationCheckCodeViewModel(ProcessingCodeService processingCodeService,
                                                   ReportTaskService reportTaskService,
                                                   LocalDBService localDBService)
        {
            ScannedCode = new ScannedCode();
            ScannedCode.ProccessingScannedCode += ProccessingCode;

            ProcessingCodeService = processingCodeService;
            ReportTaskService = reportTaskService;
            LocalDBService = localDBService;
        }


        private void ProccessingCode()
        {
            if (ScannedCode.Code.Replace("\0", "") == string.Empty)
            {
                return;
            }

            string code = ScannedCode.Code.Trim();
            if (code.Contains("\0"))
            {
                code = code.Replace("\0", "");

                bool isProductCode = ProcessingCodeService.IsProductCode(code);
                if (isProductCode)
                {
                    bool isProductCodeTheCurrentTask = ProcessingCodeService.IsProductCodeTheCurrentTask(code);
                    if (isProductCodeTheCurrentTask)
                    {
                        bool isRepeatProductCode = ProcessingCodeService.IsRepeatProductCode(code);
                        if (isRepeatProductCode)
                        {
                            var products = LocalDBService.ProductDataService.GetAllWithInclude((p => p.MarkingCode.Replace("\u001d", "") == code.Replace("\u001d", "") && p.ReportTaskGuid == ReportTaskService.CurrentReportTask.Guid), p => p.Box, p => p.Pallet).ToList();
                            if (products.Count == 1)
                            {
                                ScannedCode.StatusScannedCode = $"Найден код продукта.\n" +
                                                                $"{code}.\n" +
                                                                $"Код короба {products[0].Box.MarkingCode}.\n" +
                                                                $"Код паллеты {products[0].Box.Pallet.MarkingCode}.";
                            }
                            else
                            {
                                ScannedCode.StatusScannedCode = $"Найден несколько кодов продукта.\n" +
                                                                 $"{code}.";
                                                                 
                                foreach (var product in products)
                                {
                                    ScannedCode.StatusScannedCode += $"\nКод короба {product.Box.MarkingCode}.";
                                    ScannedCode.StatusScannedCode += $"\nКод паллеты {product.Box.Pallet.MarkingCode}.";
                                }
                            }

                            ScannedCode.IsErrorStatusScannedCode = false;
                        }
                        else
                        {
                            ScannedCode.StatusScannedCode = $"Введённый код продукта не найден.\n{code}.";
                            ScannedCode.IsErrorStatusScannedCode = true;
                        }
                    }
                    else
                    {
                        ScannedCode.StatusScannedCode = $"Введённый код продукта не соответствует коду продукта текущего задания.\n{code}.";
                        ScannedCode.IsErrorStatusScannedCode = true;
                    }
                }
                else
                {
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

                                if (boxes.Count == 1)
                                {
                                    ScannedCode.StatusScannedCode = $"Найден код короба.\n" +
                                                                   $"{code}.\n" +
                                                                   $"Продукта в коробе {boxes[0].Products.Count}/{ReportTaskService.CurrentReportTask.CountProductInBox}.\n" +
                                                                   $"Код паллеты {boxes[0].Pallet.MarkingCode}";
                                }
                                else
                                {
                                    ScannedCode.StatusScannedCode = $"Найден несколько кодов коробов.\n" +
                                                                    $"{code}.";
                                    foreach (var box in boxes)
                                    {
                                        ScannedCode.StatusScannedCode += $"\nПродукта в коробе {box.Products.Count}/{ReportTaskService.CurrentReportTask.CountProductInBox}.";
                                        ScannedCode.StatusScannedCode += $"\nКод паллеты {box.Pallet.MarkingCode}.";
                                    }
                                }
                                ScannedCode.IsErrorStatusScannedCode = false;
                            }
                            else
                            {
                                ScannedCode.StatusScannedCode = $"Введённый код короба не найден.\n{code}.";
                                ScannedCode.IsErrorStatusScannedCode = true;
                            }
                        }
                        else
                        {
                            ScannedCode.StatusScannedCode = $"Введённый код короба не соответствует коду короба текущего задания.\n{code}.";
                            ScannedCode.IsErrorStatusScannedCode = true;
                        }
                    }
                    else
                    {
                        bool isPalletCode = ProcessingCodeService.IsPalletCode(code);
                        if (isPalletCode)
                        {
                            bool isPalletCodeTheCurrentTask = ProcessingCodeService.IsPalletCodeTheCurrentTask(code);
                            if (isPalletCodeTheCurrentTask)
                            {
                                bool isRepeatPalletCode = ProcessingCodeService.IsRepeatPalletCode(code);
                                if (isRepeatPalletCode)
                                { 
                                    var pallets = LocalDBService.PalletDataService.GetAllWithInclude(p => p.MarkingCode.Replace("\u001d", "") == code.Replace("\u001d", "") && p.ReportTaskGuid == ReportTaskService.CurrentReportTask.Guid, p => p.Boxes, p=> p.Products).ToList();
                                    if (pallets.Count == 1)
                                    {
                                        ScannedCode.StatusScannedCode = $"Найден код паллеты.\n" +
                                                                       $"{code}.\n" +
                                                                       $"Коробов в паллете {pallets[0].Boxes.Count}/{ReportTaskService.CurrentReportTask.CountBoxInPallet}.";
                                    }
                                    else
                                    {
                                        ScannedCode.StatusScannedCode = $"Найден несколько кодов паллеты.\n" +
                                                                        $"{code}.";
                                        foreach (var pallet in pallets)
                                        {
                                            ScannedCode.StatusScannedCode += $"Коробов в паллете {pallet.Boxes.Count}/{ReportTaskService.CurrentReportTask.CountBoxInPallet}.";
                                        }
                                    }

                                    ScannedCode.IsErrorStatusScannedCode = false;
                                }
                                else
                                {
                                    ScannedCode.StatusScannedCode = $"Введённый код паллеты не найден.\n{code}.";
                                    ScannedCode.IsErrorStatusScannedCode = true;
                                }
                            }
                            else
                            {
                                ScannedCode.StatusScannedCode = $"Введённый код паллеты не соответствует коду короба текущего задания.\n{code}.";
                                ScannedCode.IsErrorStatusScannedCode = true;
                            }
                        }
                        else
                        {
                            ScannedCode.StatusScannedCode = $"Введённый код не соответсвует ни одному из шаблонов кодов.\n{code}.";
                            ScannedCode.IsErrorStatusScannedCode = true;
                        }
                    }
                }
            }
            ScannedCode.Code = string.Empty;
        }
    }
}
