package com.mobile.application.payin.dto.arguments;

import java.io.Serializable;
import java.util.ArrayList;

public class ControlPresenceCheckArguments implements Serializable {
    public int						Id;
    public String					Date;
    public String					Image;
    public Double					Latitude;
    public Double					Longitude;
    public ArrayList<Item>          Items;

    public ControlPresenceCheckArguments() {
        Items = new ArrayList<>();
    }

    public static class Item implements Serializable {
        public int Id;
        public Integer CheckId;
        public Integer CheckPointId;
        public ArrayList<Assign> Assigns;

        public Item() {
            Assigns = new ArrayList<>();
        }
    }

    public static class Assign implements Serializable {
        public int              Id;
        public ArrayList<Value> Values;

        public Assign() {
            Values = new ArrayList<>();
        }
    }

    public static class Value implements Serializable {
        public int     Id;
        public String  ValueString;
        public Double  ValueNumeric;
        public Boolean ValueBool;
        public String  ValueDateTime;
        public String  ValueImage;

        public Value() {}
    }
}
