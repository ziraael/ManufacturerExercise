namespace AssemblyService.Domain.Entities
{
    public class Customer
    {
       public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        // Navigation properties
        public ICollection<Order> Orders { get; set; }

        // Domain logic
        public string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
