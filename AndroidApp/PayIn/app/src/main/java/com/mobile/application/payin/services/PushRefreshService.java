package com.mobile.application.payin.services;

import android.content.Intent;

import com.google.android.gms.iid.InstanceIDListenerService;

public class PushRefreshService extends InstanceIDListenerService {
    @Override
    public void onTokenRefresh() {
        Intent intent = new Intent(this, PushRegisterService.class);
        startService(intent);
    }
}
