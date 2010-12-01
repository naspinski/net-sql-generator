using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetSqlGenerator.Library.Objects;
using DotNetSqlGenerator.Library.DbProviders.PostgreSQL;

namespace DotNetPostgresSqlGenerator.Library
{
    public static class GenerateSql
    {
        public static Type[] UnQuotedTypes = new Type[] { typeof(int), typeof(double), typeof(float), typeof(bool) }; 

        public static class Delete
        {

        }

        public static class Update
        {

        }

        public static class Select
        {
            public static string For(Table T, IEnumerable<object> singleRandomRecord, int numberOfColumnsToReturn = -1, int numberOfColumnsToSearch = -1)
            {
                int WHERE_LIMITER = 4;
                StringBuilder sql = new StringBuilder(), select = new StringBuilder(), where = new StringBuilder();
                Random rand = new Random();
                int r;
                
                while (numberOfColumnsToReturn < WHERE_LIMITER) WHERE_LIMITER--;
                if (numberOfColumnsToSearch < 1) numberOfColumnsToSearch = rand.Next(WHERE_LIMITER, numberOfColumnsToReturn) / WHERE_LIMITER; //search at most 1/WHERE_LIMITER of attributes
                if (numberOfColumnsToSearch < 1) numberOfColumnsToSearch = 1;
                if (numberOfColumnsToSearch > T.Columns.Count()) numberOfColumnsToSearch = T.Columns.Count();

                List<int> used = new List<int>();
                string comma, quote, and;

                if (numberOfColumnsToReturn < 0 || numberOfColumnsToReturn >= T.Columns.Count()) select.Append("*");
                else // Select random attributes
                {
                    while (used.Count < numberOfColumnsToReturn)
                    {
                        r = rand.Next(0, T.Columns.Count());
                        comma = (used.Count < numberOfColumnsToReturn - 1) ? ", " : string.Empty;
                        if (!used.Contains(r)) select.Append(T.Columns.Skip(r).First().Name + comma);
                        used.Add(r);
                    }
                }

                //generate a random where clause which is guaranteed to match at least one record
                used = new List<int>();
                while (used.Count < numberOfColumnsToSearch)
                {
                    r = rand.Next(0, T.Columns.Count());
                    and = (used.Count < numberOfColumnsToSearch - 1) ? " AND " : string.Empty;
                    if (!used.Contains(r))
                    {
                        Column c = T.Columns.Skip(r).First();
                        quote = UnQuotedTypes.Contains(c.DotNetType) ? string.Empty : "'";
                        where.Append(c.Name + " = " + quote + singleRandomRecord.Skip(r).First().ToString() + quote + and);
                        used.Add(r);
                    }
                }


                string sel = select.ToString();
                if (sel.EndsWith(", ")) sel = sel.Substring(0, sel.Length - 2);
                sql.Append("SELECT " + sel + " FROM " + T.Name + " WHERE " + where.ToString() + ";");
                return sql.ToString();
            }
        }

        public static class Insert
        {

            /// <summary>
            /// returns an insert query for Table T
            /// </summary>
            /// <param name="T">Table to make query for</param>
            /// <returns>an string insert query for T</returns>
            public static string For(Table T)
            {
                StringBuilder sql = new StringBuilder("INSERT INTO " + T.Name + " (");
                StringBuilder fields = new StringBuilder();
                StringBuilder values = new StringBuilder();
                string comma;
                for(int i=0;i<T.Columns.Count();i++)
                {
                    comma = (i < T.Columns.Count() - 1) ? ", " : string.Empty;
                    Column c = T.Columns.Skip(i).First();
                    fields.Append(c.Name+comma);
                    values.Append(GenerateValues.ForColumn(c) + comma);
                }
                sql.Append(fields.ToString() + ") VALUES (" + values.ToString() + ");");
                return sql.ToString();
            }

            /// <summary>
            /// returns multiple insert queries
            /// </summary>
            /// <param name="T">Table to make queries for</param>
            /// <param name="howMany">number of queries to make</param>
            /// <returns>an IEnumerable of strings which are insert queries for T</returns>
            public static IEnumerable<string> For(Table T, int howMany)
            {
                if (howMany < 1) throw new Exception("howMany in GenerateSql.Insert.For cannot be lower than 1");

                List<string> queries = new List<string>();
                for (int i = 0; i < howMany; i++) queries.Add(GenerateSql.Insert.For(T));
                return queries;
            }
        }
    }
}
