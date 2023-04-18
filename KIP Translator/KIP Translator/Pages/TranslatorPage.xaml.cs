using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Speech.Synthesis;
using System.Speech.Recognition; //здесь подключение библиотеки на которую ругается
using System.Globalization;

namespace KIP_Translator.Pages
{
    public partial class TranslatorPage : Page
    {
        private string _lWrite;
        private string _lRead;
        private DateTime _thisDate;


        public List<Langs> GetLang { get; set; }
        public TranslatorPage()
        {
            InitializeComponent();
            DataContext = this;

            GetLang = CoreProject.GetContext().Langs.ToList();
            inputLang.ItemsSource = GetLang;
            outputLang.ItemsSource = GetLang;

            inputLang.SelectedIndex = 0;
            outputLang.SelectedIndex = 0;

            _thisDate = DateTime.Today;
            _thisDate.ToShortDateString();

            Properties.Settings.Default.PropertyChanged += Settings_PropertyChanged;

        }
        private void Settings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            textWrite.FontSize = Properties.Settings.Default.fontSizeValue;
            textRead.FontSize = Properties.Settings.Default.fontSizeValue;
        }
        public string TranslateText(string input, string _lWrite, string _lRead)
        {
            try
            {
                string url = String.Format
                ("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}",
                _lWrite, _lRead, Uri.EscapeUriString(input));

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
            catch 
            {
                return "";
            }
        }

        private void changeBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var item = inputLang.SelectedItem;
            inputLang.SelectedItem = outputLang.SelectedItem;
            outputLang.SelectedItem = item;
        }

        private void inputLang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = CoreProject.GetContext().Langs.ToList();
            _lWrite = item.First(x => x == inputLang.SelectedItem as Langs).CodeLang;
        }

        private void outputLang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = CoreProject.GetContext().Langs.ToList();
            _lRead = item.First(x => x == outputLang.SelectedItem as Langs).CodeLang;
        }

        private void textWrite_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            string result;
            if (e.Key == Key.Enter)
            {
                result = TranslateText(textWrite.Text, _lWrite, _lRead);
                if (!String.IsNullOrEmpty(result))
                {
                    textRead.Text = result;

                    History hist = new History();
                    hist.TranslateSource = textWrite.Text;
                    hist.TranslateTarget = textRead.Text;
                    hist.Date = _thisDate;
                    hist.IdLangIn = inputLang.SelectedIndex + 1;
                    hist.IdLangOut = outputLang.SelectedIndex + 1;

                    CoreProject.GetContext().History.Add(hist);
                    CoreProject.GetContext().SaveChanges();
                }
                else { MessageBox.Show("Внимание!\n", "ПРЕДУПРЕЖДЕНИЕ", MessageBoxButton.OK, MessageBoxImage.Warning); }
            }
        }

        private static void TextToSpeech(string a)
        {
            SpeechSynthesizer _speech = new SpeechSynthesizer();
            _speech.Speak(a);
            _speech.Dispose();
        }

        private void sourceSpeech_Click(object sender, RoutedEventArgs e)
        {
            if (textWrite.Text != string.Empty) 
            {
                TextToSpeech(textWrite.Text);
            }
        }

        private void targetSpeech_Click(object sender, RoutedEventArgs e)
        {
            if (textRead.Text != string.Empty)
            {
                TextToSpeech(textRead.Text);
            }
        }

        private void _recogEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e) 
        {
            string text = e.Result.Text;
            textWrite.Text = text;
        }
    }
}
