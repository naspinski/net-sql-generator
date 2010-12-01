using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetSqlGenerator.Library.Objects;

namespace DotNetPostgresSqlGenerator.Library
{
    /// <summary>
    /// Generates random values for use in the queries
    /// </summary>
    public static class GenerateValues
    {
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

        public static int Integer(int max = int.MaxValue)
        {
            Random rand = new Random();
            return rand.Next(-max, max);
        }

        public static DateTime Date()
        {
            DateTime start = new DateTime(1995, 1, 1);
            Random rand = new Random();
            int range = ((TimeSpan)(DateTime.Today - start)).Days;
            return start.AddDays(rand.Next(range));
        }

        public static string ForColumn(Column c)
        {
            // switch statement won't work here
            if (c.DotNetType == typeof(string)) return GenerateValues.Quote(GenerateValues.String((c.Limit > -1 ? c.Limit : 255)));
            else if (c.DotNetType == typeof(int)) return GenerateValues.Integer().ToString();
            else if (c.DotNetType == typeof(DateTime)) return GenerateValues.Quote(GenerateValues.Date().ToString()) ;
            else throw new Exception("problem making value for column: " + c.Name + " type: " + c.SqlType.ToString());
        }

        public static string Quote(string s)
        { return "'" + s + "'"; }
    }
}
