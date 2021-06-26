using PayIn.Common;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class EventVisibilityArguments : IArgumentsBase
	{
														public int					Id			{ get; set; }
		[Display(Name = "resources.event.visibility")]	public EventVisibility		Visibility	{ get; set; }

		#region Constructors
		public EventVisibilityArguments(int id, EventVisibility visibility)
		{
			Id = id;
			Visibility = visibility;
		}
		#endregion Constructors
	}
}
