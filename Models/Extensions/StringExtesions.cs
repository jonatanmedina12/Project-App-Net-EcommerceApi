using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Extensions
{
    public  static class StringExtesions
    {
        
            public static string NormalizeText(this string text)
            {
                if (string.IsNullOrEmpty(text)) return text;
                return text.Trim().ToLower();
            }
        
    }
}
