   M S B A M L   `   `   `  ����P  LKursovik_Kocherzhenko, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:Kursovik_Kocherzhenko.ViewKursovik_Kocherzhenko.View  5      +    %Kursovik_Kocherzhenko.View.DialogPage   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      :local(clr-namespace:Kursovik_Kocherzhenko.View     5        N� Title$  
DialogPage��5	      .��5      �  � RowDefinitions 5   
   �� 5      �� 5      $��6* �6   �� 5      .+�5   
   �� -   5        $��
GetterName��5      $��Center=�6&   $��30(�5      $��
ExtraLight"�6"   �� 5   
   $��0��6   ���#00ADB5$��1q�5      $��Bottom=�5      �� 5   
   -   5        $��Scroller��5      $��1��5      $��Hidden=�6$   $��False��5      $
��15,0q�60   .��5      ]� -   5        $��DialogResult��5      $��Stretch=�6,   $��Bottom=�5      5      5      � 5   
   $��2��6   $��10q�6    � ColumnDefinitions 5      �� 5      $��5* �6#   �� 5       .+�5"      �� $��0��6   ���#00ADB5$��2q�5#       �� CornerRadius$ 15��6)   
�������5$      �� 5%      -   5        $��
MessageBox��5%      $��Stretch=�5'      $��20(�5(      $��10q�6     �� TextWrapping$
 Wrap=�5)      $��0q�6&   ��5&      �� *MessageText+5)   &   �� 5*      $��1��6   $��Отправить��6%   $��10q�5+   5   ��6   �� *SendMessage+65   5,      5-      5.      