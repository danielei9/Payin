using PayIn.Domain.Transport.Eige.Enums;
using PayIn.Domain.Transport.Eige.Types;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Domain.Transport.Eige
{
	public class EigeTitulo : MifareClassicElement
	{
		[MifareClassicMemory("TGTA",    1, 1,   1,   4)] public GenericEnum<EigeTitulosActivosEnum> TitulosActivos { get { return Get<GenericEnum<EigeTitulosActivosEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TGP1",    1, 0,  77,  78)] public EigeInt8 PrioridadTitulo1 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TGP2",    1, 0,  79,  80)] public EigeInt8 PrioridadTitulo2 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TGPM",    1, 0,  81,  82)] public EigeInt8 PrioridadMonedero { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TGPB",    1, 0,  83,  84)] public EigeInt8 PrioridadBonus { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TGTU",    1, 1,   5,   6)] public GenericEnum<EigeTituloEnUsoEnum> TituloEnUso { get { return Get<GenericEnum<EigeTituloEnUsoEnum>>(); } set { Set(value.Raw); } }
		// Titulo 1
		[MifareClassicMemory("TEP",     3, 0,   1,   5)] public EigeInt8 EmpresaPropietaria1 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TF1CT",   3, 0,   6,  17)] public EigeInt16 CodigoTitulo1 { get { return Get<EigeInt16>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TF1VH",   3, 0,  18,  21)] public GenericEnum<TituloValidezHorariaEnum> ValidezHoraria1 { get { return Get<GenericEnum<TituloValidezHorariaEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TF1VD",   3, 0,  22,  25)] public GenericEnum<TituloValidezDiariaEnum> ValidezDiaria1 { get { return Get<GenericEnum<TituloValidezDiariaEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TF1TT",   3, 0,  26,  29)] public GenericEnum<EigeTituloTipoTarifaEnum> TipoTarifa1 { get { return Get<GenericEnum<EigeTituloTipoTarifaEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TF1TF",   3, 0,  30,  32)] public EigeInt8 ControlTarifa1 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TF1AO",   3, 0,  33,  64)] public GenericEnum<EigeTituloAmbitoOperadoresEnum> AmbitoOperadores1 { get { return Get<GenericEnum<EigeTituloAmbitoOperadoresEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TF1VO",   3, 0,  65,  65)] public GenericEnum<EigeValidezTitulosEnum> TieneValidezOperador1 { get { return Get<GenericEnum<EigeValidezTitulosEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TF1VOTT", 3, 0,  66,  67)] public GenericEnum<EigeTicketTipoInformacionEnum> TipoInformacion1 { get { return Get<GenericEnum<EigeTicketTipoInformacionEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TF1VOT",  3, 0,  68,  77)] public EigeBytes ValidezOperador1 { get { return Get<EigeBytes>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TF1VZ",   3, 0,  78,  82)] public GenericEnum<EigeZonaEnum> ValidezZonal1 { get { return Get<GenericEnum<EigeZonaEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TF1TUVT", 3, 0,  83,  84)] public GenericEnum<EigeTipoUnidadesValidezTemporalEnum> TipoUnidadesValidezTemporal1 { get { return Get<GenericEnum<EigeTipoUnidadesValidezTemporalEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TF1NUVT", 3, 0,  85,  92)] public EigeInt8 NumeroUnidadesValidezTemporal1 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TF1MTEA", 3, 0,  93,  94)] public EigeInt8 MaxTransbordosExternos1 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TF1MPT",  3, 0,  95,  98)] public EigeInt8 MaxPersonasEnTransbordo1 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TF1MTT",  3, 0,  99, 106)] public EigeInt8 MaxTiempoTransbordo1 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TF1UC",   3, 0, 107, 107)] public EigeBool UsoDelCampoSaldoViajes1 { get { return Get<EigeBool>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("ATF1LIB1",3, 0, 120, 120)] public EigeBool ValidezSextaZona1 { get { return Get<EigeBool>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TV1BTA",  1, 1,   7,   7)] public EigeBool TituloEnAmpliacion1 { get { return Get<EigeBool>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TV1FIV",  1, 1,   8,  32)] public EigeDateTime FechaValidez1 { get { return Get<EigeDateTime>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TV1SV",   1, 1,  33,  40)] public EigeInt8 SaldoViaje1 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TIT1BT",  3, 0,   1, 120)] public EigeBytes CheckSumBytes1 { get { return Get<EigeBytes>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TIT1CK",  3, 0, 121, 128)] public EigeInt8 CRC { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		// Titulo 2
		[MifareClassicMemory("TF2EP",   4, 0,   1,   5)] public EigeInt8 EmpresaPropietaria2 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TF2CT",   4, 0,   6,  17)] public EigeInt16 CodigoTitulo2 { get { return Get<EigeInt16>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TF2VH",   4, 0,  18,  21)] public GenericEnum<TituloValidezHorariaEnum> ValidezHoraria2 { get { return Get<GenericEnum<TituloValidezHorariaEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TF2VD",   4, 0,  22,  25)] public GenericEnum<TituloValidezDiariaEnum> ValidezDiaria2 { get { return Get<GenericEnum<TituloValidezDiariaEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TF2TT",   4, 0,  26,  29)] public GenericEnum<EigeTituloTipoTarifaEnum> TipoTarifa2 { get { return Get<GenericEnum<EigeTituloTipoTarifaEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TF2TF",   4, 0,  30,  32)] public EigeInt8 ControlTarifa2 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TF2AO",   1, 0,  85, 116)] public GenericEnum<EigeTituloAmbitoOperadoresEnum> AmbitoOperadores2 { get { return Get<GenericEnum<EigeTituloAmbitoOperadoresEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TF2VO",   4, 0,  33,  33)] public GenericEnum<EigeValidezTitulosEnum> TieneValidezOperador2 { get { return Get<GenericEnum<EigeValidezTitulosEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TF2VOTT", 4, 0,  34,  35)] public GenericEnum<EigeTicketTipoInformacionEnum> TipoInformacion2 { get { return Get<GenericEnum<EigeTicketTipoInformacionEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TF2VOT",  4, 0,  36,  45)] public EigeBytes ValidezOperador2 { get { return Get<EigeBytes>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TF2VZ",   4, 0,  46,  50)] public GenericEnum<EigeZonaEnum> ValidezZonal2 { get { return Get<GenericEnum<EigeZonaEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TF2TUVT", 4, 0,  51,  52)] public GenericEnum<EigeTipoUnidadesValidezTemporalEnum> TipoUnidadesValidezTemporal2 { get { return Get<GenericEnum<EigeTipoUnidadesValidezTemporalEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TF2NUVT", 4, 0,  53,  60)] public EigeInt8 NumeroUnidadesValidezTemporal2 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TF2MTEA", 4, 0,  61,  62)] public EigeInt8 MaxTransbordosExternos2 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TF2MPT",  4, 0,  63,  66)] public EigeInt8 MaxPersonasEnTransbordo2 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TF2MTT",  4, 0,  67,  74)] public EigeInt8 MaxTiempoTransbordo2 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TF2SV",   4, 0,  75,  75)] public EigeBool UsoDelCampoSaldoViajes2 { get { return Get<EigeBool>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("ATF2VZ6", 4, 0, 120, 120)] public EigeBool ValidezSextaZona2 { get { return Get<EigeBool>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TV2BTA",  4, 0,  76,  76)] public EigeBool TituloEnAmpliacion2 { get { return Get<EigeBool>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TV2FIV",  4, 0,  77, 101)] public EigeDateTime FechaValidez2 { get { return Get<EigeDateTime>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("TV2SV",   4, 0, 102, 109)] public EigeInt8 SaldoViaje2 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		// Monedero
		[MifareClassicMemory("MS",      6, 0,   1,  32)] public EigeCurrency SaldoMonedero { get { return Get<EigeCurrency>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("M2EP",    6, 0,  97, 105)] public EigeInt8 EmpresaPropietariaM { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		// Bonus
		[MifareClassicMemory("BS",      5, 0,   1,  32)] public EigeInt32 SaldoBonus { get { return Get<EigeInt32>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("B2EP",    5, 0,  97, 105)] public EigeInt8 EmpresaPropietariaB { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }

		//Copia Titulo 1
		//[MifareClassicMemory("CTV1SV", 1, 2, 33, 40)] public EigeInt8 CopiaSaldoViaje1 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		//[MifareClassicMemory("CTGTA", 1, 2, 1, 4)] 	public GenericEnum<EigeTitulosActivosEnum> CopiaTitulosActivos { get { return Get<GenericEnum<EigeTitulosActivosEnum>>(); } set { Set(value.Raw); } }
		//[MifareClassicMemory("CTGTU", 1, 2, 5, 6)]	public GenericEnum<EigeTituloEnUsoEnum> CopiaTituloEnUso { get { return Get<GenericEnum<EigeTituloEnUsoEnum>>(); } set { Set(value.Raw); } }
		//[MifareClassicMemory("CTV1BTA", 1, 2, 7, 7)] public EigeBool CopiaTituloEnAmpliacion1 { get { return Get<EigeBool>(); } set { Set(value.Raw); } }
		//[MifareClassicMemory("CTV1FIV", 1, 2, 8, 32)]	public EigeDateTime CopiaFechaValidez1 { get { return Get<EigeDateTime>(); } set { Set(value.Raw); } }

		////Copia Titulo 2
		//[MifareClassicMemory("CTV2SV", 4, 1, 102, 109)] public EigeInt8 CopiaSaldoViaje2 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		//[MifareClassicMemory("CTF2EP", 4, 1, 1, 5)]		public EigeInt8 CopiaEmpresaPropietaria2 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		//[MifareClassicMemory("CTF2CT", 4, 1, 6, 17)]	public EigeInt16 CopiaCodigoTitulo2 { get { return Get<EigeInt16>(); } set { Set(value.Raw); } }
		//[MifareClassicMemory("CTF2VH", 4, 1, 18, 21)]	public GenericEnum<TituloValidezHorariaEnum> CopiaValidezHoraria2 { get { return Get<GenericEnum<TituloValidezHorariaEnum>>(); } set { Set(value.Raw); } }
		//[MifareClassicMemory("CTF2VD", 4, 1, 22, 25)]	public GenericEnum<TituloValidezDiariaEnum> CopiaValidezDiaria2 { get { return Get<GenericEnum<TituloValidezDiariaEnum>>(); } set { Set(value.Raw); } }
		//[MifareClassicMemory("CTF2TT", 4, 1, 26, 29)]	public GenericEnum<EigeTituloTipoTarifaEnum> CopiaTipoTarifa2 { get { return Get<GenericEnum<EigeTituloTipoTarifaEnum>>(); } set { Set(value.Raw); } }
		//[MifareClassicMemory("CTF2TF", 4, 1, 30, 32)]	public EigeInt8 CopiaControlTarifa2 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		//[MifareClassicMemory("CTF2VO", 4, 1, 33, 33)]    public GenericEnum<EigeValidezTitulosEnum> CopiaTieneValidezOperador2 { get { return Get<GenericEnum<EigeValidezTitulosEnum>>(); } set { Set(value.Raw); } }
		//[MifareClassicMemory("CTF2VOTT", 4, 1, 34, 35)]	public GenericEnum<EigeTicketTipoInformacionEnum> CopiaTipoInformacion2 { get { return Get<GenericEnum<EigeTicketTipoInformacionEnum>>(); } set { Set(value.Raw); } }
		//[MifareClassicMemory("CTF2VOT", 4, 1, 36, 45)]	public EigeBytes CopiaValidezOperador2 { get { return Get<EigeBytes>(); } set { Set(value.Raw); } }
		//[MifareClassicMemory("CTF2VZ", 4, 1, 46, 50)]	public GenericEnum<EigeZonaEnum> CopiaValidezZonal2 { get { return Get<GenericEnum<EigeZonaEnum>>(); } set { Set(value.Raw); } }
		//[MifareClassicMemory("CTF2TUVT", 4, 1, 51, 52)]	public GenericEnum<EigeTipoUnidadesValidezTemporalEnum> CopiaTipoUnidadesValidezTemporal2 { get { return Get<GenericEnum<EigeTipoUnidadesValidezTemporalEnum>>(); } set { Set(value.Raw); } }
		//[MifareClassicMemory("CTF2NUVT", 4, 1, 53, 60)]	public EigeInt8 CopiaNumeroUnidadesValidezTemporal2 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		//[MifareClassicMemory("CTF2MTEA", 4, 1, 61, 62)]	public EigeInt8 CopiaMaxTransbordosExternos2 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		//[MifareClassicMemory("CTF2MPT", 4, 1, 63, 66)]	public EigeInt8 CopiaMaxPersonasEnTransbordo2 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		//[MifareClassicMemory("CTF2MTT", 4, 1, 67, 74)]	public EigeInt8 CopiaMaxTiempoTransbordo2 { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		//[MifareClassicMemory("CTF2SV", 4, 1, 75, 75)]	public EigeBool CopiaUsoDelCampoSaldoViajes2 { get { return Get<EigeBool>(); } set { Set(value.Raw); } }
		//[MifareClassicMemory("CATF2VZ6", 4, 1, 120, 120)]public EigeBool CopiaValidezSextaZona2 { get { return Get<EigeBool>(); } set { Set(value.Raw); } }
		//[MifareClassicMemory("CTV2BTA", 4, 1, 76, 76)]	public EigeBool CopiaTituloEnAmpliacion2 { get { return Get<EigeBool>(); } set { Set(value.Raw); } }
		//[MifareClassicMemory("CTV2FIV", 4, 1, 77, 101)]	public EigeDateTime CopiaFechaValidez2 { get { return Get<EigeDateTime>(); } set { Set(value.Raw); } }

		#region Constructors
		public EigeTitulo(MifareClassicCard card)
			: base(card)
		{
		}
		#endregion Constructors
	}
}
