using PayIn.Domain.Payments;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class NoticeIsVisibleArguments : IUpdateArgumentsBase<Notice>
	{
		public int Id { get; set; }
		public bool IsVisible { get; set; }

		#region Constructors
		public NoticeIsVisibleArguments(bool isVisible)
		{
			IsVisible = isVisible;
		}
		#endregion Constructors
	}
}
