package com.mobile.application.payin.dto.arguments;

import android.content.Context;

public class TicketPayArguments extends MobileConfigurationArguments {
    public int    Id;
    public int    PaymentMediaId;
    public String Pin;

    public TicketPayArguments(Context context) {
        super(context);
    }
}
