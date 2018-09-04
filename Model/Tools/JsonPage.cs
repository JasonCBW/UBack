using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Tools
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonPage
    {
        /// <summary>
        /// 状态码,正常为0
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 状态消息
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 数据总量
        /// </summary>
        public int count { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public object data { get; set; }
    }
}
