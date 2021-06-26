package com.mobile.application.payin.common.utilities;

import android.app.Activity;
import android.content.Context;
import android.content.SharedPreferences;

import com.android.application.payin.R;
import com.mobile.application.payin.common.serverconnections.ServerPost;
import com.mobile.application.payin.domain.ControlTrack;
import com.mobile.application.payin.dto.arguments.ControlPresenceMobileTrackArguments;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Set;

public class CheckTrack {
    private static DatabaseHandler db;

    public static void Check(Context context) {
        SharedPreferences pref = context.getSharedPreferences(context.getResources().getString(R.string.prefs), Context.MODE_PRIVATE);
        db = new DatabaseHandler(context);

        Set<String> set = pref.getStringSet("trackIds", new HashSet<String>());
        if (set.size() == 0) return;

        String[] trackIds = set.toArray(new String[set.size()]);

        ControlPresenceMobileTrackArguments query = new ControlPresenceMobileTrackArguments();
        query.TrackItems = leerTrackingDB(context);

        if (query.TrackItems.isEmpty()) return ;

        for (String tckId : trackIds) {
            ControlPresenceMobileTrackArguments.Track tck = new ControlPresenceMobileTrackArguments.Track();
            tck.Id = Integer.parseInt(tckId);
            query.Tracks.add(tck);
        }

        ServerPost task = new ServerPost(context);
        task.delegate = null;
        task.showProgress = false;
        task.execute(context.getString(R.string.apiControlPresenceMobileTrack), CustomGson.getGson().toJson(query));

        db.deleteAllTracking();
    }

    private static ArrayList<ControlPresenceMobileTrackArguments.TrackItem> leerTrackingDB(Context context) {
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
}
