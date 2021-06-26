package com.mobile.application.payin.dto.results;

import java.io.Serializable;
import java.util.ArrayList;

public class PaymentConcessionGetAll {
    public ArrayList<PaymentConcession> Data;

    public PaymentConcessionGetAll() {
        Data = new ArrayList<>();
    }

    public static class PaymentConcession implements Serializable {
        public int    Id;
        public String Name;
        public String Phone;
        public String Address;
        public String Cif;
        
        public PaymentConcession() {
        }
    }
}
