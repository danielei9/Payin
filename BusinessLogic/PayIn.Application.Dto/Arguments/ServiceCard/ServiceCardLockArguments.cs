using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class ServiceCardLockArguments : IArgumentsBase
	{

		public long Id { get; private set; }
		public long Uid { get; private set; }

		#region Constructors
		public ServiceCardLockArguments(long id, long uid)
		{
			Id = id;
			Uid = uid;
		}
		#endregion Constructors
	}
}

