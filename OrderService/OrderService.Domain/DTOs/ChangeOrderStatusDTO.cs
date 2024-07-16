using System;
namespace OrderService.Domain.DTOs
{
	public class ChangeOrderStatusDTO
	{
        public string OrderId { get; set; }
        public string Type { get; set; }
        public bool StatusValue { get; set; }
    }
}

