package com.payin.palago;

import android.os.AsyncTask;
import android.util.Log;
import android.widget.Toast;

import com.nxp.tasks.ActivateVCTask;
import com.nxp.tasks.ListVCTask;
import com.nxp.tasks.CreateCardVCTask;
import com.nxp.tasks.DeleteCardVCTask;

import java.text.DecimalFormat;
import java.util.Random;

import org.apache.cordova.CallbackContext;
import org.apache.cordova.CordovaInterface;
import org.apache.cordova.CordovaPlugin;
import org.apache.cordova.CordovaWebView;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
 
public class CoolPlugin extends CordovaPlugin {
    public static final String TAG = "Palago Smartband";
    public static Bluetooth mBluetooth;

    public void initialize(CordovaInterface cordova, CordovaWebView webView) {
        super.initialize(cordova, webView);
        Log.v(TAG,"Init CoolPlugin");
    }
    public boolean execute(final String action, JSONArray args, CallbackContext callbackContext) throws JSONException {
        try {
            final int duration = Toast.LENGTH_SHORT;
            // Shows a toast
            Log.v(TAG, "Action received:" + action);
            Log.v(TAG, "Args received:" + args);
            
            if (action.equals("connect")) {
                if (mBluetooth == null)
                    mBluetooth = new Bluetooth(cordova, webView);
                mBluetooth.connect(args.getString(0), args.getString(1));
                callbackContext.success("Connected");
                return true;
            }
            else if (action.equals("scanDevices")) {
                if (mBluetooth == null)
                    mBluetooth = new Bluetooth(cordova, webView);
                mBluetooth.scanDevices();
                callbackContext.success("Executing scanDevices");
                return true;
            }
            else if (action.equals("stopScan")) {
                if (mBluetooth == null)
                    mBluetooth = new Bluetooth(cordova, webView);
                mBluetooth.stopScan();
                callbackContext.success("Executing stopScan");
                return true;
            }
            else if (action.equals("getAllCards")) {
                if (mBluetooth == null)
                    mBluetooth = new Bluetooth(cordova, webView);

                AsyncTask<?, ?, ?> task = new ListVCTask(cordova, mBluetooth)
                    .execute();
                
                callbackContext.success("getAllCards");
                return true;
            }
            else if (action.equals("activate")) {
                if (mBluetooth == null)
                    mBluetooth = new Bluetooth(cordova, webView);

                AsyncTask<?, ?, ?> task = new ActivateVCTask(cordova, mBluetooth, args.getInt(0))
                    .execute();
                mBluetooth.activate();
                callbackContext.success("activate");
                return true;
            }
            else if (action.equals("deactivate")) {
                if (mBluetooth == null)
                    mBluetooth = new Bluetooth(cordova, webView);
                mBluetooth.deactivate();
                callbackContext.success("deactivate");
                return true;
            }
            else if (action.equals("createCard")) {
                if (mBluetooth == null)
                    mBluetooth = new Bluetooth(cordova, webView);

	            String randId = new DecimalFormat("00").format(new Random().nextInt(99));

                new CreateCardVCTask(cordova, mBluetooth, callbackContext, randId)
                    .execute();
                return true;
            }
            else if (action.equals("deleteCard")) {
                if (mBluetooth == null)
                    mBluetooth = new Bluetooth(cordova, webView);

                 AsyncTask<?, ?, ?> task = new DeleteCardVCTask(cordova, mBluetooth, callbackContext, args.getInt(0))
                    .execute();
                return true;
            }

            cordova.getActivity().runOnUiThread(new Runnable() {
                public void run() {
                    Toast toast = Toast.makeText(cordova.getActivity().getApplicationContext(), action + " executed", duration);
                    toast.show();
                }
            });

            return true;
        } catch (Exception e) {
            e.printStackTrace();
            callbackContext.error(e.getMessage());
            return false;
        }
    }
}