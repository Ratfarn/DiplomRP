using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KIP_Translator.Pages
{
    /// <summary>
    /// Логика взаимодействия для settingsPage.xaml
    /// </summary>
    public partial class settingsPage : Page
    {
        public settingsPage()
        {
            InitializeComponent();
            TitleMob.Content = "МОБИЛЬНАЯ\n    ВЕРСИЯ";
        }
        private void AddLang_Click(object sender, RoutedEventArgs e)
        {
            AddLangWindow Window = new AddLangWindow();
            bool result = (bool)Window.ShowDialog();
            if (result)
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.nextPage.Refresh();
            }
        }
        private void fontSizeValue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Properties.Settings.Default.fontSizeValue = fontSizeValue.Value;
            Properties.Settings.Default.Save();
        }
    }
}
