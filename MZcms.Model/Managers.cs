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
    
    public partial class Managers:BaseModel
    {
        long _id;
        public long Id { get{ return _id; } set{ _id=value;} }
        public Nullable<long> RoleId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string Remark { get; set; }
        public string RealName { get; set; }
    
        public virtual Roles Roles { get; set; }
    }
}
