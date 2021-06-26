using PayIn.Common;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
    public class ServiceCardCreateArguments : IArgumentsBase
	{
		[Display(Name = "resources.serviceCard.batch")]				[Required]	public int ServiceCardBatchId { get; set; }
		[Display(Name = "resources.serviceCard.uidformat")]			[Required]	public UidFormat UidFormat { get; set; }
		[Display(Name = "resources.serviceCard.uids")]							public string Uids { get; set; }

		#region Constructors
		public ServiceCardCreateArguments(int serviceCardBatchId, UidFormat uidFormat, string uids)
		{
			ServiceCardBatchId = serviceCardBatchId;
			UidFormat = uidFormat;
			Uids = uids;
		}
		#endregion Constructors
	}
}
