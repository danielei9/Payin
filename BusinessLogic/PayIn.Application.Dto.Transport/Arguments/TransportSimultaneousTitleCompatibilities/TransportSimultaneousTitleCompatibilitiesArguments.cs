using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.TransportSimultaneousTitleCompatibilities
{
	public class TransportSimultaneousTitleCompatibilitiesArguments : IArgumentsBase
	{
        public int TitleId { get; set; }
        public string Filter { get; set; }

        #region Constructors
        public TransportSimultaneousTitleCompatibilitiesArguments(int titleId, string filter)
		{
            TitleId = titleId;
            Filter = filter;
        }
		#endregion Constructors
	}
}
