using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Translation.Api;
using Translation.WebApi;

namespace TranslationTool.ViewModels
{
    public class PronounceModel : ViewModelBase
    {
        private string _pronounce;
        /// <summary>
        /// 发音
        /// </summary>
        public string Pronounce
        {
            get => _pronounce;
            set
            {
                _pronounce = value;
                OnPropertyChanged();
            }
        }
        private string _pronounceUri;
        /// <summary>
        /// 发音音频地址(本地)
        /// </summary>
        public string PronounceUri
        {
            get => _pronounceUri;
            set
            {
                _pronounceUri = value;
                OnPropertyChanged();
            }
        }
        public static PronounceModel ConvertFrom(PronounceInfo pronounceInfo)
        {
            var pronounceModel = new PronounceModel();
            pronounceModel.Pronounce = pronounceInfo.Pronounce;
            Task.Run(() =>
            {
                var downloadSuccess =
                    WebResourceDownloadHelper.Download(pronounceInfo.PronounceUri, out var downloadPath);
                if (downloadSuccess)
                {
                    pronounceModel.PronounceUri = downloadPath;
                }
            });
            return pronounceModel;
        }
    }

    public static class PronounceModelExtension
    {
        public static void AAA()
        {

        }
    }
}
