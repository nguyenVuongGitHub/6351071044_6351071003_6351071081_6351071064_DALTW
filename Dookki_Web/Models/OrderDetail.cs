//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dookki_Web.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderDetail
    {
        public int ID { get; set; }
        public decimal quantily { get; set; }
        public Nullable<int> ticketID { get; set; }
        public Nullable<int> paymentID { get; set; }
        public Nullable<int> orderID { get; set; }
    
        public virtual Order Order { get; set; }
        public virtual Payment Payment { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}