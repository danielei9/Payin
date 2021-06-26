package com.mobile.application.payin.dto.results;

import java.io.Serializable;
import java.util.ArrayList;

public class AccountGetResult {
    public ArrayList<User> data;

    public AccountGetResult() {
        data = new ArrayList<>();
    }

    public static class User implements Serializable {
        public String Name;
        public String Mobile;
        public int    Sex;
        public String TaxNumber;
        public String TaxName;
        public String TaxAddress;
        public String Birthday;
        public String PhotoUrl;

        public User() {
        }
    }
}
