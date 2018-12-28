using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MZcms.Core
{
    /// <summary>
    /// 实例创建异常
    /// </summary>
    public class InstanceCreateException : MZcmsException
    {
        public InstanceCreateException() { }

        public InstanceCreateException(string message) : base(message) { }

        public InstanceCreateException(string message, Exception inner) : base(message, inner) { }
    }
}
