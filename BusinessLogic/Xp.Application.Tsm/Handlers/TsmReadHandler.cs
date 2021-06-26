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

namespace Xp.Application.Tsm.Handlers
{
	public class TsmReadHandler : IQueryBaseHandler<TsmReadArguments, TsmReadArguments>
	{
		private TsmSelectHandler SelectHandler { get; set; }
		private TsmInitializeUpdateHandler InitializeUpdateHandler { get; set; }
		private TsmExternalAuthenticateHandler ExternalAuthenticateHandler { get; set; }
		private TsmInstallForPersoHandler InstallForPersoHandler { get; set; }
		private TsmStoreDataHandler StoreDataHandler { get; set; }

		#region Constructors
		public TsmReadHandler()
		{
			SelectHandler = new TsmSelectHandler(InitializeUpdateHandler);
			InitializeUpdateHandler = new TsmInitializeUpdateHandler(ExternalAuthenticateHandler);
			ExternalAuthenticateHandler = new TsmExternalAuthenticateHandler(InstallForPersoHandler);
			InstallForPersoHandler = new TsmInstallForPersoHandler(StoreDataHandler);
			StoreDataHandler = new TsmStoreDataHandler();

			StoreDataHandler.InstallForPersoHandler = InstallForPersoHandler;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<TsmReadArguments>> ExecuteAsync(TsmReadArguments arguments)
		{
			//Debug.WriteLine("A >> " + arguments.Data);

			TsmReadArguments result = null;
			//if (arguments.Type.ToLower() == "inittransaction")
			{
				//if (arguments.State == null)
				{
					var state = new TsmSelectState
					{
						Script = new List<byte[]>
						{
							(
								new M4mUpdateSmMetadataCommand()
							).ToString().FromHexadecimal(),
							(
								new M4mAddAndUpdateMdacCommand()
							).ToString().FromHexadecimal(),
							(
								new M4mUpdateMifareClassicSectorCommand(0)
								{
									Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block3 = "A8844B0BCA06FF07806986CCAAE576A2".FromHexadecimal()
								}
							).ToString().FromHexadecimal(),
							(
								new M4mUpdateMifareClassicSectorCommand(1)
								{
									Block0 = "3110C0D20000000000200000000040D8".FromHexadecimal(),
									Block1 = "010000000A000000000000000000004C".FromHexadecimal(),
									Block2 = "010000000A000000000000000000004C".FromHexadecimal(),
									Block3 = "CB5ED0E57B087E17886968C867397AD5".FromHexadecimal()
								}
							).ToString().FromHexadecimal(),
							(
								new M4mUpdateMifareClassicSectorCommand(2)
								{
									Block0 = "0000000018A83083559A00842D010070".FromHexadecimal(),
									Block1 = "20202020202020202020202020200048".FromHexadecimal(),
									Block2 = "808080808000030000000000000018A4".FromHexadecimal(),
									Block3 = "749934CC8ED3787788694427385D72AB".FromHexadecimal()
								}
							).ToString().FromHexadecimal(),
							(
								new M4mUpdateMifareClassicSectorCommand(3)
								{
									Block0 = "01C200A0074000000020FCDFDF04004D".FromHexadecimal(),
									Block1 = "0A77E82A0D000000000000000000008D".FromHexadecimal(),
									Block2 = "2020202020202020202020202020206B".FromHexadecimal(),
									Block3 = "AE381EA0811B787788699B2C3E00B561".FromHexadecimal()
								}
							).ToString().FromHexadecimal(),
							(
								new M4mUpdateMifareClassicSectorCommand(4)
								{
									Block0 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block3 = "40454EE642297F078869120A7837BB5D".FromHexadecimal()
								}
							).ToString().FromHexadecimal(),
							(
								new M4mUpdateMifareClassicSectorCommand(5)
								{
									Block0 = "00000000FFFFFFFF0000000000FF00FF".FromHexadecimal(),
									Block1 = "00000000FFFFFFFF0000000000FF00FF".FromHexadecimal(),
									Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block3 = "66A4932816D37F078869B19A0664ECA6".FromHexadecimal()
								}
							).ToString().FromHexadecimal(),
							(
								new M4mUpdateMifareClassicSectorCommand(6)
								{
									Block0 = "00000000FFFFFFFF0000000000FF00FF".FromHexadecimal(),
									Block1 = "00000000FFFFFFFF0000000000FF00FF".FromHexadecimal(),
									Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block3 = "B54D99618ADC4C378B69B456E1951216".FromHexadecimal()
								}
							).ToString().FromHexadecimal(),
							(
								new M4mUpdateMifareClassicSectorCommand(7)
								{
									Block0 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block3 = "08D6A77656407F078869E87E3554727E".FromHexadecimal()
								}
							).ToString().FromHexadecimal(),
							(
								new M4mUpdateMifareClassicSectorCommand(8)
								{
									Block0 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block3 = "3E0557273982787788698D96A0BA7234".FromHexadecimal()
								}
							).ToString().FromHexadecimal(),
							(
								new M4mUpdateMifareClassicSectorCommand(9)
								{
									Block0 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block3 = "C0C1C2C3C4C5FF078069B0B1B2B3B4B5".FromHexadecimal()
								}
							).ToString().FromHexadecimal(),
							(
								new M4mUpdateMifareClassicSectorCommand(10)
								{
									Block0 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block3 = "C0C1C2C3C4C5FF078069B0B1B2B3B4B5".FromHexadecimal()
								}
							).ToString().FromHexadecimal(),
							(
								new M4mUpdateMifareClassicSectorCommand(11)
								{
									Block0 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block3 = "C0C1C2C3C4C5FF078069B0B1B2B3B4B5".FromHexadecimal()
								}
							).ToString().FromHexadecimal(),
							(
								new M4mUpdateMifareClassicSectorCommand(12)
								{
									Block0 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block3 = "C0C1C2C3C4C5FF078069B0B1B2B3B4B5".FromHexadecimal()
								}
							).ToString().FromHexadecimal(),
							(
								new M4mUpdateMifareClassicSectorCommand(13)
								{
									Block0 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block3 = "C0C1C2C3C4C5FF078069B0B1B2B3B4B5".FromHexadecimal()
								}
							).ToString().FromHexadecimal(),
							(
								new M4mUpdateMifareClassicSectorCommand(14)
								{
									Block0 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block3 = "C0C1C2C3C4C5FF078069B0B1B2B3B4B5".FromHexadecimal()
								}
							).ToString().FromHexadecimal(),
							(
								new M4mUpdateMifareClassicSectorCommand(15)
								{
									Block0 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block1 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block2 = "000000000000000000000000000000B1".FromHexadecimal(),
									Block3 = "C0C1C2C3C4C5FF078069B0B1B2B3B4B5".FromHexadecimal()
								}
							).ToString().FromHexadecimal()
						}
					};

					//result = (await SelectHandler.CreateAsync(arguments, state)).Data.FirstOrDefault();
				}
				//else if (arguments.NextStep == NextStepEnum.Select)
				//	result = (await SelectHandler.ExecuteAsync(arguments)).Data.FirstOrDefault();
				//else if (arguments.NextStep == NextStepEnum.InitializeUpdate)
				//	result = (await InitializeUpdateHandler.ExecuteAsync(arguments)).Data.FirstOrDefault();
				//else if (arguments.NextStep == NextStepEnum.ExternalAuthenticate)
				//	result = (await ExternalAuthenticateHandler.ExecuteAsync(arguments)).Data.FirstOrDefault();
				//else if (arguments.NextStep == NextStepEnum.InstallForPerso)
				//	result = (await InstallForPersoHandler.ExecuteAsync(arguments)).Data.FirstOrDefault();
				//else if (arguments.NextStep == NextStepEnum.StoreData)
				//	result = (await StoreDataHandler.ExecuteAsync(arguments)).Data.FirstOrDefault();
				//else
				//	throw new ApplicationException("Step {0} not defined for InitTransaction".FormatString(arguments.NextStep));
			}

			if (result == null)
			{
				Debug.WriteLine("A << FIN");
				return new ResultBase<TsmReadArguments>
				{
					Data = new List<TsmReadArguments>()
				};
			}
			else
			{
				//Debug.WriteLine("A << " + result.Data);
				return new ResultBase<TsmReadArguments>
				{
					//Data = new List<TsmPersonalizeArguments> { result }
				};
			}
		}
		#endregion ExecuteAsync
	}
}
