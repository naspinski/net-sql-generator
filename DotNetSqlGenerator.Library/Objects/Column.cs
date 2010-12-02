using System;
using System.Data;
using DotNetSqlGenerator.Library.Interfaces;

namespace DotNetSqlGenerator.Library.Objects
{
    /// <summary>
    /// Holds information about a column, such as data types, name, limit and if it is nullable or not
    /// </summary>
    public class Column : IColumn
    {
        public string Name {get; private set;}
        public SqlDbType SqlType { get; private set; }
        public Type DotNetType { get; private set; }
        public bool NotNull { get; protected set; }
        public int Limit { get; private set; }

        public Column(string name, string datatype)
        {
            NotNull = true;
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

            switch(dt.ToLower())
            {
                case "integer": SqlType = SqlDbType.Int; DotNetType = typeof(Int32); break;
                case "character varying": SqlType = SqlDbType.VarChar; DotNetType = typeof(String); break;
                case "date": SqlType = SqlDbType.Date; DotNetType = typeof(DateTime); break;
                case "smallint": SqlType = SqlDbType.SmallInt; DotNetType = typeof(Int16); break;
                case "bigint": SqlType = SqlDbType.BigInt; DotNetType = typeof(Int64); break;
                case "numeric": SqlType = SqlDbType.Decimal; DotNetType = typeof(Decimal); break;
                case "real": SqlType = SqlDbType.Real; DotNetType = typeof(Single); break;
                case "double precision": SqlType = SqlDbType.Float; DotNetType = typeof(Double); break;
                case "money": SqlType = SqlDbType.Money; DotNetType = typeof(Decimal); break;
                case "char": case "character": SqlType = SqlDbType.Char; DotNetType = typeof(String); break;
                case "text": case "ntext": SqlType = SqlDbType.Text; DotNetType = typeof(String); break;
                case "bytea": SqlType = SqlDbType.Binary; DotNetType = typeof(Byte[]); break;
                case "interval": SqlType = SqlDbType.Time; DotNetType = typeof(TimeSpan); break;
                case "time":
                case "time without time zone":
                case "time with time zone":
                case "timestamp without time zone": 
                case "timestamp with time zone":
                case "timestamp": SqlType = SqlDbType.Timestamp; DotNetType = typeof(DateTime); break;
                case "bit":
                case "bool":
                case "boolean": SqlType = SqlDbType.Bit; DotNetType = typeof(Boolean); break;
                default: throw new Exception(dt + " not yet implemented");
            }
        }
    }
}
