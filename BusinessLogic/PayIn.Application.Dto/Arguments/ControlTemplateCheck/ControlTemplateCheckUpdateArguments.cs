using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;


namespace PayIn.Application.Dto.Arguments.ControlTemplateCheck
{
	public partial class ControlTemplateCheckUpdateArguments : IArgumentsBase
	{
																						  [Required] public int    Id    { get; private set; }
		[Display(Name = "resources.controlTemplateCheck.time")] [DataType(DataType.Time)] [Required] public XpTime Time  { get; private set; }

		#region Constructors
		public ControlTemplateCheckUpdateArguments(int id, XpTime time)
		{
			Id = id;
			Time = time;
		}
		#endregion Constructors
	}
}
