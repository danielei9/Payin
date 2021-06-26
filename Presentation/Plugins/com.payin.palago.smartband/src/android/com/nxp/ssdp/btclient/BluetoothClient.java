package com.nxp.ssdp.btclient;

import android.app.Activity;
import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;
import android.bluetooth.BluetoothGatt;
import android.bluetooth.BluetoothGattCallback;
import android.bluetooth.BluetoothGattCharacteristic;
import android.bluetooth.BluetoothGattDescriptor;
import android.bluetooth.BluetoothProfile;
import android.content.Context;
import android.os.CountDownTimer;
import android.os.Handler;
import android.os.Looper;
import android.util.Log;
import android.widget.Toast;

import com.quintic.libqpp.QppApi;
import com.quintic.libqpp.iQppCallback;

import com.nxp.utils.Parsers;

public class BluetoothClient {
	protected static final String TAG = "-----------> Bluetooth LE Client";

	// Timeout between following BLE Messages
	public static final int TIMER_BLE = 50;

	/// qpp start
	protected static String uuidQppService = "0000fee9-0000-1000-8000-00805f9b34fb";
	protected static String uuidQppCharWrite = "d44bc439-abfd-45a2-b575-925416129600";
	
	private BluetoothAdapter mBluetoothAdapter = null;
	public BluetoothGatt mBluetoothGatt = null;

	public OnBluetoothWriteListener mDelegateWrite;
	public OnBluetoothReadListener mDelegateRead;
	public OnBluetoothConnectListener mDelegateConnected;
	public OnBluetoothConnectionPendingListener mDelegateConnectionPending;
	
	/** scan all Service ? */
	public boolean isInitialize = false;
	private boolean qppSendDataState = false;
	
	private boolean waitingForConnResp = false;
	
	private Context mContext;
	private Activity mAct;
	
	private SendThread sendDataThread = null;;
	
	// Countdown used to wait for the second connection attempt
	private static CountDownTimer counterConn;
	
	
	private boolean mNextCharacteristicWrite = false;
	
	public BluetoothClient(BluetoothAdapter adapter, Context ctx, Activity act) {
		this.mBluetoothAdapter = adapter;
		this.mContext = ctx;
		this.mAct = act;
		
		// Set the callback to receive data
		receiveDataCallback();
	}
	
	public boolean isConnected() {
		return isInitialize;
	}
	
	public void receiveDataCallback() {
		Log.v(TAG, "Executing: receiveDataCallback ");
		QppApi.setCallback(new iQppCallback() {				
			@Override
			public void onQppReceiveData(BluetoothGatt mBluetoothGatt, String qppUUIDForNotifyChar, byte[] qppData) {	
				Log.v(TAG, "Executing: receiveDataCallback " + Parsers.arrayToHex(qppData));		
				mDelegateRead.onRead(qppData);
			}
		}); 
	}
	public void sendData(byte[] data) {
		Log.v(TAG, "Executing: sendData " + isInitialize + " " + Parsers.arrayToHex(data));

		Integer i = 0;
		try {
			while ((!isInitialize) && i < 300) { // 30s
				Thread.sleep(100);
				i++;
			}
		} catch (Exception ex) {}

		Log.v(TAG, "Executing: sendData (" + i + ") " + isInitialize + " " + Parsers.arrayToHex(data));

		if(isInitialize) {
			Log.v(TAG, "Executing: sendData success");
			sendDataThread = new SendThread(data, true);
			sendDataThread.start();
		} else {
			Log.v(TAG, "Executing: sendData error");
			mDelegateRead.onRead(null);
		}
	}
	private class SendThread extends Thread {
		byte[] data;
		
		public SendThread(byte[] data, boolean send) {
			this.data = data;
		}
						
