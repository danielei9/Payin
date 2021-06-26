package com.mobile.application.payin.dto.results;

import java.util.ArrayList;

public class ControlPresenceMobileGetTimetableResult {
    public ArrayList<Item> Data;

    public ControlPresenceMobileGetTimetableResult() {
    }

    public static class Item{
        public int    Id;
        public int    Type;
        public String Title;
        public String Location;
        public String Info;
        public int    ItemId;
        public String Start;
        public String End;
        public String Duration;

        public Item() {
        }
    }
}
