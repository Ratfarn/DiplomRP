using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tesseract;

namespace KIP_Translator.Pages
{
    /// <summary>
    /// Логика взаимодействия для imageToTextPage.xaml
    /// </summary>
    public partial class imageToTextPage : System.Windows.Controls.Page
    {
        OpenFileDialog open = new OpenFileDialog();
        string chooseLang;
        public imageToTextPage()
        {
            InitializeComponent();
            ChangeLang.SelectedIndex = 0;
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.Navigate(new Pages.TranslatorPage());
        }

        private void picImageBtn_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage bitmap = new BitmapImage();
            try
            {
                open.Filter = "Image Files(*.jpg; *.jpeg; *.bmp;)|*.jpg; *.jpeg; *.bmp;";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(open.FileName, UriKind.Absolute);
                    bitmap.EndInit();

                    imagePic.Source = bitmap;
                    pathText.Text = open.FileName;
                }
            }
            catch 
            {
                System.Windows.MessageBox.Show("Внимание!\nЗагруженное изображение имеет неправильное расширение","ПРЕДУПРЕЖДЕНИЕ",MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void convertBtn_Click(object sender, RoutedEventArgs e)
        {
            if (open.FileName != string.Empty)
            {
                using (var engine = new TesseractEngine(@"./tessdata", chooseLang, EngineMode.Default))
                {
                    using (var img = Pix.LoadFromFile(open.FileName))
                    {
                        using (var page = engine.Process(img))
                        {
                            imageText.Text = page.GetText();
                        }
                    }
                }
            }
        }

        private void ChangeLang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (ChangeLang.SelectedIndex)
            {
                case 0:
                    chooseLang = "rus";
                    break;
                case 1:
                    chooseLang = "eng";
                    break;
            }
        }

        private void ToTranslateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (imageText.Text != string.Empty) 
            {
                Global.text = imageText.Text;
                NavigationService.Navigate(new Pages.TranslatorPage());
            }
        }
    }
}
