﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MZcms.Core.Plugins.Message
{
    /// <summary>
    /// 每个消息操作的状态
    /// </summary>
    public enum MessageOrderType
    {
        Normal = 1,
        Applet = 2,
        O2OApplet = 3
    }
}
