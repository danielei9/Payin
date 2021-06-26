using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class EventUpdatePhotoMenuArguments : IArgumentsBase
	{
		[JsonIgnore]
		public int Id { get; set; }
		[DataType(DataType.ImageUrl)]
		[Display(Name = "resources.event.updateMenuImage")]
		public string PhotoMenuUrl { get; set; }

		public int zero;

		#region Constructor
		public EventUpdatePhotoMenuArguments(string photoMenuUrl)
		{
			PhotoMenuUrl = photoMenuUrl;
		}
		#endregion Constructor
	}
}
