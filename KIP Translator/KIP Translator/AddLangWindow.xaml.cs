using KIP_Translator.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KIP_Translator
{
    /// <summary>
    /// Логика взаимодействия для AddLangWindow.xaml
    /// </summary>
    public partial class AddLangWindow : Window
    {
        public AddLangWindow()
        {
            InitializeComponent();
        }

        private void addLangBtn_Click(object sender, RoutedEventArgs e)
        {
            if (NameLangText.Text != string.Empty && CodeLangText.Text != string.Empty)
            {
                CoreProject.RunNonQuery($"INSERT INTO Lang(NameLang, CodeLang) VALUES (\'{NameLangText.Text}\', \'{CodeLangText.Text}\')");
                this.DialogResult = true;
                this.Close();
            }
            else { MessageBox.Show("Заполните все поля","ВНИМАНИЕ"); }
        }

        private void CodeLangText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CodeLangText.Text.Length <= 8)
            {
                CodeLangText.Background = new SolidColorBrush(Colors.White);
                addLangBtn.IsEnabled = true;
            }
            else
            {
                CodeLangText.Background = new SolidColorBrush(Colors.Red);
                addLangBtn.IsEnabled = false;
            }
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
