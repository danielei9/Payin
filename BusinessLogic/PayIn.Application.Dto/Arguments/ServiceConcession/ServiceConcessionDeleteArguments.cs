using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceConcession
{
	public partial class ServiceConcessionDeleteArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public ServiceConcessionDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
