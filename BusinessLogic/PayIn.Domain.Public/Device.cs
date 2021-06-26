using System;
using Xp.Domain;

namespace PayIn.Domain.Public
{
	public class Device : IEntity
	{
		public int Id { get; set; }
		public string Token { get; set; }
		public DateTime CreatedAt { get; set; }
		public string Login { get; set; }

		#region Platform
		public int PlatformId { get; set; }
		public Platform Platform { get; set; }
		#endregion Platform
	}
}
