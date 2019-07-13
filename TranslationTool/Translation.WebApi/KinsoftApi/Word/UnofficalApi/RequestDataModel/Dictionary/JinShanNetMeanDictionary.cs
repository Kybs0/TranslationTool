using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Translation.Api;

namespace Translation.WebApi.KinsoftApi
{
    [DataContract]
    public class KinsoftNetMeanInfoDictionary
    {
        [DataMember(Name = "RelatedPhrase")]
        public List<KinsoftRelatedPhrase> KinsoftRelatedPhrases { get; set; }

        /// <summary>
        /// 获取同根词列表
        /// </summary>
        /// <returns></returns>
        public List<CognateInfo> GetCognateWords()
        {
            var cognateInfos = new List<CognateInfo>();
            if (KinsoftRelatedPhrases != null)
            {
                foreach (var cognateWordData in KinsoftRelatedPhrases)
                {
                    if (cognateWordData.KinsoftCognateTranslations != null)
                    {
                        var cognate = cognateWordData.Cognate;
                        var list = cognateWordData.KinsoftCognateTranslations?.Select(i=>i.Explanation).ToList()??new List<string>();
                        cognateInfos.Add(new CognateInfo()
                        {
                            Cognate = cognate,
                            Translation = string.Join(";", list),
                        });
                    }
                }
            }

            return cognateInfos;
        }
    }
    [DataContract]
    public class KinsoftRelatedPhrase
    {
        /// <summary>
        /// 同根词
        /// </summary>
        [DataMember(Name = "word")]
        public string Cognate { get; set; }

        [DataMember(Name = "list")]
        public List<KinsoftCognateTranslation> KinsoftCognateTranslations { get; set; }
    }
    [DataContract]
    public class KinsoftCognateTranslation
    {
        [DataMember(Name = "exp")]
        public string Explanation { get; set; }
    }
}
