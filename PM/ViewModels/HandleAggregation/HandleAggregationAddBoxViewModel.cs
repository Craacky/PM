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
    public class HandleAggregationAddBoxViewModel : ViewModel
    {
        public DAL.Entities.Pallet CurrentPallet { get; set; }
        private DAL.Entities.Box CurrentBox { get; set; }


        public ScannedCode PalletCode { get; set; }
        public ScannedCode BoxCode { get; set; }
        public ScannedCode ProductCode { get; set; }


        public ProcessingCodeService ProcessingCodeService { get; set; }
        public ReportTaskService ReportTaskService { get; set; }
        public LocalDBService LocalDBService { get; set; }
        public ErrorsService ErrorsService { get; set; }

        public HandleAggregationAddBoxViewModel(ProcessingCodeService processingCodeService,
                                                    ReportTaskService reportTaskService,
                                                    LocalDBService localDBService,
                                                    ErrorsService errorService)
        {

            PalletCode = new ScannedCode();
            PalletCode.ProccessingScannedCode += ProccessingPalletCode;

            BoxCode = new ScannedCode();
            BoxCode.IsEnabledFieldScannedCode = false;
            BoxCode.ProccessingScannedCode += ProccessingBoxCode;

            ProductCode = new ScannedCode();
            ProductCode.IsEnabledFieldScannedCode = false;
            ProductCode.ProccessingScannedCode += ProccessingProductCode;


            ProcessingCodeService = processingCodeService;
            ReportTaskService = reportTaskService;
            LocalDBService = localDBService;
            ErrorsService = errorService;
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
                                if (pallets[0].IsFulled)
                                {
                                    MessageBox.Show("Найденная паллета заполненна");

                                    PalletCode.IsErrorStatusScannedCode = true;
                                    BoxCode.IsEnabledFieldScannedCode = false;
                                    ProductCode.IsEnabledFieldScannedCode = false;
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
                                PalletCode.StatusScannedCode = $"Найденно несколько паллет с данным кодом.\n" +
                                                               $"{code}.";

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


            CurrentBox = null;

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
                        if (!isRepeatBoxCode)
                        {
                            DAL.Entities.Box newBox = new DAL.Entities.Box()
                            {
                                MarkingCode = code,
                                ReportTaskGuid = ReportTaskService.CurrentReportTask.Guid,
                                LineId = ReportTaskService.CurrentReportTask.LineId,
                                PalletId = CurrentPallet.Id
                            };

                            CurrentBox = LocalDBService.BoxDataService.Create(newBox);

                            ReportTaskService.Statistic.BoxCodes.Add(CurrentBox);
                            ReportTaskService.Statistic.CountBoxes++;

                            CurrentPallet.Boxes.Add(CurrentBox);

                            if (CurrentPallet == ReportTaskService.Statistic.PalletCodes[^1])
                            {
                                ReportTaskService.Statistic.CountBoxInCurrentPallet++;
                            }

                            PalletCode.StatusScannedCode = $"Код паллеты.\n" +
                                                            $"{CurrentPallet.MarkingCode}.\n" +
                                                            $"Коробов в паллете {CurrentPallet.Boxes.Count}/{ReportTaskService.CurrentReportTask.CountBoxInPallet}.";

                            BoxCode.StatusScannedCode = $"Код короба создан.\n" +
                                                        $"{code}.\n" +
                                                        $"Продукта в коробе {CurrentBox.Products.Count}/{ReportTaskService.CurrentReportTask.CountProductInBox}.\n" +
                                                        $"Код паллеты {CurrentPallet.MarkingCode}.";

                            BoxCode.IsErrorStatusScannedCode = false;
                            ProductCode.IsEnabledFieldScannedCode = true;


                            Error newError = new Error { TypeError = "Неполный короб" };
                            newError.BoxCodes.Add(CurrentBox.MarkingCode);
                            newError.PalletCodes.Add(CurrentPallet.MarkingCode);

                            ErrorsService.Errors.Add(newError);
                            ErrorsService.Errors = new List<Error>(ErrorsService.Errors);
                        }
                        else
                        {
                            BoxCode.StatusScannedCode = $"Введённый код короба уже существует.\n" +
                                                        $"{code}.";

                            var boxes = LocalDBService.BoxDataService.GetAllWithInclude(p => p.MarkingCode.Replace("\u001d", "") == code.Replace("\u001d", "") && p.ReportTaskGuid == ReportTaskService.CurrentReportTask.Guid, p => p.Products).ToList();
                            for (int i = 0; i < boxes.Count; i++)
                            {
                                boxes[i].Pallet = LocalDBService.PalletDataService.Get((int)boxes[i].PalletId);
                            }

                            foreach (var box in boxes)
                            {
                                BoxCode.StatusScannedCode += $"\nКод паллеты {box.Pallet.MarkingCode}.";
                            }

                            BoxCode.IsErrorStatusScannedCode = true;

                            ProductCode.StatusScannedCode = string.Empty;
                            ProductCode.IsEnabledFieldScannedCode = false;

                        }
                    }
                    else
                    {
                        BoxCode.StatusScannedCode = $"Введённый код короба не соответствует коду короба текущего задания.\n{code}.";
                        BoxCode.IsErrorStatusScannedCode = true;

                        ProductCode.StatusScannedCode = string.Empty;
                        ProductCode.IsEnabledFieldScannedCode = false;
                    }
                }
                else
                {
                    BoxCode.StatusScannedCode = $"Введённый код не соответствует коду короба.\n{code}.";
                    BoxCode.IsErrorStatusScannedCode = true;

                    ProductCode.StatusScannedCode = string.Empty;
                    ProductCode.IsEnabledFieldScannedCode = false;
                }
            }

            BoxCode.Code = string.Empty;
        }

        private void ProccessingProductCode()
        {
            if (ProductCode.Code.Replace("\0", "") == string.Empty)
            {
                return;
            }


            string code = ProductCode.Code.Trim();
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
                        if (!isRepeatProductCode)
                        {
                            DAL.Entities.Product newProduct = new DAL.Entities.Product()
                            {
                                MarkingCode = code,
                                ReportTaskGuid = ReportTaskService.CurrentReportTask.Guid,
                                LineId = ReportTaskService.CurrentReportTask.LineId,
                                BoxId = CurrentBox.Id
                            };
                            newProduct = LocalDBService.ProductDataService.Create(newProduct);

                            CurrentBox.Products.Add(newProduct);

                            ReportTaskService.Statistic.ProductCodes.Add(newProduct);
                            ReportTaskService.Statistic.CountProducts++;


                            ProductCode.StatusScannedCode = $"Код продукта добавлен.\n" +
                                                            $"{code}.\n" +
                                                            $"Код короба {CurrentBox.MarkingCode}.\n" +
                                                            $"Код паллеты {CurrentPallet.MarkingCode}.";
                            
                           
                            BoxCode.StatusScannedCode = $"Код короба.\n" +
                                                        $"{CurrentBox.MarkingCode}.\n" +
                                                        $"Продукта в коробе {CurrentBox.Products.Count}/{ReportTaskService.CurrentReportTask.CountProductInBox}.\n" +
                                                        $"Код паллеты {CurrentPallet.MarkingCode}.";

                            PalletCode.StatusScannedCode = $"Код паллеты.\n" +
                                                           $"{CurrentPallet.MarkingCode}.\n" +
                                                           $"Коробов в паллете {CurrentPallet.Boxes.Count}/{ReportTaskService.CurrentReportTask.CountBoxInPallet}.";

                            ProductCode.IsErrorStatusScannedCode = false;


                            if (CurrentBox.Products.Count == Convert.ToInt32(ReportTaskService.CurrentReportTask.CountProductInBox))
                            {
                                MessageBox.Show("Короб заполнен");

                                var errors = ErrorsService.Errors.FindAll(e => e.BoxCodes.Contains(CurrentBox.MarkingCode) && e.TypeError == "Неполный короб");
                                foreach (var error in errors)
                                {
                                    ErrorsService.Errors.Remove(error);
                                }

                                ErrorsService.Errors = new List<Error>(ErrorsService.Errors);

                                CurrentBox = null;

                                BoxCode.Code = string.Empty;
                                BoxCode.StatusScannedCode = string.Empty;
                                BoxCode.IsEnabledFieldScannedCode = true;
                                BoxCode.IsErrorStatusScannedCode = true;

                                ProductCode.Code = string.Empty;
                                ProductCode.StatusScannedCode = string.Empty;
                                ProductCode.IsEnabledFieldScannedCode = false;
                            }

                        }
                        else
                        {
                            ProductCode.StatusScannedCode = $"Введённый код продукта уже существует.\n" +
                                                            $"{code}.";

                            var products = LocalDBService.ProductDataService.GetAllWithInclude((p => p.MarkingCode.Replace("\u001d", "") == code.Replace("\u001d", "") && p.ReportTaskGuid == ReportTaskService.CurrentReportTask.Guid), p => p.Box, p => p.Pallet).ToList();
                            foreach (var product in products)
                            {
                                ProductCode.StatusScannedCode += $"\nКод короба {product.Box.MarkingCode}.";
                                ProductCode.StatusScannedCode += $"\nКод паллеты {product.Box.Pallet.MarkingCode}.";
                            }
                            ProductCode.IsErrorStatusScannedCode = true;
                        }
                    }
                    else
                    {
                        ProductCode.StatusScannedCode = $"Введённый код продукта не соответствует коду продукта текущего задания.\n{code}.";
                        ProductCode.IsErrorStatusScannedCode = true;
                    }
                }
                else
                {
                    ProductCode.StatusScannedCode = $"Введённый код не соответствует коду продукта.\n{code}.";
                    ProductCode.IsErrorStatusScannedCode = true;
                }
            }

            ProductCode.Code = string.Empty;
        }
    }
}
