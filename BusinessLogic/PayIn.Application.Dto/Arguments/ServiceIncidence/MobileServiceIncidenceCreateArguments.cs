using System.ComponentModel.DataAnnotations;
using PayIn.Common;
using Xp.Common.Dto.Arguments;
using Xp.Domain;

namespace PayIn.Application.Dto.Arguments
{
	public partial class MobileServiceIncidenceCreateArguments : IArgumentsBase
	{
		[Display(Name = "resources.incidence.type")]
		public IncidenceType Type { get; set; }

		[Display(Name = "resources.incidence.category")]
		public IncidenceCategory Category { get; set; }

		[Display(Name = "resources.incidence.subCategory")]
		public IncidenceSubCategory SubCategory { get; set; }

		[Display(Name = "resources.incidence.name")]
		[Required(AllowEmptyStrings = false)]
		public string Name { get; set; }

		[Display(Name = "resources.notification.message")]
		[Required(AllowEmptyStrings = true)]
		public string Message { get; set; }

		public int PaymentConcessionId{ get; set; }

		[Required(AllowEmptyStrings = true)]
		public string PhotoUrl { get; set; } = "";

		[Precision(9, 6)]
		public decimal? Longitude { get; set; }

		[Precision(9, 6)]
		public decimal? Latitude { get; set; }

		#region Constructors
		public MobileServiceIncidenceCreateArguments(IncidenceType type, IncidenceCategory category, IncidenceSubCategory subCategory, string name, string message, int paymentConcessionId, string photoUrl, decimal? longitude, decimal? latitude)
		{
			Type = type;
			Category = category;
			SubCategory = subCategory;
			Name = name;
			Message = message;
			PaymentConcessionId = paymentConcessionId;
			PhotoUrl = photoUrl;
			Longitude = longitude;
			Latitude = latitude;
		}
		#endregion Constructors
	}
}
