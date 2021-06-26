package com.nxp.tasks;

import android.content.Context;
import android.os.AsyncTask;
import android.util.Log;
import android.widget.Toast;

import com.nxp.listeners.OnOperationListener;
import com.nxp.listeners.OnTransmitApduListener;
import com.nxp.ssdp.btclient.BluetoothTLV;
import com.nxp.ssdp.encryption.JSBLEncryption;
import com.nxp.tasks.NfcHttpProxyTask;
import com.nxp.utils.Parsers;

import com.payin.palago.Bluetooth;

import java.util.ArrayList;
import java.util.List;

import org.apache.cordova.CallbackContext;
import org.apache.cordova.CordovaInterface;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

public class CreateCardVCTask extends AsyncTask<String, Integer, Boolean> implements OnOperationListener {
	public final static String TAG = "CreateCardVCTask";
	public static final int TIMEOUT = 10000;

	public static final String SP_SD_KVN1 = "48"; // Must not be "00"
	public static final String SP_SD_ENC1 = "606162636465666768696A6B6C6D6E6F";
	public static final String SP_SD_MAC1 = "606162636465666768696A6B6C6D6E6F";
	public static final String SP_SD_DEK1 = "606162636465666768696A6B6C6D6E6F";

	//String createVCData; // "460102A50702020101030100A60705020400060108A11F80080FFFFFFFFFFFFFFF810100820200008301008401008501008603000000A8122010AAAAAAAAAAAA08778F00313131313131";
	// String persoVCData = 
	// 	"4210A0000003964D344D240081DB69020008" +
	// 	"4310A0000003964D344DA40081DB69010008" +
	// 	"470100";

	private static JSBLEncryption jsblEncryption;
	public static String JSBL_KEY_FILENAME = "";

	private static String MIFARE_CLASSIC = "01";
	private static String MIFARE_CLASSIC_1KB = MIFARE_CLASSIC + "01";
	private static String MIFARE_CLASSIC_4KB = MIFARE_CLASSIC + "04";
	private static String MIFARE_DESFIRE = "02";
	private static String MIFARE_DESFIRE_2KB = MIFARE_DESFIRE + "02";
	private static String MIFARE_DESFIRE_4KB = MIFARE_DESFIRE + "04";
	private static String MIFARE_DESFIRE_8KB = MIFARE_DESFIRE + "08";

	String randId;
	String login = "user@pay-in.es";
	String vcTypeValue = MIFARE_CLASSIC_1KB;
	//String vcUidValue = "00"; // 7 bytes
	String vcUidValue = "01"; // 4 bytes
	String vcKeysetValue = "FFFFFFFFFFFFFF078069FFFFFFFFFFFF"; // Default keys
	String vcDesfCryptoValue = "00";
	String vcKeyVersionValue = "";

	String command = "";
	
	private CordovaInterface cordova;
	private Bluetooth bluetooth;
	private CallbackContext callbackContext;

	OnTransmitApduListener listener;

