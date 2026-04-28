#pragma warning disable CA1515 // Consider making public types internal

namespace EF.Core.Utilities.Test.Web.Data
{
    public class Order
    {
        public virtual int Id { get; set; }

        public virtual int CustomerId { get; set; }

        public ICollection<Item> Items { get; } = [];


        public Order SetItems(params string[] itemNames)
        {
            ArgumentNullException.ThrowIfNull(itemNames);

            Items.Clear();

            foreach (var itemName in itemNames)
            {
                Items.Add( new Item { OrderId = Id, Name = itemName } );
            }

            return this;
        }
    }
}
