package com.payin.nfc;

//import android.nfc;

import java.io.ObjectInputStream;
import java.lang.Object;
import java.io.FileReader;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.InputStream;
import java.io.IOException;
import java.text.MessageFormat;
import java.util.*;
import java.math.*;
import java.util.Iterator;
import java.util.List;
import java.lang.Thread;
import java.net.MalformedURLException;

// using wildcard imports so we can support Cordova 3.x and Cordova 2.9
import org.apache.cordova.*; // Cordova 3.x
//import org.apache.cordova.api.*;  // Cordova 2.9

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import android.app.Activity;
import android.app.PendingIntent;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.res.AssetManager;
import android.content.IntentFilter.MalformedMimeTypeException;
import android.net.Uri;
import android.nfc.FormatException;
import android.nfc.NdefMessage;
import android.nfc.NdefRecord;
import android.nfc.NfcAdapter;
//import android.nfc.NfcAla;
import android.nfc.NfcEvent;
import android.nfc.Tag;
import android.nfc.TagLostException;
import android.nfc.tech.Ndef;
import android.nfc.tech.NdefFormatable;

import android.os.Parcelable;
import android.os.RemoteException;
import android.os.Environment;
import android.util.Log;

import android.widget.Toast;

import android.nfc.tech.MifareClassic;
import android.nfc.tech.IsoDep;
import android.nfc.tech.NfcA;

import com.nxp.ltsm.mymifareapp.cards.VCEntries;
import com.nxp.ltsm.mymifareapp.cards.VirtualCard;
import com.nxp.ltsm.mymifareapp.encryption.JSBLEncryption;
import com.nxp.ltsm.mymifareapp.preferences.MyPreferences;
import com.nxp.ltsm.mymifareapp.utils.Parsers;
import com.nxp.ltsm.mymifareapp.utils.StatusBytes;
import com.nxp.ltsm.ltsmclient.ILTSMClient;
import com.nxp.ltsm.ltsmclient.ServiceConnector;

/*import java.util.Calendar;
import java.security.*;
import java.security.spec.*;*/
import java.security.KeyPair;
import java.security.KeyStore;
import java.security.KeyPairGenerator;
/*import java.security.KeyFactory;
import java.security.Security;*/
import java.security.interfaces.RSAPrivateKey;
import java.security.interfaces.RSAPublicKey;
import java.security.cert.X509Certificate;
/*import java.security.spec.RSAPrivateKeySpec;
import java.security.spec.RSAPublicKeySpec;
import javax.crypto.Cipher;
import javax.crypto.spec.SecretKeySpec;*/
import java.security.spec.RSAKeyGenParameterSpec;
import android.security.KeyPairGeneratorSpec;
import javax.security.auth.x500.X500Principal;
/*import java.util.Enumeration;
import java.security.cert.Certificate;
*/

public class NfcPlugin extends CordovaPlugin implements NfcAdapter.OnNdefPushCompleteCallback {
    private static final String REGISTER_MIME_TYPE = "registerMimeType";
    private static final String REMOVE_MIME_TYPE = "removeMimeType";
    private static final String REGISTER_NDEF = "registerNdef";
    private static final String REMOVE_NDEF = "removeNdef";
    private static final String REGISTER_NDEF_FORMATABLE = "registerNdefFormatable";
    private static final String REGISTER_DEFAULT_TAG = "registerTag";
    private static final String REMOVE_DEFAULT_TAG = "removeTag";
    private static final String WRITE_TAG = "writeTag";
    private static final String MAKE_READ_ONLY = "makeReadOnly";
    private static final String ERASE_TAG = "eraseTag";
    private static final String SHARE_TAG = "shareTag";
    private static final String UNSHARE_TAG = "unshareTag";
    private static final String HANDOVER = "handover"; // Android Beam
    private static final String STOP_HANDOVER = "stopHandover";
    private static final String ENABLED = "enabled";
    
    private static final String WRITE = "write";
    private static final String GETALLVC = "getAllVC";
    private static final String CREATEVC = "createVC";
    private static final String DELETEVC = "deleteVC";
    private static final String ACTIVATEVC = "activateVC";
    private static final String NFC_SCRIPT_DETECT = "nfc-script-detect";
    private static final String NFC_SCRIPT_SUCCESS = "nfc-script-success";
    private static final String NFC_SCRIPT_ERROR = "nfc-script-error";

    private static final String NDEF = "ndef";
    private static final String NDEF_MIME = "ndef-mime";
    private static final String NDEF_FORMATABLE = "ndef-formatable";
    private static final String READ_NDEF_FORMATABLE = "read-ndef-formatable";
    private static final String TAG_DEFAULT = "tag";

    private static final String STATUS_NFC_OK = "NFC_OK";
    private static final String STATUS_NO_NFC = "NO_NFC";
    private static final String STATUS_NFC_DISABLED = "NFC_DISABLED";
    private static final String STATUS_NDEF_PUSH_DISABLED = "NDEF_PUSH_DISABLED";

    private static final String TAG = "NfcPlugin";
    private static String scriptData = "";
    private static JSONArray resScriptData;
    private static String errorText = "";
    private static int countI =-1;
    private static int countJ =-1;
    private static int length=-1;
    
    private static Tag tag = null;
    private final List<IntentFilter> intentFilters = new ArrayList<IntentFilter>();
    private final ArrayList<String[]> techLists = new ArrayList<String[]>();

    private NdefMessage p2pMessage = null;
    private PendingIntent pendingIntent = null;

    private Intent savedIntent = null;

    private CallbackContext shareTagCallback;
    private CallbackContext handoverCallback;
    private CallbackContext myCallBack;
    //static KeyPair keyPairStore;
    private EigeHsmService hsm;
    
    private NfcHandler _nfcHandler;
    public NfcHandler getNfcHandler(){
        if (_nfcHandler == null)
            _nfcHandler = new NfcHandler(this.webView, cordova.getActivity(), cordova.getThreadPool(), this.webView.getContext().getAssets());
        return _nfcHandler;
    }

    @Override
    public boolean execute(String action, JSONArray data, CallbackContext callbackContext) throws JSONException {
        Log.d(TAG, "execute " + action + " " + data);    
        getNfcHandler().setCallbackContext(callbackContext);

        createPendingIntent();
        myCallBack = callbackContext;

        if ((!getNfcStatus().equals(STATUS_NFC_OK)) && (!action.equalsIgnoreCase("getNfcStatus"))) {
            callbackContext.error(getNfcStatus());
            return true; // short circuit
        }

        if (action.equalsIgnoreCase(REGISTER_MIME_TYPE)) {
            registerMimeType(data, callbackContext);

        } else if (action.equalsIgnoreCase(REMOVE_MIME_TYPE)) {
          removeMimeType(data, callbackContext);

        } else if (action.equalsIgnoreCase(REGISTER_NDEF)) {
          registerNdef(callbackContext);

        } else if (action.equalsIgnoreCase(REMOVE_NDEF)) {
          removeNdef(callbackContext);

        } else if (action.equalsIgnoreCase(REGISTER_NDEF_FORMATABLE)) {
            registerNdefFormatable(callbackContext);
  
        } else if (action.equals(REGISTER_DEFAULT_TAG)) {
          registerDefaultTag(callbackContext);

        } else if (action.equals(REMOVE_DEFAULT_TAG)) {
          removeDefaultTag(callbackContext);

        } else if (action.equalsIgnoreCase(WRITE_TAG)) {
            writeTag(data, callbackContext);

        } else if (action.equalsIgnoreCase(MAKE_READ_ONLY)) {
            makeReadOnly(callbackContext);

        } else if (action.equalsIgnoreCase(ERASE_TAG)) {
            eraseTag(callbackContext);

        } else if (action.equalsIgnoreCase(SHARE_TAG)) {
            shareTag(data, callbackContext);

        } else if (action.equalsIgnoreCase(UNSHARE_TAG)) {
            unshareTag(callbackContext);

        } else if (action.equalsIgnoreCase(HANDOVER)) {
            handover(data, callbackContext);

        } else if (action.equalsIgnoreCase(STOP_HANDOVER)) {
            stopHandover(callbackContext);

        } else if (action.equalsIgnoreCase("init")) {
            init(callbackContext);

        } else if (action.equalsIgnoreCase(ENABLED)) {
            // status is checked before every call
            // if code made it here, NFC is enabled
            callbackContext.success(STATUS_NFC_OK);
        } else if (action.equalsIgnoreCase("execute")) {
            getNfcHandler()
            .execute(
                data.getJSONObject(0),
                data.getString(1),
                data.getJSONArray(2),
                data.getJSONObject(3),
                data.getInt(4),
                callbackContext
            );
        } else if (action.equalsIgnoreCase("getNfcStatus")) {
            String result = getNfcStatus();
            callbackContext.success(result);
        } else if (action.equalsIgnoreCase(WRITE)) {
            scriptData=data.toString();

        } else if (action.equalsIgnoreCase(GETALLVC)) {
            xpGetAllVC(data, callbackContext);

        } else if (action.equalsIgnoreCase(CREATEVC)) {
            xpCreateVC(data, callbackContext);

        } else if (action.equalsIgnoreCase(DELETEVC)) {
            xpDeleteVC(data, callbackContext);

        } else if (action.equalsIgnoreCase(ACTIVATEVC)) {
            xpActivateVC(data, callbackContext);

        } else {
            // invalid action
            return false;
        }

        return true;
    }

