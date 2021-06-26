using Newtonsoft.Json.Linq;
using PayIn.Application.Dto.Results;
using PayIn.Application.Dto.Transport.Arguments.TransportCard;
using PayIn.Application.Dto.Transport.Arguments.TransportOperation;
using PayIn.Application.Dto.Transport.Results.TransportOperation;
using PayIn.Application.Payments.Handlers;
using PayIn.Application.Transport.Handlers;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Transport;
using PayIn.Domain.Transport.MifareClassic.Operations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Application.Dto.Tsm.Arguments;
using Xp.Application.Dto.Tsm.GlobalPlatform;
using Xp.Application.Dto.Tsm.GlobalPlatform.States;
using Xp.Application.Dto.Tsm.Mifare4Mobile;
using Xp.Application.Hsm;
using Xp.Application.Results;
using Xp.Application.Tsm.GlobalPlatform.Config;
using Xp.Application.Tsm.GlobalPlatform.Handlers;
using Xp.Application.Tsm.Mifare4Mobile.Commands;
using Xp.Domain.Transport;
using Xp.Domain.Transport.MifareClassic;

namespace Xp.Application.Tsm.Handlers
{
	public class TsmExecuteHandler : IServiceBaseHandler<TsmExecuteArguments>
	{
		private byte[] SdAid = "A0 00 00 03 96 4D 34 4D A4 00 81 DB 69 01 00 01".FromHexadecimal();
		private byte[] Aid = "A0 00 00 03 96 4D 34 4D 24 00 81 DB 69 02 00 01".FromHexadecimal();

		protected const byte KeyId = 0x00;
		protected const ScpMode Scp = ScpMode.SCP_03_6D;
		protected const SecLevel SecurityLevel = SecLevel.C_ENC_AND_R_ENC_AND_C_MAC_AND_R_MAC;
		
		protected List<ISCKey> Keys { get; set; } = new List<ISCKey> {
			new SCGPKey(0x48, 0x01, KeyType.AES_CBC, "60 61 62 63 64 65 66 67 68 69 6A 6B 6C 6D 6E 6F".FromHexadecimal()),
			new SCGPKey(0x48, 0x02, KeyType.AES_CBC, "60 61 62 63 64 65 66 67 68 69 6A 6B 6C 6D 6E 6F".FromHexadecimal()),
			new SCGPKey(0x48, 0x03, KeyType.AES_CBC, "60 61 62 63 64 65 66 67 68 69 6A 6B 6C 6D 6E 6F".FromHexadecimal())
		};

		private byte[] GetSdAid(string cardRandomId)
		{
			SdAid[SdAid.Length - 1] = (cardRandomId.FromHexadecimal())[0];
			return SdAid;
		}
		private byte[] GetAid(string cardRandomId)
		{
			Aid[Aid.Length - 1] = (cardRandomId.FromHexadecimal())[0];
			return Aid;
		}

		private TsmSelectHandler SelectHandler { get; set; }
		private TsmInitializeUpdateHandler InitializeUpdateHandler { get; set; }
		private TsmExternalAuthenticateHandler ExternalAuthenticateHandler { get; set; }
		private TsmInstallForPersoHandler InstallForPersoHandler { get; set; }
		private TsmStoreDataHandler StoreDataHandler { get; set; }
		private IMifare4MobileHsm Hsm { get; set; }
		private ISessionData SessionData { get; set; }
		private TransportCardCreateHandler TransportCardCreateHandler { get; set; }
		private TransportOperationGetReadInfoHandler GetReadInfoHandler { get; set; }
        private TransportOperationReadInfoHandler ReadInfoHandler { get; set; }
		private TransportOperationConfirmHandler ConfirmHandler { get; set; }
		private TransportOperationConfirmAndReadInfoHandler ConfirmAndReadInfoHandler { get; set; }
		private TransportOperationPersonalizeHandler PersonalizeHandler { get; set; }
		private MobileTicketCreateAndGetHandler TicketMobileCreateAndGetHandler { get; set; }

