using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetSqlGenerator.Library.DbProviders.PostgreSQL;
using DotNetSqlGenerator.Library.DbProviders.PostgreSQL;
using Npgsql;

namespace DotNetSqlGenerator.Library.Objects
{
    /// <summary>
    /// Holds information about the table, including the columns and name
    /// </summary>
    public class Table
    {
        public IEnumerable<Column> Columns { get; private set; }
        public string Name { get; private set; }
        public int MaxColumnIndex { get { return Columns.Count();  } }

        public Table(PgSqlGenerator pg, string tablename)
        {
            Name = tablename;
            Columns = GetColumns(pg, tablename);
        }

        private IEnumerable<PgColumn> GetColumns(PgSqlGenerator pg, string tablename)
        {
            List<PgColumn> columns = new List<PgColumn>();
            string query = "SELECT a.attname as \"Column\", pg_catalog.format_type(a.atttypid, a.atttypmod) as \"Datatype\", a.attnotnull as \"NotNull\" " +
                            "FROM pg_catalog.pg_attribute a " +
                            "WHERE a.attnum > 0 AND NOT a.attisdropped AND a.attrelid = (" +
                                "SELECT c.oid FROM pg_catalog.pg_class c " +
                                    "LEFT JOIN pg_catalog.pg_namespace n ON n.oid = c.relnamespace " +
                                "WHERE c.relname ~ '^(" + tablename.ToLower() + ")$' AND pg_catalog.pg_table_is_visible(c.oid));";
            NpgsqlDataReader reader = pg.RunReader(query);
            while (reader.HasRows && reader.Read()) columns.Add(new PgColumn(reader));
            return columns;
        }
    }
}