    private String getNfcStatus() {
        NfcAdapter nfcAdapter = NfcAdapter.getDefaultAdapter(getActivity());

        if (nfcAdapter == null) {
            Log.d(TAG, "getNfcStatus() NONFC");
            return STATUS_NO_NFC;
        } else if (!nfcAdapter.isEnabled()) {
            Log.d(TAG, "getNfcStatus() DISABLED");
            return STATUS_NFC_DISABLED;
        } else {
            Log.d(TAG, "getNfcStatus() OK");
            return STATUS_NFC_OK;
        }
    }

    private void registerDefaultTag(CallbackContext callbackContext) {
      addTagFilter();
      callbackContext.success();
  }

    private void removeDefaultTag(CallbackContext callbackContext) {
      removeTagFilter();
      callbackContext.success();
  }

    private void registerNdefFormatable(CallbackContext callbackContext) {
        addTechList(new String[]{NdefFormatable.class.getName()});
        callbackContext.success();
    }
    
    private void registerNdef(CallbackContext callbackContext) {
      addTechList(new String[]{Ndef.class.getName()});
      callbackContext.success();
  }

    private void removeNdef(CallbackContext callbackContext) {
      removeTechList(new String[]{Ndef.class.getName()});
      callbackContext.success();
  }

    private void unshareTag(CallbackContext callbackContext) {
        p2pMessage = null;
        stopNdefPush();
        shareTagCallback = null;
        callbackContext.success();
    }

    private void init(CallbackContext callbackContext) {
        Log.d(TAG, "init() " + getIntent());

        // getNfcHandler().startNfc();
        // if (!getNfcHandler().recycledIntent()) {
        //     getNfcHandler().parseMessage();
        // }
        callbackContext.success();
    }

    private void removeMimeType(JSONArray data, CallbackContext callbackContext) throws JSONException {
        String mimeType = "";
        try {
            mimeType = data.getString(0);
            /*boolean removed =*/ removeIntentFilter(mimeType);
            callbackContext.success();
        } catch (MalformedMimeTypeException e) {
            callbackContext.error("Invalid MIME Type " + mimeType);
        }
    }

    private void registerMimeType(JSONArray dataJ, CallbackContext callbackContext) throws JSONException {
        String mimeType = "";
        try {
            mimeType = dataJ.getString(0);
            intentFilters.add(createIntentFilter(mimeType));
            callbackContext.success();
        } catch (MalformedMimeTypeException e) {
            callbackContext.error("Invalid MIME Type " + mimeType);
        }
    }

    // Cheating and writing an empty record. We may actually be able to erase some tag types.
    private void eraseTag(CallbackContext callbackContext) throws JSONException {
        Tag tag = savedIntent.getParcelableExtra(NfcAdapter.EXTRA_TAG);
        NdefRecord[] records = {
            new NdefRecord(NdefRecord.TNF_EMPTY, new byte[0], new byte[0], new byte[0])
        };
        writeNdefMessage(new NdefMessage(records), tag, callbackContext);
    }

    private void writeTag(JSONArray data, CallbackContext callbackContext) throws JSONException {
        if (getIntent() == null) {  // TODO remove this and handle LostTag
            callbackContext.error("Failed to write tag, received null intent");
        }

        Tag tag = savedIntent.getParcelableExtra(NfcAdapter.EXTRA_TAG);
        NdefRecord[] records = Util.jsonToNdefRecords(data.getString(0));
        writeNdefMessage(new NdefMessage(records), tag, callbackContext);
    }

    private void writeNdefMessage(final NdefMessage message, final Tag tag, final CallbackContext callbackContext) {
        cordova.getThreadPool().execute(new Runnable() {
            @Override
            public void run() {
                try {
                    Ndef ndef = Ndef.get(tag);
                    if (ndef != null) {
                        ndef.connect();

                        if (ndef.isWritable()) {
                            int size = message.toByteArray().length;
                            if (ndef.getMaxSize() < size) {
                                callbackContext.error("Tag capacity is " + ndef.getMaxSize() +
                                        " bytes, message is " + size + " bytes.");
                            } else {
                                ndef.writeNdefMessage(message);
                                callbackContext.success();
                            }
                        } else {
                            callbackContext.error("Tag is read only");
                        }
                        ndef.close();
                    } else {
                        NdefFormatable formatable = NdefFormatable.get(tag);
                        if (formatable != null) {
                            formatable.connect();
                            formatable.format(message);
                            callbackContext.success();
                            formatable.close();
                        } else {
                            callbackContext.error("Tag doesn't support NDEF");
                        }
                    }
                } catch (FormatException e) {
                    callbackContext.error(e.getMessage());
                } catch (TagLostException e) {
                    callbackContext.error(e.getMessage());
                } catch (IOException e) {
                    callbackContext.error(e.getMessage());
                }
            }
        });
    }

    private void makeReadOnly(final CallbackContext callbackContext) throws JSONException {

        if (getIntent() == null) { // Lost Tag
            callbackContext.error("Failed to make tag read only, received null intent");
            return;
        }

        final Tag tag = savedIntent.getParcelableExtra(NfcAdapter.EXTRA_TAG);
        if (tag == null) {
            callbackContext.error("Failed to make tag read only, tag is null");
            return;
        }

        cordova.getThreadPool().execute(new Runnable() {
            @Override
            public void run() {
                boolean success = false;
                String message = "Could not make tag read only";

                Ndef ndef = Ndef.get(tag);

                try {
                    if (ndef != null) {

                        ndef.connect();

                        if (!ndef.isWritable()) {
                            message = "Tag is not writable";
                        } else if (ndef.canMakeReadOnly()) {
                            success = ndef.makeReadOnly();
                        } else {
                            message = "Tag can not be made read only";
                        }

                    } else {
                        message = "Tag is not NDEF";
                    }

                } catch (IOException e) {
                    Log.e(TAG, "Failed to make tag read only", e);
                    if (e.getMessage() != null) {
                        message = e.getMessage();
                    } else {
                        message = e.toString();
                    }
                }

                if (success) {
                    callbackContext.success();
                } else {
                    callbackContext.error(message);
                }
            }
        });
    }

    private void shareTag(JSONArray data, CallbackContext callbackContext) throws JSONException {
        NdefRecord[] records = Util.jsonToNdefRecords(data.getString(0));
        this.p2pMessage = new NdefMessage(records);

        startNdefPush(callbackContext);
    }
    // See http://developer.android.com/reference/android/nfc/NfcAdapter.html#setBeamPushUris(android.net.Uri[],%20android.app.Activity)
    private void handover(JSONArray data, CallbackContext callbackContext) throws JSONException {

        Uri[] uri = new Uri[data.length()];

        for (int i = 0; i < data.length(); i++) {
            uri[i] = Uri.parse(data.getString(i));
        }

        startNdefBeam(callbackContext, uri);
    }

    private void stopHandover(CallbackContext callbackContext) throws JSONException {
        stopNdefBeam();
        handoverCallback = null;
        callbackContext.success();
    }

    private void createPendingIntent() {
        if (pendingIntent == null) {
            Activity activity = cordova.getActivity();
            Intent intent = new Intent(activity, activity.getClass());
            intent.addFlags(Intent.FLAG_ACTIVITY_SINGLE_TOP | Intent.FLAG_ACTIVITY_CLEAR_TOP);
            pendingIntent = PendingIntent.getActivity(activity, 0, intent, 0);
        }
    }

    private void addTechList(String[] list) {
      this.addTechFilter();
      this.addToTechList(list);
    }

    private void removeTechList(String[] list) {
      this.removeTechFilter();
      this.removeFromTechList(list);
    }

    private void addTechFilter() {
      intentFilters.add(new IntentFilter(NfcAdapter.ACTION_TECH_DISCOVERED));
    }

    private boolean removeTechFilter() {
      boolean removed = false;
      Iterator<IntentFilter> iter = intentFilters.iterator();
      while (iter.hasNext()) {
        IntentFilter intentFilter = iter.next();
        if (NfcAdapter.ACTION_TECH_DISCOVERED.equals(intentFilter.getAction(0))) {
          intentFilters.remove(intentFilter);
          removed = true;
        }
      }
      return removed;
    }

