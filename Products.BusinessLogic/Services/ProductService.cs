using AutoMapper;
using Products.BusinessLogic.DTO;
using Products.DataAccess.Entities;
using Products.BusinessLogic.RepositoryContracts;
using Products.BusinessLogic.ServiceContracts;

namespace Products.BusinessLogic.Services;

internal class ProductService : IProductService
{
    private readonly IProductsRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductsRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
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
        Product? productToUpdate = await _productRepository.GetProductByCondition(ProductID: updateRequest.ProductID).ContinueWith(t => t.Result.FirstOrDefault());
        
        if (productToUpdate is null) return null;
        
        productToUpdate.ProductName = updateRequest.ProductName ?? productToUpdate.ProductName;
        productToUpdate.Category = updateRequest.Category ?? productToUpdate.Category;
        productToUpdate.UnitPrice = updateRequest.UnitPrice ?? productToUpdate.UnitPrice;
        productToUpdate.QuantityInStock = updateRequest.QuantityInStock ?? productToUpdate.QuantityInStock;
        
        Product? updatedProduct =  await _productRepository.UpdateProduct(productToUpdate);
        
        if (updatedProduct is null) return null;
        
        return _mapper.Map<ProductResponse>(updatedProduct) with { Success = true };
    }

    public async Task<ProductResponse?> Delete(Guid? ProductID = null)
    {
        Product? productToDelete = await _productRepository.GetProductByCondition(ProductID: ProductID).ContinueWith(t => t.Result.FirstOrDefault());

        if (productToDelete is null) return null;

        bool deleted = await _productRepository.DeleteProduct(productToDelete);

        return _mapper.Map<ProductResponse>(productToDelete) with { Success = deleted };
    }

    public async Task<ProductResponse?> Add(ProductAddRequest registerRequest)
    {
        Product? product = _mapper.Map<Product>(registerRequest);

        Product? registeredProduct = await _productRepository.AddProduct(product);

        if (registeredProduct is null) return null;

        return _mapper.Map<ProductResponse>(registeredProduct) with { Success = true };
    }
}
