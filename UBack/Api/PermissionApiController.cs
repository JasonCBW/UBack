using CommonTools;
using Model.Tools;
using DBLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using UBack.Areas.WebAdmin.Controllers;

namespace UBack.Api
{
    public class PermissionApiController : ApiController
    {
        /// <summary>
        /// 获取模块节点树
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/permission/getTree")]
        public IHttpActionResult getTree()
        {
            RetMsg ret = new RetMsg();

            List<object> tree = new List<object>();

            try
            {
                string maintreesql = string.Format(@"select t1.id,t1.modulename,GROUP_CONCAT(CONCAT_WS('|',t2.modulename,t2.id) order by t2.id asc) as childs from sys_module t1 left join (SELECT id,modulename,parentid FROM `sys_module` where parentid!=0)t2 on t1.id=t2.parentid where t1.parentid=0 group by t2.parentid");

                DataTable treedt = MySQLDB.Query(maintreesql);

                string menusql = string.Format(@"select id,menuname,code,moduleid,`status` from sys_menu order by id asc");

                DataTable menudt = MySQLDB.Query(menusql);

                foreach (DataRow item in treedt.Rows)
                {
                    List<object> mts = new List<object>();
                    //第二级节点
                    var childs = item["childs"].ToString().Split(',');
                    for (int i = 0; i < childs.Count(); i++)
                    {
                        //模块编号
                        var child = childs[i].Split('|');
                        //第三级节点
                        //var menus = from m in menudt.AsEnumerable()
                        //            where m.Field<int>("moduleid")._ToInt32() == child[1]._ToInt32()
                        //            select new
                        //            {
                        //                title = m.Field<string>("menuname"),
                        //                value = m.Field<int>("id"),
                        //                @checked = false,
                        //                data = new object { }
                        //            };

                        mts.Add(new { title = child[0].ToString(), value = child[1]._ToInt32(), @checked = false, data = new object { } });
                    }
                    //第一级节点
                    var t = new { title = item["modulename"].ToString(), value = item["id"]._ToInt32(), data = mts };

                    tree.Add(t);
                }

                ret.flag = true;

                ret.data = tree;
            }
            catch (Exception ex)
            {
                ret.msg = ex.Message;
            }
            return Json(ret);
        }

        /// <summary>
        /// 左侧菜单列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/permission/getLeftTree")]
        public IHttpActionResult getLeftRoleTree(string userid)
        {
            RetMsg ret = new RetMsg();

            ret = new PermissionController().getLeftRoleTree(userid);

            return Json(ret);
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }
    }
}