    private void addTagFilter() {
      intentFilters.add(new IntentFilter(NfcAdapter.ACTION_TAG_DISCOVERED));
  }

    private boolean removeTagFilter() {
      boolean removed = false;
      Iterator<IntentFilter> iter = intentFilters.iterator();
      while (iter.hasNext()) {
        IntentFilter intentFilter = iter.next();
        if (NfcAdapter.ACTION_TAG_DISCOVERED.equals(intentFilter.getAction(0))) {
          intentFilters.remove(intentFilter);
          removed = true;
        }
      }
      return removed;
  }

    private void startNfc() {
        createPendingIntent(); // onResume can call startNfc before execute

        getActivity().runOnUiThread(new Runnable() {
            public void run() {
                NfcAdapter nfcAdapter = NfcAdapter.getDefaultAdapter(getActivity());

                if (nfcAdapter != null && !getActivity().isFinishing()) {
                    try {
                        nfcAdapter.enableForegroundDispatch(getActivity(), getPendingIntent(), getIntentFilters(), getTechLists());

                        if (p2pMessage != null) {
                            nfcAdapter.setNdefPushMessage(p2pMessage, getActivity());
                        }
                    } catch (IllegalStateException e) {
                        // issue 110 - user exits app with home button while nfc is initializing
                        Log.w(TAG, "Illegal State Exception starting NFC. Assuming application is terminating.");
                    }

                }
            }
        });
    }

    private void stopNfc() {
        Log.d(TAG, "stopNfc");
        getActivity().runOnUiThread(new Runnable() {
            public void run() {

                NfcAdapter nfcAdapter = NfcAdapter.getDefaultAdapter(getActivity());

                if (nfcAdapter != null) {
                    try {
                        nfcAdapter.disableForegroundDispatch(getActivity());
                    } catch (IllegalStateException e) {
                        // issue 125 - user exits app with back button while nfc
                        Log.w(TAG, "Illegal State Exception stopping NFC. Assuming application is terminating.");
                    }
                }
            }
        });
    }

    private void startNdefBeam(final CallbackContext callbackContext, final Uri[] uris) {
        getActivity().runOnUiThread(new Runnable() {
            public void run() {

                NfcAdapter nfcAdapter = NfcAdapter.getDefaultAdapter(getActivity());

                if (nfcAdapter == null) {
                    callbackContext.error(STATUS_NO_NFC);
                } else if (!nfcAdapter.isNdefPushEnabled()) {
                    callbackContext.error(STATUS_NDEF_PUSH_DISABLED);
                } else {
                    nfcAdapter.setOnNdefPushCompleteCallback(NfcPlugin.this, getActivity());
                    try {
                        nfcAdapter.setBeamPushUris(uris, getActivity());

                        PluginResult result = new PluginResult(PluginResult.Status.NO_RESULT);
                        result.setKeepCallback(true);
                        handoverCallback = callbackContext;
                        callbackContext.sendPluginResult(result);

                    } catch (IllegalArgumentException e) {
                        callbackContext.error(e.getMessage());
                    }
                }
            }
        });
    }

    private void startNdefPush(final CallbackContext callbackContext) {
        getActivity().runOnUiThread(new Runnable() {
            public void run() {

                NfcAdapter nfcAdapter = NfcAdapter.getDefaultAdapter(getActivity());

                if (nfcAdapter == null) {
                    callbackContext.error(STATUS_NO_NFC);
                } else if (!nfcAdapter.isNdefPushEnabled()) {
                    callbackContext.error(STATUS_NDEF_PUSH_DISABLED);
                } else {
                    nfcAdapter.setNdefPushMessage(p2pMessage, getActivity());
                    nfcAdapter.setOnNdefPushCompleteCallback(NfcPlugin.this, getActivity());

                    PluginResult result = new PluginResult(PluginResult.Status.NO_RESULT);
                    result.setKeepCallback(true);
                    shareTagCallback = callbackContext;
                    callbackContext.sendPluginResult(result);
                }
            }
        });
    }

    private void stopNdefPush() {
        getActivity().runOnUiThread(new Runnable() {
            public void run() {

                NfcAdapter nfcAdapter = NfcAdapter.getDefaultAdapter(getActivity());

                if (nfcAdapter != null) {
                    nfcAdapter.setNdefPushMessage(null, getActivity());
                }

            }
        });
    }

    private void stopNdefBeam() {
        getActivity().runOnUiThread(new Runnable() {
            public void run() {

                NfcAdapter nfcAdapter = NfcAdapter.getDefaultAdapter(getActivity());

                if (nfcAdapter != null) {
                    nfcAdapter.setBeamPushUris(null, getActivity());
                }

            }
        });
    }

    private void addToTechList(String[] techs) {
      techLists.add(techs);
  }

    private void removeFromTechList(String[] techs) {
      techLists.remove(techs);
  }

    private boolean removeIntentFilter(String mimeType) throws MalformedMimeTypeException {
      boolean removed = false;
      Iterator<IntentFilter> iter = intentFilters.iterator();
      while (iter.hasNext()) {
        IntentFilter intentFilter = iter.next();
        String mt = intentFilter.getDataType(0);
        if (mimeType.equals(mt)) {
          intentFilters.remove(intentFilter);
          removed = true;
        }
      }
      return removed;
    }

    private IntentFilter createIntentFilter(String mimeType) throws MalformedMimeTypeException {
        IntentFilter intentFilter = new IntentFilter(NfcAdapter.ACTION_NDEF_DISCOVERED);
        intentFilter.addDataType(mimeType);
        return intentFilter;
    }

    private PendingIntent getPendingIntent() {
        return pendingIntent;
    }

    private IntentFilter[] getIntentFilters() {
        return intentFilters.toArray(new IntentFilter[intentFilters.size()]);
    }

    private String[][] getTechLists() {
        //noinspection ToArrayCallWithZeroLengthArrayArgument
        return techLists.toArray(new String[0][0]);
    }
    String javaScriptEventTemplateScript =
        "var e = document.createEvent(''Events'');\n" +
        "e.initEvent(''{0}'');\n" +
        "e.script = {1};\n" +
        "document.dispatchEvent(e);";
    
    private void fireNfcScriptDetect (byte[] uid, int type) {
        String command = MessageFormat.format(javaScriptEventTemplate, NFC_SCRIPT_DETECT, Util.tagToJSON2(uid, type, null, 0, 0, 0, null));
        Log.v(TAG, command);
        this.webView.sendJavascript(command);
    }
    private void fireNfcScriptSuccess (byte[] uid, int type, JSONArray script) {
        String command = MessageFormat.format(javaScriptEventTemplate, NFC_SCRIPT_SUCCESS, Util.tagToJSON2(uid, type, script, 0, 0, 0, null));
        Log.v(TAG, command);
        this.webView.sendJavascript(command);
    }
    private void fireNfcScriptError (byte[] uid, int type, String error) {   
        //execScript(tag);
        String command = MessageFormat.format(javaScriptEventTemplate, NFC_SCRIPT_ERROR, Util.tagToJSON2(uid, type, null, 0, 0, 0, error));
        Log.v(TAG, command);
        this.webView.sendJavascript(command);
    }
    
    private void fireNdefFormatableEvent (Tag tag) {
        String command = MessageFormat.format(javaScriptEventTemplate, NDEF_FORMATABLE, Util.tagToJSON(tag));
        Log.v(TAG, command);
        this.webView.sendJavascript(command);
    }
    private void fireNdefFormatableEventRead (Tag tag) {   
        execScript(tag);
        String command = MessageFormat.format(javaScriptEventTemplate, NDEF_FORMATABLE, Util.stringToJSON(tag,resScriptData));
        Log.v(TAG, command);
        this.webView.sendJavascript(command);
    }
    
    private void fireDesfireEvent(Tag tag){
        final byte[] SELECT = {
                (byte) 0x00, // CLA Class
                (byte) 0xA4, // INS Instruction
                (byte) 0x04, // P1  Parameter 1
                (byte) 0x00, // P2  Parameter 2
                (byte) 0x08, // Length
                (byte) 0x31,  (byte)0x54, (byte)0x49, (byte)0x43, (byte)0x2e,
                (byte) 0x49, (byte)0x43, (byte)0x41, // AID 315449432e494341
        };
        IsoDep desf = IsoDep.get(tag);
        if(desf!=null)
        {
            byte[] data;
            try{
                desf.connect();
                byte[] result = desf.transceive(SELECT);
                scriptData = bytesToHex(result);
                desf.close();
                getActivity().runOnUiThread(new Runnable() {
                    public void run() {
                        Toast.makeText(cordova.getActivity().getApplicationContext(),
                            "Desfire Card: "+scriptData, 
                            Toast.LENGTH_SHORT).show();
                    }
                });
            }catch(IOException e) { 
                Log.e(TAG, e.getLocalizedMessage());              
            }
            
        }     
    }
    
