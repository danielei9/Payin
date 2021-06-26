package com.nxp.ssdp.btclient;

import android.util.Log;

import com.nxp.utils.Parsers;

import java.util.ArrayList;
import java.util.List;

import org.apache.cordova.CallbackContext;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

public class BluetoothTLV {
    public static final String TAG = "Bluetooth TLV";

	public static final byte WIRED_SUCCESS =		0x00; /// Success for WIRED_ENABLE and WIRED_TRANSCEIVE OPERATIONS

	public static final byte SE_MODE =               0x01; ///< This is TLV Type for all SE Mode related Operations
	public static final byte WIRED_TRANSCEIVE =      0x10; ///< TLV Sub type for Wired Transceive
	public static final byte WIREDMODE_ENABLE =      0x11; ///< TLV Sub type for Enabling /disabling wired mode
	public static final byte WIREDMODE_SERESET =     0x12; ///< TLV Sub type for SE reset
	public static final byte SUB_SE_NTFN =           0x13; ///< TLV Sub type for subscribing for SE notification
	public static final byte UNSUB_SE_NTFN =         0x14; ///< TLV Sub type for unsubscribing for SE notification
	public static final byte TRANSACTION_SE_NTFN =   0x15; ///< TLV Sub type for sending transaction notification to companion device
	public static final byte CONNECTIVITY_SE_NTFN =  0x16; ///< TLV Sub type for sending connectivity notification to companion device
	public static final byte RFFIELD_SE_NTFN =       0x17; ///< TLV Sub type for sending RF Field notification to companion device
	public static final byte ACTIVATION_SE_NTFN =    0x18; ///< TLV Sub type for sending Activation/deactivation notification to companion device
	public static final byte WIREDMODE_GET_ATR =     0x19; ///< TLV Sub type for get ATR
	
	
	public static final byte FWDND_MODE =            0x02;       ///< This is TLV Type for all Firmware Download related Operations
	public static final byte FW_DND_PREP =           0x20;       ///< TLV Sub type for Preparing wearable for firmware download
	public static final byte NFCC_MODESET =          0x21;       ///< TLV Sub type for setting mode (normal/ download) of wearable
	public static final byte READ_ABORT =            0x22;       ///< TLV Sub type for aborting pending TML read
	public static final byte GET_DEVICE_FW_VER =     0x23;       ///< TLV Sub type for getting firmware version form wearable
	public static final byte CHECK_VALID_FW_VER =    0x24;       ///< TLV Sub type for invoking phNxpNciHal_CheckValidFwVersion at wearable
	public static final byte GET_CLK_FREQ =          0x25;       ///< TLV Sub type for getting clock frequency form wearable
	public static final byte TML_WRITE_REQ =         0x26;       ///< TLV Sub type for invoking phTmlNfc_Write at wearable
	public static final byte TML_READ_REQ =          0x27;       ///< TLV Sub type for invoking phTmlNfc_Read at wearable
	public static final byte FW_DND_POST =           0x28;       ///< TLV Sub type for for reinitializing wearable post to firmware download
	public static final byte GET_DEVICE_INFO =       0x29;       ///< TLV Sub type for for getting wearable device info before firmware download
	public static final byte RESET_WEARABLE_DEV =    0x2A;       ///< TLV Sub type for for resetting wearable device after firmware download.
	

	public static final byte WEARABLE_LS_LTSM = 0x08;     ///< This is TLV Type for executing LS LTSM clients at wearable from companion.
	public static final byte LS_EXECUTE_SCRIPT = (byte) 0x81;       ///< TLV Sub type for executing LS script at wearable sent by companion.
	public static final byte CREATE_VC = (byte) 0x82;       ///< TLV Sub type for creating mifare card.
	public static final byte DELETE_VC = (byte) 0x83;       ///< TLV Sub type for deleting mifare card.
	public static final byte ACTIVATE_VC = (byte) 0x84;       ///< TLV Sub type for activating mifare card.
	public static final byte DEACTIVATE_VC = (byte) 0x85;       ///< TLV Sub type for deactivating mifare card.
	public static final byte SET_MDAC_VC = (byte) 0x86;       ///< TLV Sub type for update MDAC.
	public static final byte READ_VC = (byte) 0x87;       ///< TLV Sub type for read VC.
	public static final byte GET_VC_LIST = (byte) 0x88;       ///< TLV Sub type for VC List.
	public static final byte GET_VC_STATUS = (byte) 0x89;       ///< TLV Sub type for VC Status.

