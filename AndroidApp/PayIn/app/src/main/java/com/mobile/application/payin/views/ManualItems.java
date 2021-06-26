package com.mobile.application.payin.views;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.os.Bundle;
import android.support.v4.content.res.ResourcesCompat;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.TextView;

import com.android.application.payin.R;
import com.mobile.application.payin.common.serverconnections.ServerGet;
import com.mobile.application.payin.common.interfaces.AsyncResponse;
import com.mobile.application.payin.common.types.PresenceType;
import com.mobile.application.payin.common.utilities.CustomGson;
import com.mobile.application.payin.common.utilities.HandleServerError;
import com.mobile.application.payin.dto.results.ControlItemMobileGetAllResult;

import java.util.ArrayList;
import java.util.Calendar;
import java.util.HashMap;

public class ManualItems extends AppCompatActivity implements AsyncResponse {
    private static ListView lv;
    private static Context context = null;
    private static ArrayList<ControlItemMobileGetAllResult.Item> lista = null;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.manual_items);

        Toolbar toolbar = (Toolbar) findViewById(R.id.tool_bar);
        setSupportActionBar(toolbar);
        toolbar.setNavigationIcon(ResourcesCompat.getDrawable(getResources(), R.drawable.ic_action_back, null));
        toolbar.setNavigationOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                finish();
            }
        });

        context = this;

        TextView txtDay = (TextView) findViewById(R.id.txtDay);

        Calendar now = Calendar.getInstance();

        txtDay.setText(String.format("%02d-%02d-%04d", now.get(Calendar.DAY_OF_MONTH), now.get(Calendar.MONTH) + 1, now.get(Calendar.YEAR)));

        // Comprobamos las preferencias del modo debug
        SharedPreferences pref = getSharedPreferences(getResources().getString(R.string.prefs), MODE_PRIVATE);

        Boolean debug = pref.getBoolean("debug", false);

        // Con el modo debug activado ponemos el color de fondo en rojo
        if (debug){
            toolbar.setBackgroundColor(Color.parseColor("#ff1a1a"));
            txtDay.setBackgroundColor(Color.parseColor("#ff1a1a"));
        } else {
            toolbar.setBackgroundColor(Color.parseColor("#e9af30"));
            txtDay.setBackgroundColor(Color.parseColor("#e9af30"));
        }

        lv = (ListView) findViewById(R.id.listFichajeManual);

        lv.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                ControlItemMobileGetAllResult.Item item = lista.get(position);

                Intent i = new Intent(ManualItems.this, ManualCheck.class);
                i.putExtra("result", item);
                i.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TASK);
                startActivity(i);
            }
        });

        ServerGet tarea = new ServerGet(this);
        tarea.delegate = this;
        tarea.execute(getResources().getString(R.string.apiControlItemMobileGetAll));
    }

    @Override
    public void onBackPressed() {
        finish();
    }

    @Override
    public void onAsyncFinish (HashMap<String, String> map){
        if (HandleServerError.Handle(map, this, this)) return ;

        ControlItemMobileGetAllResult res = CustomGson.getGson().fromJson(map.get("json"), ControlItemMobileGetAllResult.class);

        lista = res.Data;
        if (context != null) {
            Adaptador adapter = new Adaptador(context, lista);
            lv.setAdapter(adapter);
        }
    }

    public static class Adaptador extends BaseAdapter {
        private static LayoutInflater inflador = null;
        private ArrayList<ControlItemMobileGetAllResult.Item> lista;

        public Adaptador(Context contexto, ArrayList<ControlItemMobileGetAllResult.Item> lista){
            if (inflador == null){
                inflador = (LayoutInflater) contexto.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
            }
            this.lista = lista;
        }

        public View getView(int posicion, View vistaReciclada, ViewGroup padre) {
            if (vistaReciclada == null) {
                vistaReciclada = inflador.inflate(R.layout.manual_items_list_items, padre, false);
            }

            TextView txtName = (TextView) vistaReciclada.findViewById(R.id.txtName);
            ImageView imgType = (ImageView) vistaReciclada.findViewById(R.id.imgType);

            ControlItemMobileGetAllResult.Item res = lista.get(posicion);

            txtName.setText(res.Name);

            int presenceType = res.PresenceType;

            if (presenceType == PresenceType.ENTRANCE) { // Entrada
                imgType.setImageResource(R.drawable.ic_p_in);
            } else if (presenceType == PresenceType.EXIT) { // Salida
                imgType.setImageResource(R.drawable.ic_p_out);
            } else if (presenceType == PresenceType.ROUND)
                imgType.setImageResource(R.drawable.ic_p_check);
            return vistaReciclada;
        }

        public int getCount() {
            if (lista != null)
                return lista.size();
            else
                return 0;
        }

        public Object getItem(int posicion) {
            return lista.get(posicion);
        }

        public long getItemId(int posicion) {
            return posicion;
        }
    }
}
