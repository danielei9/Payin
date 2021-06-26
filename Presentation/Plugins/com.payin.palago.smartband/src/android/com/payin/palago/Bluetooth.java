package com.payin.palago;

import android.app.Activity;
import android.app.AlertDialog;
import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;
import android.bluetooth.BluetoothManager;
import android.bluetooth.le.ScanCallback;
import android.bluetooth.le.ScanResult;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.Manifest;
import android.os.Handler;
import android.support.v4.content.ContextCompat;
import android.support.v4.app.ActivityCompat;
import android.util.Log;
import android.widget.ArrayAdapter;
import android.widget.Toast;

import com.nxp.listeners.OnOperationListener;
import com.nxp.listeners.OnTransmitApduListener;
import com.nxp.ssdp.btclient.BluetoothClient;
import com.nxp.ssdp.btclient.BluetoothClient.OnBluetoothConnectListener;
import com.nxp.ssdp.btclient.BluetoothClient.OnBluetoothConnectionPendingListener;
import com.nxp.ssdp.btclient.BluetoothClient.OnBluetoothReadListener;
import com.nxp.ssdp.btclient.BluetoothClient.OnBluetoothWriteListener;
import com.nxp.ssdp.btclient.BluetoothDiscovery;
import com.nxp.ssdp.btclient.BluetoothTLV;
import com.nxp.utils.Parsers;
import com.quintic.libqpp.QppApi;

import org.apache.cordova.CallbackContext;
import org.apache.cordova.CordovaInterface;
import org.apache.cordova.CordovaWebView;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.text.MessageFormat;
import java.util.ArrayList;
import java.util.List;
import java.util.Set;

