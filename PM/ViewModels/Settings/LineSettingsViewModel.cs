using PM.Commands;
using PM.DAL.Entities;
using PM.ViewModels.Base;
using System.Windows.Forms;
using System.Windows.Input;

namespace PM.ViewModels
{
    public class LineSettingsViewModel : ViewModel
    {
        private ICommand choosePathLoadNomenclatureFilesCommand;
        public ICommand ChoosePathLoadNomenclatureFilesCommand => choosePathLoadNomenclatureFilesCommand;
        private bool CanChoosePathLoadNomenclatureFilesCommandExecute(object p)
        {
            return true;
        }
        private void OnChoosePathLoadNomenclatureFilesCommandExecuted(object p)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if(folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                CurrentSettings.Line.PathLoadNomenclatureFiles = folderBrowserDialog.SelectedPath;
            }
        }
        
        private ICommand choosePathSaveReportTaskFilesCommand;
        public ICommand ChoosePathSaveReportTaskFilesCommand => choosePathSaveReportTaskFilesCommand;
        private bool CanChoosePathSaveReportTaskFilesCommandExecute(object p)
        {
            return true;
        }
        private void OnChoosePathSaveReportTaskFilesCommandExecuted(object p)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                CurrentSettings.Line.PathSaveReportTaskFiles = folderBrowserDialog.SelectedPath;
            }
        }


        public Settings CurrentSettings { get; set; }


        public LineSettingsViewModel(Settings currentSettings)
        {
            CurrentSettings = currentSettings;

            choosePathLoadNomenclatureFilesCommand = new RelayCommand(OnChoosePathLoadNomenclatureFilesCommandExecuted, CanChoosePathLoadNomenclatureFilesCommandExecute);
            choosePathSaveReportTaskFilesCommand = new RelayCommand(OnChoosePathSaveReportTaskFilesCommandExecuted, CanChoosePathSaveReportTaskFilesCommandExecute);
        }
    }
}
