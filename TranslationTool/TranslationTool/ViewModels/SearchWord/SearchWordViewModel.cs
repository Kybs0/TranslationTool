using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Translation.Api;
using Translation.WebApi;
using Translation.WebApi.AudioApi;
using Translation.WebApi.KinsoftApi;
using Translation.WebApi.YouDaoApi;
using TranslationTool.Annotations;
using TranslationTool.Helper;

namespace TranslationTool.ViewModels
{
    public class SearchWordViewModel : ViewModelBase
    {
        public SearchWordViewModel()
        {
            SearchCommand = new DelegateCommand(Search_OnExecute);
            SpeekCommand = new DelegateCommand<string>(Speek_OnExecute);
        }

        #region 发音

        public ICommand SpeekCommand { get; }
        private Mp3Player _mp3Player = null;

        private void Speek_OnExecute(string audioPath)
        {
            if (File.Exists(audioPath))
            {
                if (_mp3Player == null)
                {
                    _mp3Player = new Mp3Player();
                }
                else
                {
                    _mp3Player.Pause();

                }
                _mp3Player.FilePath = audioPath;
                _mp3Player.Play();
            }
        }

        #endregion

        #region 搜索

        private string _searchingText;
        public string SearchingText
        {
            get => _searchingText;
            set
            {
                _searchingText = value;
                OnPropertyChanged();
            }
        }

        public ICommand SearchCommand { get; }
        private async void Search_OnExecute()
        {
            var searchingText = SearchingText ?? string.Empty;
            await SearchWord(searchingText);
        }

        #region 搜索单词

        /// <summary>
        /// 搜索单词
        /// </summary>
        /// <param name="searchingText"></param>
        /// <returns></returns>
        private async Task SearchWord(string searchingText)
        {
            Application.Current.Dispatcher.Invoke(() => { IsSearching = true; });
            var wordData = new EnglishWordTranslationData();
            if (!string.IsNullOrWhiteSpace(searchingText))
            {
                switch (SelectedApiType)
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
                            var wordDataKinsoft = await KinsoftDictApiService.GetWordsAsync(searchingText);
                            wordData.Word = string.IsNullOrEmpty(wordDataKinsoft.Word)?wordDataYoudao.Word:wordDataKinsoft.Word;
                            wordData.UsPronounce = string.IsNullOrEmpty(wordDataKinsoft.UsPronounce?.Pronounce) ? wordDataYoudao.UsPronounce:wordDataKinsoft.UsPronounce;
                            wordData.UkPronounce = string.IsNullOrEmpty(wordDataKinsoft.UkPronounce?.Pronounce) ? wordDataYoudao.UkPronounce:wordDataKinsoft.UkPronounce;
                            wordData.DetailJson = wordDataYoudao.DetailJson + "\r\n" + wordDataKinsoft.DetailJson;

                            wordData.Translations = wordDataKinsoft.Translations.Count > 0 ?wordDataKinsoft.Translations:wordDataYoudao.Translations;
                            wordData.Phrases = wordDataYoudao.Phrases.Count > 0?wordDataYoudao.Phrases:wordDataKinsoft.Phrases;
                            wordData.Sentences = wordDataKinsoft.Sentences.Count > 0?wordDataKinsoft.Sentences:wordDataYoudao.Sentences;
                            wordData.Synonyms = wordDataYoudao.Synonyms.Count > 0 ? wordDataYoudao.Synonyms : wordDataKinsoft.Synonyms;
                            wordData.Cognates = wordDataKinsoft.Cognates.Count > 0 ? wordDataKinsoft.Cognates : wordDataYoudao.Cognates;
                        }
                        break;
                }
            }

