using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace CommonTools
{
    public static class Converte
    {
        /// <summary>
        /// 字符串转换成整形
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int _ToInt32(this object obj)
        {
            try
            {
                return Convert.ToInt32(obj);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// 将对象转为JSON字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string _ToJson(this object obj)
        {
            string str = JsonConvert.SerializeObject(obj);
            return str;
        }

        /// <summary>
        /// 数据表转为JSON字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string _TableToJson(this DataTable obj)
        {
            string str = obj._ToJArray().ToString();
            return str;
        }

        /// <summary>
        /// 将对象转为JArray数组
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static JArray _ToJArray(this object obj)
        {
            JArray jarray = JArray.FromObject(obj);
            return jarray;
        }

        /// <summary>
        /// 数据表转泛型实体集合
        /// </summary>
        /// <typeparam name="T">泛型T</typeparam>
        /// <param name="dt">DataTable</param>
        /// <returns></returns>
        public static List<T> _TableToModel<T>(this DataTable dt) where T : new()
        {
            if (dt == null || dt.Rows.Count == 0)
                return null;

            List<T> modelList = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                T model = new T();
                for (int i = 0; i < dr.Table.Columns.Count; i++)
                {
                    PropertyInfo propertyInfo = model.GetType().GetProperty(dr.Table.Columns[i].ColumnName);
                    if (propertyInfo != null && dr[i] != DBNull.Value)
                        propertyInfo.SetValue(model, dr[i], null);
                }
                modelList.Add(model);
            }
            return modelList;
        }

        /// <summary>
        /// json对象数组转泛型list集合
        /// </summary>
        /// <typeparam name="T">泛型T</typeparam>
        /// <param name="str">json数组</param>
        /// <returns></returns>
        public static List<T> _JsonStrToModelList<T>(this string str) where T : new()
        {
            if (string.IsNullOrEmpty(str))
                return null;

            List<T> modelList = new List<T>();

            try
            {
                if (!string.IsNullOrEmpty(str))
                {
                    JArray array = JArray.Parse(str);
                    modelList = array.ToObject<List<T>>();
                }
            }
            catch (Exception)
            {
            }
            return modelList;
        }

        /// <summary>
        /// json对象转泛型实体
        /// </summary>
        /// <typeparam name="T">泛型T</typeparam>
        /// <param name="str">json字符串</param>
        /// <returns></returns>
        public static T _JsonStrToModel<T>(this string str) where T : new()
        {
            T model = new T();

            try
            {
                JObject obj = JObject.Parse(str);

                model = obj.ToObject<T>();
            }
            catch (Exception)
            {
            }

            return model;
        }

        /// <summary>
        /// 将json字符串反序列化为字典类型
        /// </summary>
        /// <typeparam name="TKey">字典key</typeparam>
        /// <typeparam name="TValue">字典value</typeparam>
        /// <param name="jsonStr">json字符串</param>
        /// <returns>字典数据</returns>
        public static Dictionary<TKey, TValue> _JsonToDictionary<TKey, TValue>(this string jsonStr)
        {
            if (string.IsNullOrEmpty(jsonStr))
                return new Dictionary<TKey, TValue>();

            Dictionary<TKey, TValue> jsondic = JsonConvert.DeserializeObject<Dictionary<TKey, TValue>>(jsonStr);

            return jsondic;
        }
    }
}
