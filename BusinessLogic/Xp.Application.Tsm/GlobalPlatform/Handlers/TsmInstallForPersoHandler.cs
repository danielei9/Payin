using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Application.Dto.Tsm.Arguments;
using Xp.Application.Dto.Tsm.GlobalPlatform;
using Xp.Application.Dto.Tsm.GlobalPlatform.States;
using Xp.Application.Tsm.GlobalPlatform.Commands;
using Xp.Application.Tsm.GlobalPlatform.Constants;

namespace Xp.Application.Tsm.GlobalPlatform.Handlers
{
	public class TsmInstallForPersoHandler
	{
		TsmStoreDataHandler StoreDataHandler { get; set; }

		#region Constructors
		public TsmInstallForPersoHandler(TsmStoreDataHandler storeDataHandler)
		{
			StoreDataHandler = storeDataHandler;
		}
		#endregion Constructors

		#region CreateAsync
		public async Task<TsmExecuteArguments> CreateAsync(TsmExecuteArguments arguments, TsmState state, byte[] aid, SecLevel secLevel, ScpMode scp)
		{
			return await Task.Run(() =>
			{
				var result = new byte[6 + aid.Length];
				var i = 0;
				result[i++] = 0x00;
				result[i++] = 0x00;
				result[i++] = (byte)aid.Length;
				result.Copy(aid, 0, aid.Length, i);
				i = i + aid.Length;
				result[i++] = 0x00;
				result[i++] = 0x00;
				result[i++] = 0x00;

				var command = new ApduCommand(
					(secLevel == SecLevel.NO_SECURITY_LEVEL) ?
						TsmApduCla.COMMAND_GP : // 0x80
						TsmApduCla.GET_CHALLENGE, // 0x84
					TsmApduIns.INSTALL, // 0xE6
					TsmApduP1.INSTALL_ForPerso, // 0x20
					TsmApduP2.PERSO, // 0x00
					result,
					(secLevel != SecLevel.NO_SECURITY_LEVEL) ?
						(byte)(result.Length + 8) :
						(byte)(result.Length)
				);

				arguments.Data.Add(command.ToHexadecimalWithMac(state, secLevel, scp));
				arguments.NextStep = NextStepEnum.InstallForPerso;
				arguments.State = state.ToJson();

				return arguments;
			});
		}
		#endregion ExecuteAsync

		#region ExecuteAsync
		public async Task<TsmState> ExecuteAsync(TsmExecuteArguments arguments, TsmState state, SecLevel secLevel, ScpMode scp)
		{
			return await Task.Run(() =>
			{
				var command = arguments.Data[0];
				arguments.Data.RemoveAt(0);

				command.FromHexadecimal()
					.GP_CheckMac(state, secLevel, scp)
					.GP_CheckGeneralErrors()
					.GP_CheckError(0x65, 0x81, "Memory failure")
					.GP_CheckError(0x6A, 0x80, "Incorrect parameters in data field")
					.GP_CheckError(0x6A, 0x84, "Not enough memory space")
					.GP_CheckError(0x6A, 0x88, "Referenced data not found")
					.GP_CheckSuccess(false)
					.GP_ReadResponse(0x00);

				return state;
			});
		}
		#endregion ExecuteAsync
	}
}
