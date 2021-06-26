package com.payin.nfc;

import android.app.Activity;
import android.app.PendingIntent;

import android.content.Intent;
import android.content.IntentFilter;
import android.content.res.AssetManager;

import android.os.Parcelable;

import android.nfc.NdefMessage;
import android.nfc.NfcAdapter;
import android.nfc.Tag;
import android.nfc.TagLostException;

import android.nfc.tech.MifareClassic;
import android.nfc.tech.NfcA;
import android.nfc.tech.Ndef;

import android.util.Log;

import java.lang.Runnable;

import java.io.IOException;

import java.security.cert.CertificateException;
import java.security.KeyStoreException;
import java.security.NoSuchAlgorithmException;

import java.text.MessageFormat;

import java.util.ArrayList;
import java.util.List;

import java.util.concurrent.ExecutorService;

import org.apache.cordova.CallbackContext;
import org.apache.cordova.CordovaWebView;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

public class NfcHandler {
    private int CARDTYPE_CLASSIC = 1;
    private int CARDTYPE_DESFIRE = 2;
    private int CARDTYPE_NFCA = 3;
    private static final String TAG = "NfcHandler";
    private static Tag tag = null;
    private static String responseKeys = "";
    private static JSONArray responseScript = null;
    private static JSONObject responseOperation = null;
    private static int responseSlot = 0;

    private static String javaScriptEventTemplate =
        "var e = document.createEvent(''Events'');\n" +
        "e.initEvent(''{0}'');\n" +
        "e.tag = {1};\n" +
        "document.dispatchEvent(e);";

    private CordovaWebView webView;
    private Activity activity;
    private ExecutorService threadPool;
    private CallbackContext callbackContext;

    private NdefMessage p2pMessage = null;

    private static PendingIntent pendingIntent = null;
    private static IntentFilter[] intentFilters = null;
    private static String[][] techLists = null;

    private NfcHandlerClassic nfcHandlerClassic;

