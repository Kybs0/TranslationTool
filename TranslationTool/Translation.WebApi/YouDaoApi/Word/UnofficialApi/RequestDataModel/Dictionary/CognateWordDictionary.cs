using System.Collections.Generic;
using System.Runtime.Serialization;
using Translation.Api;

namespace Translation.WebApi.YouDaoApi
{
    /// <summary>
    /// 同根词
    /// </summary>
    [DataContract]
    public class CognateWordDictionary
    {
        [DataMember(Name = "rels")]
        public List<CognateWordRel> CognateWordRels { get; set; }

        /// <summary>
        /// 获取同根词列表
        /// </summary>
        /// <returns></returns>
        public List<CognateInfo> GetCognateWords()
        {
            var cognateInfos = new List<CognateInfo>();
            if (CognateWordRels != null)
            {
                foreach (var cognateWordRel in CognateWordRels)
                {
                    var cognateWordData = cognateWordRel.CognateWordData;
                    if (cognateWordData != null)
                    {
                        var cognateType = cognateWordData.CognateType;
                        foreach (var cognateWordDataDetai in cognateWordData.CognateWordDataDetails)
                        {
                            cognateInfos.Add(new CognateInfo()
                            {
                                Cognate = cognateWordDataDetai.Word,
                                Translation = cognateWordDataDetai.Translation,
                                WordType = cognateType
                            });
                        }
                    }
                }
            }

            return cognateInfos;
        }
    }
    [DataContract]
    public class CognateWordRel
    {
        [DataMember(Name = "rel")]
        public CognateWordData CognateWordData { get; set; }
    }
    [DataContract]
    public class CognateWordData
    {
        [DataMember(Name = "pos")]
        public string CognateType { get; set; }
        [DataMember(Name = "words")]
        public List<CognateWordDataDetail> CognateWordDataDetails { get; set; }
    }
    [DataContract]
    public class CognateWordDataDetail
    {
        [DataMember(Name = "tran")]
        public string Translation { get; set; }
        [DataMember(Name = "word")]
        public string Word { get; set; }
    }
}
