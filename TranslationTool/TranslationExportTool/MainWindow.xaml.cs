using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Translation.Api;
using Translation.Business;
using Translation.Business.ExportService;
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
        private EnglishWordSqliteHelper _englishWordOutput;
        public MainWindow()
        {
            InitializeComponent();
            _englishWordSource = new EnglishWordSqliteHelper(CustomPathUtil.DbSourcePath);
            _englishWordOutput = new EnglishWordSqliteHelper(CustomPathUtil.DbOuputPath);
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

            TransferCurrentWords();
            await AddNewWords();
        }

        private void TransferCurrentWords()
        {
            var wordInfos = _englishWordSource.GetAllWords();
            foreach (var wordInfo in wordInfos)
            {
                _englishWordOutput.SaveWord(wordInfo);
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

                EnglishWordExportService.SaveWord(_englishWordSource, _englishWordOutput, wordData);
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
            var currentWords = GetCurrentWords();
            var allwords = wordList.Where(i => currentWords.All(j => j != i) && !string.IsNullOrEmpty(i)).ToList();
            return allwords;
        }
        private List<string> GetWordList()
        {
            string executeFolder = AppDomain.CurrentDomain.BaseDirectory;
            string wordsTxtFilePath = Path.Combine(executeFolder, @"Resources\words.txt");
            var allLines = File.ReadAllLines(wordsTxtFilePath).Select(i => i.Trim()).ToList();
            var words = new List<string>();
            foreach (var word in allLines)
            {
                if (words.All(i => i != word))
                {
                    words.Add(word);
                }
            }
            return words;
        }
        private List<string> GetCurrentWords()
        {
            var wordInfos = _englishWordSource.GetAllWords();
            var list = wordInfos.Select(i => i.Word.Trim()).ToList();
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
