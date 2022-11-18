using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider;

namespace Translation.Business
{
    /// <summary>
    /// 英汉字典数据
    /// </summary>
    public partial class EnglishDictDB : DataConnection
    {
        public ITable<EnglishWordDbModel> Words => GetTable<EnglishWordDbModel>();

        public EnglishDictDB()
        {
            InitDataContext();
        }

        public EnglishDictDB(string configuration)
            : base(configuration)
        {
            InitDataContext();
        }

        public EnglishDictDB(IDataProvider dataProvider, string connectionString)
            : base(dataProvider, connectionString)
        {
            InitDataContext();
        }

        partial void InitDataContext();
    }
}
