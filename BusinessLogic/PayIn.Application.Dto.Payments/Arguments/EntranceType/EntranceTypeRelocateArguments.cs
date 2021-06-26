using Xp.Common.Dto.Arguments;
using System.ComponentModel.DataAnnotations;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class EntranceTypeRelocateArguments : IArgumentsBase
	{
        [Display(Name = "resources.entranceType.name")] public int Id    { set; get; }
        [Display(Name = "resources.entranceType.name")] public int OldId { set; get; }

        #region Constructor
        public EntranceTypeRelocateArguments(int id , int oldId)
		{
			Id = id;
            OldId = oldId;
		}
		#endregion Constructor
	}
}
