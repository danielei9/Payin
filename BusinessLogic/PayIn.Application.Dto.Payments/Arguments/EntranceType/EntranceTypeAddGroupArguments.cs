using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class EntranceTypeAddGroupArguments : IArgumentsBase
	{
		public int Id { get; set; }
		[Display(ResourceType = typeof(EntranceTypeResources), Name = "Group")]
		public int GroupId { get; set; }

		#region Constructors
		public EntranceTypeAddGroupArguments(int id, int groupId)
		{
			Id = id;
			GroupId = groupId;
		}
		#endregion Constructors
	}
}
