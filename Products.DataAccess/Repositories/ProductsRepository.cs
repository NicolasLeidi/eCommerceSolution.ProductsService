using Dapper;
using Products.DataAccess.DbContext;
using Products.DataAccess.Entities;
using Products.DataAccess.RepositoryContracts;

namespace Products.DataAccess.Repositories;

internal class ProductsRepository : IProductsRepository
{
    private readonly DapperDbContext _dbContext;

    public ProductsRepository(DapperDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IList<Product>> GetProducts()
    {
        string query = "SELECT * FROM public.\"Products\"";

        IList<Product> products = (await _dbContext.DbConnection.QueryAsync<Product>(query)).ToList();

        return products;
    }

    public async Task<IList<Product>> GetProductByCondition(Guid? ProductID = null, string? ProductName = null, string? Category = null, double? UnitPrice = null, int? QuantityInStock = null, string? ProductNameOrCategory = null)
    {
        string query = "SELECT * FROM public.\"Products\"";

        List<string> conditions = new List<string>();

        if (ProductID is not null)
            conditions.Add(" \"ProductID\" = @ProductID");

        if (ProductName is not null)
            conditions.Add(" \"ProductName\" LIKE @ProductName");

        if (Category is not null)
            conditions.Add(" \"Category\" LIKE @Category");

        if (UnitPrice is not null)
            conditions.Add(" \"UnitPrice\" = @UnitPrice");

        if (QuantityInStock is not null)
            conditions.Add(" \"QuantityInStock\" = @QuantityInStock");

        if (ProductNameOrCategory is not null)
            conditions.Add(" \"ProductName\" LIKE @ProductName OR \"Category\" LIKE @Category");

        if (conditions.Count > 0)
            query += " WHERE " + string.Join(" AND ", conditions);

        IList<Product> products = (await _dbContext.DbConnection.QueryAsync<Product>(query, new Product() { ProductID = ProductID ?? Guid.NewGuid(), ProductName = (ProductName ?? ProductNameOrCategory) + "%", Category = (Category ?? ProductNameOrCategory) + "%", UnitPrice = UnitPrice, QuantityInStock = QuantityInStock })).ToList();

        return products;
    }

    public async Task<Product?> AddProduct(Product product)
    { 
        product.ProductID = Guid.NewGuid();

        string query = "INSERT INTO public.\"Products\" (\"ProductID\", \"ProductName\", \"Category\", \"UnitPrice\", \"QuantityInStock\") VALUES (@ProductID, @ProductName, @Category, @UnitPrice, @QuantityInStock)";

        int rowsAffected = await _dbContext.DbConnection.ExecuteAsync(query, product);

        if (rowsAffected > 0)
            return product;
        else
            return null;
    }

    public async Task<Product?> UpdateProduct(Product product)
    {
        string query = "UPDATE public.\"Products\" SET \"ProductName\" = @ProductName, \"Category\" = @Category, \"UnitPrice\" = @UnitPrice, \"QuantityInStock\" = @QuantityInStock WHERE \"ProductID\" = @ProductID";

        int rowsAffected = await _dbContext.DbConnection.ExecuteAsync(query, product);

        if (rowsAffected > 0)
            return product;
        else
            return null;
    }

    public async Task<bool> DeleteProduct(Product product)
    {
        string query = "DELETE FROM public.\"Products\" WHERE \"ProductID\" = @ProductID";

        int rowsAffected = await _dbContext.DbConnection.ExecuteAsync(query, product);

        if (rowsAffected > 0)
            return true;
        else
            return false;
    }
}
