using System;

namespace PayIn.Domain.Transport.Eige.Enums
{
	[Flags]
	public enum EigeTituloAmbitoOperadoresEnum : long
	{
		Especifico                      = 0x00000000,
		Cualquiera                      = 0xFFFFFFFF,
		// Valencia
		Valencia_GV                     = 0x00000001,
		Valencia_FGV                    = 0x00000002,
		Valencia_EMT                    = 0x00000004,
		Valencia_AUVACA                 = 0x00000008,
		Valencia_EDETANIA               = 0x00000010,
		Valencia_AVSA                   = 0x00000020,
		Valencia_BUNYOL                 = 0x00000040,
		Valencia_HERCA                  = 0x00000080,
		Valencia_FERNANBUS              = 0x00000100,
		Valencia_TorrenteUrbano         = 0x00000200,
		Valencia_Ubesa                  = 0x00000400,
		Valencia_RenfeCercanias         = 0x00000800,
		Valencia_ValenciaRibarroja      = 0x00001000,
		Valencia_Urbetur                = 0x00002000,
		Valencia_Metrorbital            = 0x00004000,
		Valencia_Alboraia               = 0x08000000,
		// Castellón
		Castellon_ACCSA                 = 0x00000001,
		Castellon_AMSA                  = 0x00000002,
		Castellon_HICID                 = 0x00000004
	}
}
