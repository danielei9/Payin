package com.mobile.application.payin.common.utilities;

public class test {
    public static void main(String[] args) {
        String json = "{\"name\":\"Variable\",\"value\":\"2\"}";

        Clase c = CustomGson.getGson().fromJson(json, Clase.class);

        System.out.println(c.Name);
    }

    public static class Clase {
        String Name;

        public Clase() {
        }
    }
}
