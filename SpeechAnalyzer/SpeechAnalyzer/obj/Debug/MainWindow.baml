   M S B A M L   `   `   `  ����  SpeechAnalyzer    SpeechAnalyzer.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   J FWpfAnimatedGif, Version=1.4.18.0, Culture=neutral, PublicKeyToken=null,gif"http://wpfanimatedgif.codeplex.com  5   	   $	��300��5   	   $	��550��6     � Title$!  Miejsce na nazwę narzedzia��6"    � 
ResizeMode$ NoResize=�5	   	   .��5
      � .+�5   
   � $	��200��6   $
��Left=�6   $	��Top=�66   .+�5      �� $��
Recording:��6   $��22(�6)   $��	20,50,0,0q�67   (� 5      $��30��6   $��
130,55,0,0q�6"   $	��Top=�66   $
��Left=�6N   �� 5      -   5        $��scramblingLabel��5      $��Scrambling: false��6+   $��22(�6G   $��
20,100,0,0q�6U   5      �� 5   
   -   5        $��classificationButton��5      $y�Visible=�6-   $	��Top=�6B   $
��Left=�6Z   $	��350��6u   .��5      �� -   5        $��currentClassification��5      $/��)Current classification: NATO UNCLASSIFIED��61   5      �� 5   
   -   5        $��recordingGif��5      $y�Hidden=�6$   "  WpfAnimatedGif.ImageBehavior   AnimatedSource$ animacja.gif��68   $��50��6h   $��50��6t   $	��Top=�6   $
��Left=�6�   $��130,35q�6�   �� 5   
   -   5        $��noRecording��5      $��0.png��6#   $��50��62   $��50��6>   $	��Top=�6I   $
��Left=�6a   $��130,35q�6|   � 5   
   $	��200��6   $��Right=�6   ���	#FF7C7C7C � RowDefinitions 5      �� 5      $��50 �6    �� 5      $��* �6     J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5      .+�5      � ���	#FF666666.+�5       �� $��Connected users:��6   $��22(�63   $��Center=�6A   $��Center=�6\   $
��Bold"�6y   5!      �� 5#      $��1��6   .+�5$      �� $
��10,5q�6   $��90��6'   .��5%      �� -   5        $��firstConnectedUser��5%   �   $��16(�6    $
��Wrap=�6.   $B��<Michal Bryla        NATO RESTRICTED          IP: 9.124.42.22��6B   6�   5&      �� 5'      $
��10,5q�6   $��90��6'   .��5(      �� -   5        $��secondConnectedUser��5(   �   $��0,0,30,0q�6    $��16(�62   $
��Wrap=�6@   $I��CRafal Huk                 NATO SECRET               IP: 9.124.42.12��6T   $
��Left=�6�   5)      5*      5+      �� 50   
   -   5        $��startConversationButton��50      $��Start conversation��60   $��
100,0,0,20q�6M   $	��150��6a   $��50��6m   $��Bottom=�6y   $
��Left=�6�   6�   �� 51   
   -	   5        $��stopConversationButton��51      $��Stop conversation��6/   $y�Hidden=�6K   $��
100,0,0,20q�6_   $	��150��6s   $��50��6   $��Bottom=�6�   $
��Left=�6�   6�   53      54      