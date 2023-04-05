using KIP_Translator.Pages;
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
using System.Windows.Threading;

namespace KIP_Translator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        TranslatorPage page;
        public MainWindow()
        {
            InitializeComponent();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
        }
        int time = 0;
        private void Timer_Tick(object sender, EventArgs e)
        {
            time++;
            if (time == 5)
            {
                settingsFrame.Visibility = Visibility.Hidden;
                time = 0;
                timer.Stop();
            }
        }
        private void settingsFrame_MouseEnter(object sender, MouseEventArgs e)
        {
            timer.Stop();
            time = 0;
        }
        private void settingsFrame_MouseLeave(object sender, MouseEventArgs e)
        {
            timer.Start();
        }

        private void settingsBtn_Click(object sender, RoutedEventArgs e)
        {
            settingsFrame.Visibility = Visibility.Visible;
        }

        private void historyBtn_Click(object sender, RoutedEventArgs e)
        {
            nextPage.NavigationService.Navigate(new Pages.historyPage());
        }

        private void imageConvertBtn_Click(object sender, RoutedEventArgs e)
        {
            nextPage.NavigationService.Navigate(new Pages.imageToTextPage());
        }
    }
}
