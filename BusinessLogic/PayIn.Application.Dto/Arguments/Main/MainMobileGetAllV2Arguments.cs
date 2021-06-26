using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.Main
{
	public partial class MainMobileGetAllV2Arguments : IArgumentsBase
	{
		public string Filter   { get; private set; }
		public int Skip { get; set; }
		public int Top { get; set; }

		#region Constructors
		public MainMobileGetAllV2Arguments(string filter, int skip, int top) 
		{
			Filter   = filter ?? "";
			Skip = skip;
			Top = top;
		}
		#endregion Constructors
	}
}
