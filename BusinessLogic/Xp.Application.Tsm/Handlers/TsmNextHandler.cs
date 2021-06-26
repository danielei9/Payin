using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Application.Dto.Tsm.Arguments;
using Xp.Application.Dto.Tsm.GlobalPlatform;
using System.Diagnostics;
using Xp.Application.Dto.Tsm.GlobalPlatform.States;
using Xp.Application.Tsm.Mifare4Mobile.Commands;
using Xp.Application.Tsm.GlobalPlatform.Handlers;
using Xp.Application.Hsm;

namespace Xp.Application.Tsm.Handlers
{
	public class TsmNextHandler : IQueryBaseHandler<TsmNextArguments, TsmNextArguments>
	{
		private TsmSelectHandler SelectHandler { get; set; }
		private TsmInitializeUpdateHandler InitializeUpdateHandler { get; set; }
		private TsmExternalAuthenticateHandler ExternalAuthenticateHandler { get; set; }
		private TsmInstallForPersoHandler InstallForPersoHandler { get; set; }
		private TsmStoreDataHandler StoreDataHandler { get; set; }
		private IMifare4MobileHsm Hsm { get; set; }

		#region Constructors
		public TsmNextHandler(IMifare4MobileHsm hsm)
		{
			StoreDataHandler = new TsmStoreDataHandler();
			InstallForPersoHandler = new TsmInstallForPersoHandler(StoreDataHandler);
			ExternalAuthenticateHandler = new TsmExternalAuthenticateHandler(InstallForPersoHandler);
			InitializeUpdateHandler = new TsmInitializeUpdateHandler(ExternalAuthenticateHandler);
			SelectHandler = new TsmSelectHandler(InitializeUpdateHandler);

			StoreDataHandler.InstallForPersoHandler = InstallForPersoHandler;

			Hsm = hsm;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<TsmNextArguments>> ExecuteAsync(TsmNextArguments arguments)
		{
			Debug.WriteLine("A >> " + arguments.Data);

			TsmNextArguments result = null;
			//if (arguments.Type.ToLower() == "inittransaction")
			{
				if (arguments.State == null)
				{
					var state = new TsmSelectState
					{
						Script = new List<byte[]>
						{
							(await (
								new M4mUpdateSmMetadataCommand()
							).ToHexadecimalAsync()).FromHexadecimal(),
							(await (
								new M4mAddAndUpdateMdacCommand()
							).ToHexadecimalAsync()).FromHexadecimal(),
							(await (
								new M4mUpdateMifareClassicSectorCommand(Hsm, 0)
								{
									Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
									AccessConditions = "FF078069".FromHexadecimal()
								}
							).ToHexadecimalAsync()).FromHexadecimal(),
							(await (
								new M4mUpdateMifareClassicSectorCommand(Hsm, 1)
								{
									Block0 = "3110C0D20000000000200000000040D8".FromHexadecimal(),
									Block1 = "010000000A000000000000000000004C".FromHexadecimal(),
									Block2 = "010000000A000000000000000000004C".FromHexadecimal(),
									AccessConditions = "7E178869".FromHexadecimal()
								}
							).ToHexadecimalAsync()).FromHexadecimal(),
							(await (
								new M4mUpdateMifareClassicSectorCommand(Hsm, 2)
								{
									Block0 = "0000000018A83083559A00842D010070".FromHexadecimal(),
									Block1 = "20202020202020202020202020200048".FromHexadecimal(),
									Block2 = "808080808000030000000000000018A4".FromHexadecimal(),
									AccessConditions = "78778869".FromHexadecimal()
								}
							).ToHexadecimalAsync()).FromHexadecimal(),
							(await (
								new M4mUpdateMifareClassicSectorCommand(Hsm, 3)
								{
									Block0 = "01C200A0074000000020FCDFDF04004D".FromHexadecimal(),
									Block1 = "0A77E82A0D000000000000000000008D".FromHexadecimal(),
									Block2 = "2020202020202020202020202020206B".FromHexadecimal(),
									AccessConditions = "78778869".FromHexadecimal()
								}
							).ToHexadecimalAsync()).FromHexadecimal(),
							(await (
								new M4mUpdateMifareClassicSectorCommand(Hsm, 4)
								{
									Block0 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
									AccessConditions = "7F078869".FromHexadecimal()
								}
							).ToHexadecimalAsync()).FromHexadecimal(),
							(await (
								new M4mUpdateMifareClassicSectorCommand(Hsm, 5)
								{
									Block0 = "00000000FFFFFFFF0000000000FF00FF".FromHexadecimal(),
									Block1 = "00000000FFFFFFFF0000000000FF00FF".FromHexadecimal(),
									Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
									AccessConditions = "7F078869".FromHexadecimal()
								}
							).ToHexadecimalAsync()).FromHexadecimal(),
							(await (
								new M4mUpdateMifareClassicSectorCommand(Hsm, 6)
								{
									Block0 = "00000000FFFFFFFF0000000000FF00FF".FromHexadecimal(),
									Block1 = "00000000FFFFFFFF0000000000FF00FF".FromHexadecimal(),
									Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
									AccessConditions = "4C378B69".FromHexadecimal()
								}
							).ToHexadecimalAsync()).FromHexadecimal(),
							(await (
								new M4mUpdateMifareClassicSectorCommand(Hsm, 7)
								{
									Block0 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
									AccessConditions = "7F078869".FromHexadecimal()
								}
							).ToHexadecimalAsync()).FromHexadecimal(),
							(await (
								new M4mUpdateMifareClassicSectorCommand(Hsm, 8)
								{
									Block0 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
									AccessConditions = "78778869".FromHexadecimal()
								}
							).ToHexadecimalAsync()).FromHexadecimal(),
							(await (
								new M4mUpdateMifareClassicSectorCommand(Hsm, 9)
								{
									Block0 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
									AccessConditions = "FF078069".FromHexadecimal()
								}
							).ToHexadecimalAsync()).FromHexadecimal(),
							(await (
								new M4mUpdateMifareClassicSectorCommand(Hsm, 10)
								{
									Block0 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
									AccessConditions = "FF078069".FromHexadecimal()
								}
							).ToHexadecimalAsync()).FromHexadecimal(),
							(await (
								new M4mUpdateMifareClassicSectorCommand(Hsm, 11)
								{
									Block0 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
									AccessConditions = "FF078069".FromHexadecimal()
								}
							).ToHexadecimalAsync()).FromHexadecimal(),
							(await (
								new M4mUpdateMifareClassicSectorCommand(Hsm, 12)
								{
									Block0 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
									AccessConditions = "FF078069".FromHexadecimal()
								}
							).ToHexadecimalAsync()).FromHexadecimal(),
							(await (
								new M4mUpdateMifareClassicSectorCommand(Hsm, 13)
								{
									Block0 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
									AccessConditions = "FF078069".FromHexadecimal()
								}
							).ToHexadecimalAsync()).FromHexadecimal(),
							(await (
								new M4mUpdateMifareClassicSectorCommand(Hsm, 14)
								{
									Block0 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
									AccessConditions = "FF078069".FromHexadecimal()
								}
							).ToHexadecimalAsync()).FromHexadecimal(),
							(await (
								new M4mUpdateMifareClassicSectorCommand(Hsm, 15)
								{
									Block0 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
									AccessConditions = "FF078069".FromHexadecimal()
								}
							).ToHexadecimalAsync()).FromHexadecimal()
						}
					};

					result = (await SelectHandler.CreateAsync(arguments, state)).Data.FirstOrDefault();
				}
				else if (arguments.NextStep == NextStepEnum.Select)
					result = (await SelectHandler.ExecuteAsync(arguments)).Data.FirstOrDefault();
				else if (arguments.NextStep == NextStepEnum.InitializeUpdate)
					result = (await InitializeUpdateHandler.ExecuteAsync(arguments)).Data.FirstOrDefault();
				else if (arguments.NextStep == NextStepEnum.ExternalAuthenticate)
					result = (await ExternalAuthenticateHandler.ExecuteAsync(arguments)).Data.FirstOrDefault();
				else if (arguments.NextStep == NextStepEnum.InstallForPerso)
					result = (await InstallForPersoHandler.ExecuteAsync(arguments)).Data.FirstOrDefault();
				else if (arguments.NextStep == NextStepEnum.StoreData)
					result = (await StoreDataHandler.ExecuteAsync(arguments)).Data.FirstOrDefault();
				else
					throw new ApplicationException("Step {0} not defined for InitTransaction".FormatString(arguments.NextStep));
			}

			if (result == null)
			{
				Debug.WriteLine("A << FIN");
				return new ResultBase<TsmNextArguments>
				{
					Data = new List<TsmNextArguments>()
				};
			}
			else
			{
				Debug.WriteLine("A << " + result.Data);
				return new ResultBase<TsmNextArguments>
				{
					Data = new List<TsmNextArguments> { result }
				};
			}
		}
		#endregion ExecuteAsync
	}
}
