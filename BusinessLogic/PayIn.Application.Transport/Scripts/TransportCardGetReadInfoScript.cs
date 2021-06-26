using PayIn.Application.Transport.Scripts.Mobilis;
using PayIn.Domain.Transport.Eige;
using PayIn.Domain.Transport.MifareClassic.Operations;
using System.Collections;
using System.Collections.Generic;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Scripts
{
	public class TransportCardGetReadInfoScript : MifareClassicScript<EigeCard>,
		ITransportCardIsRechargableScript,
		ITransportCardCheckBlackListScript,
		ITransportCardCheckGreyListScript,
		ITransportCardCheckExhaustedScript,
		ITransportCardGetCardOwnerNameScript,
		ITransportCardGetTitleCodeScript,
		ITransportCardGetTitleNameScript,
		ITransportCardGetTitleOwnerNameScript,
		ITransportCardGetCardTypeNameScript,
		ITransportCardGetExpiredDateScript,
		ITransportCardGetExhaustedDateScript,
		ITransportCardGetActivatedDateScript,
		ITransportCardGetIsTemporalScript,
		ITransportCardGetHasBalanceScript,
		ITransportCardGetHasTariffScript,
		ITransportCardGetHasHourValidityScript,
		ITransportCardGetHasDayValidityScript,
		ITransportCardCheckWhiteListScript,
		ITransportCardGetIsPersonalizedScript,
		ITransportCardGetUserCodeScript,
		ITransportCardGetUserNameScript,
		ITransportCardGetUserSurnameScript,
		ITransportCardGetUserDniScript,
		ITransportCardGetAmpliationScript,
		ITransportCardGetTitleZoneNameScript,
		ITransportCardGetTitleCaducityScript,
		ITransportCardGetIsExhaustedScript,
		ITransportCardGetMaxExternalTransfersScript,
		ITransportCardGetMaxPeopleInTransferScript,
		ITransportCardGetBalanceScript,
		ITransportCardGetBalanceUnitsScript,
		ITransportCardGetAmpliationQuantityScript,
		ITransportCardGetAmpliationUnitsScript,
		ITransportCardGetValidationTypeNameScript,
		ITransportCardGetHistoricoIndiceScript,
		ITransportCardGetHistoricoTiene8Script,
		ITransportCardGetHistoricoCodeScript,
		ITransportCardGetHistoricoZoneScript,
		ITransportCardGetHistoricoQuantityScript,
		ITransportCardGetValidationPlaceScript,
		ITransportCardGetValidationOperatorScript,
		ITransportCardGetCargaDateScript,
		ITransportCardGetCargaTypeNameScript,
		ITransportCardGetValidationDateScript,
		ITransportCardGetValidationPeopleTravelingScript,
		ITransportCardGetValidationPeopleInTransferScript,
		ITransportCardGetValidationInternalTransfersScript,
		ITransportCardGetValidationExternalTransfersScript,
		ITransportCardGetValidationMaxInternalTransfersScript,
		ITransportCardGetValidationMaxExternalTransfersScript
	{
		#region Constructors
		public TransportCardGetReadInfoScript(string userName, IMifareClassicHsmService hsm)
			: base(new EigeCard(userName, hsm))
		{
			for (byte sector = 0; sector < 16; sector++)
				for (byte block = 0; block < 3; block++)
					Read(Request, sector, block);

			//Read(Request, x => x.Tarjeta.EmpresaPropietaria);
			//Read(Request, x => x.Tarjeta.Tipo);
			//Read(Request, x => x.Tarjeta.CodigoEntorno);
			//Read(Request, x => x.Tarjeta.Subtipo);
			//Read(Request, x => x.Tarjeta.Caducidad);
			//Read(Request, x => x.Usuario.Tiene8Historicos);
			//Read(Request, x => x.Validacion.FechaValidacion);
			//Read(Request, x => x.Validacion.TipoValidacion);
			//Read(Request, x => x.Validacion.VLEPS);
			//Read(Request, x => x.Validacion.NumeroPersonasViaje);
			//Read(Request, x => x.Validacion.NumeroPersonasTrasbordo);
			//Read(Request, x => x.Validacion.TiempoRestanteTransbordo);
			//Read(Request, x => x.Validacion.ContadorTransbordosInternos);
			//Read(Request, x => x.Validacion.ContadorTransbordosExternos);
			//Read(Request, x => x.Validacion.MaxTransbordosInternos);
			//Read(Request, x => x.Personalizacion.TipoOperacion);
			//Read(Request, x => x.Personalizacion.FechaPersonalizacion_Dispositivo);
			//Read(Request, x => x.Historico.IndiceTransaccion);
			//Read(Request, x => x.Titulo.TitulosActivos);
			//Read(Request, x => x.Carga.PosicionUltima);
			//// Titulo 1
			//Read(Request, x => x.Titulo.CodigoTitulo1);
			//Read(Request, x => x.Titulo.EmpresaPropietaria1);
			//Read(Request, x => x.Titulo.ValidezZonal1);
			//Read(Request, x => x.Titulo.FechaValidez1);
			//Read(Request, x => x.Titulo.SaldoViaje1);
			//Read(Request, x => x.Titulo.MaxTransbordosExternos1);
			//Read(Request, x => x.Titulo.MaxPersonasEnTransbordo1);
			//Read(Request, x => x.Titulo.MaxTiempoTransbordo1);
			//// Titulo 2
			//Read(Request, x => x.Titulo.CodigoTitulo2);
			//Read(Request, x => x.Titulo.EmpresaPropietaria2);
			//Read(Request, x => x.Titulo.ValidezZonal2);
			//Read(Request, x => x.Titulo.FechaValidez2);
			//Read(Request, x => x.Titulo.SaldoViaje2);
			//Read(Request, x => x.Titulo.MaxTransbordosExternos2);
			//Read(Request, x => x.Titulo.MaxPersonasEnTransbordo2);
			//Read(Request, x => x.Titulo.MaxTiempoTransbordo2);
			//// Monedero
			//Read(Request, x => x.Titulo.SaldoMonedero);
			//Read(Request, x => x.Titulo.EmpresaPropietariaM);
			//// Bonus
			//Read(Request, x => x.Titulo.SaldoBonus);
			//Read(Request, x => x.Titulo.EmpresaPropietariaB);
			//// Historico1
			//Read(Request, x => x.Historico.FechaHora1);
			//Read(Request, x => x.Historico.TipoValidacion1);
			//Read(Request, x => x.Historico.CodigoTitulo1);
			//Read(Request, x => x.Historico.Zona1);
			//Read(Request, x => x.Historico.UnidadesConsumidas1);
			//Read(Request, x => x.Historico.VLEPS1);
			//// Historico2
			//Read(Request, x => x.Historico.FechaHora2);
			//Read(Request, x => x.Historico.TipoValidacion2);
			//Read(Request, x => x.Historico.CodigoTitulo2);
			//Read(Request, x => x.Historico.Zona2);
			//Read(Request, x => x.Historico.UnidadesConsumidas2);
			//Read(Request, x => x.Historico.VLEPS2);
			//// Historico3
			//Read(Request, x => x.Historico.FechaHora3);
			//Read(Request, x => x.Historico.TipoValidacion3);
			//Read(Request, x => x.Historico.CodigoTitulo3);
			//Read(Request, x => x.Historico.Zona3);
			//Read(Request, x => x.Historico.UnidadesConsumidas3);
			//Read(Request, x => x.Historico.VLEPS3);
			//// Historico4
			//Read(Request, x => x.Historico.FechaHora4);
			//Read(Request, x => x.Historico.TipoValidacion4);
			//Read(Request, x => x.Historico.CodigoTitulo4);
			//Read(Request, x => x.Historico.Zona4);
			//Read(Request, x => x.Historico.UnidadesConsumidas4);
			//Read(Request, x => x.Historico.VLEPS4);
			//Read(Request, x => x.Historico.NumeroMaquina4);
			//// Historico5
			//Read(Request, x => x.Historico.FechaHora5);
			//Read(Request, x => x.Historico.TipoValidacion5);
			//Read(Request, x => x.Historico.CodigoTitulo5);
			//Read(Request, x => x.Historico.Zona5);
			//Read(Request, x => x.Historico.UnidadesConsumidas5);
			//Read(Request, x => x.Historico.VLEPS5);
			//// Historico6
			//Read(Request, x => x.Historico.FechaHora6);
			//Read(Request, x => x.Historico.TipoValidacion6);
			//Read(Request, x => x.Historico.CodigoTitulo6);
			//Read(Request, x => x.Historico.Zona6);
			//Read(Request, x => x.Historico.UnidadesConsumidas6);
			//Read(Request, x => x.Historico.VLEPS6);
			//// Historico7
			//Read(Request, x => x.Historico.FechaHora7);
			//Read(Request, x => x.Historico.TipoValidacion7);
			//Read(Request, x => x.Historico.CodigoTitulo7);
			//Read(Request, x => x.Historico.Zona7);
			//Read(Request, x => x.Historico.UnidadesConsumidas7);
			//Read(Request, x => x.Historico.VLEPS7);
			//// Historico8
			//Read(Request, x => x.Historico.FechaHora8);
			//Read(Request, x => x.Historico.TipoValidacion8);
			//Read(Request, x => x.Historico.CodigoTitulo8);
			//Read(Request, x => x.Historico.Zona8);
			//Read(Request, x => x.Historico.UnidadesConsumidas8);
			//Read(Request, x => x.Historico.VLEPS8);
			//// Carga1
			//Read(Request, x => x.Carga.Fecha1);
			//Read(Request, x => x.Carga.TipoOperacion1_Operacion);
			//Read(Request, x => x.Carga.Importe1);
			//// Carga2
			//Read(Request, x => x.Carga.Fecha2);
			//Read(Request, x => x.Carga.TipoOperacion2_Operacion);
			//Read(Request, x => x.Carga.Importe2);

			// Para poder recargar sin pegar otro tiro
			//Request.AddRange(new TransportCardGetRechargeScript(userName, hsm).Request);
			//(this as ITransportCardIsRechargableScript).AddRequest();
			//(this as ITransportCardCheckBlackListScript).AddRequest();
			//(this as ITransportCardCheckGreyListScript).AddRequest();
			//(this as ITransportCardCheckExhaustedScript).AddRequest();
			//(this as ITransportCardGetCardOwnerNameScript).AddRequest();
			//(this as ITransportCardGetTitleCodeScript).AddRequest();
			//(this as ITransportCardGetTitleNameScript).AddRequest();
			//(this as ITransportCardGetTitleOwnerNameScript).AddRequest();
			//(this as ITransportCardGetCardTypeNameScript).AddRequest();
			//(this as ITransportCardGetExpiredDateScript).AddRequest();
			//(this as ITransportCardGetExhaustedDateScript).AddRequest();
			//(this as ITransportCardGetActivatedDateScript).AddRequest();
			//(this as ITransportCardGetIsTemporalScript).AddRequest();
			//(this as ITransportCardGetHasBalanceScript).AddRequest();
			//(this as ITransportCardGetHasTariffScript).AddRequest();
			//(this as ITransportCardCheckWhiteListScript).AddRequest();
			//(this as ITransportCardGetIsPersonalizedScript).AddRequest();
			//(this as ITransportCardGetUserCodeScript).AddRequest();
			//(this as ITransportCardGetUserNameScript).AddRequest();
			//(this as ITransportCardGetUserSurnameScript).AddRequest();
			//(this as ITransportCardGetUserDniScript).AddRequest();
			//(this as ITransportCardGetAmpliationScript).AddRequest();
			//(this as ITransportCardGetTitleZoneNameScript).AddRequest();
			//(this as ITransportCardGetTitleCaducityScript).AddRequest();
			//(this as ITransportCardGetMaxExternalTransfersScript).AddRequest();
			//(this as ITransportCardGetMaxPeopleInTransferScript).AddRequest();
			//(this as ITransportCardGetBalanceScript).AddRequest();
			//(this as ITransportCardGetBalanceUnitsScript).AddRequest();
			//(this as ITransportCardGetAmpliationQuantityScript).AddRequest();
			//(this as ITransportCardGetAmpliationUnitsScript).AddRequest();
			//(this as ITransportCardGetValidationTypeNameScript).AddRequest();
			//(this as ITransportCardGetHistoricoIndiceScript).AddRequest();
			//(this as ITransportCardGetHistoricoTiene8Script).AddRequest();
			//(this as ITransportCardGetHistoricoCodeScript).AddRequest();
			//(this as ITransportCardGetHistoricoZoneScript).AddRequest();
			//(this as ITransportCardGetHistoricoQuantityScript).AddRequest();
			//(this as ITransportCardGetValidationOperatorScript).AddRequest();
			//(this as ITransportCardGetValidationPlaceScript).AddRequest();
			//(this as ITransportCardGetCargaDateScript).AddRequest();
			//(this as ITransportCardGetCargaTypeNameScript).AddRequest();
			//(this as ITransportCardGetValidationDateScript).AddRequest();
			//(this as ITransportCardGetValidationPeopleTravelingScript).AddRequest();
			//(this as ITransportCardGetValidationPeopleInTransferScript).AddRequest();
			//(this as ITransportCardGetValidationInternalTransfersScript).AddRequest();
			//(this as ITransportCardGetValidationExternalTransfersScript).AddRequest();
			//(this as ITransportCardGetValidationMaxInternalTransfersScript).AddRequest();
			//(this as ITransportCardGetValidationMaxExternalTransfersScript).AddRequest();

			////ITransportCardGetIsExhaustedScript
			//var interfaces = this.GetType().GetInterfaces();
			//foreach(var _interface in interfaces)
			//{
			//	var reads = _interface.GetMethods(System.Reflection.BindingFlags.Static);
			//}
			////foreach(var interface in this.)

			//// No script response
		}
		public TransportCardGetReadInfoScript(string userName, IMifareClassicHsmService hsm, IEnumerable<MifareOperationResultArguments> values)
				: this(userName, hsm)
		{
			Load(values);
		}


		public TransportCardGetReadInfoScript(string userName, IMifareClassicHsmService hsm, MifareClassicScript<EigeCard> values)
				: this(userName, hsm)
		{
			Load(values);
		}

		#endregion Constructors
	}
}