    final protected static char[] hexArray = "0123456789ABCDEF".toCharArray();
    public static String bytesToHex(byte[] bytes) {
        char[] hexChars = new char[bytes.length * 2];
        for ( int j = 0; j < bytes.length; j++ ) {
            int v = bytes[j] & 0xFF;
            hexChars[j * 2] = hexArray[v >>> 4];
            hexChars[j * 2 + 1] = hexArray[v & 0x0F];
        }
        return new String(hexChars);
    }
    
    public static byte[] hexStringToByteArray(String s) {
        int len = s.length();
        byte[] data = new byte[len / 2];
        for (int i = 0; i < len; i += 2) {
            data[i / 2] = (byte) ((Character.digit(s.charAt(i), 16) << 4)
                    + Character.digit(s.charAt(i+1), 16));
        }
        return data;
    }
    private void responseScript(JSONObject card, String keys, JSONArray script, CallbackContext callbackContext) {
        getNfcHandler()
            .execute(card, keys, script, null, 0, callbackContext);
    }
    
    static ServiceConnector commService = null;
    private void xpGetAllVC(JSONArray data, CallbackContext callbackContext) {
        boolean success = false;
        
        try {
            byte[] vcListData = null ;
            short status = 1;
            
            Log.e("NfcPlugin","xpGetAllVC antes");
            if (commService == null) {
                commService = new ServiceConnector(cordova.getActivity().getApplicationContext());
                Thread.sleep(1000);
                VCEntries.ReadArrayListFromSD(cordova.getActivity().getApplicationContext(), "VCPayinFile");
            }
            Log.e("NfcPlugin","xpGetAllVC despues ");
            
            vcListData = commService.getInterface().getVcList();
            status = Parsers.getSW(vcListData);
            
            switch (status) {
                case StatusBytes.SW_NO_ERROR:
                    ArrayList<Integer> vcList = Parsers.getVCList(vcListData, cordova.getActivity().getApplicationContext());
                    removeVCsFromMemoryInternal(vcList);
                    addVCsFromMemoryInternal(vcList);
               
                    JSONObject result = new JSONObject();
                    JSONArray list = new JSONArray();
                    result.put("data", list);
                    
                    for (int i = 0 ; i < vcList.size(); i++) {
                        Integer id = vcList.get(i);
                        VirtualCard vc = VCEntries.getVCEntry(id);
                        
                        JSONObject object = new JSONObject();
                        object.put("id", id);
                        object.put("isActivated", xpGetActivatedVCInternal(id));
                        if (vc != null) {
                            object.put("name", vc.getVcName());
                            object.put("type", vc.getVcType());
                            object.put("uid", vc.getVcUID());
                        }
                        list.put(object);
                    }
                    
                    callbackContext.success(result);
                    
                    break;
                case StatusBytes.NO_VC_ENTRY_PRESENT:
                    callbackContext.error("No existe entradas de VC");
                    break;
                default:
                    callbackContext.error("Error ocurred");
                    break;
            }
        } catch (Exception e) {
            e.printStackTrace();
            callbackContext.error(e.getMessage());
        }
    }
    private void removeVCsFromMemoryInternal(ArrayList<Integer> listVC) throws Exception {
        for(int i = 0; i < VCEntries.getEntries().size(); i++) {
            VirtualCard vc = VCEntries.getEntries().get(i);
            
            if(listVC.contains(vc.getVcId()) == false) {
                VCEntries.deleteEntry(vc.getVcId(), cordova.getActivity().getApplicationContext());
                i--;
            }
        }
    }
    private void addVCsFromMemoryInternal(ArrayList<Integer> listVC) {
        for(int i = 0; i < listVC.size(); i++) {
            int vcEntryId = listVC.get(i);

            boolean exists = false;
            for(int j = 0; j < VCEntries.getEntries().size(); j++) {
                VirtualCard vc = VCEntries.getEntries().get(j);
                if(vcEntryId == vc.getVcId()) {
                    exists = true;
                    break;
                }
            }
            
            // if (exists == false) {
            //     VirtualCard vc = new VirtualCard(VirtualCard.VC_MIFARE_CLASSIC, vcEntryId, "", "", R.drawable.generic_card, "");
            //     VCEntries.addEntry(vc, cordova.getActivity().getApplicationContext());
            // }
        }
    }
    private boolean xpGetActivatedVCInternal(int entryId) throws JSONException, RemoteException {
        byte[] vcStatusData = commService.getInterface().getVirtualCardStatus(entryId);
                    
        short status = Parsers.getSW(vcStatusData);
        switch (status) {
            case StatusBytes.SW_NO_ERROR:
                return (Parsers.getVcStatus(vcStatusData) == 1);
            case StatusBytes.SW_ALREADY_ACTIVATED:
                return true;
        }
        return false;
    }
    private void executarScript() {
    }
    private void xpCreateVC(JSONArray data, CallbackContext callbackContext) {
        try {
            JSONObject jsonObject = data.getJSONObject(0);
            String cardName = jsonObject.get("name").toString();
            Integer cardType = jsonObject.getInt("type");
            Integer uidType = jsonObject.getInt("uidType");
            Integer perso_ = jsonObject.getInt("perso");
            JSONArray jsonApps = jsonObject.getJSONArray("apps");
            
            List<String> listApps = new ArrayList<String>();
            for (int i = 0 ; i < jsonApps.length(); i++) {
                listApps.add(jsonApps
                    .getJSONObject(i)
                    .get("aid")
                    .toString()
                );
            }
            
            String command = "";
            String perso = "";
            
            String vcConfValue =
                perso_ == 1 ? "01" : // Preperso with applet
                perso_ == 2 ? "00" : // No preperso 
                "";
            if (vcConfValue == "")
                callbackContext.error("Perso not allowed (" + cardType + ")");
                
            String vcUidValue =
                uidType == 2 ? "00" : // 7 bytes
                uidType == 1 ? "01" : // 4 bytes random
                ""; // 
            if (vcUidValue == "")
                callbackContext.error("Uid type not allowed (" + uidType + ")");
            
            String vcTypeValue =
                cardType == 1 ? "0101" : // MIFARE Classic 1KB
                //cardType ==  ? "0104" : // MIFARE Classic 4KB 
                //cardType ==  ? "0202" : // MIFARE DESFire 2KB 
                cardType == 2 ? "0204" : // MIFARE DESFire 4KB 
                //cardType ==  ? "0208" : // MIFARE DESFire 8KB 
                "";
            if (vcTypeValue == "")
                callbackContext.error("Card type not allowed (" + cardType + ")");
            if (
                (cardType == 2) && // MIFARE DESFire 4KB
                (uidType != 2) // NO 7 bytes
            )
                callbackContext.error("MIFARE Desfire cards only allow 7 bytes UID");
            
            if (cardType == 1)
                executarScript();
            
            int vcUidValues=0x7f04000f;
            boolean mEncryptMifareKeyset = false;
            String vcKeysetValue =
                cardType == 1 ? "FFFFFFFFFFFFFF078069FFFFFFFFFFFF" : // Default MIFARE Classic 1Kb
                cardType == 2 ? "00000000000000000000000000000000" : // MIFARE DESFire 4KB
                "";
            if (vcKeysetValue == "")
                callbackContext.error("Default key set not allowed");
            String vcDesfCryptoValue = "00";
            String JSBL_KEY_FILENAME = "keyfile.txt";
            byte SP_SD_KVN1 = 0x48; // Must not be 0x00
            byte[] SP_SD_ENC1 = Parsers.hexToArray("404142434445464748494A4B4C4D4E4F");
            byte[] SP_SD_MAC1 = Parsers.hexToArray("404142434445464748494A4B4C4D4E4F");
            byte[] SP_SD_DEK1 = Parsers.hexToArray("404142434445464748494A4B4C4D4E4F");
        
            //JSBLEncryption jsblEncryption = new JSBLEncryption(JSBL_KEY_FILENAME, cordova.getActivity().getApplicationContext());
            
            
            byte[] bVcConf = {(byte)
                0x46, 1,
                    0x00
            };
            System.arraycopy(Parsers.hexToArray(vcConfValue), 0, bVcConf, 2, 1);
            command = command.concat(Parsers.arrayToHex(bVcConf));
            
            byte[] bVcType = {(byte)
                0xA5, 7,
                    0x02, 0x02, 0x00, 0x00,
                    0x03, 0x01, 0x00
            };
            System.arraycopy(Parsers.hexToArray(vcTypeValue), 0, bVcType, 4, 2);
            System.arraycopy(Parsers.hexToArray(vcUidValue), 0, bVcType, 8, 1);
            command = command.concat(Parsers.arrayToHex(bVcType));
                        
            // SAK Value
            if(vcTypeValue.equals("0101") == true) // MIFARE Classic 1kB
                command = command.concat("A60705020400060108");
            else if(vcTypeValue.equals("0104") == true) // MIFARE Classic 4kB 
                command = command.concat("A60705020200060118");
            else if(vcTypeValue.equals("0202") == true) // MIFARE DESFire 2kB
                command = command.concat("A60705024403060120");
            else if(vcTypeValue.equals("0204") == true) // MIFARE DESFire 4kB
                command = command.concat("A60705024403060120");
            else if(vcTypeValue.equals("0208") == true) // MIFARE DESFire 8kB
                command = command.concat("A60705024403060120");

            if(vcUidValue.equals("00") == true) {  // 7 bytes
                command = command.concat("A11F80080FFFFFFFFFFFFFFF81011F8202FFFF8301008401008501008603000000");
            } else { // 4 bytes
                command = command.concat("A11C80050FFFFFFFFF81011F8202FFFF8301008401008501008603000000");
            }
            
            if(mEncryptMifareKeyset == true) {
            	// byte[] bVcKeysetEnc = {(byte)
                //     0xA8, 117
                //         0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                //         0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                //         0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                //         0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                //         0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                //         0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                //         0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                //         0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                //         0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                //         0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                //         0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                //         0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
            	// byte[] encryptedKeys = jsblEncryption.getEncryptedTLV(Parsers.hexToArray(vcKeysetValue), (byte) 0x20);
            	// System.arraycopy(encryptedKeys, 0, bVcKeysetEnc, 2, encryptedKeys.length);
            	// command = command.concat(Parsers.arrayToHex(bVcKeysetEnc));
                callbackContext.error("Caso no tenido en cuenta 1");
            } else {
            	String cryptoValue = "";
            	
            	if(vcTypeValue.startsWith("01")) { // MIFARE Classic
            		byte[] bVcKeysetPlain = {(byte)
                        0xA8, 18,
                            0x20, 16,
                                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                0x00, 0x00, 0x00, 0x00, 0x00, 0x00
                    };
            		
            		System.arraycopy(Parsers.hexToArray(vcKeysetValue), 0, bVcKeysetPlain, 4, Parsers.hexToArray(vcKeysetValue).length);
            		cryptoValue = Parsers.arrayToHex(bVcKeysetPlain);
            	} else if(vcTypeValue.startsWith("02")) {  // MIFARE Desfire
            		if(vcDesfCryptoValue.equals("00")) {
            			byte[] bVcKeysetPlain = {(byte)
                            0xA8, 19,
                                0x20, 17,
                                    0x00,
                                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
                        };
            			
            			bVcKeysetPlain[4] = 0x00;
            			System.arraycopy(Parsers.hexToArray(vcKeysetValue), 0, bVcKeysetPlain, 5, Parsers.hexToArray(vcKeysetValue).length);
            			cryptoValue = Parsers.arrayToHex(bVcKeysetPlain);
            		} else if(vcDesfCryptoValue.equals("01")) {
            			byte[] bVcKeysetPlain = {(byte) 
                            0xA8, 33,
                                0x20, 25,
                                    0x00,
                                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
                        };
            			
            			bVcKeysetPlain[4] = 0x01;
            			System.arraycopy(Parsers.hexToArray(vcKeysetValue), 0, bVcKeysetPlain, 5, Parsers.hexToArray(vcKeysetValue).length);
            			cryptoValue = Parsers.arrayToHex(bVcKeysetPlain);
            		} else if(vcDesfCryptoValue.equals("02")) {
            			byte[] bVcKeysetPlain = {(byte) 
                            0xA8, 20,
                                0x20, 18,
                                    0x00,
                                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                    0x00
                        };
            
            			bVcKeysetPlain[4] = 0x02;
            			System.arraycopy(Parsers.hexToArray(vcKeysetValue), 0, bVcKeysetPlain, 5, Parsers.hexToArray(vcKeysetValue).length);
            			
            			String vcKeyVersionValue = ""; // vcKeyVersion.getText().toString();
            			if(vcKeyVersionValue != null && vcKeyVersionValue.length() == 2)
            				bVcKeysetPlain[21] = Parsers.hexToArray(vcKeyVersionValue)[0];
            			else
            				bVcKeysetPlain[21] = 0x00;
            			
            			cryptoValue = Parsers.arrayToHex(bVcKeysetPlain);
            		}
            	}
            
            	command = command.concat(cryptoValue);
            }
            
            if (vcTypeValue.startsWith("02")) {
                String desfAids = "F8";
                            
                desfAids = desfAids.concat("00".substring(
                        Integer.toHexString(listApps.size() * 3).length()) + Integer.toHexString(listApps.size() * 3));
                    
                for(String aid : listApps)
                    desfAids = desfAids.concat(aid);
                    
                command = command.concat(desfAids);
            }
            
            if(vcConfValue.equals("02") == true) {
                callbackContext.error("Caso no tenido en cuenta 2");
            } else {
                perso = "";
            }
            
            // Crear tarjeta
            boolean success = false;
			
	        byte[] VC_Data = Parsers.parseHexProperty("VCCreateCommand", command);
	        byte[] PersonalizeData = null;
	        
	        Log.d("MyMifareApp", "CreateVC Command: " + command);
	        
	        if(perso.equals("") == false) {
	        	PersonalizeData = Parsers.parseHexProperty("VCCreatePerso", perso);
	        	Log.d("MyMifareApp", "VCCreatePerso data: " + perso);
	        }
	        
	        byte[] result = commService.getInterface().createVirtualCard(VC_Data, PersonalizeData);
            
            short status = Parsers.getSW(result);
            switch (status) {
                case StatusBytes.SW_NO_ERROR:
                    VirtualCard vcEntry;
                    
                    if (vcTypeValue.startsWith("01")) {
                        vcEntry = Parsers.getVcEntry(VirtualCard.VC_MIFARE_CLASSIC, result);
                    } else {
                        vcEntry = Parsers.getVcEntry(VirtualCard.VC_MIFARE_DESFIRE, result);
                    }
                    
                    vcEntry.setVcName(cardName);
                    //vcEntry.setVcIconPath(vcIconPath);			       
                    	
                    VCEntries.addEntry(vcEntry, cordova.getActivity().getApplicationContext());
                    
                    // If the entry was created defining the SM AID we have to update the value for the next VC Creation
                    if (vcConfValue.equals("02") == true) {
                        int index = MyPreferences.getEntryIndex(cordova.getActivity().getApplicationContext());
                        MyPreferences.setEntryIndex(cordova.getActivity().getApplicationContext(), index + 1);
                    }
                    
        	        callbackContext.success(Parsers.arrayToHex(result));
                    
                    break;
                case StatusBytes.SW_NO_SET_SP_SD:
        	        callbackContext.success("NO SET SP SD");
                    break;
                case StatusBytes.SW_OPEN_LOGICAL_CHANNEL_FAILED:
        	        callbackContext.success("SW_OPEN_LOGICAL_CHANNEL_FAILED");
                    break;
                case StatusBytes.SW_REGISTRY_FULL:
        	        callbackContext.success("SW_REGISTRY_FULL");
                    break;
                case StatusBytes.SW_SE_TRANSCEIVE_FAILED:
        	        callbackContext.success("SW_SE_TRANSCEIVE_FAILED");
                    break;
                case StatusBytes.SW_SELECT_LTSM_FAILED:
        	        callbackContext.success("SW_SELECT_LTSM_FAILED");
                    break;
                case StatusBytes.CREATEVC_INVALID_SIGNATURE:
        	        callbackContext.success("INVALID_SIGNATURE");
                    break;
                case StatusBytes.CREATEVC_PREPERSO_NOT_FOUND:
        	        callbackContext.success("CREATEVC_PREPERSO_NOT_FOUND");
                    break;
                case StatusBytes.CREATEVC_REGISTRY_ENTRY_CREATION_FAILED:
        	        callbackContext.success("CREATEVC_REGISTRY_ENTRY_CREATION_FAILED");
                    break;
                case StatusBytes.CREATEVC_SET_SP_SD_NOT_SENT:
        	        callbackContext.success("CREATEVC_SET_SP_SD_NOT_SENT");
                    break;
                case StatusBytes.CREATEVC_SET_SP_SD_SENT:
        	        callbackContext.success("CREATEVC_SET_SP_SD_SENT");
                    break;
                default:
        	        callbackContext.error("Error Occured :" +  Parsers.bytArrayToHex(result));
                    break;
            }
        } catch (Exception e) {
            e.printStackTrace();
            callbackContext.error(e.getMessage());
        }
    }
    private void xpDeleteVC(JSONArray data, CallbackContext callbackContext) {
        try {
            JSONObject object = data.getJSONObject(0);
            Integer entryId = object.getInt("id");
            
			byte[] result = commService.getInterface().deleteVirtualCard(entryId);
        			
            short status = Parsers.getSW(result);
            switch (status) {
                case StatusBytes.SW_NO_ERROR:
                    VCEntries.deleteEntry(entryId, cordova.getActivity().getApplicationContext());
        	        callbackContext.success(Parsers.arrayToHex(result));
                    break;
                case StatusBytes.SW_INVALID_VC_ENTRY:
        	        callbackContext.error("SW_INVALID_VC_ENTRY");
                    break;
                case StatusBytes.SW_OPEN_LOGICAL_CHANNEL_FAILED:
        	        callbackContext.error("SW_OPEN_LOGICAL_CHANNEL_FAILED");
                    break;
                case StatusBytes.SW_IMPROPER_REGISTRY:
        	        callbackContext.error("SW_IMPROPER_REGISTRY");
                    break;
                case StatusBytes.SW_PROCESSING_ERROR:
        	        callbackContext.error("SW_PROCESSING_ERROR");
                    break;
                case StatusBytes.SW_REGISTRY_FULL:
        	        callbackContext.error("SW_REGISTRY_FULL");
                    break;
                default:
        	        callbackContext.error("Error Occured :" +  Parsers.bytArrayToHex(result));
                    break;
            }
        } catch (Exception e) {
            e.printStackTrace();
            callbackContext.error(e.getMessage());
        }
    }
    private void xpActivateVC(JSONArray data, CallbackContext callbackContext) {
        try {
            JSONObject object = data.getJSONObject(0);
            Integer entryId = object.getInt("id");

	        Log.d("NFCPlugin", "xpActivateVC : " + entryId);
            
			byte[] result = commService.getInterface().activateVirtualCard(entryId, true, true);
        			
            short status = Parsers.getSW(result);
            switch (status) {
                case StatusBytes.SW_NO_ERROR:
                    callbackContext.success(Parsers.arrayToHex(result));
                    break;
                case StatusBytes.SW_INVALID_VC_ENTRY:
                    callbackContext.error("SW_INVALID_VC_ENTRY");
                    break;
                case StatusBytes.SW_ALREADY_ACTIVATED:
                    callbackContext.error("SW_ALREADY_ACTIVATED");
                    break;
                case StatusBytes.SW_IMPROPER_REGISTRY:
                    callbackContext.error("SW_IMPROPER_REGISTRY");
                    break;
                case StatusBytes.SW_PROCESSING_ERROR:
                    callbackContext.error("SW_PROCESSING_ERROR");
                    break;
                case StatusBytes.SW_REGISTRY_FULL:
                    callbackContext.error("SW_REGISTRY_FULL");
                    break;
                case StatusBytes.SW_ALREADY_DEACTIVATED:
                    callbackContext.error("SW_ALREADY_DEACTIVATED");
                    break;
                case StatusBytes.SW_CRS_SELECT_FAILED:
                    callbackContext.error("SW_CRS_SELECT_FAILED");
                    break;
                default:
                    callbackContext.error("Error Occured :" +  Parsers.bytArrayToHex(result));
                    break;
            }
        } catch (Exception e) {
            e.printStackTrace();
            callbackContext.error(e.getMessage());
        }
    }
                          
