using CommonTools;
using DBLib;
using Model;
using Model.Tools;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Web;
using System.Web.Http;

namespace UBack.Api
{
    public class ModuleApiController : ApiController
    {
        /// <summary>
        /// 获取模块列表
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="limit">条数</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/module/getlist")]
        public IHttpActionResult getlist(int page, int limit)
        {
            JsonPage ret = new JsonPage();

            string sql = string.Format(@"SELECT b.id,b.modulename,b.url,a.modulename as parentname,b.`status` FROM `sys_module` a inner join sys_module b on a.id=b.parentid order by id desc");

            ret = PageHelper.getPage(sql, page, limit);

            return Json(ret);
        }

        /// <summary>
        /// 根据ID获取单个数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/module/getByID")]
        public IHttpActionResult getByID(int id)
        {
            RetMsg ret = new RetMsg();

            try
            {
                sys_Module r = MySQLDB.QueryById<sys_Module, int>(id);

                ret.flag = true;
                ret.data = r;
            }
            catch (Exception ex)
            {
                ret.msg = ex.Message;
            }
            return Json(ret);
        }

        /// <summary>
        /// 更新模块
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/module/addOrupdate")]
        public IHttpActionResult addOrupdate(JObject job)
        {
            RetMsg ret = new RetMsg();

            string sql = string.Empty;
            try
            {
                var id = job["id"]._ToInt32();

                if (id > 0)
                    sql = Util.CreateSqlByJsonData(SQLEnum.UPDATE, "sys_module", "id", job);
                else
                    sql = Util.CreateSqlByJsonData(SQLEnum.INSERT, "sys_module", "id", job);

                int effect = MySQLDB.ExecuteSql(sql);

                ret.flag = effect > 0;
            }
            catch (Exception ex)
            {
                ret.msg = ex.Message;
            }
            return Json(ret);
        }

        /// <summary>
        /// 删除模块
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/module/del")]
        public IHttpActionResult delrole(JObject data)
        {
            RetMsg ret = new RetMsg();
            try
            {
                string ids = data["ids"].ToString();

                string sql = string.Format(@"delete from sys_module where id in ({0})", ids == "" ? "0" : ids);

                ret.flag = MySQLDB.ExecuteSql(sql) > 0;
            }
            catch (Exception ex)
            {
                ret.msg = ex.Message;
            }
            return Json(ret);
        }
    }
}
