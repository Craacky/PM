   M S B A M L   `   `   `  ���� =  9PM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:PM.ViewModelsPM.ViewModels  -    'PM.Views.HandleAggregationCheckCodeView   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     2
viewmodelsclr-namespace:PM.ViewModels       � 	Resources   �� MergedDictionaries �� Source �� 
TargetType   TextBoxStyle �� BasedOn �� Triggers �� Binding �� Value �� Property	 �� Value
 ]� 
Conditions �� Property �� Value  
LabelStyle  HeaderBodyTextBlockStyle��  �� $ ../../Styles/TextBox.xaml?��� $ ../../Styles/Label.xaml?��� $! ../../Styles/TextBlock.xaml?��� $ ../../Styles/Border.xaml?�%�  '��       7   '�� �    7  '�� �    7  �� " ��8    ��  �� *IsErrorStatusScannedCode+ True.a���  � ��
	 �  ����  �@�Tag$		 ����  �� *IsErrorStatusScannedCode+	 False.a���  � ��
	 � � ���  �@�Tag$		 ����  �� *StatusScannedCode+  .a���  � ��
	 �������  �@�Tag$	  ��]� 
 ��  �@J�IsKeyboardFocused$
 True����  �@�Tag$  ��./���  � ��	 �#015DFE�� " ��8   .���  � ��
	 � � ���  � ��	 �#E0F9E3��  �@�Tag$		 �� ��  �� *IsErrorStatusScannedCode+ true.a���  � ��
	 �  ����  � ��	 �#FAE8E6��  �@�Tag$		 ���� " ��8   .���� ._��  � RowDefinitions �� $
��auto ��� .+��� $��0��  TableHeaderBorderStyle#��[ ._��� $��Проверка кода��  HeaderTextBlockStyle#��[ �� $��1��._��  � DataContext �� *ScannedCode+{��� *IsEnabledFieldScannedCode+ �� $
��auto ��� $
��auto ��� $
��auto �.+��� $��0��$;��5Сканируйте/Введите любой код���� -   $��1������ *Code+ �� UpdateSourceTrigger$ PropertyChanged=� �� Mode$ TwoWay=��� $��2������ *StatusScannedCode+$ PropertyChanged=