using System.ComponentModel.DataAnnotations.Schema;
using AssemblyService.Infrastructure.Context.Entities;

namespace AssemblyService.Infrastructure.Context.Entities
{
    [Table("PRODUCT")]
    public class AssemblyProduct : BaseEntity
    {
        [Column("NAME")]
        public string Name { get; set; } = string.Empty;
        [Column("DESCRIPTION")]
        public string Description { get; set; } = string.Empty;
        [Column("PRICE")]
        public decimal Price { get; set; }
        [Column("STOCK")]
        public int Stock { get; set; }
    }
}