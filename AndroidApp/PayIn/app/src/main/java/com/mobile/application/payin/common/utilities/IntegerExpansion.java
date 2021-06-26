package com.mobile.application.payin.common.utilities;

public class IntegerExpansion {
    /*
    public static int getIntegerValue(Integer I){
        try {
            return I;
        } catch (NullPointerException e){
            return -1;
        }
    }

    public static int getIntegerValue(Integer I, int val){
        try {
            return I;
        } catch (NullPointerException e){
            return val;
        }
    }
    */
    public static int getIntValueOf(String s){
        try {
            return Integer.valueOf(s);
        } catch (NumberFormatException e){
            return -1;
        }
    }
    /*
    public static int getIntValueOf(String s, int val){
        try {
            return Integer.valueOf(s);
        } catch (NumberFormatException e){
            return val;
        }
    }
    */
    public static Integer getIntegerValueOf(String s){
        try {
            return Integer.valueOf(s);
        } catch (NumberFormatException e){
            return null;
        }
    }
}
