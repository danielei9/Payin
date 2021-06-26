using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class MobileNoticeGetAllByEventArguments : IArgumentsBase
	{
		public int Id { get; set; }
		public string Filter { get; set; }

		#region Constructors
		public MobileNoticeGetAllByEventArguments(string filter, int id)
		{
			Id = id;
			Filter = filter;
		}
		#endregion Constructors
	}
}
