using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.SystemCardMember
{
    public partial class SystemCardMemberGetArguments : IArgumentsBase
    {
        public int Id { get; set; }

        #region Constructors
        public SystemCardMemberGetArguments(int id)
        {
            Id = id;
        }
        #endregion Constructors
    }

}

