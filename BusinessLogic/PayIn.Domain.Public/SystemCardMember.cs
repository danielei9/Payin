using PayIn.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Public
{
	public class SystemCardMember : Entity
    {
                   public SystemCardMemberState State { get; set; } = SystemCardMemberState.Active;
		[Required] public string Name { get; set; }
		[Required] public string Login { get; set; }
		           public bool CanEmit { get; set; }

        #region SystemCard
        public int SystemCardId { get; set; }
		[ForeignKey("SystemCardId")]
		public SystemCard SystemCard { get; set; }
		#endregion SystemCard
	}
}
