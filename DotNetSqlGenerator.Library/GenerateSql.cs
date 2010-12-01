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
            public static string For(Table T, PgSqlGenerator pg)
            {
                IEnumerable<object> singleRandomRecord = pg.GetSingleRandomRecordFrom(T);
                StringBuilder sql = new StringBuilder();
                Random rand = new Random();
                int r = rand.Next(0, T.MaxColumnIndex);
                Column c = T.Columns.Skip(r).First();
                sql.Append("DELETE FROM " + T.Name + " WHERE " + c.Name + " = " + Quote(singleRandomRecord.Skip(r).First().ToString(), c) + ";");
                return sql.ToString();
            }
        }

        public static class Update
        {
            public static string For(Table T, PgSqlGenerator pg)
            {
                IEnumerable<object> singleRandomRecord = pg.GetSingleRandomRecordFrom(T);
                Random rand = new Random();
                int whereColumn = rand.Next(0, T.Columns.Count());
                string[] insertSet = CreateInsertSetFor(T);
                StringBuilder sql = new StringBuilder("UPDATE " + T.Name + " SET ");
                Column c;
                
                for (int i = 0; i < T.Columns.Count(); i++)
                {
                    c = T.Columns.Skip(i).First();
                    sql.Append(c.Name + " = " + GenerateValues.ForColumn(c) + ((i < T.Columns.Count() - 1) ? ", " : string.Empty));
                }

                c = T.Columns.Skip(whereColumn).First();
                sql.Append(" WHERE " + c.Name + " = " + GenerateValues.ForColumn(c) + ";");

                return sql.ToString();
            }
        }

        public static class Select
        {
            public static string For(Table T, PgSqlGenerator pg, int numberOfColumnsToReturn = -1, int numberOfColumnsToSearch = -1)
            {
                IEnumerable<object> singleRandomRecord = pg.GetSingleRandomRecordFrom(T);
                int WHERE_LIMITER = 4;
                StringBuilder sql = new StringBuilder(), select = new StringBuilder(), where = new StringBuilder();
                Random rand = new Random();
                int r;
                
                while (numberOfColumnsToReturn < WHERE_LIMITER) WHERE_LIMITER--;
                if (numberOfColumnsToSearch < 1) numberOfColumnsToSearch = rand.Next(WHERE_LIMITER, numberOfColumnsToReturn) / WHERE_LIMITER; //search at most 1/WHERE_LIMITER of attributes
                if (numberOfColumnsToSearch < 1) numberOfColumnsToSearch = 1;
                if (numberOfColumnsToSearch > T.Columns.Count()) numberOfColumnsToSearch = T.Columns.Count();

                List<int> used = new List<int>();
                string comma, and;

                if (numberOfColumnsToReturn < 0 || numberOfColumnsToReturn >= T.Columns.Count()) select.Append("*");
                else // Select random attributes
                {
                    while (used.Count < numberOfColumnsToReturn)
                    {
                        r = rand.Next(0, T.MaxColumnIndex);
                        comma = (used.Count < numberOfColumnsToReturn - 1) ? ", " : string.Empty;
                        if (!used.Contains(r))
                        {
                            select.Append(T.Columns.Skip(r).First().Name + comma);
                            used.Add(r);
                        }
                    }
                }

                //generate a random where clause which is guaranteed to match at least one record
                used = new List<int>();
                while (used.Count < numberOfColumnsToSearch)
                {
                    r = rand.Next(0, T.MaxColumnIndex);
                    and = (used.Count < numberOfColumnsToSearch - 1) ? " AND " : string.Empty;
                    if (!used.Contains(r))
                    {
                        Column c = T.Columns.Skip(r).First();
                        where.Append(c.Name + " = " + Quote(singleRandomRecord.Skip(r).First().ToString(), c) + and);
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
                string[] insertSet = CreateInsertSetFor(T);
                StringBuilder sql = new StringBuilder("INSERT INTO " + T.Name + " (" + insertSet[0] + ") VALUES (" + insertSet[1] + ");");
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

            public static string BulkFor(Table T, int howMany)
            {
                if (howMany < 1) throw new Exception("howMany in GenerateSql.Insert.BulkFor cannot be lower than 1");
                string[] insertSet = CreateInsertSetFor(T);
                StringBuilder sql = new StringBuilder("INSERT INTO " + T.Name + " (" + insertSet[0] + ") VALUES ");
                for (int i = 0; i < howMany; i++)
                {
                    insertSet = CreateInsertSetFor(T);
                    sql.Append("(" + insertSet[1] + ")" + ((i < howMany - 1) ? ", " : string.Empty));
                }
                sql.Append(";");
                return sql.ToString();
            }

        }

        private static string[] CreateInsertSetFor(Table T)
        {
            StringBuilder fields = new StringBuilder(), values = new StringBuilder();
            string comma;
            for (int i = 0; i < T.Columns.Count(); i++)
            {
                comma = (i < T.Columns.Count() - 1) ? ", " : string.Empty;
                Column c = T.Columns.Skip(i).First();
                fields.Append(c.Name + comma);
                values.Append(GenerateValues.ForColumn(c) + comma);
            }
            return new string[] { fields.ToString(), values.ToString() };
        }

        public static string Quote(string s, Column c)
        {
            if (UnQuotedTypes.Contains(c.DotNetType)) return s;
            return "'" + s + "'";
        }
    }
}