		#region Constructors
		public TsmExecuteHandler(
			IMifare4MobileHsm hsm,
			ISessionData sessionData,
			TransportCardCreateHandler transportCardCreateHandler,
			TransportOperationGetReadInfoHandler getReadInfoHandler,
			TransportOperationReadInfoHandler readInfoHandler,
			TransportOperationConfirmHandler confirmHandler,
			TransportOperationConfirmAndReadInfoHandler confirmAndReadInfoHandler,
			TransportOperationPersonalizeHandler personalizeHandler,
			MobileTicketCreateAndGetHandler ticketMobileCreateAndGetHandler
		)
		{
			StoreDataHandler = new TsmStoreDataHandler();
			InstallForPersoHandler = new TsmInstallForPersoHandler(StoreDataHandler);
			ExternalAuthenticateHandler = new TsmExternalAuthenticateHandler(InstallForPersoHandler);
			InitializeUpdateHandler = new TsmInitializeUpdateHandler(ExternalAuthenticateHandler);
			SelectHandler = new TsmSelectHandler(InitializeUpdateHandler);

			StoreDataHandler.InstallForPersoHandler = InstallForPersoHandler;

			Hsm = hsm;
			SessionData = sessionData;
			TransportCardCreateHandler = transportCardCreateHandler;
			GetReadInfoHandler = getReadInfoHandler;
            ReadInfoHandler = readInfoHandler;
			ConfirmHandler = confirmHandler;
			ConfirmAndReadInfoHandler = confirmAndReadInfoHandler;
			PersonalizeHandler = personalizeHandler;
			TicketMobileCreateAndGetHandler = ticketMobileCreateAndGetHandler;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(TsmExecuteArguments arguments)
		{
			Debug.WriteLine("A >> " + arguments.Data);

			if (arguments.Type == TsmExecuteArguments.OperationType.Personalize)
				return await PersonalizeAsync(arguments);
			if (arguments.Type == TsmExecuteArguments.OperationType.Read)
				return await ReadAsync(arguments);
			if (arguments.Type == TsmExecuteArguments.OperationType.Execute)
				return await ExecuteCardAsync(arguments);

			throw new NotImplementedException();
		}
		#endregion ExecuteAsync

		#region GetPersonalizeScriptAsync
		private async Task<List<MifareOperationResultArguments>> GetPersonalizeScriptAsync(long uid)
		{
			return await Task.Run(() =>
			{
				var script = new List<MifareOperationResultArguments>
			{
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 0,
					Block = 1,
					Data = "000000000000000000000000000000B1"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 0,
					Block = 2,
					Data = "000000000000000000000000000000B1"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 1,
					Block = 0,
					Data = "3110C4D200000000002000000000407A"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 1,
					Block = 1,
					Data = "010000000000000000000000000000D3"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 1,
					Block = 2,
					Data = "010000000A000000000000000000004C"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 2,
					Block = 0,
					Data = "000000000000000000000000000000B1"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 2,
					Block = 1,
					Data = "20202020202020202020202020200048"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 2,
					Block = 2,
					Data = "808080808000030000000000000018A4"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 3,
					Block = 0,
					Data = "000000000000000000000000000000B1"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 3,
					Block = 1,
					Data = "0A77E82A0D000000000000000000008D"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 3,
					Block = 2,
					Data = "2020202020202020202020202020206B"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 4,
					Block = 0,
					Data = "000000000000000000000000000000B1"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 4,
					Block = 1,
					Data = "000000000000000000000000000000B1"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 4,
					Block = 2,
					Data = "000000000000000000000000000000B1"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 5,
					Block = 0,
					Data = "00000000FFFFFFFF0000000000FF00FF"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 5,
					Block = 1,
					Data = "00000000FFFFFFFF0000000000FF00FF"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 5,
					Block = 2,
					Data = "000000000000000000000000000000B1"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 6,
					Block = 0,
					Data = "00000000FFFFFFFF0000000000FF00FF"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 6,
					Block = 1,
					Data = "00000000FFFFFFFF0000000000FF00FF"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 6,
					Block = 2,
					Data = "000000000000000000000000000000B1"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 7,
					Block = 0,
					Data = "000000000000000000000000000000B1"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 7,
					Block = 1,
					Data = "000000000000000000000000000000B1"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 7,
					Block = 2,
					Data = "000000000000000000000000000000B1"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 8,
					Block = 0,
					Data = "000000000000000000000000000000B1"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 8,
					Block = 1,
					Data = "000000000000000000000000000000B1"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 8,
					Block = 2,
					Data = "000000000000000000000000000000B1"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 9,
					Block = 0,
					Data = "000000000000000000000000000000B1"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 9,
					Block = 1,
					Data = "000000000000000000000000000000B1"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 9,
					Block = 2,
					Data = "000000000000000000000000000000B1"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 10,
					Block = 0,
					Data = "000000000000000000000000000000B1"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 10,
					Block = 1,
					Data = "000000000000000000000000000000B1"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 10,
					Block = 2,
					Data = "000000000000000000000000000000B1"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 11,
					Block = 0,
					Data = "000000000000000000000000000000B1"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 11,
					Block = 1,
					Data = "000000000000000000000000000000B1"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 11,
					Block = 2,
					Data = "000000000000000000000000000000B1"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 12,
					Block = 0,
					Data = "000000000000000000000000000000B1"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 12,
					Block = 1,
					Data = "000000000000000000000000000000B1"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 12,
					Block = 2,
					Data = "000000000000000000000000000000B1"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 13,
					Block = 0,
					Data = "000000000000000000000000000000B1"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 13,
					Block = 1,
					Data = "000000000000000000000000000000B1"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 13,
					Block = 2,
					Data = "000000000000000000000000000000B1"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 14,
					Block = 0,
					Data = "000000000000000000000000000000B1"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 14,
					Block = 1,
					Data = "000000000000000000000000000000B1"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 14,
					Block = 2,
					Data = "000000000000000000000000000000B1"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 15,
					Block = 0,
					Data = "000000000000000000000000000000B1"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 15,
					Block = 1,
					Data = "000000000000000000000000000000B1"
				},
				new MifareOperationResultArguments
				{
					Operation = MifareOperationType.ReadBlock,
					Sector = 15,
					Block = 2,
					Data = "000000000000000000000000000000B1"
				}
			};
				return script;



				//return new List<byte[]>
				//{
				//	(await (
				//		new M4mUpdateSmMetadataCommand()
				//	).ToHexadecimalAsync()).FromHexadecimal(),
				//	(await (
				//		new M4mAddAndUpdateMdacCommand()
				//	).ToHexadecimalAsync()).FromHexadecimal(),
				//	(await (
				//		new M4mUpdateMifareClassicSectorCommand(Hsm, 0)
				//		{
				//			Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
				//			Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
				//			AccessConditions = "FF078069".FromHexadecimal()
				//		}
				//	).ToHexadecimalAsync()).FromHexadecimal(),
				//	(await (
				//		new M4mUpdateMifareClassicSectorCommand(Hsm, 1)
				//		{
				//			//Block0 = "3110C0D20000000000200000000040D8".FromHexadecimal(),
				//			Block0 =   "3110C4D200000000002000000000407A".FromHexadecimal(),
				//			//Block1 = "010000000A000000000000000000004C".FromHexadecimal(),
				//			Block1 = "010000000000000000000000000000D3".FromHexadecimal(),
				//			Block2 = "010000000A000000000000000000004C".FromHexadecimal(),
				//			AccessConditions = "7E178869".FromHexadecimal()
				//		}
				//	).ToHexadecimalAsync()).FromHexadecimal(),
				//	(await (
				//		new M4mUpdateMifareClassicSectorCommand(Hsm, 2)
				//		{
				//			//Block0 = "0000000018A83083559A00842D010070".FromHexadecimal(),
				//			Block0 = "000000000000000000000000000000B1".FromHexadecimal(),
				//			Block1 = "20202020202020202020202020200048".FromHexadecimal(),
				//			Block2 = "808080808000030000000000000018A4".FromHexadecimal(),
				//			AccessConditions = "78778869".FromHexadecimal()
				//		}
				//	).ToHexadecimalAsync()).FromHexadecimal(),
				//	(await (
				//		new M4mUpdateMifareClassicSectorCommand(Hsm, 3)
				//		{
				//			//Block0 = "01C200A0074000000020FCDFDF04004D".FromHexadecimal(),
				//			Block0 = "000000000000000000000000000000B1".FromHexadecimal(),
				//			Block1 = "0A77E82A0D000000000000000000008D".FromHexadecimal(),
				//			Block2 = "2020202020202020202020202020206B".FromHexadecimal(),
				//			AccessConditions = "78778869".FromHexadecimal()
				//		}
				//	).ToHexadecimalAsync()).FromHexadecimal(),
				//	(await (
				//		new M4mUpdateMifareClassicSectorCommand(Hsm, 4)
				//		{
				//			Block0 = "000000000000000000000000000000B1".FromHexadecimal(),
				//			Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
				//			Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
				//			AccessConditions = "7F078869".FromHexadecimal()
				//		}
				//	).ToHexadecimalAsync()).FromHexadecimal(),
				//	(await (
				//		new M4mUpdateMifareClassicSectorCommand(Hsm, 5)
				//		{
				//			Block0 = "00000000FFFFFFFF0000000000FF00FF".FromHexadecimal(),
				//			Block1 = "00000000FFFFFFFF0000000000FF00FF".FromHexadecimal(),
				//			Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
				//			AccessConditions = "7F078869".FromHexadecimal()
				//		}
				//	).ToHexadecimalAsync()).FromHexadecimal(),
				//	(await (
				//		new M4mUpdateMifareClassicSectorCommand(Hsm, 6)
				//		{
				//			Block0 = "00000000FFFFFFFF0000000000FF00FF".FromHexadecimal(),
				//			Block1 = "00000000FFFFFFFF0000000000FF00FF".FromHexadecimal(),
				//			Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
				//			AccessConditions = "4C378B69".FromHexadecimal()
				//		}
				//	).ToHexadecimalAsync()).FromHexadecimal(),
				//	(await (
				//		new M4mUpdateMifareClassicSectorCommand(Hsm, 7)
				//		{
				//			Block0 = "000000000000000000000000000000B1".FromHexadecimal(),
				//			Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
				//			Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
				//			AccessConditions = "7F078869".FromHexadecimal()
				//		}
				//	).ToHexadecimalAsync()).FromHexadecimal(),
				//	(await (
				//		new M4mUpdateMifareClassicSectorCommand(Hsm, 8)
				//		{
				//			Block0 = "000000000000000000000000000000B1".FromHexadecimal(),
				//			Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
				//			Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
				//			AccessConditions = "78778869".FromHexadecimal()
				//		}
				//	).ToHexadecimalAsync()).FromHexadecimal(),
				//	(await (
				//		new M4mUpdateMifareClassicSectorCommand(Hsm, 9)
				//		{
				//			Block0 = "000000000000000000000000000000B1".FromHexadecimal(),
				//			Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
				//			Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
				//			AccessConditions = "FF078069".FromHexadecimal()
				//		}
				//	).ToHexadecimalAsync()).FromHexadecimal(),
				//	(await (
				//		new M4mUpdateMifareClassicSectorCommand(Hsm, 10)
				//		{
				//			Block0 = "000000000000000000000000000000B1".FromHexadecimal(),
				//			Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
				//			Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
				//			AccessConditions = "FF078069".FromHexadecimal()
				//		}
				//	).ToHexadecimalAsync()).FromHexadecimal(),
				//	(await (
				//		new M4mUpdateMifareClassicSectorCommand(Hsm, 11)
				//		{
				//			Block0 = "000000000000000000000000000000B1".FromHexadecimal(),
				//			Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
				//			Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
				//			AccessConditions = "FF078069".FromHexadecimal()
				//		}
				//	).ToHexadecimalAsync()).FromHexadecimal(),
				//	(await (
				//		new M4mUpdateMifareClassicSectorCommand(Hsm, 12)
				//		{
				//			Block0 = "000000000000000000000000000000B1".FromHexadecimal(),
				//			Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
				//			Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
				//			AccessConditions = "FF078069".FromHexadecimal()
				//		}
				//	).ToHexadecimalAsync()).FromHexadecimal(),
				//	(await (
				//		new M4mUpdateMifareClassicSectorCommand(Hsm, 13)
				//		{
				//			Block0 = "000000000000000000000000000000B1".FromHexadecimal(),
				//			Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
				//			Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
				//			AccessConditions = "FF078069".FromHexadecimal()
				//		}
				//	).ToHexadecimalAsync()).FromHexadecimal(),
				//	(await (
				//		new M4mUpdateMifareClassicSectorCommand(Hsm, 14)
				//		{
				//			Block0 = "000000000000000000000000000000B1".FromHexadecimal(),
				//			Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
				//			Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
				//			AccessConditions = "FF078069".FromHexadecimal()
				//		}
				//	).ToHexadecimalAsync()).FromHexadecimal(),
				//	(await (
				//		new M4mUpdateMifareClassicSectorCommand(Hsm, 15)
				//		{
				//			Block0 = "000000000000000000000000000000B1".FromHexadecimal(),
				//			Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
				//			Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
				//			AccessConditions = "FF078069".FromHexadecimal()
				//		}
				//	).ToHexadecimalAsync()).FromHexadecimal()
				//};
			});
		}
		#endregion GetPersonalizeScriptAsync

