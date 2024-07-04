using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Api.OrderService.Application.DTOS
{
    public class OrderDTO
	{
        public int Id { get; set; }
        public string EngineType { get; set; }
        public string ChassisColor { get; set; }
        public string OptionPack { get; set; }
        public DateTime OrderDate { get; set; }
    }
}