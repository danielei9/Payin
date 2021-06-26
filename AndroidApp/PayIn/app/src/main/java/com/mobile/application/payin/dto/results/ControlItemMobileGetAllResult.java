package com.mobile.application.payin.dto.results;

import java.io.Serializable;
import java.util.ArrayList;

public class ControlItemMobileGetAllResult implements Serializable {

    public ArrayList<Item> Data;

    public ControlItemMobileGetAllResult() {
        Data = new ArrayList<>();
    }

    public  static class Item implements Serializable {
        public int                 Id;
        public String              Name;
        public String              Observations;
        public Boolean             SaveTrack;
        public Boolean             SaveFacialRecognition;
        public Boolean             CheckTimetable;
        public int                 PresenceType;
        public ArrayList<Planning> Plannings;

        public Item() {
            Plannings = new ArrayList<>();
        }
    }

    public class Planning implements Serializable {
        public int               Id;
        public String            Date;
        public int               EllapsedMinutes;
        public ArrayList<Assign> Assigns;
        public Integer           CheckId;
        public Integer           CheckPointId;
        public int               PresenceType;

        public Planning() {
            Assigns = new ArrayList<>();
        }
    }

    public class Assign implements Serializable {
        public int              Id;
        public String           FormName;
        public String           FormObservations;
        public ArrayList<Value> Values;

        public Assign() {
            Values = new ArrayList<>();
        }
    }

    public class Value implements Serializable {
        public int     Id;
        public String  Name;
        public String  Observations;
        public int     Type;
        public int     Target;
        public boolean IsRequired;
        public String  ValueString;
        public Double  ValueNumeric;
        public Boolean ValueBool;
        public String  ValueDateTime;

        public Value() {}
    }
}
