using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class ProductFamilyUpdatePhotoArguments : IArgumentsBase
	{
		public int Id { get; set; }

		[Display(Name = "resources.productFamily.photoUrl")]
		[DataType(DataType.ImageUrl)]
		public string PhotoUrl { get; set; }

		#region Constructor
		public ProductFamilyUpdatePhotoArguments(int id, string photoUrl)
		{
			Id = id;
			PhotoUrl = photoUrl;
		}
		#endregion Constructor	
	}
}
