using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Newtonsoft.Json;
using Translation.Api;
using TranslationExportTool;
using Translation.Util;
using Translation.WebApi.KinsoftApi;
using Translation.WebApi.YouDaoApi;
using Path = System.IO.Path;

namespace TranslationExportTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private EnglishWordSqliteHelper _englishWordSource;
        //private EnglishWordSqliteHelper _englishWordOutput;
        public MainWindow()
        {
            InitializeComponent();
            _englishWordSource = new EnglishWordSqliteHelper(CustomPathUtil.DbSourcePath);
            //_englishWordOutput = new EnglishWordSqliteHelper(CustomPathUtil.DbOuputPath);
        }

        public string DbSourcePath => CustomPathUtil.DbSourcePath;
        public string DbOuputPath => CustomPathUtil.DbOuputPath;

        private async void ExportButton_OnClick(object sender, RoutedEventArgs e)
        {
            //if (Directory.Exists(CustomPathUtil.WordAudioFolder))
            //{
            //    Directory.Delete(CustomPathUtil.WordAudioFolder, true);
            //}

            //if (Directory.Exists(CustomPathUtil.SentenceAudioFolder))
            //{
            //    Directory.Delete(CustomPathUtil.SentenceAudioFolder, true);
            //}

            var words = GetRequestingWords();
            int index = 0;
            foreach (var word in words)
            {
                ProgressTextBlock.Text = $"{++index}.{word}";
                //延时获取，否则获取次数过多会被禁用
                await Task.Delay(TimeSpan.FromSeconds(0.001));
                var wordData = await SearchWordAsync(word);
                if (string.IsNullOrEmpty(wordData.UkPronounce.PronounceUri) && string.IsNullOrEmpty(wordData.UsPronounce?.PronounceUri)
                    && wordData.Translations.Count == 0)
                {
                    var wordInfos = _englishWordSource.GetAllWords();
                    var wordInfo = wordInfos.First(i => i.Word == word);
                    _englishWordSource.DeleteWord(wordInfo);
                    continue;
                }
                EnglishWordExportService.SaveWord(_englishWordSource, wordData);
            }
            //List<string> specialChars=new List<string>(){ "，","）","（","；" };
            //var wordInfos = _englishWordSource.GetAllWords();
            //int index = 0;
            //foreach (var wordInfo in wordInfos)
            //{
            //    ProgressTextBlock.Text = $"{++index}.{wordInfo.Word}";
            //    if (wordInfo.Synonym!=null && specialChars.Any(i=>wordInfo.Synonym.Contains(i)))
            //    {
            //        wordInfo.Synonym = wordInfo.Synonym.Replace("，", ",")
            //            .Replace("（", "(")
            //            .Replace("）", ")")
            //            .Replace("；",";");
            //    }
            //    if (wordInfo.Phrase != null && specialChars.Any(i => wordInfo.Phrase.Contains(i)))
            //    {
            //        wordInfo.Phrase = wordInfo.Phrase.Replace("，", ",")
            //            .Replace("（", "(")
            //            .Replace("）", ")")
            //            .Replace("；", ";");
            //    }
            //    if (wordInfo.Sentence != null && specialChars.Any(i => wordInfo.Sentence.Contains(i)))
            //    {
            //        wordInfo.Sentence = wordInfo.Sentence.Replace("，", ",")
            //            .Replace("（", "(")
            //            .Replace("）", ")")
            //            .Replace("；", ";");
            //    }
            //    if (wordInfo.Semantic != null && specialChars.Any(i => wordInfo.Semantic.Contains(i)))
            //    {
            //        wordInfo.Semantic = wordInfo.Semantic.Replace("，", ",")
            //            .Replace("（", "(")
            //            .Replace("）", ")")
            //            .Replace("；", ";");
            //    }
            //    _englishWordSource.UpdateWordInfo(wordInfo);
        }

        private void TransferCurrentWords()
        {
            var wordInfos = _englishWordSource.GetAllWords();
            foreach (var wordInfo in wordInfos)
            {
                if (wordInfo.Synonym == "[]")
                {
                    wordInfo.Synonym = string.Empty;
                }
                //_englishWordOutput.SaveWord(wordInfo);
            }
        }

        private async Task AddNewWords()
        {
            var words = GetRequestingWords();
            int index = 0;
            foreach (var word in words)
            {
                ProgressTextBlock.Text = $"{++index}.{word}";
                //延时获取，否则获取次数过多会被禁用
                await Task.Delay(TimeSpan.FromSeconds(1));
                var wordData = await SearchWordAsync(word);

                //EnglishWordExportService.SaveWord(_englishWordSource, _englishWordOutput, wordData);
            }
        }

        private void SaveOutputFiles()
        {
            var outputFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "EnglishDict");
            Directory.CreateDirectory(outputFolder);
            File.Delete(Path.Combine(outputFolder, "EnglishDict.db3"));
            File.Copy(CustomPathUtil.DbSourcePath, Path.Combine(outputFolder, "EnglishDict.db3"));

            var wordFolder = CustomPathUtil.WordAudioFolder;
            var sentenceFolder = CustomPathUtil.SentenceAudioFolder;

            var outputWordFolder = Path.Combine(outputFolder, "Words");
            var outputSentenceFolder = Path.Combine(outputFolder, "Sentences");
            if (Directory.Exists(outputWordFolder))
            {
                Directory.Delete(outputWordFolder, true);
            }
            Directory.CreateDirectory(outputWordFolder);
            if (Directory.Exists(outputSentenceFolder))
            {
                Directory.Delete(outputSentenceFolder, true);
            }
            Directory.CreateDirectory(outputSentenceFolder);

            Directory.Move(wordFolder, outputWordFolder);
            Directory.Move(sentenceFolder, outputSentenceFolder);
        }

        /// <summary>
        /// 搜索单词
        /// </summary>
        /// <param name="searchingText"></param>
        /// <param name="selectedApiType"></param>
        /// <returns></returns>
        private async Task<EnglishWordTranslationData> SearchWordAsync(string searchingText, string selectedApiType = "")
        {
            var wordData = new EnglishWordTranslationData();
            if (!string.IsNullOrWhiteSpace(searchingText))
            {
                switch (selectedApiType)
                {
                    case "有道":
                        {
                            wordData = await YouDaoUnOfficialWordApiService.GetWordsAsync(searchingText);
                        }
                        break;
                    case "金山":
                        {
                            wordData = await KinsoftUnOfficialApiService.GetWordsAsync(searchingText);
                        }
                        break;
                    default:
                        {
                            var wordDataYoudao = await YouDaoUnOfficialWordApiService.GetWordsAsync(searchingText);
                            var wordDataKinsoft = await KinsoftUnOfficialApiService.GetWordsAsync(searchingText);
                            wordData.Word = wordDataKinsoft.Word;
                            wordData.UsPronounce = string.IsNullOrEmpty(wordDataKinsoft.UsPronounce.Pronounce) ? wordDataYoudao.UsPronounce : wordDataKinsoft.UsPronounce;
                            wordData.UkPronounce = string.IsNullOrEmpty(wordDataKinsoft.UkPronounce.Pronounce) ? wordDataYoudao.UkPronounce : wordDataKinsoft.UkPronounce;
                            //wordData.DetailJson = wordDataKinsoft.DetailJson + "\r\n" + wordDataYoudao.DetailJson;

                            wordData.Translations = wordDataKinsoft.Translations.Count > 0 ? wordDataKinsoft.Translations : wordDataYoudao.Translations;
                            wordData.Phrases = wordDataKinsoft.Phrases.Count > 0 ? wordDataKinsoft.Phrases : wordDataYoudao.Phrases;
                            wordData.Sentences = wordDataKinsoft.Sentences.Count > 0 ? wordDataKinsoft.Sentences : wordDataYoudao.Sentences;
                            wordData.Synonyms = wordDataYoudao.Synonyms.Count > 0 ? wordDataYoudao.Synonyms : wordDataKinsoft.Synonyms;
                            wordData.Cognates = wordDataYoudao.Cognates.Count > 0 ? wordDataYoudao.Cognates : wordDataKinsoft.Cognates;
                        }
                        break;
                }
            }

            return wordData;
        }

        private List<string> GetRequestingWords()
        {
            var wordList = GetWordList();
            //var currentWords = GetCurrentWords();
            //var allwords = wordList.Where(i => currentWords.All(j => j != i) && !string.IsNullOrEmpty(i)).ToList();
            return wordList;
        }
        private List<string> GetWordList()
        {
            //string executeFolder = AppDomain.CurrentDomain.BaseDirectory;
            //string wordsTxtFilePath = Path.Combine(executeFolder, @"Resources\words.txt");
            //var allLines = File.ReadAllLines(wordsTxtFilePath).Select(i => i.Trim()).ToList();
            var words = new List<string>();
            //foreach (var word in allLines)
            //{
            //    if (words.All(i => i != word))
            //    {
            //        words.Add(word);
            //    }
            //}
            words.Add("longer");
            words.Add("Mars");
            return words;
        }
        private List<string> GetCurrentWords()
        {
            var wordInfos = _englishWordSource.GetAllWords();
            var list = new List<string>();
            
            foreach (var wordInfo in wordInfos)
            {
                //补缺音标音频
                if (string.IsNullOrEmpty(wordInfo.Pronounce) && string.IsNullOrEmpty(wordInfo.PronounceFileName) && string.IsNullOrEmpty(wordInfo.Semantic))
                {
                    list.Add(wordInfo.Word);
                }
            }

            if (!list.Contains("erudite"))
            {
                list.Add("erudite");
            }

            return list;
        }

        private void MinimizeButton_OnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            Hide(sender, e);
        }
        private void Hide(object sender, EventArgs e)
        {
            this.ShowInTaskbar = false;
            this.Visibility = Visibility.Hidden;
        }

        private void HeaderGrid_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}
