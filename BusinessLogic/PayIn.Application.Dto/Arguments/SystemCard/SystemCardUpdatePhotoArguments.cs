using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public class SystemCardUpdatePhotoArguments : IArgumentsBase
	{
		public int Id { get; set; }
		[DataType(DataType.ImageUrl)]
		[Display(Name = "resources.systemCard.updateImage")]
		public string PhotoUrl { get; set; }

		#region Constructor
		public SystemCardUpdatePhotoArguments(string photoUrl)
		{
			PhotoUrl = photoUrl;
		}
		#endregion Constructor
	}
}
