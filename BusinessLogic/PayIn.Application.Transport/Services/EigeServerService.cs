using Microsoft.Practices.Unity;
using PayIn.Domain.Payments;
using PayIn.Domain.Transport;
using PayIn.Domain.Transport.Eige.Enums;
using PayIn.Infrastructure.Transport.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Xp.Domain;

namespace PayIn.Application.Transport.Services
{
    public class EigeServerService : IServerService
    {
        [Dependency] public IEntityRepository<BlackList> BlackListRepository { get; set; }
        [Dependency] public IEntityRepository<GreyList> GreyListRepository { get; set; }
        [Dependency] public SigapuntService SigapuntService { get; set; }

		#region Recharged
		public async Task<IEnumerable<ServerResult>> Recharged(TransportOperation operation, Payment payment, string mobileSerial, DateTime now)
		{
			if (operation.Ticket.Amount > operation.Price.Price)
			{
                // En caso de canje hay que enviar dos llamadas una para el canje y otra para la recarga
                var result1 = new ServerResult();
                try
                {
                    result1.Result = await SigapuntService.CARGAAsync(
                        (long)operation.Uid,
                        operation.Price.Title.OwnerCode,
                        operation.Slot.Value == EigeTituloEnUsoEnum.Titulo1 ? 1 : 2, //arguments.Slot.Value,
                        operation.RechargeType,
                        operation.TitleCodeErasedSlot1,
                        operation.TitleCodeErasedSlot2,
                        operation.Price.Title.Priority,
                        operation.Price.Title.Code,
                        operation.Price.Zone,
                        operation.Price.Title.TemporalUnit,
                        operation.DateTimeValue,
                        operation.QuantityValueOld,
                        operation.QuantityValue,
                        operation.Ticket.Amount - operation.Price.Price,
                        mobileSerial,
                        operation.Price.Version,
                        now
                    );
                }
                catch (Exception e)
                {
                    result1.Success = false;
                    result1.Exception = e.Message;
                }

                var result2 = new ServerResult();
                try
                {
                    result2.Result = await SigapuntService.CARGAAsync(
                        (long)operation.Uid,
                        operation.Price.Title.OwnerCode,
                        operation.Slot == EigeTituloEnUsoEnum.Titulo1 ? 1 : 2, //arguments.Slot.Value,
                        operation.RechargeType,
                        null,
                        null,
                        operation.Price.Title.Priority,
                        operation.Price.Title.Code,
                        operation.Price.Zone,
                        operation.Price.Title.TemporalUnit,
                        operation.DateTimeValue,
                        operation.QuantityValueOld,
                        operation.QuantityValue,
                        operation.Ticket.Amount,
                        mobileSerial,
                        operation.Price.Version,
                        now
                    );
                }
                catch (Exception e)
                {
                    result2.Success = false;
                    result2.Exception = e.Message;
                }

                return new ServerResult[] { result1, result2 };
            }
			else
            {
                var result = new ServerResult();
                try
                {
                    result.Result = await SigapuntService.CARGAAsync(
                        (long)operation.Uid,
                        operation.Price.Title.OwnerCode,
                        operation.Slot == EigeTituloEnUsoEnum.Titulo1 ? 1 : 2, //arguments.Slot.Value,
                        operation.RechargeType,
                        operation.TitleCodeErasedSlot1,
                        operation.TitleCodeErasedSlot2,
                        operation.Price.Title.Priority,
                        operation.Price.Title.Code,
                        operation.Price.Zone,
                        operation.Price.Title.TemporalUnit,
                        operation.DateTimeValue,
                        operation.QuantityValueOld,
                        operation.QuantityValue,
                        operation.Price.Title.Quantity == null ? 0 : operation.Price.Price * ((operation.QuantityValue ?? 0) - (operation.QuantityValueOld ?? 0)) / operation.Price.Title.Quantity.Value,
                        mobileSerial,
                        operation.Price.Version,
                        now
                    );
                }
                catch (Exception e)
                {
                    result.Success = false;
                    result.Exception = e.Message;
                }

                return new ServerResult[] { result  };
            }
		}
		#endregion Recharged

		#region Revoked
		public async Task<IEnumerable<ServerResult>> Revoked(TransportOperation operation, string mobileSerial, DateTime now)
        {
            // En caso de canje hay que enviar dos llamadas una para el canje y otra para la recarga
            var result = new ServerResult();
            try
            {
                result.Result = await SigapuntService.ANUCARGAAsync(
				    operation.Uid.Value,
				    1,
				    now,
				    operation.Price.Title.Code,
				    operation.Price.Zone,
				    operation.Slot == EigeTituloEnUsoEnum.Titulo1 ? 1 : 2 // arguments.Slot.Value
			    );
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Exception = e.Message;
            }

            return new ServerResult[] { result };
		}
        #endregion Revoked

        #region Refund
        public async Task<IEnumerable<ServerResult>> Refund(TransportOperation operation, Payment payment, string mobileSerial, DateTime now)
        {
            return await Task.Run(() =>
            {
                // De momento no hay refund
                return new ServerResult[] { };
            });
        }
        #endregion Refund

