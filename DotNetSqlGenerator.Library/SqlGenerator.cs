using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DotNetSqlGenerator.Library.Interfaces;

namespace DotNetSqlGenerator.Library
{
    /// <summary>
    /// Generic SqlGenerator factory so this can be extended to other SQL types such as MySQL, MSSQL, etc.
    /// </summary>
    public class SqlGenerator
    {
        public enum DbProvider {PostgreSQL}; //add more as they are added

        #region Properties_Initializers

        protected IDbConnection Connection;
        protected string ConnectionString;
        public DbProvider DatabaseProvider;

        public SqlGenerator() { }

        #endregion Properties_Initializers

        #region Generators

        public void GenerateTable(int columns)
        {
        }

        public void Dispose()
        {
            Connection.Dispose();
        }

        #endregion Generators

        #region General_Methods

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
