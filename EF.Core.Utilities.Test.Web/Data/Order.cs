using System.Collections.Generic;

namespace EF.Core.Utilities.Test.Web.Data
{
    public class Order
    {
        public virtual int Id { get; set; }

        public virtual int CustomerId { get; set; }

        public ICollection<Item> Items { get; } = new List<Item>();


        public Order SetItems(params string[] itemNames)
        {
            Items.Clear();

            foreach (var itemName in itemNames)
            {
                Items.Add( new Item { OrderId = Id, Name = itemName } );
            }

            return this;
        }
    }
}
