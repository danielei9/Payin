package com.nxp.tasks;

import android.content.Context;
import android.content.ContentValues;
import android.content.Intent;
import android.os.AsyncTask;
import android.util.Log;
import android.widget.Toast;

import com.payin.palago.Bluetooth;

import com.nxp.arguments.TsmPersonalizeArguments;
import com.nxp.listeners.OnOperationListener;
import com.nxp.listeners.OnTransmitApduListener;
import com.nxp.httpcomm.HttpTransaction;
import com.nxp.ssdp.btclient.BluetoothTLV;
import com.nxp.results.TsmResults;
import com.nxp.utils.Parsers;

import java.util.LinkedList;
import java.util.Random;

import microsoft.aspnet.signalr.client.hubs.HubConnection;
import microsoft.aspnet.signalr.client.hubs.HubProxy;
import microsoft.aspnet.signalr.client.hubs.SubscriptionHandler1;
import microsoft.aspnet.signalr.client.transport.ClientTransport;
import microsoft.aspnet.signalr.client.transport.ServerSentEventsTransport;

import org.apache.cordova.CordovaInterface;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

public final class NfcHttpProxyTask extends AsyncTask<Intent, Void, StringBuffer> implements OnOperationListener {
	private static final String TAG = "NfcHttpProxyTask";
	private static final String serverUrl = "http://localhost:8080/Tsm/v1/Personalize";
	private static final String hubUrl = "http://localhost:8080/signalr";
	private static final String cardName = "Mobilis";

	private StringBuffer console;
	private String userName = "XP";
	private String randId;
	private String errorCause = "";
	
	private CordovaInterface cordova;

	private OnTransmitApduListener listener;
	
	static byte seResponse[] = null;

	public NfcHttpProxyTask(CordovaInterface cordova, Bluetooth bluetooth, String randId) {
		Log.d(TAG, "constructor");

		this.cordova = cordova;
		this.randId = randId;

		this.listener = (OnTransmitApduListener) bluetooth;
		this.listener.setOnOperationListener(this);

		console = new StringBuffer("");
	}

	@Override
	protected void onPreExecute() {
		Log.d(TAG, "onPreExecute");
		super.onPreExecute();
		
		// // Set the operation delegate
		// BaseActivity.setOperationDelegate((OnOperationListener) ctx);
		
		// // Make sure no other operations are completed until this one is completed
		// MyPreferences.setCardOperationOngoing(ctx, true);
	}

