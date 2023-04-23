using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KIP_Translator.Model
{
    public class History
    {
        public int Id { get; set; }
        public string TranslateSource { get; set; }
        public string TranslateTarget { get; set; }
        public DateTime Date { get; set; }
        public int IdLangIn { get; set; }
        public int IdLangOut { get; set; }

        public Lang LangIn { get; set; }
        public Lang LangOut { get; set; }
    }
}
