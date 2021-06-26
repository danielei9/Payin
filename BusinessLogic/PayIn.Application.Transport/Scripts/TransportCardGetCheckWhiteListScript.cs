using PayIn.Domain.Transport.Eige;
using PayIn.Domain.Transport.MifareClassic.Operations;
using System.Collections.Generic;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts
{
	public class TransportCardGetCheckWhiteListScript : MifareClassicScript<EigeCard>
	{
		#region Constructors
		public TransportCardGetCheckWhiteListScript(string userName, IMifareClassicHsmService hsm)
			:base(new EigeCard(userName, hsm))
		{
			Read(Request, x => x.Validacion.ListaBlanca);
		}
		public TransportCardGetCheckWhiteListScript(string userName, IMifareClassicHsmService hsm, IEnumerable<MifareOperationResultArguments> values)
			: this(userName, hsm)
		{
			Load(values);
		}
		#endregion Constructors
	}
}
