using SqlSugar.Repository.Tests.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugar.Repository.Tests.Repositories
{
    internal class TestDBClientContext : SqlSugarClientDbContext
    {
        public TestDBClientContext(params string[] connectionNames) : base(connectionNames) { }

        public SimpleClient<PersionDetail> PersionDetailSqlServer { get { return new SimpleClient<PersionDetail>(Db.GetConnection("test_sqlserver")); } }
        public SimpleClient<PersionDetail> PersionDetailMySQL { get { return new SimpleClient<PersionDetail>(Db.GetConnection("test_mysql")); } }

        protected override void Aop(SqlSugarClient db)
        {
            if (db.IsAnyConnection("test_sqlserver"))
                db.GetConnection("test_sqlserver").Aop.OnLogExecuting = (sql, pars) => Console.WriteLine(sql, pars);
            if (db.IsAnyConnection("test_mysql"))
                db.GetConnection("test_mysql").Aop.OnLogExecuting = (sql, pars) => Console.WriteLine(sql, pars);
        }
    }
}
