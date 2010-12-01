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
        /// generates a random integer within bounds
        /// </summary>
        /// <param name="max">maximum value, this is both upper and lower bounds</param>
        /// <returns>random integer</returns>
        public static int Integer(int max = int.MaxValue)
        {
            Random rand = new Random();
            return rand.Next(-max, max);
        }

        /// <summary>
        /// generates a random date
        /// </summary>
        /// <returns>random date</returns>
        public static DateTime Date()
        {
            DateTime start = new DateTime(1995, 1, 1);
            Random rand = new Random();
            int range = ((TimeSpan)(DateTime.Today - start)).Days;
            return start.AddDays(rand.Next(range));
        }

        /// <summary>
        /// generates the appropriate random value given the column type
        /// </summary>
        /// <param name="c">column to make a random value for</param>
        /// <returns>string version of the value</returns>
        public static string ForColumn(Column c)
        {
            // switch statement won't work here
            if (c.DotNetType == typeof(string)) return GenerateValues.Quote(GenerateValues.String((c.Limit > -1 ? c.Limit : 255)));
            else if (c.DotNetType == typeof(int)) return GenerateValues.Integer().ToString();
            else if (c.DotNetType == typeof(DateTime)) return GenerateValues.Quote(GenerateValues.Date().ToString()) ;
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
