﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XebecAPI.DTOs
{
    public class UnsuccessfulReasonDTO
    {
        public int Id { get; set; }

        public string Reason { get; set; }

        public string EmailTemplate { get; set; }
    }
}
