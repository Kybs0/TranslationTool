using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translation.WebApi.AudioApi
{
    /// <summary>
    /// 音频直接下载接口
    /// </summary>
    public class WordAudioFileService
    {
        public static string GetYouDaoAudioDownloadUrl(string word,PronounceType pronounceType)
        {
            switch (pronounceType)
            {
                case PronounceType.Us:
                    return $"http://dict.youdao.com/dictvoice?type=0&audio={word}";
                default:
                    return $"http://dict.youdao.com/dictvoice?type=1&audio={word}";
            }
        }
        public static string  GetShanBeiAudioDownloadUrl(string word,PronounceType pronounceType)
        {
            switch (pronounceType)
            {
                case PronounceType.Us:
                    return $"http://media.shanbay.com/audio/us/{word}.mp3";
                default:
                    return $"http://media.shanbay.com/audio/uk/{word}.mp3";
            }
        }
    }

    public enum PronounceType
    {
        Us,
        Uk
    }
}
