using System;

namespace Abpro.WebApiClient.Auditing
{
    public class HttpCallingAuditingInfo
    {
        public HttpCallingAuditingInfo()
        {
            CallingTime = DateTime.Now;
        }

        /// <summary>
        /// 远程调用地址
        /// </summary>
        public string RemoteApiUrl { get; set; }

        /// <summary>
        /// 请求时间
        /// </summary>
        public DateTime CallingTime { get; set; }

        /// <summary>
        /// 请求跟踪Id
        /// </summary>
        public string TrackId { get; set; }

        /// <summary>
        /// Http请求方法，POST，GET，PUT，DELETE
        /// </summary>
        public string HttpMethod { get; set; }

        /// <summary>
        /// 请求头
        /// </summary>
        public string RequestHeaders { get; set; }

        /// <summary>
        /// 请求体
        /// </summary>
        public string RequestBody { get; set; }

        /// <summary>
        /// Total duration of the method call.
        /// </summary>
        public double ExecutionDuration { get; set; }

        /// <summary>
        /// Http请求状态码
        /// </summary>
        public int HttpStatusCode { get; set; }

        /// <summary>
        /// 请求响应头
        /// </summary>
        public string ResponseHeaders { get; set; }

        /// <summary>
        /// 请求响应体
        /// </summary>
        public string ResponseBody { get; set; }

    }
}