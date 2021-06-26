package com.mobile.application.payin.dto.results;

import java.io.Serializable;
import java.util.ArrayList;

public class ControlItemMobileGetSelectorResult implements Serializable {
    public ArrayList<ControlItemMobileGetSelectorResult.Item> Data;

    public ControlItemMobileGetSelectorResult() {

    }

    public static class Item implements Serializable {
        public int    Id;
        public String Value;

        public Item() {
        }
    }
}
