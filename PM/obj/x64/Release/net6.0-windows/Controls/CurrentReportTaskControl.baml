   M S B A M L   `   `   `  ����   PM*    $PM.Controls.CurrentReportTaskControl   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008       � 	Resources   �� MergedDictionaries �� Source��  �� $ ../Styles/TextBlock.xaml?��� $ ../Styles/Border.xaml?�%    .����    ControlBorderStyle#��[  ._��  � RowDefinitions �� $
��auto ��� .+��� $��0��  TableHeaderBorderStyle#��[ ._��    �� 
TargetType  HeaderTeableTextBlockStyle �� BasedOn��%   '��       7  �� " ��8    � ColumnDefinitions �� $
��0.7* ��� $��1* ��� $
��0.7* ��� $��0.45* ��� $
��0.7* ��� $
��0.7* ��� $
��0.5* ��� $
��0.5* �.+��� $��0��$*��$Дата/Время
создания���� $��1��$��Наименование���� $��2��$
��Gtin���� $��3��$��Номер
партии��$��Center=��� $��4��$'��!Дата
производства��$��Center=��� $��5��$*��$Дата
срока годности��$��Center=��� $��6��$&�� Продукта
в коробе��$��Center=��� $��7��$&�� Коробов
в паллете��$��Center=��� $��1��  TableBodyBorderStyle#��[ ._�� $��70��    BodyTeableTextBlockStyle��%   '��       7  �� " ��8    �� $
��0.7* ��� $��1* ��� $
��0.7* ��� $��0.45* ��� $
��0.7* ��� $
��0.7* ��� $
��0.5* ��� $
��0.5* �.+��� $��0������ *
DateTime+ �� UpdateSourceTrigger$ PropertyChanged=� �� FallbackValue$ --.--.-- --:--:--��	 �� StringFormat$	 {0:dd.MM.yy HH:mm:ss}���� $��1������ *Nomenclature.Name+$ PropertyChanged=�$ -------------------���� $��2������ *Nomenclature.Gtin+$ PropertyChanged=�$ --------------���� $��3��$��Center=����� *	LotNumber+$ PropertyChanged=�$
 ----���� $��4��$��Center=����� *ManufactureDate+$ PropertyChanged=�$ --.--.--��$	 {0:dd.MM.yy}���� $��5��$��Center=����� *
ExpiryDate+$ PropertyChanged=�$ --.--.--��$	 {0:dd.MM.yy}���� $��6��$��Center=����� *CountProductInBox+$ PropertyChanged=�$ --���� $��7��$��Center=����� *CountBoxInPallet+$ PropertyChanged=�$	 ---��