using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Bus.Arguments
{
	public class BusMobileRequestCreateArguments : IArgumentsBase
	{
		                                                              public int LineId { get; set; }
		[Required] [Display(Name = "resources.bus.request.from")]     public int FromId { get; set; }
		[Required] [Display(Name = "resources.bus.request.to")]	      public int ToId { get; set; }

		#region Constructors
		public BusMobileRequestCreateArguments(int fromId, int toId)
		{
			FromId = fromId;
			ToId = toId;
		}
		#endregion Constructors
	}
}
