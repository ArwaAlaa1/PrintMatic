using AutoMapper;
using PrintMatic.Core.Entities.Order;
using PrintMatic.DTOS.OrderDTOS;

namespace PrintMatic.Helper
{
    public class CompanyNameResolver : IValueResolver<OrderItem, OrderItemInDelivery, string>
    {
        private readonly CompanyService _companyService;

        public CompanyNameResolver(CompanyService companyService)
        {
            _companyService = companyService;
        }

        public string Resolve(OrderItem source, OrderItemInDelivery destination, string destMember, ResolutionContext context)
        {
            return _companyService.GetCompanyNameAsync(source.TraderId).Result;
        }
    }

    public class CompanyLocationResolver : IValueResolver<OrderItem, OrderItemInDelivery, string>
    {
        private readonly CompanyService _companyService;

        public CompanyLocationResolver(CompanyService companyService)
        {
            _companyService = companyService;
        }

        public string Resolve(OrderItem source, OrderItemInDelivery destination, string destMember, ResolutionContext context)
        {
            return _companyService.GetCompanyLocationAsync(source.TraderId).Result;
        }
    }

}
