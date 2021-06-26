using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class ApiCampaignUpdatePhotoArguments : IArgumentsBase
	{
		[DataType(DataType.ImageUrl)]
		[Display(Name = "resources.campaign.updateImage")]
		public string PhotoUrl { get; set; }
		public int Id { get; set; }

		#region Constructor
		public ApiCampaignUpdatePhotoArguments(int id, string photoUrl)
		{
			Id = id;
			PhotoUrl = photoUrl;
		}
		#endregion Constructor	
	}			
}
