using PayIn.Application.Dto.Results;
using PayIn.Application.Dto.Transport.Results.TransportOperation;
using PayIn.Application.Transport.Handlers;
using PayIn.Application.Transport.Scripts;
using PayIn.Application.Transport.Scripts.Mobilis;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Transport;
using PayIn.Domain.Transport.Eige;
using PayIn.Domain.Transport.Eige.Enums;
using PayIn.Domain.Transport.Eige.Types;
using PayIn.Infrastructure.Transport.Repositories;
using PayIn.Infrastructure.Transport.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Domain;
using Xp.Domain.Transport;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Transport.Services
{
	public class EigeService
	{
		private readonly SigapuntService SIGAPuntService;
		private readonly EmtService EmtService;
		private readonly BlackListRepository BlackListRepository;
		private readonly GreyListRepository GreyListRepository;
		private readonly IEntityRepository<TransportPrice> RepositoryPrice;
		private readonly IEntityRepository<TransportTitle> RepositoryTitle;
		private readonly IEntityRepository<TransportCardSupport> RepositorySupport;
		public readonly static int TransferMode = 2;
        // 5 años
        public readonly static int TitleDurationYears = 5;
        // Media hora
        public readonly static int TimeToExchange = 30; 
        // Valencia
        public readonly static int PayinCode = 23;
		public readonly static int EigeCode = 1;
		public readonly static int FgvValenciaCode = 2;
		public readonly static int EmtCode = 3;
        // Alicante
        public readonly static int FgvAlicanteCode = 3;

		public class Titulo
		{
			public int Code { get; set; }
			public string Name { get; set; }
			public string Owner { get; set; }
		}

		#region Constructors
		public EigeService(
			SigapuntService sigAPuntService,
			EmtService emtService,
			BlackListRepository blackListRepository,
			GreyListRepository greyListRepository,
			IEntityRepository<TransportPrice> repositoryPrice,
			IEntityRepository<TransportTitle> repositoryTitle,
			IEntityRepository<TransportCardSupport> repositorySupport
		)
		{
			if (sigAPuntService == null) throw new ArgumentNullException("sigAPuntService");
			if (emtService == null) throw new ArgumentNullException("emtService");
			if (blackListRepository == null) throw new ArgumentNullException("blackListRepository");
			if (greyListRepository == null) throw new ArgumentNullException("greyListRepository");
			if (repositoryPrice == null) throw new ArgumentNullException("repositoryPrice");
			if (repositoryTitle == null) throw new ArgumentNullException("repositoryTitle");
			if (repositorySupport == null) throw new ArgumentNullException("repositorySupport");

			SIGAPuntService = sigAPuntService;
			EmtService = emtService;
			BlackListRepository = blackListRepository;
			GreyListRepository = greyListRepository;
			RepositoryPrice = repositoryPrice;
			RepositoryTitle = repositoryTitle;
			RepositorySupport = repositorySupport;
		}
		#endregion Constructors

		#region IsTuiN
		public bool IsTuiN(int? code)
		{
			return ((code >= 1271) && (code <= 1277));
		}
		public async Task<bool> IsTuiN(long uid, int? code)
		{
			return await Task.Run(() =>
			{
				return ((code >= 1271) && (code <= 1277));
			});
		}
		#endregion IsTuiN

		#region SaldoTuiN
		public int SaldoTuiN(int oldMoney, Ticket ticket, int? quantity)
		{			
			var amount = Convert.ToInt32(ticket.Amount * 100);

			if (quantity != null && quantity > 0)
			{
				amount = amount / (1 - ((int)quantity / 100));
			}
			return oldMoney + amount;
		}
		#endregion SaldoTuiN

		#region GetOperationToRevoke
		public async Task<TransportOperation> GetOperationToRevoke(long uid, TransportCardGetReadInfoScript script, IQueryable<TransportOperation> operation, DateTime now)
		{
			// Get time period
			var revoqueDateMin = now.AddMinutes(-TimeToExchange).ToUTC();
			#if DEBUG || TEST || HOMO
			var revoqueDateMax = now.ToUTC();
#endif // DEBUG || TEST || HOMO

			// Get last charge/recharge in card
			var lastChargePosition = script.Card.Carga.PosicionUltima.Value;
			var lastChargeDate =
				lastChargePosition == EigePosicionUltimaCargaEnum.Carga1 ?
					await GetCargaDate1Async(uid, script) :
					await GetCargaDate2Async(uid, script);
			var chargeOwner =
				lastChargePosition == EigePosicionUltimaCargaEnum.Carga1 ?
					script.Card.Carga.Empresa1.Value :
					script.Card.Carga.Empresa2.Value;
			if (chargeOwner != PayinCode)
				return null;

			// Get last validation in card
			var numHistoricos = (await GetHistoricoTiene8Async(uid, script) == true) ? 8 : 4;
			var lastValidationDate = new List<DateTime?>
				{
					1 > numHistoricos ? null : (await GetHistoricoDate1Async(uid, script)),
					2 > numHistoricos ? null : (await GetHistoricoDate2Async(uid, script)),
					3 > numHistoricos ? null : (await GetHistoricoDate3Async(uid, script)),
					4 > numHistoricos ? null : (await GetHistoricoDate4Async(uid, script)),
					5 > numHistoricos ? null : (await GetHistoricoDate5Async(uid, script)),
					6 > numHistoricos ? null : (await GetHistoricoDate6Async(uid, script)),
					7 > numHistoricos ? null : (await GetHistoricoDate7Async(uid, script)),
					8 > numHistoricos ? null : (await GetHistoricoDate8Async(uid, script))
				}
				.Where(x => x != null)
				.OrderByDescending(x => x.Value)
				.FirstOrDefault();
			lastValidationDate = (lastValidationDate != null) ? lastValidationDate.ToUTC() : revoqueDateMin.AddDays(-1);

			// Get operation
			var lastOperation = operation
				.Where(x =>
					(x.Uid == uid) &&
					(x.Error == "") &&
					(
						(x.OperationType == OperationType.Recharge) ||
						(x.OperationType == OperationType.Charge) ||
						(x.OperationType == OperationType.Revoke)
					) &&
					x.OperationDate > lastValidationDate && // La fecha de la op ha de ser > que la de la última validación
					x.OperationDate > revoqueDateMin &&
#if DEBUG || TEST || HOMO
					x.OperationDate <= revoqueDateMax &&
#endif // DEBUG || TEST || HOMO
					x.OperationDate >= lastChargeDate
				)
				.OrderByDescending(x => x.OperationDate)
				.ThenByDescending(x => x.Id)
				.FirstOrDefault();
			if ((lastOperation == null) || (lastOperation.OperationType == OperationType.Revoke))
				return null;

			// Check revokable type
			if (lastOperation.RechargeType == RechargeType.RechargeAndUpdateZone)
				return null;
			if (lastOperation.RechargeType == RechargeType.Replace)
				return null;

			return lastOperation;
		}
		#endregion GetOperationToRevoke

		#region InWhiteListAsync
		public async Task<bool> InWhiteList1Async(long uid, ITransportCardCheckWhiteListScript script)
		{
			return await InWhiteListAsync(uid, script, WhiteListTitleType.Title1, WhiteListOperationType.Precharge);
		}
		public async Task<bool> InWhiteList2Async(long uid, ITransportCardCheckWhiteListScript script)
		{
			return await InWhiteListAsync(uid, script, WhiteListTitleType.Title2, WhiteListOperationType.Precharge);
		}
		public async Task<bool> InWhiteListPAsync(long uid, ITransportCardCheckWhiteListScript script)
		{
			return await InWhiteListAsync(uid, script, WhiteListTitleType.Purse, WhiteListOperationType.Precharge);
		}
		public async Task<bool> InWhiteListBAsync(long uid, ITransportCardCheckWhiteListScript script)
		{
			return await InWhiteListAsync(uid, script, WhiteListTitleType.Bonus, WhiteListOperationType.Precharge);
		}
		private async Task<bool> InWhiteListAsync(long uid, ITransportCardCheckWhiteListScript script, WhiteListTitleType title, WhiteListOperationType operation)
		{
			return await Task.Run(() =>
			{
				//await transportCardRechargeFinishHandler.ExecuteAsync(new TransportCardRechargeFinishArguments(script.Card.Titulo, script.Card.Uid.ToString(), script, script.Card, 0));
				//var now = DateTime.Now;
				//if (script.Card.Titulo.TituloEnAmpliacion1.Value && (script.Card.Titulo.FechaValidez1.Value > now || script.Card.Titulo.FechaValidez1.Value == null))
				//	throw new Exception(WhiteListResources.ImpossibleRecharge);
				//else if (script.Card.Titulo.TituloEnAmpliacion1.Value && script.Card.Titulo.FechaValidez1.Value < now)
				//	script.Card.Titulo.FechaValidez1 = new EigeDateTime(now.AddDays(script.Card.Titulo.NumeroUnidadesValidezTemporal1.Value));
				//else if (!script.Card.Titulo.TituloEnAmpliacion1.Value && (script.Card.Titulo.FechaValidez1.Value < now || script.Card.Titulo.FechaValidez1.Value > now || script.Card.Titulo.FechaValidez1.Value == null))
				//	script.Card.Titulo.TituloEnAmpliacion1 = new EigeBool(true);
				return true;
			});
		}
		#endregion InWhiteListAsync

		#region InGreyListAsync
		public async Task<bool> InGreyListAsync(long uid, ITransportCardCheckGreyListScript script)
		{
			return await InGreyListAsync(uid, script, GreyList.MachineType.Charge);
		}
		//public async Task<bool> InGreyList2Async(long uid, ITransportCardCheckGreyListScript script)
		//{
		//	return await InGreyListAsync(uid, script, BlackListServiceType.Title2, BlackListMachineType.Charge);
		//}
		//public async Task<bool> InGreyListPAsync(long uid, ITransportCardCheckGreyListScript script)
		//{
		//	return await InGreyListAsync(uid, script, BlackListServiceType.Purse, BlackListMachineType.Charge);
		//}
		//public async Task<bool> InGreyListBAsync(long uid, ITransportCardCheckGreyListScript script)
		//{
		//	return await InGreyListAsync(uid, script, BlackListServiceType.Bonus, BlackListMachineType.Charge);
		//}
		private async Task<bool> InGreyListAsync(long uid, ITransportCardCheckGreyListScript script/*, BlackListServiceType service*/, GreyList.MachineType machine)
		{
			var now = DateTime.Now;

			if (script.Card.Validacion.ListaGris.Value)
			{
				var recentInGreyList = (await GreyListRepository.GetAsync())
					.Where(x =>
						x.Uid == uid && (
							x.ResolutionDate == null ||
							now.Subtract(x.ResolutionDate.Value) < EigeCard.GreyListResolvedElapsed
						)
					)
					.Any();

				if (!recentInGreyList)
				{
					script.Card.Validacion.ListaGris = new EigeBool(false);
					script.Write(script.Response, x => x.Validacion.ListaGris);
					await SIGAPuntService.BLALGAsync(uid);
				}
			}
			else
			{
				var inGreyList = (await GreyListRepository.GetAsync())
					.Where(x =>
						x.Uid == uid &&
						x.ResolutionDate == null &&
						//(x.Service | service) != 0 &&
						(x.Machine | machine) != 0
					);
				byte[] changedField = new byte[4];
				if (inGreyList.Any())
				{
					script.Card.Validacion.ListaGris = new EigeBool(true);
					script.Write(script.Response, x => x.Validacion.ListaGris);

					foreach (var item in inGreyList)
					{
						if (item.Action == GreyList.ActionType.IncreaseBalance)
						{
							//foreach (var section in script.Card.GetType().GetProperties()
							//.Select(x => x.GetValue(script.Card))
							//)
							//{
							//	foreach (var property in section.GetType().GetProperties())
							//	{
							//		foreach (var attribute in property.GetCustomAttributes(typeof(MifareClassicMemoryAttribute), true)
							//			.Cast<MifareClassicMemoryAttribute>()
							//			.Where(y => y.Name == item.Field)
							//		)
							//		{
							//			property.SetValue(section, Activator.CreateInstance(property.PropertyType, item.NewValue));
							//			script.Sum(script.Response, attribute.Sector, attribute.Block, item.NewValue, attribute.StartBit, attribute.EndBit);
							//		}
							//	}
							//}
						}
						else if (item.Action == GreyList.ActionType.DiscountBalance)
						{
							//foreach (var section in script.Card.GetType().GetProperties()
							//.Select(x => x.GetValue(script.Card))
							//)
							//{
							//	foreach (var property in section.GetType().GetProperties())
							//	{
							//		foreach (var attribute in property.GetCustomAttributes(typeof(MifareClassicMemoryAttribute), true)
							//			.Cast<MifareClassicMemoryAttribute>()
							//			.Where(y => y.Name == item.Field)
							//		)
							//		{
							//			property.SetValue(section, Activator.CreateInstance(property.PropertyType, item.NewValue.FromHexadecimal()));
							//			script.Sum(script.Response, attribute.Sector, attribute.Block, "-" + item.NewValue, attribute.StartBit, attribute.EndBit);
							//		}
							//	}
							//}
						}
						else if (item.Action == GreyList.ActionType.ModifyBalance)
						{
							foreach (var section in script.Card.GetType().GetProperties()
						.Select(x => x.GetValue(script.Card))
						)
							{
								foreach (var property in section.GetType().GetProperties())
								{
									foreach (var attribute in property.GetCustomAttributes(typeof(MifareClassicMemoryAttribute), true)
										.Cast<MifareClassicMemoryAttribute>()
										.Where(y => y.Name == item.Field)
									)
									{
										property.SetValue(section, Activator.CreateInstance(property.PropertyType, item.NewValue));
										script.Write(script.Response, attribute.Sector, attribute.Block, item.NewValue, attribute.StartBit, attribute.EndBit);
									}
								}
							}
						}
						else if (item.Action == GreyList.ActionType.IncreaseUnities)
						{
							foreach (var section in script.Card.GetType().GetProperties()
							.Select(x => x.GetValue(script.Card))
							)
							{
								foreach (var property in section.GetType().GetProperties())
								{
									foreach (var attribute in property.GetCustomAttributes(typeof(MifareClassicMemoryAttribute), true)
										.Cast<MifareClassicMemoryAttribute>()
										.Where(y => y.Name == item.Field)
									)
									{
										property.SetValue(section, Activator.CreateInstance(property.PropertyType, item.NewValue));
										script.Write(script.Response, attribute.Sector, attribute.Block, item.NewValue, attribute.StartBit, attribute.EndBit);
									}
								}
							}
						}
						else if (item.Action == GreyList.ActionType.DiscountUnities)
						{
							foreach (var section in script.Card.GetType().GetProperties()
							.Select(x => x.GetValue(script.Card))
							)
							{
								foreach (var property in section.GetType().GetProperties())
								{
									foreach (var attribute in property.GetCustomAttributes(typeof(MifareClassicMemoryAttribute), true)
										.Cast<MifareClassicMemoryAttribute>()
										.Where(y => y.Name == item.Field)
									)
									{
										property.SetValue(section, Activator.CreateInstance(property.PropertyType, item.NewValue));
										script.Write(script.Response, attribute.Sector, attribute.Block, item.NewValue, attribute.StartBit, attribute.EndBit);
									}
								}
							}
						}
						else if (item.Action == GreyList.ActionType.ModifyStartDate)
						{
							foreach (var section in script.Card.GetType().GetProperties()
							.Select(x => x.GetValue(script.Card))
							)
							{
								foreach (var property in section.GetType().GetProperties())
								{
									foreach (var attribute in property.GetCustomAttributes(typeof(MifareClassicMemoryAttribute), true)
										.Cast<MifareClassicMemoryAttribute>()
										.Where(y => y.Name == item.Field)
									)
									{
										property.SetValue(section, Activator.CreateInstance(property.PropertyType, item.NewValue));
										script.Write(script.Response, attribute.Sector, attribute.Block, item.NewValue, attribute.StartBit, attribute.EndBit);
									}
								}
							}
						}
						else if (item.Action == GreyList.ActionType.ModifyExtensionBit)
						{
							foreach (var section in script.Card.GetType().GetProperties()
								.Select(x => x.GetValue(script.Card))
								)
							{
								foreach (var property in section.GetType().GetProperties())
								{
									foreach (var attribute in property.GetCustomAttributes(typeof(MifareClassicMemoryAttribute), true)
										.Cast<MifareClassicMemoryAttribute>()
										.Where(y => y.Name == item.Field)
									)
									{
										property.SetValue(section, Activator.CreateInstance(property.PropertyType, item.NewValue));
										script.Write(script.Response, attribute.Sector, attribute.Block, item.NewValue, attribute.StartBit, attribute.EndBit);
									}
								}
							}
						}
						else if (item.Action == GreyList.ActionType.ModifyField)
						{
							foreach (var section in script.Card.GetType().GetProperties()
								.Select(x => x.GetValue(script.Card))
							)
							{
								foreach (var property in section.GetType().GetProperties())
								{
									foreach (var attribute in property.GetCustomAttributes(typeof(MifareClassicMemoryAttribute), true)
										.Cast<MifareClassicMemoryAttribute>()
										.Where(y => y.Name == item.Field)
									)
									{
										property.SetValue(section, Activator.CreateInstance(property.PropertyType, item.NewValue.FromHexadecimal()));
										script.Write(script.Response, attribute.Sector, attribute.Block, item.NewValue, attribute.StartBit, attribute.EndBit);
									}
								}
							}
						}
						else if (item.Action == GreyList.ActionType.ModifyBlock)
						{
							var block = Convert.ToInt32(item.Field.Substring(3));
							var sector = block / 4;
							block = block % 4;
							changedField = script.Card.Sectors[sector].Blocks[block].Value;
							script.Card.Sectors[sector].Blocks[block].Value = item.NewValue.FromHexadecimal();
							script.Write(script.Response, (byte)sector, (byte)block, item.NewValue);
						}
						else if (item.Action == GreyList.ActionType.EmitCard)
						{
						}
					}
					var card = (await GreyListRepository.GetAsync())
						.Where(x => x.Uid == uid)
						.FirstOrDefault();
					await SIGAPuntService.MARCALGAsync(uid, card.NewValue, card.Machine, card.Field, changedField);
					return true;
				}
			}
			return false;
		}
		#endregion InGreyListAsync

		#region IsPersonalizedAsync
		public async Task<bool> IsPersonalizedAsync(long uid, ITransportCardGetIsPersonalizedScript script)
		{
			return await IsPersonalizedAsync(uid, script.Card.Tarjeta.Tipo.Value, script.Card.Tarjeta.Subtipo.Value);
		}
		public async Task<bool> IsPersonalizedAsync(long uid, SigapuntScript script)
		{
			return await IsPersonalizedAsync(uid, script.Tipo, script.Subtipo);
		}
		private async Task<bool> IsPersonalizedAsync(long uid, EigeTipoTarjetaEnum? tipo, EigeSubtipoTarjetaEnum? subtipo)
		{
			return await Task.Run(() =>
			{
				return
					//(tipo == EigeTipoTarjetaEnum.Viajero) ||
					(tipo == EigeTipoTarjetaEnum.ViajeroPersonalizado) ||
					(tipo == EigeTipoTarjetaEnum.Inspector) ||
					(tipo == EigeTipoTarjetaEnum.Empleado) ||
					//(tipo == EigeTipoTarjetaEnum.Pase) ||
					(tipo == EigeTipoTarjetaEnum.PasePersonalizado) ||
					(tipo == EigeTipoTarjetaEnum.Expendedor) ||
					(
						(tipo == EigeTipoTarjetaEnum.TarjetaCiudadana) &&
						(
							(subtipo == EigeSubtipoTarjetaEnum.Ciudadano_Empradronado) ||
							(subtipo == EigeSubtipoTarjetaEnum.Ciudadano_Forastero) ||
							(subtipo == EigeSubtipoTarjetaEnum.Ciudadano_GrupoMunicipio) ||
							(subtipo == EigeSubtipoTarjetaEnum.Ciudadano_GrupoOtroMunicipio) ||
							(subtipo == EigeSubtipoTarjetaEnum.Ciudadano_EmpleadoAyuntamiento) ||
							(subtipo == EigeSubtipoTarjetaEnum.Ciudadano_Estudianto_Joven) ||
							(subtipo == EigeSubtipoTarjetaEnum.Ciudadano_Jubilado) ||
							(subtipo == EigeSubtipoTarjetaEnum.Ciudadano_Municipal) ||
							//(subtipo == EigeSubtipoTarjetaEnum.Ciudadano_ViajeroNoPerso) ||
							(subtipo == EigeSubtipoTarjetaEnum.Ciudadano_ViajeroPerso) ||
							(subtipo == EigeSubtipoTarjetaEnum.Ciudadano_EstudianteOtroId) ||
							(subtipo == EigeSubtipoTarjetaEnum.Ciudadano_MinusvalidoOtroId) ||
							(subtipo == EigeSubtipoTarjetaEnum.Ciudadano_Desempleado) ||
							(subtipo == EigeSubtipoTarjetaEnum.Ciudadano_SubvencionesSociales) ||
							(subtipo == EigeSubtipoTarjetaEnum.Ciudadano_FamiliaNumerosaGeneral20) ||
							(subtipo == EigeSubtipoTarjetaEnum.Ciudadano_FamiliaNumerosaEspecial50)
						)
					) ||
					//(tipo == EigeTipoTarjetaEnum.ValenciaCardNoPerso) ||
					(tipo == EigeTipoTarjetaEnum.ValenciaCardPerso) ||
					(tipo == EigeTipoTarjetaEnum.TarifaEspecial) ||
					(
						(tipo == EigeTipoTarjetaEnum.DispotivoMovil) &&
						(subtipo != EigeSubtipoTarjetaEnum.Movil_ViajeroNoPerso)
					);
			});
		}
		#endregion IsPersonalizedAsync

		#region IsYoungCardAsync
		public async Task<bool> IsYoungCardAsync(long uid, ITransportCardGetCardTypeNameScript script)
		{
			return await IsYoungCardAsync(uid, script.Card.Tarjeta.Tipo.Value, script.Card.Tarjeta.Subtipo.Value);
		}
		public async Task<bool> IsYoungCardAsync(long uid, SigapuntScript script)
		{
			return await IsYoungCardAsync(uid, script.Tipo, script.Subtipo);
		}
		private async Task<bool> IsYoungCardAsync(long uid, EigeTipoTarjetaEnum? tipo, EigeSubtipoTarjetaEnum? subtipo)
		{
			return await Task.Run(() =>
			{
				return
					(tipo == EigeTipoTarjetaEnum.ViajeroPersonalizado) &&
					(
						(subtipo == EigeSubtipoTarjetaEnum.ViajeroPerso_JoveConDni) ||
						(subtipo == EigeSubtipoTarjetaEnum.ViajeroPerso_JoveConOtroId)
					);
			});
		}
		#endregion IsYoungCardAsync

		#region IsDamagedAsync
		public async Task<bool> IsDamagedAsync(long uid, IMifareClassicScript<EigeCard> script)
		{
			for (int i = 0; i < 16; i++)
				for (int j = 0; j < 4; j++)
					if (!await script.Card.CheckImportantAsync(script.Card.Sectors[i].Blocks[j]))
						return true;

			return false;
		}
		#endregion IsDamagedAsync

		#region GetUserCodeAsync
		public async Task<long?> GetUserCodeAsync(long uid, SigapuntScript script)
		{
			return await Task.Run(() =>
			{
				return script.CodigoViajero;
			});
		}
		public async Task<long?> GetUserCodeAsync(long uid, ITransportCardGetUserNameScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[1].Blocks[0]))
			))
				return null;

			return await Task.Run(() =>
			{
				return script.Card.Usuario.CodigoViajero.Value;
			});
		}
		#endregion GetUserCodeAsync

		#region GetUserNameAsync
		public async Task<string> GetUserNameAsync(long uid, SigapuntScript script)
		{
			return await Task.Run(() =>
			{
				return (script.Nombre).Trim();
			});
		}
		public async Task<string> GetUserNameAsync(long uid, ITransportCardGetUserNameScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[2].Blocks[1])) &&
				(await script.Card.CheckAsync(uid, script.Card.Sectors[2].Blocks[2]))
			))
				return "";

			return await Task.Run(() =>
			{
				return (
					script.Card.Usuario.Nombre.Value.Trim() + " " +
					(script.Card.Usuario.ExtranjeroSinDocumentacion.Value && !script.Card.Usuario.TipoIdentificacion2.Value ?
						"" :
						script.Card.Usuario.Nombre2.Value.Trim())
				).Trim();
			});
		}
		#endregion GetUserNameAsync

		#region GetUserSurnameAsync
		public async Task<string> GetUserSurnameAsync(long uid, SigapuntScript script)
		{
			return await Task.Run(() =>
			{
				return script.Apellidos;
			});
		}
		public async Task<string> GetUserSurnameAsync(long uid, ITransportCardGetUserNameScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[3].Blocks[2]))
			))
				return "";


			return script.Card.Usuario.Apellidos.Value.Trim();
		}
		#endregion GetUserSurnameAsync

		#region GetUserDniAsync
		public async Task<string> GetUserDniAsync(long uid, SigapuntScript script)
		{
			return await Task.Run(() =>
			{
				if (script.Dni == null)
					return "";

				return script.Dni;
			});
		}
		public async Task<string> GetUserDniAsync(long uid, ITransportCardGetUserDniScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[2].Blocks[2]))
			))
				return "";
			return await Task.Run(() =>
			{
				return
					!script.Card.Usuario.ExtranjeroSinDocumentacion.Value && !script.Card.Usuario.TipoIdentificacion2.Value ?
						script.Card.Usuario.Dni_Numero + script.Card.Usuario.Dni_Letra :
					!script.Card.Usuario.ExtranjeroSinDocumentacion.Value && script.Card.Usuario.TipoIdentificacion2.Value ?
						"X" + script.Card.Usuario.Dni_Numero + script.Card.Usuario.Dni_Letra :
					script.Card.Usuario.ExtranjeroSinDocumentacion.Value && script.Card.Usuario.TipoIdentificacion2.Value ?
						script.Card.Usuario.Cif_Letra + script.Card.Usuario.Cif_Numero + script.Card.Usuario.Cif_Letra2 :
						script.Card.Usuario.Dni + script.Card.Usuario.Nombre2;
			});
		}
		#endregion GetUserDniAsync

		#region Titulos
		Dictionary<int, Titulo> Titulos = new Dictionary<int, Titulo>
		{
			{1, new Titulo {Code=1,Name="BONO ANUAL JUBILADOS",Owner="Ayto. Alboraya"} },
			{2, new Titulo {Code=2,Name="BONO ANUAL JOVENES",Owner="Ayto. Alboraya"} },
			{3, new Titulo {Code=3,Name="BONO ANUAL NORMAL",Owner="Ayto. Alboraya"} },
			{4, new Titulo {Code=4,Name="BONO SEMESTRAL JOVENES",Owner="Ayto. Alboraya"} },
			{5, new Titulo {Code=5,Name="BONO SEMESTRAL NORMAL",Owner="Ayto. Alboraya"} },
			{6, new Titulo {Code=6,Name="BONO TRIMESTRAL JOVENES",Owner="Ayto. Alboraya"} },
			{7, new Titulo {Code=7,Name="BONO TRIMESTRAL NORMAL",Owner="Ayto. Alboraya"} },
			{8, new Titulo {Code=8,Name="BONO DE 10 VIAJES",Owner="Ayto. Alboraya"} },
			{9, new Titulo {Code=9,Name="BILLETE SENCILLO",Owner="Ayto. Alboraya"} },
			{10, new Titulo {Code=10,Name="Reservado Futuros Usos",Owner="Ayto. Alboraya"} },
			{11, new Titulo {Code=11,Name="Bono 10 Paterna",Owner="Ayto. Paterna"} },
			{12, new Titulo {Code=12,Name="Bono 10 Metropolitano Paterna",Owner="Ayto. Paterna"} },
			{13, new Titulo {Code=13,Name="Bono 10 Metropolitano Transbordo Paterna",Owner="Ayto. Paterna"} },
			{14, new Titulo {Code=14,Name="Abono Transporte Urbano de Paterna",Owner="Ayto. Paterna"} },
			{15, new Titulo {Code=15,Name="Bono Personalizado Estudiante",Owner="Ayto. Paterna"} },
			{16, new Titulo {Code=16,Name="Bono Personalizado Jubilado",Owner="Ayto. Paterna"} },
			{17, new Titulo {Code=17,Name="Bono Personalizado Municipal",Owner="Ayto. Paterna"} },
			{18, new Titulo {Code=18,Name="Bono 10 Estudiante",Owner="Ayto. Paterna"} },
			{19, new Titulo {Code=19,Name="Bono 10 Jubilado",Owner="Ayto. Paterna"} },
			{20, new Titulo {Code=20,Name="Bono 10Municipal",Owner="Ayto. Paterna"} },
			{21, new Titulo {Code=21,Name="Bono 10 Normal",Owner="Ayto. de Castellón"} },
			{22, new Titulo {Code=22,Name="Bono 10 Joven",Owner="Ayto. de Castellón"} },
			{23, new Titulo {Code=23,Name="Bono 30 Días",Owner="Ayto. de Castellón"} },
			{24, new Titulo {Code=24,Name="Bono Ilimitado Jubilado",Owner="Ayto. de Castellón"} },
			{25, new Titulo {Code=25,Name="Bono Ilimitado Subvención Social",Owner="Ayto. de Castellón"} },
			{26, new Titulo {Code=26,Name="Reservado Título Castellón",Owner="Ayto. de Castellón"} },
			{27, new Titulo {Code=27,Name="Reservado Título Castellón",Owner="Ayto. de Castellón"} },
			{28, new Titulo {Code=28,Name="Reservado Título Castellón",Owner="Ayto. de Castellón"} },
			{29, new Titulo {Code=29,Name="Reservado Título Castellón",Owner="Ayto. de Castellón"} },
			{30, new Titulo {Code=30,Name="Reservado Título Castellón",Owner="Ayto. de Castellón"} },
			{31, new Titulo {Code=31,Name="Bono desempleado normal",Owner="Ayto. de Castellón"} }, // Rev 074
			{32, new Titulo {Code=32,Name="Bono desempleado reducido",Owner="Ayto. de Castellón"} }, // Rev 074
			{41, new Titulo {Code=41,Name="Bono 10 AVSA, TRANSPORT",Owner="AVSA"} },
			{42, new Titulo {Code=42,Name="Bono 10 AVSA, JUBILAT",Owner="AVSA"} },
			{43, new Titulo {Code=43,Name="Bono 10 AVSA, ESTUDIANT",Owner="AVSA"} },
			{44, new Titulo {Code=44,Name="Bono 10 AVSA, MUNICIPAL",Owner="AVSA"} },
			{45, new Titulo {Code=45,Name="Abono AVSA otorgado por el Ayuntamiento",Owner="AVSA"} },
			{46, new Titulo {Code=46,Name="ESTUDIANT TRANSBORD",Owner="AVSA"} },
			{47, new Titulo {Code=47,Name="Reservado Título AVSA",Owner="AVSA"} },
			{48, new Titulo {Code=48,Name="Reservado Título AVSA",Owner="AVSA"} },
			{49, new Titulo {Code=49,Name="Reservado Título AVSA",Owner="AVSA"} },
			{50, new Titulo {Code=50,Name="Reservado Título AVSA",Owner="AVSA"} },
			{96, new Titulo {Code=96,Name="Bonobús",Owner="EMT "} },
			{112, new Titulo {Code=112,Name="T-1",Owner="Coordinación aVM"} },
			{128, new Titulo {Code=128,Name="Bono Oro",Owner="EMT"} },
			//{368, new Titulo {Code=368,Name="Valencia Tourist Card 24 Hs",Owner="Turismo Valencia"} },
			{368, new Titulo {Code=368,Name="ValenciaCard 24H",Owner="Turismo Valencia"} },
			//{624, new Titulo {Code=624,Name="Valencia Tourist Card 48 Hs",Owner="Turismo Valencia"} },
			{624, new Titulo {Code=624,Name="ValenciaCard 48H",Owner="Turismo Valencia"} },
			{800, new Titulo {Code=800,Name="ESCOLAR",Owner="TUASA (Alcoi)"} },
			{801, new Titulo {Code=801,Name="Reservado TítuloTUASA",Owner="TUASA (Alcoi)"} },
			{802, new Titulo {Code=802,Name="ESTUDIANTE",Owner="TUASA (Alcoi)"} },
			{803, new Titulo {Code=803,Name="Bono Ordinario",Owner="TUASA (Alcoi)"} },
			{804, new Titulo {Code=804,Name="Bono Jubilado (ANTIGUO)",Owner="TUASA (Alcoi)"} },
			{805, new Titulo {Code=805,Name="Bono Estudiante (ANTIGUO)",Owner="TUASA (Alcoi)"} },
			{806, new Titulo {Code=806,Name="Bono Concesión CVA018 combinado urbano Alcoi",Owner="TUASA (Alcoi)"} },
			{807, new Titulo {Code=807,Name="Bono Concesión CVA018",Owner="TUASA (Alcoi)"} },
			{808, new Titulo {Code=808,Name="ET PORTEM PENS. CLASSE 1",Owner="TUASA (Alcoi)"} },
			{809, new Titulo {Code=809,Name="ET PORTEM PENS. CLASSE 2",Owner="TUASA (Alcoi)"} },
			{810, new Titulo {Code=810,Name="ET PORTEM DESEMPLEADOS",Owner="TUASA (Alcoi)"} },
			{811, new Titulo {Code=811,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{812, new Titulo {Code=812,Name="Título Turístico de 24h",Owner="TAM (Alicante)"} },
			{813, new Titulo {Code=813,Name="Título TuriBús",Owner="TAM (Alicante)"} },
			{814, new Titulo {Code=814,Name="Título Congresista",Owner="TAM (Alicante)"} },
			{815, new Titulo {Code=815,Name="Título Turístico de 48h",Owner="TAM (Alicante)"} },
			{816, new Titulo {Code=816,Name="Título Turístico de 72h",Owner="TAM (Alicante)"} },
			{817, new Titulo {Code=817,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{818, new Titulo {Code=818,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{819, new Titulo {Code=819,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{820, new Titulo {Code=820,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{821, new Titulo {Code=821,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{822, new Titulo {Code=822,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{823, new Titulo {Code=823,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{824, new Titulo {Code=824,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{825, new Titulo {Code=825,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{826, new Titulo {Code=826,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{827, new Titulo {Code=827,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{828, new Titulo {Code=828,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{829, new Titulo {Code=829,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{830, new Titulo {Code=830,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{831, new Titulo {Code=831,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{832, new Titulo {Code=832,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{833, new Titulo {Code=833,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{834, new Titulo {Code=834,Name="Bono Oro Mutxamel",Owner="TAM (Alicante)"} },
			{835, new Titulo {Code=835,Name="Bono Oro San Vicente",Owner="TAM (Alicante)"} },
			{836, new Titulo {Code=836,Name="Bono Móbilis unipersonal Bonificado",Owner="TAM (Alicante)"} },
			{837, new Titulo {Code=837,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{838, new Titulo {Code=838,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{839, new Titulo {Code=839,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{840, new Titulo {Code=840,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{841, new Titulo {Code=841,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{842, new Titulo {Code=842,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{843, new Titulo {Code=843,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{844, new Titulo {Code=844,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{845, new Titulo {Code=845,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{846, new Titulo {Code=846,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{847, new Titulo {Code=847,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{848, new Titulo {Code=848,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{849, new Titulo {Code=849,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{850, new Titulo {Code=850,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{851, new Titulo {Code=851,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{852, new Titulo {Code=852,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{853, new Titulo {Code=853,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{854, new Titulo {Code=854,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{855, new Titulo {Code=855,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{856, new Titulo {Code=856,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{857, new Titulo {Code=857,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{858, new Titulo {Code=858,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{859, new Titulo {Code=859,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{860, new Titulo {Code=860,Name="Bono Estudiante Campello",Owner="TAM (Alicante)"} },
			{861, new Titulo {Code=861,Name="Bono Bus 10",Owner="TAM (Alicante)"} },
			{862, new Titulo {Code=862,Name="Bono Móbilis Unipersonal no Bonificado",Owner="TAM (Alicante)"} },
			{863, new Titulo {Code=863,Name="Bono Jove 30",Owner="TAM (Alicante)"} },
			{864, new Titulo {Code=864,Name="Bono Escolar 30",Owner="TAM (Alicante)"} },
			{865, new Titulo {Code=865,Name="Bono Oro Alicante",Owner="TAM (Alicante)"} },
			{866, new Titulo {Code=866,Name="Bono Oro San Juan",Owner="TAM (Alicante)"} },
			{867, new Titulo {Code=867,Name="Bono Oro Campello",Owner="TAM (Alicante)"} },
			{868, new Titulo {Code=868,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{869, new Titulo {Code=869,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{870, new Titulo {Code=870,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{871, new Titulo {Code=871,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{872, new Titulo {Code=872,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{873, new Titulo {Code=873,Name="Bono Jubilado Masatusa",Owner="Masatusa"} },
			{874, new Titulo {Code=874,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{875, new Titulo {Code=875,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{876, new Titulo {Code=876,Name="Bono Empleado Masatusa",Owner="TAM (Alicante)"} },
			{877, new Titulo {Code=877,Name="Bono Empleado Alcoyana",Owner="TAM (Alicante)"} },
			{878, new Titulo {Code=878,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{879, new Titulo {Code=879,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			//{880, new Titulo {Code=880,Name="Valencia Tourist Card 72 Hs",Owner="Turismo Valencia"} },
			{880, new Titulo {Code=880,Name="ValenciaCard 72H",Owner="Turismo Valencia"} },
			{881, new Titulo {Code=881,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{882, new Titulo {Code=882,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{883, new Titulo {Code=883,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{884, new Titulo {Code=884,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{885, new Titulo {Code=885,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{886, new Titulo {Code=886,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{887, new Titulo {Code=887,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{888, new Titulo {Code=888,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{889, new Titulo {Code=889,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{890, new Titulo {Code=890,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{891, new Titulo {Code=891,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{892, new Titulo {Code=892,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{893, new Titulo {Code=893,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{894, new Titulo {Code=894,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{895, new Titulo {Code=895,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{896, new Titulo {Code=896,Name="Nuevo Bono oro Alicante",Owner="TAM (Alicante)"} },
			{897, new Titulo {Code=897,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{898, new Titulo {Code=898,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{899, new Titulo {Code=899,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{900, new Titulo {Code=900,Name="Reservado Título TAM",Owner="TAM (Alicante)"} },
			{901, new Titulo {Code=901,Name="Bono 10 TRAM",Owner="TRAM (FGV Alicante)"} },
			{902, new Titulo {Code=902,Name="Bono 30 TRAM (macrobono)",Owner="TRAM (FGV Alicante)"} },
			{903, new Titulo {Code=903,Name="Bono 30 Jove TRAM (macrobono)",Owner="TRAM (FGV Alicante)"} },
			{904, new Titulo {Code=904,Name="TAT Mensual TRAM",Owner="TRAM (FGV Alicante)"} },
			{905, new Titulo {Code=905,Name="TAT Mensual JoveTRAM",Owner="TRAM (FGV Alicante)"} },
			{906, new Titulo {Code=906,Name="TAT Mensual Gent Major TRAM",Owner="TRAM (FGV Alicante)"} },
			{907, new Titulo {Code=907,Name="TAT Anual Gent Major Major TRAM",Owner="TRAM (FGV Alicante)"} },
			{908, new Titulo {Code=908,Name="Reservado Título TRAM",Owner="TRAM (FGV Alicante)"} },
			{909, new Titulo {Code=909,Name="Reservado Título TRAM",Owner="TRAM (FGV Alicante)"} },
			{910, new Titulo {Code=910,Name="Reservado Título TRAM",Owner="TRAM (FGV Alicante)"} },
			{911, new Titulo {Code=911,Name="Reservado Título TRAM",Owner="TRAM (FGV Alicante)"} },
			{912, new Titulo {Code=912,Name="Reservado Título TRAM",Owner="TRAM (FGV Alicante)"} },
			{913, new Titulo {Code=913,Name="PENSIONISTA IDA",Owner="TRAM (FGV Alicante)"} },
			{914, new Titulo {Code=914,Name="PENSIONISTA IDA/VUELTA",Owner="TRAM (FGV Alicante)"} },
			{915, new Titulo {Code=915,Name="Reservado Título TRAM",Owner="TRAM (FGV Alicante)"} },
			{916, new Titulo {Code=916,Name="Reservado Título TRAM",Owner="TRAM (FGV Alicante)"} },
			{917, new Titulo {Code=917,Name="Reservado Título TRAM",Owner="TRAM (FGV Alicante)"} },
			{918, new Titulo {Code=918,Name="Reservado Título TRAM",Owner="TRAM (FGV Alicante)"} },
			{919, new Titulo {Code=919,Name="Reservado Título TRAM",Owner="TRAM (FGV Alicante)"} },
			{920, new Titulo {Code=920,Name="Reservado Título TRAM",Owner="TRAM (FGV Alicante)"} },
			{1001, new Titulo {Code=1001,Name="Sencillo (TSC)",Owner="FGV (Valencia)"} },
			{1002, new Titulo {Code=1002,Name="Ida y Vuelta (TSC)",Owner="FGV (Valencia)"} },
			{1003, new Titulo {Code=1003,Name="Bonometro",Owner="FGV (Valencia)"} },
			{1004, new Titulo {Code=1004,Name="TAT",Owner="FGV (Valencia)"} },
			{1005, new Titulo {Code=1005,Name="TAT Jove",Owner="FGV (Valencia)"} },
			{1006, new Titulo {Code=1006,Name="TAT Gent Major Mensual",Owner="FGV (Valencia)"} },
			{1007, new Titulo {Code=1007,Name="TAT Gent Major Anual",Owner="FGV (Valencia)"} },
			{1008, new Titulo {Code=1008,Name="TAT Familiar fin de semana",Owner="FGV (Valencia)"} },
			{1009, new Titulo {Code=1009,Name="SOV (TSC)",Owner="FGV (Valencia)"} },
			{1010, new Titulo {Code=1010,Name="Sencillo 20% (TSC)",Owner="FGV (Valencia)"} },
			{1011, new Titulo {Code=1011,Name="Sencillo 50% (TSC)",Owner="FGV (Valencia)"} },
			{1012, new Titulo {Code=1012,Name="Familiares de agentes de FGV en activo",Owner="FGV (Valencia)"} },
			{1013, new Titulo {Code=1013,Name="Pensionistas de FGV y FEVE (transferidos a FGV)",Owner="FGV (Valencia)"} },
			{1014, new Titulo {Code=1014,Name="Familiares de pensionistas de FGV y FEVE (transferidos a FGV)",Owner="FGV (Valencia)"} },
			{1015, new Titulo {Code=1015,Name="Pase de validez Limitada (Personalizado)",Owner="FGV (Valencia)"} },
			{1016, new Titulo {Code=1016,Name="Pase de validez Limitada (No personalizado)",Owner="FGV (Valencia)"} },
			{1017, new Titulo {Code=1017,Name="GTP",Owner="FGV (Valencia)"} },
			{1018, new Titulo {Code=1018,Name="Bono Promoción",Owner="FGV (Valencia)"} },
			{1019, new Titulo {Code=1019,Name="Bono EMT",Owner="FGV (Valencia)"} },
			{1020, new Titulo {Code=1020,Name="TAT Anual",Owner="FGV (Valencia)"} },
			{1021, new Titulo {Code=1021,Name="TAT Anual / Trimestre 1",Owner="FGV (Valencia)"} },
			{1022, new Titulo {Code=1022,Name="TAT Anual / Trimestre 2",Owner="FGV (Valencia)"} },
			{1023, new Titulo {Code=1023,Name="TAT Anual / Trimestre 3",Owner="FGV (Valencia)"} },
			{1024, new Titulo {Code=1024,Name="TAT Anual / Trimestre 4",Owner="FGV (Valencia)"} },
			{1025, new Titulo {Code=1025,Name="Bonometro 10%",Owner="FGV (Valencia)"} },
			{1026, new Titulo {Code=1026,Name="Pase inspección/mantenimiento de peaje",Owner="FGV (Valencia)"} },
			{1027, new Titulo {Code=1027,Name="TAT Mobilitat mensual",Owner="FGV (Valencia)"} },
			{1028, new Titulo {Code=1028,Name="TAT Mobilitat anual",Owner="FGV (Valencia)"} },
			{1029, new Titulo {Code=1029,Name="Bono 1",Owner="FGV (Valencia)"} },
			{1030, new Titulo {Code=1030,Name="Billete de salida",Owner="FGV (Valencia)"} },
			{1031, new Titulo {Code=1031,Name="TAT Trimestral Colegio Mayor La Coma",Owner="FGV (Valencia)"} },
			{1032, new Titulo {Code=1032,Name="TAT Trimestral La Coma residente",Owner="FGV (Valencia)"} },
			{1033, new Titulo {Code=1033,Name="CEU Trimestral",Owner="FGV (Valencia)"} },
			{1034, new Titulo {Code=1034,Name="UCV - 9 M",Owner="FGV (Valencia)"} },
			{1035, new Titulo {Code=1035,Name="UCV - 10 M",Owner="FGV (Valencia)"} },
			{1036, new Titulo {Code=1036,Name="Convenio IALE Trimestral escolar",Owner="FGV (Valencia)"} },
			{1037, new Titulo {Code=1037,Name="Convenio IALE Anual escolar",Owner="FGV (Valencia)"} },
			{1038, new Titulo {Code=1038,Name="Escola La Masia",Owner="FGV (Valencia)"} },
			{1039, new Titulo {Code=1039,Name="EPLA",Owner="FGV (Valencia)"} },
			{1040, new Titulo {Code=1040,Name="Colegio del Vedat Trimestral",Owner="FGV (Valencia)"} },
			{1041, new Titulo {Code=1041,Name="Colegio El Vedat",Owner="FGV (Valencia)"} },
			{1042, new Titulo {Code=1042,Name="UVEG Mensual",Owner="FGV (Valencia)"} },
			{1043, new Titulo {Code=1043,Name="UVEG Mensual Jove",Owner="FGV (Valencia)"} },
			{1044, new Titulo {Code=1044,Name="UVEG Anual",Owner="FGV (Valencia)"} },
			{1045, new Titulo {Code=1045,Name="Hospital General mensual",Owner="FGV (Valencia)"} },
			{1046, new Titulo {Code=1046,Name="Convenio con AENA",Owner="FGV (Valencia)"} },
			{1047, new Titulo {Code=1047,Name="UCV - Edetania Trimestral",Owner="FGV (Valencia)"} },
			{1048, new Titulo {Code=1048,Name="Bonometro FN20",Owner="FGV (Valencia)"} },
			{1049, new Titulo {Code=1049,Name="E.P. La Salle Anual",Owner="FGV (Valencia)"} },
			{1050, new Titulo {Code=1050,Name="Estudiantes Ayto. Burjassot",Owner="FGV (Valencia)"} },
			{1051, new Titulo {Code=1051,Name="Estudiantes Ayto. Moncada",Owner="FGV (Valencia)"} },
			{1052, new Titulo {Code=1052,Name="Estudiantes Ayto. Rocafort",Owner="FGV (Valencia)"} },
			{1053, new Titulo {Code=1053,Name="Estudiantes Ayto. La Pobla de Farnals",Owner="FGV (Valencia)"} },
			{1054, new Titulo {Code=1054,Name="Estudiantes Ayto. Bétera",Owner="FGV (Valencia)"} },
			{1055, new Titulo {Code=1055,Name="Estudiantes Ayto. Massamagrell",Owner="FGV (Valencia)"} },
			{1056, new Titulo {Code=1056,Name="Estudiantes Ayto. Godella",Owner="FGV (Valencia)"} },
			{1057, new Titulo {Code=1057,Name="Bono 60x60",Owner="FGV (Valencia)"} },
			{1058, new Titulo {Code=1058,Name="Pensionistas Ayto. Godella",Owner="FGV (Valencia)"} },
			{1059, new Titulo {Code=1059,Name="Pensionistas Ayto. Massamagrell",Owner="FGV (Valencia)"} },
			{1060, new Titulo {Code=1060,Name="Pensionistas Ayto. Museros",Owner="FGV (Valencia)"} },
			{1061, new Titulo {Code=1061,Name="Pensionistas Ayto. Foios",Owner="FGV (Valencia)"} },
			{1062, new Titulo {Code=1062,Name="Pensionistas Ayto. Rafelbunyol",Owner="FGV (Valencia)"} },
			{1063, new Titulo {Code=1063,Name="Pensionistas Ayto. Moncada",Owner="FGV (Valencia)"} },
			{1064, new Titulo {Code=1064,Name="Pensionistas Ayto. Meliana",Owner="FGV (Valencia)"} },
			{1065, new Titulo {Code=1065,Name="Pensionistas Ayto. Emperador",Owner="FGV (Valencia)"} },
			{1066, new Titulo {Code=1066,Name="Pensionistas Ayto. Albalat dels Sorells",Owner="FGV (Valencia)"} },
			{1067, new Titulo {Code=1067,Name="Pensionistas Ayto. La Pobla de Farnals",Owner="FGV (Valencia)"} },
			{1068, new Titulo {Code=1068,Name="Pensionistas Ayto. Almàssera",Owner="FGV (Valencia)"} },
			{1069, new Titulo {Code=1069,Name="Pensionistas Ayto. Rocafort",Owner="FGV (Valencia)"} },
			{1070, new Titulo {Code=1070,Name="Pensionistas Ayto. Alboraya",Owner="FGV (Valencia)"} },
			{1071, new Titulo {Code=1071,Name="Pensionistas Ayto. Mislata",Owner="FGV (Valencia)"} },
			{1072, new Titulo {Code=1072,Name="Pensionistas Ayto. Alfara del Patriarca",Owner="FGV (Valencia)"} },
			{1073, new Titulo {Code=1073,Name="Pensionistas Ayto. Manises",Owner="FGV (Valencia)"} },
			{1074, new Titulo {Code=1074,Name="UPV Mensual",Owner="FGV (Valencia)"} },
			{1075, new Titulo {Code=1075,Name="UPV Mensual Jove",Owner="FGV (Valencia)"} },
			{1076, new Titulo {Code=1076,Name="Estudiantes Ayto. Albalat dels Sorells",Owner="FGV (Valencia)"} },
			{1077, new Titulo {Code=1077,Name="CEU Mensual",Owner="FGV (Valencia)"} },
			{1078, new Titulo {Code=1078,Name="CEU Mensual Jove",Owner="FGV (Valencia)"} },
			{1079, new Titulo {Code=1079,Name="Pensionistas Ayto. Paterna",Owner="FGV (Valencia)"} },
			{1080, new Titulo {Code=1080,Name="Estudiantes Ayto. Paterna 9 meses",Owner="FGV (Valencia)"} },
			{1081, new Titulo {Code=1081,Name="Estudiantes Ayto. Paterna Mensual",Owner="FGV (Valencia)"} },
			{1082, new Titulo {Code=1082,Name="Estudiantes Ayto. Paterna Mensual Jove",Owner="FGV (Valencia)"} },
			{1083, new Titulo {Code=1083,Name="Estudiantes Ayto. Paterna Bonometro 10%",Owner="FGV (Valencia)"} },
			{1084, new Titulo {Code=1084,Name="Fundación La Fé Bonometro 10%",Owner="FGV (Valencia)"} },
			{1085, new Titulo {Code=1085,Name="Fundación La Fé Mensual",Owner="FGV (Valencia)"} },
			{1086, new Titulo {Code=1086,Name="Fundación La Fé Mensual Jove",Owner="FGV (Valencia)"} },
			{1087, new Titulo {Code=1087,Name="Pens. Ayto. Rafelbunyol anual",Owner="FGV (Valencia)"} },
			{1088, new Titulo {Code=1088,Name="Hospital General mensual jove",Owner="FGV (Valencia)"} },
			{1089, new Titulo {Code=1089,Name="Hospital General bonometro 10%",Owner="FGV (Valencia)"} },
			{1090, new Titulo {Code=1090,Name="Billete zonal de expecdición manual",Owner="FGV (Valencia)"} },
			{1091, new Titulo {Code=1091,Name="Billete modelo I-4",Owner="FGV (Valencia)"} },
			{1092, new Titulo {Code=1092,Name="Sencillo (Papel térmico)",Owner="FGV (Valencia)"} },
			{1093, new Titulo {Code=1093,Name="Sencillo 20% (Papel térmico)",Owner="FGV (Valencia)"} },
			{1094, new Titulo {Code=1094,Name="Sencillo 50% (Papel térmico)",Owner="FGV (Valencia)"} },
			{1095, new Titulo {Code=1095,Name="SOV (Papel térmico)",Owner="FGV (Valencia)"} },
			{1096, new Titulo {Code=1096,Name="Billete colectivo (Grupo) (Papel térmico)",Owner="FGV (Valencia)"} },
			{1097, new Titulo {Code=1097,Name="Billete de sustitución (Papel térmico)",Owner="FGV (Valencia)"} },
			{1098, new Titulo {Code=1098,Name="Suplemento valor doble sencillo (Papel térmico)",Owner="FGV (Valencia)"} },
			{1099, new Titulo {Code=1099,Name="Suplemento valor 10€ (Papel térmico)",Owner="FGV (Valencia)"} },
			{1100, new Titulo {Code=1100,Name="Suplemento valor 50€ (Papel térmico)",Owner="FGV (Valencia)"} },
			{1101, new Titulo {Code=1101,Name="Reservado Título EMT",Owner="EMT"} },
			{1102, new Titulo {Code=1102,Name="Reservado Título EMT",Owner="EMT"} },
			{1103, new Titulo {Code=1103,Name="Reservado Título EMT",Owner="EMT"} },
			{1104, new Titulo {Code=1104,Name="Reservado Título EMT",Owner="EMT"} },
			{1105, new Titulo {Code=1105,Name="Pensionistas EMT",Owner="EMT"} },
			{1106, new Titulo {Code=1106,Name="Reservado Título EMT",Owner="EMT"} },
			{1107, new Titulo {Code=1107,Name="Reservado Título EMT",Owner="EMT"} },
			{1108, new Titulo {Code=1108,Name="Reservado Título EMT",Owner="EMT"} },
			{1109, new Titulo {Code=1109,Name="Reservado Título EMT",Owner="EMT"} },
			{1110, new Titulo {Code=1110,Name="Reservado Título EMT",Owner="EMT"} },
			{1111, new Titulo {Code=1111,Name="Reservado Título EMT",Owner="EMT"} },
			{1112, new Titulo {Code=1112,Name="Reservado Título EMT",Owner="EMT"} },
			{1113, new Titulo {Code=1113,Name="Reservado Título EMT",Owner="EMT"} },
			{1114, new Titulo {Code=1114,Name="Reservado Título EMT",Owner="EMT"} },
			{1115, new Titulo {Code=1115,Name="Reservado Título EMT",Owner="EMT"} },
			{1116, new Titulo {Code=1116,Name="Reservado Título EMT",Owner="EMT"} },
			{1117, new Titulo {Code=1117,Name="Reservado Título EMT",Owner="EMT"} },
			{1118, new Titulo {Code=1118,Name="Reservado Título EMT",Owner="EMT"} },
			{1119, new Titulo {Code=1119,Name="Reservado Título EMT",Owner="EMT"} },
			{1120, new Titulo {Code=1120,Name="Reservado Título EMT",Owner="EMT"} },
			{1121, new Titulo {Code=1121,Name="Reservado Título EMT",Owner="EMT"} },
			{1122, new Titulo {Code=1122,Name="Reservado Título EMT",Owner="EMT"} },
			{1123, new Titulo {Code=1123,Name="Reservado Título EMT",Owner="EMT"} },
			{1124, new Titulo {Code=1124,Name="Reservado Título EMT",Owner="EMT"} },
			{1125, new Titulo {Code=1125,Name="Reservado Título EMT",Owner="EMT"} },
			{1126, new Titulo {Code=1126,Name="Reservado Título EMT",Owner="EMT"} },
			{1127, new Titulo {Code=1127,Name="Reservado Título EMT",Owner="EMT"} },
			{1128, new Titulo {Code=1128,Name="Reservado Título EMT",Owner="EMT"} },
			{1129, new Titulo {Code=1129,Name="Reservado Título EMT",Owner="EMT"} },
			{1130, new Titulo {Code=1130,Name="Reservado Título EMT",Owner="EMT"} },
			{1131, new Titulo {Code=1131,Name="Reservado Título EMT",Owner="EMT"} },
			{1132, new Titulo {Code=1132,Name="Reservado Título EMT",Owner="EMT"} },
			{1133, new Titulo {Code=1133,Name="Reservado Título EMT",Owner="EMT"} },
			{1134, new Titulo {Code=1134,Name="Reservado Título EMT",Owner="EMT"} },
			{1135, new Titulo {Code=1135,Name="Reservado Título EMT",Owner="EMT"} },
			{1136, new Titulo {Code=1136,Name="Reservado Título EMT",Owner="EMT"} },
			{1137, new Titulo {Code=1137,Name="Reservado Título EMT",Owner="EMT"} },
			{1138, new Titulo {Code=1138,Name="Reservado Título EMT",Owner="EMT"} },
			{1139, new Titulo {Code=1139,Name="Reservado Título EMT",Owner="EMT"} },
			{1140, new Titulo {Code=1140,Name="Reservado Título EMT",Owner="EMT"} },
			{1141, new Titulo {Code=1141,Name="Reservado Título EMT",Owner="EMT"} },
			{1142, new Titulo {Code=1142,Name="Reservado Título EMT",Owner="EMT"} },
			{1143, new Titulo {Code=1143,Name="Reservado Título EMT",Owner="EMT"} },
			{1144, new Titulo {Code=1144,Name="Reservado Título EMT",Owner="EMT"} },
			{1145, new Titulo {Code=1145,Name="Reservado Título EMT",Owner="EMT"} },
			{1146, new Titulo {Code=1146,Name="Reservado Título EMT",Owner="EMT"} },
			{1147, new Titulo {Code=1147,Name="Reservado Título EMT",Owner="EMT"} },
			{1148, new Titulo {Code=1148,Name="Reservado Título EMT",Owner="EMT"} },
			{1149, new Titulo {Code=1149,Name="Reservado Título EMT",Owner="EMT"} },
			{1150, new Titulo {Code=1150,Name="Reservado Título EMT",Owner="EMT"} },
			{1151, new Titulo {Code=1151,Name="Reservado Título EMT",Owner="EMT"} },
			{1152, new Titulo {Code=1152,Name="Reservado Título EMT",Owner="EMT"} },
			{1153, new Titulo {Code=1153,Name="Reservado Título EMT",Owner="EMT"} },
			{1154, new Titulo {Code=1154,Name="Reservado Título EMT",Owner="EMT"} },
			{1155, new Titulo {Code=1155,Name="Reservado Título EMT",Owner="EMT"} },
			{1156, new Titulo {Code=1156,Name="Reservado Título EMT",Owner="EMT"} },
			{1157, new Titulo {Code=1157,Name="Reservado Título EMT",Owner="EMT"} },
			{1158, new Titulo {Code=1158,Name="Reservado Título EMT",Owner="EMT"} },
			{1159, new Titulo {Code=1159,Name="Reservado Título EMT",Owner="EMT"} },
			{1160, new Titulo {Code=1160,Name="Reservado Título EMT",Owner="EMT"} },
			{1161, new Titulo {Code=1161,Name="Reservado Título EMT",Owner="EMT"} },
			{1162, new Titulo {Code=1162,Name="Reservado Título EMT",Owner="EMT"} },
			{1163, new Titulo {Code=1163,Name="Reservado Título EMT",Owner="EMT"} },
			{1164, new Titulo {Code=1164,Name="Reservado Título EMT",Owner="EMT"} },
			{1165, new Titulo {Code=1165,Name="Reservado Título EMT",Owner="EMT"} },
			{1166, new Titulo {Code=1166,Name="Reservado Título EMT",Owner="EMT"} },
			{1167, new Titulo {Code=1167,Name="Reservado Título EMT",Owner="EMT"} },
			{1168, new Titulo {Code=1168,Name="Reservado Título EMT",Owner="EMT"} },
			{1169, new Titulo {Code=1169,Name="Reservado Título EMT",Owner="EMT"} },
			{1170, new Titulo {Code=1170,Name="Reservado Título EMT",Owner="EMT"} },
			{1171, new Titulo {Code=1171,Name="Reservado Título EMT",Owner="EMT"} },
			{1172, new Titulo {Code=1172,Name="Reservado Título EMT",Owner="EMT"} },
			{1173, new Titulo {Code=1173,Name="Reservado Título EMT",Owner="EMT"} },
			{1174, new Titulo {Code=1174,Name="Reservado Título EMT",Owner="EMT"} },
			{1175, new Titulo {Code=1175,Name="Reservado Título EMT",Owner="EMT"} },
			{1176, new Titulo {Code=1176,Name="Reservado Título EMT",Owner="EMT"} },
			{1177, new Titulo {Code=1177,Name="Reservado Título EMT",Owner="EMT"} },
			{1178, new Titulo {Code=1178,Name="Reservado Título EMT",Owner="EMT"} },
			{1179, new Titulo {Code=1179,Name="Reservado Título EMT",Owner="EMT"} },
			{1180, new Titulo {Code=1180,Name="Reservado Título EMT",Owner="EMT"} },
			{1181, new Titulo {Code=1181,Name="Reservado Título EMT",Owner="EMT"} },
			{1182, new Titulo {Code=1182,Name="Reservado Título EMT",Owner="EMT"} },
			{1183, new Titulo {Code=1183,Name="Reservado Título EMT",Owner="EMT"} },
			{1184, new Titulo {Code=1184,Name="Reservado Título EMT",Owner="EMT"} },
			{1185, new Titulo {Code=1185,Name="Reservado Título EMT",Owner="EMT"} },
			{1186, new Titulo {Code=1186,Name="Reservado Título EMT",Owner="EMT"} },
			{1187, new Titulo {Code=1187,Name="Reservado Título EMT",Owner="EMT"} },
			{1188, new Titulo {Code=1188,Name="Reservado Título EMT",Owner="EMT"} },
			{1189, new Titulo {Code=1189,Name="Reservado Título EMT",Owner="EMT"} },
			{1190, new Titulo {Code=1190,Name="Reservado Título EMT",Owner="EMT"} },
			{1191, new Titulo {Code=1191,Name="Reservado Título EMT",Owner="EMT"} },
			{1192, new Titulo {Code=1192,Name="Reservado Título EMT",Owner="EMT"} },
			{1193, new Titulo {Code=1193,Name="Reservado Título EMT",Owner="EMT"} },
			{1194, new Titulo {Code=1194,Name="Reservado Título EMT",Owner="EMT"} },
			{1195, new Titulo {Code=1195,Name="Reservado Título EMT",Owner="EMT"} },
			{1196, new Titulo {Code=1196,Name="Reservado Título EMT",Owner="EMT"} },
			{1197, new Titulo {Code=1197,Name="Reservado Título EMT",Owner="EMT"} },
			{1198, new Titulo {Code=1198,Name="Reservado Título EMT",Owner="EMT"} },
			{1199, new Titulo {Code=1199,Name="Reservado Título EMT",Owner="EMT"} },
			{1200, new Titulo {Code=1200,Name="Reservado Título EMT",Owner="EMT"} },
			{1201, new Titulo {Code=1201,Name="Reservado FGV",Owner="FGV"} },
			{1202, new Titulo {Code=1202,Name="Reservado FGV",Owner="FGV"} },
			{1203, new Titulo {Code=1203,Name="Reservado FGV",Owner="FGV"} },
			{1204, new Titulo {Code=1204,Name="Reservado FGV",Owner="FGV"} },
			{1205, new Titulo {Code=1205,Name="Reservado FGV",Owner="FGV"} },
			{1206, new Titulo {Code=1206,Name="Reservado FGV",Owner="FGV"} },
			{1207, new Titulo {Code=1207,Name="Reservado FGV",Owner="FGV"} },
			{1208, new Titulo {Code=1208,Name="Reservado FGV",Owner="FGV"} },
			{1209, new Titulo {Code=1209,Name="Reservado FGV",Owner="FGV"} },
			{1210, new Titulo {Code=1210,Name="Pase empleado RENFE",Owner="FGV"} },
			{1211, new Titulo {Code=1211,Name="Pase empleado ADIF",Owner="FGV"} },
			{1212, new Titulo {Code=1212,Name="Reservado FGV",Owner="FGV"} },
			{1213, new Titulo {Code=1213,Name="Reservado FGV",Owner="FGV"} },
			{1214, new Titulo {Code=1214,Name="Reservado FGV",Owner="FGV"} },
			{1215, new Titulo {Code=1215,Name="Reservado FGV",Owner="FGV"} },
			{1216, new Titulo {Code=1216,Name="Reservado FGV",Owner="FGV"} },
			{1217, new Titulo {Code=1217,Name="FUNDAR",Owner="FGV"} },
			{1218, new Titulo {Code=1218,Name="Reservado FGV",Owner="FGV"} },
			{1219, new Titulo {Code=1219,Name="Sencillo FN 50%",Owner="FGV"} },
			{1220, new Titulo {Code=1220,Name="Ida y Vuelta FN 50%",Owner="FGV"} },
			{1221, new Titulo {Code=1221,Name="Bonometro FN 50%",Owner="FGV"} },
			{1222, new Titulo {Code=1222,Name="TAT FN 50%",Owner="FGV"} },
			{1223, new Titulo {Code=1223,Name="TAT Anual FN 50%",Owner="FGV"} },
			{1224, new Titulo {Code=1224,Name="Bono 60x60 FN 50%",Owner="FGV"} },
			{1225, new Titulo {Code=1225,Name="AENA/Aeroport",Owner="FGV"} },
			{1226, new Titulo {Code=1226,Name="TAT Tr. Aeroport (Mensual tarifa especial trabajadores empresas Aeroport-no AENA)",Owner="FGV"} },
			{1227, new Titulo {Code=1227,Name="Bono AENA (Bonometro trabajadores AENA)",Owner="FGV"} },
			{1228, new Titulo {Code=1228,Name="Bono Tr. Aeroport (Bonometro tarifa especial trabajadores empresas Aeroport-no AENA)",Owner="FGV"} },
			{1229, new Titulo {Code=1229,Name="Reservado FGV",Owner="FGV"} },
			{1230, new Titulo {Code=1230,Name="Reservado FGV",Owner="FGV"} },
			{1231, new Titulo {Code=1231,Name="Reservado FGV",Owner="FGV"} },
			{1232, new Titulo {Code=1232,Name="Reservado FGV",Owner="FGV"} },
			{1233, new Titulo {Code=1233,Name="Reservado FGV",Owner="FGV"} },
			{1234, new Titulo {Code=1234,Name="Reservado FGV",Owner="FGV"} },
			{1235, new Titulo {Code=1235,Name="Reservado FGV",Owner="FGV"} },
			{1236, new Titulo {Code=1236,Name="Reservado FGV",Owner="FGV"} },
			{1237, new Titulo {Code=1237,Name="Reservado FGV",Owner="FGV"} },
			{1238, new Titulo {Code=1238,Name="Reservado FGV",Owner="FGV"} },
			{1239, new Titulo {Code=1239,Name="Estudiantess Ayto. Mislata",Owner="FGV"} },
			{1240, new Titulo {Code=1240,Name="Estudiantes Ayto. Museos (Temporal Ayto Mislata)",Owner="FGV"} },
			{1241, new Titulo {Code=1241,Name="Reservado FGV",Owner="FGV"} },
			{1242, new Titulo {Code=1242,Name="Reservado FGV",Owner="FGV"} },
			{1243, new Titulo {Code=1243,Name="Reservado FGV",Owner="FGV"} },
			{1244, new Titulo {Code=1244,Name="Reservado FGV",Owner="FGV"} },
			{1245, new Titulo {Code=1245,Name="Reservado FGV",Owner="FGV"} },
			{1246, new Titulo {Code=1246,Name="Reservado FGV",Owner="FGV"} },
			{1247, new Titulo {Code=1247,Name="Reservado FGV",Owner="FGV"} },
			{1248, new Titulo {Code=1248,Name="Reservado FGV",Owner="FGV"} },
			{1249, new Titulo {Code=1249,Name="Reservado FGV",Owner="FGV"} },
			{1250, new Titulo {Code=1250,Name="Reservado FGV",Owner="FGV"} },
			{1251, new Titulo {Code=1251,Name="Reservado FGV",Owner="FGV"} },
			{1252, new Titulo {Code=1252,Name="Reservado FGV",Owner="FGV"} },
			{1253, new Titulo {Code=1253,Name="Reservado FGV",Owner="FGV"} },
			{1254, new Titulo {Code=1254,Name="Reservado FGV",Owner="FGV"} },
			{1255, new Titulo {Code=1255,Name="Reservado FGV",Owner="FGV"} },
			{1256, new Titulo {Code=1256,Name="Reservado FGV",Owner="FGV"} },
			{1257, new Titulo {Code=1257,Name="Reservado FGV",Owner="FGV"} },
			{1258, new Titulo {Code=1258,Name="Reservado FGV",Owner="FGV"} },
			{1259, new Titulo {Code=1259,Name="Reservado FGV",Owner="FGV"} },
			{1260, new Titulo {Code=1260,Name="Reservado FGV",Owner="FGV"} },
			{1261, new Titulo {Code=1261,Name="Reservado FGV",Owner="FGV"} },
			{1262, new Titulo {Code=1262,Name="Reservado FGV",Owner="FGV"} },
			{1263, new Titulo {Code=1263,Name="Reservado FGV",Owner="FGV"} },
			{1264, new Titulo {Code=1264,Name="Reservado FGV",Owner="FGV"} },
			{1265, new Titulo {Code=1265,Name="Reservado FGV",Owner="FGV"} },
			{1266, new Titulo {Code=1266,Name="Reservado FGV",Owner="FGV"} },
			{1267, new Titulo {Code=1267,Name="Reservado FGV",Owner="FGV"} },
			{1268, new Titulo {Code=1268,Name="Reservado FGV",Owner="FGV"} },
			{1269, new Titulo {Code=1269,Name="Reservado FGV",Owner="FGV"} },
			{1270, new Titulo {Code=1270,Name="Reservado FGV",Owner="FGV"} },
			//{1271, new Titulo {Code=1271,Name="Reservado FGV",Owner="FGV"} },
			//{1272, new Titulo {Code=1272,Name="Reservado FGV",Owner="FGV"} },
			//{1273, new Titulo {Code=1273,Name="Reservado FGV",Owner="FGV"} },
			//{1274, new Titulo {Code=1274,Name="Reservado FGV",Owner="FGV"} },
			//{1275, new Titulo {Code=1275,Name="Reservado FGV",Owner="FGV"} },
			//{1276, new Titulo {Code=1276,Name="Reservado FGV",Owner="FGV"} },
			//{1277, new Titulo {Code=1277,Name="Reservado FGV",Owner="FGV"} },
			{1271, new Titulo {Code=1271,Name="TuiN",Owner="FGV"} }, // Modificado para TuiN
			{1272, new Titulo {Code=1272,Name="TuiN 20% FN",Owner="FGV"} }, // Modificado para TuiN
			{1273, new Titulo {Code=1273,Name="TuiN 50% FN",Owner="FGV"} }, // Modificado para TuiN
			{1274, new Titulo {Code=1274,Name="TuiN Mensual",Owner="FGV"} }, // Modificado para TuiN
			{1275, new Titulo {Code=1275,Name="TuiN 20% MonoParental",Owner="FGV"} }, // Modificado para TuiN
			{1276, new Titulo {Code=1276,Name="TuiN 50% MonoParental",Owner="FGV"} }, // Modificado para TuiN
			{1277, new Titulo {Code=1277,Name="TuiN Universitaria",Owner="FGV"} }, // Modificado para TuiN
			{1278, new Titulo {Code=1278,Name="Reservado FGV",Owner="FGV"} },
			{1279, new Titulo {Code=1279,Name="Reservado FGV",Owner="FGV"} },
			{1280, new Titulo {Code=1280,Name="Reservado FGV",Owner="FGV"} },
			{1281, new Titulo {Code=1281,Name="Reservado FGV",Owner="FGV"} },
			{1282, new Titulo {Code=1282,Name="Reservado FGV",Owner="FGV"} },
			{1283, new Titulo {Code=1283,Name="Reservado FGV",Owner="FGV"} },
			{1284, new Titulo {Code=1284,Name="Reservado FGV",Owner="FGV"} },
			{1285, new Titulo {Code=1285,Name="Reservado FGV",Owner="FGV"} },
			{1286, new Titulo {Code=1286,Name="Reservado FGV",Owner="FGV"} },
			{1287, new Titulo {Code=1287,Name="Reservado FGV",Owner="FGV"} },
			{1288, new Titulo {Code=1288,Name="Reservado FGV",Owner="FGV"} },
			{1289, new Titulo {Code=1289,Name="Reservado FGV",Owner="FGV"} },
			{1290, new Titulo {Code=1290,Name="Reservado FGV",Owner="FGV"} },
			{1291, new Titulo {Code=1291,Name="Reservado FGV",Owner="FGV"} },
			{1292, new Titulo {Code=1292,Name="Reservado FGV",Owner="FGV"} },
			{1293, new Titulo {Code=1293,Name="Reservado FGV",Owner="FGV"} },
			{1294, new Titulo {Code=1294,Name="Reservado FGV",Owner="FGV"} },
			{1295, new Titulo {Code=1295,Name="Reservado FGV",Owner="FGV"} },
			{1296, new Titulo {Code=1296,Name="Reservado FGV",Owner="FGV"} },
			{1297, new Titulo {Code=1297,Name="Reservado FGV",Owner="FGV"} },
			{1298, new Titulo {Code=1298,Name="Reservado FGV",Owner="FGV"} },
			{1299, new Titulo {Code=1299,Name="Reservado FGV",Owner="FGV"} },
			{1300, new Titulo {Code=1300,Name="Reservado FGV",Owner="FGV"} },
			//{1552, new Titulo {Code=1552,Name="Bono Transbordo (10 viajes)",Owner="Coordinación aVM"} },
			{1552, new Titulo {Code=1552,Name="Bono Transbordo",Owner="Coordinación aVM"} },
			{1568, new Titulo {Code=1568,Name="Abono Transporte",Owner="Coordinación aVM"} },
			{1648, new Titulo {Code=1648,Name="T-2",Owner="Coordinación aVM"} },
			{1808, new Titulo {Code=1808,Name="Bono metro",Owner="FGV"} }, // No estaba
			//{1824, new Titulo {Code=1824,Name="Abono Transporte Jove",Owner="Coordinación aVM"} },
			{1824, new Titulo {Code=1824,Name="Abono Trans Jove",Owner="Coordinación aVM"} },
			{1904, new Titulo {Code=1904,Name="T-3",Owner="Coordinación aVM"} },
			{2000, new Titulo {Code=2000,Name="Reserva para título monedero",Owner="Ayto. TEULADA"} },
			{2001, new Titulo {Code=2001,Name="Reservado Ayto. Teulada",Owner="Ayto. Teulada"} },
			{2002, new Titulo {Code=2002,Name="Reservado Ayto. Teulada",Owner="Ayto. Teulada"} },
			{2003, new Titulo {Code=2003,Name="Reservado Ayto. Teulada",Owner="Ayto. Teulada"} },
			{2004, new Titulo {Code=2004,Name="Reservado Ayto. Teulada",Owner="Ayto. Teulada"} },
			{2005, new Titulo {Code=2005,Name="Reservado Ayto. Teulada",Owner="Ayto. Teulada"} },
			{2006, new Titulo {Code=2006,Name="Reservado Ayto. Teulada",Owner="Ayto. Teulada"} },
			{2007, new Titulo {Code=2007,Name="Reservado Ayto. Teulada",Owner="Ayto. Teulada"} },
			{2008, new Titulo {Code=2008,Name="Reservado Ayto. Teulada",Owner="Ayto. Teulada"} },
			{2009, new Titulo {Code=2009,Name="Reservado Ayto. Teulada",Owner="Ayto. Teulada"} },
			{2010, new Titulo {Code=2010,Name="Reservado Ayto. Teulada",Owner="Ayto. Teulada"} },
			{2011, new Titulo {Code=2011,Name="Título: Bono Temporal 30 dias",Owner="Llorente Bus"} },
			{2012, new Titulo {Code=2012,Name="TBus",Owner="Llorente Bus"} },
			{2013, new Titulo {Code=2013,Name="Bono Dis Altea",Owner="Llorente Bus"} },
			{2014, new Titulo {Code=2014,Name="Bono Oro Villajoyosa",Owner="Llorente Bus"} },
			{2015, new Titulo {Code=2015,Name="Bono Oro L'Alfas",Owner="Llorente Bus"} },
			{2016, new Titulo {Code=2016,Name="Bono Oro Benidorm",Owner="Llorente Bus"} },
			{2017, new Titulo {Code=2017,Name="Bono Oro Finestrat",Owner="Llorente Bus"} },
			{2018, new Titulo {Code=2018,Name="Bono Oro Altea",Owner="Llorente Bus"} },
			{2019, new Titulo {Code=2019,Name="Bono Oro General",Owner="Llorente Bus"} },
			{2020, new Titulo {Code=2020,Name="Reservado Llorente Bus",Owner="Llorente Bus"} },
			{2021, new Titulo {Code=2021,Name="Reservado Llorente Bus",Owner="Llorente Bus"} },
			{2022, new Titulo {Code=2022,Name="Reservado Llorente Bus",Owner="Llorente Bus"} },
			{2023, new Titulo {Code=2023,Name="Reservado Llorente Bus",Owner="Llorente Bus"} },
			{2024, new Titulo {Code=2024,Name="Bono Joven/ escolar Altea",Owner="Llorente Bus"} },
			{2025, new Titulo {Code=2025,Name="Bono Joven/escolar L’Alfàs",Owner="Llorente Bus"} },
			{2026, new Titulo {Code=2026,Name="Bono Escolar Benidorm",Owner="Llorente Bus"} },
			{2027, new Titulo {Code=2027,Name="Bono<14 Villajoyosa",Owner="Llorente Bus"} },
			{2028, new Titulo {Code=2028,Name="Pase Escuelas Deportivas Villajoyosa",Owner="Llorente Bus"} },
			{2029, new Titulo {Code=2029,Name="Bono Escolar Finestrat",Owner="Llorente Bus"} },
			{2030, new Titulo {Code=2030,Name="Pase Escolar C.Educación",Owner="Llorente Bus"} },
			{2031, new Titulo {Code=2031,Name="Bono Joven General",Owner="Llorente Bus"} },
			{2032, new Titulo {Code=2032,Name="Pass 1day",Owner="Llorente Bus"} },
			{2033, new Titulo {Code=2033,Name="Pass 4days",Owner="Llorente Bus"} },
			{2034, new Titulo {Code=2034,Name="Pass 7days",Owner="Llorente Bus"} },
			{2035, new Titulo {Code=2035,Name="Pass 2 days",Owner="Llorente Bus"} },
			{2036, new Titulo {Code=2036,Name="Pase empleado",Owner="Llorente Bus"} },
			{2037, new Titulo {Code=2037,Name="Pase favor empresa",Owner="Llorente Bus"} },
			{2038, new Titulo {Code=2038,Name="Familia Numerosa General",Owner="Llorente Bus"} },
			{2039, new Titulo {Code=2039,Name="Familia numerosa especial",Owner="Llorente Bus"} },
			{2040, new Titulo {Code=2040,Name="Familia numerosa general Villajoyosa",Owner="Llorente Bus"} },
			{2041, new Titulo {Code=2041,Name="Familia NumerosaEspecial Villajoyosa",Owner="Llorente Bus"} },
			{2042, new Titulo {Code=2042,Name="Reservado Llorente Bus",Owner="Llorente Bus"} },
			{2043, new Titulo {Code=2043,Name="Reservado Llorente Bus",Owner="Llorente Bus"} },
			{2044, new Titulo {Code=2044,Name="Reservado Llorente Bus",Owner="Llorente Bus"} },
			{2045, new Titulo {Code=2045,Name="Reservado Llorente Bus",Owner="Llorente Bus"} },
			{2046, new Titulo {Code=2046,Name="Reservado Llorente Bus",Owner="Llorente Bus"} },
			{2047, new Titulo {Code=2047,Name="Reservado Llorente Bus",Owner="Llorente Bus"} },
			{2048, new Titulo {Code=2048,Name="Reservado Llorente Bus",Owner="Llorente Bus"} },
			{2049, new Titulo {Code=2049,Name="Reservado Llorente Bus",Owner="Llorente Bus"} },
			{2050, new Titulo {Code=2050,Name="Reservado Llorente Bus",Owner="Llorente Bus"} },
			{2080, new Titulo {Code=2080,Name="TAT",Owner="FGV"} }, // No estaba
			{2081, new Titulo {Code=2081,Name="Bono 10 Tarifa Estudiante",Owner="Ayto. Buñol"} },
			{2082, new Titulo {Code=2082,Name="Bono 10 Tarifa Jubilado",Owner="Ayto. Buñol"} },
			{2083, new Titulo {Code=2083,Name="Bono 10 Tarifa Normal",Owner="Ayto. Buñol"} },
			{2084, new Titulo {Code=2084,Name="Reservado Ayto. Buñol",Owner="Ayto. Buñol"} },
			{2085, new Titulo {Code=2085,Name="Reservado Ayto. Buñol",Owner="Ayto. Buñol"} },
			{2086, new Titulo {Code=2086,Name="Reservado Ayto. Buñol",Owner="Ayto. Buñol"} },
			{2087, new Titulo {Code=2087,Name="Reservado Ayto. Buñol",Owner="Ayto. Buñol"} },
			{2088, new Titulo {Code=2088,Name="Reservado Ayto. Buñol",Owner="Ayto. Buñol"} },
			{2089, new Titulo {Code=2089,Name="Reservado Ayto. Buñol",Owner="Ayto. Buñol"} },
			{2090, new Titulo {Code=2090,Name="Reservado Ayto. Buñol",Owner="Ayto. Buñol"} },
			{2091, new Titulo {Code=2091,Name="Reservado Mancomunitat L’Horta Sud",Owner="Mancomunitat L’Horta Sud"} },
			{2092, new Titulo {Code=2092,Name="Reservado Mancomunitat L’Horta Sud",Owner="Mancomunitat L’Horta Sud"} },
			{2093, new Titulo {Code=2093,Name="Reservado Mancomunitat L’Horta Sud",Owner="Mancomunitat L’Horta Sud"} },
			{2094, new Titulo {Code=2094,Name="Reservado Mancomunitat L’Horta Sud",Owner="Mancomunitat L’Horta Sud"} },
			{2095, new Titulo {Code=2095,Name="Reservado Mancomunitat L’Horta Sud",Owner="Mancomunitat L’Horta Sud"} },
			{2096, new Titulo {Code=2096,Name="Reservado Mancomunitat L’Horta Sud",Owner="Mancomunitat L’Horta Sud"} },
			{2097, new Titulo {Code=2097,Name="Reservado Mancomunitat L’Horta Sud",Owner="Mancomunitat L’Horta Sud"} },
			{2098, new Titulo {Code=2098,Name="Reservado Mancomunitat L’Horta Sud",Owner="Mancomunitat L’Horta Sud"} },
			{2099, new Titulo {Code=2099,Name="Reservado Mancomunitat L’Horta Sud",Owner="Mancomunitat L’Horta Sud"} },
			{2100, new Titulo {Code=2100,Name="Reservado Mancomunitat L’Horta Sud",Owner="Mancomunitat L’Horta Sud"} },
			{2101, new Titulo {Code=2101,Name="Título Monedero",Owner="Los Serranos"} },
			{2102, new Titulo {Code=2102,Name="Reservado Los Serranos",Owner="Los Serranos"} },
			{2103, new Titulo {Code=2103,Name="Reservado Los Serranos",Owner="Los Serranos"} },
			{2104, new Titulo {Code=2104,Name="Reservado Los Serranos",Owner="Los Serranos"} },
			{2105, new Titulo {Code=2105,Name="Reservado Los Serranos",Owner="Los Serranos"} },
			{2106, new Titulo {Code=2106,Name="Reservado Los Serranos",Owner="Los Serranos"} },
			{2107, new Titulo {Code=2107,Name="Reservado Los Serranos",Owner="Los Serranos"} },
			{2108, new Titulo {Code=2108,Name="Reservado Los Serranos",Owner="Los Serranos"} },
			{2109, new Titulo {Code=2109,Name="Reservado Los Serranos",Owner="Los Serranos"} },
			{2110, new Titulo {Code=2110,Name="Reservado Los Serranos",Owner="Los Serranos"} },
			{2111, new Titulo {Code=2111,Name="Bono 10 METRORBITAL",Owner="METRORBITAL"} },
			{2112, new Titulo {Code=2112,Name="Reservado METRORBITAL",Owner="METRORBITAL"} },
			{2113, new Titulo {Code=2113,Name="Reservado METRORBITAL",Owner="METRORBITAL"} },
			{2114, new Titulo {Code=2114,Name="Reservado METRORBITAL",Owner="METRORBITAL"} },
			{2115, new Titulo {Code=2115,Name="Reservado METRORBITAL",Owner="METRORBITAL"} },
			{2116, new Titulo {Code=2116,Name="Reservado METRORBITAL",Owner="METRORBITAL"} },
			{2117, new Titulo {Code=2117,Name="Reservado METRORBITAL",Owner="METRORBITAL"} },
			{2118, new Titulo {Code=2118,Name="Reservado METRORBITAL",Owner="METRORBITAL"} },
			{2119, new Titulo {Code=2119,Name="Reservado METRORBITAL",Owner="METRORBITAL"} },
			{2120, new Titulo {Code=2120,Name="Reservado METRORBITAL",Owner="METRORBITAL"} },
			{2121, new Titulo {Code=2121,Name="Título Monedero",Owner="Valle de Cárcer"} },
			{2122, new Titulo {Code=2122,Name="Reservado Valle de Carcer",Owner="Valle de Cárcer"} },
			{2123, new Titulo {Code=2123,Name="Reservado Valle de Carcer",Owner="Valle de Cárcer"} },
			{2124, new Titulo {Code=2124,Name="Reservado Valle de Carcer",Owner="Valle de Cárcer"} },
			{2125, new Titulo {Code=2125,Name="Reservado Valle de Carcer",Owner="Valle de Cárcer"} },
			{2126, new Titulo {Code=2126,Name="Reservado Valle de Carcer",Owner="Valle de Cárcer"} },
			{2127, new Titulo {Code=2127,Name="Reservado Valle de Carcer",Owner="Valle de Cárcer"} },
			{2128, new Titulo {Code=2128,Name="Reservado Valle de Carcer",Owner="Valle de Cárcer"} },
			{2129, new Titulo {Code=2129,Name="Reservado Valle de Carcer",Owner="Valle de Cárcer"} },
			{2130, new Titulo {Code=2130,Name="Reservado Valle de Carcer",Owner="Valle de Cárcer"} },
			{2131, new Titulo {Code=2131,Name="Godella Residente Semestral",Owner="Godella"} },
			{2132, new Titulo {Code=2132,Name="Godella Residente Anual",Owner="Godella"} },
			{2133, new Titulo {Code=2133,Name="Godella Residente Semestral FN (familia numerosa)",Owner="Godella"} },
			{2134, new Titulo {Code=2134,Name="Godella Residente Anual FN",Owner="Godella"} },
			{2135, new Titulo {Code=2135,Name="Godella No Residente Semestral",Owner="Godella"} },
			{2136, new Titulo {Code=2136,Name="Godella No Residente Anual",Owner="Godella"} },
			{2137, new Titulo {Code=2137,Name="Godella No Residente Semestral FN",Owner="Godella"} },
			{2138, new Titulo {Code=2138,Name="Godella No Residente Anual FN",Owner="Godella"} },
			{2139, new Titulo {Code=2139,Name="Godella Parados",Owner="Godella"} },
			{2140, new Titulo {Code=2140,Name="Godella Jubilados Rentas Bajas",Owner="Godella"} },
			{2141, new Titulo {Code=2141,Name="Reservado Godella",Owner="Godella"} },
			{2142, new Titulo {Code=2142,Name="Reservado Godella",Owner="Godella"} },
			{2143, new Titulo {Code=2143,Name="Reservado Godella",Owner="Godella"} },
			{2144, new Titulo {Code=2144,Name="Reservado Godella",Owner="Godella"} },
			{2145, new Titulo {Code=2145,Name="Reservado Godella",Owner="Godella"} },
			{2146, new Titulo {Code=2146,Name="Reservado Godella",Owner="Godella"} },
			{2147, new Titulo {Code=2147,Name="Reservado Godella",Owner="Godella"} },
			{2148, new Titulo {Code=2148,Name="Reservado Godella",Owner="Godella"} },
			{2149, new Titulo {Code=2149,Name="Reservado Godella",Owner="Godella"} },
			{2150, new Titulo {Code=2150,Name="Reservado Godella",Owner="Godella"} },
			{2336, new Titulo {Code=2336,Name="TAT Jove",Owner="FGV"} },  // NO estaba
			{2501, new Titulo {Code=2501,Name="Sencillo",Owner="RENFE"} },
			{2502, new Titulo {Code=2502,Name="Ida y vuelta",Owner="RENFE"} },
			{2503, new Titulo {Code=2503,Name="Bonotren",Owner="RENFE"} },
			{2504, new Titulo {Code=2504,Name="Mensual limitado",Owner="RENFE"} },
			{2505, new Titulo {Code=2505,Name="Mensual ilimitado",Owner="RENFE"} },
			{2506, new Titulo {Code=2506,Name="Reservado RENFE",Owner="RENFE"} },
			{2507, new Titulo {Code=2507,Name="Reservado RENFE",Owner="RENFE"} },
			{2508, new Titulo {Code=2508,Name="Reservado RENFE",Owner="RENFE"} },
			{2509, new Titulo {Code=2509,Name="Reservado RENFE",Owner="RENFE"} },
			{2510, new Titulo {Code=2510,Name="Reservado RENFE",Owner="RENFE"} },
			{2592, new Titulo {Code=2592,Name="Escolares",Owner="FGV"} }, // No estaba
			{2848, new Titulo {Code=2848,Name="Get Major",Owner="FGV"} }, // No estaba
			{3001, new Titulo {Code=3001,Name="Bono 10 EIGE",Owner="FGV"} }, // Rev 074
			{3002, new Titulo {Code=3002,Name="Bono 30 días EIGE",Owner="FGV"} }, // Rev 074
			{3003, new Titulo {Code=3003,Name="Bono 30 días Jove EIGE",Owner="FGV"} }, // Rev 074
			{3120, new Titulo {Code=3120,Name="Pase Empleado",Owner="aVM"} },
			//{3376, new Titulo {Code=3376,Name="Agentes de FGV en activo",Owner="FGV"} },
			{3376, new Titulo {Code=3376,Name="Pase empleado FGV",Owner="FGV"} },
			{3632, new Titulo {Code=3632,Name="Pase Empleado",Owner="EMT"} },
			{4000, new Titulo {Code=4000,Name="Bono trasbordo (otras zonas)",Owner="aVM"} },
			{4001, new Titulo {Code=4001,Name="(Vinalesa y P. Tecnológico)",Owner="aVM"} },
			//{4002, new Titulo {Code=4002,Name="Bono 10 Viajes - S. Transversal Horta Nord",Owner="aVM"} },
			{4002, new Titulo {Code=4002,Name="Bono 10 S. TranHN",Owner="aVM"} },
			//{4003, new Titulo {Code=4003,Name="Bono 10 Viajes - València - Burjassot- P. Tecnològic B",Owner="aVM"} },
			{4003, new Titulo {Code=4003,Name="Bono 10 Bur - Par B",Owner="aVM"} },
			//{4004, new Titulo {Code=4004,Name="Bono Transbordo - Bono 10 Viajes - València - Burjassot- P. Tecnològic AB",Owner="aVM"} },
			{4004, new Titulo {Code=4004,Name="Bono 1o Bur - Par AB",Owner="aVM"} },
			//{4005, new Titulo {Code=4005,Name="A>65 Corredor Valencia – Perellonet",Owner="aVM"} },
			{4005, new Titulo {Code=4005,Name="A>65 VLC - Perello",Owner="aVM"} },
			//{4006, new Titulo {Code=4006,Name="Bono 10 Viajes Riba-Roja - Manises – Valencia",Owner="aVM"} },
			{4006, new Titulo {Code=4006,Name="Bono 10 Ri - Ma - VaA",Owner="aVM"} },
			// {4007, new Titulo {Code=4007,Name="Bono 10 Viajes Servicio Alcàsser – Silla",Owner="aVM"} },
			{4007, new Titulo {Code=4007,Name="Bono 10 Alc – Silla",Owner="aVM"} },
			{4008, new Titulo {Code=4008,Name="Sencillo Jubilado Metrobus",Owner="Metrobus"} },
			{4009, new Titulo {Code=4009,Name="Otros Sencillos Metrobus",Owner="Metrobus"} },
			{4010, new Titulo {Code=4010,Name="Bono 10 Viajes Metrobus Nord",Owner="aVM"} },
			{4011, new Titulo {Code=4011,Name="Bono 10 viajes Marxant-bus",Owner="aVM"} },
			{4012, new Titulo {Code=4012,Name="Bono Transbordo MISLATA",Owner="AVM"} },
			{4013, new Titulo {Code=4013,Name="Bono Transbordo ALBORAYA",Owner="AVM"} },
			{4014, new Titulo {Code=4014,Name="Bono 10 viajes Estudios superiores Marxant-bus",Owner="aVM"} },
			{4015, new Titulo {Code=4015,Name="Reservado Título aVM",Owner="aVM"} },
			{4016, new Titulo {Code=4016,Name="Reservado Título aVM",Owner="aVM"} },
			{4017, new Titulo {Code=4017,Name="Reservado Título aVM",Owner="aVM"} },
			{4018, new Titulo {Code=4018,Name="Reservado Título aVM",Owner="aVM"} },
			{4019, new Titulo {Code=4019,Name="Reservado Título aVM",Owner="aVM"} },
			{4020, new Titulo {Code=4020,Name="Reservado Título aVM",Owner="aVM"} },
			{4021, new Titulo {Code=4021,Name="Reservado Título aVM",Owner="aVM"} },
			{4022, new Titulo {Code=4022,Name="Reservado Título aVM",Owner="aVM"} },
			{4023, new Titulo {Code=4023,Name="Reservado Título aVM",Owner="aVM"} },
			{4024, new Titulo {Code=4024,Name="Reservado Título aVM",Owner="aVM"} },
			{4025, new Titulo {Code=4025,Name="Reservado Título aVM",Owner="aVM"} },
			{4026, new Titulo {Code=4026,Name="Reservado Título aVM",Owner="aVM"} },
			{4027, new Titulo {Code=4027,Name="Reservado Título aVM",Owner="aVM"} },
			{4028, new Titulo {Code=4028,Name="Reservado Título aVM",Owner="aVM"} },
			{4029, new Titulo {Code=4029,Name="Reservado Título aVM",Owner="aVM"} },
			{4030, new Titulo {Code=4030,Name="Reservado Título aVM",Owner="aVM"} },
			{4031, new Titulo {Code=4031,Name="Reservado Título aVM",Owner="aVM"} },
			{4032, new Titulo {Code=4032,Name="Reservado Título aVM",Owner="aVM"} },
			{4033, new Titulo {Code=4033,Name="Reservado Título aVM",Owner="aVM"} },
			{4034, new Titulo {Code=4034,Name="Reservado Título aVM",Owner="aVM"} },
			{4035, new Titulo {Code=4035,Name="Reservado Título aVM",Owner="aVM"} },
			{4036, new Titulo {Code=4036,Name="Reservado Título aVM",Owner="aVM"} },
			{4037, new Titulo {Code=4037,Name="Reservado Título aVM",Owner="aVM"} },
			{4038, new Titulo {Code=4038,Name="Reservado Título aVM",Owner="aVM"} },
			{4039, new Titulo {Code=4039,Name="Reservado Título aVM",Owner="aVM"} },
			{4040, new Titulo {Code=4040,Name="Reservado Título aVM",Owner="aVM"} },
			{4041, new Titulo {Code=4041,Name="Reservado Título aVM",Owner="aVM"} },
			{4042, new Titulo {Code=4042,Name="Reservado Título aVM",Owner="aVM"} },
			{4043, new Titulo {Code=4043,Name="Reservado Título aVM",Owner="aVM"} },
			{4044, new Titulo {Code=4044,Name="Reservado Título aVM",Owner="aVM"} },
			{4045, new Titulo {Code=4045,Name="Reservado Título aVM",Owner="aVM"} },
			{4046, new Titulo {Code=4046,Name="Reservado Título aVM",Owner="aVM"} },
			{4047, new Titulo {Code=4047,Name="Reservado Título aVM",Owner="aVM"} },
			{4048, new Titulo {Code=4048,Name="Reservado Título aVM",Owner="aVM"} },
			{4049, new Titulo {Code=4049,Name="Reservado Título aVM",Owner="aVM"} },
			{4050, new Titulo {Code=4050,Name="Reservado Título aVM",Owner="aVM"} },
			{4051, new Titulo {Code=4051,Name="Título Torrent Daurada",Owner="Torrent"} },
			{4052, new Titulo {Code=4052,Name="Título Torrent Estudiant",Owner="Torrent"} },
			{4053, new Titulo {Code=4053,Name="Abonamente 10",Owner="Torrent"} },
			{4054, new Titulo {Code=4054,Name="Reservado Ayto. Torrent",Owner="Torrent"} },
			{4055, new Titulo {Code=4055,Name="Reservado Ayto. Torrent",Owner="Torrent"} },
			{4056, new Titulo {Code=4056,Name="Reservado Ayto. Torrent",Owner="Torrent"} },
			{4057, new Titulo {Code=4057,Name="Reservado Ayto. Torrent",Owner="Torrent"} },
			{4058, new Titulo {Code=4058,Name="Reservado Ayto. Torrent",Owner="Torrent"} },
			{4059, new Titulo {Code=4059,Name="Reservado Ayto. Torrent",Owner="Torrent"} },
			{4060, new Titulo {Code=4060,Name="Reservado Ayto. Torrent",Owner="Torrent"} },
			{4061, new Titulo {Code=4061,Name="Bono 10 Tamarit- Santa Pola- Alicante",Owner="Autocares Baile"} },
			{4062, new Titulo {Code=4062,Name="Bono 10 Tamarit- Santa Pola",Owner="Autocares Baile"} },
			{4063, new Titulo {Code=4063,Name="Bono 10 Gran Alacant- Arenales-Alicante",Owner="Autocares Baile"} },
			{4064, new Titulo {Code=4064,Name="Bono 10 Santa Pola-Gran Alacant-Arenales-El Altet",Owner="Autocares Baile"} },
			{4065, new Titulo {Code=4065,Name="Bono 10 Alicante-El Altet- Playa del Altet",Owner="Autocares Baile"} },
			{4066, new Titulo {Code=4066,Name="Bono 10 San Vicente- Arenales del Sol",Owner="Autocares Baile"} },
			{4067, new Titulo {Code=4067,Name="Bono 10 San Vicente- Santa Pola",Owner="Autocares Baile"} },
			{4068, new Titulo {Code=4068,Name="Bono 10 Gran Alacant- Universidad de Alicante",Owner="Autocares Baile"} },
			{4069, new Titulo {Code=4069,Name="Bono 10 Agua Amarga- Alicante",Owner="Autocares Baile"} },
			{4070, new Titulo {Code=4070,Name="Bono 20 Viajes Agua- Alicante",Owner="Autocares Baile"} },
			{4071, new Titulo {Code=4071,Name="Bono 10 Gran Alacant- Gran Alacant",Owner="Autocares Baile"} },
			{4072, new Titulo {Code=4072,Name="Bono Santa Pola- Universidad Alicante",Owner="Autocares Baile"} },
			{4073, new Titulo {Code=4073,Name="Bono Gran Alacant- Universidad",Owner="Autocares Baile"} },
			{4074, new Titulo {Code=4074,Name="Bono Arenales del Sol- Universidad",Owner="Autocares Baile"} },
			{4075, new Titulo {Code=4075,Name="Bono El Altel-Universidad",Owner="Autocares Baile"} },
			{4076, new Titulo {Code=4076,Name="Reservado Autocares Baile",Owner="Autocares Baile"} },
			{4077, new Titulo {Code=4077,Name="Reservado Autocares Baile",Owner="Autocares Baile"} },
			{4078, new Titulo {Code=4078,Name="Reservado Autocares Baile",Owner="Autocares Baile"} },
			{4079, new Titulo {Code=4079,Name="Reservado Autocares Baile",Owner="Autocares Baile"} },
			{4080, new Titulo {Code=4080,Name="Reservado Autocares Baile",Owner="Autocares Baile"} },
			//{4080, new Titulo {Code=4080,Name="Reservado Ayto. Aldaia",Owner="Ayto. Aldaia"} },
			{4081, new Titulo {Code=4081,Name="Reservado Ayto. Aldaia",Owner="Ayto. Aldaia"} },
			{4082, new Titulo {Code=4082,Name="Reservado Ayto. Aldaia",Owner="Ayto. Aldaia"} },
			{4083, new Titulo {Code=4083,Name="Reservado Ayto. Aldaia",Owner="Ayto. Aldaia"} },
			{4084, new Titulo {Code=4084,Name="Reservado Ayto. Aldaia",Owner="Ayto. Aldaia"} },
			{4085, new Titulo {Code=4085,Name="Reservado Ayto. Aldaia",Owner="Ayto. Aldaia"} },
			{4086, new Titulo {Code=4086,Name="Reservado Ayto. Aldaia",Owner="Ayto. Aldaia"} },
			{4087, new Titulo {Code=4087,Name="Reservado Ayto. Aldaia",Owner="Ayto. Aldaia"} },
			{4088, new Titulo {Code=4088,Name="Reservado Ayto. Aldaia",Owner="Ayto. Aldaia"} },
			{4089, new Titulo {Code=4089,Name="Reservado Ayto. Aldaia",Owner="Ayto. Aldaia"} },
			{4090, new Titulo {Code=4090,Name="Reservado Ayto. Aldaia",Owner="Ayto. Aldaia"} },
			{4094, new Titulo {Code=4094,Name="Tarjeta vinculada",Owner="GV"} },
		};
		#endregion Titulos

		// Revisar
		#region GetValidationPlaceAsync
		private async Task<string> GetValidationPlaceAsync(EigeTipoValidacion_TransporteEnum transporte, int estacion, int vestibulo, int linea, int convoy)
		{
			return await Task.Run(() =>
				transporte == EigeTipoValidacion_TransporteEnum.Ferrocarril ?
					GetFerrocarrilEstationName(estacion, vestibulo) :
					GetBusEstationName(linea, convoy)
			);
		}
		#endregion GetValidationPlaceAsync

		// Usar BD
		#region GetFerrocarrilEstationName
		Dictionary<int, string> ferrocarrilEstacion = new Dictionary<int, string>
		{
			{001,"V. de Castellón"},
			{002,"Alberic"},
			{003,"Massalavés"},
			{004,"Montortal"},
			{005,"L'Alcúdia"},
			{006,"Benimodo"},
			{007,"Carlet"},
			{008,"Ausias March"},
			{009,"Alginet"},
			{010,"F. Almaguer"},
			{011,"Espioca"},
			{012,"Omet"},
			{013,"Picassent"},
			{014,"Sant Ramon"},
			{015,"Realon"},
			{016,"Col.legi Vedat"},
			{017,"Torrent"},
			{018,"Picanya"},
			{019,"Paiporta"},
			{020,"Valencia S."},
			{021,"Safranar"},
			{022,"Patraix"},
			{023,"Joaquín Sorolla-Jesús"},
			{024,"Pl. Espanya"},
			{025,"A. Guimerà"},
			{026,"Túria"},
			{027,"Campanar"},
			{028,"Beniferri"},
			{029,"Empalme L1"},
			{030,"Burjassot"},
			{031,"Burja.-Godella"},
			{032,"Godella"},
			{033,"Rocafort"},
			{034,"Massarrojos"},
			{035,"Moncada"},
			{036,"Seminari"},
			{037,"Masies"},
			{038,"Psiquiàtric"},
			{039,"Bétera"},
			{040,"Cantereria"},
			{041,"Benimàmet"},
			{042,"Carolines/Fira"},
			{043,"Campament"},
			{044,"Paterna"},
			{045,"Font del Jarro"},
			{046,"La Canyada"},
			{047,"La Vallesa"},
			{048,"El Clot"},
			{049,"Montesol"},
			{050,"L'Eliana"},
			{051,"Pobla de Vallbona"},
			{052,"Benaguasil 1r"},
			{053,"Benaguasil 2n"},
			{054,"Llíria"},
			{055,"P. Congressos"},
			{056,"Alboraya-Peris Aragó"},
			{057,"Almàssera"},
			{058,"Meliana"},
			{059,"Foios"},
			{060,"Albalat"},
			{061,"Museros"},
			{062,"Massamagrell"},
			{063,"Pobla de Farnals"},
			{064,"Rafelbunyol"},
			{065,"Entrepins"},
			{066,"Machado"},
			{067,"Benimaclet M"},
			{068,"Facultats"},
			{069,"Alameda "},
			{070,"Colón"},
			{071,"Xàtiva"},
			{072,"A. Guimera L3"},
			{073,"Av. del Cid"},
			{074,"9 d'Octubre"},
			{075,"Mislata"},
			{076,"Almassil"},
			{077,"Santa Rita"},
			{078,"Sant Isidre"},
			{079,"Alboraya-Palmaret"},
			{080,"Florista"},
			{081,"Garbí"},
			{082,"Benicalap"},
			{083,"Trànsits"},
			{084,"Marxalenes"},
			{085,"Reus"},
			{086,"Sagunt"},
			{087,"Pont de Fusta"},
			{088,"Primado Reig"},
			{089,"Benimaclet"},
			{090,"V. Zaragozá"},
			{091,"U. Politècnica"},
			{092,"La Carrasca"},
			{093,"Tarongers"},
			{094,"Serreria"},
			{095,"La Cadena"},
			{096,"Eugenia Viñes"},
			{097,"Les Arenes"},
			{098,"Dr. Lluch"},
			{099,"La Marina"},
			{100,"Fira"},
			{102,"V. Andrés E."},
			{103,"Campus"},
			{104,"Sant Joan"},
			{105,"La Granja"},
			{106,"Empalme L4"},
			{107,"Torrent Avgda."},
			{108,"Bailén"},
			{110,"TVV"},
			{111,"Santa Gemma-Parc C. UV"},
			{112,"Tomás y Val."},
			{113,"La Coma"},
			{114,"Mas del Rosari"},
			{115,"Ll. Terramelar"},
			{119,"Torre del Virrei"},
			{120,"Aragón"},
			{121,"Amistat"},
			{122,"Ayora"},
			{123,"Marítim-Serreria"},
			{124,"Francisco Cubells"},
			{125,"Grau-Canyamelar"},
			{126,"Marina Reial J.C.I"},
			{127,"Mediterrani"},
			{128,"Alfauir"},
			{129,"Orriols"},
			{130,"Estadi del Llevant"},
			{131,"Sant Miquel dels Reis"},
			{132,"Tossal del Rei"},
			{177,"Faitanar"},
			{178,"Quart de Poblet"},
			{179,"Salt de L'Aigua"},
			{180,"Manises"},
			{181,"Rosas"},
			{182,"Aeroport"},
			{183,"La Cova"},
			{184,"La Presa"},
			{185,"Masia del Traver"},
			{186,"Ribarroja del Turia"}
		};
		private string GetFerrocarrilEstationName(int estacion, int vestibulo)
		{
			string name = ferrocarrilEstacion.ContainsKey(estacion) ?
				ferrocarrilEstacion[estacion] :
				"e" + estacion.ToString();

			return name; // + " v" + vestibulo;
		}
		#endregion GetFerrocarrilEstationName

		#region GetBusEstationName
		private string GetBusEstationName(int linea, int convoy)
		{
			return "Línea " + linea + " (convoy " + convoy + ")";
		}
		#endregion GetBusEstationName

		#region GetValidationTypeNameAsync
		private async Task<string> GetValidationTypeNameAsync(EigeTipoValidacion_TransbordoEnum transbordo, EigeTipoValidacion_SentidoEnum sentido)
		{
			return await Task.Run(() =>
						transbordo == EigeTipoValidacion_TransbordoEnum.Transbordo ? "Transbordo" :
						sentido == EigeTipoValidacion_SentidoEnum.Entrada ? "Entrada" :
						"Salida"
			);
		}
		#endregion GetValidationTypeNameAsync

		#region GetValidationDateAsync
		private async Task<DateTime?> GetValidationDateAsync(DateTime? fechaHora)
		{
			return await Task.Run(() => fechaHora); // No hay que invocar a ToUTC() porque ya está en horario local del móvil
		}
		#endregion GetValidationDateAsync

		#region GetValidationQuantityAsync
		private async Task<DateTime?> GetValidationQuantityAsync(DateTime? fechaHora)
		{
			return await Task.Run(() => fechaHora); // No hay que invocar a ToUTC() porque ya está en horario local del móvil
		}
		#endregion GetValidationQuantityAsync

		#region GetValidationPeopleTravelingAsync
		private async Task<int> GetValidationPeopleTravelingAsync(int fechaHora)
		{
			return await Task.Run(() => fechaHora); // No hay que invocar a ToUTC() porque ya está en horario local del móvil
		}
		#endregion GetValidationPeopleTravelingAsync

		#region GetValidationPeopleInTransferAsync
		private async Task<int> GetValidationPeopleInTransferAsync(int numeroPersonasTrasbordo, int contadorViajerosSalida)
		{
			return await Task.Run(() =>
				TransferMode == 1 ? numeroPersonasTrasbordo :
				TransferMode == 2 ? contadorViajerosSalida :
				0); // No hay que invocar a ToUTC() porque ya está en horario local del móvil
		}
		private async Task<int> GetValidationPeopleInTransferTuinAsync(int numeroPersonasTrasbordo)
		{
			return await Task.Run(() => numeroPersonasTrasbordo);
		}
		#endregion GetValidationPeopleInTransferAsync

		#region GetValidationInternalTransfersAsync
		private async Task<int> GetValidationInternalTransfersAsync(int numeroPersonasTrasbordo)
		{
			return await Task.Run(() => numeroPersonasTrasbordo); // No hay que invocar a ToUTC() porque ya está en horario local del móvil
		}
		#endregion GetValidationInternalTransfersAsync

		#region GetValidationExternalTransfersAsync
		private async Task<int> GetValidationExternalTransfersAsync(int numeroPersonasTrasbordo)
		{
			return await Task.Run(() => numeroPersonasTrasbordo); // No hay que invocar a ToUTC() porque ya está en horario local del móvil
		}
		#endregion GetValidationExternalTransfersAsync

		#region GetValidationMaxInternalTransfersAsync
		private async Task<int> GetValidationMaxInternalTransfersAsync(int numeroPersonasTrasbordo)
		{
			return await Task.Run(() =>
				(TransferMode == 1) && (numeroPersonasTrasbordo == 3) ? 0 :
				(TransferMode == 2) ? 0 :
				numeroPersonasTrasbordo
			); // No hay que invocar a ToUTC() porque ya está en horario local del móvil
		}
		#endregion GetValidationMaInternalTransfersAsync

		#region HasHourValidityAsync
		public async Task<bool> HasHourValidity1Async(long uid, ITransportCardGetHasHourValidityScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[3].Blocks[0]))
			))
				return false;

			return
				(await HasHourValidityAsync(script.Card.Titulo.ValidezHoraria1.Value));
		}
		public async Task<bool> HasHourValidity2Async(long uid, ITransportCardGetHasHourValidityScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[4].Blocks[0]))
			))
				return false;

			return
				(await HasHourValidityAsync(script.Card.Titulo.ValidezHoraria2.Value));
		}
		private async Task<bool> HasHourValidityAsync(TituloValidezHorariaEnum validezHoraria)
		{
			return await Task.Run(() =>
			{
				return
					validezHoraria == TituloValidezHorariaEnum.HoraPunta ||
					validezHoraria == TituloValidezHorariaEnum.HoraValle ||
					validezHoraria == TituloValidezHorariaEnum.SinDistincion;
			});
		}
		#endregion HasHourValidityAsync

		#region HasDayValidityAsync
		public async Task<bool> HasDayValidity1Async(long uid, ITransportCardGetHasDayValidityScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[3].Blocks[0]))
			))
				return false;

			return
				(await HasDayValidityAsync(script.Card.Titulo.ValidezDiaria1.Value));
		}
		public async Task<bool> HasDayValidity2Async(long uid, ITransportCardGetHasDayValidityScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[4].Blocks[0]))
			))
				return false;

			return
				(await HasDayValidityAsync(script.Card.Titulo.ValidezDiaria2.Value));
		}
		private async Task<bool> HasDayValidityAsync(TituloValidezDiariaEnum validezDiaria)
		{
			return await Task.Run(() =>
			{
				return
					validezDiaria == TituloValidezDiariaEnum.DiasLectivosNoFestivos ||
					validezDiaria == TituloValidezDiariaEnum.FinDeSemana ||
					validezDiaria == TituloValidezDiariaEnum.LunesViernes ||
					validezDiaria == TituloValidezDiariaEnum.SinRestriccion ||
					validezDiaria == TituloValidezDiariaEnum.Viernes15Domingo;
			});
		}
		#endregion HasDayValidityAsync

		#region GetExpiredDateAsync
		public async Task<DateTime?> GetExpiredDateAsync(long uid, SigapuntScript script)
		{
			return await GetExpiredDateAsync(
				script.FechaCaducidad
			);
		}
		public async Task<DateTime?> GetExpiredDateAsync(long uid, ITransportCardGetExpiredDateScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[1].Blocks[0]))
			))
				return null;

			//Si es jove, la caducidad se calcula de otra forma
			if (
				(
					script.Card.Tarjeta.Subtipo.Value == EigeSubtipoTarjetaEnum.ViajeroPerso_JoveConDni ||
					script.Card.Tarjeta.Subtipo.Value == EigeSubtipoTarjetaEnum.ViajeroPerso_JoveConOtroId
				) &&
				script.Card.Tarjeta.Tipo.Value == EigeTipoTarjetaEnum.ViajeroPersonalizado)
				return await GetExpiredDateAsync(Convert.ToDateTime(script.Card.Emision.FechaEmision.Value).AddYears(5).AddMinutes(-1)
			);

			return await GetExpiredDateAsync(
				script.Card.Tarjeta.Caducidad.Value
			);
		}
		public async Task<DateTime?> GetExpiredDateAsync(DateTime? caducidad)
		{
			return await Task.Run(() =>
			{
				var expired =
					caducidad == null ? null :
					caducidad.Value == new DateTime(2000, 1, 1) ? (DateTime?)null :
					caducidad.Value;

				return expired;
			});
		}
		#endregion GetExpiredDateAsync

		#region GetExpiredYouthDateAsync
		public async Task<DateTime?> GetExpiredYouthDateAsync(long uid, ITransportCardGetExpiredDateScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[1].Blocks[0]))
			))
				return null;

			//Si es jove, la caducidad se calcula de otra forma
			return await GetExpiredYouthDateAsync(script.Card.Tarjeta.Tipo.Value, script.Card.Tarjeta.Subtipo.Value, script.Card.Tarjeta.Caducidad.Value);
		}
		public async Task<DateTime?> GetExpiredYouthDateAsync(EigeTipoTarjetaEnum type, EigeSubtipoTarjetaEnum subtype, DateTime? expiredDate)
		{
			if (
				(type == EigeTipoTarjetaEnum.ViajeroPersonalizado) &&
				(
					subtype == EigeSubtipoTarjetaEnum.ViajeroPerso_JoveConDni ||
					subtype == EigeSubtipoTarjetaEnum.ViajeroPerso_JoveConOtroId
				)
			)
				return await GetExpiredDateAsync(expiredDate);

			return null;
		}
        #endregion GetExpiredYouthDateAsync
        
        #region IsCardExpiredAsync
        public async Task<bool> IsCardExpiredAsync(long uid, SigapuntScript script, TransportCardSupport cardSupport, DateTime now)
		{
			return await IsCardExpiredAsync(
				script.FechaCaducidad,
                cardSupport,
				now
            );
		}
		public async Task<bool> IsCardExpiredAsync(long uid, ITransportCardGetExpiredDateScript script, TransportCardSupport cardSupport, DateTime now)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[1].Blocks[0]))
			))
				return false;

			//Si es jove, la caducidad se calcula de otra forma
			if ((script.Card.Tarjeta.Subtipo.Value == EigeSubtipoTarjetaEnum.ViajeroPerso_JoveConDni || script.Card.Tarjeta.Subtipo.Value == EigeSubtipoTarjetaEnum.ViajeroPerso_JoveConOtroId) && script.Card.Tarjeta.Tipo.Value == EigeTipoTarjetaEnum.ViajeroPersonalizado)
				return await IsCardExpiredAsync(
				Convert.ToDateTime(script.Card.Personalizacion.FechaPersonalizacion.Value).AddYears(5).AddMinutes(-1),
                cardSupport,
				now
			);

			return await IsCardExpiredAsync(
				script.Card.Tarjeta.Caducidad.Value,
                cardSupport,
				now
			);
		}
		public async Task<bool> IsCardExpiredAsync(DateTime? caducidad, TransportCardSupport cardSupport, DateTime now)
		{
			var expired = await GetExpiredDateAsync(caducidad);

			return (
                (
                    (cardSupport?.UsefulWhenExpired == false) &&
                    (expired != null) &&
                    (expired < now)
                )
            );
		}
		#endregion IsCardExpiredAsync

		#region GetCardTypeNameAsync
		public async Task<string> GetCardTypeNameAsync(long uid, SigapuntScript script)
		{
			return await GetCardTypeNameAsync(
				script.Tipo,
				script.Subtipo
			);
		}
		public async Task<string> GetCardTypeNameAsync(long uid, ITransportCardGetCardTypeNameScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[1].Blocks[0]))
			))
				return "";

			return await GetCardTypeNameAsync(script.Card.Tarjeta.Tipo.Value, script.Card.Tarjeta.Subtipo.Value);
		}
		private async Task<string> GetCardTypeNameAsync(EigeTipoTarjetaEnum? tipo, EigeSubtipoTarjetaEnum? subtipo)
		{
			return await Task.Run(() =>
			{
				var result = "";
				if (tipo == EigeTipoTarjetaEnum.Viajero)
				{
					result = "Viajero";
					if (subtipo == EigeSubtipoTarjetaEnum.ViajeroNoPerso)
						result += " no perso.";
					else if (subtipo == EigeSubtipoTarjetaEnum.ViajeroNoPerso_TarjetaNormalizadaValencia)
						result += " no perso. T.Normalizada";
					else if (subtipo == EigeSubtipoTarjetaEnum.ViajeroNoPerso_Turistico)
						result += " no perso. Turístico";

					return result;
				}
				else if (tipo == EigeTipoTarjetaEnum.ViajeroPersonalizado)
				{
					result = "Viajero personalizado";

					if (subtipo == EigeSubtipoTarjetaEnum.ViajeroPerso_ConDni || subtipo == EigeSubtipoTarjetaEnum.ViajeroPerso_ConOtroId)
					{ }
					else if (subtipo == EigeSubtipoTarjetaEnum.ViajeroPerso_ConvenioConDni || subtipo == EigeSubtipoTarjetaEnum.ViajeroPerso_ConvenioConOtroId)
						result += " Jove";
					else if (subtipo == EigeSubtipoTarjetaEnum.ViajeroPerso_DiscapacitadoConDni || subtipo == EigeSubtipoTarjetaEnum.ViajeroPerso_DiscapacitadoConOtroId)
						result += " Discapacitado";
					else if (subtipo == EigeSubtipoTarjetaEnum.ViajeroPerso_EstudianteConDni || subtipo == EigeSubtipoTarjetaEnum.ViajeroPerso_EstudianteConOtroId)
						result += " Estudiante";
					else if (subtipo == EigeSubtipoTarjetaEnum.ViajeroPerso_Familia)
						result += " Familia";
					else if (subtipo == EigeSubtipoTarjetaEnum.ViajeroPerso_JoveConDni || subtipo == EigeSubtipoTarjetaEnum.ViajeroPerso_JoveConOtroId)
						result += " Jove";
					else if (subtipo == EigeSubtipoTarjetaEnum.ViajeroPerso_MajorConDni || subtipo == EigeSubtipoTarjetaEnum.ViajeroPerso_MajorConOtroId)
						result += " Major";
					else if (subtipo == EigeSubtipoTarjetaEnum.ViajeroPerso_MenorConPadreConDni || subtipo == EigeSubtipoTarjetaEnum.ViajeroPerso_MenorConPadreOtroId)
						result += " Menor con padre";

					return result;
				}
				else if (tipo == EigeTipoTarjetaEnum.Inspector)
				{
					result = "Inspector / Mantenimiento";
					if (subtipo == EigeSubtipoTarjetaEnum.Empleado_Familiar)
						result = "Inspector Ferroviario";
					else if (subtipo == EigeSubtipoTarjetaEnum.Inspector_Autobus)
						result = "Inspector Autobus";
					else if (subtipo == EigeSubtipoTarjetaEnum.Inspector_aVM)
						result = "Inspector GV";
					else if (subtipo == EigeSubtipoTarjetaEnum.Mantenimiento_Ferroviario)
						result = "Mantenimiento Ferroviario";
					else if (subtipo == EigeSubtipoTarjetaEnum.Mantenimiento_Autobus)
						result = "Mantenimiento Autobús";

					return result;
				}
				else if (tipo == EigeTipoTarjetaEnum.Empleado)
				{
					result = "Empleado";
					if (subtipo == EigeSubtipoTarjetaEnum.Empleado_Familiar)
						result += " Familiar";
					else if (subtipo == EigeSubtipoTarjetaEnum.Empleado_Juvilado)
						result += " Jubilado";
					else if (subtipo == EigeSubtipoTarjetaEnum.Empleado_FamiliarJuvilado)
						result += " Familiar Jubilado";
					else if (subtipo == EigeSubtipoTarjetaEnum.Empleado_Inspector)
						result += " Inspector";

					return result;
				}
				else if (tipo == EigeTipoTarjetaEnum.Pase)
				{
					result = "Pase anónimo";
					if (subtipo == EigeSubtipoTarjetaEnum.PaseNoPerso_aVM)
						result += " GV";
					else if (subtipo == EigeSubtipoTarjetaEnum.PaseNoPerso_Operador)
						result += " Operador";
					else if (subtipo == EigeSubtipoTarjetaEnum.PaseNoPerso_Generalitat)
						result += " Generalitat";
					else if (subtipo == EigeSubtipoTarjetaEnum.PaseNoPerso_Subcontratas)
						result += " SubContratas";
					//else if (subtipo == EigeSubtipoTarjetaEnum.PaseNoPerso_General)
					//	result += "";
					else if (subtipo == EigeSubtipoTarjetaEnum.PaseNoPerso_OrganismosOficiales)
						result += " Organismos Oficiales";

					return result;
				}
				else if (tipo == EigeTipoTarjetaEnum.PasePersonalizado)
				{
					result = "Pase personalizado";
					if (subtipo == EigeSubtipoTarjetaEnum.PasePerso_aVM)
						result += " GV";
					else if (subtipo == EigeSubtipoTarjetaEnum.PasePerso_Operdor)
						result += " Operador";
					else if (subtipo == EigeSubtipoTarjetaEnum.PasePerso_Generalitat)
						result += " Generalitat";
					else if (subtipo == EigeSubtipoTarjetaEnum.PaseNoPerso_Subcontratas)
						result += " SubContratas";
					//else if (subtipo == EigeSubtipoTarjetaEnum.PaseNoPerso_General)
					//	result += "";

					return result;
				}
				else if (tipo == EigeTipoTarjetaEnum.Expendedor)
				{
					result = "Expendedor / Inspector";
					if (subtipo == EigeSubtipoTarjetaEnum.ExpendedorPerso)
						result = "Expendedor Personalizado";
					else if (subtipo == EigeSubtipoTarjetaEnum.ExpendedorCargaRecarga)
						result = "Expendedor Carga Recarga";
					else if (subtipo == EigeSubtipoTarjetaEnum.ExpendedorInspectoraVM)
						result = "Inspector GV";

					return result;
				}
				else if (tipo == EigeTipoTarjetaEnum.TarjetaCiudadana)
				{
					result = "Tarj. ciudadana";
					if (subtipo == EigeSubtipoTarjetaEnum.Ciudadano_Empradronado)
						result += " Empradronado";
					else if (subtipo == EigeSubtipoTarjetaEnum.Ciudadano_Forastero)
						result += " Forastero";
					else if (subtipo == EigeSubtipoTarjetaEnum.Ciudadano_GrupoMunicipio)
						result += " Grupo Municipio";
					else if (subtipo == EigeSubtipoTarjetaEnum.Ciudadano_GrupoOtroMunicipio)
						result += " Grupo Otro Municipio";
					else if (subtipo == EigeSubtipoTarjetaEnum.Ciudadano_EmpleadoAyuntamiento)
						result += " Empleado Ayuntamiento";
					else if (subtipo == EigeSubtipoTarjetaEnum.Ciudadano_Estudianto_Joven)
						result += " Estudiante Joven";
					else if (subtipo == EigeSubtipoTarjetaEnum.Ciudadano_Jubilado)
						result += " Jubilado";
					else if (subtipo == EigeSubtipoTarjetaEnum.Ciudadano_Municipal)
						result += " Municipal";
					else if (subtipo == EigeSubtipoTarjetaEnum.Ciudadano_ViajeroNoPerso)
						result += " Viajero No Personalizado";
					else if (subtipo == EigeSubtipoTarjetaEnum.Ciudadano_ViajeroPerso)
						result += " Viajero Personalizado";
					else if (subtipo == EigeSubtipoTarjetaEnum.Ciudadano_EstudianteOtroId)
						result += " Estudiante sin DNI";
					else if (subtipo == EigeSubtipoTarjetaEnum.Ciudadano_MinusvalidoOtroId)
						result += " Minusvalido sin DNI";
					else if (subtipo == EigeSubtipoTarjetaEnum.Ciudadano_Desempleado)
						result += " Desempleado"; // Rev 074
					else if (subtipo == EigeSubtipoTarjetaEnum.Ciudadano_SubvencionesSociales)
						result += " Subvenciones Sociales";
					else if (subtipo == EigeSubtipoTarjetaEnum.Ciudadano_FamiliaNumerosaGeneral20)
						result += " Familia Numerosa General 20";
					else if (subtipo == EigeSubtipoTarjetaEnum.Ciudadano_FamiliaNumerosaEspecial50)
						result += " Familia Numerosa Especial 50";

					return result;
				}
				else if (tipo == EigeTipoTarjetaEnum.ValenciaCardNoPerso)
				{
					result = "ValenciaCard anónima";
					return result;
				}
				else if (tipo == EigeTipoTarjetaEnum.ValenciaCardPerso)
				{
					result = "ValenciaCard personalizada";
					return result;
				}
				else if (tipo == EigeTipoTarjetaEnum.TarifaEspecial)
				{
					result = "Tarjeta tarifa especial";
					if (subtipo == EigeSubtipoTarjetaEnum.Tarifa_FamiliaNumerosaGeneral20)
						result = "Tarjeta Familia Numerosa General 20";
					else if (subtipo == EigeSubtipoTarjetaEnum.Tarifa_FamiliaNumerosaEspecial50)
						result = "Tarjeta Familia Numerosa Especial 50";
					else if (subtipo == EigeSubtipoTarjetaEnum.Tarifa_Desempleado)
						result = "Tarjeta Desempleo";
					else if (subtipo == EigeSubtipoTarjetaEnum.Tarifa_PensionistaClase1)
						result = "Tarjeta Pensionista 1";
					else if (subtipo == EigeSubtipoTarjetaEnum.Tarifa_PensionistaClase2)
						result = "Tarjeta Pensionista 2";

					return result;
				}
				else if (tipo == EigeTipoTarjetaEnum.DispotivoMovil)
				{
					result = "Dispositivo móvil";
					if (subtipo == EigeSubtipoTarjetaEnum.Movil_ViajeroNoPerso)
						result += " no perso.";

					return result;
				}

				return result;
			});
		}
		#endregion GetCardTypeNameAsync

		#region GetCardOwnerAsync
		public async Task<int?> GetCardOwnerAsync(long uid, SigapuntScript script)
		{
			return await GetCardOwnerAsync(script.EmpresaPropietaria);
		}
		public async Task<int?> GetCardOwnerAsync(long uid, ITransportCardGetCardOwnerNameScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[1].Blocks[0]))
			))
				return null;

			return await GetCardOwnerAsync(script.Card.Tarjeta.EmpresaPropietaria.Value);
		}
		public async Task<int?> GetCardOwnerAsync(int? empresaPropietaria)
		{
			return await Task.Run(() =>
			{
				return empresaPropietaria;
			});
		}
		#endregion GetCardOwnerAsync

		#region GetCardOwnerNameAsync
		public async Task<string> GetCardOwnerNameAsync(long uid, SigapuntScript sigapuntScript, FgvScript fgvScript, EmtScript emtScript)
		{
			var result = (sigapuntScript != null) ?
				await GetCardOwnerNameAsync(EigeCodigoEntornoTarjetaEnum.Valencia, sigapuntScript.EmpresaPropietaria) :
				"";
			if (!result.IsNullOrEmpty())
				return result;

			return
				sigapuntScript != null ? "GV" :
				fgvScript != null ? "FGV" :
				emtScript != null ? "EMT" :
				"";
		}
		public async Task<string> GetCardOwnerNameAsync(long uid, ITransportCardGetCardOwnerNameScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[0].Blocks[1])) &&
				(await script.Card.CheckAsync(uid, script.Card.Sectors[1].Blocks[0]))
			))
				return "";

			return await GetCardOwnerNameAsync(script.Card.Tarjeta.CodigoEntorno.Value, script.Card.Tarjeta.EmpresaPropietaria.Value) ?? "";
		}
		private async Task<string> GetCardOwnerNameAsync(EigeCodigoEntornoTarjetaEnum codigoEntorno, int? code)
		{
			return await Task.Run(() =>
			{
				if (code == null)
					return null;

				if (codigoEntorno == EigeCodigoEntornoTarjetaEnum.Valencia)
				{
					if (code == 1) return "GV";
					else if (code == 2) return "FGV";
					else if (code == 3) return "EMT";
					else if (code == 4) return "AUVACA";
					else if (code == 5) return "EDETANIA";
					else if (code == 6) return "AVSA";
					else if (code == 7) return "BUÑOL";
					else if (code == 8) return "HERCA";
					else if (code == 9) return "FERNANBUS";
					else if (code == 10) return "TORRENTE";
					else if (code == 11) return "UBESA";
					else if (code == 12) return "RENFE";
					else if (code == 13) return "VAlencia - Ribarroja";
					else if (code == 14) return "URBETUR";
					else if (code == 15) return "METRORBITAL";
					else if (code == 16) return "ADIF";
					else if (code == 17) return "MAREA";
					else if (code == 19) return "Caixa Popular";
					else if (code == 20) return "GV Logista";
					else if (code == 21) return "FV Panini";
					else if (code == 22) return "GV Valdisme";
					else if (code == 23) return "Pay[in]"; // "YUM-Yo Unique Money";
					else if (code == 24) return "Banco Bankia";
					else if (code == 25) return "Sagunto";
					else if (code == 26) return "Togsa";
					else if (code == 27) return "Hispano Chelvana";
					else if (code == 28) return "Alboraya";
					else if (code == 29) return "Paterna";
					else if (code == 30) return "Castellón";
					else if (code == 31) return "Turismo Valencia";
				}
				else if (codigoEntorno == EigeCodigoEntornoTarjetaEnum.Alicante)
				{
					if (code == 1) return "TAM";
					else if (code == 2) return "MASATUSA";
					else if (code == 3) return "TRAM";
					else if (code == 4) return "ALCOYANA";
					else if (code == 9) return "SAETIC";
					else if (code == 10) return "VALDISM";
					else if (code == 11) return "CALMELL";
					else if (code == 12) return "RENFE";
					else if (code == 13) return "AUTOCARES BAILE";
					else if (code == 14) return "PARKEON";
					else if (code == 16) return "ADIF";
					else if (code == 17) return "MAREA MAREA";
					else if (code == 18) return "TRANSERMOBILE";
					else if (code == 19) return "TUASA";
					else if (code == 20) return "Llorente";
					else if (code == 24) return "Bankia";
					else if (code == 30) return "Ayto. Campello";
					else if (code == 31) return "Ayto. Alicante";
				}
				else if (codigoEntorno == EigeCodigoEntornoTarjetaEnum.ComunitatValenciana)
				{
				}

				return null;
			});
		}
        #endregion GetCardOwnerNameAsync

        #region GetSupportCardAsync
        public async Task<TransportCardSupport> GetSupportCardAsync(long uid, SigapuntScript script, DateTime now)
        {
            return await GetSupportCardAsync(
                uid,
                script.EmpresaPropietaria,
                script.Tipo,
                script.Subtipo,
                now
            );
        }
        public async Task<TransportCardSupport> GetSupportCardAsync(long uid, TransportCardGetReadInfoScript script, DateTime now)
        {
            return await GetSupportCardAsync(
                uid,
                script.Card.Tarjeta.EmpresaPropietaria.Value,
                script.Card.Tarjeta.Tipo.Value,
                script.Card.Tarjeta.Subtipo.Value,
                now);
        }
        private async Task<TransportCardSupport> GetSupportCardAsync(long uid, int? empresaPropietaria, EigeTipoTarjetaEnum? tarjetaTipo, EigeSubtipoTarjetaEnum? tarjetaSubtipo, DateTime now)
        {
            if (
                empresaPropietaria == null ||
                tarjetaTipo == null ||
                tarjetaSubtipo == null
            )
                return null;

            var support = (await RepositorySupport.GetAsync())
                .Where(x =>
                    (x.OwnerCode == empresaPropietaria) &&
                    (x.Type == (int)tarjetaTipo) &&
                    (
                        (x.SubType == null) ||
                        (x.SubType == (int)tarjetaSubtipo)
                    )
                )
                .OrderBy(x => x.SubType ?? 1000)
                .FirstOrDefault();

            return support;
        }
        #endregion GetSupportCardAsync

        #region HasTariff
        public async Task<bool> HasTariff1Async(long uid, SigapuntScript script)
		{
			return await HasTariffAsync(script.TipoTarifa1);
		}
		public async Task<bool> HasTariff1Async(long uid, ITransportCardGetHasTariffScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[3].Blocks[0]))
			))
				return false;

			return await HasTariffAsync(script.Card.Titulo.TipoTarifa1.Value);
		}
		public async Task<bool> HasTariff2Async(long uid, SigapuntScript script)
		{
			return await HasTariffAsync(script.TipoTarifa2);
		}
		public async Task<bool> HasTariff2Async(long uid, ITransportCardGetHasTariffScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[4].Blocks[0]))
			))
				return false;

			return await HasTariffAsync(script.Card.Titulo.TipoTarifa2.Value);
		}
		public async Task<bool> HasTariffMAsync(long uid, SigapuntScript script)
		{
			return await Task.Run(() => { return false; });
		}
		public async Task<bool> HasTariffMAsync(long uid, ITransportCardGetHasTariffScript script)
		{
			return await Task.Run(() => { return false; });
		}
		public async Task<bool> HasTariffBAsync(long uid, SigapuntScript script)
		{
			return await Task.Run(() => { return false; });
		}
		public async Task<bool> HasTariffBAsync(long uid, ITransportCardGetHasTariffScript script)
		{
			return await Task.Run(() => { return false; });
		}
		public async Task<bool> HasTariffAsync(EigeTituloTipoTarifaEnum? tipoTarifa)
		{
			return await Task.Run(() =>
			{
				return
					tipoTarifa == EigeTituloTipoTarifaEnum.Normal ||
					tipoTarifa == EigeTituloTipoTarifaEnum.Joven ||
					tipoTarifa == EigeTituloTipoTarifaEnum.Mayores ||
					tipoTarifa == EigeTituloTipoTarifaEnum.Promocional ||
					tipoTarifa == EigeTituloTipoTarifaEnum.Estudiante ||
					tipoTarifa == EigeTituloTipoTarifaEnum.Discapacitado ||
					tipoTarifa == EigeTituloTipoTarifaEnum.Familia1 ||
					tipoTarifa == EigeTituloTipoTarifaEnum.Familia2 ||
					tipoTarifa == EigeTituloTipoTarifaEnum.Desempleado;
			});
		}
		#endregion HasTariff

		// Hay que revisar
		#region InBlackListAsync

		public async Task<bool> InBlackListAsync(long uid, ITransportCardCheckBlackListScript script)
		{
			return await InBlackListAsync(uid, script.Card.Validacion.ListaNegra.Value, script.Card.Usuario.DesbloqueoListaNegra.Value);
		}
		private async Task<bool> InBlackListAsync(long uid, bool listaNegra, bool desbloqueoListaNegra)
		{
			if (listaNegra && !desbloqueoListaNegra)
				return true;

			var now = DateTime.Now;
			var itemBlackList = (await BlackListRepository.GetAsync())
				.Where(x =>
					x.Uid == uid &&
					x.State == BlackList.BlackListStateType.Active &&
					x.Machine == (x.Machine | BlackListMachineType.Charge) &&
					!x.Resolved
				)
				.Any();

			return itemBlackList;
		}
		#endregion InBlackListAsync

		#region IsRechargableAsync
		public async Task<bool> IsRechargable1Async(long uid, SigapuntScript script)
		{
			return await Task.Run(() =>
			{
				return false;
			});
		}
		public async Task<bool> IsRechargable1Async(long uid, ITransportCardIsRechargableScript script, TransportCardSupport cardSupport, DateTime now)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[1].Blocks[0]))
			))
				return false;

			return await IsRechargableAsync(script.Card.Tarjeta.Caducidad.Value, script.Card.Tarjeta.Tipo.Value, script.Card.Tarjeta.Subtipo.Value, cardSupport, now);
		}
		public async Task<bool> IsRechargable2Async(long uid, SigapuntScript script)
		{
			return await Task.Run(() =>
			{
				return false;
			});
		}
		public async Task<bool> IsRechargable2Async(long uid, ITransportCardIsRechargableScript script, TransportCardSupport cardSupport, DateTime now)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[1].Blocks[0]))
			))
				return false;

			return await IsRechargableAsync(script.Card.Tarjeta.Caducidad.Value, script.Card.Tarjeta.Tipo.Value, script.Card.Tarjeta.Subtipo.Value, cardSupport, now);
		}
		public async Task<bool> IsRechargableMAsync(long uid, SigapuntScript script)
		{
			return await Task.Run(() =>
			{
				return false;
			});
		}
		public async Task<bool> IsRechargableMAsync(long uid, ITransportCardIsRechargableScript script, TransportCardSupport cardSupport, DateTime now)
		{
			return await Task.Run(() =>
			{
				return false;
			});
		}
		public async Task<bool> IsRechargableBAsync(long uid, SigapuntScript script)
		{
			return await Task.Run(() =>
			{
				return false;
			});
		}
		public async Task<bool> IsRechargableBAsync(long uid, ITransportCardIsRechargableScript script, TransportCardSupport cardSupport, DateTime now)
		{
			return await Task.Run(() =>
			{
				return false;
			});
		}
		private async Task<bool> IsRechargableAsync(DateTime? caducidad, EigeTipoTarjetaEnum tipo, EigeSubtipoTarjetaEnum subtipo, TransportCardSupport cardSupport, DateTime now)
		{
			return (
				(!(await IsCardExpiredAsync(caducidad, cardSupport, now))) &&
				(!(await GetCardTypeNameAsync(tipo, subtipo)).IsNullOrEmpty())
			);
		}
		#endregion InBlackListAsync

		#region GetTitleActiveAsync
		public async Task<bool> GetTitleActive1Async(long uid, SigapuntScript script)
		{
			return await Task.Run(() =>
			 {
				 return script.TituloActivo1 == null ? false : script.TituloActivo1.Value;
			 });
		}
		public async Task<bool> GetTitleActive1Async(long uid, ITransportCardGetTitleCodeScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[1].Blocks[1]))
			))
				return false;

			return
				((script.Card.Titulo.TitulosActivos.Value & EigeTitulosActivosEnum.Titulo1) == EigeTitulosActivosEnum.Titulo1) &&
				(await GetTitleCode1Async(uid, script) != 0);
		}
		public async Task<bool> GetTitleActive2Async(long uid, SigapuntScript script)
		{
			return await Task.Run(() =>
			{
				return script.TituloActivo2 == null ? false : script.TituloActivo2.Value;
			});
		}
		public async Task<bool> GetTitleActive2Async(long uid, ITransportCardGetTitleCodeScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[1].Blocks[1]))
			))
				return false;

			return
				((script.Card.Titulo.TitulosActivos.Value & EigeTitulosActivosEnum.Titulo2) == EigeTitulosActivosEnum.Titulo2) &&
				(await GetTitleCode2Async(uid, script) != 0);
		}
		public async Task<bool> GetTitleActiveMAsync(long uid, SigapuntScript script)
		{
			return await Task.Run(() =>
			{
				return script.TituloActivoM == null ? false : script.TituloActivoM.Value;
			});
		}
		public async Task<bool> GetTitleActiveMAsync(long uid, ITransportCardGetTitleCodeScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[1].Blocks[1]))
			))
				return false;

			return (script.Card.Titulo.TitulosActivos.Value & EigeTitulosActivosEnum.Monedero) == EigeTitulosActivosEnum.Monedero;
		}
		public async Task<bool> GetTitleActiveBAsync(long uid, SigapuntScript script)
		{
			return await Task.Run(() =>
			{
				return script.TituloActivoB == null ? false : script.TituloActivoB.Value;
			});
		}
		public async Task<bool> GetTitleActiveBAsync(long uid, ITransportCardGetTitleCodeScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[1].Blocks[1]))
			))
				return false;

			return (script.Card.Titulo.TitulosActivos.Value & EigeTitulosActivosEnum.Bonus) == EigeTitulosActivosEnum.Bonus;
		}
		#endregion GetTitleCodeAsync

		#region GetTitleCodeAsync
		public async Task<int?> GetTitleCode1Async(long uid, SigapuntScript script)
		{
			if (script.CodigoTitulo1 == null)
				return null;

			return await GetTitleCodeAsync(
				script.CodigoTitulo1.Value
			);
		}
		public async Task<int?> GetTitleCode1Async(long uid, ITransportCardGetTitleCodeScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[3].Blocks[0]))
			))
				return null;

			return await GetTitleCodeAsync(
				script.Card.Titulo.CodigoTitulo1.Value
			);
		}
		public async Task<int?> GetTitleCode2Async(long uid, SigapuntScript script)
		{
			if (script.CodigoTitulo2 == null)
				return null;

			return await GetTitleCodeAsync(
				script.CodigoTitulo2.Value
			);
		}
		public async Task<int?> GetTitleCode2Async(long uid, ITransportCardGetTitleCodeScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[4].Blocks[0]))
			))
				return null;

			return await GetTitleCodeAsync(script.Card.Titulo.CodigoTitulo2.Value);
		}
		public async Task<int?> GetTitleCodeMAsync(long uid, SigapuntScript script)
		{
			return await Task.Run(() =>
			{
				return (int?)null;
			});
		}
		public async Task<int?> GetTitleCodeMAsync(long uid, ITransportCardGetTitleCodeScript script)
		{
			return await Task.Run(() =>
			{
				return (int?)null;
			});
		}
		public async Task<int?> GetTitleCodeBAsync(long uid, SigapuntScript script)
		{
			return await Task.Run(() =>
			{
				return (int?)null;
			});
		}
		public async Task<int?> GetTitleCodeBAsync(long uid, ITransportCardGetTitleCodeScript script)
		{
			return await Task.Run(() =>
			{
				return (int?)null;
			});
		}
		public async Task<int?> GetTitleCodeAsync(int code)
		{
			return await Task.Run(() =>
			{
				return code;
			});
		}
		#endregion GetTitleCodeAsync

		// Buscar en BD
		#region GetTitleNameAsync
		public async Task<string> GetTitleName1Async(long uid, SigapuntScript script)
		{
			if (script.CodigoTitulo1 == null)
				return "Desconocido";

			return await GetTitleNameAsync(script.CodigoTitulo1.Value);
		}
		public async Task<string> GetTitleName1Async(long uid, ITransportCardGetTitleNameScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[3].Blocks[0]))
			))
				return "";

			return await GetTitleNameAsync(script.Card.Titulo.CodigoTitulo1.Value);
		}
		public async Task<string> GetTitleName2Async(long uid, SigapuntScript script)
		{
			if (script.CodigoTitulo2 == null)
				return "Desconocido";

			return await GetTitleNameAsync(script.CodigoTitulo2.Value);
		}
		public async Task<string> GetTitleName2Async(long uid, ITransportCardGetTitleNameScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[4].Blocks[0]))
			))
				return "";

			return await GetTitleNameAsync(script.Card.Titulo.CodigoTitulo2.Value);
		}
		public async Task<string> GetTitleNameMAsync(long uid, SigapuntScript script)
		{
			return await Task.Run(() => "Monedero");
		}
		public async Task<string> GetTitleNameMAsync(long uid, ITransportCardGetTitleNameScript script)
		{
			return await Task.Run(() => "Monedero");
		}
		public async Task<string> GetTitleNameBAsync(long uid, SigapuntScript script)
		{
			return await Task.Run(() => "Bonus");
		}
		public async Task<string> GetTitleNameBAsync(long uid, ITransportCardGetTitleNameScript script)
		{
			return await Task.Run(() => "Bonus");
		}

		public async Task<string> GetTitleNameAsync(int code)
		{
			return await Task.Run(() =>
			{
				if (!Titulos.ContainsKey(code))
					return "Título '" + code + "' desconocido";

				var titulo = Titulos[code];
				return titulo == null ? "Título '" + code + "' desconocido" : titulo.Name;
			});
		}
		#endregion GetTitleNameAsync

		#region GetTitleOwnerNameAsync
		public async Task<string> GetTitleOwnerName1Async(long uid, SigapuntScript sigapuntScript, FgvScript fgvScript, EmtScript emtScript)
		{
			return await GetTitleOwnerNameAsync(sigapuntScript, fgvScript, emtScript);
		}
		public async Task<string> GetTitleOwnerName1Async(long uid, ITransportCardGetTitleOwnerNameScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[0].Blocks[1])) &&
				(await script.Card.CheckAsync(uid, script.Card.Sectors[3].Blocks[0]))
			))
				return "";

			return await GetCardOwnerNameAsync(script.Card.Tarjeta.CodigoEntorno.Value, script.Card.Titulo.EmpresaPropietaria1.Value) ?? "";
		}
		public async Task<string> GetTitleOwnerName2Async(long uid, SigapuntScript sigapuntScript, FgvScript fgvScript, EmtScript emtScript)
		{
			return await GetTitleOwnerNameAsync(sigapuntScript, fgvScript, emtScript);
		}
		public async Task<string> GetTitleOwnerName2Async(long uid, ITransportCardGetTitleOwnerNameScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[0].Blocks[1])) &&
				(await script.Card.CheckAsync(uid, script.Card.Sectors[4].Blocks[0]))
			))
				return "";

			return await GetCardOwnerNameAsync(script.Card.Tarjeta.CodigoEntorno.Value, script.Card.Titulo.EmpresaPropietaria2.Value) ?? "";
		}
		public async Task<string> GetTitleOwnerNameMAsync(long uid, SigapuntScript sigapuntScript, FgvScript fgvScript, EmtScript emtScript)
		{
			return await GetTitleOwnerNameAsync(sigapuntScript, fgvScript, emtScript);
		}
		public async Task<string> GetTitleOwnerNameMAsync(long uid, ITransportCardGetTitleOwnerNameScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[0].Blocks[1])) &&
				(await script.Card.CheckAsync(uid, script.Card.Sectors[6].Blocks[0]))
			))
				return "";

			return await GetCardOwnerNameAsync(script.Card.Tarjeta.CodigoEntorno.Value, script.Card.Titulo.EmpresaPropietariaM.Value);
		}
		public async Task<string> GetTitleOwnerNameBAsync(long uid, SigapuntScript sigapuntScript, FgvScript fgvScript, EmtScript emtScript)
		{
			return await GetTitleOwnerNameAsync(sigapuntScript, fgvScript, emtScript);
		}
		public async Task<string> GetTitleOwnerNameBAsync(long uid, ITransportCardGetTitleOwnerNameScript script)
		{

			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[0].Blocks[1])) &&
				(await script.Card.CheckAsync(uid, script.Card.Sectors[5].Blocks[0]))
			))
				return "";

			return await GetCardOwnerNameAsync(script.Card.Tarjeta.CodigoEntorno.Value, script.Card.Titulo.EmpresaPropietariaB.Value);
		}
		public async Task<string> GetTitleOwnerNameAsync(long uid, EmtScript emtScript)
		{
			return await GetTitleOwnerNameAsync(null, null, emtScript);
		}
		public async Task<string> GetTitleOwnerNameAsync(long uid, FgvScript fgvScript)
		{
			return await GetTitleOwnerNameAsync(null, fgvScript, null);
		}
		public async Task<string> GetTitleOwnerNameAsync(SigapuntScript sigapuntScript, FgvScript fgvScript, EmtScript emtScript)
		{
			return await Task.Run(() =>
				sigapuntScript != null ? "GV" :
				fgvScript != null ? "FGV" :
				emtScript != null ? "EMT" :
				""
			);
		}
		#endregion GetTitleNameAsync

		#region GetTitleZoneNameAsync
		public async Task<EigeZonaEnum?> GetTitleZoneName1Async(long uid, SigapuntScript script)
		{
			if (script.ValidezZonal1 == null)
				return null;

			return await GetZoneNameAsync((EigeZonaEnum)script.ValidezZonal1.Value);
		}
		public async Task<EigeZonaEnum?> GetTitleZoneName1Async(long uid, ITransportCardGetTitleZoneNameScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[3].Blocks[0]))
			))
				return null;

			return await GetZoneNameAsync((EigeZonaEnum)script.Card.Titulo.ValidezZonal1.Value);
		}
		public async Task<EigeZonaEnum?> GetTitleZoneName2Async(long uid, SigapuntScript script)
		{
			if (script.ValidezZonal2 == null)
				return null;

			return await GetZoneNameAsync((EigeZonaEnum)script.ValidezZonal2.Value);
		}
		public async Task<EigeZonaEnum?> GetTitleZoneName2Async(long uid, ITransportCardGetTitleZoneNameScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[4].Blocks[0]))
			))
				return null;

			return await GetZoneNameAsync((EigeZonaEnum)script.Card.Titulo.ValidezZonal2.Value);
		}
		public async Task<EigeZonaEnum?> GetTitleZoneNameMAsync(long uid, SigapuntScript script)
		{
			return await Task.Run(() => (EigeZonaEnum?)null);
		}
		public async Task<EigeZonaEnum?> GetTitleZoneNameMAsync(long uid, ITransportCardGetTitleZoneNameScript script)
		{
			return await Task.Run(() => (EigeZonaEnum?)null);
		}
		public async Task<EigeZonaEnum?> GetTitleZoneNameBAsync(long uid, SigapuntScript script)
		{
			return await Task.Run(() => (EigeZonaEnum?)null);
		}
		public async Task<EigeZonaEnum?> GetTitleZoneNameBAsync(long uid, ITransportCardGetTitleZoneNameScript script)
		{
			return await Task.Run(() => (EigeZonaEnum?)null);
		}
		private async Task<EigeZonaEnum?> GetZoneNameAsync(EigeZonaEnum? validezZonal)
		{
			return await Task.Run(() =>
			{
				return
					validezZonal == 0 ? null : validezZonal;
			});
		}
		#endregion GetTitleNameAsync

		#region GetTitleCaducityAsync
		public async Task<DateTime?> GetTitleCaducity1Async(long uid, SigapuntScript script)
		{
			if (script.FechaValidez1 == null)
				return null;

			return await GetTitleCaducityAsync(script.FechaValidez1.Value);
		}
		public async Task<DateTime?> GetTitleCaducity1Async(long uid, ITransportCardGetTitleCaducityScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[1].Blocks[1]))
			))
				return null;

			return await GetTitleCaducityAsync(script.Card.Titulo.FechaValidez1.Value);
		}
		public async Task<DateTime?> GetTitleCaducity2Async(long uid, SigapuntScript script)
		{
			if (script.FechaValidez2 == null)
				return null;

			return await GetTitleCaducityAsync(script.FechaValidez2.Value);
		}
		public async Task<DateTime?> GetTitleCaducity2Async(long uid, ITransportCardGetTitleCaducityScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[4].Blocks[1]))
			))
				return null;

			return await GetTitleCaducityAsync(script.Card.Titulo.FechaValidez2.Value);
		}
		public async Task<DateTime?> GetTitleCaducityMAsync(long uid, SigapuntScript script)
		{

			return await Task.Run(() => (DateTime?)null);
		}
		public async Task<DateTime?> GetTitleCaducityMAsync(long uid, ITransportCardGetTitleCaducityScript script)
		{
			return await Task.Run(() => (DateTime?)null);
		}
		public async Task<DateTime?> GetTitleCaducityBAsync(long uid, SigapuntScript script)
		{
			return await Task.Run(() => (DateTime?)null);
		}
		public async Task<DateTime?> GetTitleCaducityBAsync(long uid, ITransportCardGetTitleCaducityScript script)
		{
			return await Task.Run(() => (DateTime?)null);
		}
		private async Task<DateTime?> GetTitleCaducityAsync(DateTime? fechaValidez)
		{
			return await Task.Run(() =>
			{
				return
					fechaValidez == null ? (DateTime?)null :
						fechaValidez.Value.Subtract(TimeSpan.FromSeconds(1)); // No hay que invocar a ToUTC() porque ya está en horario local del móvil;
			});
		}
		#endregion GetTitleCaducityAsync			

		#region GetTitleIsExhaustedAsync
		public async Task<bool> GetTitleIsExhausted1Async(long uid, SigapuntScript script, DateTime now)
		{
			if (script == null || script.TituloEnAmpliacion1 == null)
				return false;

			return IsTuiN(script.CodigoTitulo1) ?
				await GetTitleIsExhaustedTuinAsync(false, script.SaldoViaje1) :
				await GetTitleIsExhaustedAsync(null, script.NumeroUnidadesValidezTemporal1, script.FechaValidez1, script.SaldoViaje1 != null, script.SaldoViaje1, script.TituloEnAmpliacion1.Value, now);
		}
		public async Task<bool> GetTitleIsExhausted1Async(long uid, ITransportCardCheckExhaustedScript script, DateTime now)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[3].Blocks[0])) &&
				(await script.Card.CheckAsync(uid, script.Card.Sectors[1].Blocks[1])) &&
				(await script.Card.CheckAsync(uid, script.Card.Sectors[9].Blocks[0]))
			))
				return false;

			return IsTuiN(script.Card.Titulo.CodigoTitulo1.Value) ?
				await GetTitleIsExhaustedTuinAsync(script.Card.TituloTuiN.SaldoViaje_Sign.Value, script.Card.TituloTuiN.SaldoViaje_Value.Value) :
				await GetTitleIsExhaustedAsync(script.Card.Titulo.TipoUnidadesValidezTemporal1.Value, script.Card.Titulo.NumeroUnidadesValidezTemporal1.Value, script.Card.Titulo.FechaValidez1.Value, script.Card.Titulo.UsoDelCampoSaldoViajes1.Value, script.Card.Titulo.SaldoViaje1.Value, script.Card.Titulo.TituloEnAmpliacion1.Value, now);
		}
		public async Task<bool> GetTitleIsExhausted2Async(long uid, SigapuntScript script, DateTime now)
		{
			if (script == null || script.TituloEnAmpliacion2 == null)
				return false;

			return IsTuiN(script.CodigoTitulo2) ?
				await GetTitleIsExhaustedTuinAsync(false, script.SaldoViaje2) :
				await GetTitleIsExhaustedAsync(null, script.NumeroUnidadesValidezTemporal2, script.FechaValidez2, script.SaldoViaje2 != null, script.SaldoViaje2, script.TituloEnAmpliacion2.Value, now);
		}
		public async Task<bool> GetTitleIsExhausted2Async(long uid, ITransportCardCheckExhaustedScript script, DateTime now)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[4].Blocks[0])) &&
				(await script.Card.CheckAsync(uid, script.Card.Sectors[4].Blocks[1])) &&
				(await script.Card.CheckAsync(uid, script.Card.Sectors[9].Blocks[0]))
			))
				return false;

			return IsTuiN(script.Card.Titulo.CodigoTitulo2.Value) ?
				await GetTitleIsExhaustedTuinAsync(script.Card.TituloTuiN.SaldoViaje_Sign.Value, script.Card.TituloTuiN.SaldoViaje_Value.Value) :
				await GetTitleIsExhaustedAsync(script.Card.Titulo.TipoUnidadesValidezTemporal2.Value, script.Card.Titulo.NumeroUnidadesValidezTemporal2.Value, script.Card.Titulo.FechaValidez2.Value, script.Card.Titulo.UsoDelCampoSaldoViajes2.Value, script.Card.Titulo.SaldoViaje2.Value, script.Card.Titulo.TituloEnAmpliacion2.Value, now);
		}
		public async Task<bool> GetTitleIsExhaustedMAsync(long uid, SigapuntScript script, DateTime now)
		{
			return await Task.Run(() =>
			{
				return script.SaldoMonedero == 0;
			});
		}
		public async Task<bool> GetTitleIsExhaustedMAsync(long uid, ITransportCardCheckExhaustedScript script, DateTime now)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[6].Blocks[0]))
			))
				return false;

			return await Task.Run(() =>
			{
				return script.Card.Titulo.SaldoMonedero.Value == 0;
			});
		}
		public async Task<bool> GetTitleIsExhaustedBAsync(long uid, SigapuntScript script, DateTime now)
		{
			return await Task.Run(() =>
			{
				return script.SaldoBonus <= 0;
			});
		}
		public async Task<bool> GetTitleIsExhaustedBAsync(long uid, ITransportCardCheckExhaustedScript script, DateTime now)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[5].Blocks[0]))
			))
				return false;

			return await Task.Run(() =>
			{
				return script.Card.Titulo.SaldoBonus.Value <= 0;
			});
		}
		private async Task<bool> GetTitleIsExhaustedAsync(EigeTipoUnidadesValidezTemporalEnum? tipoUnidades, int? numeroUnidades, DateTime? fechaValidez, bool usoSaldo, int? saldo, bool enAmpliacion, DateTime now)
		{
			return (
				(
					(await GetTitleHasBalanceAsync(usoSaldo)) &&
					(await GetTitleBalanceAsync(saldo) <= 0)
				) || (
					(numeroUnidades != null) &&
					(await GetTitleIsTemporalAsync(numeroUnidades)) &&
					(await GetTitleAmpliationAsync(enAmpliacion, fechaValidez) == 0) &&
					(
						((tipoUnidades == EigeTipoUnidadesValidezTemporalEnum.Horas) && (fechaValidez.Value.FloorHour().AddHours(numeroUnidades.Value) <= now)) ||
						((tipoUnidades == EigeTipoUnidadesValidezTemporalEnum.Dias) && (fechaValidez.Value.Date.AddDays(numeroUnidades.Value) <= now)) ||
						((tipoUnidades == EigeTipoUnidadesValidezTemporalEnum.Meses) && (fechaValidez.Value.FloorMonth().AddMonths(numeroUnidades.Value) <= now)) ||
						((tipoUnidades == EigeTipoUnidadesValidezTemporalEnum.Anyos) && (fechaValidez.Value.FloorYear().AddYears(numeroUnidades.Value) <= now))
					)
				)
			);
		}
		private async Task<bool> GetTitleIsExhaustedTuinAsync(bool saldoSign, decimal? saldo)
		{
			return (
				(await GetTitleHasBalanceTuinAsync()) &&
				(await GetTitleBalanceTuinAsync(saldoSign, saldo) <= 0)

			);
		}
		#endregion GetTitleIsExhaustedAsync

		#region GetTitleIsExpiredAsync
		private async Task<bool> GetTitleIsExpiredAsync(long uid, int codigoTitulo, EigeZonaEnum zone, int version, DateTime now)
		{
			var result2 = (await RepositoryTitle.GetAsync())
			.Where(x =>
			    x.Code == codigoTitulo &&
			    x.State == TransportTitleState.Active &&
			    !x.Prices
			        .Where(y =>
			            // Price
			            (y.State == TransportPriceState.Active) &&
			            // Between dates
			            (y.Start <= now) &&
			            (y.End >= now) &&
			            // Zona
			            ((!x.HasZone) || (y.Zone == zone)) &&
			            // Version
			            (y.Version == version)
			        )
			        .Any() &&
			    x.Prices
			        .Where(y =>
			            // Price
			            (y.State == TransportPriceState.Active) &&
			            // Zona
			            ((!x.HasZone) || (y.Zone == zone)) &&
			            // Version
			            (y.Version >= version)
			        )
			        .Any()
			 ).ToList();


			var result = (await RepositoryTitle.GetAsync())
			    .Where(x =>
			        x.Code == codigoTitulo &&
			        x.State == TransportTitleState.Active &&
			        !x.Prices
			            .Where(y =>
			                // Price
			                (y.State == TransportPriceState.Active) &&
			                // Between dates
			                (y.Start <= now) &&
			                (y.End >= now) &&
			                // Zona
			                ((!x.HasZone) || (y.Zone == zone)) &&
			                // Version
			                (y.Version == version)
			            )
			            .Any() &&
			        x.Prices
			            .Where(y =>
			                // Price
			                (y.State == TransportPriceState.Active) &&
			                // Zona
			                ((!x.HasZone) || (y.Zone == zone)) &&
			                // Version
			                (y.Version >= version)
			            )
			            .Any()
			    ).Any();

			return result;
		}
		public async Task<bool> GetTitleIsExpired1Async(long uid, SigapuntScript script, DateTime now)
		{
			return await Task.Run(() => false);
		}
		public async Task<bool> GetTitleIsExpired1Async(long uid, ITransportCardCheckExhaustedScript script, DateTime now)
		{
			var result = await GetTitleIsExpiredAsync(uid,
			 script.Card.Titulo.CodigoTitulo1.Value,
			 script.Card.Titulo.ValidezZonal1.Value,
			 script.Card.Titulo.ControlTarifa1.Value,
			 now);
			return result;
		}
		public async Task<bool> GetTitleIsExpired2Async(long uid, ITransportCardCheckExhaustedScript script, DateTime now)
		{
			var result = await GetTitleIsExpiredAsync(uid,
			 script.Card.Titulo.CodigoTitulo2.Value,
			 script.Card.Titulo.ValidezZonal2.Value,
			 script.Card.Titulo.ControlTarifa2.Value,
			 now);
			return result;
		}
		public async Task<bool> GetTitleIsExpired2Async(long uid, SigapuntScript script, DateTime now)
		{
			return await Task.Run(() => false);
		}
		public async Task<bool> GetTitleIsExpiredMAsync(long uid, SigapuntScript script, DateTime now)
		{
			return await Task.Run(() => false);
		}
		public async Task<bool> GetTitleIsExpiredMAsync(long uid, ITransportCardCheckExhaustedScript script, DateTime now)
		{
			return await Task.Run(() => false);
		}
		public async Task<bool> GetTitleIsExpiredBAsync(long uid, SigapuntScript script, DateTime now)
		{
			return await Task.Run(() => false);
		}
		public async Task<bool> GetTitleIsExpiredBAsync(long uid, ITransportCardCheckExhaustedScript script, DateTime now)
		{
			return await Task.Run(() => false);
		}
		private async Task<bool> GetTitleIsExpiredAsync(EigeTipoUnidadesValidezTemporalEnum? tipoUnidades, int? numeroUnidades, DateTime? fechaValidez, bool usoSaldo, int? saldo, bool enAmpliacion, DateTime now)
		{
			return await Task.Run(() => false);
		}
		private async Task<bool> GetTitleIsExpiredTuinAsync(bool saldoSign, decimal? saldo)
		{
			return await Task.Run(() => false);
		}
		#endregion GetTitleIsExhaustedAsync

		#region GetTitleMaxExternalTransfersAsync
		public async Task<int?> GetTitleMaxExternalTransfers1Async(long uid, ITransportCardGetMaxExternalTransfersScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[3].Blocks[0]))
			))
				return null;

			return await GetTitleMaxExternalTransfersAsync(script.Card.Titulo.MaxTransbordosExternos1.Value);
		}
		public async Task<int?> GetTitleMaxExternalTransfers2Async(long uid, ITransportCardCheckExhaustedScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[4].Blocks[0]))
			))
				return null;

			return await GetTitleMaxExternalTransfersAsync(script.Card.Titulo.MaxTransbordosExternos2.Value);
		}
		public async Task<int?> GetTitleMaxExternalTransfersMAsync(long uid, ITransportCardCheckExhaustedScript script)
		{
			return await Task.Run(() =>
			{
				return (int?)null;
			});
		}
		public async Task<int?> GetTitleMaxExternalTransfersBAsync(long uid, ITransportCardCheckExhaustedScript script)
		{
			return await Task.Run(() =>
			{
				return (int?)null;
			});
		}
		private async Task<int?> GetTitleMaxExternalTransfersAsync(int maxTransbordosExternos)
		{
			return await Task.Run(() =>
			{
				return (maxTransbordosExternos);
			});
		}
		#endregion GetTitleMaxExternalTransfersAsync

		#region GetTitleMaxExternalTransfersAsync
		public async Task<int?> GetTitleMaxPeopleInTransfer1Async(long uid, ITransportCardGetMaxPeopleInTransferScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[3].Blocks[0]))
			))
				return null;

			return await GetTitleMaxPeopleInTransferAsync(script.Card.Titulo.MaxPersonasEnTransbordo1.Value);
		}
		public async Task<int?> GetTitleMaxPeopleInTransfer2Async(long uid, ITransportCardGetMaxPeopleInTransferScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[4].Blocks[0]))
			))
				return null;

			return await GetTitleMaxPeopleInTransferAsync(script.Card.Titulo.MaxPersonasEnTransbordo2.Value);
		}
		public async Task<int?> GetTitleMaxPeopleInTransferMAsync(long uid, ITransportCardGetMaxPeopleInTransferScript script)
		{
			return await Task.Run(() =>
			{
				return (int?)null;
			});
		}
		public async Task<int?> GetTitleMaxPeopleInTransferBAsync(long uid, ITransportCardGetMaxPeopleInTransferScript script)
		{
			return await Task.Run(() =>
			{
				return (int?)null;
			});
		}
		private async Task<int?> GetTitleMaxPeopleInTransferAsync(int maxPersonasEnTransbordo)
		{
			return await Task.Run(() =>
			{
				return (maxPersonasEnTransbordo == 15 ?
					(int?)null :
						maxPersonasEnTransbordo);
			});
		}
		#endregion GetTitleMaxPeopleInTransferAsync

		#region GetTitleHasBalanceAsync
		public async Task<bool> GetTitleHasBalance1Async(long uid, ITransportCardGetHasBalanceScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[3].Blocks[0]))
			))
				return false;

			return IsTuiN(script.Card.Titulo.CodigoTitulo1.Value) ?
				await GetTitleHasBalanceTuinAsync() :
				await GetTitleHasBalanceAsync(script.Card.Titulo.UsoDelCampoSaldoViajes1.Value);
		}
		public async Task<bool> GetTitleHasBalance2Async(long uid, ITransportCardGetHasBalanceScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[4].Blocks[0]))
			))
				return false;

			return IsTuiN(script.Card.Titulo.CodigoTitulo2.Value) ?
				await GetTitleHasBalanceTuinAsync() :
				await GetTitleHasBalanceAsync(script.Card.Titulo.UsoDelCampoSaldoViajes2.Value);
		}
		public async Task<bool> GetTitleHasBalanceMAsync(long uid, ITransportCardGetHasBalanceScript script)
		{
			return await Task.Run(() => true);
		}
		public async Task<bool> GetTitleHasBalanceBAsync(long uid, ITransportCardGetHasBalanceScript script)
		{
			return await Task.Run(() => true);
		}
		private async Task<bool> GetTitleHasBalanceAsync(bool usoSaldo)
		{
			return await Task.Run(() =>
			{
				return usoSaldo;
			});
		}
		private async Task<bool> GetTitleHasBalanceTuinAsync()
		{
			return await Task.Run(() =>
			{
				return true;
			});
		}
		#endregion HasBalance1Async

		#region GetTitleBalanceAsync
		public async Task<decimal?> GetTitleBalance1Async(long uid, SigapuntScript script)
		{
			if (script.SaldoViaje1 == null)
				return null;

			return IsTuiN(script.CodigoTitulo1) ?
				await GetTitleBalanceAsync(script.SaldoViaje1) :
				await GetTitleBalanceTuinAsync(false, script.SaldoViaje1);
		}
		public async Task<decimal?> GetTitleBalance1Async(long uid, ITransportCardGetBalanceScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[1].Blocks[1])) &&
				(await script.Card.CheckAsync(uid, script.Card.Sectors[3].Blocks[0])) &&
				(await script.Card.CheckAsync(uid, script.Card.Sectors[9].Blocks[0]))
			))
				return null;

			return IsTuiN(script.Card.Titulo.CodigoTitulo1.Value) ?
				await GetTitleBalanceTuinAsync(script.Card.TituloTuiN.SaldoViaje_Sign.Value, script.Card.TituloTuiN.SaldoViaje_Value.Value) :
				await GetTitleBalanceAsync(script.Card.Titulo.SaldoViaje1.Value);
		}
		public async Task<decimal?> GetTitleBalance2Async(long uid, SigapuntScript script)
		{
			if (script.SaldoViaje2 == null)
				return null;

			return IsTuiN(script.CodigoTitulo2) ?
				await GetTitleBalanceAsync(script.SaldoViaje2) :
				await GetTitleBalanceTuinAsync(false, script.SaldoViaje2);
		}
		public async Task<decimal?> GetTitleBalance2Async(long uid, ITransportCardGetBalanceScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[4].Blocks[0])) &&
				(await script.Card.CheckAsync(uid, script.Card.Sectors[4].Blocks[1])) &&
				(await script.Card.CheckAsync(uid, script.Card.Sectors[9].Blocks[0]))
			))
				return null;

			return IsTuiN(script.Card.Titulo.CodigoTitulo2.Value) ?
				await GetTitleBalanceTuinAsync(script.Card.TituloTuiN.SaldoViaje_Sign.Value, script.Card.TituloTuiN.SaldoViaje_Value.Value) :
				await GetTitleBalanceAsync(script.Card.Titulo.SaldoViaje2.Value);
		}
		public async Task<decimal?> GetTitleBalanceMAsync(long uid, SigapuntScript script)
		{
			return await Task.Run(() =>
			{
				return script.SaldoMonedero;
			});
		}
		public async Task<decimal?> GetTitleBalanceMAsync(long uid, ITransportCardGetBalanceScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[6].Blocks[0]))
			))
				return null;

			return await Task.Run(() =>
			{
				return script.Card.Titulo.SaldoMonedero.Value;
			});
		}
		public async Task<decimal?> GetTitleBalanceBAsync(long uid, SigapuntScript script)
		{
			return await Task.Run(() =>
			{
				return script.SaldoBonus;
			});
		}
		public async Task<decimal?> GetTitleBalanceBAsync(long uid, ITransportCardGetBalanceScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[5].Blocks[0]))
			))
				return null;

			return await Task.Run(() =>
			{
				return script.Card.Titulo.SaldoBonus.Value;
			});
		}
		private async Task<decimal?> GetTitleBalanceAsync(decimal? saldo)
		{
			return await Task.Run(() =>
			{
				return saldo;
			});
		}
		private async Task<decimal?> GetTitleBalanceTuinAsync(bool saldoTuinSign, decimal? saldoTuinValue)
		{
			return await Task.Run(() =>
			{
				return saldoTuinSign == true ?
					-1 * saldoTuinValue :
					saldoTuinValue;
			});
		}
        #endregion HasBalanceAsync

        #region GetTitleBalanceInverseAsync
        public async Task<decimal?> GetTitleBalanceInverse1Async(long uid, SigapuntScript script)
        {
            var value = await GetTitleBalance1Async(uid, script);
            return
                (value ?? 0) == 0 ? 0 :
                Math.Round(1 / value.Value, 2);
        }
        public async Task<decimal?> GetTitleBalanceInverse1Async(long uid, ITransportCardGetBalanceScript script)
        {
            var value = await GetTitleBalance1Async(uid, script);
            return
                (value ?? 0) == 0 ? 0 :
                Math.Round(1 / value.Value, 2);
        }
        public async Task<decimal?> GetTitleBalanceInverse2Async(long uid, SigapuntScript script)
        {
            var value = await GetTitleBalance2Async(uid, script);
            return
                (value ?? 0) == 0 ? 0 :
                Math.Round(1 / value.Value, 2);
        }
        public async Task<decimal?> GetTitleBalanceInverse2Async(long uid, ITransportCardGetBalanceScript script)
        {
            var value = await GetTitleBalance2Async(uid, script);
            return
                (value ?? 0) == 0 ? 0 :
                Math.Round(1 / value.Value, 2);
        }
        public async Task<decimal?> GetTitleBalanceInverseMAsync(long uid, SigapuntScript script)
        {
            var value = await GetTitleBalanceMAsync(uid, script);
            return
                (value ?? 0) == 0 ? 0 :
                Math.Round(1 / value.Value, 2);
        }
        public async Task<decimal?> GetTitleBalanceInverseMAsync(long uid, ITransportCardGetBalanceScript script)
        {
            var value = await GetTitleBalanceMAsync(uid, script);
            return
                (value ?? 0) == 0 ? 0 :
                Math.Round(1 / value.Value, 2);
        }
        public async Task<decimal?> GetTitleBalanceInverseBAsync(long uid, SigapuntScript script)
        {
            var value = await GetTitleBalanceBAsync(uid, script);
            return
                (value ?? 0) == 0 ? 0 :
                Math.Round(1 / value.Value, 2);
        }
        public async Task<decimal?> GetTitleBalanceInverseBAsync(long uid, ITransportCardGetBalanceScript script)
        {
            var value = await GetTitleBalanceBAsync(uid, script);
            return
                (value ?? 0) == 0 ? 0 :
                Math.Round(1 / value.Value, 2);
        }
        #endregion HasBalanceInverseAsync

        #region GetTitleBalanceUnitsAsync
        public async Task<string> GetTitleBalanceUnits1Async(long uid, SigapuntScript script)
		{
			if (script.SaldoViaje1 == null)
				return null;

			return await GetTitleBalanceUnitsAsync(script.CodigoTitulo1);
		}
		public async Task<string> GetTitleBalanceUnits1Async(long uid, ITransportCardGetBalanceUnitsScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[3].Blocks[0]))
			))
				return null;

			return await GetTitleBalanceUnitsAsync(script.Card.Titulo.CodigoTitulo1.Value);
		}
		public async Task<string> GetTitleBalanceUnits2Async(long uid, SigapuntScript script)
		{
			if (script.SaldoViaje2 == null)
				return null;

			return await GetTitleBalanceUnitsAsync(script.CodigoTitulo2);
		}
		public async Task<string> GetTitleBalanceUnits2Async(long uid, ITransportCardGetBalanceUnitsScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[4].Blocks[0]))
			))
				return null;

			return await GetTitleBalanceUnitsAsync(script.Card.Titulo.CodigoTitulo2.Value);
		}
		public async Task<string> GetTitleBalanceUnitsMAsync(long uid, SigapuntScript script)
		{
			return await Task.Run(() =>
			{
				return "€";
			});
		}
		public async Task<string> GetTitleBalanceUnitsMAsync(long uid, ITransportCardGetBalanceUnitsScript script)
		{
			return await Task.Run(() =>
			{
				return "€";
			});
		}
		public async Task<string> GetTitleBalanceUnitsBAsync(long uid, SigapuntScript script)
		{
			return await Task.Run(() =>
			{
				return "p";
			});
		}
		public async Task<string> GetTitleBalanceUnitsBAsync(long uid, ITransportCardGetBalanceUnitsScript script)
		{
			return await Task.Run(() =>
			{
				return "p";
			});
		}
		private async Task<string> GetTitleBalanceUnitsAsync(int? code)
		{
			return await Task.Run(() =>
			{
				return IsTuiN(code) ?
					"€" :
					"v";
			});
		}
        #endregion GetTitleBalanceUnitsAsync

        #region GetTitleIsTemporalAsync
        public async Task<bool> GetTitleIsTemporal1Async(long uid, SigapuntScript script)
		{
			return
				(await GetTitleIsTemporalAsync(script.NumeroUnidadesValidezTemporal1));
		}
		public async Task<bool> GetTitleIsTemporal1Async(long uid, ITransportCardGetIsTemporalScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[3].Blocks[0]))
			))
				return false;

			return
				(await GetTitleIsTemporalAsync(script.Card.Titulo.NumeroUnidadesValidezTemporal1.Value));
		}
		public async Task<bool> GetTitleIsTemporal2Async(long uid, SigapuntScript script)
		{
			return
				(await GetTitleIsTemporalAsync(script.NumeroUnidadesValidezTemporal2));
		}
		public async Task<bool> GetTitleIsTemporal2Async(long uid, ITransportCardGetIsTemporalScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[4].Blocks[0]))
			))
				return false;

			return
				(await GetTitleIsTemporalAsync(script.Card.Titulo.NumeroUnidadesValidezTemporal2.Value));
		}
		public async Task<bool> GetTitleIsTemporalMAsync(long uid, SigapuntScript script)
		{
			return await Task.Run(() => false);
		}
		public async Task<bool> GetTitleIsTemporalMAsync(long uid, ITransportCardGetIsTemporalScript script)
		{
			return await Task.Run(() => false);
		}
		public async Task<bool> GetTitleIsTemporalBAsync(long uid, SigapuntScript script)
		{
			return await Task.Run(() => false);
		}
		public async Task<bool> GetTitleIsTemporalBAsync(long uid, ITransportCardGetIsTemporalScript script)
		{
			return await Task.Run(() => false);
		}
		private async Task<bool> GetTitleIsTemporalAsync(int? unidadesValidezTemporal)
		{
			return await Task.Run(() =>
			{
				return
					unidadesValidezTemporal != null &&
					unidadesValidezTemporal < 255;
			});
		}
		#endregion GetTitleIsTemporalAsync

		#region GetTitleExhaustedDateAsync
		public async Task<DateTime?> GetTitleExhaustedDate1Async(long uid, SigapuntScript script)
		{
			return
				(await GetTitleExhaustedDateAsync(
					null, // XAVI
					script.NumeroUnidadesValidezTemporal1,
					script.FechaValidez1,
					script.TituloEnAmpliacion1
				));
		}
		public async Task<DateTime?> GetTitleExhaustedDate1Async(long uid, ITransportCardGetExhaustedDateScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[3].Blocks[0])) &&
				(await script.Card.CheckAsync(uid, script.Card.Sectors[1].Blocks[1]))
			))
				return null;

			return
				(await GetTitleExhaustedDateAsync(
					script.Card.Titulo.TipoUnidadesValidezTemporal1.Value,
					script.Card.Titulo.NumeroUnidadesValidezTemporal1.Value,
					script.Card.Titulo.FechaValidez1.Value,
					script.Card.Titulo.TituloEnAmpliacion1.Value
				));
		}
		public async Task<DateTime?> GetTitleExhaustedDate2Async(long uid, SigapuntScript script)
		{
			return
				(await GetTitleExhaustedDateAsync(
					null, // XAVI
					script.NumeroUnidadesValidezTemporal2,
					script.FechaValidez2,
					script.TituloEnAmpliacion2
				));
		}
		public async Task<DateTime?> GetTitleExhaustedDate2Async(long uid, ITransportCardGetExhaustedDateScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[4].Blocks[0])) &&
				(await script.Card.CheckAsync(uid, script.Card.Sectors[4].Blocks[1]))
			))
				return null;

			return
				(await GetTitleExhaustedDateAsync(
					script.Card.Titulo.TipoUnidadesValidezTemporal2.Value,
					script.Card.Titulo.NumeroUnidadesValidezTemporal2.Value,
					script.Card.Titulo.FechaValidez2.Value,
					script.Card.Titulo.TituloEnAmpliacion2.Value
				));
		}
		public async Task<DateTime?> GetTitleExhaustedDateMAsync(long uid, SigapuntScript script)
		{
			return await Task.Run(() => (DateTime?)null);
		}
		public async Task<DateTime?> GetTitleExhaustedDateMAsync(long uid, ITransportCardGetExhaustedDateScript script)
		{
			return await Task.Run(() => (DateTime?)null);
		}
		public async Task<DateTime?> GetTitleExhaustedDateBAsync(long uid, SigapuntScript script)
		{
			return await Task.Run(() => (DateTime?)null);
		}
		public async Task<DateTime?> GetTitleExhaustedDateBAsync(long uid, ITransportCardGetExhaustedDateScript script)
		{
			return await Task.Run(() => (DateTime?)null);
		}
		private async Task<DateTime?> GetTitleExhaustedDateAsync(EigeTipoUnidadesValidezTemporalEnum? tipoUnidades, int? numeroUnidades, DateTime? fechaValidez, bool? bitAmpliacion)
		{
			return await Task.Run(() =>
			{
				return
					numeroUnidades == null || fechaValidez == null ? (DateTime?)null :
					numeroUnidades.Value == 0 ? fechaValidez.Value :
					numeroUnidades.Value == 255 ? (DateTime?)null :
					bitAmpliacion.Value ? fechaValidez.Value : // Cuando el bit de ampliación se activa, la fechaValidez pasa de ser la de activación y pasa a ser la de caducidad
					tipoUnidades == EigeTipoUnidadesValidezTemporalEnum.Anyos ? fechaValidez.Value.FloorYear().AddYears(numeroUnidades.Value + 1).AddSeconds(-1) :
					tipoUnidades == EigeTipoUnidadesValidezTemporalEnum.Meses ? fechaValidez.Value.FloorMonth().AddMonths(numeroUnidades.Value + 1).AddSeconds(-1) :
					tipoUnidades == EigeTipoUnidadesValidezTemporalEnum.Dias ? fechaValidez.Value.Date.AddDays(numeroUnidades.Value + 1).AddSeconds(-1) :
					tipoUnidades == EigeTipoUnidadesValidezTemporalEnum.Horas ? fechaValidez.Value.FloorHour().AddHours(numeroUnidades.Value + 1).AddSeconds(-1) :
						(DateTime?)null
				;
			});
		}
		#endregion GetTitleExhaustedDateAsync

		#region GetTitleUsedUpAsync

		/// <summary>
		/// Returns if title 1 is used up (agotado) Repetido(isExhausted)
		/// </summary>
		/// <param name="uid"></param>
		/// <param name="script"></param>
		/// <returns></returns>
		public async Task<bool> GetTitleUsedUp1Async(long uid, ITransportCardGetExhaustedDateScript script)
		{
			var isTemporal = await GetTitleIsTemporalAsync(script.Card.Titulo.NumeroUnidadesValidezTemporal1.Value);

			return
				(await GetTitleUsedUpAsync(
					script.Card.Titulo.SaldoViaje1.Value,
					isTemporal,
					script.Card.Titulo.TituloEnAmpliacion1.Value,
					script.Card.Titulo.FechaValidez1.Value,
					script.Card.Titulo.NumeroUnidadesValidezTemporal1.Value,
					script.Card.Titulo.TipoUnidadesValidezTemporal1.Value

				));
		}
		/// <summary>
		/// Returns if title 2 is used up (agotado)
		/// </summary>
		/// <param name="uid"></param>
		/// <param name="script"></param>
		/// <returns></returns>
		public async Task<bool> GetTitleUsedUp2Async(long uid, ITransportCardGetExhaustedDateScript script)
		{
			var isTemporal = await GetTitleIsTemporalAsync(script.Card.Titulo.NumeroUnidadesValidezTemporal2.Value);

			return
				(await GetTitleUsedUpAsync(
					script.Card.Titulo.SaldoViaje2.Value,
					isTemporal,
					script.Card.Titulo.TituloEnAmpliacion2.Value,
					script.Card.Titulo.FechaValidez2.Value,
					script.Card.Titulo.NumeroUnidadesValidezTemporal2.Value,
					script.Card.Titulo.TipoUnidadesValidezTemporal2.Value
				));
		}
		private async Task<bool> GetTitleUsedUpAsync(int? cantidadViajes, bool isTemporal, bool? bitExtended, DateTime? validationDate, int? validityNumber, EigeTipoUnidadesValidezTemporalEnum? validityType
			)
		{
			var now = DateTime.Now;
			var expirationDate = await GetTitleExhaustedDateAsync(validityType, validityNumber, validationDate, bitExtended);
			return await Task.Run(() =>
			{
				return
					//Bono
					!isTemporal && cantidadViajes != null && cantidadViajes == 0 ? true :
					//Abono
					isTemporal && expirationDate == null ? false :
					isTemporal && bitExtended != true && (validationDate != null && expirationDate < now) ? true : //FechaValidez = Validación
					isTemporal && bitExtended == true && (validationDate != null && expirationDate < now) ? true : //FechaValidez = Fecha Caducidad
					false;
			});
		}

		#endregion GetTitleUsedUpAsync

		#region GetTitleActivatedDateAsync
		public async Task<DateTime?> GetTitleActivatedDate1Async(long uid, SigapuntScript script)
		{
			return
				(await GetTitleActivatedDateAsync(
					script.FechaValidez1
				));
		}
		public async Task<DateTime?> GetTitleActivatedDate1Async(long uid, ITransportCardGetActivatedDateScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[1].Blocks[1]))
			))
				return null;
			return
				(await GetTitleActivatedDateAsync(
					script.Card.Titulo.FechaValidez1.Value
				));
		}
		public async Task<DateTime?> GetTitleActivatedDate2Async(long uid, SigapuntScript script)
		{
			return
				(await GetTitleActivatedDateAsync(
					script.FechaValidez2
				));
		}
		public async Task<DateTime?> GetTitleActivatedDate2Async(long uid, ITransportCardGetActivatedDateScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[4].Blocks[1]))
			))
				return null;
			return
				(await GetTitleActivatedDateAsync(
					script.Card.Titulo.FechaValidez2.Value
				));
		}
		public async Task<DateTime?> GetTitleActivatedDateMAsync(long uid, SigapuntScript script)
		{
			return await Task.Run(() => (DateTime?)null);
		}
		public async Task<DateTime?> GetTitleActivatedDateMAsync(long uid, ITransportCardGetActivatedDateScript script)
		{
			return await Task.Run(() => (DateTime?)null);
		}
		public async Task<DateTime?> GetTitleActivatedDateBAsync(long uid, SigapuntScript script)
		{
			return await Task.Run(() => (DateTime?)null);
		}
		public async Task<DateTime?> GetTitleActivatedDateBAsync(long uid, ITransportCardGetActivatedDateScript script)
		{
			return await Task.Run(() => (DateTime?)null);
		}
		private async Task<DateTime?> GetTitleActivatedDateAsync(DateTime? fechaValidez)
		{
			return await Task.Run(() => fechaValidez);
		}
		#endregion GetTitleActivatedDateAsync

		#region GetTitleAmpliationAsync
		public async Task<int?> GetTitleAmpliation1Async(long uid, SigapuntScript script)
		{
			if ((script.TituloEnAmpliacion1 == null) || (script.FechaValidez1 == null))
				return null;

			return await GetTitleAmpliationAsync(script.TituloEnAmpliacion1.Value, script.FechaValidez1.Value);
		}
		public async Task<int?> GetTitleAmpliation1Async(long uid, ITransportCardGetAmpliationScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[1].Blocks[1]))
			))
				return null;

			return await GetTitleAmpliationAsync(script.Card.Titulo.TituloEnAmpliacion1.Value, script.Card.Titulo.FechaValidez1.Value);
		}
		public async Task<int?> GetTitleAmpliation2Async(long uid, SigapuntScript script)
		{
			if ((script.TituloEnAmpliacion2 == null) || (script.FechaValidez2 == null))
				return null;

			return await GetTitleAmpliationAsync(script.TituloEnAmpliacion2.Value, script.FechaValidez2.Value);
		}
		public async Task<int?> GetTitleAmpliation2Async(long uid, ITransportCardGetAmpliationScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[4].Blocks[0])) &&
				(await script.Card.CheckAsync(uid, script.Card.Sectors[4].Blocks[1]))
			))
				return null;

			return await GetTitleAmpliationAsync(script.Card.Titulo.TituloEnAmpliacion2.Value, script.Card.Titulo.FechaValidez2.Value);
		}
		public async Task<int?> GetTitleAmpliationMAsync(long uid, SigapuntScript script)
		{
			return await Task.Run(() => (int?)null);
		}
		public async Task<int?> GetTitleAmpliationMAsync(long uid, ITransportCardGetAmpliationScript script)
		{
			return await Task.Run(() => (int?)null);
		}
		public async Task<int?> GetTitleAmpliationBAsync(long uid, SigapuntScript script)
		{
			return await Task.Run(() => (int?)null);
		}
		public async Task<int?> GetTitleAmpliationBAsync(long uid, ITransportCardGetAmpliationScript script)
		{
			return await Task.Run(() => (int?)null);
		}
		private async Task<int?> GetTitleAmpliationAsync(bool enAmpliation, DateTime? inicio)
		{
			return await Task.Run(() =>
			{
				return
					((inicio == null) ? 1 : 0) +
					(enAmpliation ? 1 : 0)
					;
			});
		}
		#endregion GetTitleAmpliationAsync

		#region GetTitleAmpliationQuantity
		public async Task<int?> GetTitleAmpliationQuantity1Async(long uid, SigapuntScript script)
		{
			if (script.NumeroUnidadesValidezTemporal1 == null)
				return null;

			return await GetTitleAmpliationQuantityAsync(script.NumeroUnidadesValidezTemporal1.Value);
		}
		public async Task<int?> GetTitleAmpliationQuantity1Async(long uid, ITransportCardGetAmpliationQuantityScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[3].Blocks[0]))
			))
				return null;

			return await GetTitleAmpliationQuantityAsync(script.Card.Titulo.NumeroUnidadesValidezTemporal1.Value);
		}
		public async Task<int?> GetTitleAmpliationQuantity2Async(long uid, SigapuntScript script)
		{
			if (script.NumeroUnidadesValidezTemporal2 == null)
				return null;

			return await GetTitleAmpliationQuantityAsync(script.NumeroUnidadesValidezTemporal2.Value);
		}
		public async Task<int?> GetTitleAmpliationQuantity2Async(long uid, ITransportCardGetAmpliationQuantityScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[4].Blocks[0]))
			))
				return null;

			return await GetTitleAmpliationQuantityAsync(script.Card.Titulo.NumeroUnidadesValidezTemporal2.Value);
		}
		public async Task<int?> GetTitleAmpliationQuantityMAsync(long uid, SigapuntScript script)
		{
			return await Task.Run(() => (int?)null);
		}
		public async Task<int?> GetTitleAmpliationQuantityMAsync(long uid, ITransportCardGetAmpliationQuantityScript script)
		{
			return await Task.Run(() => (int?)null);
		}
		public async Task<int?> GetTitleAmpliationQuantityBAsync(long uid, SigapuntScript script)
		{
			return await Task.Run(() => (int?)null);
		}
		public async Task<int?> GetTitleAmpliationQuantityBAsync(long uid, ITransportCardGetAmpliationQuantityScript script)
		{
			return await Task.Run(() => (int?)null);
		}
		private async Task<int?> GetTitleAmpliationQuantityAsync(int value)
		{
			return await Task.Run(() =>
			{
				return value;
			});
		}
		#endregion GetTitleAmpliationQuantity

		#region GetTitleAmpliationUnitsAsync
		public async Task<string> GetTitleAmpliationUnits1Async(long uid, SigapuntScript script)
		{
			return await GetTitleAmpliationUnitsAsync(null); // XAVI
		}
		public async Task<string> GetTitleAmpliationUnits1Async(long uid, ITransportCardGetAmpliationUnitsScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[3].Blocks[0]))
			))
				return null;

			return await GetTitleAmpliationUnitsAsync(script.Card.Titulo.TipoUnidadesValidezTemporal1.Value);
		}
		public async Task<string> GetTitleAmpliationUnits2Async(long uid, SigapuntScript script)
		{
			return await GetTitleAmpliationUnitsAsync(null); // XAVI
		}
		public async Task<string> GetTitleAmpliationUnits2Async(long uid, ITransportCardGetAmpliationUnitsScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[4].Blocks[0]))
			))
				return null;

			return await GetTitleAmpliationUnitsAsync(script.Card.Titulo.TipoUnidadesValidezTemporal2.Value);
		}
		public async Task<string> GetTitleAmpliationUnitsMAsync(long uid, SigapuntScript script)
		{
			return await Task.Run(() => "");
		}
		public async Task<string> GetTitleAmpliationUnitsMAsync(long uid, ITransportCardGetAmpliationUnitsScript script)
		{
			return await Task.Run(() => "");
		}
		public async Task<string> GetTitleAmpliationUnitsBAsync(long uid, SigapuntScript script)
		{
			return await Task.Run(() => "");
		}
		public async Task<string> GetTitleAmpliationUnitsBAsync(long uid, ITransportCardGetAmpliationUnitsScript script)
		{
			return await Task.Run(() => "");
		}
		private async Task<string> GetTitleAmpliationUnitsAsync(EigeTipoUnidadesValidezTemporalEnum? value)
		{
			return await Task.Run(() =>
			{
				return
					value == EigeTipoUnidadesValidezTemporalEnum.Anyos ? "a" :
					value == EigeTipoUnidadesValidezTemporalEnum.Meses ? "m" :
					value == EigeTipoUnidadesValidezTemporalEnum.Dias ? "d" :
					value == EigeTipoUnidadesValidezTemporalEnum.Horas ? "h" :
					"";
			});
		}
		#endregion GetTitleAmpliationUnitsAsync

		#region GetHistoricoDateAsync
		public async Task<DateTime?> GetHistoricoDate1Async(long uid, ITransportCardGetValidationDateScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[4].Blocks[2]))
			))
				return null;

			return await GetValidationDateAsync(script.Card.Historico.FechaHora1.Value);
		}
		public async Task<DateTime?> GetHistoricoDate2Async(long uid, ITransportCardGetValidationDateScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[7].Blocks[0]))
			))
				return null;

			return await GetValidationDateAsync(script.Card.Historico.FechaHora2.Value);
		}
		public async Task<DateTime?> GetHistoricoDate3Async(long uid, ITransportCardGetValidationDateScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[7].Blocks[1]))
			))
				return null;

			return await GetValidationDateAsync(script.Card.Historico.FechaHora3.Value);
		}
		public async Task<DateTime?> GetHistoricoDate4Async(long uid, ITransportCardGetValidationDateScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[7].Blocks[2]))
			))
				return null;

			return await GetValidationDateAsync(script.Card.Historico.FechaHora4.Value);
		}
		public async Task<DateTime?> GetHistoricoDate5Async(long uid, ITransportCardGetValidationDateScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[5].Blocks[2]))
			))
				return null;

			return await GetValidationDateAsync(script.Card.Historico.FechaHora5.Value);
		}
		public async Task<DateTime?> GetHistoricoDate6Async(long uid, ITransportCardGetValidationDateScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[11].Blocks[0]))
			))
				return null;

			return await GetValidationDateAsync(script.Card.Historico.FechaHora6.Value);
		}
		public async Task<DateTime?> GetHistoricoDate7Async(long uid, ITransportCardGetValidationDateScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[11].Blocks[1]))
			))
				return null;

			return await GetValidationDateAsync(script.Card.Historico.FechaHora7.Value);
		}
		public async Task<DateTime?> GetHistoricoDate8Async(long uid, ITransportCardGetValidationDateScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[11].Blocks[2]))
			))
				return null;

			return await GetValidationDateAsync(script.Card.Historico.FechaHora8.Value);
		}
		#endregion GetHistoricoDateAsync

		#region GetHistoricoTypeNameAsync
		public async Task<string> GetHistoricoTypeName1Async(long uid, ITransportCardGetValidationTypeNameScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[4].Blocks[2]))
			))
				return null;

			return await GetValidationTypeNameAsync(
				script.Card.Historico.TipoValidacion1_Transbordo.Value,
				script.Card.Historico.TipoValidacion1_Sentido.Value
			);
		}
		public async Task<string> GetHistoricoTypeName2Async(long uid, ITransportCardGetValidationTypeNameScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[7].Blocks[0]))
			))
				return null;

			return await GetValidationTypeNameAsync(
				script.Card.Historico.TipoValidacion2_Transbordo.Value,
				script.Card.Historico.TipoValidacion2_Sentido.Value
			);
		}
		public async Task<string> GetHistoricoTypeName3Async(long uid, ITransportCardGetValidationTypeNameScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[7].Blocks[1]))
			))
				return null;

			return await GetValidationTypeNameAsync(
				script.Card.Historico.TipoValidacion3_Transbordo.Value,
				script.Card.Historico.TipoValidacion3_Sentido.Value
			);
		}
		public async Task<string> GetHistoricoTypeName4Async(long uid, ITransportCardGetValidationTypeNameScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[7].Blocks[2]))
			))
				return null;

			return await GetValidationTypeNameAsync(
				script.Card.Historico.TipoValidacion4_Transbordo.Value,
				script.Card.Historico.TipoValidacion4_Sentido.Value
			);
		}
		public async Task<string> GetHistoricoTypeName5Async(long uid, ITransportCardGetValidationTypeNameScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[5].Blocks[2]))
			))
				return null;

			return await GetValidationTypeNameAsync(
				script.Card.Historico.TipoValidacion5_Transbordo.Value,
				script.Card.Historico.TipoValidacion5_Sentido.Value
			);
		}
		public async Task<string> GetHistoricoTypeName6Async(long uid, ITransportCardGetValidationTypeNameScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[11].Blocks[0]))
			))
				return null;

			return await GetValidationTypeNameAsync(
				script.Card.Historico.TipoValidacion6_Transbordo.Value,
				script.Card.Historico.TipoValidacion6_Sentido.Value
			);
		}
		public async Task<string> GetHistoricoTypeName7Async(long uid, ITransportCardGetValidationTypeNameScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[11].Blocks[1]))
			))
				return null;

			return await GetValidationTypeNameAsync(
				script.Card.Historico.TipoValidacion7_Transbordo.Value,
				script.Card.Historico.TipoValidacion7_Sentido.Value
			);
		}
		public async Task<string> GetHistoricoTypeName8Async(long uid, ITransportCardGetValidationTypeNameScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[11].Blocks[2]))
			))
				return null;

			return await GetValidationTypeNameAsync(
				script.Card.Historico.TipoValidacion8_Transbordo.Value,
				script.Card.Historico.TipoValidacion8_Sentido.Value
			);
		}
		#endregion GetHistoricoTypeNameAsync

		#region GetHistoricoCodeAsync
		public async Task<int?> GetHistoricoCode1Async(long uid, ITransportCardGetHistoricoCodeScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[4].Blocks[2]))
			))
				return null;

			return await GetHistoricoCodeAsync(script.Card.Historico.CodigoTitulo1.Value);
		}
		public async Task<int?> GetHistoricoCode2Async(long uid, ITransportCardGetHistoricoCodeScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[7].Blocks[0]))
			))
				return null;

			return await GetHistoricoCodeAsync(script.Card.Historico.CodigoTitulo2.Value);
		}
		public async Task<int?> GetHistoricoCode3Async(long uid, ITransportCardGetHistoricoCodeScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[7].Blocks[1]))
			))
				return null;

			return await GetHistoricoCodeAsync(script.Card.Historico.CodigoTitulo3.Value);
		}
		public async Task<int?> GetHistoricoCode4Async(long uid, ITransportCardGetHistoricoCodeScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[7].Blocks[2]))
			))
				return null;

			return await GetHistoricoCodeAsync(script.Card.Historico.CodigoTitulo4.Value);
		}
		public async Task<int?> GetHistoricoCode5Async(long uid, ITransportCardGetHistoricoCodeScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[5].Blocks[2]))
			))
				return null;

			return await GetHistoricoCodeAsync(script.Card.Historico.CodigoTitulo5.Value);
		}
		public async Task<int?> GetHistoricoCode6Async(long uid, ITransportCardGetHistoricoCodeScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[11].Blocks[0]))
			))
				return null;

			return await GetHistoricoCodeAsync(script.Card.Historico.CodigoTitulo6.Value);
		}
		public async Task<int?> GetHistoricoCode7Async(long uid, ITransportCardGetHistoricoCodeScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[11].Blocks[1]))
			))
				return null;

			return await GetHistoricoCodeAsync(script.Card.Historico.CodigoTitulo7.Value);
		}
		public async Task<int?> GetHistoricoCode8Async(long uid, ITransportCardGetHistoricoCodeScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[11].Blocks[2]))
			))
				return null;

			return await GetHistoricoCodeAsync(script.Card.Historico.CodigoTitulo8.Value);
		}
		private async Task<int?> GetHistoricoCodeAsync(int codigo)
		{
			return await Task.Run(() => codigo);
		}
		#endregion GetHistoricoCodeAsync

		#region GetHistoricoIndiceAsync
		public async Task<int?> GetHistoricoIndiceAsync(long uid, ITransportCardGetHistoricoIndiceScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[1].Blocks[1])) &&
				(await script.Card.CheckAsync(uid, script.Card.Sectors[3].Blocks[0])) &&
				(await script.Card.CheckAsync(uid, script.Card.Sectors[4].Blocks[0])) &&
				(await script.Card.CheckAsync(uid, script.Card.Sectors[9].Blocks[2]))
			))
				return null;

			return (IsTuiN(script.Card.Titulo.CodigoTitulo1.Value) || IsTuiN(script.Card.Titulo.CodigoTitulo2.Value)) ?
				script.Card.TituloTuiN.IndiceTransaccion.Value :
				script.Card.Historico.IndiceTransaccion.Value;
		}
		#endregion GetHistoricoIndiceAsync

		#region GetHistoricoTiene8Async
		public async Task<bool?> GetHistoricoTiene8Async(long uid, ITransportCardGetHistoricoTiene8Script script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[1].Blocks[0])) &&
				(await script.Card.CheckAsync(uid, script.Card.Sectors[3].Blocks[0])) &&
				(await script.Card.CheckAsync(uid, script.Card.Sectors[4].Blocks[0])) &&
				(await script.Card.CheckAsync(uid, script.Card.Sectors[9].Blocks[2]))
			))
				return null;

			return (IsTuiN(script.Card.Titulo.CodigoTitulo1.Value) || IsTuiN(script.Card.Titulo.CodigoTitulo2.Value)) ?
				script.Card.TituloTuiN.Tiene8Historicos.Value :
				script.Card.Usuario.Tiene8Historicos.Value;
		}
		#endregion GetHistoricoTiene8Async

		#region GetHistoricoNameAsync
		public async Task<string> GetHistoricoName1Async(long uid, ITransportCardGetTitleNameScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[4].Blocks[2]))
			))
				return "";

			return await GetTitleNameAsync(script.Card.Historico.CodigoTitulo1.Value);
		}
		public async Task<string> GetHistoricoName2Async(long uid, ITransportCardGetTitleNameScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[7].Blocks[0]))
			))
				return "";

			return await GetTitleNameAsync(script.Card.Historico.CodigoTitulo2.Value);
		}
		public async Task<string> GetHistoricoName3Async(long uid, ITransportCardGetTitleNameScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[7].Blocks[1]))
			))
				return "";

			return await GetTitleNameAsync(script.Card.Historico.CodigoTitulo3.Value);
		}
		public async Task<string> GetHistoricoName4Async(long uid, ITransportCardGetTitleNameScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[7].Blocks[2]))
			))
				return "";

			return await GetTitleNameAsync(script.Card.Historico.CodigoTitulo4.Value);
		}
		public async Task<string> GetHistoricoName5Async(long uid, ITransportCardGetTitleNameScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[5].Blocks[2]))
			))
				return "";

			return await GetTitleNameAsync(script.Card.Historico.CodigoTitulo5.Value);
		}
		public async Task<string> GetHistoricoName6Async(long uid, ITransportCardGetTitleNameScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[11].Blocks[0]))
			))
				return "";

			return await GetTitleNameAsync(script.Card.Historico.CodigoTitulo6.Value);
		}
		public async Task<string> GetHistoricoName7Async(long uid, ITransportCardGetTitleNameScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[11].Blocks[1]))
			))
				return "";

			return await GetTitleNameAsync(script.Card.Historico.CodigoTitulo7.Value);
		}
		public async Task<string> GetHistoricoName8Async(long uid, ITransportCardGetTitleNameScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[11].Blocks[2]))
			))
				return "";

			return await GetTitleNameAsync(script.Card.Historico.CodigoTitulo8.Value);
		}
		#endregion GetHistoricoNameAsync

		#region GetHistoricoZoneAsync
		public async Task<EigeZonaHistoricoEnum?> GetHistoricoZone1Async(long uid, ITransportCardGetHistoricoCodeScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[4].Blocks[2]))
			))
				return null;

			return await GetHistoricoZoneAsync(script.Card.Historico.Zona1.Value);
		}
		public async Task<EigeZonaHistoricoEnum?> GetHistoricoZone2Async(long uid, ITransportCardGetHistoricoCodeScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[7].Blocks[0]))
			))
				return null;

			return await GetHistoricoZoneAsync(script.Card.Historico.Zona2.Value);
		}
		public async Task<EigeZonaHistoricoEnum?> GetHistoricoZone3Async(long uid, ITransportCardGetHistoricoCodeScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[7].Blocks[1]))
			))
				return null;

			return await GetHistoricoZoneAsync(script.Card.Historico.Zona3.Value);
		}
		public async Task<EigeZonaHistoricoEnum?> GetHistoricoZone4Async(long uid, ITransportCardGetHistoricoCodeScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[7].Blocks[2]))
			))
				return null;

			return await GetHistoricoZoneAsync(script.Card.Historico.Zona4.Value);
		}
		public async Task<EigeZonaHistoricoEnum?> GetHistoricoZone5Async(long uid, ITransportCardGetHistoricoCodeScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[5].Blocks[2]))
			))
				return null;

			return await GetHistoricoZoneAsync(script.Card.Historico.Zona5.Value);
		}
		public async Task<EigeZonaHistoricoEnum?> GetHistoricoZone6Async(long uid, ITransportCardGetHistoricoCodeScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[11].Blocks[0]))
			))
				return null;

			return await GetHistoricoZoneAsync(script.Card.Historico.Zona6.Value);
		}
		public async Task<EigeZonaHistoricoEnum?> GetHistoricoZone7Async(long uid, ITransportCardGetHistoricoCodeScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[11].Blocks[1]))
			))
				return null;

			return await GetHistoricoZoneAsync(script.Card.Historico.Zona7.Value);
		}
		public async Task<EigeZonaHistoricoEnum?> GetHistoricoZone8Async(long uid, ITransportCardGetHistoricoCodeScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[11].Blocks[2]))
			))
				return null;

			return await GetHistoricoZoneAsync(script.Card.Historico.Zona8.Value);
		}
		private async Task<EigeZonaHistoricoEnum?> GetHistoricoZoneAsync(EigeZonaHistoricoEnum zona)
		{
			return await Task.Run(() => zona);
		}
		#endregion GetHistoricoZoneAsync

		#region GetHistoricoQuantityAsync
		public async Task<decimal?> GetHistoricoQuantity1Async(long uid, ITransportCardGetHistoricoQuantityScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[4].Blocks[2]))
			))
				return null;

			return await GetHistoricoQuantityAsync(script.Card.Historico.CodigoTitulo1.Value, script.Card.Historico.UnidadesConsumidas1.Value);
		}
		public async Task<decimal?> GetHistoricoQuantity2Async(long uid, ITransportCardGetHistoricoQuantityScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[7].Blocks[0]))
			))
				return null;

			return await GetHistoricoQuantityAsync(script.Card.Historico.CodigoTitulo2.Value, script.Card.Historico.UnidadesConsumidas2.Value);
		}
		public async Task<decimal?> GetHistoricoQuantity3Async(long uid, ITransportCardGetHistoricoQuantityScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[7].Blocks[1]))
			))
				return null;

			return await GetHistoricoQuantityAsync(script.Card.Historico.CodigoTitulo3.Value, script.Card.Historico.UnidadesConsumidas3.Value);
		}
		public async Task<decimal?> GetHistoricoQuantity4Async(long uid, ITransportCardGetHistoricoQuantityScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[7].Blocks[2]))
			))
				return null;

			return await GetHistoricoQuantityAsync(script.Card.Historico.CodigoTitulo4.Value, script.Card.Historico.UnidadesConsumidas4.Value);
		}
		public async Task<decimal?> GetHistoricoQuantity5Async(long uid, ITransportCardGetHistoricoQuantityScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[5].Blocks[2]))
			))
				return null;

			return await GetHistoricoQuantityAsync(script.Card.Historico.CodigoTitulo5.Value, script.Card.Historico.UnidadesConsumidas5.Value);
		}
		public async Task<decimal?> GetHistoricoQuantity6Async(long uid, ITransportCardGetHistoricoQuantityScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[11].Blocks[0]))
			))
				return null;

			return await GetHistoricoQuantityAsync(script.Card.Historico.CodigoTitulo6.Value, script.Card.Historico.UnidadesConsumidas6.Value);
		}
		public async Task<decimal?> GetHistoricoQuantity7Async(long uid, ITransportCardGetHistoricoQuantityScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[11].Blocks[1]))
			))
				return null;

			return await GetHistoricoQuantityAsync(script.Card.Historico.CodigoTitulo7.Value, script.Card.Historico.UnidadesConsumidas7.Value);
		}
		public async Task<decimal?> GetHistoricoQuantity8Async(long uid, ITransportCardGetHistoricoQuantityScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[11].Blocks[2]))
			))
				return null;

			return await GetHistoricoQuantityAsync(script.Card.Historico.CodigoTitulo8.Value, script.Card.Historico.UnidadesConsumidas8.Value);
		}
		private async Task<decimal?> GetHistoricoQuantityAsync(int code, int quantity)
		{
			return await Task.Run(() =>
			{
				if (IsTuiN(code))
					return quantity / 100m;
				else
					return quantity;
			});
		}
		#endregion GetHistoricoQuantityAsync

		#region GetHistoricoQuantityUnitsAsync
		public async Task<string> GetHistoricoQuantityUnits1Async(long uid, ITransportCardGetHistoricoQuantityScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[4].Blocks[2]))
			))
				return null;

			return await GetHistoricoQuantityUnitsAsync(script.Card.Historico.CodigoTitulo1.Value);
		}
		public async Task<string> GetHistoricoQuantityUnits2Async(long uid, ITransportCardGetHistoricoQuantityScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[7].Blocks[0]))
			))
				return null;

			return await GetHistoricoQuantityUnitsAsync(script.Card.Historico.CodigoTitulo2.Value);
		}
		public async Task<string> GetHistoricoQuantityUnits3Async(long uid, ITransportCardGetHistoricoQuantityScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[7].Blocks[1]))
			))
				return null;

			return await GetHistoricoQuantityUnitsAsync(script.Card.Historico.CodigoTitulo3.Value);
		}
		public async Task<string> GetHistoricoQuantityUnits4Async(long uid, ITransportCardGetHistoricoQuantityScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[7].Blocks[2]))
			))
				return null;

			return await GetHistoricoQuantityUnitsAsync(script.Card.Historico.CodigoTitulo4.Value);
		}
		public async Task<string> GetHistoricoQuantityUnits5Async(long uid, ITransportCardGetHistoricoQuantityScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[5].Blocks[2]))
			))
				return null;

			return await GetHistoricoQuantityUnitsAsync(script.Card.Historico.CodigoTitulo5.Value);
		}
		public async Task<string> GetHistoricoQuantityUnits6Async(long uid, ITransportCardGetHistoricoQuantityScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[11].Blocks[0]))
			))
				return null;

			return await GetHistoricoQuantityUnitsAsync(script.Card.Historico.CodigoTitulo6.Value);
		}
		public async Task<string> GetHistoricoQuantityUnits7Async(long uid, ITransportCardGetHistoricoQuantityScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[11].Blocks[1]))
			))
				return null;

			return await GetHistoricoQuantityUnitsAsync(script.Card.Historico.CodigoTitulo7.Value);
		}
		public async Task<string> GetHistoricoQuantityUnits8Async(long uid, ITransportCardGetHistoricoQuantityScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[11].Blocks[2]))
			))
				return null;

			return await GetHistoricoQuantityUnitsAsync(script.Card.Historico.CodigoTitulo8.Value);
		}
		private async Task<string> GetHistoricoQuantityUnitsAsync(int code)
		{
			return await Task.Run(() =>
			{
				if (IsTuiN(code))
					return "€";
				else
					return "v";
			});
		}
		#endregion GetHistoricoQuantityAsync

		#region GetHistoricoPlaceAsync
		public async Task<string> GetHistoricoPlace1Async(long uid, ITransportCardGetValidationPlaceScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[4].Blocks[2]))
			))
				return null;

			return await GetValidationPlaceAsync(
				script.Card.Historico.TipoValidacion1_Transporte.Value,
				script.Card.Historico.VLEPS1_Ferrocarril_Estacion.Value,
				script.Card.Historico.VLEPS1_Ferrocarril_Vestibulo.Value,
				script.Card.Historico.VLEPS1_Bus_Linea.Value,
				script.Card.Historico.VLEPS1_Bus_Convoy.Value
			);
		}
		public async Task<string> GetHistoricoPlace2Async(long uid, ITransportCardGetValidationPlaceScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[7].Blocks[0]))
			))
				return null;

			return await GetValidationPlaceAsync(
				script.Card.Historico.TipoValidacion2_Transporte.Value,
				script.Card.Historico.VLEPS2_Ferrocarril_Estacion.Value,
				script.Card.Historico.VLEPS2_Ferrocarril_Vestibulo.Value,
				script.Card.Historico.VLEPS2_Bus_Linea.Value,
				script.Card.Historico.VLEPS2_Bus_Convoy.Value
			);
		}
		public async Task<string> GetHistoricoPlace3Async(long uid, ITransportCardGetValidationPlaceScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[7].Blocks[1]))
			))
				return null;

			return await GetValidationPlaceAsync(
				script.Card.Historico.TipoValidacion3_Transporte.Value,
				script.Card.Historico.VLEPS3_Ferrocarril_Estacion.Value,
				script.Card.Historico.VLEPS3_Ferrocarril_Vestibulo.Value,
				script.Card.Historico.VLEPS3_Bus_Linea.Value,
				script.Card.Historico.VLEPS3_Bus_Convoy.Value
			);
		}
		public async Task<string> GetHistoricoPlace4Async(long uid, ITransportCardGetValidationPlaceScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[7].Blocks[2]))
			))
				return null;

			return await GetValidationPlaceAsync(
				script.Card.Historico.TipoValidacion4_Transporte.Value,
				script.Card.Historico.VLEPS4_Ferrocarril_Estacion.Value,
				script.Card.Historico.VLEPS4_Ferrocarril_Vestibulo.Value,
				script.Card.Historico.VLEPS4_Bus_Linea.Value,
				script.Card.Historico.VLEPS4_Bus_Convoy.Value
			);
		}
		public async Task<string> GetHistoricoPlace5Async(long uid, ITransportCardGetValidationPlaceScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[5].Blocks[2]))
			))
				return null;

			return await GetValidationPlaceAsync(
				script.Card.Historico.TipoValidacion5_Transporte.Value,
				script.Card.Historico.VLEPS5_Ferrocarril_Estacion.Value,
				script.Card.Historico.VLEPS5_Ferrocarril_Vestibulo.Value,
				script.Card.Historico.VLEPS5_Bus_Linea.Value,
				script.Card.Historico.VLEPS5_Bus_Convoy.Value
			);
		}
		public async Task<string> GetHistoricoPlace6Async(long uid, ITransportCardGetValidationPlaceScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[11].Blocks[0]))
			))
				return null;

			return await GetValidationPlaceAsync(
				script.Card.Historico.TipoValidacion6_Transporte.Value,
				script.Card.Historico.VLEPS6_Ferrocarril_Estacion.Value,
				script.Card.Historico.VLEPS6_Ferrocarril_Vestibulo.Value,
				script.Card.Historico.VLEPS6_Bus_Linea.Value,
				script.Card.Historico.VLEPS6_Bus_Convoy.Value
			);
		}
		public async Task<string> GetHistoricoPlace7Async(long uid, ITransportCardGetValidationPlaceScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[11].Blocks[1]))
			))
				return null;

			return await GetValidationPlaceAsync(
				script.Card.Historico.TipoValidacion7_Transporte.Value,
				script.Card.Historico.VLEPS7_Ferrocarril_Estacion.Value,
				script.Card.Historico.VLEPS7_Ferrocarril_Vestibulo.Value,
				script.Card.Historico.VLEPS7_Bus_Linea.Value,
				script.Card.Historico.VLEPS7_Bus_Convoy.Value
			);
		}
		public async Task<string> GetHistoricoPlace8Async(long uid, ITransportCardGetValidationPlaceScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[11].Blocks[2]))
			))
				return null;

			return await GetValidationPlaceAsync(
				script.Card.Historico.TipoValidacion8_Transporte.Value,
				script.Card.Historico.VLEPS8_Ferrocarril_Estacion.Value,
				script.Card.Historico.VLEPS8_Ferrocarril_Vestibulo.Value,
				script.Card.Historico.VLEPS8_Bus_Linea.Value,
				script.Card.Historico.VLEPS8_Bus_Convoy.Value
			);
		}
		#endregion GetHistoricoPlaceAsync

		#region GetHistoricoOperatorAsync
		public async Task<string> GetHistoricoOperator1Async(long uid, ITransportCardGetValidationOperatorScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[0].Blocks[1])) &&
				(await script.Card.CheckAsync(uid, script.Card.Sectors[4].Blocks[2]))
			))
				return null;

			return await GetCardOwnerNameAsync(script.Card.Tarjeta.CodigoEntorno.Value, script.Card.Historico.EmpresaOperadora1.Value);
		}
		public async Task<string> GetHistoricoOperator2Async(long uid, ITransportCardGetValidationOperatorScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[0].Blocks[1])) &&
				(await script.Card.CheckAsync(uid, script.Card.Sectors[7].Blocks[0]))
			))
				return null;

			return await GetCardOwnerNameAsync(script.Card.Tarjeta.CodigoEntorno.Value, script.Card.Historico.EmpresaOperadora2.Value);
		}
		public async Task<string> GetHistoricoOperator3Async(long uid, ITransportCardGetValidationOperatorScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[0].Blocks[1])) &&
				(await script.Card.CheckAsync(uid, script.Card.Sectors[7].Blocks[1]))
			))
				return null;

			return await GetCardOwnerNameAsync(script.Card.Tarjeta.CodigoEntorno.Value, script.Card.Historico.EmpresaOperadora3.Value);
		}
		public async Task<string> GetHistoricoOperator4Async(long uid, ITransportCardGetValidationOperatorScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[0].Blocks[1])) &&
				(await script.Card.CheckAsync(uid, script.Card.Sectors[7].Blocks[2]))
			))
				return null;

			return await GetCardOwnerNameAsync(script.Card.Tarjeta.CodigoEntorno.Value, script.Card.Historico.EmpresaOperadora4.Value);
		}
		public async Task<string> GetHistoricoOperator5Async(long uid, ITransportCardGetValidationOperatorScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[0].Blocks[1])) &&
				(await script.Card.CheckAsync(uid, script.Card.Sectors[5].Blocks[2]))
			))
				return null;

			return await GetCardOwnerNameAsync(script.Card.Tarjeta.CodigoEntorno.Value, script.Card.Historico.EmpresaOperadora5.Value);
		}
		public async Task<string> GetHistoricoOperator6Async(long uid, ITransportCardGetValidationOperatorScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[0].Blocks[1])) &&
				(await script.Card.CheckAsync(uid, script.Card.Sectors[11].Blocks[0]))
			))
				return null;

			return await GetCardOwnerNameAsync(script.Card.Tarjeta.CodigoEntorno.Value, script.Card.Historico.EmpresaOperadora6.Value);
		}
		public async Task<string> GetHistoricoOperator7Async(long uid, ITransportCardGetValidationOperatorScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[0].Blocks[1])) &&
				(await script.Card.CheckAsync(uid, script.Card.Sectors[11].Blocks[1]))
			))
				return null;

			return await GetCardOwnerNameAsync(script.Card.Tarjeta.CodigoEntorno.Value, script.Card.Historico.EmpresaOperadora7.Value);
		}
		public async Task<string> GetHistoricoOperator8Async(long uid, ITransportCardGetValidationOperatorScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[0].Blocks[1])) &&
				(await script.Card.CheckAsync(uid, script.Card.Sectors[11].Blocks[2]))
			))
				return null;

			return await GetCardOwnerNameAsync(script.Card.Tarjeta.CodigoEntorno.Value, script.Card.Historico.EmpresaOperadora8.Value);
		}
		#endregion GetHistoricoOperatorAsync

		#region GetCargaDateAsync
		public async Task<DateTime?> GetCargaDate1Async(long uid, ITransportCardGetCargaDateScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[2].Blocks[0]))
			))
				return null;

			return await GetCargaDateAsync(
				script.Card.Carga.Fecha1.Value
			);
		}
		public async Task<DateTime?> GetCargaDate2Async(long uid, ITransportCardGetCargaDateScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[3].Blocks[1]))
			))
				return null;

			return await GetCargaDateAsync(
				script.Card.Carga.Fecha2.Value
			);
		}
		private async Task<DateTime?> GetCargaDateAsync(DateTime? date)
		{
			return await Task.Run(() => date);
		}
		#endregion GetCargaDateAsync

		#region GetCargaTypeNameAsync
		public async Task<string> GetCargaTypeName1Async(long uid, ITransportCardGetCargaTypeNameScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[2].Blocks[0]))
			))
				return null;

			return await GetCargaTypeNameAsync(
				script.Card.Carga.TipoOperacion1_Operacion.Value
			);
		}
		public async Task<string> GetCargaTypeName2Async(long uid, ITransportCardGetCargaTypeNameScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[2].Blocks[0]))
			))
				return null;

			return await GetCargaTypeNameAsync(
				script.Card.Carga.TipoOperacion2_Operacion.Value
			);
		}
		private async Task<string> GetCargaTypeNameAsync(EigeTipoOperacionCarga_OperacionEnum operacion)
		{
			return await Task.Run(() =>
				operacion == EigeTipoOperacionCarga_OperacionEnum.Carga ? "Carga" :
				operacion == EigeTipoOperacionCarga_OperacionEnum.Recarga ? "Recarga" :
				operacion == EigeTipoOperacionCarga_OperacionEnum.AmpliacionTemporal ? "Amplicación temporal" :
				operacion == EigeTipoOperacionCarga_OperacionEnum.AmpliacionCantidad ? "Amplicación cantidad" :
				operacion == EigeTipoOperacionCarga_OperacionEnum.CambioOperador ? "Cambio operador" :
				operacion == EigeTipoOperacionCarga_OperacionEnum.CambioZona ? "Cambio zona" :
				operacion == EigeTipoOperacionCarga_OperacionEnum.Anulacion ? "Anulación" :
				"Otros"
			);
		}
		#endregion GetCargaTypeNameAsync

		#region GetCargaTitleNameAsync
		public async Task<string> GetCargaTitleName1Async(long uid, ITransportCardGetTitleNameScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[2].Blocks[0]))
			))
				return null;

			return await GetTitleNameAsync(
				script.Card.Carga.CodigoTitulo1.Value
			);
		}
		public async Task<string> GetCargaTitleName2Async(long uid, ITransportCardGetTitleNameScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[3].Blocks[1]))
			))
				return null;

			return await GetTitleNameAsync(
				script.Card.Carga.CodigoTitulo2.Value
			);
		}
		#endregion GetCargaTitleNameAsync

		#region GetCargaQuantityAsync
		public async Task<decimal?> GetCargaQuantity1Async(long uid, ITransportCardGetCargaTypeNameScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[2].Blocks[0]))
			))
				return null;

			return await GetCargaQuantityAsync(
				script.Card.Carga.Importe1.Value
			);
		}
		public async Task<decimal?> GetCargaQuantity2Async(long uid, ITransportCardGetCargaTypeNameScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[3].Blocks[1]))
			))
				return null;

			return await GetCargaQuantityAsync(
				script.Card.Carga.Importe2.Value
			);
		}
		private async Task<decimal?> GetCargaQuantityAsync(decimal importe)
		{
			return await Task.Run(() => importe);
		}
		#endregion GetCargaQuantityAsync

		// LastValidation
		#region GetLastValidationTypeNameAsync
		public async Task<string> GetLastValidationTypeNameAsync(long uid, ITransportCardGetValidationTypeNameScript script, int? code)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[1].Blocks[1])) &&
				(await script.Card.CheckAsync(uid, script.Card.Sectors[9].Blocks[0]))
			))
				return null;

			return
				IsTuiN(code) ?
					await GetValidationTypeNameAsync(script.Card.TituloTuiN.UltimaValidacionTipo_Transbordo.Value, script.Card.TituloTuiN.UltimaValidacionTipo_Sentido.Value)
				:
					await GetValidationTypeNameAsync(script.Card.Validacion.TipoValidacion_Transbordo.Value, script.Card.Validacion.TipoValidacion_Sentido.Value);
		}
		#endregion GetLastValidationTypeNameAsync

		#region GetLastValidationPlaceAsync
		public async Task<string> GetLastValidationPlaceAsync(long uid, ITransportCardGetValidationPlaceScript script, int? code)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[1].Blocks[1])) &&
				(await script.Card.CheckAsync(uid, script.Card.Sectors[9].Blocks[0]))
			))
				return null;

			return
				IsTuiN(code) ?
					await GetValidationPlaceAsync(
						EigeTipoValidacion_TransporteEnum.Ferrocarril,
						script.Card.TituloTuiN.UltimaValidacionEstacion.Value,
						0,
						0,
						0
					)
				:
					await GetValidationPlaceAsync(
						script.Card.Validacion.TipoValidacion_Transporte.Value,
						script.Card.Validacion.VLEPS_Ferrocarril_Estacion.Value,
						script.Card.Validacion.VLEPS_Ferrocarril_Vestibulo.Value,
						script.Card.Validacion.VLEPS_Bus_Linea.Value,
						script.Card.Validacion.VLEPS_Bus_Convoy.Value
					);
		}
		#endregion GetLastValidationPlaceAsync

		#region GetLastValidationOperatorAsync
		public async Task<string> GetLastValidationOperatorAsync(long uid, ITransportCardGetValidationOperatorScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[0].Blocks[1])) &&
				(await script.Card.CheckAsync(uid, script.Card.Sectors[1].Blocks[1]))
			))
				return null;

			return await GetCardOwnerNameAsync(script.Card.Tarjeta.CodigoEntorno.Value, script.Card.Validacion.EmpresaOperadora.Value);
		}
		#endregion GetLastValidationOperatorAsync

		#region GetLastValidationDateAsync
		public async Task<DateTime?> GetLastValidationDateAsync(long uid, ITransportCardGetValidationDateScript script, int? code)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[1].Blocks[1])) &&
				(await script.Card.CheckAsync(uid, script.Card.Sectors[9].Blocks[0]))
			))
				return (DateTime?)null;

			return
				IsTuiN(code) ?
					await GetValidationDateAsync(script.Card.TituloTuiN.UltimaValidacionFechaHora.Value)
				:
					await GetValidationDateAsync(script.Card.Validacion.FechaValidacion.Value);
		}
		#endregion GetLastValidationDateAsync

		#region GetLastValidationPeopleTravelingAsync
		public async Task<int?> GetLastValidationPeopleTravelingAsync(long uid, ITransportCardGetValidationPeopleTravelingScript script, int? code)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[1].Blocks[1])) &&
				(await script.Card.CheckAsync(uid, script.Card.Sectors[9].Blocks[0]))
			))
				return null;

			return
				IsTuiN(code) ? await GetValidationPeopleTravelingAsync(script.Card.TituloTuiN.NumeroPersonasViajando.Value) :
				await GetValidationPeopleTravelingAsync(script.Card.Validacion.NumeroPersonasViaje.Value);
		}
		#endregion GetLastValidationPeopleTravelingAsync

		#region GetLastValidationPeopleInTransferAsync
		public async Task<int?> GetLastValidationPeopleInTransferAsync(long uid, ITransportCardGetValidationPeopleInTransferScript script, int? code)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[1].Blocks[1]))
			))
				return null;

			return IsTuiN(code) ?
				await GetValidationPeopleInTransferTuinAsync(script.Card.Validacion.NumeroPersonasTrasbordo.Value) :
				await GetValidationPeopleInTransferAsync(script.Card.Validacion.NumeroPersonasTrasbordo.Value, script.Card.Validacion.ContadorViajerosSalida.Value);
		}
		#endregion GetLastValidationPeopleInTransferAsync

		#region GetLastValidationInternalTransfersAsync
		public async Task<int?> GetLastValidationInternalTransfersAsync(long uid, ITransportCardGetValidationInternalTransfersScript script, int? code)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[1].Blocks[1])) &&
				(await script.Card.CheckAsync(uid, script.Card.Sectors[9].Blocks[0]))
			))
				return null;

			return IsTuiN(code) ?
				await GetValidationInternalTransfersAsync(script.Card.TituloTuiN.ContadorTransbordosInternos.Value) :
				await GetValidationInternalTransfersAsync(script.Card.Validacion.ContadorTransbordosInternos.Value);
		}
		#endregion GetLastValidationInternalTransfersAsync

		#region GetLastValidationExternalTransfersAsync
		public async Task<int?> GetLastValidationExternalTransfersAsync(long uid, ITransportCardGetValidationExternalTransfersScript script, int? code)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[1].Blocks[1]))
			))
				return null;

			return IsTuiN(code) ?
				0 :
				await GetValidationExternalTransfersAsync(script.Card.Validacion.ContadorTransbordosExternos.Value);
		}
		#endregion GetLastValidationExternalTransfersAsync

		#region GetLastValidationMaxInternalTransfersAsync
		public async Task<int?> GetLastValidationMaxInternalTransfersAsync(long uid, ITransportCardGetValidationMaxInternalTransfersScript script, int? code)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[1].Blocks[1]))
			))
				return null;

			return IsTuiN(code) ?
				1 :
				await GetValidationMaxInternalTransfersAsync(script.Card.Validacion.MaxTransbordosInternos.Value);
		}
		#endregion GetLastValidationMaxInternalTransfersAsync

		#region GetLastValidationMaxExternalTransfersAsync
		public async Task<int?> GetLastValidationMaxExternalTransfersAsync(long uid, ITransportCardGetValidationMaxExternalTransfersScript script, int? code, int? maxExternalTransfers)
		{
			return await Task.Run(() =>
			{
				return IsTuiN(code) ?
					0 :
					maxExternalTransfers ?? 0;
			});
		}
		#endregion GetLastValidationMaxExternalTransfersAsync

		#region GetDeviceTypeAsync
		public async Task<EigeFechaPersonalizacion_DispositivoEnum?> GetDeviceTypeAsync(long uid, ITransportCardGetValidationMaxInternalTransfersScript script)
		{
			if (!(
				(await script.Card.CheckAsync(uid, script.Card.Sectors[2].Blocks[0]))
			))
				return null;

			return await GetDeviceTypeAsync(
				script.Card.Personalizacion.TipoOperacion.Value,
				script.Card.Personalizacion.FechaPersonalizacion_Dispositivo.Value
			);
		}
		private async Task<EigeFechaPersonalizacion_DispositivoEnum?> GetDeviceTypeAsync(EigeTipoOperacionEnum tipoOperacion, EigeFechaPersonalizacion_DispositivoEnum? fechaPersonalizacion)
		{
			return await Task.Run(() =>
				tipoOperacion != 0 ? null :
				fechaPersonalizacion
			);
		}
		#endregion GetDeviceTypeAsync

		#region GetRechargesAsync
		public async Task<IQueryable<TransportPrice>> GetRechargesAsync(
			long uid,
			TransportCardGetReadInfoScript script,
			DateTime now,
			RechargeType rechargeType,
			int? titleCode,
			IEnumerable<int> priceIds,
			int rechargeCount,
			bool checkMaxBalance = true
		)
		{
			// Card
			var cardOwner = await GetCardOwnerAsync(uid, script);
			var cardExpiredDate = await GetExpiredDateAsync(uid, script);
			var cardType = script.Card.Tarjeta.Tipo == null ? null : (int?)script.Card.Tarjeta.Tipo.Value;
			var cardSubtype = script.Card.Tarjeta.Subtipo == null ? null : (int?)script.Card.Tarjeta.Subtipo.Value;
			var cardYoungDate = await GetExpiredYouthDateAsync(uid, script);

			// Title 1
			var titleOwnerCode1 = script.Card.Titulo.EmpresaPropietaria1.Value;
			var titleCode1 =
				((script.Card.Titulo.TitulosActivos.Value & EigeTitulosActivosEnum.Titulo1) == 0) ? (int?)null :
					script.Card.Titulo.CodigoTitulo1.Value;
			var titleZone1 = script.Card.Titulo.ValidezZonal1.Value;
			var titleHasBalance1 = await GetTitleHasBalance1Async(uid, script);
			var titleBalance1 = script.Card.Titulo.SaldoViaje1.Value;
			var titleTarifa1 = script.Card.Titulo.ControlTarifa1.Value;
			var titleIsTemporal1 = await GetTitleIsTemporal1Async(uid, script);
			var titleInAmpliation1 = script.Card.Titulo.TituloEnAmpliacion1.Value;
			var titleValidez1 = script.Card.Titulo.FechaValidez1.Value;
			var titleActive1 = await GetTitleActive1Async(uid, script);
			var titleIsExhausted1 = await GetTitleIsExhausted1Async(uid, script, now);
			var titleExhaustedDate1 = await GetTitleExhaustedDate1Async(uid, script);
			var viajes1 = script.Card.Titulo.SaldoViaje1.Value;

			// Title 2
			var titleOwnerCode2 = script.Card.Titulo.EmpresaPropietaria2.Value;
			var titleCode2 =
				((script.Card.Titulo.TitulosActivos.Value & EigeTitulosActivosEnum.Titulo2) == 0) ? (int?)null :
					script.Card.Titulo.CodigoTitulo2.Value;
			var titleZone2 = script.Card.Titulo.ValidezZonal2.Value;
			var titleHasBalance2 = await GetTitleHasBalance2Async(uid, script);
			var titleBalance2 = script.Card.Titulo.SaldoViaje2.Value;
			var titleTarifa2 = script.Card.Titulo.ControlTarifa2.Value;
			var titleIsTemporal2 = await GetTitleIsTemporal2Async(uid, script);
			var titleInAmpliation2 = script.Card.Titulo.TituloEnAmpliacion2.Value;
			var titleValidez2 = script.Card.Titulo.FechaValidez2.Value;
			var titleActive2 = await GetTitleActive2Async(uid, script);
			var titleExhausted2 = await GetTitleIsExhausted2Async(uid, script, now);
			var titleExhaustedDate2 = await GetTitleExhaustedDate2Async(uid, script);
			var viajes2 = script.Card.Titulo.SaldoViaje2.Value;

			var list = (await RepositoryPrice.GetAsync("Title.FromTransportTitle", "Title.ToTransportTitle", "Title.TransportConcession", "Title.TransportConcession.Concession"));

			var support = (await RepositorySupport.GetAsync())
				.Where(x =>
					(x.OwnerCode == cardOwner) &&
					(x.Type == cardType) &&
					(
						(x.SubType == null) ||
						(x.SubType == cardSubtype)
					) &&
					// Tarjetas caducadas
					(
						(cardExpiredDate == null) ||
						(cardExpiredDate >= now) ||
						(x.UsefulWhenExpired)
					)
				)
				.OrderBy(x => x.SubType ?? 1000)
				.FirstOrDefault();
			if (support == null)
			{
				list = list.Where(x => false);
				return list;
			}

#if DEBUG
			var list4 = list.ToList();
#endif // DEBUG

			list = list.Where(x =>
				// Price
				(x.State == TransportPriceState.Active) &&
				// Title
				(x.Title.OperateByPayIn) &&
				(x.Title.State == TransportTitleState.Active) &&
				// Between dates
				(x.Start <= now) &&
				(x.End >= now) &&
				// Version
				(x.Version == x.Title.Prices
					.Where(y =>
						(y.State == TransportPriceState.Active) &&
						(y.Start <= now) &&
						(y.End >= now) &&
						(
							(!y.Title.HasZone) ||
							(y.Zone == x.Zone)
						)
					)
					.Select(y => y.Version)
					.Max()
				) &&
				// Young titles
				(
					!(x.Title.IsYoung) ||
					(
						cardYoungDate != null &&
						cardYoungDate >= now
					)
				)
			);

#if DEBUG
			var list3 = list.ToList();
#endif // DEBUG

			if (priceIds != null)
				list = list.Where(x => priceIds.Contains(x.Id));
			if (titleCode != null)
				list = list.Where(x => x.Title.Code == titleCode);

			// Titulo 1
			var result1 = await GetRechargesAsync(list, now, rechargeType, cardOwner, cardType, cardSubtype, 1,
				titleActive1, titleIsExhausted1, titleExhaustedDate1, titleCode1, titleZone1, titleIsTemporal1, titleInAmpliation1, titleValidez1, titleBalance1, titleTarifa1, titleOwnerCode1,
				titleActive2, titleExhausted2, titleExhaustedDate2, titleCode2, titleZone2, titleIsTemporal2, titleInAmpliation2, titleValidez2, titleBalance2, titleTarifa2, titleOwnerCode2,
				rechargeCount, checkMaxBalance);

			// Titulo 2
			var result2 = await GetRechargesAsync(list, now, rechargeType, cardOwner, cardType, cardSubtype, 2,
				titleActive2, titleExhausted2, titleExhaustedDate2, titleCode2, titleZone2, titleIsTemporal2, titleInAmpliation2, titleValidez2, titleBalance2, titleTarifa2, titleOwnerCode2,
				titleActive1, titleIsExhausted1, titleExhaustedDate1, titleCode1, titleZone1, titleIsTemporal1, titleInAmpliation1, titleValidez1, titleBalance1, titleTarifa1, titleOwnerCode1,
				rechargeCount, checkMaxBalance);

			IQueryable<TransportPrice> result = null;
			if (result1.Count() > 0)
				result = result1.Union(result2);
			else
				result = result2;

#if DEBUG
			var list2 = result.ToList();
#endif // DEBUG

			return result;
		}
		public async Task<IQueryable<TransportPrice>> GetRechargesAsync(
			IQueryable<TransportPrice> list,
			DateTime now,
			RechargeType rechargeType,
			int? cardOwner,
			int? cardType,
			int? cardSubtype,
			int slot,
			// Same
			bool titleActive,
			bool titleIsExhausted,
			DateTime? titleExhaustedDate,
			int? titleCode,
			EigeZonaEnum titleZone,
			bool titleIsTemporal,
			bool titleInAmpliation,
			DateTime? titleValidez,
			decimal? titleBalance,
			int titleTarifa,
			int titleOwnerCode,
			// Other
			bool titleActiveOther,
			bool titleIsExhaustedOther,
			DateTime? titleExhaustedDateOther,
			int? titleCodeOther,
			EigeZonaEnum titleZoneOther,
			bool titleIsTemporalOther,
			bool titleInAmpliationOther,
			DateTime? titleValidezOther,
			decimal? titleBalanceOther,
			int titleTarifaOther,
			int titleOwnerCodeOther,
			int rechargesCount,
			bool checkMaxBalance
		)
		{
			return await Task.Run(() =>
			{
				if ((titleActive) && (!titleActiveOther) && (titleCode == 0) && (slot == 1))
					titleActive = false;
				if ((!titleActive) && (rechargeType == RechargeType.Recharge))
					return list.Where(x => false);
				if ((!titleActive || titleIsExhausted) && (!titleActiveOther || titleIsExhaustedOther) && (rechargeType == RechargeType.Charge) && (slot == 2))
					return list.Where(x => false);
				if ((!titleActive) && (titleActiveOther) && (rechargeType == RechargeType.Recharge))
					return list.Where(x => false);
				if (
					(titleActive) &&
					!((titleIsExhausted) && (rechargeType == RechargeType.Charge))
				)
				{
#if DEBUG
					var list4 = list.ToList();
#endif // DEBUG

					list = list
						.Where(x =>
							// Misma Zona (si recarga)
							(
								(rechargeType != RechargeType.Recharge) ||
								(!x.Title.HasZone) ||
								(x.Zone == titleZone)
							) &&
							// Mismo título (recarga)
							(
								(rechargeType != RechargeType.Recharge) ||
								(x.Title.Code == titleCode)
							) &&
							// Misma version o superior
							(
								(x.Title.Code != titleCode) ||
								(x.Version >= titleTarifa)
							) &&
                            // Bit Ampliación
                            (
                                (rechargeType == RechargeType.Recharge) || // Para comprobarlo en memoria
                                (!titleIsTemporal) ||
								(!titleInAmpliation) ||
								(
									(titleExhaustedDate != null) &&
									(titleExhaustedDate < now)
								) ||
								(
									// Diferentes operadores
									(x.Title.OwnerCode != titleOwnerCode) &&
									// Mismo Operador no canjeable
									(!x.Title.FromTransportTitle
										.Where(y => y.Code == titleCode)
										.Any())
								)
							) &&
							// Max.Quantity
							(
                                (rechargeType == RechargeType.Recharge) || // Para comprobarlo en memoria
                                (!checkMaxBalance) ||
								(x.Title.MaxQuantity == null) ||
								(titleIsTemporal) ||
								// Mismo Operador no canjeable
								(!(
									(x.Title.Code == titleCode) ||
									(x.Title.FromTransportTitle
										.Where(y => y.Code == titleCode)
										.Any())
								)) ||
								(x.Title.MaxQuantity >= titleBalance + rechargesCount * x.Title.Quantity) ||
								(
									// Diferentes operadores
									(x.Title.OwnerCode != titleOwnerCode) &&
									// Mismo Operador no canjeable
									(!x.Title.FromTransportTitle
										.Where(y => y.Code == titleCode)
										.Any())
								)
							)
						);
#if DEBUG
					var list3 = list.ToList();
#endif // DEBUG
				}

				list = list
					.Where(x =>
						(
							// Incompatibilidad de titulo-titulo
							(!titleActiveOther) ||
							(titleIsExhaustedOther) ||
							(
								x.Title.TransportSimultaneousTitleCompatibility
									.Where(y => y.TransportTitle2.Code == titleCodeOther)
									.Any() ||
								x.Title.TransportSimultaneousTitleCompatibility2
									.Where(y => y.TransportTitle.Code == titleCodeOther)
									.Any()
							)
						) && (
							// Incompatibilidad de tarjeta-titulo
							x.Title.TransportCardSupportTitleCompatibility
								.Where(y =>
									y.TransportCardSupport.OwnerCode == cardOwner &&
									y.TransportCardSupport.Type == cardType &&
									(
										y.TransportCardSupport.SubType == null ||
										y.TransportCardSupport.SubType == cardSubtype
									)
								)
								.Any()
						) && (
							// Ajustar slot
							x.Title.Slot == null ||
							x.Title.Slot.Value == slot
						)
					);

#if DEBUG
				var list2 = list.ToList();
#endif // DEBUG

				return list;
			});
		}
        #endregion GetRechargesAsync

        public List<string> GetRechargeImpediments(ServiceCardReadInfoResult title, ServiceCardReadInfoResult_RechargeTitle recharge, DateTime now)
        {
            var result = new List<string>();
            if (recharge == null)
                return result;

            if (GetRechargeImpediment_MaxQuantity(title, recharge, now))
                result.Add(ServiceCardReadInfoResult.MaxQuantity);
            if (GetRechargeImpediment_MaxAmpliation(title, recharge, now))
                result.Add(ServiceCardReadInfoResult.MaxAmpliation);

            return result;
        }
        private bool GetRechargeImpediment_MaxQuantity(ServiceCardReadInfoResult title, ServiceCardReadInfoResult_RechargeTitle recharge, DateTime now)
        {
            var rechargeMin = recharge.AskQuantity ? (recharge.RechargeMin ?? 1) : 1;

            return
                (title.HasBalance) &&
                (recharge.MaxQuantity != null) &&
                (recharge.MaxQuantity < title.Balance + rechargeMin * recharge.Quantity);
        }
        private bool GetRechargeImpediment_MaxAmpliation(ServiceCardReadInfoResult title, ServiceCardReadInfoResult_RechargeTitle recharge, DateTime now)
        {
            return
                (title.IsTemporal) &&
                (
                    (title.Ampliation >= 2) ||
                    (
                        (title.Ampliation == 1) &&
                        (title.ExhaustedDate != null) &&
                        (title.ExhaustedDate > now)
                    )
                );
        }

		#region GetRechargeConfig
		public TransportOperationReadInfoHandler.RechargeConfig GetRechargeConfig(long uid, TransportCardGetReadInfoScript script,
			bool titleActive1, bool titleExhausted1, TransportPrice cardPrice1,
			bool titleActive2, bool titleExhausted2, TransportPrice cardPrice2,
			TransportPrice price, DateTime now)
		{
			// Para rellenar el titulo x (sin titulos)
			if (
				(
					((!titleActive1) && (!titleActive2)) ||
					((titleActive1) && (!titleActive2) && (script.Card.Titulo.CodigoTitulo1.Value == 0)) // En algunos billetes vacio viene el titulo 1 activo con codigo 0
				) &&
				(price != null)
			)
			{
				return new TransportOperationReadInfoHandler.RechargeConfig
				{
					ChangePrice = 0,
					RechargeType = RechargeType.Charge,
					Slot = price.Title.Slot == 2 ?
						EigeTituloEnUsoEnum.Titulo2 :
						EigeTituloEnUsoEnum.Titulo1
				};
			}

			var compatibility1 = TitleCompatibleOther(price, 1, titleActive2, titleExhausted2, cardPrice2);
			var compatibility2 = TitleCompatibleOther(price, 2, titleActive1, titleExhausted1, cardPrice1);

			// Para rellenar el titulo 1 (recarga)
			if (
                (cardPrice1 != null) &&
                (compatibility1) &&
				(titleActive1) &&
				(price.Title.Code == script.Card.Titulo.CodigoTitulo1.Value)
			)
			{
				var result = GetRechargeConfigSameSlot(
					now,
					cardPrice1,
					EigeTituloEnUsoEnum.Titulo1,
					price,
					script.Card.Titulo.EmpresaPropietaria1.Value,
					script.Card.Titulo.CodigoTitulo1.Value,
					script.Card.Titulo.ValidezZonal1.Value,
					script.Card.Titulo.SaldoViaje1.Value,
					script.Card.Titulo.FechaValidez1.Value,
					script.Card.Titulo.ControlTarifa1.Value,
					cardPrice1.Title.IsOverWritable
				);

				if (result != null)
					return result;
			}

			// Para rellenar el titulo 2 (recarga)
			if (
                (cardPrice2 != null) &&
                (compatibility2) &&
				(titleActive2) &&
				(price.Title.Code == script.Card.Titulo.CodigoTitulo2.Value)
			)
			{
				var result = GetRechargeConfigSameSlot(
					now,
					cardPrice2,
					EigeTituloEnUsoEnum.Titulo2,
					price,
					script.Card.Titulo.EmpresaPropietaria2.Value,
					script.Card.Titulo.CodigoTitulo2.Value,
					script.Card.Titulo.ValidezZonal2.Value,
					script.Card.Titulo.SaldoViaje2.Value,
					script.Card.Titulo.FechaValidez2.Value,
					script.Card.Titulo.ControlTarifa2.Value,
					cardPrice2.Title.IsOverWritable
				);
				if (result != null)
					return result;
			}

			// Para rellenar el titulo 2 (vacio)
			if (
				(compatibility2) &&
				(!titleActive2 || titleExhausted2)
			)
			{
				var result = GetRechargeConfigOtherSlot(
					EigeTituloEnUsoEnum.Titulo2,
					price,
					script.Card.Titulo.EmpresaPropietaria1.Value,
					script.Card.Titulo.CodigoTitulo1.Value,
					script.Card.Titulo.ValidezZonal1.Value,
					script.Card.Titulo.SaldoViaje1.Value
				);
				if (result != null)
					return result;
			}

			// Para rellenar el titulo 1 (vacio)
			if (
				(compatibility1) &&
				(!titleActive1 || titleExhausted1)
			)
			{
				var result = GetRechargeConfigOtherSlot(
					EigeTituloEnUsoEnum.Titulo1,
					price,
					script.Card.Titulo.EmpresaPropietaria2.Value,
					script.Card.Titulo.CodigoTitulo2.Value,
					script.Card.Titulo.ValidezZonal2.Value,
					script.Card.Titulo.SaldoViaje2.Value
				);
				if (result != null)
					return result;
			}

			// Para rellenar el titulo 1 (canje)
			if (
				(compatibility1) &&
				(titleActive1) &&
				(price.Title.FromTransportTitle.Select(x => x.Code).Contains(script.Card.Titulo.CodigoTitulo1.Value))
			)
			{
				var result = GetRechargeConfigSameSlot(
					now,
					cardPrice1,
					EigeTituloEnUsoEnum.Titulo1,
					price,
					script.Card.Titulo.EmpresaPropietaria1.Value,
					script.Card.Titulo.CodigoTitulo1.Value,
					script.Card.Titulo.ValidezZonal1.Value,
					script.Card.Titulo.SaldoViaje1.Value,
					script.Card.Titulo.FechaValidez1.Value,
					script.Card.Titulo.ControlTarifa1.Value,
					cardPrice1.Title.IsOverWritable
				);
				return result;
			}

			// Para rellenar el titulo 2 (canje)
			if (
				(compatibility2) &&
				(titleActive2) &&
				(price.Title.FromTransportTitle.Select(x => x.Code).Contains(script.Card.Titulo.CodigoTitulo2.Value))
			)
			{
				var result = GetRechargeConfigSameSlot(
					now,
					cardPrice2,
					EigeTituloEnUsoEnum.Titulo2,
					price,
					script.Card.Titulo.EmpresaPropietaria2.Value,
					script.Card.Titulo.CodigoTitulo2.Value,
					script.Card.Titulo.ValidezZonal2.Value,
					script.Card.Titulo.SaldoViaje2.Value,
					script.Card.Titulo.FechaValidez2.Value,
					script.Card.Titulo.ControlTarifa2.Value,
					cardPrice2.Title.IsOverWritable
				);
				return result;
			}

			// Para rellenar el titulo 1 (reemplazar)
			if (
				(compatibility1) &&
				(titleActive1)
			)
			{
				return new TransportOperationReadInfoHandler.RechargeConfig
				{
					ChangePrice = 0,
					RechargeType = RechargeType.Replace,
					Slot = EigeTituloEnUsoEnum.Titulo1
				};
			}

			// Para rellenar el titulo 2 (reemplazar)
			if (
				(compatibility2) &&
				(titleActive2)
			)
			{
				return new TransportOperationReadInfoHandler.RechargeConfig
				{
					ChangePrice = 0,
					RechargeType = RechargeType.Replace,
					Slot = EigeTituloEnUsoEnum.Titulo2
				};
			}

#if DEBUG
			throw new ArgumentException("Calculando canje de titulo no previsto.");
#else
			return new TransportOperationReadInfoHandler.RechargeConfig
				{
					ChangePrice = 0,
					RechargeType = RechargeType.Charge,
					Slot = EigeTituloEnUsoEnum.Titulo1
				};
#endif
		}
		private TransportOperationReadInfoHandler.RechargeConfig GetRechargeConfigSameSlot(DateTime now, TransportPrice cardPrice, EigeTituloEnUsoEnum slot, TransportPrice price, int ownerCode, int code, EigeZonaEnum? zone, decimal saldo, DateTime? fechaActivacion, int version, bool isOverWritable)
		{
			if (price.Title.OwnerCode != ownerCode)
				return null;
			if (price == null)
				return null;

			if (cardPrice == null) // No existe tarifa en BD
			{
				if (price.Title.Code == code)
				{
					if (price.Version > version) // Tarifa obsoleta (no en la BD)
						return new TransportOperationReadInfoHandler.RechargeConfig
						{
							ChangePrice = 0,
							RechargeType = RechargeType.RechargeExpiredPrice,
							Slot = slot
						};
				}
			}
			else if (price.Title.Code == code)
			{
				if ((cardPrice.Start >= now) || (cardPrice.End <= now))
				{
					// Tarifa obsoleta
					return new TransportOperationReadInfoHandler.RechargeConfig
					{
						ChangePrice = 0,
						RechargeType = RechargeType.RechargeExpiredPrice,
						Slot = slot
					};
				}
				else if ((price.Title.HasZone) && (price.Zone != zone))
				{
					// Canje desde otra zona
					if (((int)price.Zone.Value).ToByteArray().SumBits() > ((int)zone.Value).ToByteArray().SumBits())
					{
						// Zona superior
						if (price.Title.Quantity != 0)
							return new TransportOperationReadInfoHandler.RechargeConfig
							{
								ChangePrice = ChangePrice(saldo, cardPrice.Price, price.Price, price.Title.Quantity.Value),
								RechargeType = RechargeType.RechargeAndUpdateZone,
								Slot = slot
							};
						else if (price.Title.TemporalUnit > 0)
							return new TransportOperationReadInfoHandler.RechargeConfig
							{
								ChangePrice = ChangePrice(fechaActivacion, cardPrice.Price, price.Price, price.Title.TemporalUnit.Value, price.Title.TemporalType.Value, now),
								RechargeType = RechargeType.RechargeAndUpdateZone,
								Slot = slot
							};
					}
					else
					{
						// Zona inferior
						if (price.Title.Quantity != 0)
							return new TransportOperationReadInfoHandler.RechargeConfig
							{
								ChangePrice = 0,
								RechargeType = RechargeType.Replace,
								Slot = slot
							};
						else if (price.Title.TemporalUnit > 0)
							return new TransportOperationReadInfoHandler.RechargeConfig
							{
								ChangePrice = 0,
								RechargeType = RechargeType.Replace,
								Slot = slot
							};
					}
				}
				else if (price.Price == cardPrice.Price)
				{
					// Recarga normal
					return new TransportOperationReadInfoHandler.RechargeConfig
					{
						ChangePrice = 0,
						RechargeType = RechargeType.Recharge,
						Slot = slot
					};
				}
				else
				{
					// Actualizar tarifa
					if (price.Title.Quantity > 0)
						return new TransportOperationReadInfoHandler.RechargeConfig
						{
							ChangePrice = ChangePrice(saldo, cardPrice.Price, price.Price, price.Title.Quantity.Value),
							RechargeType = RechargeType.RechargeAndUpdatePrice,
							Slot = slot
						};
					else if (price.Title.TemporalUnit > 0)
						return new TransportOperationReadInfoHandler.RechargeConfig
						{
							ChangePrice = ChangePrice(fechaActivacion, cardPrice.Price, price.Price, price.Title.TemporalUnit.Value, price.Title.TemporalType.Value, now),
							RechargeType = RechargeType.RechargeAndUpdatePrice,
							Slot = slot
						};
				}
			}
			else if (isOverWritable)
			{
				// Canje a otro titulo
				if (price.Title.Quantity != 0) // Saldo
					return new TransportOperationReadInfoHandler.RechargeConfig
					{
						ChangePrice = ChangePrice(saldo, cardPrice.Price, price.Price, price.Title.Quantity.Value),
						RechargeType = RechargeType.Exchange,
						Slot = slot
					};
				else if (price.Title.TemporalUnit > 0) // Temporal
				{
					if(cardPrice.Title.Quantity != 0)
						return new TransportOperationReadInfoHandler.RechargeConfig
						{
							ChangePrice = ReturnPrice(saldo, cardPrice.Price, price.Price, cardPrice.Title.Quantity.Value),
							RechargeType = RechargeType.Exchange,
							Slot = slot
						};
					else if (cardPrice.Title.TemporalUnit > 0)
						return new TransportOperationReadInfoHandler.RechargeConfig
						{
							ChangePrice = ChangePrice(fechaActivacion, cardPrice.Price, price.Price, price.Title.TemporalUnit.Value, price.Title.TemporalType.Value, now),
							RechargeType = RechargeType.Replace,
							Slot = slot
						};
				}
			}
			//else 
			//	if (price.Unities > 0) // Canje desde otra zona (temporal)
			//	return new TransportOperationReadInfoHandler.RechargeConfig
			//	{
			//		ChangePrice = 0,
			//		RechargeType = RechargeType.Replace,
			//		Slot = slot
			//	};

			return null;
		}
		private TransportOperationReadInfoHandler.RechargeConfig GetRechargeConfigOtherSlot(EigeTituloEnUsoEnum slot, TransportPrice price, int ownerCode, int code, EigeZonaEnum? zone, decimal saldo)
		{
			return new TransportOperationReadInfoHandler.RechargeConfig
			{
				ChangePrice = 0,
				RechargeType = RechargeType.Charge,
				Slot = slot
			};
		}
		#endregion GetRechargeConfig

		#region TitleCompatibleOther
		public bool TitleCompatibleOther(TransportPrice price, int slot, bool titleActiveOther, bool titleExhaustedOther, TransportPrice cardPriceOther)
		{
			return
				(price != null) &&
				(
					(!titleActiveOther) ||
					(titleExhaustedOther) ||
					(price.Title.TransportSimultaneousTitleCompatibility
						.Where(y => y.TransportTitle2.Code == cardPriceOther.Title.Code)
						.Any()) ||
					(price.Title.TransportSimultaneousTitleCompatibility2
						.Where(y => y.TransportTitle.Code == cardPriceOther.Title.Code)
						.Any())
				) &&
				((price.Title.Slot == null) || (price.Title.Slot == slot));
		}
		#endregion TitleCompatibleOther

		#region ChangePrice
		private decimal ChangePrice(decimal saldo, decimal oldprice, decimal newPrice, decimal quantity)
		{
			return saldo * (newPrice - oldprice) / quantity;
		}
		private decimal ChangePrice(DateTime? exhausted, decimal oldprice, decimal newPrice, int total, EigeTipoUnidadesValidezTemporalEnum tipoUnidades, DateTime now)
		{
			var quantity = exhausted == null ? total :
				tipoUnidades == EigeTipoUnidadesValidezTemporalEnum.Dias ? Convert.ToDecimal((exhausted.Value - now.Date).TotalDays) :
				0;
			return (newPrice - oldprice) * quantity / total;
		}
		#endregion ChangePrice

		#region ReturnPrice
		private decimal ReturnPrice(decimal saldo, decimal oldprice, decimal newPrice, decimal quantity)
		{
			if (oldprice >= newPrice)
				return 0;
			else
				return saldo * (oldprice / quantity);
		}
		#endregion ReturnPrice
	}
}
