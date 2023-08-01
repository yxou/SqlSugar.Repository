# SqlSugar.Repository封装了SqlSugar的DbContext
# 示例
## 更换表名
``` csharp
    internal class TestDBClientContext : SqlSugarClientDbContext
    {
        public TestDBClientContext(params string[] connectionNames) : base(connectionNames) { }

        protected override void SetTableName(Type type, EntityInfo entity, DbType dbType)
        {
            if (dbType == DbType.MySql)
            {
                string tableName = entity.DbTableName;
                var replacedName = $"{tableName.Replace("[", "").Replace("]", "").Replace('.', '_')}";
                entity.DbTableName = replacedName;
            }
        }
    }
```