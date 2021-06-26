using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class ServiceCardBatchLockArguments : IArgumentsBase
	{

		public int Id { get; set; }

		#region Constructors
		public ServiceCardBatchLockArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}

