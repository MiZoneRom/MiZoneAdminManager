//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace MZcms.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Logs:BaseModel
    {
        long _id;
        public long Id { get{ return _id; } set{ _id=value;} }
        public string PageUrl { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string UserName { get; set; }
        public string IPAddress { get; set; }
        public string Description { get; set; }
    }
}