public class Bluetooth implements 
	OnBluetoothConnectListener, 
	OnBluetoothConnectionPendingListener, 
	OnBluetoothReadListener, 
	OnBluetoothWriteListener,
	OnTransmitApduListener
{
    public static final String TAG = "Bluetooth";

	// Timer to wait for an operation to be completed (operation specific)
	public static int TIMER_BLE_OP;

	public static final int REQUEST_ENABLE_BT = 0x100;
	public static final int REQUEST_CONNECT_DEVICE = 0x101;
	
    // Stops scanning after 10 seconds.
 	private static final long SCAN_PERIOD = 60000;

 	public static final String AUTO_CONNECT = "auto_connect";

	private CordovaInterface mCordova;
	private CordovaWebView mWebView;
	private BluetoothAdapter mAdapter;
	private static BluetoothClient mClient;

	private Handler mHandler;
	private static BluetoothClient mBluetooth;

    private boolean mScanning;
    private CallbackContext mGetAllCardsCallback = null;

	// onRead
	private int mBufferOffset = 0;
	private int mExpectedSize = 0;
	private byte[] mBufferDataCmd = null;

 	private BluetoothAdapter.LeScanCallback mLeScanCallback = new BluetoothAdapter.LeScanCallback() {
 		@Override
 		public void onLeScan(final BluetoothDevice device, final int rssi, byte[] scanRecord) {
			Log.v(TAG, "Executing: onLeScan");
 			mCordova.getActivity().runOnUiThread(new Runnable() {
 				@Override
 				public void run() {
 					if(device.getName() != null) {
						onConnectDetection(device.getAddress(), device.getName(), false);
 					}
 				}
 			});
 		}
 	};
	public Bluetooth(CordovaInterface cordova, CordovaWebView webView) {
		mCordova = cordova;
		mWebView = webView;
	}
	private boolean initialize() {
        // Android M Permission check
		int permissionCheck = ContextCompat.checkSelfPermission(mCordova.getActivity(), Manifest.permission.ACCESS_COARSE_LOCATION);
		if (permissionCheck != PackageManager.PERMISSION_GRANTED) {
			Log.v(TAG, "Initialize: permissionCheck NO GRANTED " + permissionCheck);
			// Should we show an explanation?
			if (ActivityCompat.shouldShowRequestPermissionRationale(mCordova.getActivity(), Manifest.permission.ACCESS_COARSE_LOCATION)) {
				Log.v(TAG, "Initialize: shouldShowRequestPermissionRationale true");
				// Show an expanation to the user *asynchronously* -- don't block
				// this thread waiting for the user's response! After the user
				// sees the explanation, try again to request the permission.
			} else {
				Log.v(TAG, "Initialize: shouldShowRequestPermissionRationale false");
				ActivityCompat.requestPermissions(mCordova.getActivity(), new String[] { Manifest.permission.ACCESS_COARSE_LOCATION }, 1);
			}
		}
		else {
			Log.v(TAG, "Initialize: permissionCheck GRANTED " + permissionCheck);
		}
		
		mAdapter = BluetoothAdapter.getDefaultAdapter();
		if (!mAdapter.isEnabled()) {
			Log.v(TAG, "Initialize: not enabled bluetooth");
			Intent intent = new Intent(BluetoothAdapter.ACTION_REQUEST_ENABLE);
			mCordova.getActivity().startActivityForResult(intent, REQUEST_ENABLE_BT);
		}
		if (mBluetooth == null) {
			mBluetooth = new BluetoothClient(mAdapter, mCordova.getActivity().getApplicationContext(), mCordova.getActivity());
		}
		if (mBluetooth == null) {
			return false;
		}

		Log.v(TAG, "Executing: initialize mBluetooth = " + mBluetooth);
		mBluetooth.mDelegateConnectionPending = this;
		mBluetooth.mDelegateConnected = this;
		mBluetooth.mDelegateRead = this;
		mBluetooth.mDelegateWrite = this;

		return true;
	}
	public boolean connect(String address, String name) {
		Log.v(TAG, "Executing: connect");
		if (!initialize())
			return false;
		
		mBluetooth.connect(address);

		// Sincronizando
		Integer i = 0;
		try {
			while ((mBluetooth.getWaitingForConnResp()) && i < 500) { // 50s
				Thread.sleep(100);
				i++;
			}
		} catch (Exception ex) {
			disconnect();
			return false;
		}

		if (i >= 300) {
			disconnect();
			return false;
		}

		return true;
	}
	public boolean disconnect() {
		mBluetooth.disconnect();

		return true;
	}
    public boolean scanDevices() {
		Log.v(TAG, "ScanDevices: Start");
		initialize();
			
		final BluetoothManager bluetoothManager = (BluetoothManager) mCordova.getActivity().getSystemService(Context.BLUETOOTH_SERVICE);
		mAdapter = bluetoothManager.getAdapter();

		mHandler = new Handler();
		mHandler.postDelayed(new Runnable() {
			@Override
			public void run() {
				Log.v(TAG, "ScanDevices: PostDelayed");
				mScanning = false;
				mAdapter.stopLeScan(mLeScanCallback);
			}
		}, SCAN_PERIOD);

		mScanning = true;
		boolean startLeScanResult = mAdapter.startLeScan(mLeScanCallback);

		Log.v(TAG, "ScanDevices: End");
		return true;
	}
    public boolean stopScan() {
		Log.v(TAG, "Executing: stopScan");
		
		if (mScanning && (mAdapter != null))
			mAdapter.stopLeScan(mLeScanCallback);

		Log.v(TAG, "Executed: stopScan");
		return true;
	}
	public boolean getAllCards(String address, String name) throws InterruptedException {
		Log.v(TAG, "Executing: getAllCards");

		if (!connect(address, name)) {
			return false;
		}

		//mGetAllCardsCallback = callbackContext;
		Thread.sleep(10000);
		
		byte[] data = { 0x00 };
		byte[] dataBT = BluetoothTLV.getTlvCommand(BluetoothTLV.WEARABLE_LS_LTSM, BluetoothTLV.GET_VC_LIST, data);

		mBluetooth.sendData(dataBT);

		Log.v(TAG, "Executed: getAllCards " + Parsers.arrayToHex(dataBT));
		return true;
	}
	public void activate() {

		byte[] data = { (byte) 0x01 };
		byte[] dataBT = BluetoothTLV.getTlvCommand(BluetoothTLV.WEARABLE_LS_LTSM, BluetoothTLV.ACTIVATE_VC, data);
	}
	public void deactivate() {
		initialize();

		byte[] data = { (byte) 0x00 };
		byte[] dataBT = BluetoothTLV.getTlvCommand(BluetoothTLV.WEARABLE_LS_LTSM, BluetoothTLV.ACTIVATE_VC, data);
	}
	private void onConnectDetection(String id, String name, boolean connected) {
		Log.v(TAG, "Executing: onConnectDetection");
		JSONObject json = new JSONObject();
		try {
			json.put("id", id);
			json.put("name", name);
			json.put("connected", connected);
            json.put("deviceType", 1);
		} catch(JSONException ex) {
			return;
		}

		sendToIonic("connect-detection", json);
	}
	public void sendToIonic(String eventName) {
		Log.v(TAG, "Executing: sendToIonic");

		String command = MessageFormat.format(
			"var e = document.createEvent(''Events'');\n" +
			"e.initEvent(''{0}'');\n" +
			"document.dispatchEvent(e);",
			eventName);
		mWebView.sendJavascript(command);		
	}
	public void sendToIonic(String eventName, JSONObject json) {
		Log.v(TAG, "Executing: sendToIonic");

		String command = MessageFormat.format(
			"var e = document.createEvent(''Events'');\n" +
			"e.initEvent(''{0}'');\n" +
			"e.item = {1};\n" +
			"document.dispatchEvent(e);",
			eventName,
			json);
		mWebView.sendJavascript(command);		
	}
	public void sendToIonic(String eventName, JSONArray json) {
		Log.v(TAG, "Executing: sendToIonic");

		String command = MessageFormat.format(
			"var e = document.createEvent(''Events'');\n" +
			"e.initEvent(''{0}'');\n" +
			"e.item = {1};\n" +
			"document.dispatchEvent(e);",
			eventName,
			json);
		mWebView.sendJavascript(command);		
	}
	// private void onCardDetection(String id, String app) {
	// 	Log.v(TAG, "Executing: onCardDetection");
	// 	JSONObject json = new JSONObject();
	// 	try {
	// 		json.put("id", id);
	// 		json.put("app", app);
	// 	} catch(JSONException ex) {
	// 		return;
	// 	}

	// 	String command = MessageFormat.format(
	// 		"var e = document.createEvent(''Events'');\n" +
	// 		"e.initEvent(''card-detection'');\n" +
	// 		"e.item = {0};\n" +
	// 		"document.dispatchEvent(e);",
	// 		json);
	// 	mWebView.sendJavascript(command);		
	// }
	public void onResume() {
		Log.v(TAG, "Executing: onResume");
	}
	@Override
	public void onConnect(final boolean connected) {
		Log.v(TAG, "Executing: onConnect " + connected);
	}
	@Override
	public void onConnectionPending() {
		Log.v(TAG, "Executing: onConnectionPending");
	}
	@Override
	public void onRead(final byte[] status) {
		Log.v(TAG, "Executing: onRead " + Parsers.arrayToHex(status));

		if(status == null) {
			Toast.makeText(mCordova.getActivity().getApplicationContext(), "Error reading data in the Bluetooth connection", Toast.LENGTH_LONG).show();
		} else {
			mCordova.getActivity()
			.runOnUiThread(new Runnable() {
			    public void run() {		
			    	int bytesToCopy = 0;
 	
			    	// This is the first message that I receive for this command
					if(mBufferOffset == 0) {
						// This is the total length of the command that I will receive
						mExpectedSize = (status[1] & 0xff) + ((status[2] & 0xff) * 0x100);	
								
						// I prepare the buffer size
						mBufferDataCmd = new byte[mExpectedSize];
						
						if(mExpectedSize > QppApi.qppServerBufferSize - 3)
							bytesToCopy = QppApi.qppServerBufferSize - 3;
						else
							bytesToCopy = mExpectedSize;
						
						// Copy the data in the first position
						System.arraycopy(status, 3, mBufferDataCmd, 0, bytesToCopy);
						mBufferOffset = bytesToCopy;
					} else {
						if(mExpectedSize - mBufferOffset > QppApi.qppServerBufferSize)
							bytesToCopy = QppApi.qppServerBufferSize;
						else
							bytesToCopy = mExpectedSize - mBufferOffset;
						
						System.arraycopy(status, 0, mBufferDataCmd, mBufferOffset, bytesToCopy);
						mBufferOffset = mBufferOffset + bytesToCopy;
					}
					
					Log.d(TAG, "BLE Message received " + mBufferOffset + " / " + mExpectedSize);
					
					// If we have received the whole command we can proceed
					if(mBufferOffset == mExpectedSize) {
						// try {
						// 	if (mGetAllCardsCallback != null) {
						// 		JSONArray result = BluetoothTLV.sparseTlvCommand(mBufferDataCmd, 0);
						// 		Log.d(TAG, "mBufferOffset " + result);

						// 		disconnect();
						// 		mGetAllCardsCallback.success(result);
						// 	}
						// } catch (Exception ex) {}

						Log.d(TAG, "Received Data: " + Parsers.arrayToHex(mBufferDataCmd));
				    		
						// Operation completed on the Connected Device
						//MyPreferences.setCardOperationOngoing(getApplicationContext(), false);
						
						// Response received, we can cancel the timer
						// if(counterOp != null) {
						// 	counterOp.cancel();
						// }
						
						if (mDelegateOperation != null) {
							mDelegateOperation.processOperationResult(mBufferDataCmd);
						} else {
							Toast.makeText(mCordova.getActivity().getApplicationContext(), "DELEGATE NULL", Toast.LENGTH_LONG).show();
						}

						mBufferDataCmd = new byte[0];
						mBufferOffset = 0;
						mExpectedSize = 0;
					} else {
						// Send ACK?

					}
			    }
			});
		}
	}
	@Override
	public void onWrite() {
		Log.v(TAG, "Executing: onWrite");
	}
	/////////////////////////////////////////
	// OnTransmitApduListener
	/////////////////////////////////////////
	@Override
	public void sendApduToSE(byte[] dataBT, int timeout) {
		// Send the data via Bluetooth
        writeBluetooth(dataBT, timeout);
	}
	@Override
    public void setOnOperationListener(OnOperationListener delegateOperation) {
		mDelegateOperation = delegateOperation;
	}
	private OnOperationListener mDelegateOperation;
	public void writeBluetooth(byte[] data, int timerOp) {
		if(mBluetooth.isConnected()) {			
			// Set the timeout associated to this operation
			TIMER_BLE_OP = timerOp;
			
			Log.d(TAG, "Write Bluetooth: " + Parsers.arrayToHex(data));
			mBluetooth.sendData(data);
		} else if (mDelegateOperation != null) {
			mDelegateOperation.processOperationResult(null);
		} else {
			Toast.makeText(mCordova.getActivity().getApplicationContext(), "DELEGATE NULL", Toast.LENGTH_LONG).show();
		}
	}
}