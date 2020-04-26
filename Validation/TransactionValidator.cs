using CCostsProject.Models;
using FluentValidation;

namespace CCostsProject.Validation
{
    public class TransactionValidator:AbstractValidator<Transaction>
    {
         public TransactionValidator()
         {
             /*RuleFor(m => m.Money).Must(m => m > 0).WithMessage("Test message");
             RuleFor(m => m.Type).Must(m => m =="tt").WithMessage("Test message1");*/

         }
    }
}