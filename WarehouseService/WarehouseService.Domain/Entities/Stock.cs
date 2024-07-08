namespace WarehouseService.Domain.Entities
{
    public class Stock
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid WarehouseId { get; set; }
        public int Quantity { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public virtual Product Product { get; set; }

    }
}