	public CreateCardVCTask(CordovaInterface cordova, Bluetooth bluetooth, CallbackContext callbackContext, String randId) {
		Log.d(TAG, "constructor");
		
		this.cordova = cordova;
		this.bluetooth = bluetooth;
		this.callbackContext = callbackContext;
		this.randId = randId;

		this.listener = (OnTransmitApduListener) bluetooth;
		this.listener.setOnOperationListener(this);

		JSBL_KEY_FILENAME = "keyfile.txt";
		
		// Load the JSBL keys
		jsblEncryption = new JSBLEncryption(JSBL_KEY_FILENAME, cordova.getActivity());
	}
	@Override
	protected void onPreExecute() {
		try {
			Log.v(TAG, "Executing: onPreExecute");
			
			List<String> listApps = new ArrayList<String>();

			// VC Configuration
			//command = command.concat("460100"); // No perso, No Remotelly managed
			command = command.concat("460102"); // No perso, Remotelly managed

			// VC Definition
			command = command.concat("A507");
			command = command.concat("0202" + vcTypeValue); // VC type & size
			command = command.concat("0301" + vcUidValue); // VC Uid option

			// Protocol parameters
			if(vcTypeValue.equals(MIFARE_CLASSIC_1KB) == true) {
				command = command.concat("A607");
				command = command.concat("05020400"); // ATQA
				command = command.concat("060108"); // SAK
			} else if(vcTypeValue.equals(MIFARE_CLASSIC_4KB) == true) {
				command = command.concat("A607");
				command = command.concat("05020200"); // ATQA
				command = command.concat("060118"); // SAK
			} else if(vcTypeValue.equals(MIFARE_DESFIRE_2KB) == true) {
				command = command.concat("A607");
				command = command.concat("05024403"); // ATQA
				command = command.concat("060120"); // SAK
			} else if(vcTypeValue.equals(MIFARE_DESFIRE_4KB) == true) {
				command = command.concat("A607");
				command = command.concat("05024403"); // ATQA
				command = command.concat("060120"); // SAK
			} else if(vcTypeValue.equals(MIFARE_DESFIRE_8KB) == true) {
				command = command.concat("A607");
				command = command.concat("05024403"); // ATQA
				command = command.concat("060120"); // SAK
			}

			// Parameter mask
			if(vcUidValue.equals("00") == true) {
				command = command.concat("A11F");
				command = command.concat("80080FFFFFFFFFFFFFFF"); // LV UID
				command = command.concat("810100"); // SAK
				command = command.concat("82020000"); // ATQA
				command = command.concat("830100"); // ATS
				command = command.concat("840100"); // FWI, SFGI
				command = command.concat("850100"); // CID_SUPPORT
				command = command.concat("8603000000"); // DATARATE_MAX
			} else {
				command = command.concat("A11C");
				command = command.concat("80050FFFFFFFFF"); // LV UID
				command = command.concat("81011F"); // SAK
				command = command.concat("82020000"); // ATQA    // Tenia FFFF
				command = command.concat("830100"); // ATS
				command = command.concat("840100"); // FWI, SFGI
				command = command.concat("850100"); // CID_SUPPORT
				command = command.concat("8603000000"); // DATARATE_MAX
			}

			String cryptoValue = "";
			if(vcTypeValue.startsWith(MIFARE_CLASSIC)) {
				cryptoValue = "A8122010" + vcKeysetValue;
			} else if(vcTypeValue.startsWith(MIFARE_DESFIRE)) {
				if(vcDesfCryptoValue.equals("00")) {
					cryptoValue = "A813201100" + vcKeysetValue;
				} else if(vcDesfCryptoValue.equals("01")) {
					cryptoValue = "A821201901" + vcKeysetValue;
				} else if(vcDesfCryptoValue.equals("02")) {
					cryptoValue = "A814201200" + vcKeysetValue + (vcKeyVersionValue.length() == 2 ? vcKeyVersionValue : "00");
				}
			}	
			command = command.concat(cryptoValue);
			
			if(vcTypeValue.startsWith("02")) {
				String concurrentActivation = "E2020001";
				command = command.concat(concurrentActivation);
				
				String desfAids = "F8";
							
				desfAids = desfAids.concat("00".substring(Integer.toHexString(listApps.size() * 3).length()) + Integer.toHexString(listApps.size() * 3));
					
				for(String aid : listApps)
					desfAids = desfAids.concat(aid);
					
				command = command.concat(desfAids);
			}

			super.onPreExecute();
		} catch (Exception ex) {
            ex.printStackTrace();
            callbackContext.error(ex.getMessage());
		}
	}
	@Override
	protected Boolean doInBackground(String... scripts) {
		Log.v(TAG, "Executing: doInBackground");
		byte[] data = Parsers.parseHexProperty("VCCreateCommand", command);

		String persoVCData = 
			"4210A0000003964D344D240081DB690200" + randId +
			"4310A0000003964D344DA40081DB690100" + randId +
			"470100";

		Log.v(TAG, "Executing: doInBackground SP_SD_KVN1 " + SP_SD_KVN1);
		Log.v(TAG, "Executing: doInBackground SP_SD_ENC1" + SP_SD_ENC1);
		Log.v(TAG, "Executing: doInBackground SP_SD_MAC1" + SP_SD_MAC1);
		Log.v(TAG, "Executing: doInBackground SP_SD_DEK1" + SP_SD_DEK1);

		byte[] keySet = Parsers.hexToArray(SP_SD_KVN1 + SP_SD_ENC1 + SP_SD_MAC1 + SP_SD_DEK1);

		byte[] persoKeys = jsblEncryption.getEncryptedTLV(keySet, (byte) 0x61);
		persoVCData = persoVCData.concat(Parsers.bytArrayToHex(persoKeys));
		byte[] PersonalizeData = Parsers.hexToArray(persoVCData);
		
		// VC Creation + Perso data
		byte[] vcCommand = new byte[data.length + PersonalizeData.length];
		System.arraycopy(data, 0, vcCommand, 0, data.length);
		System.arraycopy(PersonalizeData, 0, vcCommand, data.length, PersonalizeData.length);
		
		// Store the final commands and the VC Creation Data
		data = vcCommand;

		byte[] dataBT = BluetoothTLV.getTlvCommand(BluetoothTLV.WEARABLE_LS_LTSM, BluetoothTLV.CREATE_VC, data); 

        // Execute the APDU 
        if(isCancelled() == false)
        	listener.sendApduToSE(dataBT, TIMEOUT);
     	
     	return false;
	}
	// @Override
	// protected void onPostExecute(final Boolean result) {
	// 	super.onPostExecute(result);

