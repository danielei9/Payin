using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class MobileServiceIncidenceCreatePositionArguments : IArgumentsBase
	{
		public int IncidenceId { get; set; }
		[Display(Name = "resources.serviceNotification.latitude")]
		public decimal? Latitude { get; set; }
		[Display(Name = "resources.serviceNotification.longitude")]
		public decimal? Longitude { get; set; }

		#region Constructor
		public MobileServiceIncidenceCreatePositionArguments(decimal? latitude, decimal? longitude)
		{
			Latitude = latitude;
			Longitude = longitude;
		}
		#endregion Constructor
	}
}
