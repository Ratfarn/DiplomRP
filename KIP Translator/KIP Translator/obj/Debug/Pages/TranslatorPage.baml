   M S B A M L   `   `   `  ����I  EKIP Translator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:KIP_Translator.PagesKIP_Translator.Pages  5      )    #KIP_Translator.Pages.TranslatorPage   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      4local"clr-namespace:KIP_Translator.Pages     5      -   5          N� Title$  TranslatorPage��5	      .��5      �  � RowDefinitions 5   
   �� 5      $	��72* �6   �� 5      $��28 �6   $��28��6(   $��28��67   �� 5      $
��188* �6   �� 5      $	��95* �6    � ColumnDefinitions 5   
   �� 5      $
��167* �6   �� 5      $	��25* �6   �� 5      $
��166* �6   .+�5   
   �� -   5         ��Name$ 	textWrite��5       �� TextWrapping$
 Wrap=�6%    �� AcceptsReturn . $��20,3,8,6q�6N   $��2��6`   6m   �� 5   
   -   5        $ textRead��5      $
 Wrap=�6$    . $��4,3,22,6q�6M   $��2��6_   $��2��6o   �� 5   
   -   5         ��Name$ 	changeBtn��5      $	��Top=�6$   $��0,10,0,0q�6<   $��34��6N   $��40��6Z   $��1��6e   $��2��6u   .��5      �� $��/Images/changeLang.png��6   5      �� 5   
   -   5         ��Name$ 	inputLang��5      $��Right=�6&   $��0,4,9,0q�6B   $��1��6S   $	��158��6�   ��5      �� 5      .R�5      �� ��6    �� *
NameLang+5       5"      �� 5#   
   -   5        $ 
outputLang��5#      $��Right=�6'   $��0,4,22,0q�6C   $��2��6U   $��1��6e   $	��158��6�   ��5$      �� 5%      .R�5&      �� ��6    �� *
NameLang+5'      5)      �� 5*   
   -   5        $��sourceSpeech��5*      $��Right=�6A   $��0,0,9,0q�6]   $��3��6n   $	��Top=�6{   .��5+      �� $��/Images/speech.png��6   $��20��60   $��20��6;    J� RenderTransformOrigin$ 	0.5,0.625&�6G   5,      �� 5-   
   -   5        $��targetSpeech��5-   !   $��2��6   $��Right=�6Q   $��0,0,22,0q�6m   $��3��6   $	��Top=�6�   .��5.      �� $��/Images/speech.png��6   $��20��60   $��20��6;   5/      50      51      