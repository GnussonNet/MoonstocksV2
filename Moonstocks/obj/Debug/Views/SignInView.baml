   M S B A M L   `   `   `  ����E  AMoonstocks, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Moonstocks.ViewsMoonstocks.Views  5      =#clr-namespace:Moonstocks.ConvertersMoonstocks.Converters  5      P LLoadingIndicators.WPF, Version=0.0.1.0, Culture=neutral, PublicKeyToken=null\Bclr-namespace:LoadingIndicators.WPF;assembly=LoadingIndicators.WPFLoadingIndicators.WPF 5      !    Moonstocks.Views.SignInView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:Moonstocks.Views     5      :
converters#clr-namespace:Moonstocks.Converters     5      VloadingBclr-namespace:LoadingIndicators.WPF;assembly=LoadingIndicators.WPF     5        � 	Resources  5       �� MergedDictionaries �� Source8   2Moonstocks.Converters.BooleanToVisibilityConverter    BooleanToVisibilityConverter�� 5   
    5      �� 5      $* $/Dictionaries/DefaultDictionary.xaml?�6%   �� 5      $H Bpack://application:,,,/LoadingIndicators.WPF;component/Styles.xaml?�6%   %   &	          5      66   5      .��5      �   
Background#���  6   .+�5   
   � $��1��6�   $��2��6�   y�6   �� *IsBusy+ �� 	Converter# [   �� ConverterParameter$
 true�� �� UpdateSourceTrigger$ PropertyChanged=�.+�5      ,  &LoadingIndicators.WPF.LoadingIndicator  -   5        $��DoubleBounce��5   M    
 Header#���  6'   $��	0 -30 0 0q�6a      
SpeedRatio$ 1Q�6t      IsActive .  % !LoadingIndicatorDoubleBounceStyle#���  6�   �� 5      $��Loading user credentials��6     TextHigh#���  68   $��0 65 0 0q�6`   $��17(�6r   $��Center=�6�   $��Center=�6�   $%��Resources/Fonts/#Poppins Medium)�6�   $��Medium"�6�   5       � 5#   
   $��1��6�   $��2��6�   y�6   �� *IsBusy+# [  $ false��$ PropertyChanged=� � ColumnDefinitions 5%      �� 5&      $��* �6#   �� 5'      $
��1.5* �6#   $	��600��60   �� 5(      $��* �6#   	 � RowDefinitions	 5+      �� 5,      $��* �6    �� 5-      $
��auto �6    �� 5.      $
��auto �6    �� 5/      $��5 �6    �� 50      $
��auto �6    �� 51      $
��auto �6    �� 52      $
��auto �6    �� 53      $
��auto �6    �� 54      $
��auto �6    �� 55      $��32 �6    �� 56      $
��auto �6    �� 57      $��* �6    .+�5;      �� $��1��6   $��1��6$   $��Email��61   $��0,7,5,7q�6A   #���  6S   $��Resources/Fonts/#Poppins)�6{   6�   �� 5<      $��1��6   $��2��6&     PrimaryTextbox#��[ 63   $��0��6[   $��Resources/Fonts/#Poppins)�6�   $��13(�6�   ��6h   �� *Email+
 �� Mode$
 TwoWay=�$ PropertyChanged=�6�   �� 5?      $��1��6   $��4��6$   $��Password��61   $��0,7,5,7q�6D   #���  6V   $��Resources/Fonts/#Poppins)�6~   6�   �� 5@      $��1��6   $��5��6&   #��[ 63   $��1��6[   $��Resources/Fonts/#Poppins)�6�   $��13(�6�   ��6h   �� *
Password+$
 TwoWay=�$ PropertyChanged=�6�   � 5D      $��1��6   $��6��6#    5E      �� 5F      $��* �6'   �� 5G      $
��auto �6'   .+�5I      �� $��0��6     primaryCheckbox#��[ 6+   $
��Left=�6T   $	��0 8q�6o   $��Remember me��6|   $��2��6�   $��Resources/Fonts/#Poppins)�6�    d� 	IsChecked 6�   �� *
RememberMe+$
 TwoWay=�$ PropertyChanged=�6  �� 5J      $��1��6     ForgotPasswordButton#��[ 6)   $��20��6W   $	��106��6c   $��Forgot password?��6r    � Cursor$
 Hand��6�   $��Right=�6�   #���  6�   $��0q�6�   $��3��6$  $��Resources/Fonts/#Poppins)�61  ��6�   �� *ForgotPasswordCommand+61  5K      �� 5N      $��1��6   $��7��6%   $��Login��62     PrimaryButton#��[ 6B   $��4��6�   $��Resources/Fonts/#Poppins)�6�   $��13(�6�   ��6i   �� *SignInCommand+��6�   ��  �� ElementName$ txtPassword��6  �� 5Q      $��1��6   $��9��6$   $��or��61   $��0,6,5,6q�6>   $��Center=�6P   $��Center=�6r   #���  6�   $��Resources/Fonts/#Poppins)�6�   6�   �� 5T      $��1��6   $��10��6%   $��Create account��63   #��[ 6L   $��5��6�   $��Resources/Fonts/#Poppins)�6�   $��13(�6�   ��6s   �� *NavigateCreateAccountCommand+6�   5V      5W      5X      