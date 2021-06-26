using System;
using Xp.Application.Dto.Tsm.GlobalPlatform;
using Xp.Application.Tsm.GlobalPlatform.Commands;
using Xp.Application.Tsm.GlobalPlatform.Config;
using Xp.Application.Tsm.GlobalPlatform.SmartCardIo;

namespace Xp.Application.Tsm.GlobalPlatform.Applet
{
	public class GPApplet
	{
		// File Control Information Defines in the SELECT Command response
		protected FileControlInformation FileControlInformation;

		// Global Platform AID
		protected byte[] Aid;

		// Commands to dialog to the GP Applet
		protected ICommands Cmds;

		/// <summary>
		/// Creates the off-card "Applet"
		/// </summary>
		/// <param name="implementation">
		/// the String representation of the chosen implementation (i.e. "fr.xlim.ssd.opal.commands.GP2xCommands").
		/// This designed implementation must override the class {@link fr.xlim.ssd.opal.library.commands.Commands}
		/// </param>
		/// <param name="aid">the byte array containing the aid representation of the Applet</param>
		public GPApplet(ICommands implementation, byte[] aid)
		{
			Aid = aid.CloneArray();
			Cmds = implementation;
			FileControlInformation = null;
		}

		/* *
		 * Get Card Channel
		 *
		 * @return Card Channel
		 * /
		public CardChannel getCc()
		{
			return this.cmds.getCardChannel();
		}

		/**
		 * Get Global Platform AID
		 *
		 * @return Get Global Platform AID
		 * /
		public byte[] getAid()
		{
			return aid.clone();
		}

		/* (non-Javadoc)
		 * @see fr.xlim.ssd.opal.commands.Commands#getScp()
		 * /
		public SCPMode getScp()
		{
			return this.cmds.getScp();
		}

		/* (non-Javadoc)
		 * @see fr.xlim.ssd.opal.commands.Commands#getSessState()
		 * /
		public SessionState getSessState()
		{
			return this.cmds.getSessState();
		}

		/* (non-Javadoc)
		 * @see fr.xlim.ssd.opal.commands.Commands#getSecMode()
		 * /
		public SecLevel getSecMode()
		{
			return this.cmds.getSecMode();
		}

		/* (non-Javadoc)
		 * @see fr.xlim.ssd.opal.commands.Commands#getKeys()
		 * /
		public SCKey[] getKeys()
		{
			return this.cmds.getKeys();
		}

		/* (non-Javadoc)
		 * @see fr.xlim.ssd.opal.commands.Commands#getKey()
		 * /
		public SCKey getKey(byte keySetVersion, byte keyId)
		{
			return this.cmds.getKey(keySetVersion, keyId);
		}

		/* (non-Javadoc)
		 * @see fr.xlim.ssd.opal.commands.Commands#setOffCardKey(fr.xlim.ssd.opal.library.SCKey)
		 * /
		public SCKey setOffCardKey(SCKey key)
		{
			return this.cmds.setOffCardKey(key);
		}
		 */
		public void SetOffCardKeys(ISCKey[] keys)
		{
			Cmds.SetOffCardKeys(keys);
		}

		/* (non-Javadoc)
		 * @see fr.xlim.ssd.opal.commands.Commands#deleteOffCardKey(byte, byte)
		 * /
		public SCKey deleteOffCardKey(byte keySetVersion, byte keyId)
		{
			return this.cmds.deleteOffCardKey(keySetVersion, keyId);
		}
		 */
		/// <summary>
		/// Select GP Applet and check card response
		/// </summary>
		/// <returns>Card APDU response</returns>
		//public ApduCommand select()
		//{
		//	var command = Cmds.Select(transactionId, type, Aid);
		//	//FileControlInformation = new FileControlInformation(ret.Data);

		//	return command;
		//}
	/* *
     * Select GP Applet and check card response (Used for SCPMode that implement implecit
     * initiation mode
     * @return Card APDU response
     * @throws CardException Communication error
     * @throws IOException   Response Select APDU is ill-formed
     */
	/*
    public ResponseAPDU select(SCPMode desiredScp) throws CardException, IOException {
        ResponseAPDU ret = this.cmds.select(this.aid,desiredScp);

        this.fileControlInformation = new FileControlInformation(ret.getValue());

        return ret;
    }

     */


	/* *
     *
     * /
	public void resetParams()
	{
		this.cmds.resetParams();
	}
*/

	public ApduCommand InitializeUpdate(byte keySetVersion, byte keyId, ScpMode desiredScp)
	{
			return null; // Cmds.InitializeUpdate(keySetVersion, keyId, desiredScp);
	}
	public ApduCommand ExternalAuthenticate(SecLevel secLevel)
	{
			return null; // Cmds.ExternalAuthenticate(secLevel);
	}

	/* (non-Javadoc)
     * @see fr.xlim.ssd.opal.commands.Commands#transmit(javax.smartcardio.CommandAPDU)
     * /
	public ResponseAPDU send(CommandAPDU command) throws CardException
	{
        return this.cmds.getCardChannel().transmit(command);
	}

	/* (non-Javadoc)
     * @see fr.xlim.ssd.opal.commands.Commands#getStatus(fr.xlim.ssd.opal.library.GetStatusFileType, fr.xlim.ssd.opal.library.GetStatusFileType, byte[])
     * /
	public ResponseAPDU[] getStatus(GetStatusFileType ft, GetStatusResponseMode respMode, byte[] searchQualifier) throws CardException
	{
        return this.cmds.getStatus(ft, respMode, searchQualifier);
	}

	/**
     * Get the analysed result of the GP Applet select command.
     *
     * @return Analysed result of the GP Applet select command.
     * /
	public FileControlInformation getCardInformation()
	{
		return this.fileControlInformation;
	}

	/* (non-Javadoc)
     * @see fr.xlim.ssd.opal.commands.Commands#getValue( )
     * /
	public ResponseAPDU getData() throws CardException
	{
        return this.cmds.getData();
	}

	/* (non-Javadoc)
     * @see fr.xlim.ssd.opal.commands.Commands#InitParamForImplicitInitiationMode(byte[])
     * /
	public void InitParamForImplicitInitiationMode(SCPMode desiredScp, byte keyId) throws CardException
	{
        this.cmds.InitParamForImplicitInitiationMode(this.aid, desiredScp, keyId);
	}*/
	}
}
