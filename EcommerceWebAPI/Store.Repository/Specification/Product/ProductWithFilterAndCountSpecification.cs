﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specification.Product
{
    public class ProductWithFilterAndCountSpecification : BaseSpecification<Store.Data.Entities.Product>
    {
        public ProductWithFilterAndCountSpecification(ProductSpecification specs) 
            : base(
                product => (!specs.BrandId.HasValue || product.BrandId == specs.BrandId.Value) &&
                          (!specs.TypeId.HasValue || product.TypeId == specs.TypeId.Value)&&
                (string.IsNullOrEmpty(specs.Search) || product.Name.Trim().ToLower()
                .Contains(specs.Search)))
        {

        }
    }
}