		#region ConvertFromScriptAsync
		private async Task<List<byte[]>> ConvertFromScriptAsync(ScriptResult script)
        {
            var result = new List<byte[]>
            {
                (await (
                    new M4mUpdateSmMetadataCommand()
                ).ToHexadecimalAsync()).FromHexadecimal(),
                (await (
                    new M4mAddAndUpdateMdacCommand()
                ).ToHexadecimalAsync()).FromHexadecimal()
            };

            var commands = new List<M4mCommand>();
            foreach (var operation in script.Script)
            {
                if (operation.Operation == MifareOperationType.Autenticate) { }
                else if (operation.Operation == MifareOperationType.Autenticate) { }
                else if (operation.Operation == MifareOperationType.Check) { } // TODO: XAVI Falta crear la operacion
                else if (operation.Operation == MifareOperationType.ReadBlock)
                {
                    var customOperation = operation as MifareReadOperation;
                    var blockBitmap =
                        (customOperation.Block == 0 ? BlockBitmap.Block0 : default(BlockBitmap)) |
                        (customOperation.Block == 1 ? BlockBitmap.Block1 : default(BlockBitmap)) |
                        (customOperation.Block == 2 ? BlockBitmap.Block2 : default(BlockBitmap)) |
                        (customOperation.Block == 3 ? BlockBitmap.Block3 : default(BlockBitmap));

                    var command = new M4mReadMifareClassicSectorCommand(Hsm, customOperation.Sector, blockBitmap);
                    commands.Add(command);
                }
                else if (operation.Operation == MifareOperationType.WriteBlock)
                {
                    var customOperation = operation as MifareWriteOperation;
                    var command = new M4mUpdateMifareClassicSectorCommand(Hsm, customOperation.Sector);
                    if (customOperation.Block == 0)
                        command.Block0 = customOperation.Data.FromHexadecimal();
                    if (customOperation.Block == 1)
                        command.Block1 = customOperation.Data.FromHexadecimal();
                    if (customOperation.Block == 2)
                        command.Block2 = customOperation.Data.FromHexadecimal();
					// El block 4 simula el sector trailer
					if (customOperation.Block == 4)
						command.AccessConditions = customOperation.Data.FromHexadecimal();

					// TODO: XAVI Falta tener en cuenta el cambio de claves
					commands.Add(command);
                }
                else { }
            }

			// Optimizando
			M4mCommand previousOperation = null;
			foreach (var operation in commands)
			{
				// TODO: XAVI Falta optimizar
				var updateOperation = operation as M4mUpdateMifareClassicSectorCommand;
				var readOperation = operation as M4mReadMifareClassicSectorCommand;

				var updatePreviousOperation = previousOperation as M4mUpdateMifareClassicSectorCommand;
				var readPreviousOperation = previousOperation as M4mReadMifareClassicSectorCommand;

				if ((updateOperation != null) && (updatePreviousOperation != null) && (updateOperation.SectorNumber == updatePreviousOperation.SectorNumber))
				{
					// Update
					if (updateOperation.Block0 != null)
						updatePreviousOperation.Block0 = updateOperation.Block0;
					if (updateOperation.Block1 != null)
						updatePreviousOperation.Block1 = updateOperation.Block1;
					if (updateOperation.Block2 != null)
						updatePreviousOperation.Block2 = updateOperation.Block2;
					if (updateOperation.AccessConditions != null)
						updatePreviousOperation.AccessConditions = updateOperation.AccessConditions;
				}
				else if ((readOperation != null) && (readPreviousOperation != null) && (readOperation.SectorNumber == readPreviousOperation.SectorNumber))
				{
					// Read
					readPreviousOperation.BlockBitmap |= readOperation.BlockBitmap;
				}
				else
				{
					if (previousOperation != null)
					{
						var command = (await previousOperation.ToHexadecimalAsync()).FromHexadecimal();
						result.Add(command);
					}
					previousOperation = operation;
				}
			}
			{
				if (previousOperation != null)
				{
					var command = (await previousOperation.ToHexadecimalAsync()).FromHexadecimal();
					result.Add(command);
				}
			}

			return result;
		}
		private async Task<List<byte[]>> ConvertFromScriptAsync(JArray script)
		{
			var result = new List<byte[]>
			{
				(await (
					new M4mUpdateSmMetadataCommand()
				).ToHexadecimalAsync()).FromHexadecimal(),
				(await (
					new M4mAddAndUpdateMdacCommand()
				).ToHexadecimalAsync()).FromHexadecimal()
			};

			var commands = new List<M4mCommand>();
			foreach (JObject op in script)
			{
				var operation = (MifareOperationType) ((int)op["operation"]);
				if (operation == MifareOperationType.Autenticate) { }
				else if (operation == MifareOperationType.Autenticate) { }
				else if (operation == MifareOperationType.Check) { } // TODO: XAVI Falta crear la operacion
				else if (operation == MifareOperationType.ReadBlock)
				{
					var customOperation = new MifareReadOperation
					{
						Block = (byte)op["block"],
						Data = (string)op["data"],
						Operation = operation,
						Sector = (byte)op["sector"]
					};
					var blockBitmap =
						(customOperation.Block == 0 ? BlockBitmap.Block0 : default(BlockBitmap)) |
						(customOperation.Block == 1 ? BlockBitmap.Block1 : default(BlockBitmap)) |
						(customOperation.Block == 2 ? BlockBitmap.Block2 : default(BlockBitmap)) |
						(customOperation.Block == 3 ? BlockBitmap.Block3 : default(BlockBitmap));

					var command = new M4mReadMifareClassicSectorCommand(Hsm, customOperation.Sector, blockBitmap);
					commands.Add(command);
				}
				else if (operation == MifareOperationType.WriteBlock)
				{
					var customOperation = new MifareWriteOperation
					{
						Block = (byte)op["block"],
						Data = (string)op["data"],
						Operation = operation,
						Sector = (byte)op["sector"]
					};
					var command = new M4mUpdateMifareClassicSectorCommand(Hsm, customOperation.Sector);
					if (customOperation.Block == 0)
						command.Block0 = customOperation.Data.FromHexadecimal();
					if (customOperation.Block == 1)
						command.Block1 = customOperation.Data.FromHexadecimal();
					if (customOperation.Block == 2)
						command.Block2 = customOperation.Data.FromHexadecimal();

					// TODO: XAVI Falta tener en cuenta el cambio de claves
					commands.Add(command);
				}
				else { }
			}

			// Optimizando
			M4mCommand previousOperation = null;
			foreach (var operation in commands)
			{
				// TODO: XAVI Falta optimizar
				var updateOperation = operation as M4mUpdateMifareClassicSectorCommand;
				var readOperation = operation as M4mReadMifareClassicSectorCommand;

				var updatePreviousOperation = previousOperation as M4mUpdateMifareClassicSectorCommand;
				var readPreviousOperation = previousOperation as M4mReadMifareClassicSectorCommand;

				if ((updateOperation != null) && (updatePreviousOperation != null) && (updateOperation.SectorNumber == updatePreviousOperation.SectorNumber))
				{
					// Update
					if (updateOperation.Block0 != null)
						updatePreviousOperation.Block0 = updateOperation.Block0;
					if (updateOperation.Block1 != null)
						updatePreviousOperation.Block1 = updateOperation.Block1;
					if (updateOperation.Block2 != null)
						updatePreviousOperation.Block2 = updateOperation.Block2;
					if (updateOperation.AccessConditions != null)
						updatePreviousOperation.AccessConditions = updateOperation.AccessConditions;
				}
				else if ((readOperation != null) && (readPreviousOperation != null) && (readOperation.SectorNumber == readPreviousOperation.SectorNumber))
				{
					// Read
					readPreviousOperation.BlockBitmap |= readOperation.BlockBitmap;
				}
				else
				{
					if (previousOperation != null)
					{
						var command = (await previousOperation.ToHexadecimalAsync()).FromHexadecimal();
						result.Add(command);
					}
					previousOperation = operation;
				}
			}

			{
				if (previousOperation != null)
				{
					var command = (await previousOperation.ToHexadecimalAsync()).FromHexadecimal();
					result.Add(command);
				}
			}

			return result;
		}
		#endregion ConvertFromScriptAsync

