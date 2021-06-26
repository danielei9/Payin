package com.mobile.application.payin.common.serverconnections;

import android.annotation.SuppressLint;
import android.content.Context;
import android.content.SharedPreferences;
import android.util.Log;

import com.android.application.payin.R;

import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.nio.charset.Charset;
import java.util.HashMap;
import java.util.Iterator;

public class ServerRefresh {

    @SuppressLint("CommitPrefEdits")
    public static boolean refreshToken(Context context) {
        String json = "", line;
        byte[] data;
        URL url;
        HashMap<String, String> map = new HashMap<>();
        BufferedReader rd;

        SharedPreferences pref = context.getSharedPreferences(context.getResources().getString(R.string.prefs), Context.MODE_PRIVATE);

        String secret = context.getResources().getString(R.string.secret);
        String body = "grant_type=refresh_token&refresh_token=" + pref.getString("refresh_token", "") + "&client_id=PayInAndroidNativeApp&client_secret=" + secret;

        data = body.getBytes(Charset.forName("UTF-8"));

        Boolean debug = pref.getBoolean("debug", false);

        try {
            if (debug) {
                if (pref.contains("IP"))
                    url = new URL(pref.getString("IP", "") +"token");
                else
                    url = new URL(context.getResources().getString(R.string.url_debug) + "token");
            } else url = new URL(context.getResources().getString(R.string.url) + "token");

            HttpURLConnection urlConnection = (HttpURLConnection) url.openConnection();
            urlConnection.setRequestMethod("POST");
            urlConnection.setRequestProperty("Content-Type", "x-www-form-urlencoded");
            urlConnection.setRequestProperty("charset", "utf-8");
            urlConnection.setRequestProperty("Content-Length", Integer.toString(data.length));
            urlConnection.setConnectTimeout(25000);
            urlConnection.setDoInput(true);
            urlConnection.setDoOutput(true);
            urlConnection.setInstanceFollowRedirects(false);
            urlConnection.setUseCaches(false);

            urlConnection.getOutputStream().write(data);

            urlConnection.connect();

            int code = urlConnection.getResponseCode();

            if (code == 200) {
                rd = new BufferedReader(new InputStreamReader(urlConnection.getInputStream()));

                while ((line = rd.readLine()) != null) {
                    json += line;
                }

                JSONObject jsonObj = new JSONObject(json);

                Iterator<String> keys = jsonObj.keys();
                while (keys.hasNext()){
                    String key = keys.next();
                    String value = jsonObj.getString(key);

                    map.put(key, value);
                }

                SharedPreferences.Editor editor = pref.edit();

                editor.putString("access_token", map.get("access_token"));
                editor.putString("refresh_token", map.get("refresh_token"));

                String roles[] = map.get("as:roles").split(",");

                editor.remove("role_user");
                editor.remove("role_operator");
                editor.remove("role_superadmin");
                editor.remove("role_admin");
                editor.remove("role_commerce");
                editor.remove("role_commercePayment");
                editor.remove("role_tester");

                for (String role : roles){
                    switch (role.trim().toLowerCase()){
                        case "user":
                            editor.putBoolean("role_user", true);
                            break;
                        case "operator":
                            editor.putBoolean("role_operator", true);
                            break;
                        case "superadministrator":
                            editor.putBoolean("role_superadmin", true);
                            break;
                        case "administrator":
                            editor.putBoolean("role_admin", true);
                            break;
                        case "commerce":
                            editor.putBoolean("role_commerce", true);
                            break;
                        case "commercePayment":
                            editor.putBoolean("role_commercePayment", true);
                            break;
                        case "tester":
                            editor.putBoolean("role_tester", true);
                            break;
                    }
                }

                editor.commit();

                return true;
            } else {
                rd = new BufferedReader(new InputStreamReader(urlConnection.getErrorStream()));

                while ((line = rd.readLine()) != null) {
                    json += line;
                }

                Log.e("ServerRefresh", json);
            }
        } catch (Exception e) {
            System.err.println(e.getMessage());
        }

        return false;
    }
}
