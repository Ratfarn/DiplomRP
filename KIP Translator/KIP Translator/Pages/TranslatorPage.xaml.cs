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
    /// Логика взаимодействия для TranslatorPage.xaml
    /// </summary>
    public partial class TranslatorPage : Page
    {
        public TranslatorPage()
        {
            InitializeComponent();
        }

        private void TextWrite_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextRead.Text = TextWrite.Text;
        }
    }
}
