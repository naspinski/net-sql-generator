using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DotNetSqlGenerator.Library.Objects
{
    public class Column
    {
        public string Name {get; private set;}
        public SqlDbType SqlType { get; private set; }
        public Type DotNetType { get; private set; }
        public bool NotNull { get; protected set; }
        public int Limit { get; private set; }

        //remove
        public string TestData { get; set; }

        public Column(string name, string datatype)
        {
            NotNull = true;
            TestData = "?";
            Limit = -1;
            Name = name;
            string dt = datatype.Split(new string[] {"(",")"}, StringSplitOptions.RemoveEmptyEntries)[0].Trim();

            if (datatype.Contains("(")) // it has a limit
            {
                int start = datatype.IndexOf("(")+1;
                int end = datatype.IndexOf(")");
                string lim = datatype.Substring(start, end - start);
                Limit = Convert.ToInt32(lim);
            }

            switch(dt)
            {
                case "integer": SqlType = SqlDbType.Int; DotNetType = typeof(int); break;
                case "character varying": SqlType = SqlDbType.VarChar; DotNetType = typeof(string); break;
                case "date": SqlType = SqlDbType.Date; DotNetType = typeof(DateTime); break;
                default: throw new Exception(dt + " not yet implemented");
            }
        }
    }
}
