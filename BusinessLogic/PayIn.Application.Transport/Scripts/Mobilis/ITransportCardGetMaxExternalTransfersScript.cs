using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetMaxExternalTransfersScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class TransportCardGetMaxExternalTransfersScriptExtension
	{
		public static void AddRequest(this ITransportCardGetMaxExternalTransfersScript that)
		{
			that.Read(that.Request, x => x.Titulo.MaxTransbordosExternos1);
			that.Read(that.Request, x => x.Titulo.MaxTransbordosExternos2);
		}
	}
}
