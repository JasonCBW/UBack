using DBLib;
using Model.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CommonTools;

namespace UBack.Api
{
    public class SettingApiController : ApiController
    {
        /// <summary>
        /// 获取单位列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/setting/getunits")]
        public IHttpActionResult getunlitlist()
        {
            RetMsg ret = new RetMsg();

            string sql = string.Empty;
            try
            {
                sql = "select id,unitname from sys_unit where status=1";
                DataTable r = MySQLDB.Query(sql);

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
        /// 获取部门列表
        /// </summary>
        /// <param name="unitid">单位编号</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/setting/getdeps")]
        public IHttpActionResult getdeps(string unitid)
        {
            RetMsg ret = new RetMsg();

            string sql = string.Empty;
            try
            {
                sql = "select id,depname from sys_dep where status=1 and pid=" + unitid._ToInt32();
                DataTable r = MySQLDB.Query(sql);

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
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/setting/getroles")]
        public IHttpActionResult getroles()
        {
            RetMsg ret = new RetMsg();

            string sql = string.Empty;
            try
            {
                sql = "SELECT id,rolename,modules,menus,status,remark FROM sys_role";

                DataTable r = MySQLDB.Query(sql);

                ret.flag = true;
                ret.data = r;
            }
            catch (Exception ex)
            {
                ret.msg = ex.Message;
            }
            return Json(ret);
        }
    }
}
