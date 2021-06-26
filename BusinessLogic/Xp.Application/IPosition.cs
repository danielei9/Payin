using Xp.Common;

namespace Xp.Application
{
	public interface IPosition
	{
		int        Id           { get; set; }
		XpDateTime Date         { get; set; }
		decimal?   Latitude     { get; set; }
		decimal?   Longitude    { get; set; }
		int        Quality      { get; set; }
		XpDuration Elapsed      { get; set; }
		decimal?   Distance     { get; set; }
		decimal?   Velocity     { get; set; }
		decimal?   Acceleration { get; set; }
	}
}
