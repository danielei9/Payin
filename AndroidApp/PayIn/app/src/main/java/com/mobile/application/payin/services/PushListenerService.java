package com.mobile.application.payin.services;

import android.annotation.TargetApi;
import android.app.Notification;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.media.RingtoneManager;
import android.net.Uri;
import android.os.Bundle;
import android.support.v4.app.TaskStackBuilder;
import android.support.v7.app.NotificationCompat;

import com.android.application.payin.R;
import com.google.android.gms.gcm.GcmListenerService;
import com.mobile.application.payin.common.utilities.IntegerExpansion;
import com.mobile.application.payin.views.EnterpriseList;
import com.mobile.application.payin.views.Principal;
import com.mobile.application.payin.views.TicketCute;

public class PushListenerService  extends GcmListenerService {
    @Override
    public void onMessageReceived(String from, Bundle data) {
        int id = IntegerExpansion.getIntValueOf(data.getString("id"))/*, type = IntegerExpansion.getIntValueOf(data.getString("type"))*/;
        String message = data.getString("message"),
                clase = data.getString("class")/*,
                mensaje = ""*/;
        if (clase == null)
            return ;

        if (clase.toLowerCase().equals("paymentmedia")) {
            /*
            if (type == NotificationType.PaymentMediaCreateSucceed)
                mensaje = getString(R.string.notification_paymentMedia_ok) + message;
            else if (type == NotificationType.PaymentMediaCreateError)
                mensaje = getString(R.string.notification_paymentMedia_error) + message;
            */
            sendNotification(Principal.class, id, message);
        } else if (clase.toLowerCase().equals("ticket")) {
            /*
            if (type == NotificationType.PaymentSucceed)
                mensaje = getString(R.string.notification_payment_ok) + message;
            else if (type == NotificationType.PaymentFail)
                mensaje = getString(R.string.notification_payment_error) + message;
            else if (type == NotificationType.RefundSucceed)
                mensaje = getString(R.string.notification_refund_ok) + message;
            else if (type == NotificationType.RefundFail)
                mensaje = getString(R.string.notification_refund_error) + message;
            */
            sendNotification(TicketCute.class, id, message);
        } else if (clase.toLowerCase().equals("serviceworker") || clase.toLowerCase().equals("paymentworker"))
            sendNotification(EnterpriseList.class, id, message);
    }

    @TargetApi(16)
    private void sendNotification(Class destination, int id, String message) {
        SharedPreferences pref = getSharedPreferences(getResources().getString(R.string.prefs), MODE_PRIVATE);
        int notificationId = pref.getInt("notificationId", 1);

        Intent intent = new Intent(this, destination);
        intent.putExtra("id", id);
        intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);

        TaskStackBuilder stackBuilder = TaskStackBuilder.create(this);
        stackBuilder.addParentStack(destination);
        stackBuilder.addNextIntent(intent);

        PendingIntent pendingIntent = stackBuilder.getPendingIntent(0, PendingIntent.FLAG_UPDATE_CURRENT);

        Uri defaultSoundUri= RingtoneManager.getDefaultUri(RingtoneManager.TYPE_NOTIFICATION);
        Notification notification = new NotificationCompat.Builder(this)
                .setSmallIcon(R.drawable.ic_notification_push)
                .setContentTitle("Pay[in]")
                .setContentText(message)
                .setStyle(new NotificationCompat.BigTextStyle().bigText(message))
                .setAutoCancel(true)
                .setSound(defaultSoundUri)
                .setContentIntent(pendingIntent)
                .build();

        if (android.os.Build.VERSION.SDK_INT >= 16)
            notification.priority = Notification.PRIORITY_HIGH;
        else
            notification.flags |= Notification.FLAG_HIGH_PRIORITY;

        NotificationManager notificationManager = (NotificationManager) getSystemService(Context.NOTIFICATION_SERVICE);

        notificationManager.notify(notificationId, notification);

        pref.edit().putInt("notificationId", ++notificationId).apply();
    }
}
