   M S B A M L   `   `   `  ����=  9PM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:PM.Models	PM.Models  5      -clr-namespace:PM.ViewModelsPM.ViewModels  5	          PM.Controls.DeviceControl   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      *modelsclr-namespace:PM.Models     5      2
viewmodelsclr-namespace:PM.ViewModels     5	        � 	Resources  5       �� MergedDictionaries �� Source �� 
TargetType   ControlBorderStyle �� BasedOn �� Property �� Value �� Triggers P� Property	 P� Value
 �� Binding �� Value�� 5   
    5      �� 5      $! ../../Styles/TextBlock.xaml?�6%   �� 5      $ ../../Styles/Border.xaml?�6%   %   '��       7   �� 5      " ��6   8   .�5      ��  � ��$ 4q�6+   �� 5       � ��
 �����60   �� 5       � ��$ 1q�64   �� 5       �@��CornerRadius$ 5��61   �� 5       � ��
 �����69    5      P� 5       �@�DataContext	 65   Z� . �5      ��  � y�$ 	Collapsed=�67   5      �� 5       
 6"   �� *IsConnected+	 False6B   .a�5!      ��  � ��
 �  ��67   �� 5"       � ��
 �����6C   5#      5%      5&      .��5)      �� ._�5*   
   �   5+        IconTextBlockStyle  BodyTextBlockStyle��%�  &	       7  '�� �    7  �� 5,      " ��63   8   .�5-      ��  � ��$ Center=�6<   �� 5.       � ��$ Center=�6:   �� 5/       � ��$ 25(�61    50      �� 51      
 6&   �� *IsConnected+ True6F   .a�52      ��  � ��
 � � �6;   �� 53       � ��$	 ��65   54      �� 55      
 6&   �� *IsConnected+	 False6F   .a�56      ��  � ��
 �����6;   �� 57       � ��$	 ��65   58      5:      �� 5<      " ��6   8   .�5=      ��  � ��$ 70��62    5>      �� 5?      
 6&   �� *IsConnected+	 False6F   .a�5@      ��  � ��
 �����6;   5A      5C      5D       � ColumnDefinitions 5F      �� 5G      �� 5H      $
��auto �6#   .+�5L      �� $��0��6   $��Center=�5N      ��5M      �� *Name+ �� UpdateSourceTrigger$ PropertyChanged=� �� FallbackValue$" ----------------------------��5N      �� 5O      $��1��6   #��[ 5P      5Q      5R      5S      