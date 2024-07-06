namespace AssemblyService.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public string EngineType { get; set; }
        public string ChassisColor { get; set; }
        public string OptionPack { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime OrderDate { get; set; }

        // Navigation properties
        public Customer Customer { get; set; }

        // Domain logic
        public void ChangeStatus(OrderStatus newStatus)
        {
            Status = newStatus;
        }
    }
}
