package com.mobile.application.payin.dto.results;

import java.io.Serializable;
import java.util.ArrayList;

public class TicketMobileGetResult implements Serializable {
    public ArrayList<Ticket> Data;
    public ArrayList<PaymentMedia> PaymentMedias;
    public boolean HasPayment;

    public TicketMobileGetResult() {
        Data = new ArrayList<>();
        PaymentMedias = new ArrayList<>();
    }

    public static class Ticket {
        public int     Id;
        public String  Reference;
        public String  Title;
        public double  Amount;
        public double  PayedAmount;
        public String  Date;
        public int     State;
        public boolean CanReturn;
        public String  SupplierName;
        public String  SupplierAddress;
        public String  SupplierNumber;
        public String  SupplierPhone;
        public String  WorkerName;

        public ArrayList<Payment> Payments;

        public Ticket() {
            Payments = new ArrayList<>();
        }
    }

    public static class Payment {
        public int     Id;
        public double  Amount;
        public String  UserName;
        public String  PaymentMediaName;
        public String  Date;
        public int     State;
        public boolean CanBeReturned;

        public Payment() {
        }
    }

    public static class PaymentMedia  {
        public int     Id;
        public String  Title;
        public String  Subtitle;
        public int     VisualOrder;
        public String  NumberHash;
        public String  ImagePath;
        public double  Balance;
        public int     Type;
        public int     State;
        public String  BankEntity;
        public Integer ExpirationMonth;
        public Integer ExpirationYear;

        public PaymentMedia() {
        }
    }
}
