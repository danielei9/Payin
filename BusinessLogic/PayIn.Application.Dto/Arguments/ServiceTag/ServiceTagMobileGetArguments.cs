using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceTag
{
	public partial class ServiceTagMobileGetArguments : IArgumentsBase
	{
		public string Reference { get; private set; }

		#region Constructors
		public ServiceTagMobileGetArguments(string reference)
		{
			Reference = reference ?? "";
		}
		#endregion Constructors
	}
}
