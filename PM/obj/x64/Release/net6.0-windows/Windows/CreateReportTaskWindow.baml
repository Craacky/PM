   M S B A M L   `   `   `  ���� =  9PM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:PM.ControlsPM.Controls  -clr-namespace:PM.ViewModelsPM.ViewModels  '    !PM.Windows.CreateReportTaskWindow   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     .controlsclr-namespace:PM.Controls     2
viewmodelsclr-namespace:PM.ViewModels       � WindowStartupLocation$  CenterScreen=� � WindowStyle$
 None=� � WindowState$ Normal=� � 
ResizeMode$ CanMinimize=� � ShowInTaskbar$
 True�� � AllowsTransparency$
 True��
������  � 	Resources  �� MergedDictionaries �� Source��  �� $ ../Styles/Common.xaml?��� $ ../Styles/Border.xaml?��� $ ../Styles/Button.xaml?��� $ ../Styles/TextBlock.xaml?��� $ ../Styles/TextBox.xaml?�%    .����    WindowBorderStyle#��[  ._���   ViewBorderStyle#��[ ._�� 	 � RowDefinitions	 �� $
��auto ��� �� $
��auto �.+��� -   $��0��
������ ._�� .+��� $��0��$��Ввод задания��  HeaderTextBlockStyle#��[ *   $PM.Controls.CloseWindowButtonControl  �� $��1��  CreateReportTaskBorderStyle#��[ ._��� $
��Auto=�.��� .+���  
 �� 
TargetType  HeaderFieldTextBlockStyle �� BasedOn  TextBoxStyle(  "System.Windows.Controls.DatePicker  DatePickerStyle  ComboBoxStyle��%t   '��       7  '��      7  '       7  '�� -     7  �� "
 ��8   �� "
 ��8   �� "
  8   �� "
 ��8   .+��� .+��� $��Введите Gtin���� -    ��Name$ gtinComboBox�� �� 
IsEditable$
 True�� �� IsTextSearchEnabled$ False�� �� SelectedItem �� *SelectedNomenclature+ �� UpdateSourceTrigger$ PropertyChanged=����� *#!NomenclatureService.Nomenclatures+$ PropertyChanged=��� .+��� $,��&Введите номер партии���� -   ���� *ReportTask.LotNumber+$ PropertyChanged=��� .+��� $4��.Введите дату изготовлени��  -      SelectedDate �� *ReportTask.ManufactureDate+$ PropertyChanged=� �� Mode$ TwoWay=��� .+��� $,��&Cрок годности (в днях)����  �� 
IsReadOnly$
 True������ *ReportTask.ExpiryDateInDays+$ PropertyChanged=�$ TwoWay=��� .+��� $J��DВведите количества продукта в коробе���� -   $
 True������ *ReportTask.CountProductInBox+$ PropertyChanged=��� .+��� $J��DВведите количества коробов в паллете���� -   ���� *ReportTask.CountBoxInPallet+$ PropertyChanged=��� $��2��$��Right=�$��Создать��$��5 5 5 5q�  PrimaryButtonStyle#��[ ���� *CreateTaskCommand+