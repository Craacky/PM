   M S B A M L   `   `   `  ����W  SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089   TextBoxControlTemplate  �� 
TargetType � 	Resources  Key �� 
TargetType �� Property �� Value ��Name �� CornerRadius J� SnapsToDevicePixels �� Padding	 � ColumnDefinitions
 ��Name  IconTextBlockStyle �� RelativeSource �� AncestorType �� Path �� Mode  TextBoxStyle-   'System.Windows.Input.KeyboardNavigation �� Triggers P� Property P� Value  ScannedCodeTxtBoxStyle �� BasedOn �� Binding �� Value ]� 
Conditions �� Property �� Value�� 5      H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation       :x,http://schemas.microsoft.com/winfx/2006/xaml      5      %�	  &	        7  &	 I    7   &	 �    7  �� 5      "  ��65    5   
   ���� 5      IconTextBlockStyle " ��6/   .�5      ��  � ��$ Segoe MDL2 Assets)�6/   �� 5       � ��$ Center=�66   �� 5	       � ��$ 0 2 10 0q�6+   �� 5
       � ��$ Medium"�6/   5      5      .R�5   
   �� $ border��6   #��z��5      #��z��5      #��z��5      # z 5      $
 True��5      # z��5      ._�5      � 	 5      �� 5      �� 5      $
��auto �6'   .+�5      �� $
 PART_ContentHost��6   $��0��5      $|�false��5      $��Hidden=�5      $��Hidden=�5      #��z��5       �� 5"      $��1��6   8��  #��z��5&      #��z��5'      ��5#      ��  ��  TextBox$	 Tag���5$      ��  �� $
 Self=�$ ActualHeight�5'      5(      5)      5*      �� 5,      " ��6!   .�5-   
   ��  �@  TabNavigation$
 None=�6=   �� 5.   
    �@�FocusVisualStyle 6-   Z� �� 5/   
    �@J�	AllowDrop$
 true��6&   �� 50   
    �@��PanningMode$ VerticalFirst=�65   �� 51   
    �@��IsFlicksEnabled$ False��63   �� 53   
    � ��8   6%   �� 54   
    � ��$
 Left=�67   �� 55   
    � ��$ Center=�65   �� 56   
    � ��$ Center=�6.   �� 57   
    � ��
 ���� 6'   �� 58   
    � ��
 �����6(   �� 59   
    � ��
 �   �6'   �� 5:   
    � ��$ 1q�6,   �� 5;   
    �@��CornerRadius$ 5��60   �� 5<   
    � ��$
 10 5q�6$   �� 5=   
    � ��$ 14(�6%   �� 5>   
    � ��$
 10 5q�6#    5@   
   P� 5A       � {�$ False��6+   . �5B      ��  �@J�Opacity$	 0.6Q�6,   �� 5C       � ��
 �����6/   5D      5F      �� 5H      " ��6+   8    5I   
   �� 5J       6   �� *IsErrorStatusScannedCode+ True6G   .a�5K      ��  � ��
 �  ��60   �� 5L       �@�Tag$	 ��6(   5M      �� 5O       6   �� *IsErrorStatusScannedCode+	 False6G   .a�5P      ��  � ��
 � � �60   �� 5Q       �@�Tag$	 ��6(   5R      �� 5T       6   �� *StatusScannedCode+  6@   .a�5U      ��  � ��
 �����60   �� 5V       �@�Tag$  ��6(   5W      ]� 5Y       5Z      �� 5[       �@J�IsKeyboardFocused$
 True��6=   �� 5\       �@�Tag$  ��6/   ./�5_      ��  � �� �#015DFE60   5`      5b      5c      