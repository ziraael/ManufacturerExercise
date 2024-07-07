using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineService.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ProductType Type { get; set; }
        public decimal Price { get; set; }
    }

    public enum ProductType
    {
        Engine,
        Chassis,
        OptionPack
    }
}
