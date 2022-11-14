using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Translation.Business
{
    /// <summary>
    /// 英汉字典本地数据
    /// </summary>
    public class EnglishDictDbReader
    {
        #region 构造函数与初始化

        private readonly DbCache<EnglishDictDB> _innerCache;
        private List<EnglishWordDbData> _wordInfosCache;

        /// <summary>
        /// 单词信息
        /// </summary>
        public List<EnglishWordDbData> Words => _wordInfosCache ?? (_wordInfosCache = GetWords());

        public EnglishDictDbReader()
        {
            var dbPath = CustomPathUtil.WordsFile;

            _innerCache = new DbCache<EnglishDictDB>(() => $"Data Source={dbPath}");
        }

        #endregion

        #region 单词数据

        /// <summary>
        /// 获取所有单词
        /// </summary>
        /// <returns></returns>
        private List<EnglishWordDbData> GetWords()
        {
            List<EnglishWordDbModel> wordList = _innerCache.Execute(db => db.Words.ToList());
            if (wordList == null)
                throw new InvalidOperationException("EnglishDictionary Words is null!");

            var wordInfosCache = wordList.Select(wordInfo => new EnglishWordDbData()
            {
                Word = wordInfo.Word,
                Pronounce = wordInfo.Pronounce,
                //兼容补偿PronounceFileName
                PronounceFileName = !string.IsNullOrEmpty(wordInfo.PronounceFileName) ?
                    wordInfo.PronounceFileName : GetPronounceFileNameByFileKey(wordInfo.FileKey),
                Semantic = !string.IsNullOrEmpty(wordInfo.Semantic)
                    ? JsonConvert.DeserializeObject<List<SematicDbData>>(wordInfo.Semantic)
                    : new List<SematicDbData>(),
                Sentence = !string.IsNullOrEmpty(wordInfo.Sentence)
                    ? JsonConvert.DeserializeObject<List<SentenceDbData>>(wordInfo.Sentence)
                    : new List<SentenceDbData>(),
                Phrase = !string.IsNullOrEmpty(wordInfo.Phrase)
                    ? JsonConvert.DeserializeObject<List<PhraseDbData>>(wordInfo.Phrase)
                    : new List<PhraseDbData>(),
                Synonym = !string.IsNullOrEmpty(wordInfo.Synonym)
                    ? JsonConvert.DeserializeObject<List<SynonymDbData>>(wordInfo.Synonym)
                    : new List<SynonymDbData>(),
                Remark = wordInfo.Remark,
                FileKey = wordInfo.FileKey
            }).ToList();

            return wordInfosCache;
        }
        private static string GetPronounceFileNameByFileKey(string fileKey)
        {
            if (string.IsNullOrEmpty(fileKey))
            {
                return string.Empty;
            }
            var guidInFileKey = fileKey.Substring(fileKey.IndexOf("/", StringComparison.Ordinal) + 1);
            var wordAudioByFileKey = $"{guidInFileKey}.mp3";
            return wordAudioByFileKey;
        }

        /// <summary>
        /// 获取单个单词
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public async Task<EnglishWordDbData> GetWordAsync(string word)
        {
            var englishWordDbData =await Task.Run(() =>
            {
               return Words.FirstOrDefault(i => i.Word == word);
            });
            return englishWordDbData;
        }

        #endregion
    }
}
