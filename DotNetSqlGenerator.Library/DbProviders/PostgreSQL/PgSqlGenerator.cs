using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using DotNetSqlGenerator.Library.Interfaces;
using System.Data;
using NpgsqlTypes;
using DotNetSqlGenerator.Library.Objects;
using DotNetPostgresSqlGenerator.Library.DbProviders.PostgreSQL;

namespace DotNetSqlGenerator.Library.DbProviders.PostgreSQL
{
    public class PgSqlGenerator : SqlGenerator, IGenerator
    {
        #region Properties_Initializers
        public IEnumerable<string> TableNames { get; private set; }

        protected NpgsqlConnection Connection;

        public PgSqlGenerator(string server, int port, string user, string password, string database, string options = "")
        {
            ConnectionString = "Server=" + server + ";Port=" + port + ";User Id=" + user + ";Password=" + password + ";Database=" + database + ";" + options;
            Initialize();
        }

        public PgSqlGenerator(string connectionString)
        {
            ConnectionString = connectionString;
            Initialize();
        }

        private void Initialize()
        {
            DatabaseProvider = DbProvider.PostgreSQL;
            Connection = new Npgsql.NpgsqlConnection(ConnectionString);
            TableNames = GetTableNames();
        }

        private NpgsqlCommand CreateCommand(string query)
        {
            return new NpgsqlCommand(query, Connection);
        }

        public void Dispose()
        {
            Connection.Dispose();
        }

        #endregion Properties_Initializers

        #region General_Methods

        public int RunNonQuery(string query)
        {
            return RunNonQuery(CreateCommand(query), Connection);
        }

        public IDataReader RunReader(string query)
        {
            return RunReader(CreateCommand(query), Connection);
        }

        public object RunScalar(string query)
        {
            return RunScalar(CreateCommand(query), Connection);
        }

        #endregion General_Methods

        public IEnumerable<PgColumn> GetColumns(string tablename)
        {
            List<PgColumn> columns = new List<PgColumn>();
            string query = "SELECT a.attname as \"Column\", pg_catalog.format_type(a.atttypid, a.atttypmod) as \"Datatype\", a.attnotnull as \"NotNull\" " +
                            "FROM pg_catalog.pg_attribute a " +
                            "WHERE a.attnum > 0 AND NOT a.attisdropped AND a.attrelid = (" +
                                "SELECT c.oid FROM pg_catalog.pg_class c " +
                                    "LEFT JOIN pg_catalog.pg_namespace n ON n.oid = c.relnamespace " +
                                "WHERE c.relname ~ '^(" + tablename.ToLower() + ")$' AND pg_catalog.pg_table_is_visible(c.oid));";
            IDataReader reader = RunReader(CreateCommand(query), Connection);
            while (reader.Read()) columns.Add(new PgColumn(reader));
            return columns;
        }

        private IEnumerable<string> GetTableNames()
        {
            List<string> tableNames = new List<string>();
            string query = "SELECT table_name FROM information_schema.tables WHERE table_schema='public';";
            IDataReader reader = RunReader(query);
            while (reader.Read())
            {
                try { tableNames.Add(reader[0].ToString()); }
                catch (Exception ex) { throw new Exception("Nopgsql Error getting table names", new Exception(ex.Message)); }
            }
            return tableNames;
        }

        public Table GetTable(string tablename)
        {
            return new Table(this, tablename);
        }

        public IEnumerable<object> GetSingleRandomRecordFrom(Table T)
        {
            Random rand = new Random();
            List<object> values = new List<object>();
            int offset, count = Convert.ToInt32(RunScalar("SELECT COUNT(*) FROM " + T.Name + ";"));
            if (count < 1) throw new Exception("Can't run a select on table " + T.Name + ", it has no records");

            offset = rand.Next(0, count);
            var reader = RunReader("SELECT * FROM " + T.Name + " OFFSET " + offset + " LIMIT 1;"); // single record randomly
            count = 0;
            reader.Read();
            for (int i = 0; i < T.Columns.Count(); i++)
            {
                try { values.Add(reader[i]); }
                catch (Exception ex) { throw new Exception("NpgSql error while reading in single record: GetSingleRandomRecordFrom()", new Exception(ex.Message));  }
            }
            return values;
        }
    }
}
