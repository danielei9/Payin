using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class PaymentConcessionCreatePhotoUrlArguments : IArgumentsBase
	{
		[DataType(DataType.ImageUrl)]
		[Display(Name = "resources.paymentConcession.bannerPhotoUrl")]
		public string PhotoUrl { get; set; }
		public int Id { get; set; }

		#region Constructor
		public PaymentConcessionCreatePhotoUrlArguments(int id, string photoUrl)
		{
			Id = id;
			PhotoUrl = photoUrl;
		}
		#endregion Constructor	
	}			
}
