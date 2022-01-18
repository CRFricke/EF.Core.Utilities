using System.Collections.Generic;

namespace EF.Core.Utilities.Test.Web.Data
{
    public class Order
    {
        public virtual int Id { get; set; }

        public virtual int CustomerId { get; set; }

        public ICollection<Item> Items { get; set; }
    }
}
