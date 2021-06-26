package com.mobile.application.payin.views;
/*
import android.content.SharedPreferences;
import android.graphics.Color;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Spinner;
import android.widget.TextView;

import com.google.android.gms.maps.GoogleMap;
import com.google.android.gms.maps.SupportMapFragment;
import com.android.application.payin.R;
import com.mobile.application.payin.common.serverconnections.ServerGet;
import com.mobile.application.payin.common.interfaces.AsyncResponse;
import com.mobile.application.payin.common.utilities.HandleServerError;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;

public class Ora extends AppCompatActivity implements AsyncResponse{

    private GoogleMap map; // Might be null if Google Play services APK is not available.

    private static ArrayList serverInfo;
    private static ArrayList calleInfo;

    private static ArrayList<String> ciudadList = new ArrayList<>();
    private static ArrayList<String> calleList = new ArrayList<>();
    private static ArrayAdapter<String> ciudadAdap = null;
    private static ArrayAdapter<String> calleAdap = null;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.ora);
        setUpMapIfNeeded();

        Toolbar toolbar = (Toolbar) findViewById(R.id.tool_bar);
        setSupportActionBar(toolbar);

        // Comprobamos las preferencias del modo debug
        SharedPreferences pref = getSharedPreferences(getResources().getString(R.string.prefs), MODE_PRIVATE);

        Boolean debug = pref.getBoolean("debug", false);

        // Con el modo debug activado ponemos el color de fondo en rojo
        if (debug) toolbar.setBackgroundColor(Color.parseColor("#ff1a1a"));
        else toolbar.setBackgroundColor(Color.parseColor("#e9af30"));

        // TextView supplierText = (TextView) findViewById(R.id.supplierId);

        Spinner ciudadSpinner = (Spinner) findViewById(R.id.ciudadSpinner);
        Spinner calleSpinner = (Spinner) findViewById(R.id.calleSpinner);

        ciudadAdap = new ArrayAdapter<>(this, android.R.layout.simple_spinner_item, ciudadList);

        ciudadSpinner.setAdapter(ciudadAdap);

        ciudadAdap.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);

        ciudadSpinner.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> adapterView, View view, int position, long id) {
                Spinner ciudadSpin = (Spinner) findViewById(R.id.ciudadSpinner);
                ciudadSpin.setSelection(position);

                calleInfo = (ArrayList) ((Map)(serverInfo.get(position))).get("addresses");

                for (int i = 0; i < calleInfo.size(); i++){
                    calleList.add((String) ((Map) (calleInfo.get(i))).get("name"));
                }

                calleAdap.notifyDataSetChanged();
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {
            }
        });

        calleAdap = new ArrayAdapter<>(this, android.R.layout.simple_spinner_item, calleList);

        calleSpinner.setAdapter(calleAdap);

        calleAdap.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);

        calleSpinner.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> adapterView, View view, int position, long id) {
                // Spinner ciudadSpin = (Spinner) findViewById(R.id.ciudadSpinner);
                Spinner calleSpin = (Spinner) findViewById(R.id.calleSpinner);

                calleSpin.setSelection(position);

                TextView supplierText = (TextView) findViewById(R.id.supplierId);

                supplierText.setText( (String) ((Map) ((Map) calleInfo.get(position)).get("zone")).get("supplierName") );
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {
            }
        });

        ServerGet tarea = new ServerGet(Ora.this);
        tarea.delegate = this;
        tarea.execute("city/ConcessionType?Type=2");
    }

    @Override
    protected void onResume() {
        super.onResume();
        setUpMapIfNeeded();
    }

    @Override
    public void onAsyncFinish(HashMap<String, String> map){
        if (HandleServerError.Handle(map, this, this)) return;

        //serverInfo = (ArrayList) map.get("data");

        for (int i = 0; i < serverInfo.size(); i++)
            ciudadList.add( (String) ((Map)serverInfo.get(i)).get("name") );

        if (ciudadAdap != null)
            ciudadAdap.notifyDataSetChanged();
    }

    private void setUpMapIfNeeded() {
        // Do a null check to confirm that we have not already instantiated the map.
        if (map == null) {
            // Try to obtain the map from the SupportMapFragment.
            map = ((SupportMapFragment) getSupportFragmentManager().findFragmentById(R.id.map)).getMap();
            // Check if we were successful in obtaining the map.
            if (map != null) {
                setUpMap();
            }
        }
    }

    private void setUpMap() {

    }
}
*/