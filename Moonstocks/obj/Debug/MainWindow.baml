   M S B A M L   `   `   `  ����E  AMoonstocks, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:Moonstocks
Moonstocks  5   	   =#clr-namespace:Moonstocks.ViewModelsMoonstocks.ViewModels  5   	   3clr-namespace:Moonstocks.ViewsMoonstocks.Views  5   	       Moonstocks.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:Moonstocks     5   	   :
viewmodels#clr-namespace:Moonstocks.ViewModels     5   	   0viewsclr-namespace:Moonstocks.Views     5   	     � Icon$  Resources/logo.png��5
   	    � WindowStyle$
 None=�5   	    � Title$ 
Moonstocks��5   	   $	��550��5   	   $	��978��6   '  !System.Windows.Shell.WindowChrome   WindowChrome 5        5   
      CaptionHeight$ 34Q�6      CornerRadius$ 0��5   	      GlassFrameThickness$ 0q�5   	      NonClientFrameEdges$
 None=�5   	    � 	Resources 5      	 �� MergedDictionaries
 �� Source�� 5   
   	 5      �� 5      $)
 #Dictionaries/DefaultDictionary.xaml?�6%   %    5       J� CommandBindings 5!      )  #System.Windows.Input.CommandBinding  5"   
   -   5        #  System.Windows.SystemCommands   CloseWindowCommand   Command# Z 5"      6w     5#   
   -   5           MaximizeWindowCommand# Z 5#      6z     5$   
   -   5           MinimizeWindowCommand# Z 5$      6z     5%   
   -   5           RestoreWindowCommand# Z 5%      6y   .��5(      �� -   5         ��Name$ MainWindowBorder��5(       
  Header#���   6'   $��1q�6N   ._�5)   
   � -   5         �Name$ parentContainer��5)        
Background#���  6(    � RowDefinitions 5+      �� 5,      $
��Auto �6    �� 5-      $��* �6    .+�52      � $��0��6   $��30��6    #���   6,   .+�53      ��  �� Orientation$ 
Horizontal=�6   $
��Left=�66   $��Center=�6Q   .+�55      �� $��/Resources/logo.png��6   $��18��69   $��7,2,2,2q�6D   $
��Left=�6U   $��Center=�6p   �� 57      $��
Moonstocks��6    $��Center=�62     TextHighAlt#���  6M   $��4 0 0 0q�6x   $��Resources/Fonts/#Poppins)�6�   58      �� 5;      $ 
Horizontal=�6   $��Right=�66   .+�5<      ��   MinimizeButtonStyle#��[ 6      IsHitTestVisibleInChrome .  � ToolTip$ Minimize��6w   #��Z 5=      ��.  6W   �� 5>      -   5         ��Name$ RestoreButton��5>      $y�	Collapsed=�64     RestoreButtonStyle#��[ 6K   #��Z 5?       . $ Restore��6�   ��.  6�   �� 5@      -   5        $ MaximizeButton��5@      $y�Visible=�65     MaximizeButtonStyle#��[ 6J   #��Z 5A       . $ Maximize��6�   ��.  6�   �� 5B        CloseButtonStyle#��[ 6    . $ Close��6t   #��Z 5C      ��.  6T   5D      5E      �� 5H      -	   5         ��Name$ AppArea��5H   *   $��1��6   ��.  ��6;   �� *CurrentViewModel+ 5J       �� DataType+   %Moonstocks.ViewModels.SignInViewModel!   Moonstocks.Views.SignInView2   ,Moonstocks.ViewModels.CreateAccountViewModel(   "Moonstocks.Views.CreateAccountView)   #Moonstocks.ViewModels.HomeViewModel	   Moonstocks.Views.HomeView.
   (Moonstocks.ViewModels.ForgotPasswordView)   #Moonstocks.Views.ForgotPasswordView��%,  (��       5K   #   *, +)(�� 2     5N   #   *, +)(�� d     5Q   #   *, +)(�� �     5T   #   *,
 +)�� 5K      "  6#   .R�5L        5M      �� 5N      "  6#   .R�5O        5P      �� 5Q      "  6#   .R�5R      	  5S      �� 5T      " 
 6#   .R�5U        5V      5W      5X      5Y      5Z      5[      