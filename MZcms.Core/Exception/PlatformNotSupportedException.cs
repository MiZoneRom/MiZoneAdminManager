using System;

namespace MZcms.Core
{
    /// <summary>
    /// 平台不支持异常
    /// </summary>
    public class PlatformNotSupportedException : MZcmsException
    {
        public PlatformNotSupportedException(PlatformType platformType)
            : base("不支持" + platformType.ToDescription() + "平台")
        {
            Log.Info(this.Message, this);
        }
    }
}
