package com.mobile.application.payin.dto.arguments;

import android.content.Context;

public class PaymentRefundArguments extends MobileConfigurationArguments{
    public int Id;
    public String Pin;

    public PaymentRefundArguments(Context context) {
        super(context);
    }
}
