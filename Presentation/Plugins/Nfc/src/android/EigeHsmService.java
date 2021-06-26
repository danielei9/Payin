package com.payin.nfc;

import android.os.Environment;
import android.nfc.NdefMessage;
import android.nfc.NdefRecord;
import android.nfc.Tag;
import android.nfc.tech.Ndef;
import android.security.KeyPairGeneratorSpec;
import android.security.keystore.KeyProperties;
import android.security.keystore.KeyProtection.Builder;
import android.security.keystore.KeyGenParameterSpec;
import android.util.Log;
import android.content.res.AssetManager;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import javax.security.auth.x500.X500Principal;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.Calendar; 
import java.util.Map;
import java.util.Enumeration;
import java.util.Date;

import java.math.BigInteger;
import java.security.*;
import java.security.spec.*;
import java.security.KeyStore;
import java.security.KeyStore.*;
import java.security.KeyFactory;
import java.security.Security;
import java.security.interfaces.RSAPrivateKey;
import java.security.interfaces.RSAPublicKey;
import java.security.spec.RSAPrivateKeySpec;
import java.security.spec.RSAPublicKeySpec;
import java.security.spec.PKCS8EncodedKeySpec;
import java.security.spec.X509EncodedKeySpec;
import java.security.cert.X509Certificate;
import java.security.cert.Certificate;
import java.security.cert.CertificateFactory;
import java.security.cert.CertificateException;
import java.security.KeyPairGenerator;
import javax.crypto.Cipher;
import javax.crypto.spec.SecretKeySpec;
import java.io.ByteArrayInputStream;
import java.io.InputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.lang.String;

public class EigeHsmService {
    static final String TAG = "EigeHsmService";
    static Cipher cipher;
    /*static final String alias = "payin";
    static KeyStore keyStore;
    static KeyPairGenerator kpg;
    static KeyPair kp;*/
    static PrivateKey privateKey;
    static PublicKey publicKey;

    // public static final String PUBLIC_KEY_FILE="localhost/assets/www/key/public.key";
    // public static final String PRIVATE_KEY_FILE="cdvfile://localhost/assets/www/key/private.key";

    // XAVI
    private String password = "P2014!";
    private String mobileKeyAlias = "mobile01";
    private String keyStorePath = "www/key/keystore.bks";
    private String keyStoreType = 
        //"RSA"
        //"pkcs12"
        //"AndroidKeyStore"
        KeyStore.getDefaultType()
        //"JKS"
        //JCEKS
        ;
    
    private KeyStore keyStore;
    
