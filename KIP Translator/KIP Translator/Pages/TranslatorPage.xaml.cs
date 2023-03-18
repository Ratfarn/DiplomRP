using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;

namespace KIP_Translator.Pages
{
    public partial class TranslatorPage : Page
    {
        private string lWrite;
        private string lRead;
        public List<Langs> GetLang { get; set; }
        public TranslatorPage()
        {
            InitializeComponent();
            DataContext = this;
            GetLang = CoreProject.GetContext().Langs.ToList();
            inputLang.SelectedIndex = 0;
            outputLang.SelectedIndex = 0;
        }

        public string TranslateText(string input, string lWrite, string lRead)
        {
            string url = String.Format
            ("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}",
             lWrite, lRead, Uri.EscapeUriString(input));

            HttpClient httpClient = new HttpClient();//создание нового экземпляра HTTP-запроса
            string result = httpClient.GetStringAsync(url).Result;// получение json 

            var jsonData = new JavaScriptSerializer().Deserialize<List<dynamic>>(result);

            var translationItems = jsonData[0];

            string translation = "";
            foreach (object item in translationItems)//обработка запроса (выявление текста из запроса)
            {
                IEnumerable translationLineObject = item as IEnumerable;
                IEnumerator translationLineString = translationLineObject.GetEnumerator();
                translationLineString.MoveNext();
                translation += string.Format(" {0}", Convert.ToString(translationLineString.Current));
            }
            if (translation.Length > 1) { translation = translation.Substring(1); };
            return translation;
        }

        private void changeBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //MessageBox.Show("Не доработано","Предупреждение");
            textRead.Text = TranslateText(textWrite.Text , lWrite, lRead);
        }

        private void inputLang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = CoreProject.GetContext().Langs.ToList();
            lWrite = item.First(x => x == inputLang.SelectedItem as Langs).CodeLang;
        }

        private void outputLang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = CoreProject.GetContext().Langs.ToList();
            lRead = item.First(x => x == outputLang.SelectedItem as Langs).CodeLang;
        }
    }
}
