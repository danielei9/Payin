using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class ServiceCardUnlockArguments : IArgumentsBase
	{
		public int Id	{ get; set; }

		#region Constructors
		public ServiceCardUnlockArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}

