#pragma warning disable CA1515 // Consider making public types internal

namespace EF.Core.Utilities.Test.Web.Data;

public class Customer
{
    public virtual int Id { get; set; }

    public virtual string? Name { get; set; }

    public ICollection<Order> Orders { get; } = [];


    public Customer SetOrders(params Order[] orders)
    {
        ArgumentNullException.ThrowIfNull(orders);

        Orders.Clear();

        foreach (var order in orders)
        {
            Orders.Add(order);
        }

        return this;
    }
}
