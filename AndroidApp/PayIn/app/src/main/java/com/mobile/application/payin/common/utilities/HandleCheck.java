package com.mobile.application.payin.common.utilities;

import android.app.Activity;
import android.app.ActivityManager;
import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;

import com.android.application.payin.R;
import com.mobile.application.payin.dto.results.ControlIncidentCreateManualCheckResult;
import com.mobile.application.payin.dto.results.ControlPresenceCheckResult;
import com.mobile.application.payin.views.Principal;
import com.mobile.application.payin.services.TrackService;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.HashSet;
import java.util.Locale;
import java.util.Set;
import java.util.TimeZone;

public class HandleCheck {
    private static SharedPreferences pref;
    private static SharedPreferences.Editor edit;

    public static void Handle(boolean saveTrack, Activity activity, Context context, ControlPresenceCheckResult res) {
        pref = context.getSharedPreferences(context.getResources().getString(R.string.prefs), Context.MODE_PRIVATE);
        edit = pref.edit();

        if (saveTrack) checkTrack(activity, context, res);

        edit.apply();

        showCorrectDialog(activity, context);
    }

    public static void Handle(boolean saveTrack, Activity activity, Context context, ControlIncidentCreateManualCheckResult res) {
        pref = context.getSharedPreferences(context.getResources().getString(R.string.prefs), Context.MODE_PRIVATE);
        edit = pref.edit();

        if (saveTrack) checkTrack(activity, context, res);

        edit.apply();
        showCorrectDialog(activity, context);
    }

    private static void checkTrack(Activity activity, Context context, ControlPresenceCheckResult res) {
        TimeZone tz = TimeZone.getTimeZone("UTC");
        SimpleDateFormat sdf = new SimpleDateFormat("HH:mm:ss'Z'", Locale.getDefault());
        Set<String> trackIds = pref.getStringSet("trackIds", null);
        sdf.setTimeZone(tz);

        edit.remove("trackIds");
        edit.apply();

        if (trackIds == null)
            trackIds = new HashSet<>();

        for (ControlPresenceCheckResult.Track track : res.Entrances){
            trackIds.add("" + track.Id);
        }

        for (ControlPresenceCheckResult.Track track : res.Exits){
            trackIds.remove("" + track.Id);
        }

        edit.putStringSet("trackIds", trackIds);

        Calendar cal = Calendar.getInstance();
        int trackFrecuency;
        try {
            cal.setTimeInMillis(sdf.parse(res.TrackFrecuency).getTime());
            trackFrecuency = ((cal.get(Calendar.HOUR_OF_DAY) * 60 + cal.get(Calendar.MINUTE)) * 60 + cal.get(Calendar.SECOND)) * 1000;
        } catch (ParseException e) {
            trackFrecuency = 10000;
        }

        if (!pref.contains("trackFrecuency") || (pref.getInt("trackFrecuency", 0) > trackFrecuency)) {
            edit.putInt("trackFrecuency", trackFrecuency);
        }
        edit.apply();

        if (trackIds.size() == 0 && isMyServiceRunning(TrackService.class, activity)) {
            context.stopService(new Intent(activity, TrackService.class));
            edit.remove("trackFrecuency").apply();
        } else if (trackIds.size() != 0 && !isMyServiceRunning(TrackService.class, activity)) {
            context.startService(new Intent(activity, TrackService.class));
        }
    }

    private static void checkTrack(Activity activity, Context context, ControlIncidentCreateManualCheckResult res) {
        TimeZone tz = TimeZone.getTimeZone("UTC");
        SimpleDateFormat sdf = new SimpleDateFormat("HH:mm:ss'Z'", Locale.getDefault());
        Set<String> trackIds = pref.getStringSet("trackIds", null);
        edit.remove("trackIds");
        edit.apply();

        if (trackIds == null)
            trackIds = new HashSet<>();

        for (ControlIncidentCreateManualCheckResult.Track track : res.Entrances){
            trackIds.add("" + track.Id);
        }

        for (ControlIncidentCreateManualCheckResult.Track track : res.Exits){
            trackIds.remove("" + track.Id);
        }

        edit.putStringSet("trackIds", trackIds);

        Calendar cal = Calendar.getInstance();
        int trackFrecuency;
        try {
            cal.setTimeInMillis(sdf.parse(res.TrackFrecuency).getTime());
            trackFrecuency = ((cal.get(Calendar.HOUR_OF_DAY) * 60 + cal.get(Calendar.MINUTE)) * 60 + cal.get(Calendar.SECOND)) * 1000;
        } catch (ParseException e) {
            trackFrecuency = 10000;
        }

        if (!pref.contains("trackFrecuency") || (pref.getInt("trackFrecuency", 0) > trackFrecuency)) {
            edit.putInt("trackFrecuency", trackFrecuency);
        }
        edit.apply();

        edit.apply();
        if (trackIds.size() == 0 && isMyServiceRunning(TrackService.class, activity)) {
            context.stopService(new Intent(activity, TrackService.class));
            edit.remove("trackFrecuency").apply();
        } else if (trackIds.size() != 0 && !isMyServiceRunning(TrackService.class, activity)) {
            context.startService(new Intent(activity, TrackService.class));
        }
    }

    private static boolean isMyServiceRunning(Class<?> serviceClass, Activity activity) {
        ActivityManager manager = (ActivityManager) activity.getSystemService(Context.ACTIVITY_SERVICE);
        for (ActivityManager.RunningServiceInfo service : manager.getRunningServices(Integer.MAX_VALUE)) {
            if (serviceClass.getName().equals(service.service.getClassName())) {
                return true;
            }
        }
        return false;
    }

    private static void showCorrectDialog(final Activity activity, final Context context){
        AlertDialog.Builder builder = new AlertDialog.Builder(activity);

        builder.setMessage("El fichaje se ha realizado correctamente").setTitle("Fichaje correcto");

        builder.setPositiveButton("Ok", new DialogInterface.OnClickListener() {
            public void onClick(DialogInterface dialog, int id) {
                dialog.dismiss();
                Intent i = new Intent(activity, Principal.class);
                i.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                context.startActivity(i);
                activity.finish();
            }
        });

        builder.show();
    }
}
