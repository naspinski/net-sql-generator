using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetSqlGenerator.Library.Objects;
using NpgsqlTypes;
using DotNetSqlGenerator.Library.DbProviders.PostgreSQL;
using System.Data;

namespace DotNetPostgresSqlGenerator.Library.DbProviders.PostgreSQL
{
    /// <summary>
    /// Additional information needed for PostgreSQL columns
    /// </summary>
    public class PgColumn : Column
    {
        public NpgsqlDbType PgType { get; private set; }

        public PgColumn(IDataReader reader)
            : base(reader[0].ToString(), reader[1].ToString())
        {
            if (!reader[2].ToString().Trim().Equals("t")) NotNull = false;

            switch(this.SqlType)
            {
                case SqlDbType.Int: PgType = NpgsqlDbType.Integer; break;
                case SqlDbType.VarChar: PgType = NpgsqlDbType.Varchar; break;
                case SqlDbType.Date: PgType = NpgsqlDbType.Date; break;
                default: throw new Exception("error with SQL > PgSQL data type conversion");
            }
        }
    }
}