    private void execScript(Tag tag)
    {
        try{
            createKeyPair();
            MifareClassic mfc = MifareClassic.get(tag);
            resScriptData = new JSONArray();
            if (mfc != null) {
                byte[] data;
                byte[] dataWrite;
                boolean checkCase = true;
                boolean checkTagLost = false;
                boolean checkIOError = false;
                
                try {
                    //foreach Sector autenticar y leer bloque
                    mfc.connect();
                    if (mfc.isConnected()) {  
                        try {
                            JSONArray jsonArray = new JSONArray(scriptData);
                            JSONObject jsonObject;
                            boolean auth = false;
                            int operation;
                            int sector;
                            int block;
                            int keyType;
                            int from;
                            int to =0;
                            String dataBlock;
                            byte[] key;
                            int count = jsonArray.length();
                            if (count == 1) {
                                try {
                                jsonObject = jsonArray.getJSONObject(0);  
                                scriptData = jsonObject.get("data").toString();  
                                jsonArray = new JSONArray(scriptData);       
                                } catch (Exception e){
                                    //Parche para cuando me llega el objeto para escribir. Llega con dos corchetes de mas.
                                    scriptData = scriptData.substring(1,scriptData.length()-1);
                                    jsonArray = new JSONArray(scriptData);
                                } 
                            }       
                                
                            count = jsonArray.length();
                            for (int i=0 ; i< count; i++){ 
                                if (!checkCase) 
                                {
                                    getActivity().runOnUiThread(new Runnable() {
                                        public void run() {
                                            Toast.makeText(cordova.getActivity().getApplicationContext(),
                                                "Debe volver a acercar su tarjeta porque los datos han cambiado desde la ultima lectura", 
                                                Toast.LENGTH_LONG).show();
                                        }
                                    }); 
                                    break;
                                }
                                if (checkTagLost || checkIOError) 
                                {
                                    errorText = "";
                                    if(checkTagLost)
                                        errorText += " TagLost";
                                    if(checkIOError)
                                        errorText += " IOError";
                                    getActivity().runOnUiThread(new Runnable() {
                                        public void run() {
                                            Toast.makeText(
                                                cordova.getActivity().getApplicationContext(),
                                                "Se ha perdido la conexion con su tarjeta." + errorText, 
                                                Toast.LENGTH_LONG
                                            ).show();
                                        }
                                    }); 
                                    break;
                                }
                                jsonObject = jsonArray.getJSONObject(i);  
                                operation = jsonObject.getInt("operation");     
                                sector = jsonObject.getInt("sector");
                                
                                switch(operation){
                                    case 1: //Autentication
                                        try{
                                            key = hexStringToByteArray(jsonObject.get("key").toString());
                                            Log.e(TAG, "HSM Auth " + jsonObject.get("key").toString());
                                            
                                            if (hsm == null)
                                                Log.e(TAG, "HSM not initialized");
                                            else {
                                                Log.e(TAG, "HSM cyphered Key: " + key);
                                                key = hsm.decryptRSA(key);
                                                Log.e(TAG, "HSM plain Key: " + key);
                                            }
                                            
                                            //Pruebas AES
                                            //key = Util.encrypt(key); 
                                            //key = Util.decrypt(key); 
                                            //END Pruebas
                                            //Pruebas RSA
                                            Log.e(TAG, "encrypt INI "+jsonObject.get("key").toString());
                                            //key = EigeHsmService.encryptRSA(key);
                                            Log.e(TAG, "encrypt FIN "+Util.bytesToHex(key));
                                            // errorText = Util.bytesToHex(key);
                                            // getActivity().runOnUiThread(new Runnable() {
                                            //     public void run() {
                                            //         Toast.makeText(cordova.getActivity().getApplicationContext(),
                                            //             "Encrypt: "+errorText, 
                                            //             Toast.LENGTH_SHORT).show();
                                            //     }
                                            // }); 
                                            //key = EigeHsmService.decryptRSA(key);
                                            Log.e(TAG, "decrypt FIN "+Util.bytesToHex(key));
                                            //END Pruebas RSA
                                            keyType = jsonObject.getInt("keyType");
                                            if(keyType==0) 
                                                auth=mfc.authenticateSectorWithKeyA(sector, key);
                                            else 
                                                auth=mfc.authenticateSectorWithKeyB(sector, key);
                                            if(!auth)
                                                myCallBack.error("Authentication error: sector "+sector);
                                        }catch(TagLostException eTag){
                                            checkTagLost=true;
                                            myCallBack.error("Fallo lectura Tarjeta Perdida: sector "+sector);
                                        }catch(IOException eIO){
                                            myCallBack.error("Fallo lectura Tarjeta IO: sector "+sector);
                                        }
                                        break;
                                    case 2: //Read
                                        block = jsonObject.getInt("block");
                                        if(auth){ 
                                            try{
                                                data = mfc.readBlock(sector*4+block);       
                                                if(jsonObject.has("from") && jsonObject.has("to"))
                                                {
                                                    from = jsonObject.getInt("from");
                                                    to = jsonObject.getInt("to");
                                                    data = Util.GetLittleEndian(data, from, to);
                                                }
                                                JSONObject resJsonObject = new JSONObject();
                                                resJsonObject.put("sector",sector);
                                                resJsonObject.put("block",block);
                                                resJsonObject.put("operation",operation);
                                                resJsonObject.put("data",bytesToHex(data));
                                                resScriptData.put(resJsonObject);
                                            }catch(TagLostException eTag){
                                                checkTagLost=true;
                                                myCallBack.error("Fallo lectura Tarjeta Perdida: sector "+sector+" bloque "+block);
                                            }catch(IOException eIO){
                                                myCallBack.error("Fallo lectura Tarjeta IO: sector "+sector+" bloque "+block);
                                            }
                                        }  
                                        break;
                                    case 3: //Write
                                        block = jsonObject.getInt("block");
                                        if(auth){
                                            try{
                                                data = mfc.readBlock(sector*4+block);
                                                dataBlock = jsonObject.get("data").toString();
                                                dataWrite = hexStringToByteArray(dataBlock);
                                                if(jsonObject.has("from") && jsonObject.has("to"))
                                                {
                                                    from = jsonObject.getInt("from");
                                                    to = jsonObject.getInt("to");
                                                    dataWrite = Util.SetLittleEndian(data, from, to, hexStringToByteArray(dataBlock));
                                                }
                                                mfc.writeBlock(sector*4+block,dataWrite);
                                            }catch(TagLostException eTag){
                                                checkTagLost=true;
                                                myCallBack.error("Fallo lectura Tarjeta Perdida: sector "+sector+" bloque "+block);
                                            }catch(IOException eIO){
                                                myCallBack.error("Fallo lectura Tarjeta IO: sector "+sector+" bloque "+block);
                                            }
                                        }
                                        break;
                                    case 4: //Check 
                                        block = jsonObject.getInt("block");
                                        try{
                                            block = jsonObject.getInt("block");
                                            dataBlock = jsonObject.get("data").toString();
                                            data = mfc.readBlock(sector*4+block);
                                            
                                            dataWrite = hexStringToByteArray(dataBlock);
                                            if(jsonObject.has("from") && jsonObject.has("to"))
                                            {
                                                from = jsonObject.getInt("from");
                                                to = jsonObject.getInt("to");
                                                data = Util.GetLittleEndian(data, from, to);
                                            }
                                            if(!Arrays.equals(data, dataWrite))
                                                checkCase = false;
                                        }catch(TagLostException eTag){
                                            checkTagLost=true;
                                            myCallBack.error("Fallo lectura Tarjeta Perdida: sector "+sector+" bloque "+block);
                                        }catch(IOException eIO){
                                            myCallBack.error("Fallo lectura Tarjeta IO: sector "+sector+" bloque "+block);
                                        }
                                        break;
                                    case 5: //Suma X 
                                        block = jsonObject.getInt("block");
                                        if(auth){
                                            try{
                                                int value =0;
                                                data = mfc.readBlock(sector*4+block);
                                                value = jsonObject.getInt("data");
                                                if(jsonObject.has("from") && jsonObject.has("to"))
                                                {
                                                    from = jsonObject.getInt("from");
                                                    to = jsonObject.getInt("to");
                                                    data = Util.GetLittleEndian(data, from, to);
                                                    value += new BigInteger(data).intValue();
                                                    dataWrite = Util.SetLittleEndian(data, from, to, BigInteger.valueOf(value).toByteArray());
                                                    mfc.writeBlock(sector*4+block,dataWrite);
                                                }
                                            }catch(TagLostException eTag){
                                                checkTagLost=true;
                                                myCallBack.error("Fallo lectura Tarjeta Perdida: sector "+sector+" bloque "+block);
                                            }catch(IOException eIO){
                                                myCallBack.error("Fallo lectura Tarjeta IO: sector "+sector+" bloque "+block);
                                            }
                                        }
                                        break;
                                }
                                
                            }
                        } catch (JSONException e) {
                            e.printStackTrace();
                            myCallBack.error("Error Operation Card");     
                        }finally{
                            mfc.close();
                            myCallBack.success("OK");
                        } 
                    }     
                }catch (IOException e) { 
                    Log.e(TAG, e.getLocalizedMessage());
                    myCallBack.error("Error connecting NFC"); 
                } 
                finally{
                    scriptData="";
                }      
            }   
        }catch(Exception e){
            Log.e(TAG, e.getLocalizedMessage());
            myCallBack.error("Error connecting Tag"); 
        }
    }
    void createKeyPair()
    {
    }

