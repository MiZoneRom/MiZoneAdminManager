using MZcms.Entity;
using MZcms.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MZcms.Service
{
    public class MemberService : ServiceBase, IMemberService, IService, IDisposable
    {
        public Members GetMember(long id)
        {
            Members mem = (
                from a in context.Members
                where a.Id == id
                select a
                ).FirstOrDefault();
            return mem;
        }
    }
}
