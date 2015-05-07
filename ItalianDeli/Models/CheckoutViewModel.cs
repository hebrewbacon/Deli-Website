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
        public Dictionary<int, string> PickupType { get; set; }

        public CheckoutViewModel()
        {
            PickupType = new Dictionary<int, string>() {
                {0, "Pickup"},
                {1, "Delivery"}
            };
        }
    }
}