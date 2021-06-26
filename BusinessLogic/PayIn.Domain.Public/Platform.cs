using PayIn.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Public
{
    public class Platform : IEntity 
    {
        public int Id { get; set; }
        public DeviceType Type { get; set; }
        public string PushId {get; set;}
        public string PushCertificate { get; set; }

        #region Devices
		[InverseProperty("Platform")]
        public ICollection<Device> Devices { get; set; }
        #endregion Devices

        #region Constructors
        public Platform()
        {
			Devices = new List<Device>();
        }
        #endregion Constructors
    }
}
