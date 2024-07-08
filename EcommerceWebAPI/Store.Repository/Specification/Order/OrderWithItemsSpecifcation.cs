using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specification.Order
{
    public class OrderWithItemsSpecifcation : BaseSpecification<Data.Entities.OrderEntities.Order>
    {
        public OrderWithItemsSpecifcation(string buyerEmail) 
            : base(order=>order.BuyerEmail==buyerEmail)
        {
            AddInclude(order => order.OrderItems);
            AddInclude(order => order.DeliveryMethod);
            AddOrderByDescending(order => order.OrderDate);

        }
        public OrderWithItemsSpecifcation(Guid id,string buyerEmail)
           : base(order => order.BuyerEmail == buyerEmail && order.Id==id)
        {
            AddInclude(order => order.OrderItems);
            AddInclude(order => order.DeliveryMethod);

        }
    }
}
