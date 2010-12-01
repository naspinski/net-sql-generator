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

        public DbProvider DatabaseProvider;

        public SqlGenerator(){}

        #endregion Properties_Initializers

        #region Generators

        public void GenerateTable(int columns)
        {
        }

        #endregion Generators

        #region General_Methods

        public int RunNonQuery(IDbCommand command)
        {
            int rowsAffected = -1;
            try
            {
                rowsAffected = command.ExecuteNonQuery();
            }
            return rowsAffected;
        }

        public IDataReader RunReader(IDbCommand command)
        {
            IDataReader reader = null;
            reader = command.ExecuteReader();
            return reader;
        }

        public object RunScalar(IDbCommand command)
        {
            object column = -1;
            command.Connection.Open();
            try
            {
                column = command.ExecuteScalar();
            }
            finally
            {
                command.Connection.Close();
            }
            return column;
        }

        #endregion General_Methods
    }
}
