using MZcms.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MZcms.IServices
{
    public interface IMemberService : IService, IDisposable
    {
        Members GetMember(long id);
    }
}
