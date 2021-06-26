package com.mobile.application.payin.common.serverconnections;

import android.app.ProgressDialog;
import android.content.Context;
import android.content.SharedPreferences;
import android.os.AsyncTask;

import com.android.application.payin.R;
import com.mobile.application.payin.common.interfaces.AsyncResponse;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.SocketTimeoutException;
import java.net.URL;
import java.net.UnknownHostException;
import java.nio.charset.Charset;
import java.util.HashMap;

public class ServerPut extends AsyncTask<String, Void, HashMap<String, String>> {

    private Context context;
    private ProgressDialog progreso;

    public AsyncResponse delegate = null;
    public boolean showProgress = true;
    private boolean cancelled = false;

    public ServerPut(Context context){
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
        HashMap<String, String> map = postData(params[0], params[1]);

        if (cancelled || map == null) return null;

        if (map.containsKey("error") && map.get("error").equals("401")){
            if (ServerRefresh.refreshToken(context))
                map = postData(params[0], params[1]);
        }

        if (map != null) map.put("type", "put");

        return map;
    }

    @Override
    protected void onPostExecute(HashMap<String, String> map) {
        if (delegate != null) delegate.onAsyncFinish(map);
        if (progreso != null && progreso.isShowing()) dismissProgress();
    }

    @Override
    protected void onCancelled(HashMap<String, String> map) {
        super.onCancelled(map);
    }

    public void dismissProgress(){
        if (showProgress) progreso.dismiss();
    }

    private HashMap<String, String> postData(String route, String info){
        String json = "", line;
        byte[] data;
        URL url;
        HashMap<String, String> map = new HashMap<>();
        BufferedReader rd;

        // Comprobamos las preferencias del modo debug
        SharedPreferences pref = context.getSharedPreferences(context.getResources().getString(R.string.prefs), Context.MODE_PRIVATE);

        Boolean debug = pref.getBoolean("debug", false);

        try {
            data = info.getBytes(Charset.forName("UTF-8"));

            if (debug) {
                if (pref.contains("IP"))
                    url = new URL(pref.getString("IP", "") + route);
                else
                    url = new URL(context.getResources().getString(R.string.url_debug) + route);
            } else url = new URL(context.getResources().getString(R.string.url) + route);

            String accessToken = context.getSharedPreferences(context.getResources().getString(R.string.prefs), Context.MODE_PRIVATE).getString("access_token", "");

            HttpURLConnection urlConnection = (HttpURLConnection) url.openConnection();
            urlConnection.setRequestMethod("PUT");
            urlConnection.setRequestProperty("Content-Type", "application/json; charset=utf-8");
            urlConnection.setRequestProperty("Content-Length", Integer.toString(data.length));
            urlConnection.setRequestProperty("Authorization", "Bearer " + accessToken);
            urlConnection.setConnectTimeout(25000);
            urlConnection.setDoInput(true);
            urlConnection.setDoOutput(true);
            urlConnection.setInstanceFollowRedirects(false);
            urlConnection.setUseCaches(false);

            urlConnection.getOutputStream().write(data);

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
            } else if (code == 204){
                map.put("success", "ok");
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
        } catch (Exception e) {
            System.err.println(e.getMessage());
            map.put("error", "401");
        }

        map.put("route", route);

        return map;
    }

    @Override
    protected void onCancelled() {
        cancelled = true;
        super.onCancelled();
    }
}
