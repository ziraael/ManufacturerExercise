using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseService.Domain.Entities
{
    public class AssembledVehicleStock
    {
        public Guid Id { get; set; }
        public Guid? EngineId { get; set; }
        public Guid? ChassisId { get; set; }
        public Guid? OptionPackId { get; set; }
        public Guid? OrderId { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}
