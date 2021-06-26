package com.mobile.application.payin.dto.results;

import java.io.Serializable;
import java.util.ArrayList;

public class ServiceTagGetResult implements Serializable {
    public ArrayList<Tag> Data;

    public ServiceTagGetResult() {}

    public static class Tag implements Serializable {
        public int Id;
        public ArrayList<Item> Items;

        public Tag() {
            Items = new ArrayList<>();
        }
    }

    public static class Item implements Serializable {
        public int Id;
        public Integer PresenceType;
        public String Name;
        public boolean SaveTrack;
        public int CheckPointId;
        public boolean SaveFacialRecognition;
        public ArrayList<Planning> Plannings;

        public Item() {
            Plannings = new ArrayList<>();
        }
    }

    public static class Planning implements Serializable {
        public int Id;
        public String Date;
        public int EllapsedMinutes;
        public ArrayList<Assign> Assigns;
        public Integer CheckId;
        public int PresenceType;
    }

    public static class Assign implements Serializable {
        public int Id;
        public String FormName;
        public String FormObservations;
        public ArrayList<Value> Values;

        public Assign() {
            Values = new ArrayList<>();
        }
    }

    public static class Value implements Serializable {
        public int Id;
        public String Name;
        public int Type;
        public int Target;
        public boolean IsRequired;
        public String ValueString;
        public Double ValueNumeric;
        public Boolean ValueBool;
        public String ValueDateTime;
    }
}