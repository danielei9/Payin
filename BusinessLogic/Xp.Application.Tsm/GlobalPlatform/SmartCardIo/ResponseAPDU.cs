using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xp.Application.Tsm.GlobalPlatform.SmartCardIo
{
	public class ResponseAPDU
	{
		public byte[] Data { get; set; }

		public int Sw { get; set; }

		public byte[] Bytes { get; set; }
	}
}
