using PM.Devices;
using PM.Navigators;
using PM.Services;
using PM.Mixter.Services;
using PM.ViewModels;
using PM.Windows;
using SingleInstanceCore;
using System;
using System.Windows;
using System.Windows.Forms;
using PM.DAL.Entities;

namespace PM.Mixter
{
 
    public partial class App : System.Windows.Application, ISingleInstance
    {
        public new MainWindow MainWindow { get; set; }
        public CreateReportTaskWindow CreateReportTaskWindow { get; set; }


        public MainWindowViewModel MainWindowViewModel { get; set; }
        public CreateReportTaskWindowViewModel CreateReportTaskWindowViewModel { get; set; }


        public MainViewModel MainViewModel { get; set; }
        public ReportTasksViewModel ReportTasksViewModel { get; set; }
        public HandleAggregationViewModel HandleAggregationViewModel { get; set; }
        public EventsViewModel EventsViewModel { get; set; }
        public PrinterViewModel PrinterViewModel { get; set; }
        public TaskHistoryViewModel NomenclaturesViewModel { get; set; }
        public ErrorsViewModel ErrorsViewModel { get; set; }
        public SettingsViewModel SettingsViewModel { get; set; }


        public MainWindowNavigator MainWindowNavigator { get; set; }


        public ISettingsService SettingsService { get; set; }
        public LocalDBService LocalDBService { get; set; }
        public NomenclatureService NomenclatureService { get; set; }
        public ReportTaskService ReportTaskService { get; set; }
        public IDeviceService DeviceService { get; set; }
        public ProcessingCodeService ProcessingCodeService { get; set; }
        public ErrorsService ErrorsService { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                bool isFirstInstance = this.InitializeAsFirstInstance("PM.Mixter");
                if (!isFirstInstance)
                {
                    Current.Shutdown();
                }

                Settings.LocalDB = new DBSettings()
                {
                    Name = "База данных (локальная)",
                    ServerName = "172.25.4.116",
                    DataBaseName = "prommark1M",
                    IsAuthentification = true,
                    Login = "sa",
                    Password = "!1qazxcv",
                    IsUsed = true,
                };

                LocalDBService = new LocalDBService();

                SettingsService = new SettingsService(LocalDBService);
                SettingsService.LoadSettings();


                ErrorsService = new ErrorsService();

                NomenclatureService = new NomenclatureService(LocalDBService);
                NomenclatureService.StartLodingNomenclatureAsync(SettingsService.Settings.Line.PathLoadNomenclatureFiles);

                ReportTaskService = new ReportTaskService(LocalDBService,
                                                         SettingsService.Settings.Line,
                                                         ErrorsService);

                ProcessingCodeService = new ProcessingCodeService(ReportTaskService, LocalDBService);

                DeviceService = new DeviceService(SettingsService,
                                                  ReportTaskService,
                                                  LocalDBService,
                                                  ProcessingCodeService);


                CreateReportTaskWindowViewModel = new CreateReportTaskWindowViewModel(NomenclatureService,
                                             ReportTaskService);
                CreateReportTaskWindow = new CreateReportTaskWindow();

                CreateReportTaskWindow.DataContext = CreateReportTaskWindowViewModel;

                ErrorsViewModel = new ErrorsViewModel(ErrorsService);

                MainViewModel = new MainViewModel(ReportTaskService,
                                                  SettingsService,
                                                  DeviceService);
                ReportTasksViewModel = new ReportTasksViewModel(ReportTaskService,
                                                                CreateReportTaskWindow,
                                                                DeviceService);

                HandleAggregationViewModel = new HandleAggregationViewModel(ReportTaskService,
                                                                            ProcessingCodeService,
                                                                            LocalDBService,
                                                                            ErrorsService);

                EventsViewModel = new EventsViewModel();
                PrinterViewModel = new PrinterViewModel();
                NomenclaturesViewModel = new TaskHistoryViewModel(ReportTaskService);

                SettingsViewModel = new SettingsViewModel(SettingsService,
                                                          DeviceService,
                                                          ReportTaskService);

                MainWindowNavigator = new MainWindowNavigator(MainViewModel,
                                                              ReportTasksViewModel,
                                                              HandleAggregationViewModel,
                                                              EventsViewModel,
                                                              PrinterViewModel,
                                                              NomenclaturesViewModel,
                                                              ErrorsViewModel,
                                                              SettingsViewModel);

                MainWindowViewModel = new MainWindowViewModel(MainWindowNavigator,
                                                              SettingsService,
                                                              ErrorsService);

                MainWindow = new MainWindow();

                MainWindow.DataContext = MainWindowViewModel;
                MainWindow.Closing += MainWindow_Closing;
                MainWindow.Show();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Current.Shutdown();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            SingleInstance.Cleanup();
            NomenclatureService?.StopLodingNomenclatures();
            DeviceService?.StopDevices();
            DeviceService?.DisconnectDevices();
            CreateReportTaskWindow?.Close();
            CreateReportTaskWindow = null;
        }

        public void OnInstanceInvoked(string[] args)
        { }
    }
}

