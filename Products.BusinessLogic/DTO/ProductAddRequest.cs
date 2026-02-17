namespace Products.BusinessLogic.DTO;

public record ProductAddRequest (string? ProductName, string? Category, double? UnitPrice, int? QuantityInStock, bool? Success);
