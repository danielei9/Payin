using PayIn.Domain.Payments;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class EventIsVisibleArguments : IUpdateArgumentsBase<Event>
	{
		public int Id { get; set; }
		public bool IsVisible { get; set; }

		#region Constructors
		public EventIsVisibleArguments(bool isVisible)
		{
			IsVisible = isVisible;
		}
		#endregion Constructors
	}
}
