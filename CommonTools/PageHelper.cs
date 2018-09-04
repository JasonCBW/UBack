using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using DBLib;
using Model.Tools;

namespace CommonTools
{
    public static class PageHelper
    {
        /// <summary>
        /// 根据表名构造分页数据
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="col">字段</param>
        /// <param name="page">页码</param>
        /// <param name="limit">条数</param>
        /// <returns></returns>
        public static JsonPage getPage(string table, string col, int page, int limit)
        {
            JsonPage ret = new JsonPage();

            try
            {
                int num = (page - 1) * limit;

                string sql = string.Format(@"select {0} from {1} limit {2}, {3}", col, table, num, limit);

                var json = MySQLDB.Query(sql);

                ret.code = 0;
                ret.msg = "";
                ret.count = MySQLDB.FirstValue("select count(0) from " + table)._ToInt32();
                ret.data = json;
            }
            catch (Exception ex)
            {
                ret.code = -1;
                ret.msg = ex.Message;
            }
            return ret;
        }

        /// <summary>
        /// 根据sql构造分页数据
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="page">页码</param>
        /// <param name="limit">条数</param>
        /// <returns></returns>
        public static JsonPage getPage(string sql, int page, int limit)
        {
            JsonPage ret = new JsonPage();

            try
            {
                int num = (page - 1) * limit;

                string exsql = string.Format(sql + " limit {0},{1}", num, limit);

                var json = MySQLDB.Query(exsql);

                ret.code = 0;
                ret.msg = "";
                ret.count = MySQLDB.FirstValue("select count(0) from (" + sql + ")t")._ToInt32();
                ret.data = json;
            }
            catch (Exception ex)
            {
                ret.code = -1;
                ret.msg = ex.Message;
            }
            return ret;
        }
    }
}
