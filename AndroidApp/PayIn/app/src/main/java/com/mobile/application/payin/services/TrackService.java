package com.mobile.application.payin.services;

import android.app.Notification;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.app.Service;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.location.Location;
import android.location.LocationManager;
import android.os.Binder;
import android.os.Bundle;
import android.os.IBinder;
import android.support.v7.app.NotificationCompat;
import android.util.Log;

import com.android.application.payin.R;
import com.google.android.gms.common.ConnectionResult;
import com.google.android.gms.common.GooglePlayServicesUtil;
import com.google.android.gms.common.api.GoogleApiClient;
import com.google.android.gms.common.api.GoogleApiClient.ConnectionCallbacks;
import com.google.android.gms.common.api.GoogleApiClient.OnConnectionFailedListener;
import com.google.android.gms.location.LocationListener;
import com.google.android.gms.location.LocationRequest;
import com.google.android.gms.location.LocationServices;
import com.mobile.application.payin.common.serverconnections.ServerPost;
import com.mobile.application.payin.common.interfaces.AsyncResponse;
import com.mobile.application.payin.common.utilities.CheckNetworks;
import com.mobile.application.payin.common.utilities.CustomGson;
import com.mobile.application.payin.common.utilities.DatabaseHandler;
import com.mobile.application.payin.domain.ControlTrack;
import com.mobile.application.payin.dto.arguments.ControlPresenceMobileTrackArguments;
import com.mobile.application.payin.views.Principal;

import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.HashMap;
import java.util.HashSet;
import java.util.List;
import java.util.Locale;
import java.util.Set;
import java.util.TimeZone;

public class TrackService extends Service implements ConnectionCallbacks, OnConnectionFailedListener, LocationListener, AsyncResponse {

    private final IBinder mBinder = new LocalBinder();

    private static final String TAG = "Track";
    private Location mLastLocation;

    private GoogleApiClient mGoogleApiClient;

    private boolean mRequestingLocationUpdates = true;

    private LocationRequest mLocationRequest;

    //private static final int UPDATE_INTERVAL = 10000; // 10 segundos
    //private static final int FASTEST_INTERVAL = 5000; // 5 segundos
    private static final int DISPLACEMENT = 1; // 1 metros

    private DatabaseHandler db;
    private NotificationManager NM;

    private static long anterior = System.currentTimeMillis();
    private ThreadPosition hilo;
    private SharedPreferences pref;

    private class LocalBinder extends Binder {
        public TrackService getServerInstance() {
            return TrackService.this;
        }
    }

    @Override
    public void onCreate() {
        super.onCreate();

        db = new DatabaseHandler(this);

        pref = getSharedPreferences(getResources().getString(R.string.prefs), MODE_PRIVATE);

        if (checkPlayServices()) {
            buildGoogleApiClient();
            createLocationRequest();
        }
    }

    @Override
    public int onStartCommand(Intent intenc, int flags, int idArranque) {
        super.onStartCommand(intenc, flags, idArranque);

        if (mGoogleApiClient != null) {
            mGoogleApiClient.connect();
            if (mGoogleApiClient.isConnected() && mRequestingLocationUpdates) {
                startLocationUpdates();
            }
        }

        hilo = new ThreadPosition();
        hilo.start();

        return START_STICKY;
    }

    @Override
    public void onDestroy() {
        stopLocationUpdates();
        mRequestingLocationUpdates = false;
        if (mGoogleApiClient.isConnected()) {
            mGoogleApiClient.disconnect();
        }
        hilo.detener();
        super.onDestroy();
    }

    @Override
    public IBinder onBind(Intent intencion) {
        return mBinder;
    }

    private synchronized void buildGoogleApiClient() {
        mGoogleApiClient = new GoogleApiClient.Builder(this).addConnectionCallbacks(this).addOnConnectionFailedListener(this).addApi(LocationServices.API).build();
    }

