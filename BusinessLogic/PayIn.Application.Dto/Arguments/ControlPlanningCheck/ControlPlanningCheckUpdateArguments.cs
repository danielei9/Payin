using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlPlanningCheck
{
	public partial class ControlPlanningCheckUpdateArguments : IArgumentsBase
	{
		                                                      [Required] public int        Id   { get; private set; }
		[Display(Name="resources.controlPlanningCheck.date")] [Required] public XpDateTime Date { get; private set; }

		#region Constructors
		public ControlPlanningCheckUpdateArguments(int id, XpDateTime date)
		{
			Id = id;
			Date = date;
		}
		#endregion Constructors
	}
}
