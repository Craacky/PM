   M S B A M L   `   `   `  ���� =  9PM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:PM.ViewModelsPM.ViewModels  -clr-namespace:PM.NavigatorsPM.Navigators  '    !PM.Controls.MainWindowMenuControl   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     2
viewmodelsclr-namespace:PM.ViewModels     2
navigatorsclr-namespace:PM.Navigators       � 	Resources   �� MergedDictionaries �� Source �� 
TargetType "  MainWindowMenuRadioButtonStyle �� BasedOn��  �� $ ../Styles/Border.xaml?��� $ ../Styles/Button.xaml?�%   '	�       7   �� " 	�8   .����  $  MainWindowMenuControlBorderStyle#��[ ._��  � RowDefinitions �� �� $
��auto �.+��� $��0��  MainWindowMenuBorderStyle#��[ ._��  �� $
��auto ��� .+�A�  A� Rows$ 7��$��0��.+�	� $��Главная��
 � Tag$	 ��&    PM.Navigators.MainWindowViewType   MainView#��Z ���� *)'Navigator.UpdateCurrentViewModelCommand+	� $��Задания��$	 ��	 d� 	IsChecked$
	 TrueY�
   ReportTasksView#��Z
 ���� *)'Navigator.UpdateCurrentViewModelCommand+	� $1��+Агрегация
разагрегация��$	 ��   HandleAggregationView#��Z ���� *)'Navigator.UpdateCurrentViewModelCommand+	� $��События��$	 ��${�False��$y�	Collapsed=�   
EventsView#��Z ���� *)'Navigator.UpdateCurrentViewModelCommand+	� $��Печать��$	 ��${�False��$y�	Collapsed=�   PrinterView#��Z ���� *)'Navigator.UpdateCurrentViewModelCommand+	� $#��История
заданий��$	 ��$y�	Collapsed=�   TaskHistoryView#��Z ���� *)'Navigator.UpdateCurrentViewModelCommand+	� $	 ��   
ErrorsView#��Z ���� *)'Navigator.UpdateCurrentViewModelCommand+.���  � ColumnDefinitions �� $
��auto ��� .+��� $��Ошибки��$��0���� $��1��$��10 0 0 0q�$��Right=�
���  ������  �� Path$  ErrorsService.Errors.Count� �� UpdateSourceTrigger$ PropertyChanged=�	� $��Настройки��$	 ��   SettingsView#��Z ���� *)'Navigator.UpdateCurrentViewModelCommand+�� $��1��$��
Выход�� " MainWindowMenuCloseButtonStyle#��[ ���� *CloseWindowCommand+