using ItalianDeli.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ItalianDeli.Models
{
    public class AnalyticsViewModel
    {
        public List<OrderDateGroup> OrderData { get; set; }

        public List<OrderDateGroup> OrderDataForToday { get; set; }
    }
}