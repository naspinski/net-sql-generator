using System.Data;

namespace DotNetSqlGenerator.Library
{
    /// <summary>
    /// Generic SqlGenerator factory so this can be extended to other SQL types such as MySQL, MSSQL, etc.
    /// </summary>
    public class SqlGenerator
    {
        public enum DbProvider {PostgreSQL}; //add more as they are added
        public enum SqlStatementType { Insert, Delete, Update, Select };

        #region Properties_Initializers

        protected IDbConnection Connection;
        protected string ConnectionString;
        public DbProvider DatabaseProvider;
        public GenerateSql Generate;

        public SqlGenerator()
        {
            Generate = new GenerateSql();
        }
        
        public void Dispose()
        {
            Connection.Dispose();
        }

        #endregion Generators

        #region General_Methods

        /// <summary>
        /// Runs a query that does not return anything
        /// </summary>
        /// <param name="command">Command to run</param>
        /// <param name="connection">SQL Connection</param>
        /// <returns>number of records affected</returns>
        public int RunNonQuery(IDbCommand command, IDbConnection connection)
        {
            int rowsAffected = -1;
            connection.Open();
            try
            {
                rowsAffected = command.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
            }
            return rowsAffected;
        }

        /// <summary>
        /// Runs a query that returns records
        /// </summary>
        /// <param name="command">Command to run</param>
        /// <param name="connection">SQL Connection</param>
        /// <returns>reader with records</returns>
        public IDataReader RunReader(IDbCommand command, IDbConnection connection)
        {
            IDataReader reader = null;
            connection.Open();
            try
            {
                reader = command.ExecuteReader();
            }
            finally
            {
                connection.Close();
            }
            return reader;
        }

        /// <summary>
        /// Runs a query with a scalar results such as COUNT(*), etc.
        /// </summary>
        /// <param name="command">Command to run</param>
        /// <param name="connection">SQL Connection</param>
        /// <returns>Scalar answer to query</returns>
        public object RunScalar(IDbCommand command, IDbConnection connection)
        {
            object column = -1;
            connection.Open();
            try
            {
                column = command.ExecuteScalar();
            }
            finally
            {
                connection.Close();
            }
            return column;
        }

        #endregion General_Methods

    }
}
