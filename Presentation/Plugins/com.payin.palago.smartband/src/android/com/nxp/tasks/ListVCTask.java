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

import org.apache.cordova.CordovaInterface;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

public class ListVCTask extends AsyncTask<String, Integer, Boolean> implements OnOperationListener {
	public final static String TAG = "ListVCTask";
	public static final int TIMEOUT = 10000;
	
	private CordovaInterface cordova;
	private Bluetooth bluetooth;

	OnTransmitApduListener listener;

	public ListVCTask(CordovaInterface cordova, Bluetooth bluetooth) {
		this.cordova = cordova;
		this.bluetooth = bluetooth;
		this.listener = (OnTransmitApduListener) bluetooth;
		this.listener.setOnOperationListener(this);
	}
	
	@Override
	protected void onPreExecute() {
		Log.v(TAG, "Executing: onPreExecute");
		super.onPreExecute();
		
		// Set the operation delegate
		//BaseActivity.setOperationDelegate((OnOperationListener) ctx);
		
		// Make sure no other operations are completed until this one is completed
		//MyPreferences.setCardOperationOngoing(ctx, true);
	}
	@Override
	protected Boolean doInBackground(String... scripts) {
		Log.v(TAG, "Executing: doInBackground");
		byte[] data = { 0x00 };
		byte[] dataBT = BluetoothTLV.getTlvCommand(BluetoothTLV.WEARABLE_LS_LTSM, BluetoothTLV.GET_VC_LIST, data);
        
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
			List<JSONObject> list = new ArrayList<JSONObject>();

			int index = 0;

			while (((byte)data[index]) == 0x61)
			{
				index++;

				// Get length
				int length = 0;
				if (((byte)data[index]) == 0x82) {
					length = (data[index+1] << 8) + data[index+2];
					index += 3;
				} else if (((byte)data[index]) == 0x81) {
					length = data[index+1];
					index += 2;;
				} else {
					length = data[index];
					index++;
				}
				Log.v(TAG, "Executing: parse(" + index + ") 0x82");

				// Read uid
				int id = 0;
				if (
					(((byte)data[index]) == 0x40) && 
					(((byte)data[index+1]) == 0x02)
				)
				{
					id = (((byte)data[index+2]) << 8) + data[index+3];
					index+=4;
				}
				Log.v(TAG, "Executing: parse(" + index + ") id:" + id);

				// Read app
				String appHash = "";
				if (((byte)data[index]) == 0x4F)
				{
					index++;
					Log.v(TAG, "Executing: parse(" + index + ") 0x4F");

					// Get length
					int lengthApp = 0;
					if (((byte)data[index]) == 0x82) {
						lengthApp = (data[index+1] << 8) + data[index+2];
						index+=3;
					} else if (((byte)data[index]) == 0x81) {
						lengthApp = data[index+1];
						index+=2;
					} else {
						lengthApp = data[index];
						index++;
					}

					byte[] temp = new byte[lengthApp];
					System.arraycopy(data, index, temp, 0, lengthApp);
					appHash = Parsers.arrayToHex(temp);
					index+=lengthApp;
				}
				Log.v(TAG, "Executing: parse(" + index + ") appHash:" + appHash);
				
				JSONObject json = new JSONObject();
				json.put("id", id);
				json.put("appHash", appHash);
				list.add(json);
			}

			BluetoothTLV.parseTlvCommand(data, 0);
			
			JSONArray result = new JSONArray(list);
			bluetooth.sendToIonic("card-detection", result);
			Toast.makeText(cordova.getActivity(), "List obtained", Toast.LENGTH_LONG).show();
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
