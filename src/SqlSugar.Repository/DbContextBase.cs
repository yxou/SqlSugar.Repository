using Microsoft.Data.SqlClient;

using MySqlConnector;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace SqlSugar.Repository
{
    public class DbContextBase
    {

        /// <summary>
        /// 根据连接串名称创建List<ConnectionConfig>
        /// connectionconfig.configID为connectionName
        /// </summary>
        /// <param name="connectionNames">连接串名称数组</param>
        /// <returns></returns>
        protected List<ConnectionConfig> CreateConnectionConfig(params string[] connectionNames)
        {
            if (connectionNames == null || connectionNames.Count() == 0) throw new ArgumentNullException($"{nameof(connectionNames)}不能为空");
            List<ConnectionConfig> configs = new List<ConnectionConfig>();
            foreach (string connectionName in connectionNames)
            {
                ConnectionStringSettings _connection = this.GetConnection(connectionName) ?? throw new Exception($"未能找到名称为{connectionName}的连接串");
                DbType dbType = this.GetDbType(_connection.ProviderName);
                configs.Add(new ConnectionConfig()
                {
                    ConfigId = connectionName,
                    ConnectionString = _connection.ConnectionString,
                    DbType = dbType,
                    IsAutoCloseConnection = true,
                    InitKeyType = SqlSugar.InitKeyType.Attribute,
                    ConfigureExternalServices = new ConfigureExternalServices()
                    {
                        EntityService = (property, column) => this.SetEntity(property, column),
                        EntityNameService = (type, entity) => this.SetTableName(type, entity, dbType)
                    }
                });
            }
            return configs;
        }

        #region connection
        /// <summary>
        /// 获取连接串
        /// </summary>
        /// <returns></returns>
        protected virtual ConnectionStringSettings GetConnection(string connectionName)
        {
            return ConfigurationManager.ConnectionStrings[connectionName];
        }

        /// <summary>
        /// 获取数据库类型
        /// </summary>
        /// <param name="providerName"></param>
        /// <returns></returns>
        protected DbType GetDbType(string providerName)
        {
            DbProviderFactory dbProviderFactory = DbProviderFactories.GetFactory(providerName);
            if (dbProviderFactory is MySqlConnectorFactory)
                return DbType.MySql;
            else if (dbProviderFactory is SqlClientFactory)
                return DbType.SqlServer;
            return DbType.SqlServer;
        }
        #endregion

        #region ConfigureExternalServices
        /// <summary>
        /// 实体设置
        /// </summary>
        /// <param name="property"></param>
        /// <param name="entityColumn"></param>
        protected virtual void SetEntity(PropertyInfo property, EntityColumnInfo entityColumn) { }

        /// <summary>
        /// 设置表名
        /// </summary>
        /// <param name="type"></param>
        /// <param name="entity"></param>
        protected virtual void SetTableName(Type type, EntityInfo entity, DbType dbType)
        {
            if (dbType == DbType.MySql)
            {
                string tableName = entity.DbTableName;
                var replacedName = $"{tableName.Replace("[", "").Replace("]", "").Replace('.', '_')}";

                if (replacedName.IndexOf("dbo_", 0, StringComparison.OrdinalIgnoreCase) == 0)
                    replacedName = replacedName.Substring(4);
                entity.DbTableName = replacedName;
            }
        }
        #endregion

        #region Aop
        /// <summary>
        /// aop相关操作
        /// </summary>
        protected virtual void Aop(SqlSugarClient db) { }
        #endregion
    }
}
