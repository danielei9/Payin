using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class EntranceTypeUpdatePhotoArguments : IArgumentsBase
	{
		[DataType(DataType.ImageUrl)]
		[Display(Name = "resources.entranceType.updateImage")]
		public string PhotoUrl { get; set; }
		public int Id { get; set; }

		#region Constructor
		public EntranceTypeUpdatePhotoArguments(int id, string photoUrl)
		{
			Id = id;
			PhotoUrl = photoUrl;
		}
		#endregion Constructor
	}
}
