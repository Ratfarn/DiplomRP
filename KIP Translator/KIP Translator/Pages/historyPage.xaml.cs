using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
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
    /// Логика взаимодействия для historyPage.xaml
    /// </summary>
    public partial class historyPage : Page
    {
        public List<Langs> ThisLang { get; set; }
        public List<History> ThisHistory { get; set; }
        public historyPage()
        {
            InitializeComponent();
            DataContext = this;
            LangUpdate();
            HistoryUpdate();
        }

        private void delLang_Click(object sender, RoutedEventArgs e)
        {
            var item = LangList.SelectedItem as Langs;
            CoreProject.GetContext().Langs.Remove(item);
            CoreProject.GetContext().SaveChanges();
            LangUpdate();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            if(NavigationService.CanGoBack)
                NavigationService.GoBack();
            
        }
        private void LangUpdate() 
        {
            ThisLang = CoreProject.GetContext().Langs.ToList();
            LangList.ItemsSource = ThisLang;
        }
        private void HistoryUpdate() 
        {
            ThisHistory = CoreProject.GetContext().History.ToList();
            historyList.ItemsSource = ThisHistory;
        }

        private void DelHist_Click(object sender, RoutedEventArgs e)
        {
            var item = historyList.SelectedItem as History;
            if (item != null) {
                CoreProject.GetContext().History.Remove(item);
                CoreProject.GetContext().SaveChanges();
                HistoryUpdate();
            }
        }
    }
}
