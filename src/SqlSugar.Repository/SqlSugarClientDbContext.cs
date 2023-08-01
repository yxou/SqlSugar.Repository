using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace SqlSugar.Repository
{
    public class SqlSugarClientDbContext : DbContextBase
    {
        public SqlSugarClient Db;


        public SqlSugarClientDbContext(params string[] connectionNames)
        {
            if (connectionNames == null || connectionNames.Count() == 0) throw new ArgumentNullException($"{nameof(connectionNames)}不能为空");

            Db = new SqlSugarClient(this.CreateConnectionConfig(connectionNames), db => this.Aop(db));
        }
    }
}