	public static int getLength(byte[] value) {
		if (value[0] == 0x82) {
			return (value[1] << 8) + value[2];
		} else if (value[0] == 0x81) {
			return value[1];
		} else {
			return value[0];
		}
	}
	public static void parseTlvCommand(final byte[] value, int index) throws JSONException, Exception {
		Log.v(TAG, "Executing: parse " + Parsers.arrayToHex(value));

		// Success / Error
		if (
			(((byte)value[index+0] & 0xFF) == 0x4E) &&
			(((byte)value[index+1] & 0xFF) == 0x02)
		) {
			if (
				(((byte)value[index+2] & 0xFF) == 0x90) &&
				(((byte)value[index+3] & 0xFF) == 0x00)
			) {
				Log.v(TAG, "Executing: parse success");
			} else {
				Log.v(TAG, "Executing: parse error");
				parseTlvCommand_Error(value, index);
			}
		} else {
			Log.v(TAG, "Executing: unexpected error");
		}
	}
	public static void parseTlvCommand_Error(final byte[] value, int index) throws Exception {
		Log.v(TAG, "Executing: parseTlvCommand_Error (" + index + ") " + Parsers.arrayToHex(value));

		// No error
		if (
			(((byte)value[index+0] & 0xFF) == 0x4E) &&
			(((byte)value[index+1] & 0xFF) == 0x02)
		) {
			index+=2;
			
			// 0x9000
			if (
				(((byte)value[index+0] & 0xFF) == 0x90) &&
				(((byte)value[index+1] & 0xFF) == 0x00)
			) {
				index+=2;
				Log.v(TAG, "Executing: parse success");
				return;
			}

			// 0x6230
			if (
				(((byte)value[index+0] & 0xFF) == 0x62) &&
				(((byte)value[index+1] & 0xFF) == 0x30)
			) {
				index+=2;
				throw new Exception("The state of the VC cannot be changed");
			}

			// 0x6985
			if (
				(((byte)value[index+0] & 0xFF) == 0x69) &&
				(((byte)value[index+1] & 0xFF) == 0x85)
			) {
				index+=2;
				throw new Exception("Wrong free VC entry number, caller is not the allowed or the state is not activable");
			}

			// 0x69E8
			if (
				(((byte)value[index+0] & 0xFF) == 0x69) &&
				(((byte)value[index+1] & 0xFF) == 0xE8)
			) {
				index+=2;
				throw new Exception("Failed to select of the CRS application");
			}

			// 0x6A88
			if (
				(((byte)value[index+0] & 0xFF) == 0x6A) &&
				(((byte)value[index+1] & 0xFF) == 0x88)
			) {
				index+=2;
				throw new Exception("Reference data does not exist");
			}
		}
		
		return;
	}
	public static byte[] getTlvCommand(byte type, byte subtype, byte[] value) {
		Log.v(TAG, "Executing: getTlvCommand " + type + "/" + subtype + ": " + Parsers.arrayToHex(value));

		int lengthLength = 0;
		int valueLength = value.length + 1; // 1
		
		if(valueLength <= 127) {
			lengthLength = 1;
		} else {
			if(valueLength > 255) {
				lengthLength = 3;
			} else {
				lengthLength = 2;
			}
		}
		
		byte[] dataBT = new byte[1 + lengthLength + 1 + value.length]; // 4
		
		dataBT[0] = type;
		dataBT[1 + lengthLength] = subtype;
		System.arraycopy(value, 0, dataBT, 1 + lengthLength + 1, value.length);
		
		if(valueLength <= 127) {
			dataBT[1] = (byte) valueLength;
		} else {
			if(valueLength > 255) {
				dataBT[1] = (byte) 0x82;
				dataBT[2] = (byte) (valueLength >> 8 & 0xFF);
				dataBT[3] = (byte) (valueLength >> 0 & 0xFF);
			} else {
				dataBT[1] = (byte) 0x81;
				dataBT[2] = (byte) valueLength;
			}
		}
		
	    return dataBT;
	}
}
