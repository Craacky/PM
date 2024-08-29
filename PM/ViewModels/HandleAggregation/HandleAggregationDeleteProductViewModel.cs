using PM.Devices;
using PM.Models;
using PM.Services;

using PM.ViewModels.Base;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace PM.ViewModels
{
    public class HandleAggregationDeleteProductViewModel : ViewModel
    {
        private DAL.Entities.Box CurrentBox { get; set; }


        public ScannedCode BoxCode { get; set; }
        public ScannedCode ProductCode { get; set; }


        public ProcessingCodeService ProcessingCodeService { get; set; }
        public ReportTaskService ReportTaskService { get; set; }
        public LocalDBService LocalDBService { get; set; }
        public ErrorsService ErrorsService { get; set; }


        public HandleAggregationDeleteProductViewModel(ProcessingCodeService processingCodeService,
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

                                if (boxes[0].Products.Count == 0)
                                {
                                    MessageBox.Show("Найденный короб пустой");
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
                                    BoxCode.StatusScannedCode += $"\nКод паллеты {box.Pallet.MarkingCode}.";
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
                        if (isRepeatProductCode)
                        {
                            var products = LocalDBService.ProductDataService.GetAllWithInclude((p => p.MarkingCode.Replace("\u001d", "") == code.Replace("\u001d", "") && p.ReportTaskGuid == ReportTaskService.CurrentReportTask.Guid), p => p.Box, p => p.Pallet).ToList();
                            if (products.Count == 1 || products.Where(p => p.BoxId == CurrentBox.Id).Count() == 1)
                            {
                                DAL.Entities.Product deleteProduct = products.FirstOrDefault(b => b.BoxId == CurrentBox.Id);

                                if (deleteProduct != null)
                                {
                                    LocalDBService.ProductDataService.Delete(deleteProduct.Id);

                                    //Очищаем ошибки связанные с коробом
                                    {
                                        var errors = ErrorsService.Errors.FindAll(e => e.ProductCode == (deleteProduct.MarkingCode));
                                        foreach (var error in errors)
                                        {
                                            ErrorsService.Errors.Remove(error);
                                        }

                                        CurrentBox = LocalDBService.BoxDataService.GetWithInclude(CurrentBox.Id, p => p.Pallet);
                                        var newError = new Error { TypeError = "Неполный короб" };
                                        newError.BoxCodes.Add(CurrentBox.MarkingCode);
                                        newError.PalletCodes.Add(CurrentBox.Pallet.MarkingCode);
                                        ErrorsService.Errors.Add(newError);

                                        ErrorsService.Errors = new List<Error>(ErrorsService.Errors);
                                    }


                                    //Обновляем статистику
                                    {
                                        CurrentBox.Products.RemoveAll(p => p.Id == deleteProduct.Id);

                                        ReportTaskService.Statistic.ProductCodes.RemoveAll(p => p.Id == products[0].Id);
                                        ReportTaskService.Statistic.CountProducts--;
                                    }

                                    //Обновляем информацию о состоянии короба и текущей паллеты
                                    {
                                        ProductCode.StatusScannedCode = $"Код продукта удалён.\n" +
                                                                    $"{code}.";
                                        ProductCode.IsErrorStatusScannedCode = false;
                                      

                                        BoxCode.StatusScannedCode = $"Код короба.\n" +
                                                                    $"{CurrentBox.MarkingCode}.\n" +
                                                                    $"Продукта в коробе {CurrentBox.Products.Count}/{ReportTaskService.CurrentReportTask.CountProductInBox}\n" +
                                                                    $"Код паллеты {CurrentBox.Pallet.MarkingCode}.";

                                        if (CurrentBox.Products.Count == 0)
                                        {
                                            MessageBox.Show("Короб пустой");

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
                                    
                                }
                                else
                                {
                                    ProductCode.StatusScannedCode = $"Код продукта найден в другом коробе.\n" +
                                                                    $"{code}.\n" +
                                                                    $"Код короба {products[0].Box.MarkingCode}.";
                                    ProductCode.IsErrorStatusScannedCode = true;
                                }

                            }
                            else
                            {
                                ProductCode.StatusScannedCode = $"Найденно несколько продуктов с данным кодом." +
                                                               $"\n{code}.";
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
                            ProductCode.StatusScannedCode = $"Введённый код продукта не существует.\n" +
                                                            $"{code}.";
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
