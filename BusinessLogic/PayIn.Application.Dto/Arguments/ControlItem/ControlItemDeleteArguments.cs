using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlItem
{
	public partial class ControlItemDeleteArguments : IDeleteArgumentsBase<PayIn.Domain.Public.ControlItem>
  {
		public int Id { get; set; }

		#region Constructors
		public ControlItemDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
