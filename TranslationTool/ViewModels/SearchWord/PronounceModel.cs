using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Translation.Api;
using Translation.WebApi;
using Translation.WebApi.AudioApi;

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
        /// <summary>
        /// 发音数据转换（包含下载音频文件）
        /// </summary>
        /// <param name="pronounceInfo"></param>
        /// <param name="pronounceType">0:美式，1：英式</param>
        /// <param name="word"></param>
        /// <returns></returns>
        public static PronounceModel ConvertFrom(PronounceInfo pronounceInfo, PronounceType pronounceType, string word)
        {
            var pronounceModel = new PronounceModel();
            pronounceModel.Pronounce = pronounceInfo.Pronounce;
            if (!string.IsNullOrWhiteSpace(word)&&!string.IsNullOrWhiteSpace(pronounceInfo.Pronounce))
            {
                Task.Run(() =>
                {
                    var downloadSuccess = WebResourceDownloadHelper.Download(pronounceInfo.PronounceUri, out var downloadPath);
                    //从以下资源路径补充单词音频文件
                    if (!downloadSuccess)
                    {
                        var youDaoAudioDownloadUrl = WordAudioFileService.GetYouDaoAudioDownloadUrl(word, pronounceType);
                        downloadSuccess = WebResourceDownloadHelper.Download(youDaoAudioDownloadUrl, out downloadPath);
                    }
                    if (!downloadSuccess)
                    {
                        var shanBeiAudioDownloadUrl = WordAudioFileService.GetShanBeiAudioDownloadUrl(word, pronounceType);
                        downloadSuccess = WebResourceDownloadHelper.Download(shanBeiAudioDownloadUrl, out downloadPath);
                    }

                    if (downloadSuccess && File.Exists(downloadPath))
                    {
                        pronounceModel.PronounceUri = downloadPath;
                    }
                });
            }
            
            return pronounceModel;
        }
    }
}
