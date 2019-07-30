using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB;

namespace TranslationExportTool
{
    public class EnglishWordSqliteHelper
    {
        private readonly string DbName = "EnglishDict.db3";
        private readonly DbCache<EnglishWordDb> _englishWordCache;
        private List<WordInfo> _englishWords = new List<WordInfo>();

        public EnglishWordSqliteHelper(string dbPath)
        {
            _englishWordCache = new DbCache<EnglishWordDb>($"Data Source={dbPath}");
            GetAllWords();
        }

        public void SaveWord(WordInfo wordInfo)
        {
            _englishWordCache.ExecuteTransaction(englishWordDb =>
            {
                if (_englishWords.All(i => i.Word != wordInfo.Word))
                {
                    _englishWords.Add(wordInfo);
                    englishWordDb.Insert(wordInfo);
                }
            });
        }

        public void UpdateWordInfo(WordInfo wordInfo)
        {
            _englishWordCache.ExecuteTransaction(englishWordDb => { englishWordDb.Update(wordInfo); });
        }

        public List<WordInfo> GetAllWords()
        {
            if (_englishWords.Count == 0)
            {
                _englishWords = _englishWordCache.Execute(db => db.EnglishWords).ToList();
            }
            return _englishWords;
        }

        public WordInfo GetWord(string word)
        {
            var words = _englishWords;
            if (words == null || words.Count == 0)
            {
                words = _englishWords = GetAllWords();
            }

            return words.FirstOrDefault(i => i.Word == word);
        }

        public void DeleteWord(WordInfo word)
        {
            _englishWordCache.ExecuteTransaction(englishWordDb =>
            {
                if (_englishWords.Any(i => i.Word == word.Word))
                {
                    _englishWords.Remove(_englishWords.First(i => i.Word == word.Word));
                    englishWordDb.Delete(word);
                }
            });
        }
    }
}
