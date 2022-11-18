using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Translation.WebApi.YouDaoApi
{
    /// <summary>
    /// 字典查询参数类
    /// </summary>
    [DataContract]
    public class DictionaryQueryModel
    {
        [DataMember(Name = "count")]
        public int Count { get; set; } = 99;

        [DataMember(Name = "dicts")]
        public List<List<string>> Dictionaries { get; set; }
    }
}
