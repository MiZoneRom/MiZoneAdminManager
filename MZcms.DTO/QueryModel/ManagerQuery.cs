﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MZcms.DTO.QueryModel
{
    public partial class ManagerQuery : QueryBase
    {
        public long ShopID { set; get; }

        public long userID { set; get; }
    }
}
