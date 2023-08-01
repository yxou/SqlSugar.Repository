using Microsoft.Data.SqlClient;

using MySqlConnector;

using NUnit.Framework;

using SqlSugar.Repository.Tests.Models;
using SqlSugar.Repository.Tests.Repositories;

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugar.Repository.Tests
{
    [TestFixture]
    public class SqlSugarClientDbContextTests
    {
        [SetUp]
        public void Setup()
        {
#if NET6_0_OR_GREATER
            DbProviderFactories.RegisterFactory("System.Data.SqlClient", SqlClientFactory.Instance);
            DbProviderFactories.RegisterFactory("MySql.Data.MySqlClient", MySqlConnectorFactory.Instance);
#endif
        }

        [Test]
        public void SqlServerTest()
        {
            var obj = InitObject();
            var db = new TestDBClientContext("test_sqlserver");
            db.PersionDetailSqlServer.InsertOrUpdate(obj);

            var objLoad = db.PersionDetailSqlServer.GetFirst(x => x.Code == obj.Code);
            Assert.Multiple(() =>
            {
                Assert.That(objLoad.Code, Is.EqualTo(obj.Code));
                Assert.That(Math.Abs((obj.CreateTime - objLoad.CreateTime).TotalSeconds), Is.LessThan(1));
            });

            db.PersionDetailSqlServer.DeleteById(obj.Code);

            var loaded = db.PersionDetailSqlServer.GetFirst(x => x.Code == obj.Code);
            Assert.That(loaded, Is.Null);
        }

        [Test]
        public void MySqlTest()
        {
            var obj = InitObject();
            var db = new TestDBClientContext("test_mysql");
            db.PersionDetailMySQL.InsertOrUpdate(obj);

            var objLoad = db.PersionDetailMySQL.GetFirst(x => x.Code == obj.Code);
            Assert.Multiple(() =>
            {
                Assert.That(objLoad.Code, Is.EqualTo(obj.Code));
                Assert.That(Math.Abs((obj.CreateTime - objLoad.CreateTime).TotalSeconds), Is.LessThan(1));
            });

            db.PersionDetailMySQL.DeleteById(obj.Code);
            
            var loaded = db.PersionDetailMySQL.GetFirst(x => x.Code == obj.Code);
            Assert.That(loaded, Is.Null);
        }

        [Test]
        public void MultiDatabaseTest()
        {
            var obj = InitObject();
            var db = new TestDBClientContext("test_sqlserver", "test_mysql");
            {
                Console.WriteLine("sqlserver");
                db.PersionDetailSqlServer.InsertOrUpdate(obj);

                var objLoad = db.PersionDetailSqlServer.GetFirst(x => x.Code == obj.Code);
                Assert.Multiple(() =>
                {
                    Assert.That(objLoad.Code, Is.EqualTo(obj.Code));
                    Assert.That(Math.Abs((obj.CreateTime - objLoad.CreateTime).TotalSeconds), Is.LessThan(1));
                });

                db.PersionDetailSqlServer.DeleteById(obj.Code);

                var loaded = db.PersionDetailSqlServer.GetFirst(x => x.Code == obj.Code);
                Assert.That(loaded, Is.Null);
            }

            {
                Console.WriteLine("mysql");
                db.PersionDetailMySQL.InsertOrUpdate(obj);

                var objLoad = db.PersionDetailMySQL.GetFirst(x => x.Code == obj.Code);
                Assert.Multiple(() =>
                {
                    Assert.That(objLoad.Code, Is.EqualTo(obj.Code));
                    Assert.That(Math.Abs((obj.CreateTime - objLoad.CreateTime).TotalSeconds), Is.LessThan(1));
                });

                db.PersionDetailMySQL.DeleteById(obj.Code);

                var loaded = db.PersionDetailMySQL.GetFirst(x => x.Code == obj.Code);
                Assert.That(loaded, Is.Null);
            }
        }

        private PersionDetail InitObject()
        {
            return new PersionDetail()
            {
                Code = Guid.NewGuid().ToString(),
                CnName = nameof(PersionDetail),
                Creator = nameof(PersionDetail),
                EnName = nameof(PersionDetail),
                Data = nameof(PersionDetail),
                CreateTime = DateTime.Now,
                SortNo = 1
            };
        }
    }
}