    //data: section, block, message
    private void write(JSONArray data, CallbackContext callbackContext) throws JSONException {
        execScript(tag);

        Tag tag = savedIntent.getParcelableExtra(NfcAdapter.EXTRA_TAG);
        NdefRecord[] records = Util.jsonToNdefRecords(data.getString(0));
        writeNdefMessage(new NdefMessage(records), tag, callbackContext);
    }
    private void fireTagEvent (Tag tag) {

        String command = MessageFormat.format(javaScriptEventTemplate, TAG_DEFAULT, Util.tagToJSON(tag));
        Log.v(TAG, command);
        this.webView.sendJavascript(command);
    }

    JSONObject buildNdefJSON(Ndef ndef, Parcelable[] messages) {

        JSONObject json = Util.ndefToJSON(ndef);

        // ndef is null for peer-to-peer
        // ndef and messages are null for ndef format-able
        if (ndef == null && messages != null) {

            try {

                if (messages.length > 0) {
                    NdefMessage message = (NdefMessage) messages[0];
                    json.put("ndefMessage", Util.messageToJSON(message));
                    // guessing type, would prefer a more definitive way to determine type
                    json.put("type", "NDEF Push Protocol");
                }

                if (messages.length > 1) {
                    Log.wtf(TAG, "Expected one ndefMessage but found " + messages.length);
                }

            } catch (JSONException e) {
                // shouldn't happen
                Log.e(Util.TAG, "Failed to convert ndefMessage into json", e);
            }
        }
        return json;
    }

