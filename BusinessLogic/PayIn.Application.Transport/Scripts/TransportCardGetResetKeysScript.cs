using PayIn.Domain.Transport.Eige;
using PayIn.Domain.Transport.MifareClassic.Operations;
using PayIn.Domain.Transport.MifareClassic.Services;
using System.Collections.Generic;

namespace PayIn.Application.Transport.Scripts
{
	public class TransportCardGetResetKeysScript : MifareClassicScript<EigeCard>
	{
		#region Constructors
		public TransportCardGetResetKeysScript(string userName, IMifareClassicHsmService hsm)
			:base(new EigeCard(userName, hsm))
		{
			Read(Request, x => x.Key00A);
			Read(Request, x => x.Key00B);
			Read(Request, x => x.Key01A);
			Read(Request, x => x.Key01B);
			Read(Request, x => x.Key02A);
			Read(Request, x => x.Key02B);
			Read(Request, x => x.Key03A);
			Read(Request, x => x.Key03B);
			Read(Request, x => x.Key04A);
			Read(Request, x => x.Key04B);
			Read(Request, x => x.Key05A);
			Read(Request, x => x.Key05B);
			Read(Request, x => x.Key06A);
			Read(Request, x => x.Key06B);
			Read(Request, x => x.Key07A);
			Read(Request, x => x.Key07B);
			Read(Request, x => x.Key08A);
			Read(Request, x => x.Key08B);
			Read(Request, x => x.Key09A);
			Read(Request, x => x.Key09B);
			Read(Request, x => x.Key10A);
			Read(Request, x => x.Key10B);
			Read(Request, x => x.Key11A);
			Read(Request, x => x.Key11B);
			Read(Request, x => x.Key12A);
			Read(Request, x => x.Key12B);
			Read(Request, x => x.Key13A);
			Read(Request, x => x.Key13B);
			Read(Request, x => x.Key14A);
			Read(Request, x => x.Key14B);
			Read(Request, x => x.Key15A);
			Read(Request, x => x.Key15B);

			// Response in handler
		}
		public TransportCardGetResetKeysScript(string userName, IMifareClassicHsmService hsm, IEnumerable<MifareOperationResultArguments> values)
			: this(userName, hsm)
		{
			this.Load(values);
		}
		#endregion Constructors
	}
}
