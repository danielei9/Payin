using PayIn.Common;

namespace PayIn.Application.Dto.Results.SystemCardMember
{
    public class SystemCardMemberGetAllResult
    {
        public int Id { get; set; }
        public int SystemCardId { get; set; }
        public string SystemCardName { get; set; }
		public SystemCardMemberState State { get; set; }
        // Información comercial
        public string Name { get; set; }
		public string Email { get; set; }
	}
}
