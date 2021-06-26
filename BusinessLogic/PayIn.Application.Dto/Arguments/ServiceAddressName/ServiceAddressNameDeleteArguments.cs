using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceAddressName
{
	public partial class ServiceAddressNameDeleteArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public ServiceAddressNameDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
