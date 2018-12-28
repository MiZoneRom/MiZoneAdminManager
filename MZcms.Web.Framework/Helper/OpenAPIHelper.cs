using System.Collections.Generic;
using System.Collections.Specialized;
using Hishop.Open.Api;
using System.Net.Http;
using System.Web;

namespace MZcms.Web.Framework
{
    public class OpenAPIHelper
    {
        /// <summary>
        /// 检测基础参数和参数签名
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static void CheckBaseParamsAndSign(SortedDictionary<string, string> data)
        {
            if (data == null)
            {
                throw new MZcmsApiException(Hishop.Open.Api.OpenApiErrorCode.System_Error, "no params");
            }
            if (data.Count < 1)
            {
                throw new MZcmsApiException(Hishop.Open.Api.OpenApiErrorCode.System_Error, "no params");
            }
            string app_key = "";
            string timestamp = "";
            string sign = "";

            #region 基础检测
            if (!data.TryGetValue("app_key", out app_key))
            {
                throw new MZcmsApiException(Hishop.Open.Api.OpenApiErrorCode.Missing_App_Key, "app_key");
            }
            if (string.IsNullOrWhiteSpace(app_key))
            {
                throw new MZcmsApiException(Hishop.Open.Api.OpenApiErrorCode.Missing_App_Key, "app_key");
            }
            if (!data.TryGetValue("timestamp", out timestamp))
            {
                throw new MZcmsApiException(Hishop.Open.Api.OpenApiErrorCode.Missing_Timestamp, "timestamp");
            }
            if (string.IsNullOrWhiteSpace(timestamp))
            {
                throw new MZcmsApiException(Hishop.Open.Api.OpenApiErrorCode.Missing_Timestamp, "timestamp");
            }
            if (!OpenApiSign.CheckTimeStamp(timestamp))
            {
                throw new MZcmsApiException(Hishop.Open.Api.OpenApiErrorCode.Invalid_Timestamp, "timestamp");
            }
            if (!data.TryGetValue("sign", out sign))
            {
                throw new MZcmsApiException(Hishop.Open.Api.OpenApiErrorCode.Missing_Signature, "sign");
            }
            if (string.IsNullOrWhiteSpace(sign))
            {
                throw new MZcmsApiException(Hishop.Open.Api.OpenApiErrorCode.Missing_Signature, "sign");
            }
            #endregion

            ShopHelper shobj = new ShopHelper(app_key);

            //验签
            string msg = "";
            if (!OpenApiSign.CheckSign(data, shobj.AppSecreate, ref msg))
            {
                throw new MZcmsApiException(Hishop.Open.Api.OpenApiErrorCode.Invalid_Signature, "sign");
            }
        }

        /// <summary>
        /// 获取传递参数转换成字典
        /// </summary>
        /// <returns></returns>
        public static SortedDictionary<string, string> GetSortedParams(HttpRequestMessage curRequest)
        {
            HttpContextBase context = (HttpContextBase)curRequest.Properties["MS_HttpContext"];//获取传统context
            HttpRequestBase request = context.Request;//定义传统request对象
            NameValueCollection Parameters = new NameValueCollection { request.Form, request.QueryString };

            SortedDictionary<string, string> sortparmas = new SortedDictionary<string, string>();

            foreach (var item in Parameters.AllKeys)
            {
                sortparmas.Add(item, Parameters[item]);
            }
            return sortparmas;
        }
        /// <summary>
        /// 当前
        /// </summary>
        public static string HostUrl { get; set; }
    }
}
