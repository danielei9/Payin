using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class EntranceTypeFormUpdateArguments : IArgumentsBase
	{
		[JsonIgnore]
		public int Id { get; set; }

		[Display(Name = "resources.entranceTypeForm.order")]
		public int Order { get; set; }

		public int EntranceTypeId { get; set; }

		#region Constructors
		public EntranceTypeFormUpdateArguments(int id, int order, int entranceTypeId)
		{
			Id = id;
			Order = order;
			EntranceTypeId = entranceTypeId;
		}
		#endregion Constructors
	}
}