    @Override
    public void onPause(boolean multitasking) {
        Log.d(TAG, "onPause() " + getIntent());
        super.onPause(multitasking);
        
        if (multitasking) {
            // nfc can't run in background
            getNfcHandler().
            stopNfc();
        }
    }

    @Override
    public void onResume(boolean multitasking) {
        Log.d(TAG, "onResume() " + getIntent());
        super.onResume(multitasking);
        
        getNfcHandler().
            startNfc();
    }

    @Override
    public void onNewIntent(Intent intent) {
        try {
        Log.d(TAG, "onNewIntent " + intent);
        super.onNewIntent(intent);
        
        // setIntent(intent);
        // savedIntent = intent;
        getNfcHandler().parseMessage(intent);
        } catch (Exception ex) {}
    }

    private Activity getActivity() {
        return this.cordova.getActivity();
    }

    private Intent getIntent() {
        return getActivity().getIntent();
    }

    private void setIntent(Intent intent) {
        getActivity().setIntent(intent);
    }

    String javaScriptEventTemplate =
        "var e = document.createEvent(''Events'');\n" +
        "e.initEvent(''{0}'');\n" +
        "e.tag = {1};\n" +
        "document.dispatchEvent(e);";
        
    @Override
    public void onNdefPushComplete(NfcEvent event) {

        // handover (beam) take precedence over share tag (ndef push)
        if (handoverCallback != null) {
            PluginResult result = new PluginResult(PluginResult.Status.OK, "Beamed Message to Peer");
            result.setKeepCallback(true);
            handoverCallback.sendPluginResult(result);
        } else if (shareTagCallback != null) {
            PluginResult result = new PluginResult(PluginResult.Status.OK, "Shared Message with Peer");
            result.setKeepCallback(true);
            shareTagCallback.sendPluginResult(result);
        }

    }
    
    // This field exists only to support getEntry, below, which has been deprecated
    public static NfcPlugin filePlugin;
    private ArrayList<Filesystem> filesystems;
    private boolean configured = false;
    private PendingRequests pendingRequests;
    
    public CordovaWebView webView_;
    
