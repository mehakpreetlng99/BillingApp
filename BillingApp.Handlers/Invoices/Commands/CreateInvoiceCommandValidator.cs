//using FluentValidation;
//using BillingApp.DTO;

//namespace BillingApp.Handlers.Invoices.Commands
//{
//    public class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
//    {
//        public CreateInvoiceCommandValidator()
//        {
//            RuleFor(x => x.Invoice.CustomerId)
//                .GreaterThan(0).WithMessage("CustomerId is required.");

//            RuleFor(x => x.Invoice.Items)
//                .NotEmpty().WithMessage("Invoice must have at least one item.");

//            RuleForEach(x => x.Invoice.Items).ChildRules(item =>
//            {
//                item.RuleFor(i => i.ProductId)
//                    .GreaterThan(0).WithMessage("ProductId is required.");

//                item.RuleFor(i => i.Quantity)
//                    .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

//                item.RuleFor(i => i.Price)
//                    .GreaterThan(0).WithMessage("Price must be greater than zero.");
//            });

//            RuleFor(x => x.Invoice.TotalAmount)
//                .GreaterThan(0).WithMessage("TotalAmount must be greater than zero.");
//        }
//    }
//}

