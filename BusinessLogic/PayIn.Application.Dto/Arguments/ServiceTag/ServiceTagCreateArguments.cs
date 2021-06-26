using PayIn.Common;
using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceTag
{
	public partial class ServiceTagCreateArguments : ICreateArgumentsBase<PayIn.Domain.Public.ServiceTag>
	{
		[Display(Name = "resources.serviceTag.supplier")]           [Required]      public int            SupplierId { get; private set; }
		[Display(Name = "resources.enumResources.checkPointType_")]	[Required]		public CheckPointType Type       { get; private set; }
		[Display(Name = "resources.serviceTag.reference")]          [Required]      public string         Reference  { get; private set; }

		#region Constructors
		public ServiceTagCreateArguments(int supplierId, CheckPointType type, string reference)
		{
			SupplierId = supplierId;
			Type = type;
			Reference = reference;
		}
		#endregion Constructors
	}
}
