using AutoMapper;
using Store.Repository.BasketRepository;
using Store.Repository.BasketRepository.Models;
using Store.Service.BasketService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.BasketService
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _bascketrepository;
        private readonly IMapper _mapper;

        public BasketService(IBasketRepository bascketrepository,IMapper mapper) 
        {
            _bascketrepository = bascketrepository;
            _mapper = mapper;
        }
        public async Task<bool> DeleteBasketAsync(string basketId)
              => await _bascketrepository.DeleteBasketAsync(basketId);

        public async Task<CustomerBasketDto> GetBasketAsync(string basketId)
        {
            var basket=await _bascketrepository.GetBasketAsync(basketId);
            if (basket is null)
                return new CustomerBasketDto();
            var mappedBasket=_mapper.Map<CustomerBasketDto>(basket);
            return mappedBasket;
        }

        public async Task<CustomerBasketDto> UpdateBasketAsync(CustomerBasketDto basket)
        {
            var customerBasket=_mapper.Map<CustomerBasket>(basket);
            var updatedbasket=await _bascketrepository.UpdateBasketAsync(customerBasket);
            var mappedCustomerBasket=_mapper.Map<CustomerBasketDto>(updatedbasket);
            return mappedCustomerBasket;
        }
    }
}
