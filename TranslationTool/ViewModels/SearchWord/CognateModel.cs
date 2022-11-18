using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationTool.ViewModels
{
    public class CognateModel : ViewModelBase
    {
        private string _cognate;
        public string Cognate
        {
            get => _cognate;
            set
            {
                _cognate = value;
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
