package com.payin.nfc;
import android.net.*;
import java.net.*;
import android.os.Looper;
import android.os.Bundle;
import android.content.SharedPreferences;

import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import android.database.sqlite.SQLiteDatabase;
import android.app.Activity;
import java.io.FileOutputStream;
import android.content.res.AssetManager;

import android.nfc.Tag;
import android.nfc.TagLostException;
import android.nfc.tech.MifareClassic;
import android.database.Cursor;
import android.content.Context;


import android.util.Base64;
import android.util.Log;

import android.widget.Toast;

import java.math.BigInteger;

import java.io.IOException;

import java.security.cert.CertificateException;
import java.security.KeyStoreException;
import java.security.NoSuchAlgorithmException;

import java.util.Arrays;
import java.util.concurrent.ExecutorService;
import java.util.HashMap;

import org.apache.cordova.CallbackContext;


import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

public class NfcHandlerClassic {
    private static final String TAG = "NfcHandlerClassic";
    private static String errorText = "";
    private Activity activity;
    private static EigeHsmService hsmService;

    public NfcHandlerClassic(AssetManager _assetManager) {
        try {
            hsmService = new EigeHsmService();
            hsmService.load(_assetManager);
        }
        catch(Exception e) {
            Log.e("HSMSERVICE","ERROR decryptRSA " + e.getMessage());
            e.printStackTrace();
        }
    }

