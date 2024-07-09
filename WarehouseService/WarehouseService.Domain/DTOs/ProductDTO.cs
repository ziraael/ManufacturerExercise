using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseService.Domain.DTOs
{
    public class ProductDTO
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public DateTime StartedProduction { get; set; }
        public DateTime? EndedProduction { get; set; }
    }
}
