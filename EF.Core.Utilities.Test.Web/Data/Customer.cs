using System;
using System.Collections.Generic;

namespace EF.Core.Utilities.Test.Web.Data
{
    public class Customer
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public ICollection<Order> Orders { get; } = new List<Order>();


        public Customer SetOrders(params Order[] orders)
        {
            Orders.Clear();

            foreach (var order in orders)
            {
                Orders.Add(order);
            }

            return this;
        }
    }
}
