package com.mobile.application.payin.common.utilities;

public class DoubleExpansion {
    public static Double getDoubleValueOf(String s){
        try {
            return Double.valueOf(s);
        } catch (NumberFormatException e){
            return null;
        }
    }
}
