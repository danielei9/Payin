using Xp.Common.Dto.Arguments;
using Xp.Common;

namespace PayIn.Application.Dto.Arguments.ControlTrack
{
	public partial class ControlTrackGetArguments : IArgumentsBase
	{
		public int    Id { get; private set; }
		public XpDateTime Start { get; private set; }
		public XpDateTime End { get; private set; }

		#region Constructors
		public ControlTrackGetArguments(int id,XpDateTime start, XpDateTime end)
		{
			Id = id;
			Start = start == null ? XpDateTime.FromDateTime(new System.DateTime(2015,1,1)) : start;
			End = end == null ? XpDateTime.FromDateTime(System.DateTime.Now) : end;



		}
		#endregion Constructors
	}
}
