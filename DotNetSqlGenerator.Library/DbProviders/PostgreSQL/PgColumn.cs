using System;
using System.Data;
using DotNetSqlGenerator.Library.Objects;
using NpgsqlTypes;

namespace DotNetSqlGenerator.Library.DbProviders.PostgreSQL
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

            // this isn't used yet, but might be useful for someone else?
            switch(this.SqlType)
            {
                case SqlDbType.Int: PgType = NpgsqlDbType.Integer; break;
                case SqlDbType.VarChar: PgType = NpgsqlDbType.Varchar; break;
                case SqlDbType.Date: PgType = NpgsqlDbType.Date; break;
                case SqlDbType.SmallInt: PgType = NpgsqlDbType.Smallint; break;
                case SqlDbType.BigInt: PgType = NpgsqlDbType.Bigint; break;
                case SqlDbType.Decimal: PgType = NpgsqlDbType.Numeric; break;
                case SqlDbType.Real: PgType = NpgsqlDbType.Real; break;
                case SqlDbType.Float: PgType = NpgsqlDbType.Double; break;
                case SqlDbType.Money: PgType = NpgsqlDbType.Money; break;
                case SqlDbType.Char: PgType = NpgsqlDbType.Char; break;
                case SqlDbType.Text: PgType = NpgsqlDbType.Text; break;
                case SqlDbType.Binary: PgType = NpgsqlDbType.Bytea; break;
                case SqlDbType.Time: PgType = NpgsqlDbType.Time; break;
                case SqlDbType.Timestamp: PgType = NpgsqlDbType.Timestamp; break;
                case SqlDbType.Bit: PgType = NpgsqlDbType.Bit; break;
                default: throw new Exception("error with SQL > PgSQL data type conversion: " + this.SqlType.ToString());
            }
        }
    }
}
