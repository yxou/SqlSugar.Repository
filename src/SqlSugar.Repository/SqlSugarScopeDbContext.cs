using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;

namespace SqlSugar.Repository
{
    public class SqlSugarScopeDbContext : DbContextBase
    {
        protected SqlSugarScope Db;

        public SqlSugarScopeDbContext(params string[] connectionNames)
        {
            if (connectionNames == null || connectionNames.Count() == 0) throw new ArgumentNullException($"{nameof(connectionNames)}不能为空");

            Db = new SqlSugarScope(this.CreateConnectionConfig(connectionNames), db => this.Aop(db));
        }

    }
}