		#region ConvertToScriptAsync
		private async Task<List<MifareOperationResultArguments>> ConvertToScriptAsync(TsmState state)
		{
			return await Task.Run(() =>
			{
				var script = new List<MifareOperationResultArguments>();
				for (var sectorNumber = 0; sectorNumber < state.Card.Length; sectorNumber++)
				{
					var sector = state.Card[sectorNumber];
					for (var blockNumber = 0; blockNumber < sector.Length; blockNumber++)
					{
						var block = sector[blockNumber];
						if (block == null)
							continue;

						var operation = new MifareOperationResultArguments()
						{
							Operation = MifareOperationType.ReadBlock,
							Sector = (byte)sectorNumber,
							Block = (byte)blockNumber,
							Data = block
						};
						script.Add(operation);
					}
				}

				return script;
			});
		}
		#endregion ConvertToScriptAsync

		#region ConvertToScript2Async
		private async Task<List<IMifareOperation>> ConvertToScript2Async(TsmState state)
		{
			return await Task.Run(() =>
			{
				var script = new List<IMifareOperation>();
				for (var sectorNumber = 0; sectorNumber < state.Card.Length; sectorNumber++)
				{
					var sector = state.Card[sectorNumber];
					for (var blockNumber = 0; blockNumber < sector.Length; blockNumber++)
					{
						var block = sector[blockNumber];
						if (block == null)
							continue;

						var operation = new MifareReadOperation()
						{
							Operation = MifareOperationType.ReadBlock,
							Sector = (byte)sectorNumber,
							Block = (byte)blockNumber,
							Data = block
						};
						script.Add(operation);
					}
				}

				return script;
			});
		}
		#endregion ConvertToScript2Async
		
