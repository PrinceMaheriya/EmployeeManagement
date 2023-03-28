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

        /// <summary>
        /// This method is used to checck wheather the given text is number or not 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsTextAllowed(string text)
        {
            return _numRegex.IsMatch(text);
        }

        /// <summary>
        /// This function is used to validate the email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsValidEmail(string email)
        {
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(email);
        }

    }
}
