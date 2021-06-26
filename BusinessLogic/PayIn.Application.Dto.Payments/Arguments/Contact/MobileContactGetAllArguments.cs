using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class MobileContactGetAllArguments : IArgumentsBase
	{
		public string Filter { get; set; }

		#region Constructors
		public MobileContactGetAllArguments(string filter)
		{
			Filter = filter;
		}
		#endregion Constructors
	}
}
