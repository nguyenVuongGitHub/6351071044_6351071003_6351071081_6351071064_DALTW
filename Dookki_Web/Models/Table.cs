using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dookki_Web.Models
{
    public class Table
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public int Seat { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
    }
}