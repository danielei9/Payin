using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class ServiceCardDestroyArguments : IArgumentsBase
	{
		public int Id	{ get; set; }

		#region Constructors
		public ServiceCardDestroyArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}

