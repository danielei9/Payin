using PayIn.Application.Dto.Transport.Arguments.TransportCard;
using PayIn.Application.Dto.Transport.Results.TransportPrice;
using PayIn.Application.Dto.Transport.Results.TransportCard;
using PayIn.Application.Transport.Scripts;
using PayIn.Application.Transport.Services;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Transport;
using PayIn.Domain.Transport.Eige.Enums;
using PayIn.Domain.Transport.Eige.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Public.Handlers
{
	[XpLog("TransportCard", "RechargeFinish", RelatedId = "CardNumber")]
	public class TransportCardRechargeFinishHandler :
		IQueryBaseHandler<TransportCardRechargeFinishArguments, TransportCardRechargeFinishResult>
	{
		private readonly ISessionData SessionData;
		private readonly EigeService EigeService;
		private readonly IEntityRepository<Ticket> RepositoryTicket;
		private readonly IEntityRepository<TransportPrice> RepositoryPrice;
		private readonly IEntityRepository<TransportOffer> RepositoryOffer;
		private readonly IEntityRepository<TransportTitle> RepositoryTitle;
		private readonly IEntityRepository<PaymentConcession> PaymentConcessionRepository;
		private readonly IEntityRepository<Ticket> TicketRepository;
		private readonly IUnitOfWork UnitOfWork;
		private readonly IMifareClassicHsmService Hsm;

		#region Constructors
		public TransportCardRechargeFinishHandler(
			ISessionData sessionData,
			EigeService eigeService,
			IEntityRepository<Ticket> repositoryTicket,
			IEntityRepository<TransportPrice> repositoryPrice,
			IEntityRepository<TransportOffer> repositoryOffer,
			IEntityRepository<TransportTitle> repositoryTitle,
			IEntityRepository<Ticket> ticketRepository,
			IEntityRepository<PaymentConcession> paymentConcessionRepository,
			IUnitOfWork unitOfWork,
			IMifareClassicHsmService hsm
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (eigeService == null) throw new ArgumentNullException("eigeService");
			if (repositoryTicket == null) throw new ArgumentNullException("repositoryTicket");
			if (repositoryPrice == null) throw new ArgumentNullException("repositoryPrice");
			if (repositoryOffer == null) throw new ArgumentNullException("repositoryOffer");
			if (repositoryTitle == null) throw new ArgumentNullException("repositoryTitle");
			if (ticketRepository == null) throw new ArgumentNullException("ticketRepository");
			if (paymentConcessionRepository == null) throw new ArgumentNullException("paymentConcessionRepository");
			if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");
			if (hsm == null) throw new ArgumentNullException("hsm");

			SessionData = sessionData;
			EigeService = eigeService;
			RepositoryTicket = repositoryTicket;
			RepositoryPrice = repositoryPrice;
			RepositoryOffer = repositoryOffer;
			RepositoryTitle = repositoryTitle;
			TicketRepository = ticketRepository;
			PaymentConcessionRepository = paymentConcessionRepository;
			UnitOfWork = unitOfWork;
			Hsm = hsm;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<TransportCardRechargeFinishResult>> ExecuteAsync(TransportCardRechargeFinishArguments arguments)
		{

			return await Task.Run(async () =>
			{
				var now = DateTime.Now;
				//Test Alberto
				if (arguments.Id == 0) arguments.Id = 5;
				//End Test
				var uid = arguments.CardId.FromHexadecimal().ToInt32().Value;
				var script = new TransportCardGetRechargeScript(SessionData.Login, Hsm, arguments.Script);

				var inBlackList = await EigeService.InBlackListAsync(uid, script);

				// Cargar ticket
				var ticket = await RepositoryTicket.GetAsync(arguments.IdTicket, "Payments", "Concession.Concession.Supplier", "PaymentWorker");
				if (ticket == null)
					throw new ArgumentNullException("No se encuentra el ticket asociado al pago.");

				var paid = ticket.Payments
					.Where(x => x.State == PaymentState.Active)
					.Sum(x => (decimal?)x.Amount) ?? 0;

				if (ticket.Amount < paid)
					throw new ArgumentNullException("Ticket no pagado.");
				//Control tarifa.
				//TF1CT codigo titulo. TransportTitle(Code) TransportPrice
				//TF1TF Control Tarifa,Version
				//TF1TT Tipo tarifa
				var titulosActivos = script.Card.Titulo.TitulosActivos;
				bool titulo1 = false;
				bool titulo2 = false;

				if (titulosActivos.Value == EigeTitulosActivosEnum.Titulo1 /*&& !inBlackList1*/)
					titulo1 = true;
				if (titulosActivos.Value == EigeTitulosActivosEnum.Titulo2)
					titulo2 = true;
				var transporCardRecharge = (await RepositoryPrice.GetAsync("Title"))
						.Where(x => x.Id == arguments.Id).FirstOrDefault();
				var transportOffer = (await RepositoryOffer.GetAsync())
						.Where(x => x.TransportPriceId == arguments.Id).FirstOrDefault();
				var code = transporCardRecharge.Title.Code;
				var result = (await RepositoryPrice.GetAsync())
						.Where(x => x.Title.Code == code &&
								x.Start > now &&
								x.End < now)
						.Select(x => new TransportPriceGetAllResult
						{
							Start = x.Start,
							End = x.End,
							Price = x.Price,
							Quantity = x.Quantity.Value,
							Title = x.Title,
							TransportTitleId = x.TransportTitleId,
							Version = x.Version,
							TemporalUnities = x.TemporalUnities
						}).ToList();

				var transportPriceCard = (TransportPriceGetAllResult)null;
				foreach (var tP in result)
				{
					if (script.Card.Titulo.ControlTarifa1.Value == tP.Version || script.Card.Titulo.ControlTarifa2.Value == tP.Version)
					{
						transportPriceCard = tP;
						break;
					}
				}

				if (transportPriceCard != null)
				{
					bool abono = transportPriceCard.TemporalUnities.Value == EigeTipoUnidadesValidezTemporalEnum.Viajes ? false : true;
					bool abonoRecharge = transporCardRecharge.TemporalUnities.Value == EigeTipoUnidadesValidezTemporalEnum.Viajes ? false : true;
					if (transportPriceCard.Title != null && transportPriceCard.Title.Code == transporCardRecharge.Title.Code) //Mismo tipo de Tarifa.
					{
						if (transportPriceCard.Title.Prices.Select(x => x.Zone).FirstOrDefault() == transporCardRecharge.Title.Prices.Select(x => x.Zone).FirstOrDefault()) //Misma zona.
						{
							/*if (titulo1)
							{
								if (abono)
								{
									if (script.Card.Titulo.TituloEnAmpliacion1.Value == true)
										throw new ArgumentNullException("El titulo ya se encuentra en estado de ampliación");
									script.Card.Titulo.TituloEnAmpliacion1 = new EigeBool(true);
									script.Write(script.Response, x => x.Titulo.TituloEnAmpliacion1);
								}
								else
								{
									if (transportOffer != null) transporCardRecharge.Quantity += transportOffer.Quantity;
									script.Card.Titulo.SaldoViaje1 = new EigeInt8(script.Card.Titulo.SaldoViaje1.Value + transporCardRecharge.Quantity.Value);
									script.Write(script.Response, x => x.Titulo.SaldoViaje1);
								}
							}
							if (titulo2)
							{
								if (abono)
								{
									if (script.Card.Titulo.TituloEnAmpliacion2.Value == true)
										throw new ArgumentNullException("El titulo ya se encuentra en estado de ampliación");
									script.Card.Titulo.TituloEnAmpliacion2 = new EigeBool(true);
									script.Write(script.Response, x => x.Titulo.TituloEnAmpliacion2);
								}
								else
								{
									if (transportOffer != null) transporCardRecharge.Quantity += transportOffer.Quantity;
									script.Card.Titulo.SaldoViaje2 = new EigeInt8(script.Card.Titulo.SaldoViaje2.Value + transporCardRecharge.Quantity.Value);
									script.Write(script.Response, x => x.Titulo.SaldoViaje2);
								}
							}*/
							}
						else if (transportPriceCard.Title.Prices.Select(x => x.Zone).FirstOrDefault().ToEnumAlias().Length != transporCardRecharge.Title.Prices.Select(x => x.Zone).FirstOrDefault().ToEnumAlias().Length)
						{
							if (transporCardRecharge.Title.Prices.Select(x => x.Zone.ToEnumAlias()).FirstOrDefault().Contains(transportPriceCard.Title.Prices.Select(x => x.Zone.ToEnumAlias()).FirstOrDefault()))
							{
								/*
								if (abono)
								{
									if (titulo1)
									{
										script.Card.Titulo.TituloEnAmpliacion1 = new EigeBool(true);

										script.Card.Titulo.ValidezZonal1 = getZones(transporCardRecharge.Title.Prices.Select(x => x.Zone).FirstOrDefault());
										script.Write(script.Response, x => x.Titulo.TituloEnAmpliacion1);
									}
									if (titulo2)
									{
										if (script.Card.Titulo.TituloEnAmpliacion2.Value == false)
										{
											script.Card.Titulo.TituloEnAmpliacion1 = new EigeBool(true);
											script.Card.Titulo.ValidezZonal2 = getZones(transporCardRecharge.Title.Prices.Select(x => x.Zone).FirstOrDefault());
											script.Write(script.Response, x => x.Titulo.TituloEnAmpliacion1);
										}
										else
										{
											script.Card.Titulo.TituloEnAmpliacion1 = new EigeBool(true);
											script.Card.Titulo.ValidezZonal2 = getZones(transporCardRecharge.Title.Prices.Select(x => x.Zone).FirstOrDefault());
											script.Write(script.Response, x => x.Titulo.TituloEnAmpliacion2);
										}

									}
								}
								else
								{
									if (transportOffer != null) transporCardRecharge.Quantity += transportOffer.Quantity;
									if (titulo1)
									{	
										script.Card.Titulo.SaldoViaje1 = new EigeInt8(transporCardRecharge.Quantity.Value);
										script.Card.Titulo.ValidezZonal1 = getZones(transporCardRecharge.Title.Prices.Select(x => x.Zone).FirstOrDefault());
										script.Write(script.Response, x => x.Titulo.SaldoViaje1);
									}
									if (titulo2)
									{
										script.Card.Titulo.SaldoViaje2 = new EigeInt8(transporCardRecharge.Quantity.Value);
										script.Card.Titulo.ValidezZonal2 = getZones(transporCardRecharge.Title.Prices.Select(x => x.Zone).FirstOrDefault());
										script.Write(script.Response, x => x.Titulo.SaldoViaje2);
									}
								}
								*/
							}
						}
					}
				}
				else //Actualizar version sin poder encontrar la version vieja (no puedo hacer canje).
				{
					bool abono = transporCardRecharge.TemporalUnities.Value == EigeTipoUnidadesValidezTemporalEnum.Viajes ? false : true;
					/*if (titulo1)
					{
						script.Card.Titulo.ControlTarifa1 = new EigeInt8(transporCardRecharge.Version);
						byte[] intBytes = BitConverter.GetBytes(transporCardRecharge.Title.Code);
						if (BitConverter.IsLittleEndian)
							Array.Reverse(intBytes);
						script.Card.Titulo.CodigoTitulo1 = new EigeInt16(intBytes, 16);
						script.Write(script.Response, x => x.Titulo.ControlTarifa1);
						script.Write(script.Response, x => x.Titulo.CodigoTitulo1);
						if (abono)
						{
							script.Card.Titulo.TituloEnAmpliacion1 = new EigeBool(true);
							script.Write(script.Response, x => x.Titulo.TituloEnAmpliacion1);
						}
						else
						{
							if (transportOffer != null) transporCardRecharge.Quantity += transportOffer.Quantity;
							script.Card.Titulo.SaldoViaje1 = new EigeInt8(transporCardRecharge.Quantity.Value);
							script.Write(script.Response, x => x.Titulo.SaldoViaje1);
						}

					}
					if (titulo2)
					{
						script.Card.Titulo.ControlTarifa2 = new EigeInt8(transporCardRecharge.Version);
						byte[] intBytes = BitConverter.GetBytes(transporCardRecharge.Title.Code);
						if (BitConverter.IsLittleEndian)
							Array.Reverse(intBytes);
						script.Card.Titulo.CodigoTitulo2 = new EigeInt16(intBytes, 16);
						script.Write(script.Response, x => x.Titulo.ControlTarifa2);
						script.Write(script.Response, x => x.Titulo.CodigoTitulo2);
						if (abono)
						{
							script.Card.Titulo.TituloEnAmpliacion2 = new EigeBool(true);
							script.Write(script.Response, x => x.Titulo.TituloEnAmpliacion2);
						}
						else
						{
							script.Card.Titulo.SaldoViaje2 = new EigeInt8(transporCardRecharge.Quantity.Value);
							script.Write(script.Response, x => x.Titulo.SaldoViaje2);
						}
					}*/
				}
				#region Actualizar historico de carga y recarga.
				//Actualiza historica de carga y recarga.
				if (script.Card.Carga.PosicionUltima.Value == EigePosicionUltimaCargaEnum.Carga1)
				{
					//lo guardo en 2 y lo pongo a 1.
					script.Card.Carga.PosicionUltima = new GenericEnum<EigePosicionUltimaCargaEnum>(new byte[] { Convert.ToByte(1) }, 8);
					script.Card.Carga.TipoEquipo2 = new GenericEnum<EigeTipoEquipoCargaEnum>(new byte[] { Convert.ToByte(3) }, 8);
					//script.Card.Carga.Empresa2 = //TODO
					//script.Card.Carga.Expendedor2 =//TODO
					int aux = 0;
					if (titulo1) aux = 0;
					else if (titulo2) aux = 8;//0x01000
					//Faltaria sumarle si es carga /recarga /ampliacion/ anulacion... ver Doc
					script.Card.Carga.TipoOperacion2 = new EigeBytes(new byte[] { Convert.ToByte(aux) }, 8);
					script.Card.Carga.FormaPago2 = new GenericEnum<EigeFormaPagoEnum>(new byte[] { Convert.ToByte(5) }, 8);//movil.
					script.Card.Carga.CodigoTitulo2 = new EigeInt16(new byte[] { Convert.ToByte(transporCardRecharge.Title.Code) }, 8);
					script.Card.Carga.Importe2 = new EigeCurrency(new byte[] { Convert.ToByte(transporCardRecharge.Price) }, 8);
				}
				else
				{
					//lo guardo en 1 y lo pongo a 0.
					script.Card.Carga.PosicionUltima = new GenericEnum<EigePosicionUltimaCargaEnum>(new byte[] { Convert.ToByte(0) }, 8);
					script.Card.Carga.TipoEquipo1 = new GenericEnum<EigeTipoEquipoCargaEnum>(new byte[] { Convert.ToByte(3) }, 8);
					//script.Card.Carga.Empresa1 = //TODO
					//script.Card.Carga.Expendedor1 =//TODO
					int aux = 0;
					if (titulo1) aux = 0;
					else if (titulo2) aux = 8;//0x01000
											  //Faltaria sumarle si es carga /recarga /ampliacion/ anulacion... ver Doc
					script.Card.Carga.TipoOperacion1 = new EigeBytes(new byte[] { Convert.ToByte(aux) }, 8);
					script.Card.Carga.FormaPago1 = new GenericEnum<EigeFormaPagoEnum>(new byte[] { Convert.ToByte(5) }, 8);//movil.
					script.Card.Carga.CodigoTitulo1 = new EigeInt16(new byte[] { Convert.ToByte(transporCardRecharge.Title.Code) }, 8);
					script.Card.Carga.Importe1 = new EigeCurrency(new byte[] { Convert.ToByte(transporCardRecharge.Price) }, 8);
				}
				#endregion
				var list = new List<TransportCardRechargeFinishResult>();
				var item = new TransportCardRechargeFinishResult
				{
					Code = script.Card.Titulo.CodigoTitulo1.Value,
					//InBlackList1 = inBlackList1,
					TituloEnAmpliacion1 = script.Card.Titulo.TituloEnAmpliacion1,
					SaldoViaje1 = script.Card.Titulo.SaldoViaje1
				};
				list.Add(item);
				item = new TransportCardRechargeFinishResult
				{
					Code = script.Card.Titulo.CodigoTitulo2.Value,
					//InBlackList2 = inBlackList2,
					TituloEnAmpliacion2 = script.Card.Titulo.TituloEnAmpliacion2,
					SaldoViaje2 = script.Card.Titulo.SaldoViaje2
				};
				list.Add(item);
				return new TransportCardRechargeFinishResultBase
				{
					Data = list,
					TicketId = ticket.Id
				};

			});
		}

		private GenericEnum<EigeZonaEnum> getZones(ZoneType? zone)
		{
			var zones = (GenericEnum<EigeZonaEnum>)null;
			foreach (char c in zone.ToEnumAlias())
			{
				zones = (GenericEnum<EigeZonaEnum>)Enum.Parse(typeof(EigeZonaEnum), "Zona" + c);
			}

			return zones;
		}
		#endregion ExecuteAsync
	}
}
