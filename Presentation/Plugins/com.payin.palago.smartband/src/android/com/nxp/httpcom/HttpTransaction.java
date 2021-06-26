package com.nxp.httpcomm;

import android.content.ContentValues;
// import android.content.Context;
// import android.content.SharedPreferences;
// import android.preference.PreferenceManager;

import java.io.BufferedReader;
// import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.UnsupportedEncodingException;
import java.net.HttpURLConnection;
import java.net.URL;
import java.net.URLEncoder;
import java.util.Map;
// import java.util.List;

// import org.apache.http.HttpResponse;
// import org.apache.http.NameValuePair;
// import org.apache.http.client.HttpClient;
// import org.apache.http.client.methods.HttpGet;
// import org.apache.http.client.utils.URLEncodedUtils;
// import org.apache.http.impl.client.DefaultHttpClient;
// import org.apache.http.params.BasicHttpParams;
// import org.apache.http.params.HttpConnectionParams;
// import org.apache.http.params.HttpParams;

import android.util.Log;

public class HttpTransaction {

	private static final String TAG = "HttpTransaction";
// 	private StringBuffer console;
// 	private HttpClient client;

	public HttpTransaction(StringBuffer console/*, Context ctx*/) {
// 		this.console = console;
// 		SharedPreferences sharedPrefs = PreferenceManager.getDefaultSharedPreferences(ctx);
// 		int timeout;
// 		try{
// 		timeout = Integer.parseInt(sharedPrefs.getString("server_timeout", "60")) * 1000;
// 		} catch(NumberFormatException e){
// 			timeout = 60 * 1000;
// 		}
// 		Log.i(TAG, "Timeout Value: " + timeout);
// 		HttpParams httpParameters = new BasicHttpParams();
// 		// Set the timeout in milliseconds until a connection is established.
// 		// The default value is zero, that means the timeout is not used.
// 		int timeoutConnection = timeout;
// 		HttpConnectionParams.setConnectionTimeout(httpParameters, timeoutConnection);
// 		// Set the default socket timeout (SO_TIMEOUT)
// 		// in milliseconds which is the timeout for waiting for data.
// 		int timeoutSocket = timeout;
// 		HttpConnectionParams.setSoTimeout(httpParameters, timeoutSocket);

// 		client = new DefaultHttpClient(httpParameters);
	}

    private String getUrlString(String urlString, ContentValues params) {
        try {
            StringBuilder result = new StringBuilder();
            boolean first = true;
            if (params != null) {
                for (Map.Entry<String, Object> entry : params.valueSet()) {
                    if (first)
                        result.append("?");
                    else
                        result.append("&");
                    first = false;

                    result.append(URLEncoder.encode(entry.getKey(), "UTF-8")); // name
                    result.append("=");
                    result.append(URLEncoder.encode(entry.getValue().toString(), "UTF-8")); // value
                }
            }

            return urlString + result.toString();
        } catch (UnsupportedEncodingException ex) {
            return urlString;
        }
    }
	public String executeHttpGet(ContentValues params, String urlString) throws Exception {
// 		BufferedReader in = null;
		String page = null;
        
        // try to avoid ECONN Reset issue
        System.setProperty("http.keepAlive", "false");

        URL url = new URL(getUrlString(urlString, params));
        HttpURLConnection connection = (HttpURLConnection)url.openConnection();
        try {
            //connection.setRequestProperty("User-Agent", "");
            connection.setRequestMethod("GET");
            //connection.setDoInput(true);
            //connection.setDoOutput(true);
            connection.connect();

            InputStreamReader isr = new InputStreamReader(connection.getInputStream());
            try {
                BufferedReader rd = new BufferedReader(isr);
                try {
                    String line = "";
                    StringBuffer sb = new StringBuffer("");
                    String nl = System.getProperty("line.separator");
                    while ((line = rd.readLine()) != null) {
                        sb.append(line + nl);
                    }
                    page = sb.toString();
                } finally {
                    rd.close();
                }
            } finally {
                isr.close();
            }
        } finally {
            connection.disconnect();
        }
        
        Log.d(TAG, "\nResponse from http Server:\n" + page + "\n");

		return page;
	}
	
// 	public void safeClose() {
// 	    if(client != null && client.getConnectionManager() != null)
// 	    {
// 	        client.getConnectionManager().shutdown();
// 	    }
// 	}
}
