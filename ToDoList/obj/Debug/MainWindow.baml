   M S B A M L   `   `   `  ����C  ?ToDoList, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:ToDoListToDoList  5   	       ToDoList.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:ToDoList     5   	   V RMaterialDesignThemes.Wpf, Version=2.5.0.1205, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5	   
   -   5          � Title$  
MainWindow��5   	   $	��350��6   $��305.603��6)      MaterialDesignBody#���   5
      $��Regular"�5      $��13(�5      &   System.Windows.Media.TextOptions   TextFormattingMode$ Ideal=�5         TextRenderingMode$
 Auto=�5        MaterialDesignPaper#���  5        MaterialDesignFont#���  5       � WindowStyle$
 None=�68    � ShowInTaskbar .   � 
ResizeMode$ NoResize=�6a    � AllowsTransparency . .��5      �� -   5        $��borMain��5      ���#673AB7$��2 0 2 2q�63   ._�5   
   �  � RowDefinitions 5      �� 5      $
��auto �6    60   �� 5      6"   �� 5      $
��auto �6    .+�5      (  "MaterialDesignThemes.Wpf.ColorZone  -   5           Mode$ 
PrimaryMid=�5   	   .��5      ]� .+�5      '  !MaterialDesignThemes.Wpf.PopupBox  $��Right=�6.   	   PopupContent.	 5      �� .+�5      �� -   5        $��ckbCanResize��5   '   $��5 5 5 5q�6;   .��6�   
Can Resize6�   �� 5      -   5        $��
mnuSetting��5   *   .��6Y   	Setting6b   �� 5       �� 5!      -   5        $��mnuClose��5!   *   .��6U   Close6\   5"      5#      '  !MaterialDesignThemes.Wpf.PackIcon  5$      -   5        �  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=2.5.0.1205, Culture=neutral, PublicKeyToken=null]]
   Kind$
 AddBox=�5$   .   $��Right=�6<   $��35��6S   $��35��6_   6j     5%      -   5        $��btnSync��5%   .   $

 Sync=�6=   $��Right=�6I   $��35��6`   $��35��6l   6w   �� 5&      -	   5        $��txtTitle��5&       $��Center=�60   $��10 10 10 10q�6K   
To Do List6l   5'      5(      �� 5*      -
   5        $��
mainViewer��5*   (   $��1��6   $��Hidden=�6:   5+      �� 5-      $��2��6   .+�5.      �� �� 5/      -   5        $��txtLastUpdate��5/      $��Last Update��61    �� TextAlignment$ Center=�6D   $��Center=�6[   $��20��6v   50      ]� 51      $��2��6   .+�52        -   5        $��	btnResize��52   *   $
 ResizeBottomRight=�6;   $��Right=�6T   $��Right=�6k   $��Bottom=�6�   53      54      55      56      