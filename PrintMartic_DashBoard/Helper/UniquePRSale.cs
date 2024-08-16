using PrintMartic_DashBoard.Helper.ViewModels;
using PrintMatic.Repository.Data;
using System.ComponentModel.DataAnnotations;

namespace PrintMartic_DashBoard.Helper
{
    public class UniquePRSale: ValidationAttribute
    {

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var PSVM = (ProductSaleVM)validationContext.ObjectInstance;

            PrintMaticContext context = new PrintMaticContext();

            if(value != null && PSVM.SaleId != null)
            {
                var newValue = (int)value;
                var Ps = context.productSales.Where(x => x.ProductId== newValue &&  x.SaleId== PSVM.SaleId && x.IsDeleted== false).FirstOrDefault();

                if (Ps == null)
                {
                return ValidationResult.Success;
                }
                else
                    return new ValidationResult("Choose Unique Product & Sale");

            }

            else
            {
                return new ValidationResult("The Product & Photo is required");
            }
        }
    }
}
