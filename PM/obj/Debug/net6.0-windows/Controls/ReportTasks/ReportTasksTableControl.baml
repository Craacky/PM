   M S B A M L   `   `   `  ����=  9PM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:PM.ViewModelsPM.ViewModels  5      )    #PM.Controls.ReportTasksTableControl   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      2
viewmodelsclr-namespace:PM.ViewModels     5        � 	Resources  5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $ ../../Styles/ListBox.xaml?�6%   �� 5      $! ../../Styles/TextBlock.xaml?�6%   �� 5      $ ../../Styles/Border.xaml?�6%   %    5      .��5      ��    ControlBorderStyle#��[  6   ._�5   
   �  � RowDefinitions 5      �� 5      $
��auto �6    �� 5      .+�5      �� $��0��6     TableHeaderBorderStyle#��[ 5       ._�5!      �   5"       �� 
TargetType  HeaderTeableTextBlockStyle �� BasedOn��%4   '��       7  �� 5#      " ��6    8   5$      5%       � ColumnDefinitions 5'      �� 5(      $
��0.5* �6+   �� 5)      $
��1.2* �6+   �� 5*      $
��0.7* �6+   �� 5+      $��0.45* �6+   �� 5,      $
��0.7* �6+   �� 5-      $
��0.7* �6+   �� 5.      $
��0.5* �6+   �� 5/      $
��0.5* �6+   �� 50      $
��0.7* �6+   .+�53      �� $��0��6    $*��$Дата/Время
создания��54       �� 55      $��1��6    $��Наименование��56       �� 57      $��2��6    $
��Gtin��58       �� 59      $��3��6    $��Номер
партии��5:       $��Center=�5;       �� 5<      $��4��6    $'��!Дата
производства��5=       $��Center=�5>       �� 5?      $��5��6    $*��$Дата
срока годности��5@       $��Center=�5A       �� 5B      $��6��6    $&�� Продукта
в коробе��5C       $��Center=�5D       �� 5E      $��7��6    $&�� Коробов
в паллете��5F       $��Center=�5G       �� 5H      $��8��6    $��Статус��5I       $��Center=�5J       5K      5L      �� 5N      $��1��6   ._�5O      ��   ListBoxStyle#��[ 5Q      ��5O      �� *ReportTaskService.ReportTasks+ �� UpdateSourceTrigger$ PropertyChanged=� �� SelectedItem 5P      �� *SelectedReportTask+$ PropertyChanged=���5R      �� 	 �� RelativeSource	 �� 
 �� Mode$
 TemplatedParent=� �� Path$ ActualWidth�$ PropertyChanged=���5T      �� 5U      " ��6      ListBoxItemStyle# [ 69   .�5V      ��  �� Property � �� �� Value 5W   "   �� 5X   &    �� 
TargetType" ��66   .R�5Y   *   �� #��z��61   #��z��5Z   1   #��z��5[   1    �� Padding# z��5\   1     TableBodyBorderStyle#��[ 5^   1   $��70��5_   1   ��5]   1   �� 	 �� $
 TemplatedParent=�$ ActualWidth�$ PropertyChanged=�._�5`   .   �   5a   2     BodyTeableTextBlockStyle��%4   '��       7  �� 5b   6   " ��6<   8   5c   7   5d   3    5f   2   �� 5g   6   $
��0.5* �6G   �� 5h   6   $
��1.2* �6G   �� 5i   6   $
��0.7* �6G   �� 5j   6   $��0.45* �6G   �� 5k   6   $
��0.7* �6G   �� 5l   6   $
��0.7* �6G   �� 5m   6   $
��0.5* �6G   �� 5n   6   $
��0.5* �6G   �� 5o   6   $
��0.7* �6G   .+�5r   2   �� $��0��6<   #��z��5u   <   ��5s   <   �� *
DateTime+$ PropertyChanged=� �� FallbackValue$ --.--.-- --:--:--�� �� StringFormat$� �{0:dd.MM.yy                                                                                                                                                                           HH:mm:ss}��5u   <   �� 5v   2   $��1��6<   #��z��5x   <   ��5w   <   �� *Nomenclature.Name+$ PropertyChanged=�$ -------------------��5x   <   �� 5y   2   $��2��6<   #��z��5{   <   ��5z   <   �� *Nomenclature.Gtin+$ PropertyChanged=�$ --------------��5{   <   �� 5|   2   $��3��6<   #��z��5~   <   $��Center=�5   <   ��5}   <   �� *	LotNumber+$ PropertyChanged=�$
 ----��5   <   �� 5�   2   $��4��6<   #��z��5�   <   $��Center=�5�   <   ��5�   <   �� *ManufactureDate+$ PropertyChanged=�$ --.--.--��$ {0:dd.MM.yy}��5�   <   �� 5�   2   $��5��6<   #��z��5�   <   $��Center=�5�   <   ��5�   <   �� *
ExpiryDate+$ PropertyChanged=�$ --.--.--��$ {0:dd.MM.yy}��5�   <   �� 5�   2   $��6��6<   #��z��5�   <   $��Center=�5�   <   ��5�   <   �� *CountProductInBox+$ PropertyChanged=�$ --��5�   <   �� 5�   2   $��7��6<   #��z��5�   <   $��Center=�5�   <   ��5�   <   �� *CountBoxInPallet+$ PropertyChanged=�$	 ---��5�   <   �� 5�   2   $��8��69     StatusReportTaskBorder#��[ 6I   ._�5�   6   �� 
�������$��Center=�5�   @   ��5�   @   �� *Status+$ PropertyChanged=�$ ------��5�   @   5�   3   5�   /   5�   +   5�   '   5�      5�      ��5�      �� 5�      .R�5�      $�  %� IsVirtualizing$
 True��65   5�      5�      5�      5�      5�      5�      