		#region PersonalizeAsync
		public async Task<ResultBase<TsmExecuteArguments>> PersonalizeAsync(TsmExecuteArguments arguments)
		{
			var now = DateTime.UtcNow;

			TsmExecuteArguments result = null;
			if (arguments.State == null)
				Debug.WriteLine("A >> " + arguments.System.ToString() + " - " + arguments.Type.ToString());
			else
				Debug.WriteLine("A >> " + arguments.Data);

			if (arguments.State == null)
			{
				var script = await GetPersonalizeScriptAsync(arguments.CardUid);
				var resultPersonalize = (await PersonalizeHandler.ExecuteAsync(new TransportOperationPersonalizeArguments(
					CardType.MIFAREClassic,
					arguments.CardUid.ToHexadecimalBE(), // cardId
					script.ToArray(),
					null, //code,
					1, //quantity,
					arguments.PriceId,
					arguments.TicketId.Value,
					RechargeType.Charge,
					new List<TransportOperationPersonalizeArguments_Promotion>(),
					"", //deviceManufacturer,
					"", //deviceModel,
					"", //deviceName,
					"", //deviceSerial,
					"", //deviceId,
					"", //deviceOperator,
					"", //deviceImei,
					arguments.DeviceAddress,
					"", //operatorSim,
					"" //operatorMobile
#if DEBUG || TEST || HOMO
					, now
#endif
				))) as ScriptResultBase<TransportOperationGetReadInfoResult>;

				var resultScript = new List<IMifareOperation>();
				foreach (var item in script)
				{
					var item2 = resultPersonalize.Scripts.FirstOrDefault().Script
						.Where(x => x.Operation == MifareOperationType.WriteBlock)
						.Cast<MifareWriteOperation>()
						.Where(x =>
						   x.Sector == item.Sector &&
						   x.Block == item.Block
						).LastOrDefault();
					
					if (item2 == null)
						item2 = new MifareWriteOperation
						{
							Operation = MifareOperationType.WriteBlock,
							Sector = item.Sector,
							Block = item.Block,
							Data = item.Data
						};
					resultScript.Add(item2);

					// Access conditions
					if (item2.Block == 2)
					{
						// El block 4 simula el sector trailer
						if ((item2.Sector == 0) || (item2.Sector == 9) || (item2.Sector == 10) || (item2.Sector == 11) || (item2.Sector == 12) || (item2.Sector == 13) || (item2.Sector == 14) || (item2.Sector == 15))
						{
							item2 = new MifareWriteOperation
							{
								Operation = MifareOperationType.WriteBlock,
								Sector = item.Sector,
								Block = 4,
								Data = "FF078069"
							};
						}
						else if(item2.Sector == 1)
						{
							item2 = new MifareWriteOperation
							{
								Operation = MifareOperationType.WriteBlock,
								Sector = item.Sector,
								Block = 4,
								Data = "7E178869"
							};
						}
						else if ((item2.Sector == 2) || (item2.Sector == 3) || (item2.Sector == 8))
						{
							item2 = new MifareWriteOperation
							{
								Operation = MifareOperationType.WriteBlock,
								Sector = item.Sector,
								Block = 4,
								Data = "78778869"
							};
						}
						else if ((item2.Sector == 4) || (item2.Sector == 5) || (item2.Sector == 7))
						{
							item2 = new MifareWriteOperation
							{
								Operation = MifareOperationType.WriteBlock,
								Sector = item.Sector,
								Block = 4,
								Data = "7F078869"
							};
						}
						else if ((item2.Sector == 6))
						{
							item2 = new MifareWriteOperation
							{
								Operation = MifareOperationType.WriteBlock,
								Sector = item.Sector,
								Block = 4,
								Data = "4C378B69"
							};
						}

						resultScript.Add(item2);
					}
				}

				var state = new TsmState
				{
					Script = await ConvertFromScriptAsync(new ScriptResult()
					{
						Card = resultPersonalize.Scripts.FirstOrDefault().Card,
						Keys = resultPersonalize.Scripts.FirstOrDefault().Keys,
						Script = resultScript
					}),
					CardRandomId = arguments.CardRandomId
				};

				result = await SelectHandler.CreateAsync(arguments, state, GetSdAid(state.CardRandomId));
				//result = await InitializeUpdateHandler.CreateAsync(result, state, KeyId);

				Debug.WriteLine("A << " + result.Data);
				return new ResultBase<TsmExecuteArguments> { Data = new List<TsmExecuteArguments> { result } };
			}
			else
			{
				var state = arguments.State.FromJson<TsmState>();

				if (arguments.NextStep == NextStepEnum.Select)
				{
					state = await SelectHandler.ExecuteAsync(arguments, state, GetSdAid(arguments.CardRandomId));
					arguments.State = state.ToJson();
					if (arguments.Data.Count() == 0)
					{
						result = await InitializeUpdateHandler.CreateAsync(arguments, state, KeyId);

						Debug.WriteLine("A << " + result.Data);
						return new ResultBase<TsmExecuteArguments> { Data = new List<TsmExecuteArguments> { result } };
					}
					arguments.NextStep = NextStepEnum.InitializeUpdate;
				}
				if (arguments.NextStep == NextStepEnum.InitializeUpdate)
				{
					state = await InitializeUpdateHandler.ExecuteAsync(arguments, state, KeyId, Keys, Scp);
					arguments.State = state.ToJson();
					if (arguments.Data.Count() == 0)
					{
						result = await ExternalAuthenticateHandler.CreateAsync(arguments, state, SecurityLevel, Scp, state.HostCrypto);

						Debug.WriteLine("A << " + result.Data);
						return new ResultBase<TsmExecuteArguments> { Data = new List<TsmExecuteArguments> { result } };
					}
					arguments.NextStep = NextStepEnum.ExternalAuthenticate;
				}
				if (arguments.NextStep == NextStepEnum.ExternalAuthenticate)
				{
					state = await ExternalAuthenticateHandler.ExecuteAsync(arguments, state, GetAid(arguments.CardRandomId), SecurityLevel, Scp);
					arguments.State = state.ToJson();
					if (arguments.Data.Count() == 0)
					{
						result = await InstallForPersoHandler.CreateAsync(arguments, state, GetAid(arguments.CardRandomId), SecurityLevel, Scp);

						Debug.WriteLine("A << " + result.Data);
						return new ResultBase<TsmExecuteArguments> { Data = new List<TsmExecuteArguments> { result } };
					}
					arguments.NextStep = NextStepEnum.InstallForPerso;
				}
				if (arguments.NextStep == NextStepEnum.InstallForPerso)
				{
					state = await InstallForPersoHandler.ExecuteAsync(arguments, state, SecurityLevel, Scp);
					arguments.State = state.ToJson();
					if (arguments.Data.Count() == 0)
					{
						result = await StoreDataHandler.CreateAsync(arguments, state, SecurityLevel, Scp);

						Debug.WriteLine("A << " + result.Data);
						return new ResultBase<TsmExecuteArguments> { Data = new List<TsmExecuteArguments> { result } };
					}
					arguments.NextStep = NextStepEnum.StoreData;
				}
				if (arguments.NextStep == NextStepEnum.StoreData)
				{
					state = await StoreDataHandler.ExecuteAsync(arguments, state, GetAid(arguments.CardRandomId), SecurityLevel, Scp);
					arguments.State = state.ToJson();
					if (arguments.Data.Count() == 0)
					{
						if (state.Script.Count() > 0)
						{
							result = await InstallForPersoHandler.CreateAsync(arguments, state, GetAid(arguments.CardRandomId), SecurityLevel, Scp);

							Debug.WriteLine("A << " + result.Data);
							return new ResultBase<TsmExecuteArguments> { Data = new List<TsmExecuteArguments> { result } };
						}
					}
					arguments.NextStep = NextStepEnum.InstallForPerso;
				}
				else
					throw new ApplicationException("Step {0} not defined for InitTransaction".FormatString(arguments.NextStep));
			}
			
			// Crear tarjeta
			var resultTransportCard = await TransportCardCreateHandler.ExecuteAsync(
				new TransportCardCreateArguments(
					DeviceTypeEnum.Pulsera,
					arguments.DeviceAddress,
					arguments.CardName,
					arguments.CardEntry,
					arguments.CardRandomId,
					arguments.CardUid,
					SessionData.Login
				)
			);
			return new ResultBase<TsmExecuteArguments> { Data = new List<TsmExecuteArguments>() };
		}
		#endregion PersonalizeAsync

