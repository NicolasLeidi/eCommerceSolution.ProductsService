using AutoMapper;
using Products.BusinessLogic.DTO;
using Products.DataAccess.Entities;
using Products.BusinessLogic.RepositoryContracts;
using Products.BusinessLogic.ServiceContracts;
using FluentValidation;
using FluentValidation.Results;

namespace Products.BusinessLogic.Services;

internal class ProductService : IProductService
{
    private readonly IProductsRepository _productRepository;
    private readonly IValidator<ProductAddRequest> _addRequestValidator;
    private readonly IValidator<ProductUpdateRequest> _updateRequestValidator;
    private readonly IMapper _mapper;

    public ProductService(IProductsRepository productRepository, IMapper mapper, IValidator<ProductAddRequest> addRequestValidator, IValidator<ProductUpdateRequest> updateRequestValidator)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _addRequestValidator = addRequestValidator;
        _updateRequestValidator = updateRequestValidator;
    }

    public async Task<IList<ProductResponse>?> GetAll()
    {
        IList<Product> products = await _productRepository.GetProducts();

        if (products is null || products.Count == 0) return null;

        var result = new List<ProductResponse>();

        foreach (var product in products)
            result.Add(_mapper.Map<ProductResponse>(product) with { Success = true });
        
        return result;
    }

    public async Task<IList<ProductResponse>?> GetSpecific(Guid? ProductID = null, string? ProductName = null, string? Category = null, double? UnitPrice = null, int? QuantityInStock = null, string? ProductNameOrCategory = null)
    {
        IList<Product> products = await _productRepository.GetProductByCondition(ProductID, ProductName, Category, UnitPrice, QuantityInStock, ProductNameOrCategory);

        if (products is null || products.Count == 0) return null;

        var result = new List<ProductResponse>();

        foreach (var product in products)
            result.Add(_mapper.Map<ProductResponse>(product) with { Success = true });

        return result;
    }

    public async Task<ProductResponse?> Update(ProductUpdateRequest updateRequest)
    {
        ValidationResult validationResult = await _updateRequestValidator.ValidateAsync(updateRequest);

        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new ArgumentException($"Validation failed: {errors}");
        }

        Product product = _mapper.Map<Product>(updateRequest);
        
        Product? updatedProduct =  await _productRepository.UpdateProduct(product);

        ProductResponse? response = updatedProduct is not null ? _mapper.Map<ProductResponse>(updatedProduct) with { Success = true } : null;

        return response;
    }

    public async Task<ProductResponse?> Delete(Guid? ProductID = null)
    {
        Product? productToDelete = await _productRepository.GetProductByCondition(ProductID: ProductID).ContinueWith(t => t.Result.FirstOrDefault());

        if (productToDelete is null) return null;

        bool deleted = await _productRepository.DeleteProduct(productToDelete);

        return _mapper.Map<ProductResponse>(productToDelete) with { Success = deleted };
    }

    public async Task<ProductResponse?> Add(ProductAddRequest productAddRequest)
    {
        ValidationResult validationResult = await _addRequestValidator.ValidateAsync(productAddRequest);

        if (!validationResult.IsValid)
        {
            // Handle validation errors
            string errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new ArgumentException($"Validation failed: {errors}");
        }

        Product? product = _mapper.Map<Product>(productAddRequest);

        Product? registeredProduct = await _productRepository.AddProduct(product);

        if (registeredProduct is null) return null;

        return _mapper.Map<ProductResponse>(registeredProduct) with { Success = true };
    }
}
