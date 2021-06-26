using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class NoticeUpdatePhotoArguments : IArgumentsBase
	{
		[JsonIgnore]
		public int Id { get; set; }
		[DataType(DataType.ImageUrl)]
		[Display(Name = "resources.event.updateImage")]
		public string PhotoUrl { get; set; }

		#region Constructor
		public NoticeUpdatePhotoArguments(string photoUrl)
		{
			PhotoUrl = photoUrl;
		}
		#endregion Constructor
	}
}
