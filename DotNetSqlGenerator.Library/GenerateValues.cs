using System;
using System.Text;
using DotNetSqlGenerator.Library.Objects;

namespace DotNetSqlGenerator.Library
{
    /// <summary>
    /// Generates random values for use in the queries
    /// </summary>
    public static class GenerateValues
    {
        #region Randoms

        /// <summary>
        /// generates a random string within given bounds
        /// </summary>
        /// <param name="limit">max length of string, default is 255</param>
        /// <returns>random string</returns>
        public static string String(int limit = 255)
        {
            //added more spaces so there will be higher chance of spacing words to be more realistic
            string legalCharacters = "                 abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";//,.;:!@#$%^&*()_+=-";
            StringBuilder s = new StringBuilder();
            Random rand = new Random();
            limit = rand.Next(1, limit); //gets a random length for the string

            for (int i = 0; i < limit; i++) s.Append(legalCharacters[rand.Next(0, legalCharacters.Length)]);
            
            return s.ToString();
        }

        /// <summary>
        /// generates a random integer within bounds (uses Int16 to be sure it will fit in any int type)
        /// </summary>
        /// <param name="max">maximum value, this is both upper and lower bounds</param>
        /// <returns>random integer</returns>
        public static int Integer(int max = Int16.MaxValue)
        {
            max = max > Int16.MaxValue ? Int16.MaxValue : max;
            return new Random().Next(-max, max);
        }

        /// <summary>
        /// gets a string represntation of a binary number
        /// </summary>
        /// <returns>binary in string form</returns>
        public static string Binary(int length = 8)
        {            
            int decNum = new Random().Next(1000, int.MaxValue);
            return Convert.ToString(decNum, 2).Substring(0, length);
        }

        /// <summary>
        /// generates a random date
        /// </summary>
        /// <returns>random date</returns>
        public static DateTime Date()
        {
            DateTime start = new DateTime(1995, 1, 1);
            int range = ((TimeSpan)(DateTime.Today - start)).Days;
            Random rand = new Random();
            start.AddDays(rand.Next(range));
            return start.AddSeconds(rand.Next(86400));
        }

        /// <summary>
        /// generates a random time of day
        /// </summary>
        /// <returns>random time of day</returns>
        public static string Time()
        {
            return Quote(Date().TimeOfDay.ToString());
        }

        /// <summary>
        /// generates a random bit
        /// </summary>
        /// <returns>1 or 0</returns>
        public static int Bit()
        {
            return new Random().Next() % 2 == 0 ? 1 : 0;
        }

        /// <summary>
        /// generates the appropriate random value given the column type
        /// </summary>
        /// <param name="c">column to make a random value for</param>
        /// <returns>string version of the value</returns>
        public static string ForColumn(Column c)
        {
            // switch statement won't work here
            if (c.DotNetType == typeof(String)) return GenerateValues.Quote(GenerateValues.String((c.Limit > -1 ? c.Limit : 255)));
            else if (c.DotNetType == typeof(Int32) || c.DotNetType == typeof(Int16) || c.DotNetType == typeof(Int64) ||
                c.DotNetType == typeof(Double) || c.DotNetType == typeof(Single) || c.DotNetType == typeof(Decimal))
                return GenerateValues.Integer().ToString();
            else if (c.DotNetType == typeof(DateTime)) return GenerateValues.Quote(GenerateValues.Date().ToString());
            else if (c.DotNetType == typeof(Byte[])) return Quote(GenerateValues.Binary());
            else if (c.DotNetType == typeof(TimeSpan)) return GenerateValues.Time();
            else if (c.DotNetType == typeof(Boolean)) return Quote(GenerateValues.Bit().ToString());
            else throw new Exception("problem making value for column: " + c.Name + " type: " + c.SqlType.ToString());
        }

        #endregion

        #region Utilities
        
        /// <summary>
        /// puts single quotes around a string
        /// </summary>
        /// <param name="s">string to quote</param>
        /// <returns>'s'</returns>
        public static string Quote(string s)
        { return "'" + s + "'"; }

        #endregion
    }
}
