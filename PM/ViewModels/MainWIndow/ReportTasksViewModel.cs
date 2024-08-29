using PM.Commands;
using PM.DAL.Entities;
using PM.Services;
using PM.ViewModels.Base;
using PM.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace PM.ViewModels
{
    public class ReportTasksViewModel : ViewModel
    {
        private ReportTask selectedReportTask;
        public ReportTask SelectedReportTask
        {
            get => selectedReportTask;
            set
            {
                selectedReportTask = value;
                OnPropertyChanged(nameof(SelectedReportTask));
            }
        }


        private ICommand createReportTaskCommand;
        public ICommand CreateReportTaskCommand => createReportTaskCommand;
        private bool CanCreateReportTaskCommandExecute(object p)
        {
            return true;
        }
        private void OnCreateReportTaskCommandlCommandExecuted(object p)
        {
            CreateReportTaskWindow.ShowDialog();
        }

        private ICommand startReportTaskCommand;
        public ICommand StartReportTaskCommand => startReportTaskCommand;
        private bool CanStartReportTaskCommandExecute(object p)
        {
            bool isAllDeviceConnect = DeviceService != null && DeviceService.Devices != null && !DeviceService.Devices.Any(d => d.IsConnected == false);
            return SelectedReportTask != null &&
                   ReportTaskService.CurrentReportTask == null &&
                   (SelectedReportTask.Status == "Новое" ||
                   SelectedReportTask.Status == "Остановлено") &&
                   isAllDeviceConnect;
        }
        private void OnStartReportTaskCommandlCommandExecuted(object p)
        {
            SelectedReportTask.StartTime = DateTime.Now;
            ReportTaskService.UpdateReportTask(SelectedReportTask);
            ReportTaskService.CurrentReportTask = SelectedReportTask;

            //if (DeviceService.SettingsService.Settings.Line.LineId == 1)
            //{
            //    int weight = Convert.ToInt32(ReportTaskService.CurrentReportTask.Nomenclature.Attributes[13].Value.Replace("г", ""));
            //    if (weight >= 300)
            //    {
            //        DeviceService.ChangeSetup(3);
            //    }
            //    else if (ReportTaskService.CurrentReportTask.IsUsedCap)
            //    {
            //        DeviceService.ChangeSetup(1);
            //    }
            //    else if (!ReportTaskService.CurrentReportTask.IsUsedCap)
            //    {
            //        DeviceService.ChangeSetup(2);
            //    }
            //}

            DeviceService.StartDevices();
        }


        private ICommand pauseReportTaskCommand;
        public ICommand PauseReportTaskCommand => pauseReportTaskCommand;
        private bool CanPauseReportTaskCommandExecute(object p)
        {
            return SelectedReportTask != null &&
                   SelectedReportTask == ReportTaskService.CurrentReportTask &&
                   SelectedReportTask.Status == "Запущено";
        }
        private void OnPauseReportTaskCommandlCommandExecuted(object p)
        {
            SelectedReportTask.Status = "Остановлено";
            ReportTaskService.UpdateReportTask(SelectedReportTask);

            DeviceService.StopDevices();
            SelectedReportTask = null;
            ReportTaskService.CurrentReportTask = null;
        }
        private ICommand stopReportTaskCommand;
        public ICommand StopReportTaskCommand => stopReportTaskCommand;
        private bool CanStopReportTaskCommandExecute(object p)
        {
            bool isClosedLastPallet = ReportTaskService.Statistic.PalletCodes.Count > 0 && (ReportTaskService.Statistic.PalletCodes[^1].IsFulled || ReportTaskService.Statistic.PalletCodes[^1].Boxes.Count == 0);
            return SelectedReportTask != null &&
                   (SelectedReportTask.Status == "Запущено" ||
                   SelectedReportTask.Status == "Остановлено") && 
                   isClosedLastPallet;
        }
        private void OnStopReportTaskCommandlCommandExecuted(object p)
        {
            SelectedReportTask.StopTime = DateTime.Now;
            DeviceService.StopDevices();

            bool isSave = ReportTaskService.SaveReportTask(SelectedReportTask);

            if (isSave)
            {
                if (SelectedReportTask == ReportTaskService.CurrentReportTask)
                {                  
                    ReportTaskService.CurrentReportTask = null;
                }

                SelectedReportTask = null;
            }
            else
            {
                SelectedReportTask.StopTime = new DateTime();
                SelectedReportTask.Status = "Запущено";
                DeviceService.StartDevices();
                ReportTaskService.UpdateReportTask(SelectedReportTask);
            }
        }

        private ICommand saveReportTaskCommand;
        public ICommand SaveReportTaskCommand => saveReportTaskCommand;
        private bool CanSaveReportTaskCommandExecute(object p)
        {
            return SelectedReportTask != null &&
                   SelectedReportTask.Status == "Завершено";
        }
        private void OnSaveReportTaskCommandlCommandExecuted(object p)
        {
            ReportTaskService.SaveReportTask(SelectedReportTask);
        }

        private ICommand deleteReportTaskCommand;
        public ICommand DeleteReportTaskCommand => deleteReportTaskCommand;
        private bool CanDeleteReportTaskCommandExecute(object p)
        {
            return SelectedReportTask != null &&
                   (SelectedReportTask.Status == "Новое" ||
                   SelectedReportTask.Status == "Завершено" ||
                   SelectedReportTask.Status == "Сохранено" ||
                   SelectedReportTask.Status == "Устаревшее");
        }
        private void OnDeleteReportTaskCommandlCommandExecuted(object p)
        {
            if (SelectedReportTask.Status == "Завершено" ||
               SelectedReportTask.Status == "Сохранено")
            {
                ReportTaskService.ReportTasks.Remove(SelectedReportTask);
                ReportTaskService.ReportTasks = new List<DAL.Entities.ReportTask>(ReportTaskService.ReportTasks);
            }
            else
            {
                ReportTaskService.DeleteReportTask(SelectedReportTask);
            }
        }


        public ReportTaskService ReportTaskService { get; set; }
        public IDeviceService DeviceService { get; set; }

        public CreateReportTaskWindow CreateReportTaskWindow { get; set; }


        public ReportTasksViewModel(ReportTaskService reportTaskService,
                                    CreateReportTaskWindow createReportTaskWindow,
                                    IDeviceService deviceService)
        {
            ReportTaskService = reportTaskService;
            CreateReportTaskWindow = createReportTaskWindow;
            DeviceService = deviceService;

            createReportTaskCommand = new RelayCommand(OnCreateReportTaskCommandlCommandExecuted, CanCreateReportTaskCommandExecute);
            startReportTaskCommand = new RelayCommand(OnStartReportTaskCommandlCommandExecuted, CanStartReportTaskCommandExecute);
            pauseReportTaskCommand = new RelayCommand(OnPauseReportTaskCommandlCommandExecuted, CanPauseReportTaskCommandExecute);
            stopReportTaskCommand = new RelayCommand(OnStopReportTaskCommandlCommandExecuted, CanStopReportTaskCommandExecute);
            saveReportTaskCommand = new RelayCommand(OnSaveReportTaskCommandlCommandExecuted, CanSaveReportTaskCommandExecute);
            deleteReportTaskCommand = new RelayCommand(OnDeleteReportTaskCommandlCommandExecuted, CanDeleteReportTaskCommandExecute);
        }
    }
}