		#region ReadAsync
		public async Task<ResultBase<TsmExecuteArguments>> ReadAsync(TsmExecuteArguments arguments)
		{
            if (arguments.State == null)
                Debug.WriteLine("A >> " + arguments.System.ToString() + " - " + arguments.Type.ToString());
            else
                Debug.WriteLine("A >> " + arguments.Data);

			if (arguments.State == null)
			{
				var resultGetReadInfo = await GetReadInfoHandler.ExecuteAsync(
					new TransportOperationGetReadInfoArguments(arguments.CardUid.ToHexadecimalBE(), "")
					{
						NeedKeys = false
					}
				) as ScriptResultBase<TransportOperationGetReadInfoResult>;

				var getReadInfoScript = resultGetReadInfo.Scripts.FirstOrDefault();
				var state = new TsmState
				{
					Script = await ConvertFromScriptAsync(getReadInfoScript),
					CardRandomId = arguments.CardRandomId
				};

				arguments.OperationId = resultGetReadInfo.Operation.Id;
				arguments = await SelectHandler.CreateAsync(arguments, state, GetSdAid(state.CardRandomId));
				arguments = await InitializeUpdateHandler.CreateAsync(arguments, state, KeyId);
				arguments.NextStep = NextStepEnum.Select;

				Debug.WriteLine("A << " + arguments.Data);
				return new ResultBase<TsmExecuteArguments> { Data = new List<TsmExecuteArguments> { arguments } };
			}
			else
			{
				var state = arguments.State.FromJson<TsmState>();

				if (arguments.NextStep == NextStepEnum.Select)
				{
					state = await SelectHandler.ExecuteAsync(arguments, state, GetSdAid(arguments.CardRandomId));
					arguments.State = state.ToJson();
					if (arguments.Data.Count() == 0)
					{
						arguments = await InitializeUpdateHandler.CreateAsync(arguments, state, KeyId);

						Debug.WriteLine("A << " + arguments.Data);
						return new ResultBase<TsmExecuteArguments> { Data = new List<TsmExecuteArguments> { arguments } };
					}
					arguments.NextStep = NextStepEnum.InitializeUpdate;
				}
				if (arguments.NextStep == NextStepEnum.InitializeUpdate)
				{
					state = await InitializeUpdateHandler.ExecuteAsync(arguments, state, KeyId, Keys, Scp);
					arguments.State = state.ToJson();
					if (arguments.Data.Count() == 0)
					{
						arguments = await ExternalAuthenticateHandler.CreateAsync(arguments, state, SecurityLevel, Scp, state.HostCrypto);
						arguments = await InstallForPersoHandler.CreateAsync(arguments, state, GetAid(arguments.CardRandomId), SecurityLevel, Scp);
						//arguments = await StoreDataHandler.CreateAsync(arguments, state, SecurityLevel, Scp);
						//arguments = await InstallForPersoHandler.CreateAsync(arguments, state, GetAid(arguments.CardRandomId), SecurityLevel, Scp);
						//arguments = await StoreDataHandler.CreateAsync(arguments, state, SecurityLevel, Scp);
						arguments.NextStep = NextStepEnum.ExternalAuthenticate;

						Debug.WriteLine("A << " + arguments.Data);
						return new ResultBase<TsmExecuteArguments> { Data = new List<TsmExecuteArguments> { arguments } };
					}
					arguments.NextStep = NextStepEnum.ExternalAuthenticate;
				}
				if (arguments.NextStep == NextStepEnum.ExternalAuthenticate)
				{
					state = await ExternalAuthenticateHandler.ExecuteAsync(arguments, state, GetAid(arguments.CardRandomId), SecurityLevel, Scp);
					arguments.State = state.ToJson();
					if (arguments.Data.Count() == 0)
					{
						arguments = await InstallForPersoHandler.CreateAsync(arguments, state, GetAid(arguments.CardRandomId), SecurityLevel, Scp);

						Debug.WriteLine("A << " + arguments.Data);
						return new ResultBase<TsmExecuteArguments> { Data = new List<TsmExecuteArguments> { arguments } };
					}
					arguments.NextStep = NextStepEnum.InstallForPerso;
				}
				while (arguments.Data.Count() > 0)
				{
					if (arguments.NextStep == NextStepEnum.InstallForPerso)
					{
						state = await InstallForPersoHandler.ExecuteAsync(arguments, state, SecurityLevel, Scp);
						arguments.State = state.ToJson();
						if (arguments.Data.Count() == 0)
						{
							arguments = await StoreDataHandler.CreateAsync(arguments, state, SecurityLevel, Scp);

							Debug.WriteLine("A << " + arguments.Data);
							return new ResultBase<TsmExecuteArguments> { Data = new List<TsmExecuteArguments> { arguments } };
						}
						arguments.NextStep = NextStepEnum.StoreData;
					}
					if (arguments.NextStep == NextStepEnum.StoreData)
					{
						state = await StoreDataHandler.ExecuteAsync(arguments, state, GetAid(arguments.CardRandomId), SecurityLevel, Scp);
						arguments.State = state.ToJson();
						if (arguments.Data.Count() == 0)
						{
							if (state.Script.Count() > 0)
							{
								arguments = await InstallForPersoHandler.CreateAsync(arguments, state, GetAid(arguments.CardRandomId), SecurityLevel, Scp);

								Debug.WriteLine("A << " + arguments.Data);
								return new ResultBase<TsmExecuteArguments> { Data = new List<TsmExecuteArguments> { arguments } };
							}
						}
						arguments.NextStep = NextStepEnum.InstallForPerso;
					}
				}

				#region Call ReadInfo
				var script = await ConvertToScriptAsync(state);
				var resultReadInfo = (await ReadInfoHandler.ExecuteAsync(
					new TransportOperationReadInfoArguments(
						arguments.CardUid.ToHexadecimalBE(),
                        arguments.System,
						script.ToArray()
					)
					{
						OperationId = arguments.OperationId.Value
					}
				)) as ServiceCardReadInfoResultBase;

				// Comprobar si hay script que volver a ejecutar (LG)
				var readInfoScript = resultReadInfo.Scripts.FirstOrDefault();
				if (readInfoScript.Script.Count() > 0)
				{
					state.Script = await ConvertFromScriptAsync(readInfoScript);

					arguments.OperationId = (resultReadInfo.Operation.Id as object).ConvertTo<int>();
					arguments = await InstallForPersoHandler.CreateAsync(arguments, state, GetAid(arguments.CardRandomId), SecurityLevel, Scp);
					arguments.NextStep = NextStepEnum.InstallForPerso;

					Debug.WriteLine("A << " + arguments.Data);
					return new ResultBase<TsmExecuteArguments> { Data = new List<TsmExecuteArguments> { arguments } };
				}
				#endregion Call ReadInfo

				// Comprobar si ha terminado
				if (arguments.Data.Count() > 0)
				{
					Debug.WriteLine("A << " + arguments.Data);
					return new ResultBase<TsmExecuteArguments> { Data = new List<TsmExecuteArguments> { arguments } };
				}

				{
					#region Call Confirm
					var resultConfirm = (await ConfirmHandler.ExecuteAsync(
						new TransportOperationConfirmArguments(
							arguments.OperationId.Value,
							arguments.DeviceAddress
#if DEBUG || TEST || HOMO
							, null // now
#endif
						)
					));
					#endregion Call Confirm

					resultReadInfo.Scripts.FirstOrDefault().Script = await ConvertToScript2Async(state);
					return new TsmExecuteArgumentsBase
					{
						Data = new List<TsmExecuteArguments>(),
						Card = resultReadInfo.ToJson()
					};
				}
			}
		}
		#endregion ReadAsync

