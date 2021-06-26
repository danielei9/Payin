using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class EventAddImageGalleryArguments : IArgumentsBase
	{
		[JsonIgnore]
		public int Id { get; set; }
		[DataType(DataType.ImageUrl)]
		public string ImageUrl { get; set; }

		#region Constructor
		public EventAddImageGalleryArguments(string imageUrl)
		{
			ImageUrl = imageUrl;
		}
		#endregion Constructor
	}
}
