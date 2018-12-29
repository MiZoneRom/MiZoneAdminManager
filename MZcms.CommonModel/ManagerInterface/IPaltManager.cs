using System.Collections.Generic;

namespace MZcms.CommonModel
{
    public interface IPaltManager:IManager
    {
        List<AdminPrivilege> AdminPrivileges { set; get; }
    }
}
