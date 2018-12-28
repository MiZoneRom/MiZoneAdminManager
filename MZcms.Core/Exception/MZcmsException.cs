using System;

namespace MZcms.Core
{
    /// <summary>
    /// MZcms 异常
    /// </summary>
    public class MZcmsException : ApplicationException
    {
        public MZcmsException() {
            Log.Info(this.Message, this);
        }

        public MZcmsException(string message) : base(message) {
            Log.Info(message, this);
        }

        public MZcmsException(string message, Exception inner) : base(message, inner) {
            Log.Info(message, inner);
        }

    }
}
