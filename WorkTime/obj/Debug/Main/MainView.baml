   M S B A M L   `   `   `  ����C  ?WorkTime, Version=1.9.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:WorkTime.ConvertersWorkTime.Converters  5          WorkTime.Main.MainView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      8
converters!clr-namespace:WorkTime.Converters     5        � 	Resources  5	      .  (WorkTime.Converters.BoolToColorConverter   BoolToColorConverter4  .WorkTime.Converters.TimeStatusToColorConverter  TimeStatusToColorConverter3   -WorkTime.Converters.BoolToVisibilityConverter  BoolToVisibilityConverter3   -WorkTime.Converters.TimeSpanToStringConverter  TimeSpanToStringConverter��%t   &	        &	      &	 &     &	 9       5
   
   6*     5   
   60     5   
   6/     5   
   6/   5      .��5      �  � RowDefinitions 5   
   �� 5      $��5 �6   �� 5      $
��Auto �6   �� 5      $��2 �6   �� 5      $
��Auto �6   �� 5      $��5 �6   �� 5      $
��Auto �6   �� 5      $��5 �6   �� 5      $
��Auto �6   �� 5      $��5 �6   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� -   5         ��Name$ 	TotalDays��5$      $��1��5       $��Center=�5!      $��Center=�5"      $��18(�5#      5$      �� 5%   
   -   5        $ TotalWorkTime��5*      $��3��5&      $��Center=�5'      $��Center=�5(      $��18(�5)      5*      �� 5+   
   -   5         ��Name$ ChangeAction��50      $��18(�5,      $��5��5-      $��Center=�5.      $��Center=�5/      $��30��51      $	��180��52      $��5q�53      ��54      �� *	IsStart+ �� 	Converter# [  ��55      �� *
ActionName+�� 56   
   -   5        $ WorkDurationTime��5=      $��7��57      $��Center=�58      $��Center=�59      $��18(�5:      $��5q�5;      ��5<      �� *IsOk+# [ 5=      �� 5>   
   -   5        $ EndTime��5E      $��9��5?      $��Center=�5@      $��Center=�5A      $��14(�5B      $��5q�5C      y�5D      �� *	IsStart+# [ 5E      9� 5F   
   $��History��5G      $��11��5H      .��5I      �� .+�5J      �� -   5         ��Name$ HistoryDates��5L       �� ItemStringFormat$ MM-yyyy��6+    �� SelectedItem 5K      �� *SelectedHistoryDate+5L   +   &   System.Windows.Controls.DataGrid  5M         
IsReadOnly . $��Center=�5P      $��Center=�5Q      	   AutoGenerateColumns	 .  ��5N      �� *Data+
   Columns
 5S      0  *System.Windows.Controls.DataGridTextColumn  5T      ,  &System.Windows.Controls.DataGridColumn   Header$
 Date��6K   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6-   ��  �� Path$
 Date�6K     5U      $	 Day��6J    6-   �� $	 Day�6J     5V      $ 
Start time��6m    6-   �� $ 	StartTime� �� StringFormat$ 
{0: HH:mm}��6m     5W      $ End time��6k    6-   �� $ EndTime�$ 
{0: HH:mm}��6k     5X      $ Total��6L    6-   �� $ Total�6L     5Y      $ OverTime��6O    6-   �� $ OverTime�6O      RowStyle 5[      �� 5\       �� 
TargetType)	  #System.Windows.Controls.DataGridRow" 	 6     �� Triggers 5]      �� 5^   "    �� Binding 6.   �� *
OverTime+# [  �� Value Over6�   .a�5_   &   ��  �� Property � �� �� Value
 � � �6S   5`   #   �� 5a   "    6.   �� *
OverTime+# [  Ok6�   .a�5b   &   ��  � ��
 �   �6S   5c   #   �� 5d   "    6.   �� *
OverTime+# [  Err6�   .a�5e   &   ��  � ��
 �  ��6Q   5f   #   5h      5j      �� 5k      -   5        $ 
SaveToFile��5m      $	��0,5q�5l      $��	Save data��5n      5o      5q      5r      5s      