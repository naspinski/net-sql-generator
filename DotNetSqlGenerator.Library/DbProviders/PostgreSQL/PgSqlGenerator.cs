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
            Connection = new Npgsql.NpgsqlConnection(ConnectionString);
            return RunNonQuery(CreateCommand(query), Connection);
        }

        public NpgsqlDataReader RunReader(string query)
        {
            Connection = new Npgsql.NpgsqlConnection(ConnectionString);
            return (NpgsqlDataReader)RunReader(CreateCommand(query), Connection);
        }

        public object RunScalar(string query)
        {
            Connection = new Npgsql.NpgsqlConnection(ConnectionString);
            return RunScalar(CreateCommand(query), Connection);
        }

        #endregion General_Methods
        
        private IEnumerable<string> GetTableNames()
        {
            List<string> tableNames = new List<string>();
            string query = "SELECT table_name FROM information_schema.tables WHERE table_schema='public';";


            NpgsqlDataReader reader = RunReader(query);
            //Connection = new NpgsqlConnection(ConnectionString);
            //Connection.Open();
            //NpgsqlDataReader reader = new NpgsqlCommand(query, Connection).ExecuteReader();
            //Connection.Close();
           
 
            while (reader.HasRows && reader.VisibleFieldCount > 0 && reader.Read())
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
