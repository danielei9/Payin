package com.nxp.tasks;

import android.content.Context;
import android.os.AsyncTask;
import android.util.Log;
import android.widget.Toast;

import com.nxp.listeners.OnOperationListener;
import com.nxp.listeners.OnTransmitApduListener;
import com.nxp.ssdp.btclient.BluetoothTLV;
import com.nxp.utils.Parsers;

import com.payin.palago.Bluetooth;

import java.util.ArrayList;
import java.util.List;

import org.apache.cordova.CallbackContext;
import org.apache.cordova.CordovaInterface;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

public class DeleteCardVCTask extends AsyncTask<String, Integer, Boolean> implements OnOperationListener {
	public final static String TAG = "CreeateCardVCTask";
	public static final int TIMEOUT = 10000;

	String vcTypeValue = "0101"; // MIFARE Classic 1kB
	//String vcUidValue = "00"; // 7 bytes
	String vcUidValue = "01"; // 4 bytes
	String vcKeysetValue = "FFFFFFFFFFFFFF078069FFFFFFFFFFFF"; // Default keys
	String vcDesfCryptoValue = "00";
	String vcKeyVersionValue = "";

	String command = "";
	
	private CordovaInterface cordova;
	private Bluetooth bluetooth;
	private CallbackContext callbackContext;
	private int entry;

	OnTransmitApduListener listener;

	public DeleteCardVCTask(CordovaInterface cordova, Bluetooth bluetooth, CallbackContext callbackContext, int entry) {
		this.cordova = cordova;
		this.bluetooth = bluetooth;
		this.callbackContext = callbackContext;
		this.entry = entry;
		this.listener = (OnTransmitApduListener) bluetooth;
		this.listener.setOnOperationListener(this);
	}
	@Override
	protected void onPreExecute() {
		Log.v(TAG, "Executing: onPreExecute");
		super.onPreExecute();
	}
	@Override
	protected Boolean doInBackground(String... scripts) {
		Log.v(TAG, "Executing: doInBackground");
        byte[] data = { (byte) entry };
		byte[] dataBT = BluetoothTLV.getTlvCommand(BluetoothTLV.WEARABLE_LS_LTSM, BluetoothTLV.DELETE_VC, data);

        // Execute the APDU
        if(isCancelled() == false)
        	listener.sendApduToSE(dataBT, TIMEOUT);
     	
     	return false;
	}
	/////////////////////////////////////////
	// OnOperationListener
	/////////////////////////////////////////
	@Override
	public void processOperationResult(byte[] data) {
		Log.d(TAG, "processOperationResult " + Parsers.arrayToHex(data));
		
		try {
        	JSONObject json = new JSONObject();

			int index = 0;

			// Entry
			if (
				(((byte)data[index+0] & 0xFF) == 0x40) &&
				(((byte)data[index+1] & 0xFF) == 0x02)
			) {
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

				json.put("uid", val);
				index += length + 2;
			}

			BluetoothTLV.parseTlvCommand(data, index);

			// Success
			JSONArray result = new JSONArray();
			result.put(json);

			Log.d(TAG, "processOperationResult success: " + result);
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
