using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Bus.Arguments
{
	public class BusApiRequestCreateArguments : IArgumentsBase
	{
		                                                              public int LineId { get; set; }
		[Required] [Display(Name = "resources.bus.request.from")]     public int FromId { get; set; }
		[Required] [Display(Name = "resources.bus.request.to")]	      public int ToId { get; set; }
		           [Display(Name = "resources.bus.request.userName")] public string UserName { get; set; }

		#region Constructors
		public BusApiRequestCreateArguments(string userName, int fromId, int toId)
		{
			this.UserName = userName ?? "";
			this.FromId = fromId;
			this.ToId = toId;
		}
		#endregion Constructors
	}
}
