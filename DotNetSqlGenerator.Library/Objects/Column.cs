using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DotNetSqlGenerator.Library.Objects
{
    public class Column
    {
        public string Name;
        public DbType SystemType;
        public Type DotNetType;
        public bool Nullable;
        public int Limit;
    }
}
