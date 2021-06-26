package com.mobile.application.payin.services;

import android.app.IntentService;
import android.content.Intent;
import android.content.SharedPreferences;

import com.android.application.payin.R;
import com.google.android.gms.gcm.GoogleCloudMessaging;
import com.google.android.gms.iid.InstanceID;
import com.mobile.application.payin.common.interfaces.AsyncResponse;
import com.mobile.application.payin.common.utilities.CustomGson;
import com.mobile.application.payin.dto.arguments.DeviceMobileCreateArguments;

import java.net.HttpURLConnection;
import java.net.URL;
import java.nio.charset.Charset;
import java.util.HashMap;

public class PushRegisterService extends IntentService implements AsyncResponse {

    private static final String TAG = "NotificationRegisterService";

    public PushRegisterService() {
        super(TAG);
    }

    @Override
    protected void onHandleIntent(Intent intent) {
        SharedPreferences pref = getSharedPreferences(getResources().getString(R.string.prefs), MODE_PRIVATE);
        byte[] data; URL url;

        try {
            synchronized (TAG) {
                InstanceID instanceID = InstanceID.getInstance(this);
                String token = instanceID.getToken(getString(R.string.gcm_defaultSenderId), GoogleCloudMessaging.INSTANCE_ID_SCOPE, null);

                if (token.equals(pref.getString("pushToken", ""))) return ;

                DeviceMobileCreateArguments args = new DeviceMobileCreateArguments();
                args.Token = token;
                args.Type = DeviceMobileCreateArguments.DeviceType.ANDROID;

                Boolean debug = pref.getBoolean("debug", false);

                data = CustomGson.getGson().toJson(args, DeviceMobileCreateArguments.class).getBytes(Charset.forName("UTF-8"));

                if (debug) {
                    if (pref.contains("IP"))
                        url = new URL(pref.getString("IP", "") + getResources().getString(R.string.apiDeviceCreate));
                    else
                        url = new URL(getResources().getString(R.string.url_debug) + getResources().getString(R.string.apiDeviceCreate));
                }
                else url = new URL(getResources().getString(R.string.url) + getResources().getString(R.string.apiDeviceCreate));

                String accessToken = pref.getString("access_token", "");

                HttpURLConnection urlConnection = (HttpURLConnection) url.openConnection();
                urlConnection.setRequestMethod("POST");
                urlConnection.setRequestProperty("Content-Type", "application/json; charset=utf-8");
                urlConnection.setRequestProperty("Content-Length", Integer.toString(data.length));
                urlConnection.setRequestProperty("Authorization", "Bearer " + accessToken);
                urlConnection.setDoInput(true);
                urlConnection.setDoOutput(true);
                urlConnection.setInstanceFollowRedirects(false);
                urlConnection.setUseCaches(false);

                urlConnection.getOutputStream().write(data);

                urlConnection.connect();

                int code = urlConnection.getResponseCode();

                if (code == 200) {
                    pref.edit().putString("pushToken", token).apply();
                } else if (code == 204){
                    pref.edit().putString("pushToken", token).apply();
                }
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    @Override
    public void onAsyncFinish(HashMap<String, String> map) {
        //HandleServerError.Handle(map, null, this);
    }
}
