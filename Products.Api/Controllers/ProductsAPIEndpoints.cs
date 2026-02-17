using Microsoft.AspNetCore.Mvc;
using Products.BusinessLogic.DTO;
using Products.BusinessLogic.ServiceContracts;

namespace Products.Api.Controllers;

[Route("api/products")]
[ApiController]
public class ProductsAPIEndpoints : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsAPIEndpoints(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        IList<ProductResponse>? productsResponse = await _productService.GetAll();

        if (productsResponse is null)
            return BadRequest(productsResponse);

        return Ok(productsResponse);
    }

    [HttpGet("search/product-id/{productId}")]
    public async Task<IActionResult> SearchById(Guid productId)
    {
        ProductResponse? productResponse = await _productService.GetSpecific(ProductID: productId).ContinueWith(t => t?.Result?.FirstOrDefault());
        
        if (productResponse is null)
            return NotFound(productResponse);

        return Ok(productResponse);
    }

    [HttpGet("products/search/{searchString}")]
    public async Task<IActionResult> SearchByContent(string searchString)
    {
        IList<ProductResponse>? productResponse = await _productService.GetSpecific(ProductNameOrCategory: searchString);

        if (productResponse is null)
            return NotFound(productResponse);

        return Ok(productResponse);
    }

    [HttpPost("")]
    public async Task<IActionResult> AddProduct(ProductAddRequest productRequest)
    {
        if (productRequest is null)
            return BadRequest("ProductRequest cannot be null");

        ProductResponse? productResponse = await _productService.Add(productRequest);
        if (productResponse is null)
            return BadRequest(productResponse);

        return Ok(productResponse);
    }
    
    [HttpPut("")]
    public async Task<IActionResult> UpdateProduct(ProductUpdateRequest productRequest)
    {
        if (productRequest is null)
            return BadRequest("ProductRequest cannot be null");

        ProductResponse? productResponse = await _productService.Update(productRequest);
        if (productResponse is null)
            return BadRequest(productResponse);

        return Ok(productResponse);
    }

    [HttpDelete("{productId}")]
    public async Task<IActionResult> DeleteProduct(Guid productId)
    {
        ProductResponse? productResponse = await _productService.Delete(productId);
        if (productResponse is null)
            return NotFound(productResponse);

        return Ok(productResponse);
    }
}
