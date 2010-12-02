using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetSqlGenerator.Library.Objects;
using Npgsql;
using DotNetSqlGenerator.Library.Interfaces;

namespace DotNetSqlGenerator.Library.DbProviders.PostgreSQL
{
    public class PgTable : Table, ITable
    {
        public PgTable(PgSqlGenerator pg, string tablename) : base(tablename)
        {
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
