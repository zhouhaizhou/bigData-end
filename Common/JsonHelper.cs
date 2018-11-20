using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.IO;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
namespace Readearth.Utility
{
    public static class JsonHelper
    {
        /// <summary>
        /// 将对象转化为JSON数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJSON(object obj)
        {
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            string josnData = string.Empty;
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //return serializer.Serialize(obj); 
            //如果为DataTable对象
            if (obj == null)
            {
                return "";
            }
            else if (obj.GetType() == typeof(System.Data.DataTable))
            {
                DataTable dTable = (DataTable)obj;
                josnData = JsonConvert.SerializeObject(obj, timeFormat, new DataTableConverter());
                //JsonConvert.DeserializeObject(josnData,new DataTableConverter());
                //需要返回总的记录数的话
                if (dTable.TableName.Contains("data:{0}"))
                {
                    josnData = "{" + string.Format(dTable.TableName, josnData) + "}";
                }
                return josnData;
            }
            else if (obj.GetType() == typeof(string))
            {
                return obj.ToString();
            }
            else if (obj.GetType() == typeof(DataSet))
            {
                //DataSet ds = (DataSet)obj;
                //for (int i = 0; i < ds.Tables.Count - 1; i++)
                //{
                //    DataTable dTable = ds.Tables[i];
                josnData = JsonConvert.SerializeObject(obj, timeFormat, new DataTableConverter());
                return josnData;
                //}
            }
            else if (obj.GetType() == typeof(List<Object>))
            {
                DataContractJsonSerializer json = new DataContractJsonSerializer(obj.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    json.WriteObject(stream, obj);
                    josnData = Encoding.UTF8.GetString(stream.ToArray());
                }
                return josnData;
            }
            return JsonConvert.SerializeObject(obj, timeFormat);
        }


        public static List<T> JSONStringToList<T>(string JsonStr)
        {
            //JavaScriptSerializer Serializer = new JavaScriptSerializer();
            //List<T> objs = Serializer.Deserialize<List<T>>(JsonStr);
            //return objs;
            return JsonConvert.DeserializeObject<List<T>>(JsonStr);
        }

        public static T Deserialize<T>(string json)
        {
            //T obj = Activator.CreateInstance<T>();
            //using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            //{
            //    DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            //    return (T)serializer.ReadObject(ms);
            //}
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
