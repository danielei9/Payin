using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlItem
{
	public partial class ControlItemRemoveTagArguments : IArgumentsBase
	{
		[Required] public int Id    { get; private set; }
		[Required] public int TagId { get; private set; }

		#region Constructors
		public ControlItemRemoveTagArguments(int id, int tagId)
		{
			Id = id;
			TagId = tagId;
		}
		#endregion Constructors
	}
}
