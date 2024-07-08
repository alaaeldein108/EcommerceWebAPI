using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specification.Product
{
    public class ProductSpecification
    {
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public string? Sort { get; set; }
        public int PageIndex { get; set; } = 1;
        private const int MAXPAGESIZA = 50;
        private int _pageSize=6;

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = (value> MAXPAGESIZA)? MAXPAGESIZA: value; }
        }
        private string? _search;

        public string? Search
        {
            get { return _search; }
            set { _search = value?.Trim().ToLower(); }
        }

    }
}
