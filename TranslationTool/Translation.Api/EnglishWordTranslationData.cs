using System.Collections.Generic;

namespace Translation.Api
{
    /// <summary>
    /// 英语单词查询数据
    /// </summary>
    public class EnglishWordTranslationData
    {
        /// <summary>
        /// 查询单词
        /// </summary>
        public string Word { get; set; }

        /// <summary>
        /// 详细Json内容
        /// </summary>
        public string DetailJson { get; set; }

        /// <summary>
        /// 英式发音
        /// </summary>
        public PronounceInfo UkPronounce { get; set; } = new PronounceInfo();

        /// <summary>
        /// 美式发音
        /// </summary>
        public PronounceInfo UsPronounce { get; set; } = new PronounceInfo();

        public WordExtraExchange WordExtraExchange { get; set; }

        /// <summary>
        /// 释义
        /// </summary>
        public List<SematicInfo> Translations { get; set; } = new List<SematicInfo>();

        /// <summary>
        /// 词组
        /// </summary>
        public List<PhraseInfo> Phrases { get; set; } = new List<PhraseInfo>();

        /// <summary>
        /// 扩展词组
        /// </summary>
        public List<ExternalPhraseInfo> ExternalPhrases { get; set; } = new List<ExternalPhraseInfo>();

        /// <summary>
        /// 例句
        /// </summary>
        public List<SentenceInfo> Sentences { get; set; } = new List<SentenceInfo>();

        /// <summary>
        /// 近义词
        /// </summary>
        public List<SynonymInfo> Synonyms { get; set; } = new List<SynonymInfo>();

        /// <summary>
        /// 同根词
        /// </summary>
        public List<CognateInfo> Cognates { get; set; } = new List<CognateInfo>();
    }
}