		#region ExecuteCardAsync
		public async Task<ResultBase<TsmExecuteArguments>> ExecuteCardAsync(TsmExecuteArguments arguments)
		{
			if (arguments.State == null)
				Debug.WriteLine("A >> " + arguments.System.ToString() + " - " + arguments.Type.ToString());
			else
				Debug.WriteLine("A >> " + arguments.Data);

			if (arguments.State == null)
			{
				//var resultGetReadInfo = await GetReadInfoHandler.ExecuteAsync(new TransportOperationGetReadInfoArguments(arguments.CardUid.ToHexadecimalBE(), "")) as ScriptResultBase<TransportOperationGetReadInfoResult>;
				// TODO: XAVI Falta optimizar para que no cargue el HSM

				//var getReadInfoScript = resultGetReadInfo.Scripts.FirstOrDefault();
				var state = new TsmState
				{
					Script = await ConvertFromScriptAsync(arguments.Script.FromJson()),
					CardRandomId = arguments.CardRandomId
				};
				
				arguments = await SelectHandler.CreateAsync(arguments, state, GetSdAid(state.CardRandomId));
				arguments = await InitializeUpdateHandler.CreateAsync(arguments, state, KeyId);
				arguments.NextStep = NextStepEnum.Select;

				Debug.WriteLine("A << " + arguments.Data);
				return new ResultBase<TsmExecuteArguments> { Data = new List<TsmExecuteArguments> { arguments } };
			}
			else
			{
				var state = arguments.State.FromJson<TsmState>();

				if (arguments.NextStep == NextStepEnum.Select)
				{
					state = await SelectHandler.ExecuteAsync(arguments, state, GetSdAid(arguments.CardRandomId));
					arguments.State = state.ToJson();
					if (arguments.Data.Count() == 0)
					{
						arguments = await InitializeUpdateHandler.CreateAsync(arguments, state, KeyId);

						Debug.WriteLine("A << " + arguments.Data);
						return new ResultBase<TsmExecuteArguments> { Data = new List<TsmExecuteArguments> { arguments } };
					}
					arguments.NextStep = NextStepEnum.InitializeUpdate;
				}
				if (arguments.NextStep == NextStepEnum.InitializeUpdate)
				{
					state = await InitializeUpdateHandler.ExecuteAsync(arguments, state, KeyId, Keys, Scp);
					arguments.State = state.ToJson();
					if (arguments.Data.Count() == 0)
					{
						arguments = await ExternalAuthenticateHandler.CreateAsync(arguments, state, SecurityLevel, Scp, state.HostCrypto);
						arguments = await InstallForPersoHandler.CreateAsync(arguments, state, GetAid(arguments.CardRandomId), SecurityLevel, Scp);
						//arguments = await StoreDataHandler.CreateAsync(arguments, state, SecurityLevel, Scp);
						//arguments = await InstallForPersoHandler.CreateAsync(arguments, state, GetAid(arguments.CardRandomId), SecurityLevel, Scp);
						//arguments = await StoreDataHandler.CreateAsync(arguments, state, SecurityLevel, Scp);
						arguments.NextStep = NextStepEnum.ExternalAuthenticate;

						Debug.WriteLine("A << " + arguments.Data);
						return new ResultBase<TsmExecuteArguments> { Data = new List<TsmExecuteArguments> { arguments } };
					}
					arguments.NextStep = NextStepEnum.ExternalAuthenticate;
				}
				if (arguments.NextStep == NextStepEnum.ExternalAuthenticate)
				{
					state = await ExternalAuthenticateHandler.ExecuteAsync(arguments, state, GetAid(arguments.CardRandomId), SecurityLevel, Scp);
					arguments.State = state.ToJson();
					if (arguments.Data.Count() == 0)
					{
						arguments = await InstallForPersoHandler.CreateAsync(arguments, state, GetAid(arguments.CardRandomId), SecurityLevel, Scp);

						Debug.WriteLine("A << " + arguments.Data);
						return new ResultBase<TsmExecuteArguments> { Data = new List<TsmExecuteArguments> { arguments } };
					}
					arguments.NextStep = NextStepEnum.InstallForPerso;
				}
				while (arguments.Data.Count() > 0)
				{
					if (arguments.NextStep == NextStepEnum.InstallForPerso)
					{
						state = await InstallForPersoHandler.ExecuteAsync(arguments, state, SecurityLevel, Scp);
						arguments.State = state.ToJson();
						if (arguments.Data.Count() == 0)
						{
							arguments = await StoreDataHandler.CreateAsync(arguments, state, SecurityLevel, Scp);

							Debug.WriteLine("A << " + arguments.Data);
							return new ResultBase<TsmExecuteArguments> { Data = new List<TsmExecuteArguments> { arguments } };
						}
						arguments.NextStep = NextStepEnum.StoreData;
					}
					if (arguments.NextStep == NextStepEnum.StoreData)
					{
						state = await StoreDataHandler.ExecuteAsync(arguments, state, GetAid(arguments.CardRandomId), SecurityLevel, Scp);
						arguments.State = state.ToJson();
						if (arguments.Data.Count() == 0)
						{
							if (state.Script.Count() > 0)
							{
								arguments = await InstallForPersoHandler.CreateAsync(arguments, state, GetAid(arguments.CardRandomId), SecurityLevel, Scp);

								Debug.WriteLine("A << " + arguments.Data);
								return new ResultBase<TsmExecuteArguments> { Data = new List<TsmExecuteArguments> { arguments } };
							}
						}
						arguments.NextStep = NextStepEnum.InstallForPerso;
					}
				}
				
				#region Call Confirm
				await ConfirmHandler.ExecuteAsync(
					new TransportOperationConfirmArguments(
						arguments.OperationId.Value,
						arguments.DeviceAddress
#if DEBUG || TEST || HOMO
							, null // now
#endif
						)
				);
				#endregion Call Confirm

				#region Call ReadInfo
				var script = await ConvertToScriptAsync(state);
				var resultReadInfo = (await ReadInfoHandler.ExecuteAsync(
					new TransportOperationReadInfoArguments(
						arguments.CardUid.ToHexadecimalBE(),
                        arguments.System,
						script.ToArray()
					)
					{
						OperationId = arguments.OperationId.Value
					}
				)) as ServiceCardReadInfoResultBase;
				#endregion Call ReadInfo

				return new TsmExecuteArgumentsBase
				{
					Data = new List<TsmExecuteArguments>(),
					Card = resultReadInfo.ToJson()
				};
			}
		}
		#endregion ExecuteCardAsync
	}
}
