using AutoMapper;
using Store.Data.Entities;
using Store.Data.Entities.OrderEntities;
using Store.Repository.Interfaces;
using Store.Repository.Specification.Order;
using Store.Service.BasketService;
using Store.Service.OrderService.Dtos;
using Store.Service.PaymentService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IBasketService basketService;
        private readonly IMapper mapper;
        private readonly IPaymentService paymentService;

        public OrderService(IUnitOfWork unitOfWork
            ,IBasketService basketService,IMapper mapper,IPaymentService paymentService) 
        {
            this.unitOfWork = unitOfWork;
            this.basketService = basketService;
            this.mapper = mapper;
            this.paymentService = paymentService;
        }
        public async Task<OrderResultDto> CreateOrderAsync(OrderDto input)
        {
            var basket = await basketService.GetBasketAsync(input.BasketId);
            if (basket == null)
                throw new Exception("Basket Not Exist");
            var orderItems= new List<OrderItemDto>();
            foreach (var basketItem in basket.BasketItems)
            {
                var productItem = await unitOfWork.Repository<Product, int>().GetByIdAsync(basketItem.ProductId);
                if (productItem == null)
                    throw new Exception($"Product with id {basketItem.ProductId} Not Exist");
                var itemOrdered = new ProductItemOrdered
                {
                    ProductItemId=productItem.Id,
                    ProductName=productItem.Name,
                    PictureUrl=productItem.PictureUrl,
                };
                var orderItem = new OrderItem
                {
                    Price = productItem.Price,
                    Quantity = basketItem.Quantity,
                    ItemOrdered=itemOrdered
                };
                var mappedOrderItem=mapper.Map<OrderItemDto>(orderItem);
                orderItems.Add(mappedOrderItem);
            }
            var deliveryMethod = await unitOfWork.Repository<DeliveryMethod, int>().GetByIdAsync(input.DeliveryMethodId);
            if (deliveryMethod == null)
                throw new Exception("delivery Method not Provided ");
            var subTotal=orderItems.Sum(item=>item.Quantity*item.Price);

            //Check if Order Exist
            var specs = new OrderWithPaymentIntentSpecification(basket.PaymentIntentId);
            var existingOrder=await unitOfWork.Repository<Order,Guid>().GetWithSpecificationByIdAsync(specs);
            if (existingOrder != null)
            {
                unitOfWork.Repository<Order,Guid>().Delete(existingOrder);
                await paymentService.CreateOrUpdatePaymentIntentForExistingOrder(basket);
            }
            else
            {
                await paymentService.CreateOrUpdatePaymentIntentForNewOrder(basket.Id);

            }
            var mappedShippingAddress = mapper.Map<ShippingAddress>(input.ShippingAddress);
            var mappedOrderItems = mapper.Map<List<OrderItem>>(orderItems);
            var order = new Order
            {
                DeliveryMethodId = deliveryMethod.Id,
                ShippingAddress = mappedShippingAddress,
                BuyerEmail = input.BuyerEmail,
                OrderItems = mappedOrderItems,
                SubTotal=subTotal,
                BasketId=basket.Id,
            };
            await unitOfWork.Repository<Order,Guid>().AddAsync(order);
            await unitOfWork.CompleteAsync();
            var mappedOrder=mapper.Map<OrderResultDto>(order);
            return mappedOrder;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetAllDeliveryMethodsAsync(Guid id, string BuyerEmail)
            => await unitOfWork.Repository<DeliveryMethod, int>().GetAllAsync();

        public async Task<IReadOnlyList<OrderResultDto>> GetAllOrdersForUserAsync(string BuyerEmail)
        {
            var specs = new OrderWithItemsSpecifcation(BuyerEmail);
            var orders = await unitOfWork.Repository<Order, Guid>().GetAllWithSpecificationAsync(specs);
            if (orders is { Count: <= 0 })
                throw new Exception("You don't have any Orders yet");

            var mappedOrders=mapper.Map<List<OrderResultDto>>(orders);
            return mappedOrders;
        }

        public Task<IReadOnlyList<OrderResultDto>> GetAllOrdersForUserAsync(Guid id, string BuyerEmail)
        {
            throw new NotImplementedException();
        }

        public async Task<OrderResultDto> GetOrdersByIdAsync(Guid id, string BuyerEmail)
        {
            var specs = new OrderWithItemsSpecifcation(id,BuyerEmail);
            var order = await unitOfWork.Repository<Order, Guid>().GetWithSpecificationByIdAsync(specs);
            if(order is null)
                throw new Exception($"You don't have any Orders yet{id}");

            var mappedOrder = mapper.Map<OrderResultDto>(order);
            return mappedOrder;
        }

        
    }
}