		public void run() {
			if(data == null){
				return;
			}
			
			int length = data.length;
			int count = 0;
			int offset = 0;
			
			mNextCharacteristicWrite = false;
			
			Log.d(TAG, "--- TRANSMISSION START");
			
			while (offset < length) {
				if ((length - offset) < QppApi.qppServerBufferSize)
					count = length - offset;
				else
					count = QppApi.qppServerBufferSize;
				
				mNextCharacteristicWrite = false;
				
				Log.w(TAG, "QPP Send Data");	
				
				byte tempArray[] = new byte[count];
				System.arraycopy(data, offset, tempArray, 0, count);
				QppApi.qppSendData(mBluetoothGatt, tempArray);
				offset = offset + count;
				
				/*******
				 * Check this
				 * 
				 * 
				 * 
				 * 
				 */
				
				// TODO We need to add a timer or something to get out of here and inform the user if there was an error in the BLE transmission
				// TODO When this is ready and well-tested we should share it with Indian team
				while(mNextCharacteristicWrite == false) {
//					Log.w(TAG, "*/*/*/*/*mNextCharacteristicWrite == false/*/*/*/*/");	
				}
			}
			
//			Release 1.18.0 and new Android App 
//			4.792  bytes
//			07-01 08:59:38.517 D/GH1     ( 9400): --- TEST TRANSMISSION START
//			07-01 08:59:39.003 D/GH1     ( 9400): --- TEST TRANSMISSION END
//			
//			Release 02.02.00 and old Android App 
//			3604 bytes
//			07-01 09:02:58.580 D/GH1     ( 9687): --- TEST TRANSMISSION START
//			07-01 09:03:13.705 D/GH1     ( 9687): --- TEST TRANSMISSION END
			
			Log.d(TAG, "--- TRANSMISSION END");
			
			// We have transmitted the last BLE Message
			mDelegateWrite.onWrite();
		}
	}

	private final BluetoothGattCallback mGattCallback = new BluetoothGattCallback() {
		@Override
		public void onConnectionStateChange(BluetoothGatt gatt, int status,	int newState) {
			Log.w(TAG, "onConnectionStateChange. Status: " + status + " State: " + newState);	
			
			if(status == BluetoothGatt.GATT_SUCCESS) {
				if (newState == BluetoothProfile.STATE_CONNECTED) {
					Log.w(TAG, "BluetoothProfile.STATE_CONNECTED");
					if(isInitialize == true) {
						Log.d(TAG, "Connection received while I was already connected");
						gatt.close();
						return;
					}
					
					// Set the isInitialize here to avoid refreshing info layout before the Service Discovery
					isInitialize = true;
					Log.v(TAG, "Executing: isInitialize " + isInitialize);
					
					// Cancel the counter for the second connection attempt
					if(counterConn != null)
						counterConn.cancel();
					
					// Look for QPP Service
					mBluetoothGatt.discoverServices();
									
					mDelegateConnected.onConnect(true);
				} else if (newState == BluetoothProfile.STATE_DISCONNECTED) {
					Log.w(TAG, "BluetoothProfile.STATE_DISCONNECTED");
					isInitialize = false;
					Log.v(TAG, "Executing: isInitialize " + isInitialize);
	
					if (qppSendDataState) {
						qppSendDataState = false;
					}
					
					mDelegateConnected.onConnect(false);
					
					if (mBluetoothGatt != null) {
						mBluetoothGatt.close();
						mBluetoothGatt = null;
					}
				}
				
				// Let the user launch a new connection request
				waitingForConnResp = false;
			} else {
				/*******
				 * Check this
				 * 
				 * 
				 * 
				 * 
				 */
				
				// TODO This patch might not be longer needed with the BLE FW 0.3
				
				
				
				/*
				 * Error 133 (GATT_ERROR) does not appear in the API
				 * 
				 * This code solves the issue that occurs when the Conn Device receives EACI_MSG_EVT_CONN after a while
				 * and QPPS_CFG_INDNTF_IND is not received and therefore the connection is not well established.
				 * We can force the connections sending back this request
				 * 
				 * If the connection is not established in 4 secs we can consider the connection as not established
				 * 
				 */
				if(status == 133) {
					mBluetoothGatt.connect();
							
					Handler handler = new Handler(Looper.getMainLooper());
				    handler.post(new Runnable() {
				        @Override
				        public void run() {
				        	counterConn = new CountDownTimer(4000, 1000) {
								@Override
								public void onFinish() {
									// There was an error, so we are not connected
									mDelegateConnected.onConnect(false);
									
									if (mBluetoothGatt != null) {
										mBluetoothGatt.close();
										mBluetoothGatt = null;
									}
									
									// Let the user launch a new connection request
									waitingForConnResp = false;
								}
					
								@Override
								public void onTick(long millisUntilFinished) {
									
								}								
							};
							
							counterConn.start();
				        }
				    });
				} else {
					// There was an error, so we are not connected
					mDelegateConnected.onConnect(false);
					
					if (mBluetoothGatt != null) {
						mBluetoothGatt.close();
						mBluetoothGatt = null;
					}
					
					// Let the user launch a new connection request
					waitingForConnResp = false;
				}
			}
		}

		@Override
		public void onServicesDiscovered(BluetoothGatt gatt, int status) {
			Log.w(TAG, "onServicesDiscovered status: " + status);	
			if (QppApi.qppEnable(mBluetoothGatt, uuidQppService, uuidQppCharWrite)) {
				isInitialize = true;
			} else {
				isInitialize = false;
				
				// If this is not a QPP Profile we better close the connection
				close();
			}
		}

		@Override
		public void onCharacteristicChanged(BluetoothGatt gatt,	BluetoothGattCharacteristic characteristic) {
			Log.w(TAG, "onCharacteristicChanged");			
			QppApi.updateValueForNotification(gatt, characteristic);
		}

		@Override
		public void onDescriptorWrite(BluetoothGatt gatt, BluetoothGattDescriptor descriptor, int status) {
			// super.onDescriptorWrite(gatt, descriptor, status);
			Log.w(TAG, "onDescriptorWrite Status: " + status);
			QppApi.setQppNextNotify(gatt, true);
		}

		@Override
		public void onCharacteristicWrite(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic, int status) {
			Log.w(TAG, "onCharacteristicWrite Status: " + status);	
			// super.onCharacteristicWrite(gatt, characteristic, status);
			
			/*******
			 * Check this
			 * 
			 * 
			 * 
			 * 
			 */
			// TODO See what to do if status returns error
			
			if (status == BluetoothGatt.GATT_SUCCESS) {
				mNextCharacteristicWrite = true;
			} else {
				Log.e(TAG, "Send failed!!!!");
				close();
				Toast.makeText(mContext, "Error in BLE transmission, disconnecting device...", Toast.LENGTH_LONG).show();
			}
		}
		
		@Override
		public void onCharacteristicRead(BluetoothGatt gatt,
				BluetoothGattCharacteristic characteristic, int status) {
			Log.w(TAG, "onCharacteristicRead Status: " + status);	
//			super.onCharacteristicRead(gatt, characteristic, status);
		}
		
		@Override
		public void onDescriptorRead(BluetoothGatt gatt,
				BluetoothGattDescriptor descriptor, int status) {
			// TODO Auto-generated method stub
//			super.onDescriptorRead(gatt, descriptor, status);
			Log.w(TAG, "onCharacteristicRead Status: " + status);	
		}
	};

