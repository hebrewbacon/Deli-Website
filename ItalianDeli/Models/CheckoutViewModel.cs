using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ItalianDeli.Models
{
    public class CheckoutViewModel
    {
        public ApplicationUser User { get; set; }
        public Order Order { get; set; }
        public string OrderType { get; set; }
    }

    public class CompleteOrderViewodel
    {
        public int completeOrderId { get; set; }
        public string completeOrderType { get; set; }
    }
}