using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class MobileExhibitorGetVisitorArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public MobileExhibitorGetVisitorArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
