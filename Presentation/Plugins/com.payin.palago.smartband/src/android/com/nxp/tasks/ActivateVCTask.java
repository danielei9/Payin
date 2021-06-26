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

import org.apache.cordova.CordovaInterface;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

public class ActivateVCTask extends AsyncTask<String, Integer, Boolean> implements OnOperationListener {
	public final static String TAG = "ActivateVCTask";
	public static final int ACTIVATE_VC_TIMEOUT = 10000;
	
	private CordovaInterface cordova;
	private Bluetooth bluetooth;
	private int id;	

	OnTransmitApduListener listener;

	public ActivateVCTask(CordovaInterface cordova, Bluetooth bluetooth, int id) {
		this.cordova = cordova;
		this.bluetooth = bluetooth;
		this.id = id;
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
        byte[] data = { (byte) id };
        byte[] dataBT = BluetoothTLV.getTlvCommand(BluetoothTLV.WEARABLE_LS_LTSM, BluetoothTLV.ACTIVATE_VC, data); 
        
        // Execute the APDU 
        if(isCancelled() == false)
        	listener.sendApduToSE(dataBT, ACTIVATE_VC_TIMEOUT);
     	
     	return false;
	}
	/////////////////////////////////////////
	// OnOperationListener
	/////////////////////////////////////////
	@Override
	public void processOperationResult(byte[] data) {
		Log.d(TAG, "processOperationResult " + Parsers.arrayToHex(data));
		
		try {
			BluetoothTLV.parseTlvCommand_Error(data, 0);

        	JSONObject json = new JSONObject();
            json.put("id", id);

			bluetooth.sendToIonic("card-activation", json);
			Toast.makeText(cordova.getActivity(), "Card activated", Toast.LENGTH_LONG).show();
		} catch (Exception ex) {}
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
