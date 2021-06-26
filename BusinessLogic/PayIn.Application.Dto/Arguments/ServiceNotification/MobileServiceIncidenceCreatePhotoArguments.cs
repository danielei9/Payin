using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class MobileServiceIncidenceCreatePhotoArguments : IArgumentsBase
	{
		public int IncidenceId { get; set; }

		[DataType(DataType.ImageUrl)]
		[Display(Name = "resources.serviceNotification.photo")]
		public string PhotoUrl { get; set; }

		#region Constructor
		public MobileServiceIncidenceCreatePhotoArguments(string photoUrl)
		{
			PhotoUrl = photoUrl;
		}
		#endregion Constructor
	}
}