    private void createLocationRequest() {
        mLocationRequest = new LocationRequest();
        mLocationRequest.setInterval(pref.getInt("trackFrecuency", 10000));
        mLocationRequest.setFastestInterval(pref.getInt("trackFrecuency", 10000) / 2);
        mLocationRequest.setPriority(LocationRequest.PRIORITY_HIGH_ACCURACY);
        mLocationRequest.setSmallestDisplacement(DISPLACEMENT);
    }

    private boolean checkPlayServices() {
        int resultCode = GooglePlayServicesUtil.isGooglePlayServicesAvailable(this);
        return resultCode == ConnectionResult.SUCCESS;
    }

    private void startLocationUpdates() {
        notificationPush();
        try {
            LocationServices.FusedLocationApi.requestLocationUpdates(mGoogleApiClient, mLocationRequest, this);
        } catch (SecurityException e) {
            stopSelf();
            return;
        }
    }

    private void stopLocationUpdates() {
        if (NM != null)
            NM.cancelAll();
        try {
            LocationServices.FusedLocationApi.removeLocationUpdates(mGoogleApiClient, this);
        } catch (SecurityException e) {}
    }

    @Override
    public void onConnectionFailed(ConnectionResult result) {
        Log.i(TAG, "Conexión fallida: " + result.getErrorCode());
    }

    @Override
    public void onConnected(Bundle arg0) {
        if (mRequestingLocationUpdates) {
            startLocationUpdates();
        }
    }

    @Override
    public void onConnectionSuspended(int arg0) {
        mGoogleApiClient.connect();
    }

    @Override
    public void onLocationChanged(Location location) {
        mLastLocation = location;
    }

    private void añadirTrackingDB() {
        DateFormat df = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss'Z'", Locale.getDefault());
        df.setTimeZone(TimeZone.getTimeZone("UTC"));

        //notificationPush("A: (" + mLastLocation.getLatitude() + ", " + mLastLocation.getLongitude() + ") " + df.format(Calendar.getInstance().getTime()));

        db.addTracking(new ControlTrack(String.valueOf(mLastLocation.getLatitude()), String.valueOf(mLastLocation.getLongitude()),
                df.format(Calendar.getInstance().getTime()), String.valueOf(((int) mLastLocation.getAccuracy())), String.valueOf(mLastLocation.getSpeed())));
    }

    private ArrayList<ControlPresenceMobileTrackArguments.TrackItem> leerTrackingDB() {
        List<ControlTrack> trackings = db.getAllTracking();

        ArrayList<ControlPresenceMobileTrackArguments.TrackItem> tracks = new ArrayList<>();

        for (ControlTrack tck : trackings) {
            ControlPresenceMobileTrackArguments.TrackItem tckItem = new ControlPresenceMobileTrackArguments.TrackItem();

            tckItem.Date = tck.getFecha();
            tckItem.Latitude = Double.parseDouble(tck.getLatitud());
            tckItem.Longitude = Double.parseDouble(tck.getLongitud());
            tckItem.Quality = Integer.parseInt(tck.getCalidadGPS());
            tckItem.Speed = Double.parseDouble(tck.getVelocidadGPS());

            tracks.add(tckItem);
        }

        return tracks;
    }

    @SuppressWarnings("deprecation")
    private void notificationPush() {
        String title = "Iniciando Track";
        String subject = "Pay[in]";
        String body = "Tracking...";

        Intent intent = new Intent(TrackService.this, Principal.class);
        PendingIntent pending = PendingIntent.getActivity(this, 0, intent, 0);
        NM = (NotificationManager) getSystemService(Context.NOTIFICATION_SERVICE);

        NotificationCompat.Builder builder = new NotificationCompat.Builder(getApplicationContext());

        Notification notify = builder
                .setContentIntent(pending)
                .setSmallIcon(R.drawable.ic_notification_push)
                .setWhen(0)
                .setAutoCancel(false)
                .setTicker(title)
                .setContentTitle(subject)
                .setContentText(body.replace(" ", ""))
                .build();

        notify.flags |= Notification.FLAG_NO_CLEAR;
        notify.flags |= Notification.FLAG_ONGOING_EVENT;

        NM.notify(0, notify);
    }

