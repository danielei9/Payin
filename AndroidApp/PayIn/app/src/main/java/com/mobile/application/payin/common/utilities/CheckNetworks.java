package com.mobile.application.payin.common.utilities;

import android.content.Context;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;

public class CheckNetworks {
    /*
    public static boolean checkMobileDataFlexible(Context context){
        ConnectivityManager connManager = (ConnectivityManager) context.getSystemService(Context.CONNECTIVITY_SERVICE);
        NetworkInfo data = connManager.getNetworkInfo(ConnectivityManager.TYPE_MOBILE);

        return  data.isConnectedOrConnecting();
    }

    public static boolean checkWifiFlexible(Context context){
        ConnectivityManager connManager = (ConnectivityManager) context.getSystemService(Context.CONNECTIVITY_SERVICE);
        NetworkInfo mWifi = connManager.getNetworkInfo(ConnectivityManager.TYPE_WIFI);

        return mWifi.isConnectedOrConnecting();
    }

    public static boolean checkInternetFlexible(Context context){
        ConnectivityManager connManager = (ConnectivityManager) context.getSystemService(Context.CONNECTIVITY_SERVICE);
        NetworkInfo mWifi = connManager.getNetworkInfo(ConnectivityManager.TYPE_WIFI);
        NetworkInfo data = connManager.getNetworkInfo(ConnectivityManager.TYPE_MOBILE);

        return mWifi.isConnectedOrConnecting() || data.isConnectedOrConnecting();
    }

    public static boolean checkMobileDataStrict(Context context){
        ConnectivityManager connManager = (ConnectivityManager) context.getSystemService(Context.CONNECTIVITY_SERVICE);
        NetworkInfo data = connManager.getNetworkInfo(ConnectivityManager.TYPE_MOBILE);

        return  data.isConnected();
    }

    public static boolean checkWifiStrict(Context context){
        ConnectivityManager connManager = (ConnectivityManager) context.getSystemService(Context.CONNECTIVITY_SERVICE);
        NetworkInfo mWifi = connManager.getNetworkInfo(ConnectivityManager.TYPE_WIFI);

        return mWifi.isConnected();
    }
    */
    public static boolean checkInternetStrict(Context context){
        ConnectivityManager connManager = (ConnectivityManager) context.getSystemService(Context.CONNECTIVITY_SERVICE);
        NetworkInfo mWifi = connManager.getNetworkInfo(ConnectivityManager.TYPE_WIFI);
        NetworkInfo data = connManager.getNetworkInfo(ConnectivityManager.TYPE_MOBILE);

        return mWifi.isConnected() || data.isConnected();
    }
}
