   M S B A M L   `   `   `  ���� =  9PM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:PM.ViewModelsPM.ViewModels  +    %PM.Controls.NomenclaturesTableControl   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     2
viewmodelsclr-namespace:PM.ViewModels       � 	Resources   �� MergedDictionaries �� Source��  �� $ ../Styles/ListBox.xaml?��� $ ../Styles/TextBlock.xaml?��� $ ../Styles/Border.xaml?�%    .����    ControlBorderStyle#��[  ._��  � RowDefinitions �� $
��auto ��� .+��� $��0��  TableHeaderBorderStyle#��[ ._��    �� 
TargetType  HeaderTeableTextBlockStyle �� BasedOn��%   '��       7  �� " ��8    � ColumnDefinitions �� $
��0.5* ��� $
��1.2* ��� $
��0.7* �.+��� $��0��$*��$Дата/Время
создания���� $��1��$��Наименование���� $��2��$
��Gtin���� $��1��._���   ListBoxStyle#��[ ���� *#!NomenclatureService.Nomenclatures+ �� UpdateSourceTrigger$ PropertyChanged=�����  �� RelativeSource �� 	 �� Mode$	 TemplatedParent=�
 �� Path$
 ActualWidth�$ PropertyChanged=����� " ��  ListBoxItemStyle# [ .���  �� Property � �� �� Value ��  �� 
TargetType" ��.R��� #��z��#��z��#��z�� �� Padding# z��  TableBodyBorderStyle#��[ $��70������  �� $	 TemplatedParent=�$
 ActualWidth�$ PropertyChanged=�._��     BodyTeableTextBlockStyle��%   '��       7  �� " ��8    �� $
��0.5* ��� $
��1.2* ��� $
��0.7* �.+��� $��0��#��z������ *
DateTime+$ PropertyChanged=� �� FallbackValue$ --.--.-- --:--:--�� �� StringFormat$ {0:dd.MM.yy HH:mm:ss}���� $��1��#��z������ *Name+$ PropertyChanged=�$ -------------------���� $��2��#��z������ *Gtin+$ PropertyChanged=�$ --------------������ .R�$�  %� IsVirtualizing$
 True��