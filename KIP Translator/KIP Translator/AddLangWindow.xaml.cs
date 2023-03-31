using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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
                Langs AddLangs = new Langs();

                AddLangs.NameLang = NameLangText.Text;
                AddLangs.CodeLang = CodeLangText.Text;

                CoreProject.GetContext().Langs.Add(AddLangs);
                CoreProject.GetContext().SaveChanges();

                this.Close();
            }
            else { MessageBox.Show("Заполните все поля","ВНИМАНИЕ"); }
        }

        private void CodeLangText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CodeLangText.Text.Length > 2)
            {
                CodeLangText.Background = new SolidColorBrush(Colors.Red);
                addLangBtn.IsEnabled = false;
            }
            else
            {
                CodeLangText.Background = new SolidColorBrush(Colors.White);
                addLangBtn.IsEnabled = true;
            }
        }
    }
}
