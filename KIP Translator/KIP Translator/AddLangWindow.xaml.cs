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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Langs AddLangs = new Langs();

            AddLangs.NameLang = NameLangText.Text;
            AddLangs.CodeLang = CodeLangText.Text;

            CoreProject.GetContext().Langs.Add(AddLangs);
            CoreProject.GetContext().SaveChanges();
            
            this.Close();
        }
    }
}
