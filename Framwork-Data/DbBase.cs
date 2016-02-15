using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Dapper;
using System.Data.SqlClient;
using Mammothcode.Data;
using MySql.Data.MySqlClient;

namespace Mammothcode.Public.Data
{
    /// <summary>
    /// 数据库基础类
    /// </summary>
    public class DbBase : IDisposable
    {

        #region 私有属性

        /// <summary>
        /// 基础数据库参数前缀
        /// </summary>
        private string _ParamPrefix = "@";

        /// <summary>
        /// 基础连接数据源名称（是sqlserver,mysql)
        /// </summary>
        private string _ProviderName = "System.Data.SqlClient";

        /// <summary>
        /// 基础数据库连接（访问数据库）
        /// </summary>
        private IDbConnection _BbConnecttion;

        /// <summary>
        ///  基础数据库类型（比如SqlServer,MySql）
        /// </summary>
        private DBType _DbType = DBType.SqlServer;

        /// <summary>
        /// 创建提供程序对数据源的访问
        /// </summary>
        private DbProviderFactory _DbFactory;

        ///// <summary>
        ///// 数据库执行语句
        ///// </summary>
        //private StringBuilder _SqlString = new StringBuilder();

        ///// <summary>
        ///// 数据库执行参数
        ///// </summary>
        //private IList<object> _SqlParam = new List<object>();

        //private bool _IsTransaction = false;

        /// <summary>
        /// 功能：单个事务
        /// 创建人：甘春雨
        /// 创建时间：2015年11月5日19:12:53
        /// </summary>
        private IDbTransaction SqlTransaction;

        # endregion

        #region 公有访问属性

        /// <summary>
        /// 基础数据库连接访问属性
        /// </summary>
        public IDbConnection DbConnecttion
        {
            get
            {
                return _BbConnecttion;
            }
        }

        /// <summary>
        /// 基础数据库事务访问属性
        /// </summary>
        public IDbTransaction DbTransaction
        {
            get
            {
                if (_BbConnecttion.State == ConnectionState.Closed)
                {
                    _BbConnecttion.Open();
                }
                //return _BbConnecttion.BeginTransaction();

                // 甘春雨 添加
                try
                {
                    SqlTransaction = _BbConnecttion.BeginTransaction();
                }
                catch (Exception)
                {

                }
                return SqlTransaction;// 甘春雨  添加
            }
        }

        public IDbTransaction GetDbTransaction()
        {
            if (_BbConnecttion.State == ConnectionState.Closed)
            {
                _BbConnecttion.Open();
            }
            //return _BbConnecttion.BeginTransaction();
            // 甘春雨 添加
            try
            {
                SqlTransaction = _BbConnecttion.BeginTransaction();
            }
            catch (Exception)
            {

            }
            return SqlTransaction;// 甘春雨  添加
        }


        /// <summary>
        /// 基础数据库参数前缀访问属性
        /// </summary>
        public string ParamPrefix
        {
            get
            {
                return _ParamPrefix;
            }
        }

        /// <summary>
        /// 基础数据库数据源名称访问属性
        /// </summary>
        public string ProviderName
        {
            get
            {
                return _ProviderName;
            }
        }

        /// <summary>
        /// 基础数据库类型访问属性
        /// </summary>
        public DBType DbType
        {
            get
            {
                return _DbType;
            }
        }
        ///// <summary>
        ///// 基础数据库执行语句的设置属性
        ///// </summary>
        //public  string SqlString
        //{
        //    set
        //    {
        //        this._SqlString = this._SqlString.AppendLine(value+";");
        //    }
        //}

        ///// <summary>
        ///// 基础数据库执行参数的设置属性
        ///// </summary>
        //public object SqlParam
        //{
        //    set
        //    {
        //        this._SqlParam.Add(value);              
        //    }

        //}
        #endregion

