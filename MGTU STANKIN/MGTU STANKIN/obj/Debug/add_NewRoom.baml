   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5   	    MGTU STANKIN   MGTU_STANKIN.add_NewRoom   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   9System&clr-namespace:System;assembly=mscorlib     5   	   -   5          � Title$2  ,Добавление новых комнат��5   	   $��332.894��6)   $��648.369��6:   .��5      � .+�5   
   �� -   5         ��Name$ add_Room��5      $��Добавить��6#   $
��Left=�66   $��
46,210,0,0q�6Q   $	��Top=�6e   $	��118��6}   $��34��6�   6�   �� 5	   
   $��Корпус��6   $
��Left=�6!   $��	33,29,0,0q�6<   $	��Top=�6O   $��74��6g   $��14(�6r   �� 5
   
   $��Номер комнаты��6   $
��Left=�6(   $��	33,74,0,0q�6C   $	��Top=�6V   $��14(�6n   �� 5   
   $#��Количество мест��6   $
��Left=�6*   $��
33,124,0,0q�6E   $	��Top=�6Y   $��14(�6q   �� 5   
   -   5         ��Name$
 Room��5   �   $
��Left=�6   $��23��6-   $��
205,74,0,0q�69    �� TextWrapping$
 Wrap=�6M   $	��Top=�6a   $��75��6y   6�   �� 5   
   -   5         ��Name$ Places��5   f   $
��Left=�6   $��205,124,0,0q�6.   $	��Top=�6C   $��75��6[    J� RenderTransformOrigin$ 0.5,0.5&�6v   z�5      V� 5      .~�5      �� �� 5      �� 5       �� Angle$ 0.284Q�6&   T� 5      5      .@�5      ��1��6   ��5      2��6   ��5      3��6   ��5      4��6   5      �� 5   
   -   5        $ Housing��5   �   $
��Left=�6   $��23��6-   $��
205,29,0,0q�69   $
 Wrap=�6M   $	��Top=�6a   $��75��6y   6�   �� 5   
   -   5        $
 Exit��5   �   $��Закрыть��6   $
��Left=�6#   $��205,210,0,0q�6>   $	��Top=�6S   $	��109��6k   $ 0.459,-0.595&�6w   $��34��6�   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ 
room_Table��5   v   $��Center=�6   $��369,29,28,0q�60   $	��Top=�6E   $	��246��6]   $	��243��6j      
IsReadOnly . 	   AutoGenerateColumns	 .  
   Columns
 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$  ��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ 20 6H   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6/   �� *incr+6H     5       $ Номер комнаты��6%   $	 110 6\    6<   �� *room_Number+6\     5!      $# Количество мест��6%   $ * 6_    6>   �� *places_Count+6_   5#      5$      5%      