    public void load(AssetManager _assetManager)
        throws KeyStoreException, IOException, NoSuchAlgorithmException, CertificateException {
        keyStore = KeyStore.getInstance(keyStoreType);
        
        InputStream keyStream = _assetManager.open(keyStorePath);
        
        keyStore.load(keyStream, password.toCharArray());
        Log.d(TAG, "HSM loaded");
    }
    public void listKeys() throws KeyStoreException {
        Enumeration<String> aliases = keyStore.aliases();
        while(aliases.hasMoreElements()){
            Log.d(TAG, "HSM alias:" + aliases.nextElement());
        }
    }
	static byte[] encryptRSA(byte[] message) {
        byte[] res = null;
        try{
            /*if(kpg == null)
            {
                kpg = KeyPairGenerator.getInstance("RSA");
                kpg.initialize(1024);
                kp = kpg.genKeyPair();
                publicKey = kp.getPublic();
                privateKey = kp.getPrivate();
            }*/
            if(publicKey == null)
            {
                // Encrypt the string using the public key
                /*Log.e(TAG, "Public Key null");
                KeyFactory keyFactory = KeyFactory.getInstance("RSA");                
                byte[] bytePublicKey = Util.hexStringToByteArray(PUBLIC_KEY_FILE);
                EncodedKeySpec publicKeySpec = new X509EncodedKeySpec(bytePublicKey);
                publicKey = keyFactory.generatePublic(publicKeySpec);*/
                //Log.e(TAG, PUBLIC_KEY_FILE);
                //ObjectInputStream inputStream = new ObjectInputStream(new FileInputStream(PUBLIC_KEY_FILE));
                //publicKey = (PublicKey) inputStream.readObject();
            }
            
            if(cipher==null)
                cipher = Cipher.getInstance("RSA");
            //cipher.init(Cipher.ENCRYPT_MODE, (PublicKey)EigeHsmService.getPublicKey());
            //publicKey = NfcPlugin.keyPair.getPublic();
            cipher.init(Cipher.ENCRYPT_MODE, publicKey);
            res = cipher.doFinal(message);
            Log.e(TAG, "EN "+Util.bytesToHex(res));
            //String mensaje_original = new String(res); 
        }
        catch (Exception e) {
            Log.e("HSMSERVICE","ERRORRRRRRR encrypt");
        }
        return res;
    }
    public byte[] decryptRSA(byte[] message) {
        try {
            listKeys();
            
            Key key = getPrivateKey();
Log.d(TAG, "decryptRSA() key:" + key);
        
Log.v(TAG, "decryptRSA() message: " + message[0] + "," + message[1] + "," + message[2] + "," + message[3] + "," + message[4] + "," + message[5] + "...");

            Cipher cipher = Cipher.getInstance("RSA/NONE/OAEPWithSHA-1AndMGF1Padding");
            cipher.init(
                Cipher.DECRYPT_MODE,
                key
            );
            
            int blockSize = 256;
            byte[] result = new byte[4096];
            
            byte[] rest = message;
Log.d(TAG, "decryptRSA() message:" + message.length);
            byte[] block;
            int size = 0;
            while (rest.length > 0) {
                block = Arrays.copyOfRange(
                    rest, 
                    0, 
                    blockSize > rest.length ? rest.length : blockSize
                );
                rest = Arrays.copyOfRange(
                    rest, 
                    blockSize > rest.length ? rest.length : blockSize, 
                    rest.length
                );
Log.d(TAG, "decryptRSA() message:" + block.length + "+" + rest.length + "(" + size + ")");
                
                byte[] temp = cipher.doFinal(block);
Log.d(TAG, "decryptRSA() temp:" + temp.length);
                
                System.arraycopy(temp, 0, result, size, temp.length);
                
                size += temp.length;
            }
        
Log.v(TAG, "decryptRSA() result (" + size + "): " + result[0] + "," + result[1] + "," + result[2] + "," + result[3] + "," + result[4] + "," + result[5] + "...");
        
        
            // if(cipher==null)
            //     cipher = Cipher.getInstance("RSA");
            // //cipher.init(Cipher.DECRYPT_MODE, (PrivateKey)EigeHsmService.getPrivateKey());
            // //privateKey = NfcPlugin.keyPair.getPrivate();
            // cipher.init(Cipher.DECRYPT_MODE,privateKey);
            // res = cipher.doFinal(message);
            // Log.e(TAG, "DEC "+Util.bytesToHex(res));
            //String mensaje_original = new String(res); 
        
            return result;
        } catch (Exception e) {
            Log.e("HSMSERVICE","ERRORRRRRRR decryptRSA " + e.getMessage());
            e.printStackTrace();
        }
        
        return null;
    }

    /*public static boolean existsAlias()
    {
        try{
            
            if(keyStore!=null)
                keyStore = KeyStore.getInstance("AndroidKeyStore");
            keyStore.load(null);
        
            return keyStore.containsAlias(alias);
        }
        catch(Exception ex)
        {
            return false;
        }
    }*/
    
    /*public static RSAPublicKey getPublicKey()
    {
        // existsAlias();
        try{
            if(keyStore!=null)
                keyStore = KeyStore.getInstance("AndroidKeyStore");
            keyStore.load(null);
            KeyStore.PrivateKeyEntry privateKeyEntry = (KeyStore.PrivateKeyEntry)keyStore.getEntry(alias, null);
            RSAPrivateKey privateKey = (RSAPrivateKey) privateKeyEntry.getPrivateKey();
            RSAPublicKey publicKey = (RSAPublicKey) privateKeyEntry.getCertificate().getPublicKey();
            return publicKey;
        }catch(Exception ex)
        {
            return null;
        }
    }*/
    
    public Key getPrivateKey() throws KeyStoreException, NoSuchAlgorithmException, UnrecoverableKeyException, UnrecoverableEntryException
    {
        KeyStore.ProtectionParameter protParam = new KeyStore.PasswordProtection(password.toCharArray());
        Key privateKey = keyStore.getKey(mobileKeyAlias, password.toCharArray());

        return privateKey;
    }
}