﻿using System;
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

namespace KIP_Translator.Pages
{
    public partial class TranslatorPage : Page
    {
        private string _lWrite;
        private string _lRead;
        private DateTime _thisDate;
        public SpeechRecognitionEngine RecogEngine = new SpeechRecognitionEngine();

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

            // рекогназер
            RecogEngine.SpeechRecognized += _recogEngine_SpeechRecognized;
            Grammar dictationGrammar = new DictationGrammar();
            // Устанавливаем грамматику для распознавателя речи
            RecogEngine.LoadGrammar(dictationGrammar);
        }
        private void Settings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            textWrite.FontSize = Properties.Settings.Default.fontSizeValue;
            textRead.FontSize = Properties.Settings.Default.fontSizeValue;
        }
        public string TranslateText(string input, string _lWrite, string _lRead)
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

        private void changeBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            inputLang.SelectedItem = outputLang.SelectedItem;
            outputLang.SelectedItem = inputLang.SelectedItem;
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
            if (e.Key == Key.Enter)
            {
                textRead.Text = TranslateText(textWrite.Text, _lWrite, _lRead);

                History hist = new History();
                hist.TranslateSource = textWrite.Text;
                hist.TranslateTarget = textRead.Text;
                hist.Date = _thisDate;
                hist.IdLangIn = inputLang.SelectedIndex + 1;
                hist.IdLangOut = outputLang.SelectedIndex + 1;

                CoreProject.GetContext().History.Add(hist);
                CoreProject.GetContext().SaveChanges();
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
        // преобразование голоса в текст
        private void speechBtn_Checked(object sender, RoutedEventArgs e)
        {
            //if (speechBtn.IsChecked == true)
            //{
            //    RecogEngine.SetInputToDefaultAudioDevice();
            //    if (RecogEngine.State == SpeechRecognitionEngineState.Stopped) //ругается только на State и SpeechRecognitionEngineState
            //    {
            //        RecogEngine.RecognizeAsync(RecognizeMode.Multiple);
            //    }
            //}
            //else
            //{
            //    if (RecogEngine.State == SpeechRecognitionEngineState.Recognizing) // здесь тоже самое! и я не знаю как это решить
            //    {
            //        RecogEngine.RecognizeAsyncStop();
            //    }
            //}
        }
    }
}