	@Override
	protected StringBuffer doInBackground(final Intent... args) {
		try {
			Log.d(TAG, "doInBackground: ENTER");

			// Before start sending perso APDUs I need to open the wired mode
			byte[] enableWiredModeTLV = BluetoothTLV.getTlvCommand(
				BluetoothTLV.SE_MODE,
				BluetoothTLV.WIREDMODE_ENABLE,
				new byte[] { 0x01 }
			); 
			if(isCancelled() == false)
				listener.sendApduToSE(enableWiredModeTLV, 4000);
			Log.d(TAG, "doInBackground: wiring mode on");

			seResponse = null;
			while (isCancelled() == false) {
				if (seResponse != null) {
					if (seResponse[0] == 0x00)
						break;
					else
						console.append("\nError opening channel\n");
					return console;
				}
			}
			Log.d(TAG, "doInBackground: wired mode on");

			// SIGNALR
			Log.d(TAG, "SIGNALR starting");
			HubConnection connection = new HubConnection(hubUrl);
			HubProxy proxy = connection.createHubProxy("tsmHub");
			ClientTransport transport = new ServerSentEventsTransport(connection.getLogger());
			proxy.on(
				"personalizeResponse",
				new SubscriptionHandler1<TsmResults>() {
					@Override
					public void run(TsmResults result) {
						Log.d(TAG, "SIGNALR result: " + result);
						Response(result);
					}
				},
				TsmResults.class
			);
			connection.start(transport).get();
			
			Log.i(TAG, "Starting new http Transaction");
			String transactionId = Integer.toHexString(new Random().nextInt());

			TsmPersonalizeArguments arguments = new TsmPersonalizeArguments();
			arguments.type = "InitTransaction";
			arguments.transactionId = transactionId;
			arguments.data = cardName;
			arguments.user = userName;
			arguments.id = randId;
			arguments.state	= null;
			TsmResults response = proxy.invoke(TsmResults.class, "personalize", arguments).get();

			while (response != null)
			{
				Log.d(TAG, "SIGNALR response: " + response);
				seResponse = null;

				if(isCancelled() == false)
				{
					Log.i(TAG, "APDU Send: " + response.data);
					byte[] dataBT = BluetoothTLV.getTlvCommand(BluetoothTLV.SE_MODE, BluetoothTLV.WIRED_TRANSCEIVE, Parsers.hexToArray(response.data));
					listener.sendApduToSE(dataBT, 4000);
				}
				
				// Wait
				while (seResponse == null) {}
				
				byte[] temp = new byte[seResponse.length - 1];
				System.arraycopy(seResponse, 0, temp, 0, seResponse.length - 1);
				seResponse = temp;
				
				String seResponseString = Parsers.arrayToHex(seResponse);

				arguments = new TsmPersonalizeArguments();
				arguments.type = "InitTransaction";
				arguments.transactionId = transactionId;
				arguments.data = seResponseString;
				arguments.state	= response.state;
				// public string User { get; set; } // "Pay in"
				// public string Id { get; set; } // randId
				arguments.nextStep	= response.nextStep;

				response = proxy.invoke(TsmResults.class, "personalize", arguments).get();
			}
		} catch (Exception e) {
			console.append("\n" + errorCause + "\n");
			Log.e(TAG, errorCause);
			e.printStackTrace();
		}
		
		// // Make sure no other operations are completed until this one is completed
		// MyPreferences.setCardOperationOngoing(ctx, true);
		
		byte[] disableWiredModeTLV = BluetoothTLV.getTlvCommand(BluetoothTLV.SE_MODE, BluetoothTLV.WIREDMODE_ENABLE, new byte[] {0x00} ); 
		if(isCancelled() == false)
			listener.sendApduToSE(disableWiredModeTLV, 4000);
		Log.d(TAG, "doInBackground: wiring mode off");
		
		seResponse = null;
		while(isCancelled() == false) {
			if(seResponse != null) {
				break;
			}
		}
		Log.d(TAG, "doInBackground: wired mode off");

		// return console;
		return null;
	}
	protected TsmResults Response(TsmResults arguments) {
		String apdu = arguments.data;
		String transactionId = arguments.transactionId;

		Log.i(TAG, "apdu: " + apdu);
	
		// Send command to SE
		if(isCancelled() == false) {
			byte[] dataBT = BluetoothTLV.getTlvCommand(BluetoothTLV.SE_MODE, BluetoothTLV.WIRED_TRANSCEIVE, Parsers.hexToArray(apdu)); 
			listener.sendApduToSE(dataBT, 4000);
		}
		
		// Wait response
		seResponse = null;
		while(isCancelled() == false) {
			if (seResponse != null) {
				// Eliminar ultimo byte
				byte[] temp = new byte[seResponse.length - 1];
				System.arraycopy(seResponse, 0, temp, 0, seResponse.length - 1);
				seResponse = temp;
					
				String seResponseString = Parsers.arrayToHex(seResponse);
				
				TsmResults result = new TsmResults();
				result.type = "ResponseApdu";
				result.transactionId = transactionId;
				result.data = seResponseString;
				
				return result;
			}
		}

		return null;
	}
	@Override
	protected void onPostExecute(final StringBuffer result) {
		super.onPostExecute(result);

		//((SevenElevenActivity) ctx).proccessTransactionTaskResult(result);
	}
	@Override
	protected void onCancelled() {

	}

	/////////////////////////////////////////
	// OnOperationListener
	/////////////////////////////////////////
	@Override
	public void processOperationResult(byte[] data) {
		Log.d(TAG, "processOperationResult " + Parsers.arrayToHex(data));

		seResponse = data;

		// try {
        // 	JSONObject json = new JSONObject();
		// 	json.put("id", randId);

		// 	int index = 0;

		// 	// Entry
		// 	if (
		// 		(((byte)data[index+0] & 0xFF) == 0x40) &&
		// 		(((byte)data[index+1] & 0xFF) == 0x02)
		// 	) {
		// 		Log.d(TAG, "entry " + (((byte)data[index+2]) << 8) + data[index+3]);
		// 		json.put("entry", (((byte)data[index+2]) << 8) + data[index+3]);
		// 		index += 4;
		// 	}

		// 	// UID
		// 	if (
		// 		(((byte)data[index+0] & 0xFF) == 0x41)
		// 	) {
		// 		int length = data[index+1];
		// 		String val = "";
		// 		for(byte i = 0; i < length; i++)
		// 			val += Parsers.arrayToHex(new byte[] { data[i] });

		// 		Log.d(TAG, "uid " + val);
		// 		json.put("uid", val);
		// 		index += length + 2;
		// 	}

		// 	BluetoothTLV.parseTlvCommand(data, index);

		// 	// Success
		// 	JSONArray result = new JSONArray();
		// 	result.put(json);

		// 	AsyncTask<?, ?, ?> task = new NfcHttp7ElevenProxyTask(
		// 		cordova,
		// 		//"http://www.themobileknowledge.com/Servlets/RemotePerso/M4mServletMain",
		// 		//"7Eleven",
		// 		//login,
		// 		bluetooth,
		// 		randId
		// 	)
		// 	.execute();

		// 	Log.d(TAG, "CreateCardVcTask success " + result);
		// 	callbackContext.success(result);
		// } catch (Exception ex) {
        //     ex.printStackTrace();

		// 	Log.d(TAG, "processOperationResult error: " + ex.getMessage());
        //     callbackContext.error(ex.getMessage());
		// }
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