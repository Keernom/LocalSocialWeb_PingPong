   M S B A M L   `   `   `  ����P  LKursovik_Kocherzhenko, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:Kursovik_Kocherzhenko.ViewKursovik_Kocherzhenko.View  5      )    #Kursovik_Kocherzhenko.View.UserPage   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      :local(clr-namespace:Kursovik_Kocherzhenko.View     5        N� Title$  UserPage��5	      .��5
      �  � RowDefinitions 5   
   �� 5      $
��.15* �6   �� 5       � ColumnDefinitions 5   
   �� 5      $	��.7* �6   �� 5      �� 5      $	��.1* �6   .+�5   
   ��  �� Orientation$ Vertical=�6   $��2��6,   .+�5      �� $��10, 0q�5      $��36(�6'   $��DemiBold"�65   ��5      �� *
UserName+5   5   �� 5      $��10, 0q�5      $��36(�6'   $��DemiBold"�65   ��5      �� *UserSurname+5   5   �� 5      -   5        $��AvatarImage��5      $
��Left=�6'   $	��150��5      $��10, 20q�6!   �� 5      $��Рейтинг��6   $��50(�6'   $��Center=�5      $
��Bold"�5      $��0, 100, 0, 0q�6*   �� 5      $��50(�66   $��Center=�5      $��DemiBold"�5       ���#00ADB5��5      �� *RandomRating+5    .   5!      �� 5"   
   $��Right=�6   $��2��6-   ���#00ADB5$��.5q�5#   +   �� 5$   
   $��1��6   
������� �� CornerRadius$ 15��64   $��10q�5%      ���#00ADB5$��2q�63   �� 5&   
   $��1��6   $��20, 15q�6"   $��0q�62   $��20(�5'   ,   ��6   �� *
PostText+6,   �� 5(   
   $��Bottom=�6   $��1��6,   $��2��6<   ���#00ADB5$��.5q�5)   +    K� ZIndex$ 2��5*      �� 5+   
   $��3��6   $��+��6!   $��0, 10, 10, 10q�6-   ��5,      �� *	AddPost+�� 5-   
   -   5        $��Scroller��5/      $��1��5-      $��Hidden=�6$   $��False��5.      $
��15,0q�60   $��1��6>   $��2��5/   '   .��50      �� -   5        $��UserPostResult��50      $��Stretch=�6/   $	��Top=�51      $ Vertical=�60   52      53      54      55      