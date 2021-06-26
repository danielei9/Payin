using PayIn.Common;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public class ServiceUserUpdateCardArguments : IArgumentsBase
	{
		public int Id { get; set; }
		[Display(Name = "resources.serviceUser.uid")]
		[Required]
		public long Uid { get; set; }
		[Display(Name = "resources.serviceUser.cardState")]
		public ServiceCardState CardState { get; set; }

		#region Constructors
		public ServiceUserUpdateCardArguments(int id, long uid, ServiceCardState cardState)
		{
			Id = id;
			Uid = uid;
			CardState = cardState;
		}
		#endregion Constructors
	}
}
