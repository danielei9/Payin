package com.mobile.application.payin.common.utilities;

import com.google.gson.FieldNamingStrategy;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

import java.lang.reflect.Field;

public class CustomGson {
    private static final Gson customGson = CreateCustomGson();

    private static Gson CreateCustomGson(){
        GsonBuilder builder = new GsonBuilder();
        builder.setPrettyPrinting();
        builder.setFieldNamingStrategy(new FieldNamingStrategy() {
            @Override
            public String translateName(Field f) {
                String fieldName = f.getName();
                String firstChar = String.valueOf(fieldName.charAt(0));
                fieldName = firstChar.toLowerCase() + fieldName.substring(1, fieldName.length());
                return fieldName;
            }
        });
        return builder.create();
    }

    public static Gson getGson(){
        return customGson;
    }
}