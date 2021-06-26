using System.Collections.Generic;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlPresence
{
	public partial class ControlPresenceMobileCheckArguments : IArgumentsBase
	{
		public int							   Id             { get; private set; }
		public XpDateTime					   Date           { get; private set; }
		public byte[]						   Image		  { get; private set; }
		public decimal?						   Latitude		  { get; private set; }
		public decimal?						   Longitude   	  { get; private set; }
		public string						   Observations	  { get; private set; }
		public IEnumerable<ControlPresenceMobileCheckArguments_Item>			   Items          { get; private set; }

		#region Constructors
		public ControlPresenceMobileCheckArguments(int id, XpDateTime date, byte[] image, decimal? latitude, decimal? longitude, string observations,
												   IEnumerable<ControlPresenceMobileCheckArguments_Item> items)		
		{
			Id              = id;
			Date            = date;
			Image           = image;
			Latitude        = latitude;
			Longitude       = longitude;
			Items           = items;
			Observations    = observations ?? "";
		}
		#endregion Constructors
	}
}