    @Override
    public void initialize(CordovaInterface cordova, CordovaWebView webView) {
    	super.initialize(cordova, webView);
    	this.filesystems = new ArrayList<Filesystem>();
        this.pendingRequests = new PendingRequests();
        webView_ = webView;

    	String tempRoot = null;
    	String persistentRoot = null;

    	Activity activity = cordova.getActivity();
    	String packageName = activity.getPackageName();

        String location = preferences.getString("androidpersistentfilelocation", "internal");

    	tempRoot = activity.getCacheDir().getAbsolutePath();
    	if ("internal".equalsIgnoreCase(location)) {
    		persistentRoot = activity.getFilesDir().getAbsolutePath() + "/files/";
    		this.configured = true;
    	} else if ("compatibility".equalsIgnoreCase(location)) {
    		/*
    		 *  Fall-back to compatibility mode -- this is the logic implemented in
    		 *  earlier versions of this plugin, and should be maintained here so
    		 *  that apps which were originally deployed with older versions of the
    		 *  plugin can continue to provide access to files stored under those
    		 *  versions.
    		 */
    		if (Environment.getExternalStorageState().equals(Environment.MEDIA_MOUNTED)) {
    			persistentRoot = Environment.getExternalStorageDirectory().getAbsolutePath();
    			tempRoot = Environment.getExternalStorageDirectory().getAbsolutePath() +
    					"/Android/data/" + packageName + "/cache/";
    		} else {
    			persistentRoot = "/data/data/" + packageName;
    		}
    		this.configured = true;
    	}

    	if (this.configured) {
			// Create the directories if they don't exist.
			File tmpRootFile = new File(tempRoot);
            File persistentRootFile = new File(persistentRoot);
            tmpRootFile.mkdirs();
            persistentRootFile.mkdirs();

    		// Register initial filesystems
    		// Note: The temporary and persistent filesystems need to be the first two
    		// registered, so that they will match window.TEMPORARY and window.PERSISTENT,
    		// per spec.
    		this.registerFilesystem(new LocalFilesystem("temporary", webView.getContext(), webView.getResourceApi(), tmpRootFile));
    		this.registerFilesystem(new LocalFilesystem("persistent", webView.getContext(), webView.getResourceApi(), persistentRootFile));
    		this.registerFilesystem(new ContentFilesystem(webView.getContext(), webView.getResourceApi()));
            this.registerFilesystem(new AssetFilesystem(webView.getContext().getAssets(), webView.getResourceApi()));

            registerExtraFileSystems(getExtraFileSystemsPreference(activity), getAvailableFileSystems(activity));

    		// Initialize static plugin reference for deprecated getEntry method
    		if (filePlugin == null) {
    			filePlugin = this;
    		}
    	} else {
    		Log.e(TAG, "File plugin configuration error: Please set AndroidPersistentFileLocation in config.xml to one of \"internal\" (for new applications) or \"compatibility\" (for compatibility with previous versions)");
    		activity.finish();
    	}
    }
    public void registerFilesystem(Filesystem fs) {
    	if (fs != null && filesystemForName(fs.name)== null) {
    		this.filesystems.add(fs);
    	}
    }
    protected String[] getExtraFileSystemsPreference(Activity activity) {
        String fileSystemsStr = preferences.getString("androidextrafilesystems", "files,files-external,documents,sdcard,cache,cache-external,root");
        return fileSystemsStr.split(",");
    }
    protected HashMap<String, String> getAvailableFileSystems(Activity activity) {
        Context context = activity.getApplicationContext();
        HashMap<String, String> availableFileSystems = new HashMap<String,String>();

        availableFileSystems.put("files", context.getFilesDir().getAbsolutePath());
        availableFileSystems.put("documents", new File(context.getFilesDir(), "Documents").getAbsolutePath());
        availableFileSystems.put("cache", context.getCacheDir().getAbsolutePath());
        availableFileSystems.put("root", "/");
        if (Environment.getExternalStorageState().equals(Environment.MEDIA_MOUNTED)) {
          try {
            availableFileSystems.put("files-external", context.getExternalFilesDir(null).getAbsolutePath());
            availableFileSystems.put("sdcard", Environment.getExternalStorageDirectory().getAbsolutePath());
            availableFileSystems.put("cache-external", context.getExternalCacheDir().getAbsolutePath());
          }
          catch(NullPointerException e) {
              Log.d(TAG, "External storage unavailable, check to see if USB Mass Storage Mode is on");
          }
        }

        return availableFileSystems;
    }
    private Filesystem filesystemForName(String name) {
    	for (Filesystem fs:filesystems) {
    		if (fs != null && fs.name != null && fs.name.equals(name)) {
    			return fs;
    		}
    	}
    	return null;
    }
    protected void registerExtraFileSystems(String[] filesystems, HashMap<String, String> availableFileSystems) {
        HashSet<String> installedFileSystems = new HashSet<String>();

        /* Register filesystems in order */
        for (String fsName : filesystems) {
            if (!installedFileSystems.contains(fsName)) {
                String fsRoot = availableFileSystems.get(fsName);
                if (fsRoot != null) {
                    File newRoot = new File(fsRoot);
                    if (newRoot.mkdirs() || newRoot.isDirectory()) {
                        registerFilesystem(new LocalFilesystem(fsName, webView.getContext(), webView.getResourceApi(), newRoot));
                        installedFileSystems.add(fsName);
                    } else {
                       Log.d(TAG, "Unable to create root dir for filesystem \"" + fsName + "\", skipping");
                    }
                } else {
                    Log.d(TAG, "Unrecognized extra filesystem identifier: " + fsName);
                }
            }
        }
    }
    public JSONObject resolveLocalFileSystemURI(String uriString) throws IOException, JSONException {
        if (uriString == null) {
            throw new MalformedURLException("Unrecognized filesystem URL");
        }
        Uri uri = Uri.parse(uriString);
        boolean isNativeUri = false;

        LocalFilesystemURL inputURL = LocalFilesystemURL.parse(uri);
        if (inputURL == null) {
            /* Check for file://, content:// urls */
            inputURL = resolveNativeUri(uri);
            isNativeUri = true;
        }

        try {
            Filesystem fs = this.filesystemForURL(inputURL);
            if (fs == null) {
                throw new MalformedURLException("No installed handlers for this URL");
            }
            if (fs.exists(inputURL)) {
                if (!isNativeUri) {
                    fs.toNativeUri(inputURL);
                    // If not already resolved as native URI, resolve to a native URI and back to
                    // fix the terminating slash based on whether the entry is a directory or file.
                    inputURL = fs.toLocalUri(fs.toNativeUri(inputURL));
                }

                return fs.getEntryForLocalURL(inputURL);
            }
        } catch (IllegalArgumentException e) {
            throw new MalformedURLException("Unrecognized filesystem URL");
        } catch (Exception e) {
            throw new MalformedURLException("Unrecognized filesystem URL");
        }
        throw new FileNotFoundException();
    }
    public LocalFilesystemURL resolveNativeUri(Uri nativeUri) {
        LocalFilesystemURL localURL = null;

        // Try all installed filesystems. Return the best matching URL
        // (determined by the shortest resulting URL)
        for (Filesystem fs : filesystems) {
            LocalFilesystemURL url = fs.toLocalUri(nativeUri);
            if (url != null) {
                // A shorter fullPath implies that the filesystem is a better
                // match for the local path than the previous best.
                if (localURL == null || (url.uri.toString().length() < localURL.toString().length())) {
                    localURL = url;
                }
            }
        }
        return localURL;
    }
	public Filesystem filesystemForURL(LocalFilesystemURL localURL) {
    	if (localURL == null) return null;
    	return filesystemForName(localURL.fsName);
    }
    private void threadhelper(final FileOp f, final JSONArray args, final CallbackContext callbackContext){
        cordova.getThreadPool().execute(new Runnable() {
            public void run() {
                try {
                    //JSONArray args = new JSONArray(rawArgs);
                    f.run(args);
                } catch ( Exception e) {
                    if( e instanceof EncodingException){
                        callbackContext.error(FileUtils.ENCODING_ERR);
                    } else if(e instanceof FileNotFoundException) {
                        callbackContext.error(FileUtils.NOT_FOUND_ERR);
                    } else if(e instanceof FileExistsException) {
                        callbackContext.error(FileUtils.PATH_EXISTS_ERR);
                    } else if(e instanceof NoModificationAllowedException ) {
                        callbackContext.error(FileUtils.NO_MODIFICATION_ALLOWED_ERR);
                    } else if(e instanceof InvalidModificationException ) {
                        callbackContext.error(FileUtils.INVALID_MODIFICATION_ERR);
                    } else if(e instanceof MalformedURLException ) {
                        callbackContext.error(FileUtils.ENCODING_ERR);
                    } else if(e instanceof IOException ) {
                        callbackContext.error(FileUtils.INVALID_MODIFICATION_ERR);
                    } else if(e instanceof EncodingException ) {
                        callbackContext.error(FileUtils.ENCODING_ERR);
                    } else if(e instanceof TypeMismatchException ) {
                        callbackContext.error(FileUtils.TYPE_MISMATCH_ERR);
                    } else if(e instanceof JSONException ) {
                        callbackContext.sendPluginResult(new PluginResult(PluginResult.Status.JSON_EXCEPTION));
                    } else {
                        e.printStackTrace();
                    	callbackContext.error(FileUtils.UNKNOWN_ERR);
                    }
                }
            }
        });
    }
    private interface FileOp {
        void run(JSONArray args) throws Exception;
    }
}
