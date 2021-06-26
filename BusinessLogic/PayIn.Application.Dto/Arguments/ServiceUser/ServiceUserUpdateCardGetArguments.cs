using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public class ServiceUserUpdateCardGetArguments : IArgumentsBase
	{
		public int Id { get; set; }
		public long Uid { get; set; }

		#region Constructors
		public ServiceUserUpdateCardGetArguments(int id, long uid)
		{
			Id = id;
			Uid = uid;
		}
		#endregion Constructors
	}
}
