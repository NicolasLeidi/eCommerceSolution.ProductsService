namespace Products.BusinessLogic.DTO;

public record ProductResponse(Guid ProductId, string? ProductName, string? Category, double? UnitPrice, int? QuantityInStock, bool Success)
{
    public ProductResponse() : this(Guid.Empty, null, null, null, null, false) { }
}
