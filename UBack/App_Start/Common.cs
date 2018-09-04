using System;
using System.Web;

namespace UBack
{
    public static class Common
    {
        public static string GetUserID(this HttpSessionStateBase session)
        {
            if (session == null)
            {
                return String.Empty;
            }
            string uid = session["userid"].ToString();
            return uid;
        }
    }
}