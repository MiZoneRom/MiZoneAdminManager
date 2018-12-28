using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MZcms.DTO.QueryModel
{
    public partial class OperationLogQuery : QueryBase
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        //日志操作人
        public string UserName { set; get; }
    }
}