    public JSONArray executeScript(Activity _activity, ExecutorService threadPool, Tag tag, String keys, JSONArray script, JSONObject operation, int slot, CallbackContext callbackContext)
    {
        Log.v(TAG, "INI executeScript Script:" + script);
        activity = _activity;

        JSONArray result = new JSONArray();
        ////SQLITE
        PayinSQLiteClassic PaySQLite = new PayinSQLiteClassic(_activity, "DBPayin", null, 3);
        SQLiteDatabase db = PaySQLite.getWritableDatabase();

        ////SHAREPREFERENCES 
        //String FILENAME = "PayIN";
        //String PREFS_NAME = "PayInFile";
        //SharedPreferences settings = _activity.getSharedPreferences(PREFS_NAME,0);

        ////PREFERENCES
        //String FILENAME = "PayIN";
        //String PREFS_NAME = "PayInFile";
        //SharedPreferences settings = _activity.getPreferences(0);
        //SharedPreferences.Editor editor = settings.edit();

        try {
            MifareClassic mfc = MifareClassic.get(tag);

            if (mfc != null) {
                //byte[] data;
                //byte[] dataWrite;
                boolean checkTagLost = false;
                boolean checkIOError = false;
                boolean check = true;

                try {
                    //foreach Sector autenticar y leer bloque
                    mfc.connect();
                    if (mfc.isConnected()) {  
                        try {
                            // Get byte dictionary
                            byte[] keyEncriptedBytes = Base64.decode(keys, Base64.DEFAULT);
                            Log.v(TAG, "executeScript() keyEncriptedBytes: " + keyEncriptedBytes[0] + "," + keyEncriptedBytes[1] + "," + keyEncriptedBytes[2] + "," + keyEncriptedBytes[3] + "," + keyEncriptedBytes[4] + "," + keyEncriptedBytes[5] + "...");

                            byte[] keyDecriptedBytes = hsmService.decryptRSA(keyEncriptedBytes);
                            //byte[] keyDecriptedBytes = keyEncriptedBytes; // Descifrar claves

                            Log.v(TAG, "executeScript() keyDecriptedBytes: " + keyDecriptedBytes[0] + "," + keyDecriptedBytes[1] + "," + keyDecriptedBytes[2] + "," + keyDecriptedBytes[3] + "," + keyDecriptedBytes[4] + "," + keyDecriptedBytes[5] + "...");
                            String keyJson = new String(keyDecriptedBytes, "UTF8");
                            JSONArray keyArray = new JSONArray(keyJson);


                            HashMap<Integer, String> keysDictionary = new HashMap<Integer, String>();
                            for (int i = 0; i < keyArray.length(); i++) {
                                JSONObject keyObject = keyArray.getJSONObject(i);
                                String keyString = keyObject.getString("key");
                                int keySector = Integer.parseInt(keyString.substring(0, keyString.length() - 1));
                                int keyType = (keyString.substring(keyString.length() - 1, keyString.length()).equals("A")) ? 0 : 1;

                                keysDictionary.put(
                                    keySector * 2 + keyType,
                                    keyObject.getString("value")
                                );
                                Log.v(TAG, "executeScript() keylist(" + (keySector * 2 + keyType) + " " + keyString + "): " + keyObject.getString("value"));
                            }
                            long tiempoInicio = System.currentTimeMillis();



                            for (int i = 0; i< script.length(); i++) {
                                JSONObject jsonObject = script.getJSONObject(i);

                                //// SQLITE
                                //db.execSQL("UPDATE OPERATIONSTORAGE SET operation="+i);
                                db.execSQL("INSERT OR REPLACE INTO OPERATIONSTORAGE(operation) VALUES("+i+")"); 

                                //// SQLITE CONSULTA 
                                //Cursor resultSet = db.rawQuery("Select * from OPERATIONSTORAGE",null);
                                /*resultSet.moveToFirst();
                                user='', script='"+jsonObject.toString()+"',
                                String showScript= resultSet.getString(2);
                                int showOperation= resultSet.getInt(3);
                                Log.v(TAG," INDEX " + showOperation);*/
                                //// SHAREPREFERENCES / PREFERENCES (depen del que hi haja dalt)
                                //editor.putInt("index", i);
                                //editor.putString("script", jsonObject.toString());
                                //editor.commit();
                                //String restoredText = settings.getString("text", null);
                                //String name = settings.getString("script", "WTF?");//"No name defined" is the default value.
                                //Log.v(TAG, "SCRIPTS " + name );


                                int sectorOperation = jsonObject.getInt("operation");
                                Log.v(TAG, "executeScript() sectorOperation: " + sectorOperation);
                                int sector = jsonObject.getInt("sector");
                                Log.v(TAG, "executeScript() sector: " + sector);

                                switch(sectorOperation){
                                    case 1: //Autentication
                                        {
                                            //Operation, sector y KeyType
                                            try {
                                                int keyType = jsonObject.getInt("keyType");
                                                Log.v(TAG, "executeScript() keyType: " + keyType);

                                                String keyValue = keysDictionary.get(
                                                    sector * 2 + keyType
                                                );
                                                Log.v(TAG, "executeScript() keyValue: " + keyValue);
                                                byte[] key = hexStringToByteArray(keyValue);
                                                Log.v(TAG, "executeScript() key: " + key.toString());
                                                if (keyType == 0) {
                                                    if (!mfc.authenticateSectorWithKeyA(sector, key)) {
                                                        callbackContext.error("Authentication error: sector " + sector + "A");
                                                    } else {
                                                        Log.v(TAG, "executeScript() Authenticated " + sector + "A");
                                                        JSONObject resJsonObject = new JSONObject();
                                                        resJsonObject.put("sector", sector);
                                                        resJsonObject.put("keyType", keyType);
                                                        resJsonObject.put("operation", sectorOperation);
                                                        result.put(resJsonObject);
                                                    }
                                                } else { 
                                                    if (!mfc.authenticateSectorWithKeyB(sector, key)) {
                                                        callbackContext.error("Authentication error: sector " + sector +"B");
                                                    } else {
                                                        Log.v(TAG, "executeScript() Authenticated " + sector + "B");
                                                        JSONObject resJsonObject = new JSONObject();
                                                        resJsonObject.put("sector", sector);
                                                        resJsonObject.put("keyType", keyType);
                                                        resJsonObject.put("operation", sectorOperation);
                                                        result.put(resJsonObject);
                                                    }
                                                }


                                            } catch(TagLostException eTag){
                                                callbackContext.error("Fallo lectura Tarjeta Perdida: sector " + sector);
                                            } catch(IOException eIO){
                                                callbackContext.error("Fallo lectura Tarjeta IO: sector " + sector);
                                            }

                                            break;
                                        }
                                    case 2: //Read
                                        {
                                            int block = jsonObject.getInt("block");
                                            Log.v(TAG, "JSON OBJECT - READ " + jsonObject );
                                            try {
                                                byte[] data = mfc.readBlock(sector * 4 + block);
                                                Log.v(TAG, "executeScript() Read " + sector + "-" + block);

                                                if (jsonObject.has("from") && jsonObject.has("to"))
                                                {
                                                    int from = jsonObject.getInt("from");
                                                    int to = jsonObject.getInt("to");
                                                    data = Util.GetLittleEndian(data, from, to);
                                                }

                                                JSONObject resJsonObject = new JSONObject();
                                                resJsonObject.put("sector", sector);
                                                resJsonObject.put("block", block);
                                                resJsonObject.put("operation", sectorOperation);
                                                resJsonObject.put("data", bytesToHex(data));


                                                result.put(resJsonObject);
                                            } catch (TagLostException eTag){
                                                callbackContext.error("Fallo lectura Tarjeta Perdida: sector " + sector + " bloque " + block);
                                            } catch (IOException eIO){
                                                callbackContext.error("Fallo lectura Tarjeta IO: sector " + sector + " bloque " + block);
                                            }

                                            break;
                                        }
                                    case 3: //Write
                                        {
                                            //Operation, Sector y Block
                                            Log.v(TAG, "executeScript() writting: ");
                                            int block = jsonObject.getInt("block");
                                            Log.v(TAG, "JSON OBJECT - WRITE " + jsonObject );


                                            try {
                                                byte[] data = mfc.readBlock(sector * 4 + block);

                                                String dataBlock = jsonObject.get("data").toString();
                                                byte[] dataWrite = hexStringToByteArray(dataBlock);

                                                if(jsonObject.has("from") && jsonObject.has("to"))
                                                {
                                                    int from = jsonObject.getInt("from");
                                                    int to = jsonObject.getInt("to");
                                                    dataWrite = Util.SetLittleEndian(data, from, to, hexStringToByteArray(dataBlock));
                                                }

                                                mfc.writeBlock(sector * 4 + block, dataWrite);
                                                Log.v(TAG, "executeScript() Write " + sector + "-" + block);


                                                JSONObject resJsonObject = new JSONObject();
                                                resJsonObject.put("sector", sector);
                                                resJsonObject.put("block", block);
                                                resJsonObject.put("operation", sectorOperation);
                                                result.put(resJsonObject);
                                            } catch (TagLostException eTag) {
                                                callbackContext.error("Fallo lectura Tarjeta Perdida: sector " + sector + " bloque " + block);
                                            } catch (IOException eIO) {
                                                callbackContext.error("Fallo lectura Tarjeta IO: sector " + sector + " bloque " + block);
                                            }

                                            break;
                                        }
                                    case 4: //Check
                                        {
                                            //Operation, Sector y Block
                                            int block = jsonObject.getInt("block");
                                            Log.v(TAG, "JSON OBJECT - CHECK " + jsonObject );

                                            try {
                                                String dataBlock = jsonObject.get("data").toString();                                                

                                                byte[] data = mfc.readBlock(sector * 4 + block);
                                                byte[] dataWrite = hexStringToByteArray(dataBlock);

                                                String dataString = (Util.bytesToHex(data)).toString();
                                                String dataBlockString = dataBlock.toString();

                                                Log.e(TAG, "dataString "+ dataString);
                                                Log.e(TAG, "dataBlockString "+ dataBlockString);

                                                if(jsonObject.has("from") && jsonObject.has("to"))
                                                {
                                                    int from = jsonObject.getInt("from");
                                                    int to = jsonObject.getInt("to");
                                                    data = Util.GetLittleEndian(data, from, to);
                                                }
                                                if(!dataString.equals(dataBlockString)){
                                                    return null;
                                                }


                                                Log.v(TAG, "executeScript() Check " + data + " - " + dataWrite);

                                                JSONObject resJsonObject = new JSONObject();
                                                resJsonObject.put("sector", sector);
                                                resJsonObject.put("block", block);
                                                resJsonObject.put("operation", sectorOperation);
                                                result.put(resJsonObject);
                                            } catch(TagLostException eTag){
                                                checkTagLost=true;
                                                callbackContext.error("Fallo lectura Tarjeta Perdida: sector "+sector+" bloque "+block);
                                            } catch(IOException eIO){
                                                callbackContext.error("Fallo lectura Tarjeta IO: sector "+sector+" bloque "+block);
                                            }
                                            break;
                                        }
                                    case 5: //Suma X 
                                        int block = jsonObject.getInt("block");

                                        try {
                                            int value =0;
                                            byte[] data = mfc.readBlock(sector*4+block);
                                            value = jsonObject.getInt("data");
                                            if(jsonObject.has("from") && jsonObject.has("to"))
                                            {
                                                int from = jsonObject.getInt("from");
                                                int to = jsonObject.getInt("to");
                                                data = Util.GetLittleEndian(data, from, to);
                                                value += new BigInteger(data).intValue();
                                                byte[] dataWrite = Util.SetLittleEndian(data, from, to, BigInteger.valueOf(value).toByteArray());

                                                mfc.writeBlock(sector * 4 + block, dataWrite);
                                                Log.v(TAG, "executeScript() Add " + sector + "-" + block);
                                            }
                                        } catch(TagLostException eTag) {
                                            checkTagLost=true;
                                            callbackContext.error("Fallo lectura Tarjeta Perdida: sector "+sector+" bloque "+block);
                                        } catch(IOException eIO) {
                                            callbackContext.error("Fallo lectura Tarjeta IO: sector "+sector+" bloque "+block);
                                        }

                                        break;
                                }
                            }
                            long totalTiempo = System.currentTimeMillis() - tiempoInicio;
                            Log.v(TAG, "Tiempo total " + totalTiempo);
                        } catch (JSONException e) {
                            e.printStackTrace();
                            callbackContext.error("Error Operation Card");     
                        } finally {
                            mfc.close();
                            callbackContext.success("OK");
                        } 
                    }     
                } catch (IOException e) { 
                    Log.e(TAG, e.getLocalizedMessage());
                    callbackContext.error("Error connecting NFC"); 
                } finally {
                    script = null;
                }      
            }   
        } catch (Exception e){
            //  Log.e(TAG, "Error connecting Tag" + e.getLocalizedMessage()+ " )");
            callbackContext.error("Error connecting Tag"); 
        }

        Log.v(TAG, "END executeScript");
        return result;
    }

    public static String bytesToHex(byte[] bytes) {
        char[] hexArray = "0123456789ABCDEF".toCharArray();
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
}