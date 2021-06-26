using PayIn.Domain.Transport;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.TransportCard
{
	public partial class TransportCardCreateArguments : IArgumentsBase
	{
		[Required] public DeviceTypeEnum DeviceType { get; set; }
		           public string DeviceAddress { get; set; }
		           public string Name { get; set; }
		           public int? Entry { get; set; }
		           public string RandomId { get; set; }
		[Required] public long Uid { get; set; }
		           public string Login { get; set; }

		#region Constructors
		public TransportCardCreateArguments(DeviceTypeEnum deviceType, string deviceAddress, string name, int? entry, string randomId, long uid, string login)
		{
			DeviceType = deviceType;
			DeviceAddress = deviceAddress;
			Name = name;
			Entry = entry;
			RandomId = randomId;
			Uid = uid;
			Login = login;
		}
		#endregion Constructors
	}
}