    private void notificationPush(String body) {
        String subject = "Pay[in]";

        Intent intent = new Intent(TrackService.this, Principal.class);
        PendingIntent pending = PendingIntent.getActivity(this, 0, intent, 0);
        NM = (NotificationManager) getSystemService(Context.NOTIFICATION_SERVICE);

        NM.cancel(0);

        NotificationCompat.Builder builder = new NotificationCompat.Builder(getApplicationContext());

        Notification notify = builder
                .setContentIntent(pending)
                .setSmallIcon(R.drawable.ic_notification_push)
                .setWhen(0)
                .setAutoCancel(false)
                .setContentTitle(subject)
                .setStyle(new NotificationCompat.BigTextStyle().bigText(body.replace(" ", "")))
                .setContentText(body.replace(" ", ""))
                .build();

        notify.flags |= Notification.FLAG_NO_CLEAR;
        notify.flags |= Notification.FLAG_ONGOING_EVENT;

        NM.notify(0, notify);
    }

    private void sendPosition() {
        long ahora = System.currentTimeMillis();

        //mLastLocation = LocationServices.FusedLocationApi.getLastLocation(mGoogleApiClient);

        if ((anterior + pref.getInt("trackFrecuency", 10000) > ahora) || (mLastLocation == null))
            return ;

        if (! ((LocationManager) getSystemService(Context.LOCATION_SERVICE)).isProviderEnabled(LocationManager.GPS_PROVIDER))
            return ;


        anterior = ahora;

        if (CheckNetworks.checkInternetStrict(this)) {
            TimeZone tz = TimeZone.getTimeZone("UTC");
            DateFormat df = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss'Z'", Locale.getDefault());
            df.setTimeZone(tz);

            Set<String> set = pref.getStringSet("trackIds", new HashSet<String>());
            if (set.size() == 0) return;
            String[] trackIds = set.toArray(new String[set.size()]);

            ControlPresenceMobileTrackArguments query = new ControlPresenceMobileTrackArguments();
            query.TrackItems = leerTrackingDB();

            for (String tckId : trackIds) {
                ControlPresenceMobileTrackArguments.Track tck = new ControlPresenceMobileTrackArguments.Track();
                tck.Id = Integer.parseInt(tckId);
                query.Tracks.add(tck);
            }

            ControlPresenceMobileTrackArguments.TrackItem tckItem = new ControlPresenceMobileTrackArguments.TrackItem();
            tckItem.Date = df.format(Calendar.getInstance().getTime());
            tckItem.Latitude = mLastLocation.getLatitude();
            tckItem.Longitude = mLastLocation.getLongitude();
            tckItem.Quality = (int) mLastLocation.getAccuracy();
            tckItem.Speed = mLastLocation.getSpeed();

            query.TrackItems.add(tckItem);

            String sQuery = CustomGson.getGson().toJson(query);

            //notificationPush("E. " + query.TrackItems.size() + ": (" + tckItem.Latitude + ", " + tckItem.Longitude + ") " + tckItem.Date);

            ServerPost task = new ServerPost(this);
            task.delegate = this;
            task.showProgress = false;
            task.execute(getString(R.string.apiControlPresenceMobileTrack), sQuery);
        } else {
            añadirTrackingDB();
        }
    }

    @Override
    public void onAsyncFinish(HashMap<String, String> map) {
        if (map.containsKey("success")){
            db.deleteAllTracking();
        }
    }

    class ThreadPosition extends Thread {
        private boolean run = true;

        @Override
        public void run() {
            while (run)
                sendPosition();
        }

        public void detener() {
            run = false;
        }
    }
}