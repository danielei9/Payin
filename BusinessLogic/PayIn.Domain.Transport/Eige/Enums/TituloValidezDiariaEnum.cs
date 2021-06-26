using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayIn.Domain.Transport.Eige.Enums
{
	public enum TituloValidezDiariaEnum
	{
		SinRestriccion = 0x0,
		LunesViernes = 0x1,
		FinDeSemana = 0x2,
		DiasLectivosNoFestivos = 0x3, // Ene-Jun + Sep-Dic
		Viernes15Domingo = 0x4
		// Segun tabla de validez diaria???
	}
}
