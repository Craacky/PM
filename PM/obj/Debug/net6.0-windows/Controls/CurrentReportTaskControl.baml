   M S B A M L   `   `   `  ����  PM*    $PM.Controls.CurrentReportTaskControl   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5        � 	Resources  5       �� MergedDictionaries �� Source�� 5   
    5	      �� 5
      $ ../Styles/TextBlock.xaml?�6%   �� 5      $ ../Styles/Border.xaml?�6%   %    5      .��5      ��    ControlBorderStyle#��[  6   ._�5   
   �  � RowDefinitions 5      �� 5      $
��auto �6    �� 5      .+�5      �� $��0��6     TableHeaderBorderStyle#��[ 5      ._�5      �   5       �� 
TargetType  HeaderTeableTextBlockStyle �� BasedOn��%4   '��       7  �� 5      " ��6    8   5      5       � ColumnDefinitions 5      �� 5       $
��0.7* �6+   �� 5!      $��1* �6+   �� 5"      $
��0.7* �6+   �� 5#      $��0.45* �6+   �� 5$      $
��0.7* �6+   �� 5%      $
��0.7* �6+   �� 5&      $
��0.5* �6+   �� 5'      $
��0.5* �6+   .+�5*      �� $��0��6    $*��$Дата/Время
создания��5+       �� 5,      $��1��6    $��Наименование��5-       �� 5.      $��2��6    $
��Gtin��5/       �� 50      $��3��6    $��Номер
партии��51       $��Center=�52       �� 53      $��4��6    $'��!Дата
производства��54       $��Center=�55       �� 56      $��5��6    $*��$Дата
срока годности��57       $��Center=�58       �� 59      $��6��6    $&�� Продукта
в коробе��5:       $��Center=�5;       �� 5<      $��7��6    $&�� Коробов
в паллете��5=       $��Center=�5>       5?      5@      �� 5B      $��1��6     TableBodyBorderStyle#��[ 5C      ._�5D      � $��70��6     5E        BodyTeableTextBlockStyle��%0   '��       7  �� 5F      " ��6    8   6m   5G       5I      �� 5J      $
��0.7* �6+   �� 5K      $��1* �6+   �� 5L      $
��0.7* �6+   �� 5M      $��0.45* �6+   �� 5N      $
��0.7* �6+   �� 5O      $
��0.7* �6+   �� 5P      $
��0.5* �6+   �� 5Q      $
��0.5* �6+   .+�5T      �� $��0��6    ��5U      �� *
DateTime+ �� UpdateSourceTrigger$ PropertyChanged=� �� FallbackValue$ --.--.-- --:--:--��	 �� StringFormat$	 {0:dd.MM.yy HH:mm:ss}���� 5V      $��1��6    ��5W      �� *Nomenclature.Name+$ PropertyChanged=�$ -------------------���� 5X      $��2��6    ��5Y      �� *Nomenclature.Gtin+$ PropertyChanged=�$ --------------���� 5Z      $��3��6    $��Center=�5\      ��5[      �� *	LotNumber+$ PropertyChanged=�$
 ----��5\      �� 5]      $��4��6    $��Center=�5_      ��5^      �� *ManufactureDate+$ PropertyChanged=�$ --.--.--��$	 {0:dd.MM.yy}��5_      �� 5`      $��5��6    $��Center=�5b      ��5a      �� *
ExpiryDate+$ PropertyChanged=�$ --.--.--��$	 {0:dd.MM.yy}��5b      �� 5c      $��6��6    $��Center=�5e      ��5d      �� *CountProductInBox+$ PropertyChanged=�$ --��5e      �� 5f      $��7��6    $��Center=�5h      ��5g      �� *CountBoxInPallet+$ PropertyChanged=�$	 ---��5h      5i      5j      5k      5l      5m      