    public NfcHandler(CordovaWebView _webView, Activity _activity, ExecutorService _threadPool, AssetManager _assetManager) {
        Log.d(TAG, "INI NfcHandler() " + _activity);

        webView = _webView;
        activity = _activity;
        threadPool = _threadPool;
        nfcHandlerClassic = new NfcHandlerClassic(_assetManager);

        Log.d(TAG, "END NfcHandler() " + _activity);
    }
    public void startNfc() {
        Log.d(TAG, "INI startNfc() " + pendingIntent);

        if (NfcAdapter.getDefaultAdapter(activity) != null) {
            // onResume can call startNfc before execute
            if (pendingIntent == null) {
                // PendingIntent
                pendingIntent = PendingIntent.getActivity(
                    activity,
                    0,
                    new Intent(activity, activity.getClass())
                    .addFlags(Intent.FLAG_ACTIVITY_SINGLE_TOP | Intent.FLAG_ACTIVITY_CLEAR_TOP),
                    0);

                // Intents
                // Filters
                //IntentFilter intent1 = new IntentFilter(NfcAdapter.ACTION_NDEF_DISCOVERED);
                //intent1.addDataType("*/*");

                intentFilters = new IntentFilter[] { new IntentFilter(NfcAdapter.ACTION_TECH_DISCOVERED) };

                // TechList
                techLists = new String [][] { 
                    new String[] { NfcA.class.getName() },
                    new String[] { MifareClassic.class.getName() }
                };
            }

            //if (activity.isFinishing()) {
            NfcAdapter.getDefaultAdapter(activity)
                .enableForegroundDispatch(
                activity,
                pendingIntent,
                intentFilters,
                techLists
            );
        }

        Log.d(TAG, "END startNfc()");
    }
    public void stopNfc() {
        Log.d(TAG, "INI stopNfc()");
        
        if (NfcAdapter.getDefaultAdapter(activity) != null) {
            NfcAdapter.getDefaultAdapter(activity)
                .disableForegroundDispatch(activity);
        }

        Log.d(TAG, "END stopNfc()");
    }
    private void waitResponse(int miliseconds) {
        try {
            Log.v(TAG, "waitResponse: Esperando script de server");

            int i = 1;
            responseScript = null;
            while (responseScript == null && i < miliseconds/100) {
                Thread.sleep(1000); // 100ms
                i++;
            }
            
            Log.v(TAG, "waitResponse: Esperando script de server (" + (i * 100) + ")");
        } catch (InterruptedException ex) {
            Log.v(TAG, "cleanCache: Error de interrupción");
        }
    }
    private boolean checkWrite(JSONArray result) throws JSONException {
        Log.d(TAG, "checkWrite: " + result);

        for (int i = 0; i < result.length(); i++) {
            JSONObject obj = result.getJSONObject(i);

            if (obj.getInt("operation") == 3) {
                Log.d(TAG, "checkWrite: true");
                return true;
            }
        }

        Log.d(TAG, "checkWrite: false");
        return false;
    }
    public void parseMessage(final Intent intent) {
        Log.d(TAG, "parseMessage: INI");
        threadPool.execute(new Runnable() {
            @Override
            public void run() {
                String action = intent.getAction();
                if (action == null)
                    return;
                tag = intent.getParcelableExtra(NfcAdapter.EXTRA_TAG);
                if (action.equals(NfcAdapter.ACTION_TECH_DISCOVERED)) {
                    boolean isClassic = false;
                    boolean isNfcA = false;
                    for (String tagTech : tag.getTechList()) {
                        if (tagTech.equals(MifareClassic.class.getName())) {
                            isClassic = true;
                        } else if (tagTech.equals(NfcA.class.getName())) {
                            isNfcA = true;
                        }
                    }
                    if (isClassic) {
                        Log.v(TAG, "parseMessage: Tarjeta MifareClassic detectada");
                        try {
                            if (responseScript == null) { // filtro para lectura
                                Log.v(TAG, "parseMessage: Pidiendo script a server");

                                // Get script
                                fireNfcScriptDetect(tag.getId(), CARDTYPE_CLASSIC);
                                waitResponse(30000); // 30seg

                                if (responseScript == null) {
                                    Log.v(TAG, "parseMessage: Timeout: No ha vuelto el script");
                                    fireNfcScriptError(
                                        tag.getId(),
                                        CARDTYPE_CLASSIC,
                                        "Error esperando a leer la tarjeta"
                                    );
                                } else {
                                    Log.v(TAG, "parseMessage: Ha llegado la contestacion");
                                    JSONArray result = nfcHandlerClassic.executeScript(activity, threadPool, tag, responseKeys, responseScript, responseOperation, responseSlot, callbackContext);

                                    if (result == null) {
                                        fireNfcScriptError(
                                            tag.getId(),
                                            CARDTYPE_CLASSIC,
                                            "La tarjeta sobre la que se quiere escribir no es la misma que se había leido o ha variado su contenido."
                                        );
                                    } else if (result.length() == 0) {
                                        Log.v(TAG, "parseMessage: No se ha ejecutado el script");
                                        fireNfcScriptError(
                                            tag.getId(),
                                            CARDTYPE_CLASSIC,
                                            "Error al leer la tarjeta"
                                        );
                                    } else {
                                        Log.v(TAG, "parseMessage: Ejecutando script (" + responseOperation + ")");
                                        
                                        scriptSuccess(activity, threadPool, tag, result, callbackContext);
                                    }
                                }
                            } else { // para escritura
                                Log.v(TAG, "parseMessage: Hay script pendiente de ejecución " + responseOperation);
                                JSONArray result = nfcHandlerClassic.executeScript(activity, threadPool, tag, responseKeys, responseScript, responseOperation, responseSlot, callbackContext);

                                if (result == null) {
                                    fireNfcScriptError(
                                        tag.getId(),
                                        CARDTYPE_CLASSIC,
                                        "La tarjeta sobre la que se quiere escribir no es la misma que se había leido o ha variado su contenido."
                                    );
                                } else if (result.length() == 0) {
                                    Log.v(TAG, "parseMessage: No se ha ejecutado la script");
                                    fireNfcScriptError(
                                        tag.getId(),
                                        CARDTYPE_CLASSIC,
                                        "Error al leer la tarjeta"
                                    );
                                } else {
                                    Log.v(TAG, "parseMessage: Ejecutando script (" + responseOperation + ")");
                                    fireNfcScriptSuccess(
                                        tag.getId(),
                                        CARDTYPE_CLASSIC,
                                        result,
                                        responseOperation != null && responseOperation.length() != 0 ? responseOperation.getInt("type") : 0,
                                        responseOperation != null && responseOperation.length() != 0 ? responseOperation.getInt("id") : 0,
                                        responseSlot
                                    );
                                }
                            }
                        } catch (JSONException e) {
                            e.printStackTrace();
                            callbackContext.error("Error Operation Card");
                        }
                    } else if (isNfcA) { 
                        Log.v(TAG, "parseMessage: Tarjeta MifareClassic no compatible detectada");
                        fireNfcScriptDetect(tag.getId(), CARDTYPE_NFCA);
                    }
                }

                cleanCache();

                activity.setIntent(new Intent());
            }
        });
        Log.d(TAG, "parseMessage: END parseMessage");
    }
    public void cleanCache() {
        Log.v(TAG, "cleanCache: Limpiando cache");
        responseKeys = "";
        responseScript = null;
        responseOperation = null;
        responseSlot = 0;
    }
    public void scriptSuccess( Activity activity, ExecutorService threadPool, Tag tag, JSONArray result, CallbackContext callbackContext) {
        try {
            fireNfcScriptSuccess(
                tag.getId(),
                CARDTYPE_CLASSIC,
                result,
                responseOperation != null && responseOperation.length() != 0 ? responseOperation.getInt("type") : 0,
                responseOperation != null && responseOperation.length() != 0 ? responseOperation.getInt("id") : 0,
                responseSlot
            );
            cleanCache();
            waitResponse(30000); // 30seg

            if (responseScript == null) {
                Log.v(TAG, "scriptSuccess: Timeout: No ha vuelto el confirmandreadinfo");
            } else {
                Log.v(TAG, "scriptSuccess: Ha llegado el script");
                result = nfcHandlerClassic.executeScript(activity, threadPool, tag, responseKeys, responseScript, responseOperation, responseSlot, callbackContext);

                if (result == null) {
                    fireNfcScriptError(
                        tag.getId(),
                        CARDTYPE_CLASSIC,
                        "La tarjeta sobre la que se quiere escribir no es la misma que se había leido o ha variado su contenido."
                    );
                } else if (result.length() == 0) {
                    Log.v(TAG, "scriptSuccess: No se ha ejecutado el script");
                    fireNfcScriptError(
                        tag.getId(),
                        CARDTYPE_CLASSIC,
                        "Error al leer la tarjeta"
                    );

// TENER EN CUENTA QUE NO HAYA WRITES
                } else {
                    // Comunicar confirmacion
                    scriptSuccess(activity, threadPool, tag, result, callbackContext);
                    // fireNfcScriptSuccess(
                    //     tag.getId(),
                    //     CARDTYPE_CLASSIC,
                    //     result,
                    //     responseOperation != null && responseOperation.length() != 0 ? responseOperation.getInt("type") : 0,
                    //     responseOperation != null && responseOperation.length() != 0 ? responseOperation.getInt("id") : 0,
                    //     responseSlot
                    // );
                }
            }
        }
        catch (JSONException e) {}
    }
    public void execute(JSONObject card, String keys, JSONArray script, JSONObject operation, int slot, CallbackContext callbackContext) {
        Log.d(TAG, "INI execute()");
        Log.d(TAG, "execute card:" + card);
        Log.d(TAG, "execute keys:" + keys);
        Log.d(TAG, "execute script:" + script);
        Log.d(TAG, "execute operation:" + operation);
        Log.d(TAG, "execute slot:" + slot);

        try {
            // Script debe ser el último porque lanza la ejecución
            responseKeys = keys;
            responseOperation = operation;
            responseSlot = slot;
            responseScript = script;

            callbackContext.success();
        } catch (Exception ex) {
            Log.e(TAG,"ERROR execute() "+ex);
        }

        Log.d(TAG, "END execute()");
    }
    public void setCallbackContext(CallbackContext _callbackContext) {
        callbackContext = _callbackContext;
    }
    public boolean recycledIntent() { // TODO this is a kludge, find real solution
        Log.d(TAG, "INI recycledIntent()");
        int flags = activity.getIntent().getFlags();
        if ((flags & Intent.FLAG_ACTIVITY_LAUNCHED_FROM_HISTORY) == Intent.FLAG_ACTIVITY_LAUNCHED_FROM_HISTORY) {
            Log.v(TAG, "Launched from history, killing recycled intent");
            activity.setIntent(new Intent());

            Log.d(TAG, "END recycledIntent()");
            return true;
        }

        Log.d(TAG, "END recycledIntent()");
        return false;
    }
    private void fireNfcScriptDetect (byte[] uid, int type) {
        Log.d(TAG, "INI fireNfcScriptDetect " + uid + " " + type);

        String command = MessageFormat.format(javaScriptEventTemplate, "nfc-script-detect", Util.tagToJSON2(uid, type, null, 0, 0, 0, null));
        Log.v(TAG, command);

        this.webView.sendJavascript(command);
        Log.d(TAG, "END fireNfcScriptDetect");
    }
    private void fireNfcScriptSuccess (byte[] uid, int type, JSONArray script, int operationType, int operationId, int slot) {
        Log.d(TAG, "INI fireNfcScriptSuccess uid:" + uid + " type:" + type + " operationType:" + operationType + " operationId:" + operationId + " script:" + script);

        String command = MessageFormat.format(javaScriptEventTemplate, "nfc-script-success", Util.tagToJSON2(uid, type, script, operationType, operationId, slot, null));
        Log.v(TAG, command);

        this.webView.sendJavascript(command);
        Log.d(TAG, "END fireNfcScriptSuccess");
    }
    private void fireNfcScriptError (byte[] uid, int type, String error) {  
        Log.d(TAG, "INI fireNfcScriptError " + uid + " " + type + " " + error);

        String command = MessageFormat.format(javaScriptEventTemplate, "nfc-script-error", Util.tagToJSON2(uid, type, null, 0, 0, 0, error));
        Log.v(TAG, command);

        this.webView.sendJavascript(command);
        Log.d(TAG, "END fireNfcScriptError");
    }
    private void fireTagEvent (Tag tag) {
        Log.d(TAG, "INI fireTagEvent " + tag);
        String command = MessageFormat.format(javaScriptEventTemplate, "tag", Util.tagToJSON(tag));
        Log.v(TAG, command);
        this.webView.sendJavascript(command);
        Log.d(TAG, "END fireTagEvent");
    }
}