package com.payin.nfc;

import android.nfc.NdefMessage;
import android.nfc.NdefRecord;
import android.nfc.Tag;
import android.nfc.tech.Ndef;
import android.util.Log;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

import java.math.BigInteger;
import java.security.*;
import java.security.spec.*;
import java.security.KeyFactory;
import java.security.Security;
import java.security.interfaces.RSAPrivateKey;
import java.security.interfaces.RSAPublicKey;
import java.security.spec.RSAPrivateKeySpec;
import java.security.spec.RSAPublicKeySpec;
import javax.crypto.Cipher;
import javax.crypto.spec.SecretKeySpec;

public class Util {

    static final String TAG = "NfcPlugin";
    static Cipher cipher;
    static KeyPairGenerator kpg;
    static KeyPair kp;
    static PrivateKey privateKey;
    static PublicKey publicKey;
    //UID(4)+Payi-in(6)+UTC(6) DEBUG: 112233445566778899AABBCCDDEEFF
    static final String llaveSimetrica =   "00112233445566778899AABBCCDDEEFF";
    
    static byte[] encrypt(byte[] message) {
        byte[] res = null;
        SecretKeySpec key = new SecretKeySpec(llaveSimetrica.getBytes(), "AES");
        try{
            if(cipher==null)
                cipher = Cipher.getInstance("AES");
            cipher.init(Cipher.ENCRYPT_MODE, key);
            res = cipher.doFinal(message);
            //String mensaje_original = new String(res); 
        } catch (Exception e) {
            e.printStackTrace();
        }
        
        return res;
    }
    
    static byte[] decrypt(byte[] message){
        byte[] res = null;
        SecretKeySpec key = new SecretKeySpec(llaveSimetrica.getBytes(), "AES");
        try{
            if(cipher==null)
                cipher = Cipher.getInstance("AES");
            cipher.init(Cipher.DECRYPT_MODE, key);
            res = cipher.doFinal(message);
            //String mensaje_original = new String(res); 
        } catch (Exception e) {
            e.printStackTrace();
        }
        return res;
    }
    
    static byte[] encryptRSA(byte[] message)
    {
        byte[] res = null;
        try{
            if(kpg == null)
            {
                kpg = KeyPairGenerator.getInstance("RSA");
                kpg.initialize(1024);
                kp = kpg.genKeyPair();
                publicKey = kp.getPublic();
                privateKey = kp.getPrivate();
            }
            if(cipher==null)
                cipher = Cipher.getInstance("RSA");
            cipher.init(Cipher.ENCRYPT_MODE, publicKey);
            res = cipher.doFinal(message);
            //String mensaje_original = new String(res); 
        } catch (Exception e) {
            e.printStackTrace();
        }
        return res;
    }
    static byte[] decryptRSA(byte[] message)
    {
        byte[] res = null;
        try{
            if(kpg == null)
            {
                kpg = KeyPairGenerator.getInstance("RSA");
                kpg.initialize(1024);
                kp = kpg.genKeyPair();
                publicKey = kp.getPublic();
                privateKey = kp.getPrivate();
            }
            if(cipher==null)
                cipher = Cipher.getInstance("RSA");
            cipher.init(Cipher.DECRYPT_MODE, privateKey);
            res = cipher.doFinal(message);
            //String mensaje_original = new String(res); 
        } catch (Exception e) {
            e.printStackTrace();
        }
        return res;
    }
    
    static JSONObject ndefToJSON(Ndef ndef) {
        JSONObject json = new JSONObject();

        if (ndef != null) {
            try {

                Tag tag = ndef.getTag();
                // tag is going to be null for NDEF_FORMATABLE until NfcUtil.parseMessage is refactored
                if (tag != null) {
                    json.put("id", bytesToHex(tag.getId()));
                    json.put("techTypes", new JSONArray(Arrays.asList(tag.getTechList())));
                }

                json.put("type", translateType(ndef.getType()));
                json.put("maxSize", ndef.getMaxSize());
                json.put("isWritable", ndef.isWritable());
                json.put("ndefMessage", messageToJSON(ndef.getCachedNdefMessage()));
                // Workaround for bug in ICS (Android 4.0 and 4.0.1) where
                // mTag.getTagService(); of the Ndef object sometimes returns null
                // see http://issues.mroland.at/index.php?do=details&task_id=47
                try {
                  json.put("canMakeReadOnly", ndef.canMakeReadOnly());
                } catch (NullPointerException e) {
                  json.put("canMakeReadOnly", JSONObject.NULL);
                }
            } catch (JSONException e) {
                Log.e(TAG, "Failed to convert ndef into json: " + ndef.toString(), e);
            }
        }
        return json;
    }

