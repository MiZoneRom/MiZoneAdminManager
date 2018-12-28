using System;

namespace MZcms.ServiceProvider
{
    /// <summary>
    /// 服务实例创建异常
    /// </summary>
    public class ServiceInstacnceCreateException : MZcms.Core.MZcmsException
    {
        public ServiceInstacnceCreateException() { }

        public ServiceInstacnceCreateException(string message) : base(message) { }

        public ServiceInstacnceCreateException(string message, Exception inner) : base(message, inner) { }
    }
}
