using PayIn.Common;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlPresence
{
	public partial class ControlPresenceCreateArguments : IArgumentsBase
	{
		[Required] public string       Login           { get; private set; }
		[Required] public XpDateTime   Date            { get; private set; }
		[Required] public decimal      Latitude        { get; private set; }
		[Required] public decimal      Longitude       { get; private set; }
		[Required] public decimal      LatitudeWanted  { get; private set; }
		[Required] public decimal      LongitudeWanted { get; private set; }
		[Required] public string       Observations    { get; private set; }
		[Required] public PresenceType PresenceType    { get; private set; }
		[Required] public int          TagId           { get; private set; }
		[Required] public int          ItemId          { get; private set; }

		#region Constructors
		public ControlPresenceCreateArguments(string login, XpDateTime date, decimal latitude, decimal longitude, decimal latitudeWanted, decimal longitudeWanted, string observations, PresenceType presenceType, int tagId, int itemId)
		{
			Login = login;
			Date = date;
			Latitude = latitude;
			Longitude = longitude;
			LatitudeWanted = latitudeWanted;
			LongitudeWanted = longitudeWanted;
			Observations = observations;
			PresenceType = presenceType;
			TagId = tagId;
			ItemId = itemId;
		}
		#endregion Constructors
	}
}
