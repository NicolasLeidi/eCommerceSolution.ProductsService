using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Products.BusinessLogic.DTO;
using Products.BusinessLogic.ServiceContracts;
using Products.BusinessLogic.Validators;

namespace Products.Api.Controllers;

[Route("api/products")]
[ApiController]
public class ProductsAPIEndpoints : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ProductAddRequestValidator _productAddRequestValidator;
    private readonly ProductUpdateRequestValidator _productUpdateRequestValidator;

    public ProductsAPIEndpoints(IProductService productService, ProductAddRequestValidator productAddRequestValidator, ProductUpdateRequestValidator productUpdateRequestValidator)
    {
        _productService = productService;
        _productAddRequestValidator = productAddRequestValidator;
        _productUpdateRequestValidator = productUpdateRequestValidator;
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

        return Ok(productResponse);
    }

    [HttpGet("search/{searchString}")]
    public async Task<IActionResult> SearchByContent(string searchString)
    {
        IList<ProductResponse>? productResponse = await _productService.GetSpecific(ProductNameOrCategory: searchString);

        return Ok(productResponse);
    }

    [HttpPost("")]
    public async Task<IActionResult> AddProduct(ProductAddRequest productRequest)
    {
        // Validate here because each microservice should be responsible for validating its own data
        ValidationResult validationResult = await _productAddRequestValidator.ValidateAsync(productRequest);

        if (!validationResult.IsValid)
        {
            // Handle validation errors
            string errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            return ValidationProblem($"Validation failed: {errors}");
        }

        ProductResponse? productResponse = await _productService.Add(productRequest);

        if (productResponse is null)
            return Problem("Error in adding product");

        return Ok(productResponse);
    }
    
    [HttpPut("")]
    public async Task<IActionResult> UpdateProduct(ProductUpdateRequest productRequest)
    {
        // Validate here because each microservice should be responsible for validating its own data
        ValidationResult validationResult = await _productUpdateRequestValidator.ValidateAsync(productRequest);

        if (!validationResult.IsValid)
        {
            // Handle validation errors
            string errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            return ValidationProblem($"Validation failed: {errors}");
        }

        ProductResponse? productResponse = await _productService.Update(productRequest);

        if (productResponse is null)
            return Problem("Error in updating product");

        return Ok(productResponse);
    }

    [HttpDelete("{productId}")]
    public async Task<IActionResult> DeleteProduct(Guid productId)
    {
        ProductResponse? productResponse = await _productService.Delete(productId);

        if (productResponse is null)
            return Problem("Error in deleting product");

        return Ok(productResponse);
    }
}
