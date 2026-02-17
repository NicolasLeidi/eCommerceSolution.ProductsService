using FluentValidation;
using Products.BusinessLogic.DTO;

namespace Products.BusinessLogic.Validators;

public class ProductUpdateRequestValidator : AbstractValidator<ProductUpdateRequest>
{
    public ProductUpdateRequestValidator()
    {
        RuleFor(x => x.ProductID)
            .NotEmpty().WithMessage("Product ID is required.");
        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage("Product name is required.");
        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Category is required.");
        RuleFor(x => x.UnitPrice)
            .GreaterThan(0).WithMessage("Unit price must be greater than 0.");
        RuleFor(x => x.QuantityInStock)
            .GreaterThanOrEqualTo(0).WithMessage("Quantity in stock cannot be negative.");
    }
}
