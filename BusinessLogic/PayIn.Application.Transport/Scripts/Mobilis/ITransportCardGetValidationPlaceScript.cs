using PayIn.Domain.Transport.Eige;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts.Mobilis
{
	public interface ITransportCardGetValidationPlaceScript : IMifareClassicScript<EigeCard>
	{
	}
	public static class TransportCardGetHistoricoPlaceScriptExtension
	{
		public static void AddRequest(this ITransportCardGetValidationPlaceScript that)
		{
			that.Read(that.Request, x => x.Historico.TipoValidacion1_Transporte);
			that.Read(that.Request, x => x.Historico.TipoValidacion2_Transporte);
			that.Read(that.Request, x => x.Historico.TipoValidacion3_Transporte);
			that.Read(that.Request, x => x.Historico.TipoValidacion4_Transporte);
			that.Read(that.Request, x => x.Historico.TipoValidacion5_Transporte);
			that.Read(that.Request, x => x.Historico.TipoValidacion6_Transporte);
			that.Read(that.Request, x => x.Historico.TipoValidacion7_Transporte);
			that.Read(that.Request, x => x.Historico.TipoValidacion8_Transporte);
			that.Read(that.Request, x => x.Validacion.TipoValidacion_Transporte);
			that.Read(that.Request, x => x.Historico.VLEPS1_Ferrocarril_Estacion);
			that.Read(that.Request, x => x.Historico.VLEPS2_Ferrocarril_Estacion);
			that.Read(that.Request, x => x.Historico.VLEPS3_Ferrocarril_Estacion);
			that.Read(that.Request, x => x.Historico.VLEPS4_Ferrocarril_Estacion);
			that.Read(that.Request, x => x.Historico.VLEPS5_Ferrocarril_Estacion);
			that.Read(that.Request, x => x.Historico.VLEPS6_Ferrocarril_Estacion);
			that.Read(that.Request, x => x.Historico.VLEPS7_Ferrocarril_Estacion);
			that.Read(that.Request, x => x.Historico.VLEPS8_Ferrocarril_Estacion);
			that.Read(that.Request, x => x.Validacion.VLEPS_Ferrocarril_Estacion);
			that.Read(that.Request, x => x.Historico.VLEPS1_Ferrocarril_Vestibulo);
			that.Read(that.Request, x => x.Historico.VLEPS2_Ferrocarril_Vestibulo);
			that.Read(that.Request, x => x.Historico.VLEPS3_Ferrocarril_Vestibulo);
			that.Read(that.Request, x => x.Historico.VLEPS4_Ferrocarril_Vestibulo);
			that.Read(that.Request, x => x.Historico.VLEPS5_Ferrocarril_Vestibulo);
			that.Read(that.Request, x => x.Historico.VLEPS6_Ferrocarril_Vestibulo);
			that.Read(that.Request, x => x.Historico.VLEPS7_Ferrocarril_Vestibulo);
			that.Read(that.Request, x => x.Historico.VLEPS8_Ferrocarril_Vestibulo);
			that.Read(that.Request, x => x.Validacion.VLEPS_Ferrocarril_Vestibulo);
			that.Read(that.Request, x => x.Historico.VLEPS1_Bus_Linea);
			that.Read(that.Request, x => x.Historico.VLEPS2_Bus_Linea);
			that.Read(that.Request, x => x.Historico.VLEPS3_Bus_Linea);
			that.Read(that.Request, x => x.Historico.VLEPS4_Bus_Linea);
			that.Read(that.Request, x => x.Historico.VLEPS5_Bus_Linea);
			that.Read(that.Request, x => x.Historico.VLEPS6_Bus_Linea);
			that.Read(that.Request, x => x.Historico.VLEPS7_Bus_Linea);
			that.Read(that.Request, x => x.Historico.VLEPS8_Bus_Linea);
			that.Read(that.Request, x => x.Validacion.VLEPS_Bus_Linea);
			that.Read(that.Request, x => x.Historico.VLEPS1_Bus_Convoy);
			that.Read(that.Request, x => x.Historico.VLEPS2_Bus_Convoy);
			that.Read(that.Request, x => x.Historico.VLEPS3_Bus_Convoy);
			that.Read(that.Request, x => x.Historico.VLEPS4_Bus_Convoy);
			that.Read(that.Request, x => x.Historico.VLEPS5_Bus_Convoy);
			that.Read(that.Request, x => x.Historico.VLEPS6_Bus_Convoy);
			that.Read(that.Request, x => x.Historico.VLEPS7_Bus_Convoy);
			that.Read(that.Request, x => x.Historico.VLEPS8_Bus_Convoy);
			that.Read(that.Request, x => x.Validacion.VLEPS_Bus_Convoy);
			that.Read(that.Request, x => x.TituloTuiN.UltimaValidacionEstacion);
		}
	}
}
