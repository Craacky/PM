using PM.DAL.Entities;
using PM.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace PM.Services
{
    public class ProcessingCodeService 
    {
        private readonly string patternOnPalletCode = @"^01\d{14}" +
                                                      @"11\d{6}" +
                                                      @"17\d{6}" +
                                                      @"10\d{4}" +
                                                      @"(\u001d|)?21\d{1,5}" +
                                                      @"(\u001d|)?37\d{1,10}$";
        private string patternOnPalletCodeTheCurrentReportTask;
        private readonly string patternOnBoxCode = @"^01\d{14}" +
                                                   @"11\d{6}" +
                                                   @"17\d{6}" +
                                                   @"10\d{4}" +
                                                   @"(\u001d|)?21\d{1,5}$";
        private string patternOnBoxCodeTheCurrentReportTask;
        private readonly string patternOnProductCode = @"^01\d{14}21" +
                                                       @".{1,10}" +
                                                       @"(\u001d|)?93.{4}$";
        private string patternOnProductCodeTheCurrentReportTask;

        private readonly Regex regexOnPalletCode;
        private Regex regexOnPalletCodeTheCurrentReportTask;
        private readonly Regex regexOnBoxCode;
        private Regex regexOnBoxCodeTheCurrentReportTask;
        private readonly Regex regexOnProductCode;
        private Regex regexOnProductCodeTheCurrentReportTask;


        public ReportTaskService ReportTaskService { get; set; }
        public LocalDBService LocalDBService { get; set; }

        public ProcessingCodeService(ReportTaskService reportTaskService,
                                    LocalDBService localDBService)
        {
            ReportTaskService = reportTaskService;
            LocalDBService = localDBService;

            regexOnPalletCode = new Regex(patternOnPalletCode);
            regexOnBoxCode = new Regex(patternOnBoxCode);
            regexOnProductCode = new Regex(patternOnProductCode);
        }


        public bool IsPalletCode(string code)
        {
            bool isCode = regexOnPalletCode.Match(code).Success;
            return isCode;
        }
        public bool IsPalletCodeTheCurrentTask(string code)
        {
            patternOnPalletCodeTheCurrentReportTask = $@"^01{ReportTaskService.CurrentReportTask.Nomenclature.Gtin}" +
                                                       $@"11{ReportTaskService.CurrentReportTask.ManufactureDate.ToString("yyMMdd")}" +
                                                       $@"17{ReportTaskService.CurrentReportTask.ExpiryDate.ToString("yyMMdd")}" +
                                                       $@"10{ReportTaskService.CurrentReportTask.LotNumber}" +
                                                       @"(\u001d|)?21\d{1,5}" +
                                                       @"(\u001d|)?37\d{1,10}$";


            regexOnPalletCodeTheCurrentReportTask = new Regex(patternOnPalletCodeTheCurrentReportTask);
            
            bool isCodeTheCurrentTask = regexOnPalletCodeTheCurrentReportTask.Match(code).Success;
            return isCodeTheCurrentTask;
        }
        public bool IsRepeatPalletCode(string code)
        {
            bool isRepeatCode;
            var pallets = ReportTaskService.Statistic.PalletCodes.FirstOrDefault(p => p.MarkingCode.Replace("\u001d", "") == code.Replace("\u001d", ""));
            if (pallets == null)
            {
                isRepeatCode = false;
            }
            else
            {
                isRepeatCode = true;
            }

            return isRepeatCode;
        }

        public bool IsBoxCode(string code)
        {
            bool isCode = regexOnBoxCode.Match(code).Success;
            return isCode;
        }
        public bool IsBoxCodeTheCurrentTask(string code)
        {
            patternOnBoxCodeTheCurrentReportTask = $@"^01{ReportTaskService.CurrentReportTask.Nomenclature.Attributes.FirstOrDefault(c=> c.Code == 12).Value}" +
                                                   $@"11{ReportTaskService.CurrentReportTask.ManufactureDate.ToString("yyMMdd")}" +
                                                   $@"17{ReportTaskService.CurrentReportTask.ExpiryDate.ToString("yyMMdd")}" +
                                                   $@"10{ReportTaskService.CurrentReportTask.LotNumber}" +
                                                   @"(\u001d|)?21\d{1,5}";
            regexOnBoxCodeTheCurrentReportTask = new Regex(patternOnBoxCodeTheCurrentReportTask);

            bool isCodeTheCurrentTask = regexOnBoxCodeTheCurrentReportTask.Match(code).Success;
            return isCodeTheCurrentTask;
        }
        public bool IsRepeatBoxCode(string code)
        {
            bool isRepeatCode;
            var boxes = ReportTaskService.Statistic.BoxCodes.FirstOrDefault(p => p.MarkingCode.Replace("\u001d", "") == code.Replace("\u001d", ""));
            if (boxes == null)
            {
                isRepeatCode = false;
            }
            else
            { 
                isRepeatCode = true;
            }

            return isRepeatCode;
        }
        
        public bool IsProductCode(string code)
        {
            bool isProductCode = regexOnProductCode.Match(code).Success;
            return isProductCode;
        }
        public bool IsProductCodeTheCurrentTask(string code)
        {
            patternOnProductCodeTheCurrentReportTask = @$"01{ReportTaskService.CurrentReportTask.Nomenclature.Gtin}21" +
                                                       @".{1,10}" +
                                                       @"(\u001d|)?93.{4}$";
            regexOnProductCodeTheCurrentReportTask = new Regex(patternOnProductCodeTheCurrentReportTask);

            bool isCodeTheCurrentTask = regexOnProductCodeTheCurrentReportTask.Match(code).Success;
            return isCodeTheCurrentTask;
        }
        public bool IsRepeatProductCode(string code)
        {
            bool isRepeatCode;
            var products = ReportTaskService.Statistic.ProductCodes.FirstOrDefault(p => p.MarkingCode.Replace("\u001d", "") == code.Replace("\u001d", ""));
            if (products == null)
            {
                isRepeatCode = false;
            }
            else
            {
                isRepeatCode = true;
            }

            return isRepeatCode;
        }
    }
}
