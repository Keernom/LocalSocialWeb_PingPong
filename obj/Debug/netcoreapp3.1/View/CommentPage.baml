   M S B A M L   `   `   `  ����P  LKursovik_Kocherzhenko, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:Kursovik_Kocherzhenko.ViewKursovik_Kocherzhenko.View  5      ,    &Kursovik_Kocherzhenko.View.CommentPage   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      :local(clr-namespace:Kursovik_Kocherzhenko.View     5        N� Title$  CommentPage��5	      .��5      �  � RowDefinitions 5   
   �� 5      $��2* �6   �� 5      $��5* �6   �� 5      .+�5   
   ��  �� Orientation$ Vertical=�6   .+�5      �� $ 
Horizontal=�6   $
��Left=�5      .+�5      �� -   5         ��Name$ postAuthorImage��5      $��40��61   �� 5      -   5        $��postAuthorName��5      5      �� 5      
����������#00ADB5$��2q�5       �� CornerRadius$ 15��6%   $��10, 10, 10, 0q�67   $
��Left=�5      ._�5      �� $��20(�66   $
��Left=�5       �� Padding$ 10q�6/   ��5      �� *
PostText+5   /   5      �� 5      $ 
Horizontal=�6   $��15, 0q�62   $��Bottom=�5       .+�5!      �� -   5        $��
LikesPanel��5#   !   $ 
Horizontal=�5!      $
��Left=�5"   !   5$      �� 5%      -   5        $��CommentsPanel��5'   !   $ 
Horizontal=�5%      $
��Left=�5&   !   5(      5)      5*      �� 5,   
   ���#00ADB5$��1q�6'   $��Bottom=�5-      �� 5/   
   -   5        $��Scroller��51      $��1��5/      $��Hidden=�6$   $��False��50      $
��15,0q�60   $��1��6>   $��2��51   '   .��52      �� -   5        $��PostCommentResult��52      $��Stretch=�62   $	��Top=�53      $ Vertical=�60   54      55      �� 57   
   ���#00ADB5$��1q�6'   $	��Top=�58      $��2��6)   � 5:   
   $��2��6   $��10q�6    � ColumnDefinitions 5;      �� 5<      $��5* �6#   �� 5=      .+�5?      �� $��0��6   ���#00ADB5$��2q�5@      $ 15��6)   
�������5A      �� 5B      -   5        $��
MessageBox��5B      $��Stretch=�5D      $��20(�5E      $��10q�6    $��Center=�6,    �� TextWrapping$
 Wrap=�5F      $��0q�6&   ��5C      �� *CommentText+5F   &   �� 5G      $��1��6   $��Отправить��6%   $��10q�5H   5   ��6   �� *SendComment+65   5I      5J      5K      