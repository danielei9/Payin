using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class ServiceCardGetAllMyCardsArguments : IArgumentsBase
	{
		public string Filter { get; set; }

		#region Constructors
		public ServiceCardGetAllMyCardsArguments(string filter)
		{
			Filter = filter ?? "";
		}
		#endregion Constructors 
	}
}
