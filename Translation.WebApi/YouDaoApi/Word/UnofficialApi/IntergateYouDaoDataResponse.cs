using System.Runtime.Serialization;

namespace Translation.WebApi.YouDaoApi
{
    [DataContract]
    public class IntergateYouDaoDataResponse
    {
        [DataMember(Name = "input")]
        public string Word { get; set; }

        [DataMember(Name = "ec")]
        public EnglishToChineseDictionary EnglishToChineseData { get; set; }

        /// <summary>
        /// 21世纪大英汉字典
        /// </summary>
        [DataMember(Name = "ec21")]
        public EnglishToChinese21CentryDictionary EnglishToChinese21CentryDictionary { get; set; }

        /// <summary>
        /// 词组短语字典
        /// </summary>
        [DataMember(Name = "phrs")]
        public PhraseDictionary PhraseDictionary { get; set; }

        /// <summary>
        /// 同根词字典
        /// </summary>
        [DataMember(Name = "rel_word")]
        public CognateWordDictionary CognateWordDictionary { get; set; }

        /// <summary>
        /// 近义词字典
        /// </summary>
        [DataMember(Name = "syno")]
        public SynonymDictionary SynonymDictionary { get; set; }

        /// <summary>
        /// 近义词字典
        /// </summary>
        [DataMember(Name = "collins")]
        public CollinsDictionary CollinsDictionary { get; set; }
    }

}
