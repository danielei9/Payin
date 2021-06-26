using PayIn.Domain.Transport.Eige.Enums;
using PayIn.Domain.Transport.Eige.Types;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Domain.Transport.Eige
{
	public class EigeTituloTuiN : MifareClassicElement
	{
		[MifareClassicMemory("MTVSCE",   9, 0,   1,  21)] public EigeInt24 SaldoViaje { get { return Get<EigeInt24>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",         9, 0,   1,   1)] public EigeBool SaldoViaje_Sign { get { return Get<EigeBool>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",         9, 0,   2,  21)] public EigeCurrency SaldoViaje_Value { get { return Get<EigeCurrency>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("MVTVn",    9, 0,  22,  25)] public EigeBytes UltimaValidacionTipo { get { return Get<EigeBytes>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",         9, 0,  22,  22)] public GenericEnum<EigeTipoValidacion_SentidoEnum> UltimaValidacionTipo_Sentido { get { return Get<GenericEnum<EigeTipoValidacion_SentidoEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",         9, 0,  23,  23)] public GenericEnum<EigeTipoValidacion_TransporteEnum> UltimaValidacionTipo_Transporte { get { return Get<GenericEnum<EigeTipoValidacion_TransporteEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",         9, 0,  24,  24)] public GenericEnum<EigeTipoValidacion_TransbordoEnum> UltimaValidacionTipo_Transbordo { get { return Get<GenericEnum<EigeTipoValidacion_TransbordoEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("",         9, 0,  25,  25)] public GenericEnum<EigeTipoValidacion_ViajeroEnum> UltimaValidacionTipo_Viajero { get { return Get<GenericEnum<EigeTipoValidacion_ViajeroEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("MVEn",     9, 0,  26,  33)] public EigeInt8 UltimaValidacionEstacion { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("MVFHn",    9, 0,  34,  58)] public EigeDateTime UltimaValidacionFechaHora { get { return Get<EigeDateTime>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("MVNPV",    9, 0,  59,  64)] public EigeInt8 NumeroPersonasViajando { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("MVCTI",    9, 0,  65,  66)] public EigeInt8 ContadorTransbordosInternos { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("MVTVC",    9, 0,  67,  74)] public EigeInt8 TiempoConsumido { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("MVEini",   9, 0,  75,  82)] public EigeInt8 EstacionInicioViaje { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("MVTT",     9, 0,  83,  87)] public EigeInt8 TipoTarifa { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("MVNCCV",   9, 0,  88, 101)] public EigeCurrency Cobrado { get { return Get<EigeCurrency>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("MVCA1V",   9, 0, 102, 117)] public EigeCurrency ConsumoAcumulado { get { return Get<EigeCurrency>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("MVSn",     9, 0, 118, 119)] public EigeBytes Sentido { get { return Get<EigeBytes>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("MTTI",     9, 2,   1,   4)] public GenericEnum<EigeTipoTemporalidad_TuiNEnum> Temporalidad { get { return Get<GenericEnum<EigeTipoTemporalidad_TuiNEnum>>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("MVC",      9, 2,   5,   7)] public EigeInt8 VersionClaves { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("MT8H",     9, 2,   8,   8)] public EigeBool Tiene8Historicos { get { return Get<EigeBool>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("MHITA",    9, 2,   9,  11)] public EigeInt8 IndiceTransaccion { get { return Get<EigeInt8>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("MVNPini",  9, 2,  15,  20)] public EigeBytes PersonasDeInicio { get { return Get<EigeBytes>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("MVNCC1V",  9, 2,  21,  32)] public EigeCurrency CobradoPrimerViajero { get { return Get<EigeCurrency>(); } set { Set(value.Raw); } }
		[MifareClassicMemory("MVNCB1V",  9, 2,  33,  44)] public EigeCurrency BonificadoPrimerViajero { get { return Get<EigeCurrency>(); } set { Set(value.Raw); } }

		#region Constructors
		public EigeTituloTuiN(MifareClassicCard card)
			: base(card)
		{
		}
		#endregion Constructors
	}
}
