using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class EventUpdatePhotoArguments : IArgumentsBase
	{
		[JsonIgnore]
		public int Id { get; set; }
		[DataType(DataType.ImageUrl)]
		[Display(Name = "resources.event.updateImage")]
		public string PhotoUrl { get; set; }

		#region Constructor
		public EventUpdatePhotoArguments(string photoUrl)
		{
			PhotoUrl = photoUrl;
		}
		#endregion Constructor
	}
}
