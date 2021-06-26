using PayIn.Common;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
    public class ServiceCardBatchCreateArguments : IArgumentsBase
	{
		[Display(Name = "resources.serviceCardBatch.name")]				[Required]	public string Name { get; set; }
		[Display(Name = "resources.serviceCardBatch.uidformat")]		[Required]	public UidFormat UidFormat { get; set; }
		[Display(Name = "resources.serviceCardBatch.systemcard")]		[Required]	public int SystemCardId { get; set; }		

		#region Constructors
		public ServiceCardBatchCreateArguments(string name, UidFormat uidFormat, int systemCardId)
		{
			Name = name;
			UidFormat = uidFormat;
			SystemCardId = systemCardId;
		}
		#endregion Constructors
	}
}
