﻿using MassTransit;
using MediatR;
using WarehouseService.Api.WarehouseService.Application.Requests;

namespace WarehouseService.Api.WarehouseService.Application.Handlers
{
    public class CreateProductHandler : IRequestHandler<CreateProductRequest, int>
    {
        //Inject Validators 
        private readonly IWarehouseRepository _warehouseRepository;
        private IPublishEndpoint _publishEndpoint;
        public CreateProductHandler(IWarehouseRepository warehouseRepository, IPublishEndpoint publishEndpoint)
        {
            _warehouseRepository = warehouseRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<int> Handle(CreateProductRequest request, CancellationToken cancellationToken)
        {
            var order = await _warehouseRepository.CreateProduct(request.Product);
            return order;
        }
    }
}
