using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetValidationTypeNameScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class TransportCardGetHistoricoTypeNameScriptExtension
	{
		public static void AddRequest(this ITransportCardGetValidationTypeNameScript that)
		{
			that.Read(that.Request, x => x.Historico.TipoValidacion1_Transbordo);
			that.Read(that.Request, x => x.Historico.TipoValidacion2_Transbordo);
			that.Read(that.Request, x => x.Historico.TipoValidacion3_Transbordo);
			that.Read(that.Request, x => x.Historico.TipoValidacion4_Transbordo);
			that.Read(that.Request, x => x.Historico.TipoValidacion5_Transbordo);
			that.Read(that.Request, x => x.Historico.TipoValidacion6_Transbordo);
			that.Read(that.Request, x => x.Historico.TipoValidacion7_Transbordo);
			that.Read(that.Request, x => x.Historico.TipoValidacion8_Transbordo);
			that.Read(that.Request, x => x.Validacion.TipoValidacion_Transbordo);
			that.Read(that.Request, x => x.TituloTuiN.UltimaValidacionTipo_Transbordo);

			that.Read(that.Request, x => x.Historico.TipoValidacion1_Sentido);
			that.Read(that.Request, x => x.Historico.TipoValidacion2_Sentido);
			that.Read(that.Request, x => x.Historico.TipoValidacion3_Sentido);
			that.Read(that.Request, x => x.Historico.TipoValidacion4_Sentido);
			that.Read(that.Request, x => x.Historico.TipoValidacion5_Sentido);
			that.Read(that.Request, x => x.Historico.TipoValidacion6_Sentido);
			that.Read(that.Request, x => x.Historico.TipoValidacion7_Sentido);
			that.Read(that.Request, x => x.Historico.TipoValidacion8_Sentido);
			that.Read(that.Request, x => x.Validacion.TipoValidacion_Sentido);
			that.Read(that.Request, x => x.TituloTuiN.UltimaValidacionTipo_Sentido);
		}
	}
}
