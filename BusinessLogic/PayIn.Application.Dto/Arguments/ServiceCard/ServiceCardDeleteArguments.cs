using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class ServiceCardDeleteArguments : IArgumentsBase
	{
		public int Id	{ get; set; }

		#region Constructors
		public ServiceCardDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
