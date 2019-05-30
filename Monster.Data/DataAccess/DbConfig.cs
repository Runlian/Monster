using System;
using Monster.Common;
using SqlSugar;

namespace Monster.Data
{
    public class DbConfig
    {
        private static readonly string ConnectionString = ConfigHelper.DbConnectionString;
        public static SqlSugarClient GetDbInstance()
        {
            try
            {
                var db = new SqlSugarClient(new ConnectionConfig()
                {
                    ConnectionString = ConnectionString,
                    DbType = DbType.SqlServer,
                    IsAutoCloseConnection = true,
                    InitKeyType = InitKeyType.Attribute,
                    IsShardSameThread = true
                });
                return db;
            }
            catch (Exception ex)
            {
                throw new Exception("连接数据库出错，请检查您的连接字符串和网络。 ex:" + ex.Message);
            }
        }
    }
}
