using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class ProductAddGroupArguments : IArgumentsBase
	{
		public int Id { get; set; }
		[Display(ResourceType=typeof(ProductResources), Name="Group")]
		public int GroupId { get; set; }

		#region Constructors
		public ProductAddGroupArguments(int groupId)
		{
			GroupId = groupId;
		}
		#endregion Constructors
	}
}
