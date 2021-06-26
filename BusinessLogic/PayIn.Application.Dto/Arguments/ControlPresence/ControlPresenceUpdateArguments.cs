using PayIn.Common;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlPresence
{
	public partial class ControlPresenceUpdateArguments : IArgumentsBase
	{
		public int          Id              { get; private set; }
		public string       Login           { get; private set; }
		public XpDateTime   Date            { get; private set; }
		public decimal      Latitude        { get; private set; }
		public decimal      Longitude       { get; private set; }
		public decimal      LatitudeWanted  { get; private set; }
		public decimal      LongitudeWanted { get; private set; }
		public string       Observations    { get; private set; }
		public PresenceType PresenceType    { get; private set; }

		#region Constructors
		public ControlPresenceUpdateArguments(int id, string login, XpDateTime date, decimal latitude, decimal longitude, decimal latitudeWanted, decimal longitudeWanted, string observations, PresenceType presenceType)
		{
			Id = id;
			Login = login;
			Date = date;
			Latitude = latitude;
			Longitude = longitude;
			LatitudeWanted = latitudeWanted;
			LongitudeWanted = longitudeWanted;
			Observations = observations;
			PresenceType = presenceType;
		}
		#endregion Constructors
	}
}
