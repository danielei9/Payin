using PayIn.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public partial class EventGetVisibilityResult
	{
		public int Id { get; set; }
		public EventVisibility Visibility { get; set; }
	}
}