	// 	new NfcHttp7ElevenProxyTask(cordova, bluetooth, randId)
	// 		.execute();
	// }
	/////////////////////////////////////////
	// OnOperationListener
	/////////////////////////////////////////
	@Override
	public void processOperationResult(byte[] data) {
		Log.d(TAG, "processOperationResult " + Parsers.arrayToHex(data));

		try {
        	JSONObject json = new JSONObject();
			json.put("id", randId);

			int index = 0;

			// Entry
			if (
				(((byte)data[index+0] & 0xFF) == 0x40) &&
				(((byte)data[index+1] & 0xFF) == 0x02)
			) {
				Log.d(TAG, "entry " + (((byte)data[index+2]) << 8) + data[index+3]);
				json.put("entry", (((byte)data[index+2]) << 8) + data[index+3]);
				index += 4;
			}

			// UID
			if (
				(((byte)data[index+0] & 0xFF) == 0x41)
			) {
				int length = data[index+1];
				String val = "";
				for(byte i = 0; i < length; i++)
					val += Parsers.arrayToHex(new byte[] { data[i] });

				Log.d(TAG, "uid " + val);
				json.put("uid", val);
				index += length + 2;
			}

			BluetoothTLV.parseTlvCommand(data, index);

			// Success
			JSONArray result = new JSONArray();
			result.put(json);

			AsyncTask<?, ?, ?> task = new NfcHttpProxyTask(
				cordova,
				//"http://www.themobileknowledge.com/Servlets/RemotePerso/M4mServletMain",
				//"7Eleven",
				//login,
				bluetooth,
				randId
			)
			.execute();

			Log.d(TAG, "CreateCardVcTask success " + result);
			callbackContext.success(result);
		} catch (Exception ex) {
            ex.printStackTrace();

			Log.d(TAG, "processOperationResult error: " + ex.getMessage());
            callbackContext.error(ex.getMessage());
		}
	}
	@Override
	public void processOperationNotCompleted() {
		Log.d(TAG, "processOperationNotCompleted");
		// switch (mAction) {
		// case ACTION_GET_VERSION:
		// 	fwVersion.setText(String.format(getResources().getString(R.string.fw_version_not_available)));
		// 	break;
			
		// case ACTION_EXECUTE_SCRIPT:
		// 	// Close the dialog
		// 	progressdialog.dismiss();
			
		// 	break;
		// }
		
		// Intent broadcast = new Intent();
		// broadcast.putExtra(MyCardsActivity.BROADCAST_EXTRA, Card.STATUS_FAILED);
        // broadcast.setAction(MyCardsActivity.BROADCAST_ACTION);
        // sendBroadcast(broadcast);
        
        Toast.makeText(cordova.getActivity(), "Error detected in BLE channel", Toast.LENGTH_LONG).show();
	}
}