	public boolean connect(final String address) {
		if (mBluetoothAdapter == null || address == null) {
			Log.w("Qn Dbg", "BluetoothAdapter not initialized or unspecified address.");
			return false;
		}
		
		final BluetoothDevice device = mBluetoothAdapter.getRemoteDevice(address);
		if (device == null) {
			Log.w(TAG, "Device not found. Unable to connect.");
			return false;
		}
		
		// setting the autoConnect parameter to false.
		if (waitingForConnResp == false) {
			mBluetoothGatt = device.connectGatt(mContext, false, mGattCallback);
			
			Log.d(TAG, "Trying to create a new connection. Gatt: " + mBluetoothGatt);
			
			// Wait for the response before letting the user launch a new request
			waitingForConnResp = true;
		} else {
			Log.d(TAG, "Pending connection detected");
			
			mDelegateConnectionPending.onConnectionPending();
		}

		return true;
	}

	public void disconnect() {
		if (mBluetoothAdapter == null) {
			Log.w("Qn Dbg", "BluetoothAdapter not initialized");
			return;
		}
		
		if (mBluetoothGatt == null) {
			Log.w("Qn Dbg", "BluetoothGatt not initialized");
			return;
		}
		
		Log.d(TAG, "Disconnect");
		mBluetoothGatt.disconnect();
		
		// We are no longer connected
		isInitialize = false;
	}

	public void close() {
		Log.d(TAG, "Close Connection");
		
		// Disconnect and then close the connection
		disconnect();
		
//		if (mBluetoothGatt != null) {
//			mBluetoothGatt.close();
//			mBluetoothGatt = null;
//		}
		
		// We are no longer connected
		isInitialize = false;
	}
	
	public void closeBluetoothClient() {
		if(mBluetoothGatt != null)
			mBluetoothGatt.close();
		
		mBluetoothGatt = null;
	}
	
	public void setWaitingForConnResp(boolean wait) {
		waitingForConnResp = wait;
	}
	public boolean getWaitingForConnResp() {
		return waitingForConnResp;
	}

	// Interface callback for Bluetooth data read
	public interface OnBluetoothReadListener {
		void onRead(byte[] status);
	}
	
	// Interface callback for Bluetooth data written
	public interface OnBluetoothWriteListener {
		void onWrite();
	}

	// Interface callback for Bluetooth connect
	public interface OnBluetoothConnectListener {
		void onConnect(boolean connected);
	}
	
	// Interface callback for Bluetooth connection pending
	public interface OnBluetoothConnectionPendingListener {
		void onConnectionPending();
	}
}