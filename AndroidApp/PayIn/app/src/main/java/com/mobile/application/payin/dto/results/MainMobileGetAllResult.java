package com.mobile.application.payin.dto.results;

import java.util.ArrayList;

public class MainMobileGetAllResult {
    public int                     NumNotifications;
    public int                     AppVersion;
    public String                  SumChecks;
    public String                  CheckDuration;
    public Boolean                 Working;
    public ArrayList<Favorite>     Favorites;
    public ArrayList<Ticket>       Tickets;
    public ArrayList<PaymentMedia> Data;

    public MainMobileGetAllResult() {
        Favorites = new ArrayList<>();
        Tickets = new ArrayList<>();
        Data = new ArrayList<>();
    }

    public class Favorite
    {
        public int    Id;
        public String Title;
        public String Subtitle;
        public int    VisualOrder;
        public String ImagePath;
        public double Balance;
        public int    Type;

        public Favorite() {
        }
    }

    public static class Ticket
    {
        public int     Id;
        public String  Reference;
        public String  Title;
        public String  SupplierName;
        public double  Amount;
        public double  PayedAmount;
        public String  Date;
    }

    public static class PaymentMedia
    {
        public int     Id;
        public String  Title;
        public int     VisualOrder;
        public String  NumberHash;
        public String  ImagePath;
        public int     State;
        public int     Type;
        public String  BankEntity;
        public Integer ExpirationMonth;
        public Integer ExpirationYear;
    }

    public static class PinType
    {
        public static final int PAYMENTMEDIA    = 0;
        public static final int PARKINGMETER    = 1;
        public static final int CONTROLPRESENCE = 2;
        public static final int TICKET         =  3;
    }

    public static class ServiceType
    {
        public static final int CHARGE = 1;
        public static final int ORA = 2;
        public static final int PARKINGFINE = 3;
        public static final int CONTROL = 4;
    }
}
