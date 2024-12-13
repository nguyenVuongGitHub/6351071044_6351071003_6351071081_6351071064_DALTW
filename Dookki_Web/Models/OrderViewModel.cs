using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dookki_Web.Models
{
    public class OrderViewModel
    {
        public int IDOrder { get; set; }
        public DateTime Day { get; set; }
        public TimeSpan Time { get; set; }
        public string Status { get; set; }
        public decimal Money { get; set; }
    }
}