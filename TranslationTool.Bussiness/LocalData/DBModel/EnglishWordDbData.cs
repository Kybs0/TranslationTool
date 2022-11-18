using System.Collections.Generic;

namespace Translation.Business
{
    /// <summary>
    /// 单词信息-数据库解析数据
    /// </summary>
    public class EnglishWordDbData
    {
        /// <summary>
        /// 单词
        /// </summary>
        public string Word { get; set; }
        /// <summary>
        /// 单词音标
        /// </summary>
        public string Pronounce { get; set; }
        /// <summary>
        /// 单词发音文件
        /// </summary>
        public string PronounceFileName { get; set; }
        /// <summary>
        /// 释义
        /// </summary>
        public List<SematicDbData> Semantic { get; set; }
        /// <summary>
        /// 例句
        /// </summary>
        public List<SentenceDbData> Sentence { get; set; }
        /// <summary>
        /// 词组
        /// </summary>
        public List<PhraseDbData> Phrase { get; set; }
        /// <summary>
        /// 同义词
        /// </summary>
        public List<SynonymDbData> Synonym { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        public string FileKey { get; set; }
    }
}
