using PayIn.Domain.Transport.Eige.Enums;
using PayIn.Domain.Transport.Eige.Types;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Domain.Transport.Eige
{
	public class EigeHistorico : MifareClassicElement
	{
		[MifareClassicMemory("HITA",    1, 1, 118, 120)] public EigeInt8 IndiceTransaccion { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		// Historico 1
		[MifareClassicMemory("H1VTV",   4, 2,   1,   4)] public EigeBytes TipoValidacion1 { get { return Get<EigeBytes> (); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        4, 2,   1,   1)] public GenericEnum<EigeTipoValidacion_SentidoEnum> TipoValidacion1_Sentido { get { return Get<GenericEnum<EigeTipoValidacion_SentidoEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        4, 2,   2,   2)] public GenericEnum<EigeTipoValidacion_TransporteEnum> TipoValidacion1_Transporte { get { return Get<GenericEnum<EigeTipoValidacion_TransporteEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        4, 2,   3,   3)] public GenericEnum<EigeTipoValidacion_TransbordoEnum> TipoValidacion1_Transbordo { get { return Get<GenericEnum<EigeTipoValidacion_TransbordoEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        4, 2,   4,   4)] public GenericEnum<EigeTipoValidacion_ViajeroEnum> TipoValidacion1_Viajero { get { return Get<GenericEnum<EigeTipoValidacion_ViajeroEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H1VM",    4, 2,   5,   8)] public EigeInt8 ValidacionMultiple1 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H1VMB",   4, 2,   9,  10)] public GenericEnum<EigeMonederoBonusEnum> UsoMonederoBonus1 { get { return Get<GenericEnum<EigeMonederoBonusEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H1VCT",   4, 2,  11,  22)] public EigeInt16 CodigoTitulo1 { get { return Get<EigeInt16>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H1VFH",   4, 2,  23,  47)] public EigeDateTime FechaHora1 { get { return Get<EigeDateTime>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H1VEO",   4, 2,  48,  52)] public EigeInt8 EmpresaOperadora1 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H1VZV",   4, 2,  53,  55)] public GenericEnum<EigeZonaHistoricoEnum> Zona1 { get { return Get<GenericEnum<EigeZonaHistoricoEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H1VLEPS", 4, 2,  56,  68)] public EigeInt16 VLEPS1 { get { return Get<EigeInt16>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        4, 2,  56,  63)] public EigeInt8 VLEPS1_Ferrocarril_Estacion { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        4, 2,  64,  65)] public EigeInt8 VLEPS1_Ferrocarril_Vestibulo { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        4, 2,  56,  62)] public EigeInt8 VLEPS1_Bus_Linea { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        4, 2,  63,  68)] public EigeInt8 VLEPS1_Bus_Convoy { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        4, 2,  56,  62)] public EigeInt8 VLEPS1_Autobus_Linea { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        4, 2,  63,  67)] public EigeInt8 VLEPS1_Autobus_Parada { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        4, 2,  68,  68)] public EigeInt8 VLEPS1_Autobus_Sentido { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H1VMQ",   4, 2,  69,  74)] public EigeInt8 NumeroMaquina1 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H1VLNE",  4, 2,  75,  87)] public EigeBytes NumeroEquipo1 { get { return Get<EigeBytes>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H1VLUCV", 4, 2,  88,  99)] public EigeInt16 UnidadesConsumidas1 { get { return Get<EigeInt16>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H1VAVE",  4, 2, 100, 107)] public EigeInt8 AcumuladoValidacionesEntrada1 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H1VAVS",  4, 2, 108, 115)] public EigeInt8 AcumuladoValidacionesSalida1 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		// Historico 2
		[MifareClassicMemory("H2VTV",   7, 0,   1,   4)] public EigeBytes TipoValidacion2 { get { return Get<EigeBytes> (); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        7, 0,   1,   1)] public GenericEnum<EigeTipoValidacion_SentidoEnum> TipoValidacion2_Sentido { get { return Get<GenericEnum<EigeTipoValidacion_SentidoEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        7, 0,   2,   2)] public GenericEnum<EigeTipoValidacion_TransporteEnum> TipoValidacion2_Transporte { get { return Get<GenericEnum<EigeTipoValidacion_TransporteEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        7, 0,   3,   3)] public GenericEnum<EigeTipoValidacion_TransbordoEnum> TipoValidacion2_Transbordo { get { return Get<GenericEnum<EigeTipoValidacion_TransbordoEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        7, 0,   4,   4)] public GenericEnum<EigeTipoValidacion_ViajeroEnum> TipoValidacion2_Viajero { get { return Get<GenericEnum<EigeTipoValidacion_ViajeroEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H2VM",    7, 0,   5,   8)] public EigeInt8 ValidacionMultiple2 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H2VMB",   7, 0,   9,  10)] public GenericEnum<EigeMonederoBonusEnum> UsoMonederoBonus2 { get { return Get<GenericEnum<EigeMonederoBonusEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H2VCT",   7, 0,  11,  22)] public EigeInt16 CodigoTitulo2 { get { return Get<EigeInt16>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H2VFH",   7, 0,  23,  47)] public EigeDateTime FechaHora2 { get { return Get<EigeDateTime>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H2VEO",   7, 0,  48,  52)] public EigeInt8 EmpresaOperadora2 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H2VZV",   7, 0,  53,  55)] public GenericEnum<EigeZonaHistoricoEnum> Zona2 { get { return Get<GenericEnum<EigeZonaHistoricoEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H2VLEPS", 7, 0,  56,  68)] public EigeInt16 VLEPS2 { get { return Get<EigeInt16>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        7, 0,  56,  63)] public EigeInt8 VLEPS2_Ferrocarril_Estacion { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        7, 0,  64,  65)] public EigeInt8 VLEPS2_Ferrocarril_Vestibulo { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        7, 0,  56,  62)] public EigeInt8 VLEPS2_Bus_Linea { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        7, 0,  63,  68)] public EigeInt8 VLEPS2_Bus_Convoy { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        7, 0,  56,  62)] public EigeInt8 VLEPS2_Autobus_Linea { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        7, 0,  63,  67)] public EigeInt8 VLEPS2_Autobus_Parada { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        7, 0,  68,  68)] public EigeInt8 VLEPS2_Autobus_Sentido { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H2VMQ",   7, 0,  69,  74)] public EigeInt8 NumeroMaquina2 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H2VLNE",  7, 0,  75,  87)] public EigeBytes NumeroEquipo2 { get { return Get<EigeBytes>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H2VLUCV", 7, 0,  88,  99)] public EigeInt16 UnidadesConsumidas2 { get { return Get<EigeInt16>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H2VAVE",  7, 0, 100, 107)] public EigeInt8 AcumuladoValidacionesEntrada2 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H2VAVS",  7, 0, 108, 115)] public EigeInt8 AcumuladoValidacionesSalida2 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		// Historico 3
		[MifareClassicMemory("H3VTV",   7, 1,   1,   4)] public EigeBytes TipoValidacion3 { get { return Get<EigeBytes>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        7, 1,   1,   1)] public GenericEnum<EigeTipoValidacion_SentidoEnum> TipoValidacion3_Sentido { get { return Get<GenericEnum<EigeTipoValidacion_SentidoEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        7, 1,   2,   2)] public GenericEnum<EigeTipoValidacion_TransporteEnum> TipoValidacion3_Transporte { get { return Get<GenericEnum<EigeTipoValidacion_TransporteEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        7, 1,   3,   3)] public GenericEnum<EigeTipoValidacion_TransbordoEnum> TipoValidacion3_Transbordo { get { return Get<GenericEnum<EigeTipoValidacion_TransbordoEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        7, 1,   4,   4)] public GenericEnum<EigeTipoValidacion_ViajeroEnum> TipoValidacion3_Viajero { get { return Get<GenericEnum<EigeTipoValidacion_ViajeroEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H3VM",    7, 1,   5,   8)] public EigeInt8 ValidacionMultiple3 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H3VMB",   7, 1,   9,  10)] public GenericEnum<EigeMonederoBonusEnum> UsoMonederoBonus3 { get { return Get<GenericEnum<EigeMonederoBonusEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H3VCT",   7, 1,  11,  22)] public EigeInt16 CodigoTitulo3 { get { return Get<EigeInt16>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H3VFH",   7, 1,  23,  47)] public EigeDateTime FechaHora3 { get { return Get<EigeDateTime>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H3VEO",   7, 1,  48,  52)] public EigeInt8 EmpresaOperadora3 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H3VZV",   7, 1,  53,  55)] public GenericEnum<EigeZonaHistoricoEnum> Zona3 { get { return Get<GenericEnum<EigeZonaHistoricoEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H3VLEPS", 7, 1,  56,  68)] public EigeInt16 VLEPS3 { get { return Get<EigeInt16>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        7, 1,  56,  63)] public EigeInt8 VLEPS3_Ferrocarril_Estacion { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        7, 1,  64,  65)] public EigeInt8 VLEPS3_Ferrocarril_Vestibulo { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        7, 1,  56,  62)] public EigeInt8 VLEPS3_Bus_Linea { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        7, 1,  63,  68)] public EigeInt8 VLEPS3_Bus_Convoy { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        7, 1,  56,  62)] public EigeInt8 VLEPS3_Autobus_Linea { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        7, 1,  63,  67)] public EigeInt8 VLEPS3_Autobus_Parada { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        7, 1,  68,  68)] public EigeInt8 VLEPS3_Autobus_Sentido { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H3VMQ",   7, 1,  69,  74)] public EigeInt8 NumeroMaquina3 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H3VLNE",  7, 1,  75,  87)] public EigeBytes NumeroEquipo3 { get { return Get<EigeBytes>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H3VLUCV", 7, 1,  88,  99)] public EigeInt16 UnidadesConsumidas3 { get { return Get<EigeInt16>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H3VAVE",  7, 1, 100, 107)] public EigeInt8 AcumuladoValidacionesEntrada3 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H3VAVS",  7, 1, 108, 115)] public EigeInt8 AcumuladoValidacionesSalida3 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		// Historico 4
		[MifareClassicMemory("H4VTV",   7, 2,   1,   4)] public EigeBytes TipoValidacion4 { get { return Get<EigeBytes>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        7, 2,   1,   1)] public GenericEnum<EigeTipoValidacion_SentidoEnum> TipoValidacion4_Sentido { get { return Get<GenericEnum<EigeTipoValidacion_SentidoEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        7, 2,   2,   2)] public GenericEnum<EigeTipoValidacion_TransporteEnum> TipoValidacion4_Transporte { get { return Get<GenericEnum<EigeTipoValidacion_TransporteEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        7, 2,   3,   3)] public GenericEnum<EigeTipoValidacion_TransbordoEnum> TipoValidacion4_Transbordo { get { return Get<GenericEnum<EigeTipoValidacion_TransbordoEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        7, 2,   4,   4)] public GenericEnum<EigeTipoValidacion_ViajeroEnum> TipoValidacion4_Viajero { get { return Get<GenericEnum<EigeTipoValidacion_ViajeroEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H4VM",    7, 2,   5,   8)] public EigeInt8 ValidacionMultiple4 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H4VMB",   7, 2,   9,  10)] public GenericEnum<EigeMonederoBonusEnum> UsoMonederoBonus4 { get { return Get<GenericEnum<EigeMonederoBonusEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H4VCT",   7, 2,  11,  22)] public EigeInt16 CodigoTitulo4 { get { return Get<EigeInt16>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H4VFH",   7, 2,  23,  47)] public EigeDateTime FechaHora4 { get { return Get<EigeDateTime>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H4VEO",   7, 2,  48,  52)] public EigeInt8 EmpresaOperadora4 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H4VZV",   7, 2,  53,  55)] public GenericEnum<EigeZonaHistoricoEnum> Zona4 { get { return Get<GenericEnum<EigeZonaHistoricoEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H4VLEPS", 7, 2,  56,  68)] public EigeInt16 VLEPS4 { get { return Get<EigeInt16>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        7, 2,  56,  63)] public EigeInt8 VLEPS4_Ferrocarril_Estacion { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        7, 2,  64,  65)] public EigeInt8 VLEPS4_Ferrocarril_Vestibulo { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        7, 2,  56,  62)] public EigeInt8 VLEPS4_Bus_Linea { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        7, 2,  63,  68)] public EigeInt8 VLEPS4_Bus_Convoy { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        7, 2,  56,  62)] public EigeInt8 VLEPS4_Autobus_Linea { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        7, 2,  63,  67)] public EigeInt8 VLEPS4_Autobus_Parada { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        7, 2,  68,  68)] public EigeInt8 VLEPS4_Autobus_Sentido { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H4VMQ",   7, 2,  69,  74)] public EigeInt8 NumeroMaquina4 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H4VLNE",  7, 2,  75,  87)] public EigeBytes NumeroEquipo4 { get { return Get<EigeBytes>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H4VLUCV", 7, 2,  88,  99)] public EigeInt16 UnidadesConsumidas4 { get { return Get<EigeInt16>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H4VAVE",  7, 2, 100, 107)] public EigeInt8 AcumuladoValidacionesEntrada4 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H4VAVS",  7, 2, 108, 115)] public EigeInt8 AcumuladoValidacionesSalida4 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		// Historico 5
		[MifareClassicMemory("H5VTV",   5, 2,   1,   1)] public EigeBytes TipoValidacion5 { get { return Get<EigeBytes>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        5, 2,   1,   1)] public GenericEnum<EigeTipoValidacion_SentidoEnum> TipoValidacion5_Sentido { get { return Get<GenericEnum<EigeTipoValidacion_SentidoEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        5, 2,   2,   2)] public GenericEnum<EigeTipoValidacion_TransporteEnum> TipoValidacion5_Transporte { get { return Get<GenericEnum<EigeTipoValidacion_TransporteEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        5, 2,   3,   5)] public GenericEnum<EigeTipoValidacion_TransbordoEnum> TipoValidacion5_Transbordo { get { return Get<GenericEnum<EigeTipoValidacion_TransbordoEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        5, 2,   4,   4)] public GenericEnum<EigeTipoValidacion_ViajeroEnum> TipoValidacion5_Viajero { get { return Get<GenericEnum<EigeTipoValidacion_ViajeroEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H5VM",    5, 2,   5,   8)] public EigeInt8 ValidacionMultiple5 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H5VMB",   5, 2,   9,  10)] public GenericEnum<EigeMonederoBonusEnum> UsoMonederoBonus5 { get { return Get<GenericEnum<EigeMonederoBonusEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H5VCT",   5, 2,  11,  22)] public EigeInt16 CodigoTitulo5 { get { return Get<EigeInt16>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H5VFH",   5, 2,  23,  47)] public EigeDateTime FechaHora5 { get { return Get<EigeDateTime>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H5VEO",   5, 2,  48,  52)] public EigeInt8 EmpresaOperadora5 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H5VZV",   5, 2,  53,  55)] public GenericEnum<EigeZonaHistoricoEnum> Zona5 { get { return Get<GenericEnum<EigeZonaHistoricoEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H5VLEPS", 5, 2,  56,  68)] public EigeInt16 VLEPS5 { get { return Get<EigeInt16>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        5, 2,  56,  63)] public EigeInt8 VLEPS5_Ferrocarril_Estacion { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        5, 2,  64,  65)] public EigeInt8 VLEPS5_Ferrocarril_Vestibulo { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        5, 2,  56,  62)] public EigeInt8 VLEPS5_Bus_Linea { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        5, 2,  63,  68)] public EigeInt8 VLEPS5_Bus_Convoy { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        5, 2,  56,  62)] public EigeInt8 VLEPS5_Autobus_Linea { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        5, 2,  63,  67)] public EigeInt8 VLEPS5_Autobus_Parada { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",        5, 2,  68,  68)] public EigeInt8 VLEPS5_Autobus_Sentido { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H5VMQ",   5, 2,  69,  74)] public EigeInt8 NumeroMaquina5 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H5VLNE",  5, 2,  75,  87)] public EigeBytes NumeroEquipo5 { get { return Get<EigeBytes>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H5VLUCV", 5, 2,  88,  99)] public EigeInt16 UnidadesConsumidas5 { get { return Get<EigeInt16>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H5VAVE",  5, 2, 100, 107)] public EigeInt8 AcumuladoValidacionesEntrada5 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H5VAVS",  5, 2, 108, 115)] public EigeInt8 AcumuladoValidacionesSalida5 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		// Historico 6
		[MifareClassicMemory("H6VTV",  11, 0,   1,   4)] public EigeBytes TipoValidacion6 { get { return Get<EigeBytes> (); } set { Set(value.Raw); } }
		[MifareClassicMemory("",       11, 0,   1,   1)] public GenericEnum<EigeTipoValidacion_SentidoEnum> TipoValidacion6_Sentido { get { return Get<GenericEnum<EigeTipoValidacion_SentidoEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",       11, 0,   2,   2)] public GenericEnum<EigeTipoValidacion_TransporteEnum> TipoValidacion6_Transporte { get { return Get<GenericEnum<EigeTipoValidacion_TransporteEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",       11, 0,   3,   3)] public GenericEnum<EigeTipoValidacion_TransbordoEnum> TipoValidacion6_Transbordo { get { return Get<GenericEnum<EigeTipoValidacion_TransbordoEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",       11, 0,   4,   4)] public GenericEnum<EigeTipoValidacion_ViajeroEnum> TipoValidacion6_Viajero { get { return Get<GenericEnum<EigeTipoValidacion_ViajeroEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H6VM",   11, 0,   5,   8)] public EigeInt8 ValidacionMultiple6 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H6VMB",  11, 0,   9,  10)] public GenericEnum<EigeMonederoBonusEnum> UsoMonederoBonus6 { get { return Get<GenericEnum<EigeMonederoBonusEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H6VCT",  11, 0,  11,  22)] public EigeInt16 CodigoTitulo6 { get { return Get<EigeInt16>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H6VFH",  11, 0,  23,  47)] public EigeDateTime FechaHora6 { get { return Get<EigeDateTime>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H6VEO",  11, 0,  48,  52)] public EigeInt8 EmpresaOperadora6 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H6VZV",  11, 0,  53,  55)] public GenericEnum<EigeZonaHistoricoEnum> Zona6 { get { return Get<GenericEnum<EigeZonaHistoricoEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H6VLEPS",11, 0,  56,  68)] public EigeInt16 VLEPS6 { get { return Get<EigeInt16>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",       11, 0,  56,  63)] public EigeInt8 VLEPS6_Ferrocarril_Estacion { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",       11, 0,  64,  65)] public EigeInt8 VLEPS6_Ferrocarril_Vestibulo { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",       11, 0,  56,  62)] public EigeInt8 VLEPS6_Bus_Linea { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",       11, 0,  63,  68)] public EigeInt8 VLEPS6_Bus_Convoy { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",       11, 0,  56,  62)] public EigeInt8 VLEPS6_Autobus_Linea { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",       11, 0,  63,  67)] public EigeInt8 VLEPS6_Autobus_Parada { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",       11, 0,  68,  68)] public EigeInt8 VLEPS6_Autobus_Sentido { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H6VMQ",  11, 0,  69,  74)] public EigeInt8 NumeroMaquina6 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H6VLNE", 11, 0,  75,  87)] public EigeBytes NumeroEquipo6 { get { return Get<EigeBytes>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H6VLUCV",11, 0,  88,  99)] public EigeInt16 UnidadesConsumidas6 { get { return Get<EigeInt16>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H6VAVE", 11, 0, 100, 107)] public EigeInt8 AcumuladoValidacionesEntrada6 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H6VAVS", 11, 0, 108, 115)] public EigeInt8 AcumuladoValidacionesSalida6 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		// Historico 7
		[MifareClassicMemory("H7VTV",  11, 1,   1,   4)] public EigeBytes TipoValidacion7 { get { return Get<EigeBytes>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",       11, 1,   1,   1)] public GenericEnum<EigeTipoValidacion_SentidoEnum> TipoValidacion7_Sentido { get { return Get<GenericEnum<EigeTipoValidacion_SentidoEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",       11, 1,   2,   2)] public GenericEnum<EigeTipoValidacion_TransporteEnum> TipoValidacion7_Transporte { get { return Get<GenericEnum<EigeTipoValidacion_TransporteEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",       11, 1,   3,   3)] public GenericEnum<EigeTipoValidacion_TransbordoEnum> TipoValidacion7_Transbordo { get { return Get<GenericEnum<EigeTipoValidacion_TransbordoEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",       11, 1,   4,   4)] public GenericEnum<EigeTipoValidacion_ViajeroEnum> TipoValidacion7_Viajero { get { return Get<GenericEnum<EigeTipoValidacion_ViajeroEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H7VM",   11, 1,   5,   8)] public EigeInt8 ValidacionMultiple7 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H7VMB",  11, 1,   9,  10)] public GenericEnum<EigeMonederoBonusEnum> UsoMonederoBonus7 { get { return Get<GenericEnum<EigeMonederoBonusEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H7VCT",  11, 1,  11,  22)] public EigeInt16 CodigoTitulo7 { get { return Get<EigeInt16>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H7VFH",  11, 1,  23,  47)] public EigeDateTime FechaHora7 { get { return Get<EigeDateTime>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H7VEO",  11, 1,  48,  52)] public EigeInt8 EmpresaOperadora7 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H7VZV",  11, 1,  53,  55)] public GenericEnum<EigeZonaHistoricoEnum> Zona7 { get { return Get<GenericEnum<EigeZonaHistoricoEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H7VLEPS",11, 1,  56,  68)] public EigeInt16 VLEPS7 { get { return Get<EigeInt16>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",       11, 1,  56,  63)] public EigeInt8 VLEPS7_Ferrocarril_Estacion { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",       11, 1,  64,  65)] public EigeInt8 VLEPS7_Ferrocarril_Vestibulo { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",       11, 1,  56,  62)] public EigeInt8 VLEPS7_Bus_Linea { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",       11, 1,  63,  68)] public EigeInt8 VLEPS7_Bus_Convoy { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",       11, 1,  56,  62)] public EigeInt8 VLEPS7_Autobus_Linea { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",       11, 1,  63,  67)] public EigeInt8 VLEPS7_Autobus_Parada { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",       11, 1,  68,  68)] public EigeInt8 VLEPS7_Autobus_Sentido { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H7VMQ",  11, 1,  69,  74)] public EigeInt8 NumeroMaquina7 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H7VLNE", 11, 1,  75,  87)] public EigeBytes NumeroEquipo7 { get { return Get<EigeBytes>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H7VLUCV",11, 1,  88,  99)] public EigeInt16 UnidadesConsumidas7 { get { return Get<EigeInt16>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H7VAVE", 11, 1, 100, 107)] public EigeInt8 AcumuladoValidacionesEntrada7 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H7VAVS", 11, 1, 108, 115)] public EigeInt8 AcumuladoValidacionesSalida7 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		// Historico 8
		[MifareClassicMemory("H8VTV",  11, 2,   1,   4)] public EigeBytes TipoValidacion8 { get { return Get<EigeBytes>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",       11, 2,   1,   1)] public GenericEnum<EigeTipoValidacion_SentidoEnum> TipoValidacion8_Sentido { get { return Get<GenericEnum<EigeTipoValidacion_SentidoEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",       11, 2,   2,   2)] public GenericEnum<EigeTipoValidacion_TransporteEnum> TipoValidacion8_Transporte { get { return Get<GenericEnum<EigeTipoValidacion_TransporteEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",       11, 2,   3,   3)] public GenericEnum<EigeTipoValidacion_TransbordoEnum> TipoValidacion8_Transbordo { get { return Get<GenericEnum<EigeTipoValidacion_TransbordoEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",       11, 2,   4,   4)] public GenericEnum<EigeTipoValidacion_ViajeroEnum> TipoValidacion8_Viajero { get { return Get<GenericEnum<EigeTipoValidacion_ViajeroEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H8VM",   11, 2,   5,   8)] public EigeInt8 ValidacionMultiple8 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H8VMB",  11, 2,   9,  10)] public GenericEnum<EigeMonederoBonusEnum> UsoMonederoBonus8 { get { return Get<GenericEnum<EigeMonederoBonusEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H8VCT",  11, 2,  11,  22)] public EigeInt16 CodigoTitulo8 { get { return Get<EigeInt16>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H8VFH",  11, 2,  23,  47)] public EigeDateTime FechaHora8 { get { return Get<EigeDateTime>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H8VEO",  11, 2,  48,  52)] public EigeInt8 EmpresaOperadora8 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H8VZV",  11, 2,  53,  55)] public GenericEnum<EigeZonaHistoricoEnum> Zona8 { get { return Get<GenericEnum<EigeZonaHistoricoEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H8VLEPS",11, 2,  56,  68)] public EigeInt16 VLEPS8 { get { return Get<EigeInt16>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",       11, 2,  56,  63)] public EigeInt8 VLEPS8_Ferrocarril_Estacion { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",       11, 2,  64,  65)] public EigeInt8 VLEPS8_Ferrocarril_Vestibulo { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",       11, 2,  56,  62)] public EigeInt8 VLEPS8_Bus_Linea { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",       11, 2,  63,  68)] public EigeInt8 VLEPS8_Bus_Convoy { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",       11, 2,  56,  62)] public EigeInt8 VLEPS8_Autobus_Linea { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",       11, 2,  63,  67)] public EigeInt8 VLEPS8_Autobus_Parada { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",       11, 2,  68,  68)] public EigeInt8 VLEPS8_Autobus_Sentido { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H8VMQ",  11, 2,  69,  74)] public EigeInt8 NumeroMaquina8 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H8VLNE", 11, 2,  75,  87)] public EigeBytes NumeroEquipo8 { get { return Get<EigeBytes>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H8VLUCV",11, 2,  88,  99)] public EigeInt16 UnidadesConsumidas8 { get { return Get<EigeInt16>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H8VAVE", 11, 2, 100, 107)] public EigeInt8 AcumuladoValidacionesEntrada8 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("H8VAVS", 11, 2, 108, 115)] public EigeInt8 AcumuladoValidacionesSalida8 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }

		#region Constructors
		public EigeHistorico(MifareClassicCard card)
			: base(card)
		{
		}
		#endregion Constructors
	}
}
