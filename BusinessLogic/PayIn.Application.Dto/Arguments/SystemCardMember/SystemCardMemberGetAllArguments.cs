using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.SystemCardMember
{
    public partial class SystemCardMemberGetAllArguments : IArgumentsBase
    {
        public string Filter { get; set; }
        [Display(Name = "")]
        public int? SystemCardId { get; set; }

        #region Constructors
        public SystemCardMemberGetAllArguments(string filter, int? systemCardId)
        {
            Filter = filter ?? "";
            SystemCardId = systemCardId;
        }
        #endregion Constructors
    }

}

