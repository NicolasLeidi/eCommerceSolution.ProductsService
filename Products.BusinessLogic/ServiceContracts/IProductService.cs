using Products.BusinessLogic.DTO;

namespace Products.BusinessLogic.ServiceContracts;

public interface IProductService
{
    Task<IList<ProductResponse>?> GetAll();
    Task<IList<ProductResponse>?> GetSpecific(Guid? ProductID = null, string? ProductName = null, string? Category = null, double? UnitPrice = null, int? QuantityInStock = null, string? ProductNameOrCategory = null);
    Task<ProductResponse?> Add(ProductAddRequest request);
    Task<ProductResponse?> Update(ProductUpdateRequest request);
    Task<ProductResponse?> Delete(Guid? ProductID = null);
}
