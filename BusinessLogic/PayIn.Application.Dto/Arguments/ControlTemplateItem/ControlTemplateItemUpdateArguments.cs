using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlTemplateItem
{
	public partial class ControlTemplateItemUpdateArguments : IArgumentsBase
	{
		                                                                                [Required] public int    Id    { get; private set; }
		[Display(Name="resources.controlTemplateItem.since")] [DataType(DataType.Time)] [Required] public XpTime Since { get; private set; }
		[Display(Name="resources.controlTemplateItem.until")] [DataType(DataType.Time)] [Required] public XpTime Until { get; private set; }

		#region Constructors
		public ControlTemplateItemUpdateArguments(int id, XpTime since, XpTime until)
		{
			Id = id;
			Since = since;
			Until = until;
		}
		#endregion Constructors
	}
}
