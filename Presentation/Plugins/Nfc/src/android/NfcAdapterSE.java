package com.ionic.selibrary;

import org.apache.cordova.CordovaWebView;
import org.apache.cordova.CallbackContext;
import org.apache.cordova.CordovaPlugin;
import org.apache.cordova.CordovaInterface;
import android.util.Log;
import android.provider.Settings;
import android.widget.Toast;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;
import java.util.regex.Pattern;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.DialogInterface;
import android.nfc.NfcAdapter;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import android.app.PendingIntent;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.IntentFilter.MalformedMimeTypeException;
import android.nfc.*;
import android.nfc.tech.Ndef;
import android.nfc.tech.NdefFormatable;
import android.os.Parcelable;

import java.io.*;
import android.nfc.tech.MifareClassic;
 
public class NfcAdapterSE extends CordovaPlugin {
 
public static final String TAG = "NfcAdapterSE";

	public static final String ECHO_APPLET_AID = "010203040501";
	
	private EditText editSend;
	private EditText editResponse;
	
	private TextView sendAPDU;
	private TextView responseAPDU;
	
	private Method openMethod;
	private Method transceiveMethod;
	private Method closeMethod;
	private Object ee;
	
	private boolean appletSelected = false;
/**
* Constructor.
*/
	public NfcAdapterSE() {}
 
/**
* Sets the context of the Command. This can then be used to do things like
* get file paths associated with the Activity.
*
* @param cordova The context of the main Activity.
* @param webView The CordovaWebView Cordova is running in.
*/
 
	public boolean execute(final String action, JSONArray args, final CallbackContext callback) throws JSONException {
         
		// Shows a toast
		if (action.equalsIgnoreCase("action")) {
            	
			cordova.getActivity().runOnUiThread(new Runnable() {
				public void run() {
					Log.v(TAG,"configure"+ action);
					boolean res = configureNfcExtras();
					Log.v(TAG,"openSE"+ action);
					//openSEConnection();
					if(!res)
						callback.success();
				}
			});
		}
	
		return true;
	}
	private boolean configureNfcExtras() {
		try {
			Class nfcExtrasClazz = Class.forName("com.android.nfc_extras.NfcAdapterExtras");
			if(nfcExtrasClazz==null)
			{
				Toast.makeText(cordova.getActivity().getApplicationContext(), "nfcExtrasClazz = null", Toast.LENGTH_LONG).show();
				return false;
			}
			/*Method getMethod = nfcExtrasClazz .getMethod("get", Class.forName("android.nfc.NfcAdapter"));
			if(getMethod==null)
			{
				Toast.makeText(this.cordova.getActivity().getApplicationContext(), "getMethod = null", Toast.LENGTH_LONG).show();
				return false;
			}
			NfcAdapter adapter = NfcAdapter.getDefaultAdapter(this.cordova.getActivity());
			if(adapter==null)
			{
				Toast.makeText(this.cordova.getActivity().getApplicationContext(), "Adapter = null", Toast.LENGTH_LONG).show();
				return false;
			}
			//if(!adapter.isEnabled())
			//	return true;
			return false;
			*/
			return true;
			/*Object nfcExtras = getMethod.invoke(nfcExtrasClazz, adapter);
	
			Method getEEMethod = nfcExtras.getClass().getMethod("getEmbeddedExecutionEnvironment", (Class[]) null);
			ee = getEEMethod.invoke(nfcExtras , (Object[]) null);
			
			Class eeClazz = ee.getClass();
			openMethod = eeClazz.getMethod("open", (Class[]) null);
			transceiveMethod = ee.getClass().getMethod("transceive", new Class[] { byte[].class });
			closeMethod = eeClazz.getMethod("close", (Class[]) null);*/	
		} catch (Exception e) {
			StringWriter errors = new StringWriter();
			e.printStackTrace(new PrintWriter(errors));
			Toast.makeText(this.cordova.getActivity().getApplicationContext(), errors.toString(), Toast.LENGTH_LONG).show();
			return false;
		}
	}
	private void openSEConnection() {
		// We open the connection when the app is created and close it when the app is destroyed
		try {
			openMethod.invoke(ee, (Object[]) null);
			//Toast.makeText(this.cordova.getActivity().getApplicationContext(), "conexión abierta", Toast.LENGTH_LONG).show();
		} catch (IllegalAccessException e) {
			e.printStackTrace();
			//Toast.makeText(this.cordova.getActivity().getApplicationContext(), "No idea", Toast.LENGTH_LONG).show();
		} catch (IllegalArgumentException e) {
			e.printStackTrace();
			//Toast.makeText(this.cordova.getActivity().getApplicationContext(), "No se", Toast.LENGTH_LONG).show();
		} catch (InvocationTargetException e) {
			e.printStackTrace();
			//Toast.makeText(this.cordova.getActivity().getApplicationContext(), "No va", Toast.LENGTH_LONG).show();
		}
	}
}