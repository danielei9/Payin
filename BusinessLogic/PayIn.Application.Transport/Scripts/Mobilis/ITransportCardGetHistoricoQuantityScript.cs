using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetHistoricoQuantityScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class TransportCardGetHistoricoQuantityScriptExtension
	{
		public static void AddRequest(this ITransportCardGetHistoricoQuantityScript that)
		{
			that.Read(that.Request, x => x.Historico.UnidadesConsumidas1);
			that.Read(that.Request, x => x.Historico.UnidadesConsumidas2);
			that.Read(that.Request, x => x.Historico.UnidadesConsumidas3);
			that.Read(that.Request, x => x.Historico.UnidadesConsumidas4);
			that.Read(that.Request, x => x.Historico.UnidadesConsumidas5);
			that.Read(that.Request, x => x.Historico.UnidadesConsumidas6);
			that.Read(that.Request, x => x.Historico.UnidadesConsumidas7);
			that.Read(that.Request, x => x.Historico.UnidadesConsumidas8);
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
