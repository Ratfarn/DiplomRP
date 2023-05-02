using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Speech.Synthesis;
using KIP_Translator.Model;
using System.Windows.Navigation;

namespace KIP_Translator.Pages
{
    public partial class TranslatorPage : Page
    {
        private string _lWrite;
        private string _lRead;
        private DateTime _thisDate;
        public List<Lang> GetLang { get; set; }
        SpeechSynthesizer synthesizer = new SpeechSynthesizer();
        public TranslatorPage()
        {
            InitializeComponent();
            DataContext = this;
            LoadData();

            _thisDate = DateTime.Today;
            _thisDate.ToShortDateString();

            Properties.Settings.Default.PropertyChanged += Settings_PropertyChanged;

            textWrite.ToolTip = "Для перевода текста нажмите 'Enter'\nА для перехода на следующую строку используйте 'Shift+Enter'";
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

            var text = textWrite.Text;
            textWrite.Text = textRead.Text;
            textRead.Text = text;
        }

        private void inputLang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (inputLang.SelectedItem != null)
            {
                _lWrite = (inputLang.SelectedItem as Lang).CodeLang;
            }
            
        }

        private void outputLang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (outputLang.SelectedItem != null)
            {
                _lRead = (outputLang.SelectedItem as Lang).CodeLang;
            }
        }

        private void textWrite_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            string result;
            if (e.Key == Key.Enter && (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)))
            {
                textWrite.Text += Environment.NewLine;
                textWrite.CaretIndex = textWrite.Text.Length;
                e.Handled = true;
            }
            else if (e.Key == Key.Enter)
            {
                
                result = TranslateText(textWrite.Text, _lWrite, _lRead);
                if (!String.IsNullOrEmpty(result))
                {
                    textRead.Text = result;
                    CoreProject.RunNonQuery($"INSERT INTO History(TranslateSource, TranslateTarget, Date, IdLangIn, IdLangOut) VALUES(\'{textWrite.Text}\', \'{textRead.Text}\', \'{_thisDate}\', \'{(inputLang.SelectedItem as Lang).Id}\', \'{(outputLang.SelectedItem as Lang).Id}\')");
                }
                else { MessageBox.Show("Внимание!\n", "ПРЕДУПРЕЖДЕНИЕ", MessageBoxButton.OK, MessageBoxImage.Warning); }
                e.Handled = true;
            }

            
        }

        private void sourceSpeech_Click(object sender, RoutedEventArgs e)
        {
            if (synthesizer == null)
            {
                synthesizer = new SpeechSynthesizer();
            }

            if (synthesizer.State == SynthesizerState.Speaking)
            {
                synthesizer.Pause();
            }
            else
            {
                string text = textWrite.Text;

                if (!string.IsNullOrEmpty(text))
                {
                    synthesizer.SpeakAsync(text);
                }
            }
        }

        private void targetSpeech_Click(object sender, RoutedEventArgs e)
        {
            if (synthesizer == null)
            {
                synthesizer = new SpeechSynthesizer();
            }

            if (synthesizer.State == SynthesizerState.Speaking)
            {
                synthesizer.Pause();
            }
            else
            {
                string text = textRead.Text;

                if (!string.IsNullOrEmpty(text))
                {
                    synthesizer.SpeakAsync(text);
                }
            }
        }

        private void LoadData()
        {
            GetLang = CoreProject.RunQueryList<Lang>("SELECT * FROM Lang");
            inputLang.ItemsSource = GetLang;
            outputLang.ItemsSource = GetLang;

            inputLang.SelectedIndex = 0;
            outputLang.SelectedIndex = 0;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
            if (Global.text != null || Global.text != string.Empty) 
            {
                textWrite.Text = Global.text;
                Global.text = string.Empty;
            }
        }

    }
}