        #region WhiteListDownload
        public async Task<IEnumerable<WhiteListDto>> WhiteListDownload(DateTime now)
        {
            return await Task.Run(() =>
            {
                return new List<WhiteListDto>();
            });
        }
        #endregion WhiteListDownload

        #region WhiteListMark
        public async Task<string> WhiteListMark(WhiteList whiteItem, TransportOperation operation, string mobileSerial, DateTime now)
        {
            return await Task.Run(() =>
            {
                throw new NotImplementedException();
                return "";
            });
        }
        #endregion WhiteListMark

        #region WhiteListUnmark
        public async Task<string> WhiteListUnmark(WhiteList whiteItem, TransportOperation operation, string mobileSerial, DateTime now)
        {
            return await Task.Run(() =>
            {
                throw new NotImplementedException();
                return "";
            });
        }
        #endregion WhiteListUnmark

        #region GreyListDownload
        public async Task<IEnumerable<GreyListDto>> GreyListDownload(DateTime now)
        {
            return await Task.Run(() =>
            {
                var request = WebRequest.Create("ftp://193.144.127.124/LG/LG.TXT");
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential("listaPayIn", "p4y1n4231");

                // Load from server
                var loadedServer = new List<GreyListDto>();
                using (var response = request.GetResponse())
                {
                    using (var responseStream = response.GetResponseStream())
                    {
                        using (var file = new StreamReader(responseStream))
                        {
                            var line = file.ReadLine();
                            while (line != null)
                            {
                                var fields = line.Split(';');
                                var item = new GreyListDto
                                {
                                    Uid = long.Parse(fields[0]),
                                    OperationNumber = int.Parse(fields[1]),
                                    RegistrationDate = Convert.ToDateTime(fields[2]),
                                    Action = (GreyList.ActionType)int.Parse(fields[3]),
                                    Field = fields[4],
                                    NewValue = fields[5],
                                    Resolved = fields[6] == "1" ? true : false,
                                    ResolutionDate = fields[7] == "" ? (DateTime?)null : Convert.ToDateTime(fields[7]),
                                    Machine = (GreyList.MachineType)int.Parse(fields[8]),
                                    Source = GreyList.GreyListSourceType.SigAPunt,
                                    State = GreyList.GreyListStateType.Active
                                };
                                loadedServer.Add(item);

                                line = file.ReadLine();
                            }
                        }
                    }
                }

                return loadedServer;
            });
        }
        #endregion GreyListDownload

        #region GreyListMark
        public async Task<string> GreyListMark(GreyList greyItem, TransportOperation operation, string mobileSerial, DateTime now)
		{
			return await SigapuntService.MARCALGAsync(greyItem.Uid, greyItem.NewValue, greyItem.Machine, greyItem.Field, greyItem.OldValue.FromHexadecimal());
		}
		#endregion GreyListMark

		#region GreyListUnmark
		public async Task<string> GreyListUnmark(TransportOperation operation, string mobileSerial, DateTime now)
		{
			return await SigapuntService.BLALGAsync(operation.Uid.Value);
		}
        #endregion GreyListUnmark

        #region BlackListDownload
        public async Task<IEnumerable<BlackListDto>> BlackListDownload(DateTime now)
        {
            return await Task.Run(() =>
            {
                var request = WebRequest.Create("ftp://193.144.127.124/LN/LN.TXT");
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential("listaPayIn", "p4y1n4231");

                // Load from server
                var loadedServer = new List<BlackListDto>();
                using (var response = request.GetResponse())
                {
                    using (var responseStream = response.GetResponseStream())
                    {
                        using (var file = new StreamReader(responseStream))
                        {
                            var line = file.ReadLine();
                            while (line != null)
                            {
                                var fields = line.Split(';');
                                var item = new BlackListDto
                                {
                                    Uid = long.Parse(fields[0]),
                                    RegistrationDate = Convert.ToDateTime(fields[1]),
                                    Machine = (BlackListMachineType)int.Parse(fields[2]),
                                    Resolved = fields[3] == "1" ? true : false,
                                    ResolutionDate = fields[4] == "" ? (DateTime?)null : Convert.ToDateTime(fields[4]),
                                    Rejection = fields[5] == "1" ? true : false,
                                    Concession = int.Parse(fields[6]),
                                    Service = (BlackListServiceType)int.Parse(fields[7]),
                                    Source = BlackList.BlackListSourceType.SigAPunt,
                                    State = BlackList.BlackListStateType.Active
                                };
                                loadedServer.Add(item);

                                line = file.ReadLine();
                            }
                        }
                    }
                }

                return loadedServer;
            });
        }
        #endregion BlackListDownload

        #region BlackListMark
        public async Task<string> BlackListMark(BlackList greyItem, TransportOperation operation, string mobileSerial, DateTime now)
		{
			return await SigapuntService.MARCALNAsync(greyItem.Uid, 1);
		}
		#endregion BlackListMark

		#region BlackListUnmark
		public async Task<string> BlackListUnmark(TransportOperation operation, string mobileSerial, DateTime now)
		{
			return await SigapuntService.BLALNAsync(operation.Uid.Value, 1);
		}
        #endregion BlackListUnmark
    }
}
