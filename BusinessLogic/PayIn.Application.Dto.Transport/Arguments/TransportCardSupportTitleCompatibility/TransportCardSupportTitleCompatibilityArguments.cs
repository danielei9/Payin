using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.TransportCardSupportTitleCompatibility
{
	public class TransportCardSupportTitleCompatibilityArguments : IArgumentsBase
	{
        public int TitleId { get; set; }
        public string Filter { get; set; }

        #region Constructors
        public TransportCardSupportTitleCompatibilityArguments(int titleId, string filter)
		{
            TitleId = titleId;
            Filter = filter;
        }
		#endregion Constructors
	}
}
