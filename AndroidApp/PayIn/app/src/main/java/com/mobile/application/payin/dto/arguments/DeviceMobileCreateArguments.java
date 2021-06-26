package com.mobile.application.payin.dto.arguments;

public class DeviceMobileCreateArguments {
    public String Token;
    public int Type;

    public DeviceMobileCreateArguments() {
    }

    public static class DeviceType {
        public static final int ANDROID = 1;
    }
}
