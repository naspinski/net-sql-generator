using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DotNetSqlGenerator.Library.Interfaces;
using DotNetSqlGenerator.Library.Objects;
using Npgsql;

namespace DotNetSqlGenerator.Library.DbProviders.PostgreSQL
{
    /// <summary>
    /// Specialized SqlGenerator for pgsql
    /// </summary>
    public class PgSqlGenerator : SqlGenerator, IGenerator
    {
        #region Properties_Initializers
        public IEnumerable<string> TableNames { get; private set; }

        new protected NpgsqlConnection Connection;

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

        #endregion Properties_Initializers

        #region General_Methods

        /// <summary>
        /// Runs a query that does not return anything
        /// </summary>
        /// <param name="query">sql query to run</param>
        /// <returns>number of records affected</returns>
        public int RunNonQuery(string query)
        {
            Connection = new Npgsql.NpgsqlConnection(ConnectionString);
            return RunNonQuery(CreateCommand(query), Connection);
        }

        /// <summary>
        /// Runs a query that returns records
        /// </summary>
        /// <param name="query">sql query to run</param>
        /// <returns>reader with records</returns>
        public NpgsqlDataReader RunReader(string query)
        {
            Connection = new Npgsql.NpgsqlConnection(ConnectionString);
            return (NpgsqlDataReader)RunReader(CreateCommand(query), Connection);
        }

        /// <summary>
        /// Runs a query with a scalar results such as COUNT(*), etc.
        /// </summary>
        /// <param name="query">sql query to run</param>
        /// <returns>Scalar answer to query</returns>
        public object RunScalar(string query)
        {
            Connection = new Npgsql.NpgsqlConnection(ConnectionString);
            return RunScalar(CreateCommand(query), Connection);
        }

        #endregion General_Methods

        #region Execution

        /// <summary>
        /// Executes a random insert on specified table
        /// </summary>
        /// <param name="tablename">table to insert</param>
        /// <returns>QueryInformation object holding the information on the query</returns>
        public QueryInformation ExecuteInsert(Table T)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            QueryInformation info = new QueryInformation(SqlStatementType.Insert);
            info.Query = generateSql.Insert(T);
            info.Affected = RunNonQuery(info.Query);
            timer.Stop();
            info.Time = timer.ElapsedMilliseconds;
            return info;
        }

        /// <summary>
        /// Executes a random bulk insert on specified table
        /// </summary>
        /// <param name="tablename">table to insert</param>
        /// <param name="howMany">number of records to insert</param>
        /// <returns>QueryInformation object holding the information on the query</returns>
        public QueryInformation ExecuteBulkInsert(Table T, int howMany)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            QueryInformation info = new QueryInformation(SqlStatementType.Insert);
            info.Query = generateSql.BulkInsert(T, howMany);
            info.Affected = RunNonQuery(info.Query);
            timer.Stop();
            info.Time = timer.ElapsedMilliseconds;
            return info;
        }

        /// <summary>
        /// Executes a random delete in the specified table
        /// </summary>
        /// <param name="T">Table to delete from</param>
        /// <returns>QueryInformation object holding the information on the query</returns>
        public QueryInformation ExecuteDelete(Table T)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            QueryInformation info = new QueryInformation(SqlStatementType.Delete);
            info.Query = generateSql.Delete(T, this);
            info.Affected = RunNonQuery(info.Query);
            timer.Stop();
            info.Time = timer.ElapsedMilliseconds;
            return info;
        }

        /// <summary>
        /// Executes an update on the specified table, returning info
        /// </summary>
        /// <param name="T">Table to update</param>
        /// <returns>QueryInformation object holding the information on the query</returns>
        public QueryInformation ExecuteUpdate(Table T)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            QueryInformation info = new QueryInformation(SqlStatementType.Update);
            info.Query = generateSql.Update(T, this);
            info.Affected = RunNonQuery(info.Query);
            timer.Stop();
            info.Time = timer.ElapsedMilliseconds;
            return info;
        }

        /// <summary>
        /// Executes a random select statement and returns the reader with the results
        /// </summary>
        /// <param name="T">Table to select from</param>
        /// <param name="columnsToReturn">[optional] number of columns in the SELECT</param>
        /// <param name="columnsToSearch">[optional] number of columns in the WHERE</param>
        /// <returns>QueryInformation object holding the information on the query</returns>
        public QueryInformation ExecuteSelect(Table T, int columnsToReturn = -1, int columnsToSearch = -1)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            QueryInformation info = new QueryInformation(SqlStatementType.Select);
            info.Query = generateSql.Select(T, this, columnsToReturn, columnsToSearch);
            info.Reader = RunReader(info.Query);
            timer.Stop();
            info.Time = timer.ElapsedMilliseconds;
            return info;
        }

        #endregion

        #region Population
        /// <summary>
        /// Gets all the public tables in the database, for use in passing to GetTable()
        /// </summary>
        /// <returns>List of strings that are teh table names</returns>
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

        /// <summary>
        /// Getsa table with all the information (name, columns, etc.)
        /// </summary>
        /// <param name="tablename">name of table to get</param>
        /// <returns>Table requested</returns>
        public Table GetTable(string tablename)
        {
            return new PgTable(this, tablename);
        }

        /// <summary>
        /// Just pulls a random recod from the table
        /// This is required to make sure the queries return at least a single response
        /// </summary>
        /// <param name="T">Table to get record from</param>
        /// <returns>An IEnumerable of values from the table</returns>
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
                catch (Exception ex) { throw new Exception("NpgSql error while reading in single record: GetSingleRandomRecordFrom()", new Exception(ex.Message)); }
            }
            return values;
        }
        #endregion
    }
}
