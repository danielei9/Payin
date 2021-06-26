using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetHistoricoCodeScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class TransportCardGetHistoricoCodeScriptExtension
	{
		public static void AddRequest(this ITransportCardGetHistoricoCodeScript that)
		{
			that.Read(that.Request, x => x.Historico.CodigoTitulo1);
			that.Read(that.Request, x => x.Historico.CodigoTitulo2);
			that.Read(that.Request, x => x.Historico.CodigoTitulo3);
			that.Read(that.Request, x => x.Historico.CodigoTitulo4);
			that.Read(that.Request, x => x.Historico.CodigoTitulo5);
			that.Read(that.Request, x => x.Historico.CodigoTitulo6);
			that.Read(that.Request, x => x.Historico.CodigoTitulo7);
			that.Read(that.Request, x => x.Historico.CodigoTitulo8);
		}
	}
}
