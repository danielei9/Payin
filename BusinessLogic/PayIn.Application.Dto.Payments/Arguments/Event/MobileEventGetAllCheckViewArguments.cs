using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class MobileEventGetAllCheckViewArguments: IArgumentsBase
	{
		public string Filter { get; set; }

        #region Constructors
        public MobileEventGetAllCheckViewArguments(string filter)
		{
			Filter = filter;
        }
        #endregion Constructors
    }
}
