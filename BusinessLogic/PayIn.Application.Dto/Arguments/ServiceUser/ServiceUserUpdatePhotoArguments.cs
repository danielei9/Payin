using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public class ServiceUserUpdatePhotoArguments : IArgumentsBase
	{
		[DataSubType(DataSubType.Image)] 	public string Image { get; set; }
		                                    public int Id { get; set; }        
		#region Constructor
		public ServiceUserUpdatePhotoArguments(string image,int serviceUserId)
		{
			Image = image;
			Id = serviceUserId;
		}
		#endregion Constructor	
	}			
}
