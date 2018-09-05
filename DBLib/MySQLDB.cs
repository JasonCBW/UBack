using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Collections.Generic;
using Model.Tools;
using System.Linq;

namespace DBLib
{
    /// <summary>  
    /// 数据访问抽象基础类  
    /// </summary>  
    public abstract class MySQLDB
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["constr"].ToString();
        public MySQLDB()
        {
        }

        /// <summary>
        /// 根据id查询对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="id">对象实例的Id(泛型：类型int或string)</param>
        /// <param name="idName">条件的字段名称（主键名）</param>
        /// <returns></returns>
        public static T QueryById<T, I>(I id, string idName = "id")
        {
            Type type = typeof(T);
            string columnString = string.Join(",", type.GetProperties().Select(p => string.Format("{0}", p.Name)));
            string sqlString = string.Format("select {0} from {1} where {2}={3}", columnString, type.Name, idName, id.GetType().Name.ToString() == "String" ? ("'" + id.ToString() + "'") : id.ToString());
            var t = Activator.CreateInstance(type);
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand sqlCommand = new MySqlCommand(sqlString, conn);
                MySqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        SetValueByProperties(type, reader, t);
                    }
                }
            }
            return (T)t;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <typeparam name="tablename">表名</typeparam>
        /// <typeparam name="where">条件</typeparam>
        /// <returns></returns>
        public static List<T> QueryAll<T>(string tablename, string where = "")
        {
            Type type = typeof(T);
            string columnString = string.Join(",", type.GetProperties().Select(p => string.Format("{0}", p.Name)));
            string sqlString = string.Format("select {0} from {1} where 1=1 {2}", columnString, tablename, where == "" ? "" : " and " + where);
            List<T> dataList = new List<T>();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand sqlCommand = new MySqlCommand(sqlString, conn);
                MySqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var t = Activator.CreateInstance(type);
                        SetValueByProperties(type, reader, t);
                        dataList.Add((T)t);
                    }
                }
                else
                {
                    return null;
                }
            }
            return dataList;
        }

        /// <summary>
        /// 插入对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="t">对象实例</param>
        /// <param name="idName">不插入的字段（主键）</param>
        /// <returns></returns>
        public static bool Insert<T>(T t, string idName = "id")
        {
            Type type = typeof(T);
            string sqlString = "insert {0} ({1}) values ({2})";
            string columnString = string.Join(",", type.GetProperties().Where(p => p.Name != idName).Select(p => string.Format("{0}", p.Name)));
            string valueString = string.Join(",", type.GetProperties().Where(p => p.Name != idName).Select(p => string.Format("?{0}", p.Name)));
            sqlString = string.Format(sqlString, type.Name, columnString, valueString);
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand sqlCommand = new MySqlCommand(sqlString, connection);
                MySqlParameter[] sqlParameter = type.GetProperties().Where(p => p.Name != idName).Select(p => new MySqlParameter(string.Format("?{0}", p.Name), p.GetValue(t, null) ?? DBNull.Value)).ToArray();
                sqlCommand.Parameters.AddRange(sqlParameter);
                return sqlCommand.ExecuteNonQuery() > 0;
            }
        }

        /// <summary>
        /// 修改对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="t">对象实例</param>
        /// <param name="idName">自增键名或条件名</param>
        /// <returns></returns>
        public static bool Update<T>(T t, string idName = "id")
        {
            Type type = typeof(T);
            string sqlString = "update {0} set {1} where {2}={3}";
            string setString = string.Join(",", type.GetProperties().Where(p => p.Name != idName).Select(p => string.Format("{0}=?{0}", p.Name)));
            sqlString = string.Format(sqlString, type.Name, setString, idName, "?" + idName);
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand sqlCommand = new MySqlCommand(sqlString, conn);
                MySqlParameter[] sqlParameter = type.GetProperties().Select(p => new MySqlParameter(string.Format("?{0}", p.Name), p.GetValue(t, null) ?? DBNull.Value)).ToArray();
                sqlCommand.Parameters.AddRange(sqlParameter);
                return sqlCommand.ExecuteNonQuery() > 0;
            }
        }

        /// <summary>
        /// 设置值by属性（SQLreader）
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="reader">sqlreader</param>
        /// <param name="t">对象</param>
        private static void SetValueByProperties(Type type, MySqlDataReader reader, object t)
        {
            foreach (var item in type.GetProperties())
            {
                if (reader[item.Name] is DBNull) //判空
                {
                    item.SetValue(t, null);
                }
                else
                {
                    item.SetValue(t, reader[item.Name]);
                }
            }
        }

        #region 公用方法
        /// <summary>  
        /// 得到最大值  
        /// </summary>  
        /// <param name="FieldName"></param>  
        /// <param name="TableName"></param>  
        /// <returns></returns>  
        public static int GetMaxID(string FieldName, string TableName)
        {
            string strsql = "select max(" + FieldName + ")+1 from " + TableName;
            object obj = GetSingle(strsql);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return int.Parse(obj.ToString());
            }
        }

        /// <summary>  
        /// 是否存在  
        /// </summary>  
        /// <param name="strSql"></param>  
        /// <returns></returns>  
        public static bool Exists(string strSql)
        {
            object obj = GetSingle(strSql);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>  
        /// 是否存在（基于MySqlParameter）  
        /// </summary>  
        /// <param name="strSql"></param>  
        /// <param name="cmdParms"></param>  
        /// <returns></returns>  
        public static bool Exists(string strSql, params MySqlParameter[] cmdParms)
        {
            object obj = GetSingle(strSql, cmdParms);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region  执行简单SQL语句

        /// <summary>  
        /// 执行SQL语句，返回影响的记录数  
        /// </summary>  
        /// <param name="SQLString">SQL语句</param>  
        /// <returns>影响的记录数</returns>  
        public static int ExecuteSql(string SQLString)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (MySql.Data.MySqlClient.MySqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }

        public static int ExecuteSqlByTime(string SQLString, int Times)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        cmd.CommandTimeout = Times;
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (MySql.Data.MySqlClient.MySqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }

        /// <summary>  
        /// 执行带一个存储过程参数的的SQL语句。  
        /// </summary>  
        /// <param name="SQLString">SQL语句</param>  
        /// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>  
        /// <returns>影响的记录数</returns>  
        public static int ExecuteSql(string SQLString, string content)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(SQLString, connection);
                MySql.Data.MySqlClient.MySqlParameter myParameter = new MySql.Data.MySqlClient.MySqlParameter("@content", SqlDbType.NText);
                myParameter.Value = content;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (MySql.Data.MySqlClient.MySqlException e)
                {
                    throw e;
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }

        /// <summary>  
        /// 向数据库里插入图像格式的字段(和上面情况类似的另一种实例)  
        /// </summary>  
        /// <param name="strSQL">SQL语句</param>  
        /// <param name="fs">图像字节,数据库的字段类型为image的情况</param>  
        /// <returns>影响的记录数</returns>  
        public static int ExecuteSqlInsertImg(string strSQL, byte[] fs)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(strSQL, connection);
                MySql.Data.MySqlClient.MySqlParameter myParameter = new MySql.Data.MySqlClient.MySqlParameter("@fs", SqlDbType.Image);
                myParameter.Value = fs;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (MySql.Data.MySqlClient.MySqlException e)
                {
                    throw e;
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }

        /// <summary>  
        /// 执行查询语句，返回DataTable  
        /// </summary>  
        /// <param name="SQLString">查询语句</param>  
        /// <returns>DataTable</returns>  
        public static DataTable Query(string SQLString)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    MySqlDataAdapter command = new MySqlDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds.Tables[0];
            }
        }

        public static DataTable Query(string SQLString, int Times)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    MySqlDataAdapter command = new MySqlDataAdapter(SQLString, connection);
                    command.SelectCommand.CommandTimeout = Times;
                    command.Fill(ds, "ds");
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds.Tables[0];
            }
        }

        #endregion

        #region 执行带参数的SQL语句

        /// <summary>  
        /// 执行SQL语句，返回影响的记录数  
        /// </summary>  
        /// <param name="SQLString">SQL语句</param>  
        /// <returns>影响的记录数</returns>  
        public static int ExecuteSql(string SQLString, params MySqlParameter[] cmdParms)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (MySql.Data.MySqlClient.MySqlException e)
                    {
                        throw e;
                    }
                }
            }
        }

        /// <summary>  
        /// 执行一条计算查询结果语句，返回查询结果（object）。  
        /// </summary>  
        /// <param name="SQLString">计算查询结果语句</param>  
        /// <returns>查询结果（object）</returns>  
        public static object GetSingle(string SQLString, params MySqlParameter[] cmdParms)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (MySql.Data.MySqlClient.MySqlException e)
                    {
                        throw e;
                    }
                }
            }
        }

        /// <summary>  
        /// 执行带参数的查询语句，返回DataTable  
        /// </summary>  
        /// <param name="SQLString">查询语句</param>  
        /// <returns>DataTable</returns>  
        public static DataTable Query(string SQLString, params MySqlParameter[] cmdParms)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (MySql.Data.MySqlClient.MySqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return ds.Tables[0];
                }
            }
        }

        private static void PrepareCommand(MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans, string cmdText, MySqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;  
            if (cmdParms != null)
            {
                foreach (MySqlParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }
        #endregion

        /// <summary>  
        /// 执行查询语句,返回单个值
        /// </summary>  
        /// <param name="SQLString">查询语句</param>  
        /// <returns>DataTable</returns>  
        public static object FirstValue(string SQLString)
        {
            DataTable dt = Query(SQLString);
            if (null != dt && dt.Rows.Count > 0)
            {
                return dt.Rows[0][0];
            }
            return string.Empty;
        }
    }
}