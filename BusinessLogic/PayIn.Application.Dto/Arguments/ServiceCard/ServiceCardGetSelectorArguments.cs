using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class ServiceCardGetSelectorArguments : IArgumentsBase
	{
		public string Filter { get; set; }

        #region Constructors
        public ServiceCardGetSelectorArguments(string filter)
		{
			Filter = filter;
        }
		#endregion Constructors
	}
}
