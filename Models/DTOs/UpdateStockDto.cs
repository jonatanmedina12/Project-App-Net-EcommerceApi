﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class UpdateStockDto
    {
        public int Quantity { get; set; }
        public bool IsAdd { get; set; }
    }
}