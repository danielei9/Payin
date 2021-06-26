using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceConcession
{
	public partial class ServiceConcessionGetStateArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public ServiceConcessionGetStateArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
