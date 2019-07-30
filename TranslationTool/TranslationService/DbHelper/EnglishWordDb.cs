using LinqToDB.Data;
using LinqToDB.DataProvider;
using LinqToDB.DataProvider.SQLite;
using LinqToDB.Mapping;

namespace Translation.Business
{
    public partial class EnglishWordDb : DataConnection
    {
        public LinqToDB.ITable<WordInfo> EnglishWords => GetTable<WordInfo>();

        public EnglishWordDb()
        {
            InitDataContext();
        }

        public EnglishWordDb(string configuration)
            : base(configuration)
        {
            InitDataContext();
        }

        public EnglishWordDb(SQLiteDataProvider dataProvider, string connectionString)
            : base(dataProvider, connectionString)
        {
            InitDataContext();
        }

        partial void InitDataContext();
    }
    /// <summary>
    /// 汉字
    /// </summary>
    [Table("Words")]
    public class WordInfo
    {
        [Column("Word"),PrimaryKey,NotNull]
        public string Word { get; set; } // varchar(32)
        [Column(), Nullable]
        public string Pronounce { get; set; } // varchar(32)

        /// <summary>
        /// 释义
        /// [{"Semantic":"朝鲜人,朝鲜国民;朝鲜语","PartOfSpeech":"n."},
        /// {"Semantic":"朝鲜的;朝鲜人的;韩国(人)的;韩国(语)的","PartOfSpeech":"adj."}]
        /// </summary>
        [Column(), Nullable]
        public string Semantic { get; set; } // text(max)
        [Column(), Nullable]
        public string Sentence { get; set; } // text(max)
        [Column(), Nullable]
        public string PronounceFileName { get; set; } // text(max)
        [Column(), Nullable]
        public string Remark { get; set; } // text(max)

        /// <summary>
        /// 词组
        /// </summary>
        [Column, Nullable]
        public string Phrase { get; set; }

        /// <summary>
        /// 近义词
        /// </summary>
        [Column(), Nullable]
        public string Synonym { get; set; }
        /// <summary>
        /// 近义词
        /// </summary>
        [Column(), Nullable]
        public string Cognate { get; set; }
    }
}
