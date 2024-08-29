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
    public class HandleAggregationAddProductViewModel : ViewModel
    {
        private DAL.Entities.Box CurrentBox { get; set; }

        public ScannedCode BoxCode { get; set; }
        public ScannedCode ProductCode { get; set; }


        public ProcessingCodeService ProcessingCodeService { get; set; }
        public ReportTaskService ReportTaskService { get; set; }
        public LocalDBService LocalDBService { get; set; }
        public ErrorsService ErrorsService { get; set; }


        public HandleAggregationAddProductViewModel(ProcessingCodeService processingCodeService,
                                                    ReportTaskService reportTaskService,
                                                    LocalDBService localDBService,
                                                    ErrorsService errorsService)
        {
            BoxCode = new ScannedCode();
            BoxCode.ProccessingScannedCode += ProccessingBoxCode;

            ProductCode = new ScannedCode();
            ProductCode.IsEnabledFieldScannedCode = false;
            ProductCode.ProccessingScannedCode += ProccessingProductCode;

            ProcessingCodeService = processingCodeService;
            ReportTaskService = reportTaskService;
            LocalDBService = localDBService;
            ErrorsService = errorsService;
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
                        if (isRepeatBoxCode)
                        {
                            var boxes = LocalDBService.BoxDataService.GetAllWithInclude(p => p.MarkingCode.Replace("\u001d", "") == code.Replace("\u001d", "") && p.ReportTaskGuid == ReportTaskService.CurrentReportTask.Guid, p => p.Products).ToList();
                            for (int i = 0; i < boxes.Count; i++)
                            {
                                boxes[i].Pallet = LocalDBService.PalletDataService.Get((int)boxes[i].PalletId);
                            }

                            if (boxes.Count == 1)
                            {
                                BoxCode.StatusScannedCode = $"Найден код короба.\n" +
                                                            $"{code}.\n" +
                                                            $"Продукта в коробе {boxes[0].Products.Count}/{ReportTaskService.CurrentReportTask.CountProductInBox}.\n" +
                                                            $"Код паллеты {boxes[0].Pallet.MarkingCode}.";

                                if (boxes[0].Products.Count == Convert.ToInt32(ReportTaskService.CurrentReportTask.CountProductInBox))
                                {
                                    MessageBox.Show("Найденны короб заполнен");
                                    
                                    BoxCode.IsErrorStatusScannedCode = true;
                                    ProductCode.IsEnabledFieldScannedCode = false;
                                }
                                else
                                {
                                    CurrentBox = boxes[0];

                                    BoxCode.IsErrorStatusScannedCode = false;
                                    ProductCode.IsEnabledFieldScannedCode = true;
                                }
                            }
                            else
                            {
                                BoxCode.StatusScannedCode = $"Найденно несколько коробов с данным кодом." +
                                                            $"\n{code}.";
                                foreach (var box in boxes)
                                {
                                    BoxCode.StatusScannedCode += $"\nКод паллеты {boxes[0].Pallet.MarkingCode}.";
                                }
                                BoxCode.IsErrorStatusScannedCode = true;

                                ProductCode.StatusScannedCode = string.Empty;
                                ProductCode.IsEnabledFieldScannedCode = false;
                            }
                        }
                        else
                        {
                            BoxCode.StatusScannedCode = $"Введённый код короба не найден.\n{code}.";
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

                            ProductCode.StatusScannedCode = $"Код продукта добавлен.\n" +
                                                            $"{code}.\n" +
                                                            $"Код короба {CurrentBox.MarkingCode}.\n" +
                                                            $"Код паллеты {CurrentBox.Pallet.MarkingCode}.";
                            
                            ProductCode.IsErrorStatusScannedCode = false;

                            ReportTaskService.Statistic.ProductCodes.Add(newProduct);
                            ReportTaskService.Statistic.CountProducts++;
                           
                            CurrentBox.Products.Add(newProduct);


                            BoxCode.StatusScannedCode = $"Код короба.\n" +
                                                        $"{CurrentBox.MarkingCode}.\n" +
                                                        $"Продукта в коробе {CurrentBox.Products.Count}/{ReportTaskService.CurrentReportTask.CountProductInBox}.\n" +
                                                        $"Код паллеты {CurrentBox.Pallet.MarkingCode}.";

                            if (CurrentBox.Products.Count == Convert.ToInt32(ReportTaskService.CurrentReportTask.CountProductInBox))
                            {
                                MessageBox.Show("Короб заполнен");

                                var errors = ErrorsService.Errors.FindAll(e => e.BoxCodes.Contains(CurrentBox.MarkingCode) && e.TypeError=="Неполный короб");
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
