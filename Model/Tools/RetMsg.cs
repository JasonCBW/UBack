using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Tools
{
    public class RetMsg
    {
        /// <summary>
        /// 请求是否成功
        /// </summary>
        private bool Flag = false;

        public bool flag
        {
            get { return Flag; }
            set { Flag = value; }
        }
        
        /// <summary>
        /// 异常消息
        /// </summary>
        private string Msg = "";

        public string msg
        {
            get { return Msg; }
            set { Msg = value; }
        }
        

        /// <summary>
        /// 返回的数据
        /// </summary>
        private object Data = null;

        public object data
        {
            get { return Data; }
            set { Data = value; }
        }
    }
}
