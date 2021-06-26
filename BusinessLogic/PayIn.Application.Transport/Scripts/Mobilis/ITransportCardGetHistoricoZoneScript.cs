using PayIn.Domain.Transport.Eige;
using PayIn.Domain.Transport.MifareClassic.Operations;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetHistoricoZoneScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class TransportCardGetHistoricoZoneScriptExtension
	{
		public static void AddRequest(this ITransportCardGetHistoricoZoneScript that)
		{
			that.Read(that.Request, x => x.Historico.Zona1);
			that.Read(that.Request, x => x.Historico.Zona2);
			that.Read(that.Request, x => x.Historico.Zona3);
			that.Read(that.Request, x => x.Historico.Zona4);
			that.Read(that.Request, x => x.Historico.Zona5);
			that.Read(that.Request, x => x.Historico.Zona6);
			that.Read(that.Request, x => x.Historico.Zona7);
			that.Read(that.Request, x => x.Historico.Zona8);
		}
	}
}
