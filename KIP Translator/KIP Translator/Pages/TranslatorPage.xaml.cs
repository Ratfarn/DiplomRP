using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Script.Serialization;
using System.Windows.Controls;

namespace KIP_Translator.Pages
{
    /// <summary>
    /// Логика взаимодействия для TranslatorPage.xaml
    /// </summary>
    public partial class TranslatorPage : Page
    {
        string langWrite;
        string langRead;
        public TranslatorPage()
        {
            InitializeComponent();
        }
        public string TranslateText(string input)
        {
            string url = String.Format
            ("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}",
             "vi", "en", Uri.EscapeUriString(input));

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
            textRead.Text = TranslateText(textWrite.Text);
        }
    }
}