            SetSearchedWordData(wordData);
            Application.Current.Dispatcher.Invoke(() => { IsSearching = false; });

        }


        private void SetSearchedWordData(EnglishWordTranslationData wordData)
        {
            var result = wordData;
            CurrentWord = result.Word;
            SearchResultDetail = result.DetailJson;
            SetTranslation(wordData);
            UsPronounce = PronounceModel.ConvertFrom(result.UsPronounce,PronounceType.Us,result.Word);
            UkPronounce = PronounceModel.ConvertFrom(result.UkPronounce,PronounceType.Uk,result.Word);
            SetPhrases(result.Phrases);
            SetSynonyms(result.Synonyms);
            SetCognates(result.Cognates);
            SetSentences(result.Sentences);

            //默认英式发音
            //Speek_OnExecute(UkPronounce?.PronounceUri ?? (UsPronounce?.PronounceUri));
        }

        private void SetCognates(List<CognateInfo> resultCognates)
        {
            var cognates = string.Empty;
            int index = 1;
            foreach (var resultCognate in resultCognates)
            {
                var chineseWordType = WordTypeHelper.ConvertToChinese(resultCognate.WordType);
                cognates += $"{index++}. {chineseWordType} {resultCognate.Cognate}\r\n";
                if (!string.IsNullOrEmpty(resultCognate.Translation))
                {
                    cognates += $"{resultCognate.Translation}\r\n";
                }
            }

            Cognates = cognates;
        }

        private void SetSynonyms(List<SynonymInfo> resultSynonyms)
        {
            var synonyms = string.Empty;
            int index = 1;
            foreach (var resultSynonym in resultSynonyms)
            {
                synonyms += $"{index++}. {resultSynonym.Translation}\r\n";
                synonyms += $"{string.Join("; ", resultSynonym.Synonyms)}\r\n";
            }
            Synonyms = synonyms;
        }

        private void SetPhrases(List<PhraseInfo> resultPhrases)
        {
            var _phrases = string.Empty;
            int index = 1;
            foreach (var sentenceInfo in resultPhrases)
            {
                _phrases += $"{index++}. {sentenceInfo.Phrase?.Replace("\t", string.Empty)}\r\n";
                _phrases += $"{sentenceInfo.PhraseTranslation?.Replace("\t", string.Empty)}\r\n";
            }
            Phrases = _phrases;
        }

        private void SetTranslation(EnglishWordTranslationData wordData)
        {
            var baseInfo = string.Empty;
            foreach (var wordDataTranslation in wordData.Translations)
            {
                var separator=string.IsNullOrWhiteSpace(wordDataTranslation.WordType) ? "" : " ";
                baseInfo += $"{wordDataTranslation.WordType?.Trim()}{separator}{ wordDataTranslation.Translation.Trim()}\r\n";
            }

            Translation = baseInfo;
        }
        private void SetSentences(List<SentenceInfo> sentenceInfos)
        {
            var sentences = new List<SentenceModel>();
            int index = 1;
            foreach (var sentenceInfo in sentenceInfos)
            {
                var sentenceModel = new SentenceModel()
                {
                    EnglishSentence = $"{index++}. {sentenceInfo.Sentence}",
                    ChineseSentence = sentenceInfo.Translation,
                };
                if (!string.IsNullOrWhiteSpace(sentenceInfo.EnglishSentenceUri))
                {
                    Task.Run(() =>
                    {
                        var downloadSuccess = WebResourceDownloadHelper.Download(sentenceInfo.EnglishSentenceUri, out string downloadPath);
                        if (downloadSuccess)
                        {
                            sentenceModel.EnglishSentenceUri = downloadPath;
                        }
                    });
                }
                sentences.Add(sentenceModel);
            }
            Sentences = sentences;
        }

        //官方查询API
        //private async void Search_OnExecute()
        //{
        //    var result = await YouDaoOfficialApiService.GetWordsAsync(SearchingText);

        //    Explaniation = result.YouDaoTranslation.BasicTranslation.Phonetic + "\r\n" +
        //                   string.Join("\r\n", result.YouDaoTranslation.BasicTranslation.Explains);
        //    SearchResultDetail = result.ResultDetail;
        //}

        #endregion

        #region 搜索状态

        public bool IsSearching
        {
            get => _isSearching;
            set
            {
                _isSearching = value;
                OnPropertyChanged();
            }
        }
        private bool _isSearching = false;

        #endregion

        #endregion

        #region 搜索结果

        private string _currentWord = string.Empty;
        public string CurrentWord
        {
            get => _currentWord;
            set
            {
                _currentWord = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// 英式发音
        /// </summary>
        private PronounceModel _ukPronounce = new PronounceModel();
        public PronounceModel UkPronounce
        {
            get => _ukPronounce;
            set
            {
                _ukPronounce = value;
                OnPropertyChanged();
            }
        }
        private PronounceModel _usPronounce = new PronounceModel();
        /// <summary>
        /// 美式发音
        /// </summary>
        public PronounceModel UsPronounce
        {
            get => _usPronounce;
            set
            {
                _usPronounce = value;
                OnPropertyChanged();
            }
        }

        private List<SentenceModel> _sentences;
        public List<SentenceModel> Sentences
        {
            get => _sentences;
            set
            {
                _sentences = value;
                OnPropertyChanged();
            }
        }

        private string _phrases;
        public string Phrases
        {
            get => string.IsNullOrEmpty(_phrases)?_phrases:_phrases.Trim().Trim("\r\n".ToCharArray());
            set
            {
                _phrases = value;
                OnPropertyChanged();
            }
        }
        private string _synonyms;
        public string Synonyms
        {
            get => string.IsNullOrEmpty(_synonyms)?_synonyms:_synonyms.Trim().Trim("\r\n".ToCharArray());
            set
            {
                _synonyms = value;
                OnPropertyChanged();
            }
        }
        private string _translation;
        public string Translation
        {
            get => string.IsNullOrEmpty(_translation)?_translation:_translation.Trim().Trim("\r\n".ToCharArray());
            set
            {
                _translation = value;
                OnPropertyChanged();
            }
        }

        private string _cognates;
        public string Cognates
        {
            get => string.IsNullOrEmpty(_cognates)?_cognates:_cognates.Trim().Trim("\r\n".ToCharArray());
            set
            {
                _cognates = value;
                OnPropertyChanged();
            }
        }
        private string _searchResultDetail;
        public string SearchResultDetail
        {
            get =>  string.IsNullOrEmpty(_searchResultDetail)?_searchResultDetail:_searchResultDetail.Trim().Trim("\r\n".ToCharArray());
            set
            {
                _searchResultDetail = value;
                OnPropertyChanged();
            }
        }

        private List<string> _translations;
        public List<string> Translations
        {
            get => _translations;
            set
            {
                _translations = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region API类型

        public List<string> ApiTypeList { get; set; } = new List<string>()
        {
            "默认","有道","金山"
        };

        private string _selectedApiType = string.Empty;

        public string SelectedApiType
        {
            get
            {
                if (string.IsNullOrEmpty(_selectedApiType))
                {
                    _selectedApiType = ApiTypeList[0];
                }

                return _selectedApiType;
            }
            set
            {
                _selectedApiType = value;
                OnPropertyChanged();
            }
        }



        #endregion

    }

    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
