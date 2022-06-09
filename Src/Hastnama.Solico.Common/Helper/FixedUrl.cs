using System.Collections.Generic;

namespace Hastnama.Solico.Common.Helper
{
    public static class FixedUrl
    {

        public static string RemoveSpecialChar(this string slug)
        {
            List<string> MyChar = new List<string>() { "+", ".", "…", "'", "/", "=", "”", "?", ",", "<", ">", "[", "]", "\\", "@", "#", "$", "%", "^", "&", "*", "(", ")", " ", ":" };
            foreach (var item in MyChar)
            {
                slug = slug.Replace(item, string.Empty);
            }
            return slug;
        }
        public static string ToEnglishNumber(this string input)
        {

            var englishnumbers = new Dictionary<string, string>()
            {
                {"۰","0" }, {"۱","1" }, {"۲","2" }, {"۳","3" },{"۴","4" }, {"۵","5" },{"۶","6" }, {"۷","7" },{"۸","8" }, {"۹","9" },
                {"٠","0" }, {"١","1" }, {"٢","2" }, {"٣","3" },{"٤","4" }, {"٥","5" },{"٦","6" }, {"٧","7" },{"٨","8" }, {"٩","9" },

            };

            foreach (var numbers in englishnumbers)
                input = input.Replace(numbers.Key, numbers.Value);

            return input;
        }


        public static string GenerateSeoLink(this string input) => input.Replace(" ", "-");
    }
}
