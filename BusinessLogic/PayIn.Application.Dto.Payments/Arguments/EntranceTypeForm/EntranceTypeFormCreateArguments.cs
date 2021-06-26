using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class EntranceTypeFormCreateArguments : IArgumentsBase
    {
        [Required]
        public int EntranceTypeId { get; set; }

        [Display(Name = "resources.entranceTypeForm.formId")]
        [Required]
        public int FormId { get; set; }

        [Display(Name = "resources.entranceTypeForm.order")]
        [Required]
        public int Order { get; set; }

        #region Constructors
        public EntranceTypeFormCreateArguments(int entranceTypeId, int formId, int order)
        {
            EntranceTypeId = entranceTypeId;
            FormId = formId;
            Order = order;
        }
		#endregion Constructors
	}
}