    static JSONObject tagToJSON(Tag tag) {
        JSONObject json = new JSONObject();

        if (tag != null) {
            try {
                json.put("id", bytesToHex(tag.getId()));
                json.put("techTypes", new JSONArray(Arrays.asList(tag.getTechList())));
            } catch (JSONException e) {
                Log.e(TAG, "Failed to convert tag into json: " + tag.toString(), e);
            }
        }
        return json;
    }
    static JSONObject tagToJSON2(byte[] uid, int type, JSONArray script, int operationType, int operationId, int slot, String error) {
        JSONObject json = new JSONObject();
        try {
            json.put("uid", bytesToHex(uid));
            json.put("type", type);
            if (script != null)
                json.put("script", script);
            json.put("operationType", operationType);
            json.put("operationId", operationId);
            json.put("slot", slot);
            if (error != null)
                json.put("error", error);
        } catch (JSONException e) {
            Log.e(TAG, "Failed to convert tag into json.", e);
        }
            
        return json;
    }
    static JSONObject stringToJSON2(String read) {
        JSONObject json = new JSONObject();

        if (read != null) {
            try {
                json.put("script", read);
            } catch (JSONException e) {
                Log.e(TAG, "Failed to convert tag into json: " + read, e);
            }
        }
        return json;
    }
    
    static JSONObject stringToJSON(Tag tag,JSONArray script) {
        JSONObject json = new JSONObject();

        if (tag != null) {
            try {
                json.put("id", bytesToHex(tag.getId()));
                json.put("techTypes", new JSONArray(Arrays.asList(tag.getTechList())));
                json.put("script", script);
            } catch (JSONException e) {
                Log.e(TAG, "Failed to convert tag into json: " + tag.toString(), e);
            }
        }
        return json;
    }

    static String translateType(String type) {
        String translation;
        if (type.equals(Ndef.NFC_FORUM_TYPE_1)) {
            translation = "NFC Forum Type 1";
        } else if (type.equals(Ndef.NFC_FORUM_TYPE_2)) {
            translation = "NFC Forum Type 2";
        } else if (type.equals(Ndef.NFC_FORUM_TYPE_3)) {
            translation = "NFC Forum Type 3";
        } else if (type.equals(Ndef.NFC_FORUM_TYPE_4)) {
            translation = "NFC Forum Type 4";
        } else {
            translation = type;
        }
        return translation;
    }

    static NdefRecord[] jsonToNdefRecords(String ndefMessageAsJSON) throws JSONException {
        JSONArray jsonRecords = new JSONArray(ndefMessageAsJSON);
        NdefRecord[] records = new NdefRecord[jsonRecords.length()];
        for (int i = 0; i < jsonRecords.length(); i++) {
            JSONObject record = jsonRecords.getJSONObject(i);
            byte tnf = (byte) record.getInt("tnf");
            byte[] type = jsonToByteArray(record.getJSONArray("type"));
            byte[] id = jsonToByteArray(record.getJSONArray("id"));
            byte[] payload = jsonToByteArray(record.getJSONArray("payload"));
            records[i] = new NdefRecord(tnf, type, id, payload);
        }
        return records;
    }

    static JSONArray byteArrayToJSON(byte[] bytes) {
        JSONArray json = new JSONArray();
        for (byte aByte : bytes) {
            json.put(aByte);
        }
        return json;
    }
    final protected static char[] hexArray = "0123456789ABCDEF".toCharArray();
    public static byte[] reverse(byte[] input) { 
         // handling null, empty and one element array 
         byte[] auxByte = input;
         if(auxByte == null || auxByte.length <= 1){ 
            return null; 
         } 
         for (int i = 0; i < auxByte.length / 2; i++) { 
            byte temp = auxByte[i];
            // swap numbers 
            auxByte[i] = auxByte[auxByte.length - 1 - i];
            auxByte[auxByte.length - 1 - i] = temp; 
         } 
         return auxByte;
    }

