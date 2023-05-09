using KIP_Translator.Model;
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
        public List<Lang> ThisLang { get; set; }
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
            var item = LangList.SelectedItem as Lang;
            CoreProject.RunNonQuery($"DELETE FROM Lang WHERE Id={item.Id}");
            LangUpdate();
        }
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            if(NavigationService.CanGoBack)
                NavigationService.Navigate(new Pages.TranslatorPage());
        }
        private void LangUpdate() 
        {
            ThisLang = CoreProject.RunQueryList<Lang>("SELECT * FROM Lang");
            LangList.ItemsSource = ThisLang;
        }
        private void HistoryUpdate() 
        {
            ThisHistory = CoreProject.RunQueryList<History>("SELECT * FROM History");
            foreach (History history in ThisHistory)
            {
                history.LangIn = CoreProject.RunQuery<Lang>($"SELECT * FROM Lang WHERE Id=\'{history.IdLangIn}\'");
                history.LangOut = CoreProject.RunQuery<Lang>($"SELECT * FROM Lang WHERE Id=\'{history.IdLangOut}\'");
            }
            historyList.ItemsSource = ThisHistory;
        }
        private void DelHist_Click(object sender, RoutedEventArgs e)
        {
            var item = historyList.SelectedItem as History;
            if (item != null) {
                CoreProject.RunNonQuery($"DELETE FROM History WHERE Id={item.Id}");
                HistoryUpdate();
            }
        }
    }
}
