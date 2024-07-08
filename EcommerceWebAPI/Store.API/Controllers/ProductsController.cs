using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.API.Helper;
using Store.Repository.Specification.Product;
using Store.Service.HandleResponse;
using Store.Service.Helper;
using Store.Service.Services.ProductService;
using Store.Service.Services.ProductService.Dtos;

namespace Store.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController( IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet("Brands")]
        [Cashe(90)]
        public async Task<ActionResult<IReadOnlyList<BrandTypeDetailsDto>>> GetAllBrands() 
            =>Ok(await _productService.GetAllBrandsAsync());

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<BrandTypeDetailsDto>>> GetAllTypes()
            => Ok(await _productService.GetAllTypesAsync());

        [HttpGet("Products")]
        public async Task<ActionResult<PaginatedResultDto<ProductDetailsDto>>> GetAllProducts([FromQuery]ProductSpecification input)
            => Ok(await _productService.GetAllProductAsync(input));

        [HttpGet("Product")]
        public async Task<ActionResult<IReadOnlyList<ProductDetailsDto>>> GetProduct(int? id)
        {
            if(id is null)
                return BadRequest(new Response(400,"Id is Null"));
            var product = await _productService.GetProductByIdAsync(id);
            if (product is null) 
            {
                return NotFound(new Response(404));
            }
            return Ok(product);
        }
    }
}