    public static String bytesToHex(byte[] bytes) {
        if(bytes!=null){
            char[] hexChars = new char[bytes.length * 2];
            for ( int j = 0; j < bytes.length; j++ ) {
                int v = bytes[j] & 0xFF;
                hexChars[j * 2] = hexArray[v >>> 4];
                hexChars[j * 2 + 1] = hexArray[v & 0x0F];
            }
            return new String(hexChars);
        }
        return "";
    }
    public static long hexToDecimal(String hex)
    {
        return Long.parseLong(hex, 16);
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
    
    static byte[] jsonToByteArray(JSONArray json) throws JSONException {
        byte[] b = new byte[json.length()];
        for (int i = 0; i < json.length(); i++) {
            b[i] = (byte) json.getInt(i);
        }
        return b;
    }

    static JSONArray messageToJSON(NdefMessage message) {
        if (message == null) {
            return null;
        }

        List<JSONObject> list = new ArrayList<JSONObject>();

        for (NdefRecord ndefRecord : message.getRecords()) {
            list.add(recordToJSON(ndefRecord));
        }

        return new JSONArray(list);
    }

    static JSONObject recordToJSON(NdefRecord record) {
        JSONObject json = new JSONObject();
        try {
            json.put("tnf", record.getTnf());
            json.put("type", byteArrayToJSON(record.getType()));
            json.put("id", byteArrayToJSON(record.getId()));
            json.put("payload", byteArrayToJSON(record.getPayload()));
        } catch (JSONException e) {
            //Not sure why this would happen, documentation is unclear.
            Log.e(TAG, "Failed to convert ndef record into json: " + record.toString(), e);
        }
        return json;
    }
    
    public static byte[] GetLittleEndian(byte[] that, int start, int end)
    {
        // B15 B14 B13 B12 B11 B10 B9 B8 B7 B6 B5 B4 B3 B2 B1 B0
        
        int startByte = start / 8;
        int endByte = end / 8;

        int rightCut = start % 8;

        int shiftMask = (- end + start - 1) % 8;
        if (shiftMask < 0)
            shiftMask += 8;

        int numberBytes = (end - start) / 8 + 1;
        byte[] result = new byte[numberBytes];
        for (int i = 0; i < numberBytes; i++)
        {
            int index = startByte + i;
            int temp = that[index];

            // Cortar por la derecha
            if (index < 15)
                temp |= that[index + 1] << 8;

            // Cortar por la derecha
            temp >>= rightCut;

            // Cortar mascara y cortar por la izquierda
            int mask = 0xFF;
            if (i == numberBytes - 1) // Ultima iteracion
                mask >>= shiftMask;
            temp &= mask;

            // Guardar
            result[i] = (byte)(temp);
        }

        return result;
    }
    
    public static byte[] SetLittleEndian(byte[] that, int start, int end, byte[] value)
    {
        // B15 B14 B13 B12 B11 B10 B9 B8 B7 B6 B5 B4 B3 B2 B1 B0

        int startByte = start / 8;

        int rightCut = start % 8;
        int leftCut = (8 - (end + 1) % 8) % 8;

        int numberBytes = (end - start) / 8 + 1;
        for (int i = 0; i < numberBytes; i++)
        {
            int index = startByte + i;
            byte tempValue = value[i];

            // Cortar mascara
            int mask = 0xFF;
            if (i == 0)
                mask = (byte)((mask >> rightCut) << rightCut);
            if (i == numberBytes - 1)
                mask = (byte)((mask << leftCut) >> leftCut);

            // Mover valor
            if (i == 0)
                tempValue = (byte)(tempValue << rightCut);
            else
            {
                byte temp = (byte)((tempValue << 8) | value[i - 1]);
                tempValue = (byte)(temp >> (8 - rightCut));
            }

            that[index] = (byte)(that[index] & ~mask); // Limpiar
            that[index] = (byte)(that[index] | tempValue); // Set Value
        }
        
        return that;
    }
    
    public static int ByteToInt(byte[] b)
    {           
        int value= 0;
        for(int i=0; i<b.length; i++)
            value = (value << 8) | b[i];     
        return value;       
    }

}
