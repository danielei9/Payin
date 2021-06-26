package com.mobile.application.payin.common.serverconnections;

import android.app.ProgressDialog;
import android.content.Context;
import android.content.SharedPreferences;
import android.os.AsyncTask;

import com.android.application.payin.R;
import com.mobile.application.payin.common.interfaces.AsyncResponse;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.SocketTimeoutException;
import java.net.URL;
import java.net.UnknownHostException;
import java.util.HashMap;

public class ServerGet extends AsyncTask<String, Void, HashMap<String, String>> {

    private Context context;
    private ProgressDialog progreso = null;

    public AsyncResponse delegate = null;
    public boolean showProgress = true;
    private boolean cancelled = false;

    public ServerGet(Context context){
        this.context = context;
    }

    @Override
    protected void onPreExecute() {
        if (showProgress) {
            progreso = new ProgressDialog(context);
            progreso.setProgressStyle(ProgressDialog.STYLE_SPINNER);
            progreso.setTitle("Descargando datos");
            progreso.setMessage("Descargando...");
            progreso.setIndeterminate(true);
            progreso.setCanceledOnTouchOutside(false);
            progreso.setCancelable(false);
            progreso.show();
        }
    }

    @Override
    protected HashMap<String, String> doInBackground(String... params) {
        HashMap<String, String> map = getData(params[0]);

        if (cancelled || map == null) return null;

        if (map.containsKey("error") && map.get("error").equals("401")){
            if (ServerRefresh.refreshToken(context))
                map = getData(params[0]);
        }

        if (map != null) map.put("type", "get");

        return map;
    }

    @Override
    protected void onPostExecute(HashMap<String, String> map) {
        if (progreso != null && progreso.isShowing()) dismissProgress();
        if (delegate != null) delegate.onAsyncFinish(map);
    }

    @Override
    protected void onCancelled(HashMap<String, String> map) {
        super.onCancelled(map);
    }

    public void dismissProgress(){
        if (showProgress) {
            progreso.dismiss();
            showProgress = false;
        }
    }

    private HashMap<String, String> getData(String route) {
        String json = "", line;
        URL url;
        HashMap<String, String> map = new HashMap<>();
        BufferedReader rd;

        // Comprobamos las preferencias del modo debug
        SharedPreferences pref = context.getSharedPreferences(context.getResources().getString(R.string.prefs), Context.MODE_PRIVATE);

        Boolean debug = pref.getBoolean("debug", false);

        try {
            if (debug) {
                if (pref.contains("IP"))
                    url = new URL(pref.getString("IP", "") + route);
                else
                    url = new URL(context.getResources().getString(R.string.url_debug) + route);
            } else url = new URL(context.getResources().getString(R.string.url) + route);

            String accessToken = context.getSharedPreferences(context.getResources().getString(R.string.prefs), Context.MODE_PRIVATE).getString("access_token", "");

            HttpURLConnection urlConnection = (HttpURLConnection) url.openConnection();
            urlConnection.setRequestProperty("Content-Type", "application/json; charset=utf-8");
            urlConnection.setRequestProperty("Authorization", "Bearer " + accessToken);
            urlConnection.setRequestMethod("GET");
            urlConnection.setConnectTimeout(25000);
            urlConnection.setDoOutput(false);
            urlConnection.connect();

            int code = urlConnection.getResponseCode();

            if (cancelled) return null;

            if (code == 200) {
                rd = new BufferedReader(new InputStreamReader(urlConnection.getInputStream()));

                while ((line = rd.readLine()) != null) {
                    json += line;
                }

                map.put("json", json);
                map.put("route", route);
            } else {
                rd = new BufferedReader(new InputStreamReader(urlConnection.getErrorStream()));

                while ((line = rd.readLine()) != null) {
                    json += line;
                }

                map.put("json", json);
                map.put("error", String.valueOf(code));
            }
        } catch (SocketTimeoutException e2) {
            map.put("error", "500");
        } catch (UnknownHostException e1) {
            map.put("error", "host");
        } catch (IOException e) {
            e.printStackTrace();
            map.put("error", "401");
        }

        return map;
    }

    @Override
    protected void onCancelled() {
        cancelled = true;
        super.onCancelled();
    }
}
