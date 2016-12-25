using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LogMyWork.DTO.Rates
{
    public class RateCreateDTO
    {
        [Range(0, Double.MaxValue)]
        public Double RateValue { get; set; }
    }
}