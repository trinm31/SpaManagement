using System.Collections.Generic;
using SpaManagement.Models;

namespace SpaManagement.ViewModels
{
    public class OrderDetailViewModel
    {
        public Order Order { get; set; }
        public IEnumerable<OrderDetail> OrderDetails { get; set; }
    }
}