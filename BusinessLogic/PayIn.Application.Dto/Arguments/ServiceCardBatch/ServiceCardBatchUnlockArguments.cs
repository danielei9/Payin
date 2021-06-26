using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class ServiceCardBatchUnlockArguments : IArgumentsBase
	{
		public int Id	{ get; set; }

		#region Constructors
		public ServiceCardBatchUnlockArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}

