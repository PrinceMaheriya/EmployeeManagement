using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WpfApp1.Helper
{
   public static class Common
    {

        //gender
        public const string male = "male";
        public const string female = "female";

        //status
        public const string inactive = "inactive";
        public const string active = "active";


        private static readonly Regex _numRegex =  new Regex("[^0-9]");

        //allow only number
        public static bool IsTextAllowed(string text)
        {
            return _numRegex.IsMatch(text);
        }

    }
}
