namespace Products.BusinessLogic.DTO;

public record ProductUpdateRequest (Guid ProductID, string? ProductName, string? Category, double? UnitPrice, int? QuantityInStock, bool? Success);
