package com.mobile.application.payin.dto.arguments;

import android.content.Context;
import android.net.wifi.WifiManager;
import android.os.Build;
import android.telephony.TelephonyManager;

public class MobileConfigurationArguments {
    public String DeviceManufacturer = "";
    public String DeviceModel        = "";
    public String DeviceName         = "";
    public String DeviceSerial       = "";
    public String DeviceId           = "";
    public String DeviceOperator     = "";
    public String DeviceImei         = "";
    public String DeviceMac          = "";
    public String OperatorSim        = "";
    public String OperatorMobile     = "";

    public MobileConfigurationArguments(Context context) {
        TelephonyManager tm = (TelephonyManager) context.getSystemService(Context.TELEPHONY_SERVICE);

        String mac = ((WifiManager) context.getSystemService(Context.WIFI_SERVICE)).getConnectionInfo().getMacAddress();

        DeviceManufacturer = Build.MANUFACTURER;
        DeviceModel        = Build.MODEL;
        DeviceName         = Build.DISPLAY;
        DeviceSerial       = Build.SERIAL;
        DeviceId           = Build.ID;
        DeviceOperator     = tm.getNetworkOperatorName();
        try {
            DeviceImei = tm.getDeviceId();
        } catch (SecurityException e) { DeviceImei = ""; }
        DeviceMac          = mac == null ? "" : mac;
        OperatorSim        = tm.getSimOperatorName();
        OperatorMobile     = tm.getNetworkOperatorName();
    }
}
