using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationTool.ViewModels
{
    public class SynonymModel : ViewModelBase
    {
        private string _synonyms;
        public string Synonyms
        {
            get => _synonyms;
            set
            {
                _synonyms = value;
                OnPropertyChanged();
            }
        }
        private string _translation;
        public string Translation
        {
            get => _translation;
            set
            {
                _translation = value;
                OnPropertyChanged();
            }
        }
    }
}
