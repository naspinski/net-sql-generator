using System.Collections.Generic;
using System.Linq;
using DotNetSqlGenerator.Library.DbProviders.PostgreSQL;
using Npgsql;

namespace DotNetSqlGenerator.Library.Objects
{
    /// <summary>
    /// Holds information about the table, including the columns and name
    /// </summary>
    public class Table
    {
        public IEnumerable<Column> Columns { get; set; }
        public string Name { get; private set; }
        public int MaxColumnIndex { get { return Columns.Count();  } }

        public Table(string tablename)
        {
            Name = tablename;
        }
    }
}
