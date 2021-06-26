package com.mobile.application.payin.dto.arguments;

import java.io.Serializable;
import java.util.ArrayList;

public class ControlIncidentCreateManualCheckArguments implements Serializable {
    public int          IncidentType;
    public String       Observations;
    public IncidentItem Item;

    public ControlIncidentCreateManualCheckArguments() { }

    public static class IncidentItem implements Serializable {
        public String            Date;
        public String            Image;
        public Double            Latitude;
        public Double            Longitude;
        public int               Id;
        public Integer           CheckPointId;
        public Integer           CheckId;
        public ArrayList<Assign> Assigns;

        public IncidentItem() {
            Assigns = new ArrayList<>();
        }
    }

    public static class Assign implements Serializable {
        public int Id;
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

        public Value() { }
    }
}
