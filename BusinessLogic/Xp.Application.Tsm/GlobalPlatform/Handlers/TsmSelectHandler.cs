using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Application.Dto.Tsm.Arguments;
using Xp.Application.Dto.Tsm.GlobalPlatform;
using Xp.Application.Dto.Tsm.GlobalPlatform.States;
using Xp.Application.Tsm.GlobalPlatform.CardConfiguration;
using Xp.Application.Tsm.GlobalPlatform.Commands;
using Xp.Application.Tsm.GlobalPlatform.Constants;

namespace Xp.Application.Tsm.GlobalPlatform.Handlers
{
	internal class TsmSelectHandler
	{
		protected TsmInitializeUpdateHandler InitializeUpdateHandler { get; set; }

		#region Constructors
		public TsmSelectHandler(TsmInitializeUpdateHandler initializeUpdateHandler)
		{
			InitializeUpdateHandler = initializeUpdateHandler;
		}
		#endregion Constructors

		#region CreateAsync
		public async Task<TsmExecuteArguments> CreateAsync(TsmExecuteArguments arguments, TsmState state, byte[] sdAid)
		{
			return await Task.Run(() =>
            {
                Debug.WriteLine("CardRandomId: " + arguments.CardRandomId);
                Debug.WriteLine("sdAid: " + sdAid.ToHexadecimal());

				var command = new ApduCommand(
					TsmApduCla.COMMAND_ISO,
					TsmApduIns.SELECT,
					TsmApduP1.SELECT_ByName,
					TsmApduP2.SELECT_First,
					sdAid);

				arguments.Data = new List<string> { command.ToHexadecimal() };
                arguments.State = state.ToJson();
				arguments.NextStep = NextStepEnum.Select;

				return arguments;
			});
		}
		#endregion CreateAsync

		#region ExecuteAsync
		public async Task<TsmState> ExecuteAsync(TsmExecuteArguments arguments, TsmState state, byte[] sdAid)
		{
			return await Task.Run(() =>
			{
				var command = arguments.Data[0];
				arguments.Data.RemoveAt(0);

				command.FromHexadecimal()
					.GP_CheckWarning(0x62, 0x83, "GP SELECT WARNING: Card Life Cycle State is CARD_LOCKED")
					.GP_CheckGeneralErrors()
					.GP_CheckError(0x68, 0x82, "GP SELECT ERROR: Secure messaging not supported")
					.GP_CheckError(0x6A, 0x81, "GP SELECT ERROR: Function not supported e.g. card Life Cycle State is CARD_LOCKED")
					.GP_CheckError(0x6A, 0x82, "GP SELECT ERROR: Selected Application / file not found")
					.GP_ReadResponse(0x6F, (fileControlInformation2) =>
					{
						return fileControlInformation2
						.GP_ReadResponse(0x84, (aid2) =>
						{
							if (!aid2.SequenceEqual(sdAid))
								throw new ApplicationException("GP SELECT ERROR: Aid not equals");
						})
						.GP_ReadResponse(0xA5, (propietaryData2) =>
						{
							return propietaryData2
							.GP_ReadResponse(0x73, () =>
							{
								throw new ApplicationException("GP SELECT ERROR: Error in response");
							})
							.GP_ReadResponse(0x9F, 0x6E, () =>
							{
								throw new ApplicationException("GP SELECT ERROR: Error in response");
							})
							.GP_ReadResponse(0x9F, 0x65, (maxDataField) =>
							{
								state.MaxDataField = maxDataField.ToInt8().Value;
							});
						});
					})
					.GP_CheckSuccess();

				return state;
			});
		}
		#endregion ExecuteAsync
	}
}

