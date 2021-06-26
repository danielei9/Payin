package com.mobile.application.payin.dto.results;

import java.util.ArrayList;

public class NotificationGetAllResult {
    public ArrayList<Notification> Data;

    public NotificationGetAllResult() {
        Data = new ArrayList<>();
    }

    public static class Notification {
        public int     Id;
        public int     Type;
        public int     State;
        public String  Message;
        public String  Date;
        public int     ReferenceId;
        public String  ReferenceClass;
        public boolean IsRead;

        public Notification() {
        }
    }
}
