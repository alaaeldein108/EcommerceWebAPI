using Store.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specification.Product
{
    public class ProductWithSpecification : BaseSpecification<Store.Data.Entities.Product>
    {
        public ProductWithSpecification(ProductSpecification specs) :
            base(
                product => (!specs.BrandId.HasValue || product.BrandId == specs.BrandId.Value) &&
                          (!specs.TypeId.HasValue || product.TypeId == specs.TypeId.Value) &&
                            (string.IsNullOrEmpty(specs.Search) || product.Name.Trim().ToLower()
                .Contains(specs.Search)))
        {
            AddInclude(x => x.Brand);
            AddInclude(x => x.Type);
            AddOrderBy(x => x.Name);
            ApplyPagination(specs.PageSize*(specs.PageIndex-1), specs.PageSize);
            if (!string.IsNullOrEmpty(specs.Sort))
            {
                switch(specs.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(x => x.Name);break;
                    case "Pricedesc":
                        AddOrderByDescending(x => x.Name);break;
                    default: AddOrderBy(x => x.Name); break;

                }
            }
        }
        public ProductWithSpecification(int? id): base(product => product.Id==id)
        {
            AddInclude(x => x.Brand);
            AddInclude(x => x.Type);
        }
    }
}
