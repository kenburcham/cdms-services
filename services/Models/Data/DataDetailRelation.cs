﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace services.Models.Data
{
    public class DataDetailRelation: DataDetail
    {
        public int ParentRowId { get; set; }
    }
}