using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SqlSugar;
using Translation.Api;
using Translation.Util;

namespace Translation.Business
{
    public static class LocalWordService
    {
        private static readonly EnglishDictDbReader _dictDbReader = new EnglishDictDbReader();
        public static async Task<EnglishWordTranslationData> GetWordsAsync(string queryText)
        {
            var dbPath = CustomPathUtil.WordsFile;
            ConnectionConfig connectionConfig = new ConnectionConfig();
            connectionConfig.DbType = DbType.Sqlite;
            connectionConfig.ConnectionString = $"Data Source={dbPath};";
            using (SqlSugarClient sugarClient = new SqlSugarClient(connectionConfig))
            {
                var sugarQueryable = sugarClient.Queryable<EnglishWordDbModel>().ToList();
            }
            
            var dbData = await _dictDbReader.GetWordAsync(queryText);
            if (dbData == null)
            {
                return new EnglishWordTranslationData();
            }
            var translationData = new EnglishWordTranslationData()
            {
                Word = dbData.Word,
            };

            translationData.Translations = dbData.Semantic.Select(i => new SematicInfo()
            {
                WordType = i.WordType,
                Translation = i.Semantic
            }).ToList();

            var phraseInfos = dbData.Phrase.Select(i => new PhraseInfo()
            {
                Phrase = i.Phrase,
                PhraseTranslation = i.Translation
            }).ToList();
            translationData.Phrases = phraseInfos;

            var synonymInfos = dbData.Synonym.Select(i => new SynonymInfo()
            {
                WordType = i.WordType,
                Synonyms = i.Synonyms,
                Translation = i.Translation
            }).ToList();
            translationData.Synonyms = synonymInfos;

            var sentences = new List<SentenceInfo>();
            dbData.Sentence?.ForEach(i => sentences.Add(new SentenceInfo()
            {
                Translation = i.Translation,
                Sentence = i.Sentence,
                EnglishSentenceUri = i.SentenceFileName
            }));
            translationData.Sentences = sentences;
            return translationData;
        }
    }

}
