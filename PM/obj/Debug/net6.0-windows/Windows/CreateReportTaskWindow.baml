   M S B A M L   `   `   `  ����=  9PM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:PM.ControlsPM.Controls  5   	   -clr-namespace:PM.ViewModelsPM.ViewModels  5   	   '    !PM.Windows.CreateReportTaskWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .controlsclr-namespace:PM.Controls     5   	   2
viewmodelsclr-namespace:PM.ViewModels     5   	     � WindowStartupLocation$  CenterScreen=�5   	    � WindowStyle$
 None=�5   	    � WindowState$ Normal=�5	   	    � 
ResizeMode$ CanMinimize=�5
   	    � ShowInTaskbar$
 True��5   	    � AllowsTransparency$
 True��5   	   
������  � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $ ../Styles/Common.xaml?�6%   �� 5      $ ../Styles/Border.xaml?�6%   �� 5      $ ../Styles/Button.xaml?�6%   �� 5      $ ../Styles/TextBlock.xaml?�6%   �� 5      $ ../Styles/TextBox.xaml?�6%   %    5      .��5!      ��    WindowBorderStyle#��[  6   ._�5"   
   ��   ViewBorderStyle#��[ 6   ._�5#      � 	 � RowDefinitions	 5$      �� 5%      $
��auto �6$   �� 5&      �� 5'      $
��auto �6$   .+�5*      �� -   5        $��0��5*      
������ ._�5-      � .+�5.      �� $��0��6$   $��Ввод задания��5/   $     HeaderTextBlockStyle#��[ 50   $   *   $PM.Controls.CloseWindowButtonControl  52      53      54      �� 56      $��1��6     CreateReportTaskBorderStyle#��[ 57      ._�58      �� $
��Auto=�6#   .��59      � .+�5:      ��  5;      
 �� 
TargetType  HeaderFieldTextBlockStyle �� BasedOn  TextBoxStyle(  "System.Windows.Controls.DatePicker  DatePickerStyle  ComboBoxStyle��%�   '��       7  '�� "     7  '  D     7  '�� f     7  �� 5<   "   "
 ��6(   8   6?   �� 5=   "   "
 ��6(   8   6=   �� 5>   "   "
  6(   8   6@   �� 5?   "   "
 ��6(   8   6>   5@      .+�5B      �� .+�5C   "   �� $��Введите Gtin��6,   �� 5D   "   -   5         ��Name$ gtinComboBox��5D   +    �� 
IsEditable$
 True��5E   +    �� IsTextSearchEnabled$ False��5I   +    �� SelectedItem 5F   +   �� *SelectedNomenclature+ �� UpdateSourceTrigger$ PropertyChanged=���5G   +   �� *#!NomenclatureService.Nomenclatures+$ PropertyChanged=�5K   +   5L      �� 5N      .+�5O   "   �� $,��&Введите номер партии��6,   �� 5P   "   -   5        ��5P   *   �� *ReportTask.LotNumber+$ PropertyChanged=�5R   *   5S      �� 5U      .+�5V   "   �� $4��.Введите дату изготовлени��6,     5W   "   -   5           SelectedDate 5W   -   �� *ReportTask.ManufactureDate+$ PropertyChanged=� �� Mode$ TwoWay=�5X   %   5Y      �� 5[   "   .+�5\   &   �� $��Cрок годности��60     5]   &   -   5         5]   1   �� *ReportTask.ExpiryDate+$ PropertyChanged=�$ TwoWay=�5^      5`   #   �� 5b   "   .+�5c   "   �� $J��DВведите количества продукта в коробе��6,   �� 5d   "   -   5         �� 
IsReadOnly$
 True��5h   *   ��5d   *   �� *ReportTask.CountProductInBox+$ PropertyChanged=�5h   *   5i      �� 5k      .+�5l   "   �� $J��DВведите количества коробов в паллете��6,   �� 5m   "   -   5        ��5m   *   �� *ReportTask.CountBoxInPallet+$ PropertyChanged=�5p   *   5q      5s      5t      5u      5v      �� 5x      $��2��6   $��Right=�5y      $��Создать��5z      $��5 5 5 5q�5{        PrimaryButtonStyle#��[ 5|      ��5}      �� *CreateTaskCommand+5~      5      5�      5�      