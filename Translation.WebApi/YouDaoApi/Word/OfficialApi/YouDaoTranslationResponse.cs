using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Translation.WebApi.YouDaoApi
{
    [DataContract]
    public class YouDaoTranslationResponse
    {
        [DataMember(Name = "errorCode")]
        public string ErrorCode { get; set; }

        [DataMember(Name = "query")]
        public string QueryText { get; set; }

        [DataMember(Name = "speakUrl")]
        public string InputSpeakUrl { get; set; }

        [DataMember(Name = "tSpeakUrl")]
        public string TranslationSpeakUrl { get; set; }

        /// <summary>
        /// 首选翻译
        /// </summary>
        [DataMember(Name = "translation")]
        public List<string> FirstTranslation { get; set; }

        /// <summary>
        /// 基本释义
        /// </summary>
        [DataMember(Name = "basic")]
        public TranslationBasicData BasicTranslation { get; set; }

        ///// <summary>
        ///// 网络释义，该结果不一定存在
        ///// </summary>
        //[DataMember(Name = "web")]
        //public string WebTranslation { get; set; }
    }

    /// <summary>
    /// 基本释义
    /// </summary>
    [DataContract]
    public class TranslationBasicData
    {
        [DataMember(Name = "phonetic")]
        public string Phonetic { get; set; }

        /// <summary>
        /// 英式发音
        /// </summary>
        [DataMember(Name = "uk-phonetic")]
        public string UkPhonetic { get; set; }

        /// <summary>
        /// 美式发音
        /// </summary>
        [DataMember(Name = "us-phonetic")]
        public string UsPhonetic { get; set; }

        /// <summary>
        /// 翻译
        /// </summary>
        [DataMember(Name = "explains")]
        public List<string> Explains { get; set; }
    }

    /// <summary>
    /// 网络释义
    /// </summary>
    [DataContract]
    public class TranslationWebData
    {
        [DataMember(Name = "key")]
        public string Key { get; set; }

        [DataMember(Name = "value")]
        public List<string> Explains { get; set; }
    }
}
