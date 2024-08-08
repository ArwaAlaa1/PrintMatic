using PrintMartic_DashBoard.Helper.ViewModels;
using PrintMatic.Repository.Data;
using System.ComponentModel.DataAnnotations;

namespace PrintMartic_DashBoard.Helper
{
	public class UniquePRP : ValidationAttribute
	{
		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			var ProdPh = (ProductPhotosVM)validationContext.ObjectInstance;
			if (ProdPh.PhotoFile != null)
			{
				ProdPh.Photo = DocumentSetting.UploadFile(ProdPh.PhotoFile, "product");

			}

			PrintMaticContext context = new PrintMaticContext();

			if (value != null && ProdPh.Photo != null)
			{


				int Prophvalue = (int)value;

				var placePhoto = context.ProductPhotos.Where(x => x.ProductId == Prophvalue && x.Photo == ProdPh.Photo).FirstOrDefault();
				if (placePhoto == null)
				{
					return ValidationResult.Success;
				}
				else
				{
					return new ValidationResult("Choose Unique Product & Photo");
				}



			}

			else
			{
				return new ValidationResult("The Product & Photo is required");
			}

		}

	}
}
