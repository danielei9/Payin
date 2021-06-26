namespace PayIn.Domain.Transport.Eige.Enums
{
	public enum EigeSubtipoTarjetaEnum
	{
		// Viajero no perso
		ViajeroNoPerso = 0x1,
		ViajeroNoPerso_Turistico = 0x2,
		ViajeroNoPerso_TarjetaNormalizadaValencia = 0x3,
		// Viajero perso
		ViajeroPerso_ConDni = 0x1,
		ViajeroPerso_ConOtroId = 0x2,
		ViajeroPerso_MenorConPadreConDni = 0x3,
		ViajeroPerso_MenorConPadreOtroId = 0x4,
		ViajeroPerso_JoveConDni = 0x5,
		ViajeroPerso_JoveConOtroId = 0x6,
		ViajeroPerso_MajorConDni = 0x7,
		ViajeroPerso_MajorConOtroId = 0x8,
		ViajeroPerso_EstudianteConDni = 0x9,
		ViajeroPerso_EstudianteConOtroId = 0xA,
		ViajeroPerso_Familia = 0xB,
		ViajeroPerso_ConvenioConDni = 0xC,
		ViajeroPerso_ConvenioConOtroId = 0xD,
		ViajeroPerso_DiscapacitadoConDni = 0xE,
		ViajeroPerso_DiscapacitadoConOtroId = 0xF,
		// Inspector / Mantenimiento
		Inspector_Ferroviario = 0x1,
		Inspector_Autobus = 0x2,
		Inspector_aVM = 0x3,
		Mantenimiento_Ferroviario = 0x4,
		Mantenimiento_Autobus = 0x5,
		// Empleado
		Empleado = 0x0,
		Empleado_Familiar = 0x1,
		Empleado_Juvilado = 0x2,
		Empleado_FamiliarJuvilado = 0x3,
		Empleado_Inspector = 0x4,
		// Pase no personalizado
		PaseNoPerso_aVM = 0x1,
		PaseNoPerso_Operador = 0x2,
		PaseNoPerso_Generalitat = 0x3,
		PaseNoPerso_Subcontratas = 0x4,
		PaseNoPerso_General = 0x5,
		PaseNoPerso_OrganismosOficiales = 0x6,
		// Pase personalizado
		PasePerso_aVM = 0x1,
		PasePerso_Operdor = 0x2,
		PasePerso_Generalitat = 0x3,
		PasePerso_Subcontratas = 0x4,
		PasePerso_General = 0x5,
		// Expendedor
		ExpendedorPerso = 0x1,
		ExpendedorCargaRecarga = 0x2,
		ExpendedorInspectoraVM = 0x3, //faltaba este subtipo y lo añadí
		// Tarjeta ciudadana
		Ciudadano_Empradronado = 0x0,
		Ciudadano_Forastero = 0x1,
		Ciudadano_GrupoMunicipio = 0x2,
		Ciudadano_GrupoOtroMunicipio = 0x3,
		Ciudadano_EmpleadoAyuntamiento = 0x4,
		Ciudadano_Estudianto_Joven = 0x5,
		Ciudadano_Jubilado = 0x6,
		Ciudadano_Municipal = 0x7,
		Ciudadano_ViajeroNoPerso = 0x8,
		Ciudadano_ViajeroPerso = 0x9,
		Ciudadano_EstudianteOtroId = 0xA,
		Ciudadano_MinusvalidoOtroId = 0xB,
		Ciudadano_Desempleado = 0xC, // Rev 074
		Ciudadano_SubvencionesSociales = 0xD,
		Ciudadano_FamiliaNumerosaGeneral20 = 0xE,
		Ciudadano_FamiliaNumerosaEspecial50 = 0xF,
		// Tarifa Especial
		Tarifa_FamiliaNumerosaGeneral20 = 0x1,
		Tarifa_FamiliaNumerosaEspecial50 = 0x2,
		Tarifa_Desempleado = 0x3,
		Tarifa_PensionistaClase1 = 0x4,
		Tarifa_PensionistaClase2 = 0x5,
		// Dispositivo móvil
		Movil_ViajeroNoPerso = 0x1
	}
}
