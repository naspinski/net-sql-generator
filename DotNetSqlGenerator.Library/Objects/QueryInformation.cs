using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DotNetSqlGenerator.Library.Objects
{
    /// <summary>
    /// Holds information about a passed query
    /// </summary>
    public class QueryInformation
    {
        public string Query { get; set; }
        public int Affected { get; set; }
        public object Scalar { get; set; }
        public IDataReader Reader { get; set; }
        public DotNetSqlGenerator.Library.SqlGenerator.SqlStatementType StatementType { get; private set; }

        public QueryInformation(DotNetSqlGenerator.Library.SqlGenerator.SqlStatementType statementType)
        {
            StatementType = statementType;
        }
    }
}
