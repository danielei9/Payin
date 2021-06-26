package com.mobile.application.payin.common.serverconnections;

import android.app.ProgressDialog;
import android.content.Context;
import android.content.SharedPreferences;
import android.os.AsyncTask;

import com.android.application.payin.R;
import com.mobile.application.payin.common.interfaces.AsyncResponse;

import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.nio.charset.Charset;
import java.util.HashMap;
import java.util.Iterator;

public class ServerAuth extends AsyncTask<String, Void, HashMap<String, String>> {
    private final Context context;
    private ProgressDialog progreso;

    public AsyncResponse delegate = null;

    private boolean cancelled = false;

    public ServerAuth(Context context){
        this.context = context;
    }

    @Override
    protected void onPreExecute() {
        progreso = new ProgressDialog(context);
        progreso.setProgressStyle(ProgressDialog.STYLE_SPINNER);
        progreso.setTitle("Autenticando");
        progreso.setMessage("Iniciando sesión...");
        progreso.setIndeterminate(true);
        progreso.setCanceledOnTouchOutside(false);
        progreso.setCancelable(false);
        progreso.show();
    }

    @Override
    protected HashMap<String, String> doInBackground(String... params) {
        String json = "", line;
        byte[] data;
        URL url;
        HashMap<String, String> map = new HashMap<>();
        BufferedReader rd;

        String secret = context.getResources().getString(R.string.secret);
        String body = "grant_type=password&username=" + params[0] + "&password=" + params[1] + "&client_id=PayInAndroidNativeApp&client_secret=" + secret;

        data = body.getBytes(Charset.forName("UTF-8"));

        // Comprobamos las preferencias del modo debug
        SharedPreferences pref = context.getSharedPreferences(context.getResources().getString(R.string.prefs), Context.MODE_PRIVATE);

        Boolean debug = pref.getBoolean("debug", false);

        try {
            // Con el modo debug activado usamos la url de debug
            if (debug) {
                if (pref.contains("IP"))
                    url = new URL(pref.getString("IP", "") + "token");
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

            urlConnection.getOutputStream().write( data );

            urlConnection.connect();

            int code = urlConnection.getResponseCode();

            if (cancelled) return null;

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
                map.put("success", "ok");
            } else {
                rd = new BufferedReader(new InputStreamReader(urlConnection.getErrorStream()));

                while ((line = rd.readLine()) != null) {
                    json += line;
                }

                map.put("error", "");
            }
        } catch (Exception e) {
            System.err.println(e.getMessage());
            map.put("error", "");
        }

        return map;
    }

    @Override
    protected void onPostExecute(HashMap<String, String> map) {
        progreso.dismiss();
        if (delegate != null) delegate.onAsyncFinish(map);
    }

    @Override
    protected void onCancelled() {
        cancelled = true;
        super.onCancelled();
    }

    public void dismissProgress(){
        progreso.dismiss();
    }
}
