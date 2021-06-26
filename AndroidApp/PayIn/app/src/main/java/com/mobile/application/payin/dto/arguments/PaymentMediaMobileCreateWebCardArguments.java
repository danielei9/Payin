package com.mobile.application.payin.dto.arguments;

import android.content.Context;

public class PaymentMediaMobileCreateWebCardArguments extends MobileConfigurationArguments {
    public String Name;
    public String Pin;
    public String BankEntity;

    public PaymentMediaMobileCreateWebCardArguments(Context context) {
        super(context);
    }
}
