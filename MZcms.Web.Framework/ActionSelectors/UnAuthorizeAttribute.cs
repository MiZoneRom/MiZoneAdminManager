using System;

namespace MZcms.Web.Framework
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
   
    ///不验证权限
    public class UnAuthorize : Attribute
    {
        public UnAuthorize()
        {

        }
    }
}

