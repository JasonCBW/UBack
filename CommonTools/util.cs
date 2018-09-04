using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CommonTools
{
    public static class Util
    {
        /// <summary>
        /// 根据json生成对应的sql语句
        /// </summary>
        /// <param name="action">insert 还是 update</param>
        /// <param name="tableName">表名</param>
        /// <param name="primaryKey">主键字段</param>
        /// <param name="obj">json对象</param>
        /// <returns></returns>
        public static string CreateSqlByJsonData(SQLEnum action, string tableName, string primaryKey, JObject obj)
        {
            //sql
            string str = string.Empty;

            //field
            string field = string.Empty;

            //value
            string val = string.Empty;

            int pkid = obj["id"]._ToInt32();

            obj.Remove("id");

            if (action == SQLEnum.INSERT)
            {
                //新增
                foreach (var item in obj)
                {
                    field += item.Key + ",";
                    val += "'" + item.Value.ToString() + "',";
                }
                field = field.TrimEnd(',');
                val = val.TrimEnd(',');
                str = string.Format(action + " into " + tableName + "({0})" + " values({1})", field, val);
            }
            else if (action == SQLEnum.UPDATE)
            {
                //修改
                foreach (var item in obj)
                {
                    if (item.Key.ToString() != primaryKey)
                        field += item.Key + "='" + item.Value + "',";
                }
                field = field.TrimEnd(',');
                str = string.Format(action + " " + tableName + " set {0} where " + primaryKey + "={1}", field, pkid);
            }
            return str.ToString();
        }
    }
}
