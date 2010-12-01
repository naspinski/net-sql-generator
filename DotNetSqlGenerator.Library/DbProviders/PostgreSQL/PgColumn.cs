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
    public class PgColumn : Column
    {
        public NpgsqlDbType PgType { get; private set; }

        public PgColumn(string name, string datatype)
            : base(name, datatype)
        {
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
