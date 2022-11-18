using LinqToDB.Mapping;

namespace Translation.Business
{
    /// <summary>
    /// 英汉字典数据库映射Model
    /// </summary>
    [Table("Words")]
    public class EnglishWordDbModel
    {
        /// <summary>
        /// 单词
        /// </summary>
        [Column("Word"), NotNull]
        public string Word { get; set; }

        /// <summary>
        /// 单词音标
        /// </summary>
        [Column("Pronounce"), Nullable]
        public string Pronounce { get; set; }

        /// <summary>
        /// 单词发音文件
        /// eg:"003d9103-22a5-4a7d-8a2c-354ccda42990.mp3"
        /// </summary>
        [Column("PronounceFileName"), Nullable]
        public string PronounceFileName { get; set; }

        /// <summary>
        /// 释义
        /// eg：[{"Semantic":"账，账目;存款;记述，报告;理由","PartOfSpeech":"n."}]
        /// </summary>
        [Column("Semantic"), Nullable]
        public string Semantic { get; set; }

        /// <summary>
        /// 例句
        /// eg：[{"Sentence":"You have to take capital appreciation of the property into account.","SentenceFileName":"003d9103-22a5-4a7d-8a2c-354ccda42990.mp3"
        /// ,"Translation":"你必须将该处房产的资本增值考虑在内。"}]
        /// </summary>
        [Column("Sentence"), Nullable]
        public string Sentence { get; set; }

        /// <summary>
        /// 词组
        /// eg：[{"Phrase":"semantic analysis","Translation":"语义分析"}]
        /// </summary>
        [Column("Phrase"), Nullable]
        public string Phrase { get; set; }

        /// <summary>
        /// 同义词
        /// eg：[{"WordType":"n.","Translation":"[会计]帐户；解释；帐目，帐单；理由","Synonyms":["interpretation","explanation"]}]
        /// </summary>
        [Column("Synonym"), Nullable]
        public string Synonym { get; set; }

        ///// <summary>
        ///// 同根词
        ///// 注：当前版本未使用
        ///// </summary>
        //[Column("Cognate"), Nullable]
        //public string Cognate { get; set; }

        ///// <summary>
        ///// 扩展词组
        ///// 注：当前版本未使用
        ///// </summary>
        //[Column("ExternalPhrase"), Nullable]
        //public string ExternalPhrase { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("Remark"), Nullable]
        public string Remark { get; set; }

        [Column("fileKey"), Nullable]
        public string FileKey { get; set; }
    }
}
