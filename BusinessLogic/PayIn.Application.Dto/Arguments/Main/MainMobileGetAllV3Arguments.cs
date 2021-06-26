using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.Main
{
	public partial class MainMobileGetAllV3Arguments : IArgumentsBase
	{
		public string Filter   { get; private set; }
		public int Skip { get; set; }
		public int Top { get; set; }

		#region Constructors
		public MainMobileGetAllV3Arguments(string filter, int skip, int top) 
		{
			Filter   = filter ?? "";
			Skip = skip;
			Top = top;
		}
		#endregion Constructors
	}
}
