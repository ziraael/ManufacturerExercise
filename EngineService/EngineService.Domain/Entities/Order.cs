namespace OrderService.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsReadyForCollection { get; set; } = false;
        public bool IsCanceled { get; set; } = false;
        public Guid EngineId { get; set; }
        public Guid ChassisId { get; set; }
        public Guid OptionPackId { get; set; }
    }
}