        /// <summary>
        ///   构造方法
        /// </summary>
        /// <param name="connectionStringName">配置文件名称</param>
        public DbBase(string connectionStringName)
        {
            //从配置文件里获取连接字符串
            var connStr = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            if (!string.IsNullOrEmpty(ConfigurationManager.ConnectionStrings[connectionStringName].ProviderName))
                _ProviderName = ConfigurationManager.ConnectionStrings[connectionStringName].ProviderName;
            else
                throw new Exception("ConnectionStrings中没有配置提供程序ProviderName！");

            _DbFactory = DbProviderFactories.GetFactory(_ProviderName);
            //获取数据源名称
            SetParamPrefix();
            GetConnection();
            //获取数据库的类库   
            _BbConnecttion = _DbFactory.CreateConnection();
            _BbConnecttion.ConnectionString = connStr;
            //_BbConnecttion.Open();
            //var iDbTransaction = _BbConnecttion.BeginTransaction();  
            //_BbConnecttion.Open();

        }

        /// <summary>
        /// 对于不同数据库生成不同的连接
        /// </summary>
        /// <returns></returns>
        public IDbConnection GetConnection()
        {
            IDbConnection iDbConnection = null;
            switch (_DbType)
            {
                case DBType.SqlServer:
                    iDbConnection = new SqlConnection();
                    break;
                case DBType.Oracle:
                    // iDbConnection = new OracleConnection();
                    break;
                case DBType.MySql:
                    iDbConnection = new MySqlConnection();
                    break;
                default:
                    return null;
            }
            return iDbConnection;
        }

        public void CloseConnection()
        {
            _BbConnecttion.Close();
            _BbConnecttion.Dispose();
        }

        /// <summary>
        ///  私有方法：获取数据源名称
        /// </summary>
        private void SetParamPrefix()
        {
            string dbtype = (_DbFactory == null ? _BbConnecttion.GetType() : _DbFactory.GetType()).Name;

            // 使用类型名判断
            if (dbtype.StartsWith("MySql")) _DbType = DBType.MySql;
            else if (dbtype.StartsWith("SqlCe")) _DbType = DBType.SqlServerCE;
            else if (dbtype.StartsWith("Npgsql")) _DbType = DBType.PostgreSQL;
            else if (dbtype.StartsWith("Oracle")) _DbType = DBType.Oracle;
            else if (dbtype.StartsWith("SQLite")) _DbType = DBType.SQLite;
            else if (dbtype.StartsWith("System.Data.SqlClient") || dbtype.StartsWith("SqlClientFactory")) _DbType = DBType.SqlServer;
            // else try with provider name
            else if (_ProviderName.IndexOf("MySql", StringComparison.InvariantCultureIgnoreCase) >= 0) _DbType = DBType.MySql;
            else if (_ProviderName.IndexOf("SqlServerCe", StringComparison.InvariantCultureIgnoreCase) >= 0) _DbType = DBType.SqlServerCE;
            else if (_ProviderName.IndexOf("Npgsql", StringComparison.InvariantCultureIgnoreCase) >= 0) _DbType = DBType.PostgreSQL;
            else if (_ProviderName.IndexOf("Oracle", StringComparison.InvariantCultureIgnoreCase) >= 0) _DbType = DBType.Oracle;
            else if (_ProviderName.IndexOf("SQLite", StringComparison.InvariantCultureIgnoreCase) >= 0) _DbType = DBType.SQLite;

            if (_DbType == DBType.MySql && _BbConnecttion != null && _BbConnecttion.ConnectionString != null && _BbConnecttion.ConnectionString.IndexOf("Allow User Variables=true") >= 0)
                _ParamPrefix = "?";
            if (_DbType == DBType.Oracle)
                _ParamPrefix = ":";
        }

        public void Dispose()
        {
            if (_BbConnecttion != null)
            {
                try
                {
                    _BbConnecttion.Close();
                    _BbConnecttion.Dispose();
                }
                catch { }
            }
        }
    }

    #region 其他扩展

    /// <summary>
    /// 数据库类型
    /// SqlServer,MySql...
    /// </summary>
    public enum DBType
    {
        /// <summary>
        /// Sql Server数据库
        /// </summary>
        SqlServer,
        /// <summary>
        ///  Sql ServerCE数据库
        /// </summary>
        SqlServerCE,
        /// <summary>
        /// MySql数据库
        /// </summary>
        MySql,
        PostgreSQL,
        Oracle,
        SQLite
    }

    #endregion

}