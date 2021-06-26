package com.mobile.application.payin.common.utilities;

import java.util.ArrayList;
import java.util.List;

import android.content.ContentValues;
import android.content.Context;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;

import com.mobile.application.payin.domain.ControlTrack;

public class DatabaseHandler extends SQLiteOpenHelper {

    private static final int DATABASE_VERSION = 1;

    private static final String DATABASE_NAME = "track";

    private static final String TABLE_TRACKING = "tracking";

    private static final String KEY_ID = "id";
    private static final String KEY_LATITUD = "latitud";
    private static final String KEY_LONGITUD = "longitud";
    private static final String KEY_FECHA = "fecha";
    private static final String KEY_CALIDAD_GPS = "calidad_gps";
    private static final String KEY_SPEED_GPS = "velocidad_gps";

    public DatabaseHandler(Context context) {
        super(context, DATABASE_NAME, null, DATABASE_VERSION);
    }

    @Override
    public void onCreate(SQLiteDatabase db) {
        String CREATE_TRACK_TABLE = "CREATE TABLE " + TABLE_TRACKING + "(" + KEY_ID + " INTEGER PRIMARY KEY,"
                + KEY_LATITUD + " TEXT," + KEY_LONGITUD + " TEXT," + KEY_FECHA + " TEXT," + KEY_CALIDAD_GPS + " TEXT," + KEY_SPEED_GPS + " TEXT" + ")";
        db.execSQL(CREATE_TRACK_TABLE);
    }

    @Override
    public void onUpgrade(SQLiteDatabase db, int oldVersion, int newVersion) {
        db.execSQL("DROP TABLE IF EXISTS " + TABLE_TRACKING);

        onCreate(db);
    }

    public void addTracking(ControlTrack track) {
        SQLiteDatabase db = this.getWritableDatabase();

        ContentValues values = new ContentValues();
        values.put(KEY_LATITUD, track.getLatitud());
        values.put(KEY_LONGITUD, track.getLongitud());
        values.put(KEY_FECHA, track.getFecha());
        values.put(KEY_CALIDAD_GPS, track.getCalidadGPS());
        values.put(KEY_SPEED_GPS, track.getVelocidadGPS());

        db.insert(TABLE_TRACKING, null, values);
        db.close();
    }
    /*
    public ControlTrack getTracking(int id) {
        SQLiteDatabase db = this.getReadableDatabase();

        Cursor cursor = db.query(TABLE_TRACKING, new String[] { KEY_ID, KEY_LATITUD, KEY_LONGITUD, KEY_FECHA, KEY_CALIDAD_GPS, KEY_SPEED_GPS }, KEY_ID + "=?",
                new String[] { String.valueOf(id) }, null, null, null, null);

        if (cursor == null) return null;

        cursor.moveToFirst();

        ControlTrack track = new ControlTrack(Integer.parseInt(cursor.getString(0)), cursor.getString(1),
                cursor.getString(2), cursor.getString(3), cursor.getString(4), cursor.getString(5));

        cursor.close();

        return track;
    }
    */
    public List<ControlTrack> getAllTracking() {
        List<ControlTrack> trackList = new ArrayList<>();

        String selectQuery = "SELECT * FROM " + TABLE_TRACKING;

        SQLiteDatabase db = this.getWritableDatabase();
        Cursor cursor = db.rawQuery(selectQuery, null);

        if (cursor.moveToFirst()) {
            do {
                ControlTrack track = new ControlTrack();
                track.setID(Integer.parseInt(cursor.getString(0)));
                track.setLatitud(cursor.getString(1));
                track.setLongitud(cursor.getString(2));
                track.setFecha(cursor.getString(3));
                track.setCalidadGPS(cursor.getString(4));
                track.setVelocidadGPS(cursor.getString(5));

                trackList.add(track);
            } while (cursor.moveToNext());
        }

        cursor.close();

        return trackList;
    }
    /*
    public int updateTracking(ControlTrack track) {
        SQLiteDatabase db = this.getWritableDatabase();

        ContentValues values = new ContentValues();
        values.put(KEY_LATITUD, track.getLatitud());
        values.put(KEY_LONGITUD, track.getLongitud());
        values.put(KEY_FECHA, track.getFecha());
        values.put(KEY_CALIDAD_GPS, track.getCalidadGPS());
        values.put(KEY_SPEED_GPS, track.getVelocidadGPS());

        return db.update(TABLE_TRACKING, values, KEY_ID + " = ?",
                new String[] { String.valueOf(track.getID()) });
    }
    */
    /*
    public void deleteTracking(ControlTrack track) {
        SQLiteDatabase db = this.getWritableDatabase();
        db.delete(TABLE_TRACKING, KEY_ID + " = ?",
                new String[] { String.valueOf(track.getID()) });
        db.close();
    }
    */
    public void deleteAllTracking() {
        SQLiteDatabase db = this.getWritableDatabase();
        db.delete(TABLE_TRACKING, "1", null);
        db.close();
    }
    /*
    public int getTrackingCount() {
        String countQuery = "SELECT  * FROM " + TABLE_TRACKING;
        SQLiteDatabase db = this.getReadableDatabase();
        Cursor cursor = db.rawQuery(countQuery, null);
        cursor.close();

        return cursor.getCount();
    }
    */
}
