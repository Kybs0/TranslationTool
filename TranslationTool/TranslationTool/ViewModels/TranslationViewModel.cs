using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Translation.WebApi.BaiduApi.Translation;
using Translation.WebApi.GooleApi.BrokenTranslation;
using Translation.WebApi.KinsoftApi.Translate;
using Translation.WebApi.YouDaoApi;

namespace TranslationTool.ViewModels
{
    public class TranslationViewModel : ViewModelBase
    {
        public TranslationViewModel()
        {
            SearchCommand = new DelegateCommand(Search_OnExecute);
        }

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
            var searchingText = (SearchingText ?? string.Empty).Trim();
            var translation = string.Empty;
            if (!string.IsNullOrEmpty(searchingText))
            {
                switch (SelectedApiType)
                {
                    case "金山":
                        {
                            translation = await KinsoftUnOfficialTranslationApiService.GetTranslationAsync(searchingText);
                        }
                        break;
                    case "有道":
                        {
                            var translationResponse = await YouDaoOfficialApiService.GetWordsAsync(searchingText);
                            var translations = translationResponse.FirstTranslation ?? new List<string>();
                            translation = string.Join(";", translations);
                        }
                        break;
                    case "谷歌":
                        {
                            translation = await GooleBrokenTranslationService.GetTranslationAsync(searchingText);
                        }
                        break;
                    case "百度":
                        {
                            translation = await BaiduOfficialTranslationApiService.GetTranslationAsync(searchingText);
                        }
                        break;
                }

            }

            Translation = translation;
        }

        private string _translation;
        /// <summary>
        /// 翻译
        /// </summary>
        public string Translation
        {
            get => _translation;
            set
            {
                _translation = value;
                OnPropertyChanged();
            }
        }

        #region API类型

        public List<string> ApiTypeList { get; set; } = new List<string>()
        {
           "有道","百度"
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
}
