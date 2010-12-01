using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetSqlGenerator.Library.DbProviders.PostgreSQL;

namespace DotNetSqlGenerator.Library.Objects
{
    public class Table
    {
        public IEnumerable<Column> Columns { get; private set; }
        public string Name { get; private set; }

        public Table(PgSqlGenerator pg, string tablename)
        {
            Name = tablename;
            Columns = pg.GetColumns(tablename);
        }
    }
}
