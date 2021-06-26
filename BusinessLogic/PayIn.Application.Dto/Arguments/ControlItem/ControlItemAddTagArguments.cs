using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlItem
{
	public partial class ControlItemAddTagArguments : IArgumentsBase
	{
		                                                                 [Required] public int Id    { get; private set; }
		[Display(Name="resources.controlItem.tag")]                      [Required] public int TagId { get; private set; }

		#region Constructors
		public ControlItemAddTagArguments(int id, int tagId)
		{
			Id = id;
			TagId = tagId;
		}
		#endregion Constructors
	}
}
