using Products.DataAccess.Entities;

namespace Products.DataAccess.RepositoryContracts;

public interface IProductsRepository
{
    Task<IList<Product>> GetProducts();

    Task<IList<Product>> GetProductByCondition(Guid? ProductID = null, string? ProductName = null, string? Category = null, double? UnitPrice = null, int? QuantityInStock = null, string? ProductNameOrCategory = null);

    Task<Product?> AddProduct(Product product);

    Task<Product?> UpdateProduct(Product product);

    Task<bool> DeleteProduct(Product product);
}
