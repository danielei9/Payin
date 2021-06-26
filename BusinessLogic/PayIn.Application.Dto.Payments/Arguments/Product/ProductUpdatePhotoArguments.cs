using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class ProductUpdatePhotoArguments : IArgumentsBase
	{
		public int Id { get; set; }

		[Display(Name = "resources.product.photoUrl")]
		[DataType(DataType.ImageUrl)]
		public string PhotoUrl { get; set; }

		#region Constructor
		public ProductUpdatePhotoArguments(string photoUrl, int productId)
		{
			PhotoUrl = photoUrl;
			Id = productId;
		}
		#endregion Constructor	
	}			
}
