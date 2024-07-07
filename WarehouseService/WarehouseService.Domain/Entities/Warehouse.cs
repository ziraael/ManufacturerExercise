using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseService.Domain.Entities
{
    public class Warehouse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
    }
}
