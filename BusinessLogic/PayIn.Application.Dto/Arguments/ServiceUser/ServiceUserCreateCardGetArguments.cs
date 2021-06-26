using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public class ServiceUserCreateCardGetArguments : IArgumentsBase
	{
		public long Uid { get; set; }
		public long SystemCardId { get; set; }

		#region Constructors
		public ServiceUserCreateCardGetArguments(long uid, int systemCardId)
		{
			Uid = uid;
			SystemCardId = systemCardId;
		}
		#endregion Constructors
	}
}
