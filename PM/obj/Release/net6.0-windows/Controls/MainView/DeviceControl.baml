   M S B A M L   `   `   `  ���� =  9PM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:PM.Models	PM.Models  -clr-namespace:PM.ViewModelsPM.ViewModels      PM.Controls.DeviceControl   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     *modelsclr-namespace:PM.Models     2
viewmodelsclr-namespace:PM.ViewModels       � 	Resources   �� MergedDictionaries �� Source �� 
TargetType   ControlBorderStyle �� BasedOn �� Property �� Value �� Triggers P� Property	 P� Value
 �� Binding �� Value��  �� $! ../../Styles/TextBlock.xaml?��� $ ../../Styles/Border.xaml?�%=  '��       7   �� " ��8   .���  � ��$ 4q���  � ��
 �������  � ��$ 1q���  �@��CornerRadius$ 5����  � ��
 ����� P�  �@�DataContext	 Z� . ���  � y�$ 	Collapsed=��� 
 �� *IsConnected+	 False.a���  � ��
 �  ����  � ��
 �����.���� ._��     IconTextBlockStyle  BodyTextBlockStyle��%�  &	       7  '��     7  �� " ��8   .���  � ��$ Center=���  � ��$ Center=���  � ��$ 25(� �� 
 �� *IsConnected+ True.a���  � ��
 � � ���  � ��$	 ���� 
 �� *IsConnected+	 False.a���  � ��
 �������  � ��$	 ���� " ��8   .���  � ��$ 70�� �� 
 �� *IsConnected+	 False.a���  � ��
 ����� � ColumnDefinitions �� �� $
��auto �.+��� $��0��$��Center=����� *Name+ �� UpdateSourceTrigger$ PropertyChanged=� �� FallbackValue$" ----------------------------���� $��1��#��[ 