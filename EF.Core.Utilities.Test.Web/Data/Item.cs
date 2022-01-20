namespace EF.Core.Utilities.Test.Web.Data
{
    public class Item
    {
        public virtual int Id { get; set; }

        public virtual int OrderId { get; set; }

        public virtual string Name { get; set; }

        public override string ToString()
        {
            return $"{nameof(Item)}: Id:{Id}, OrderId:{OrderId}, Name:'{Name}'";
        }
    }
}
