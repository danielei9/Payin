package com.mobile.application.payin.views;

import android.app.AlertDialog;
import android.app.ProgressDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.location.Location;
import android.location.LocationListener;
import android.location.LocationManager;
import android.nfc.NfcAdapter;
import android.nfc.Tag;
import android.os.AsyncTask;
import android.os.Bundle;
import android.provider.Settings;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.BaseAdapter;
import android.widget.ImageButton;
import android.widget.ScrollView;
import android.widget.Spinner;
import android.widget.TextView;

import com.android.application.payin.R;
import com.mobile.application.payin.common.utilities.CheckTrack;
import com.mobile.application.payin.common.utilities.IntegerExpansion;
import com.mobile.application.payin.common.utilities.SecurityDialog;
import com.mobile.application.payin.dto.arguments.ControlPresenceCheckArguments;
import com.mobile.application.payin.dto.results.ControlPresenceCheckResult;
import com.mobile.application.payin.dto.results.ServiceTagGetResult;
import com.mobile.application.payin.common.serverconnections.ServerGet;
import com.mobile.application.payin.common.serverconnections.ServerPost;
import com.mobile.application.payin.common.interfaces.AsyncResponse;
import com.mobile.application.payin.common.types.PresenceType;
import com.mobile.application.payin.common.utilities.CustomGson;
import com.mobile.application.payin.common.utilities.HandleCheck;
import com.mobile.application.payin.common.utilities.HandleServerError;

import java.math.BigInteger;
import java.text.DateFormat;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.HashMap;
import java.util.Locale;
import java.util.TimeZone;

public class NFCCheck extends AppCompatActivity implements AsyncResponse, LocationListener {
    private ImageButton btnFichar;
    private TextView txtPress;
    private ScrollView scrollView;
    private static Context context;
    private AsyncResponse delegate;

    private String route;

    private ServiceTagGetResult.Tag TagResult;
    private ArrayList<ServiceTagGetResult.Item> Forms;
    private ControlPresenceCheckArguments PresenceArguments;

    private boolean saveFacial = false, saveTrack = false, doCheck = false;

    private LocationManager locationManager;
    private LocationListener locationListener;
    private Location location;
    private Double currentLatitude, currentLongitude;
    private boolean searchingLocation;

    private ProgressDialog progreso = null;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.nfc_check);

        context = this;
        delegate = this;

        Toolbar toolbar = (Toolbar) findViewById(R.id.tool_bar);
        setSupportActionBar(toolbar);

        // Comprobamos las preferencias del modo debug
        SharedPreferences pref = getSharedPreferences(getResources().getString(R.string.prefs), MODE_PRIVATE);

        Boolean debug = pref.getBoolean("debug", false);

        // Con el modo debug activado ponemos el color de fondo en rojo
        if (debug){
            toolbar.setBackgroundColor(Color.parseColor("#ff1a1a"));
        } else {
            toolbar.setBackgroundColor(Color.parseColor("#e9af30"));
        }

        // Obtenemos la posición desde donde se realiza el fichaje
        locationManager = (LocationManager) getSystemService(LOCATION_SERVICE);
        locationListener = this;

        // Asignamos las vistas
        btnFichar = (ImageButton) findViewById(R.id.btnNfc);

        // Damos funcionalidad a los eventos de pulsación
        btnFichar.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                if (location != null) {
                    try {
                        locationManager.removeUpdates(locationListener);
                    } catch (SecurityException e) {
                        SecurityDialog.createDialogPrincipal(context);
                        return;
                    }
                    fichar();
                } else {
                    doCheck = true;
                    progreso = new ProgressDialog(context);
                    progreso.setProgressStyle(ProgressDialog.STYLE_SPINNER);
                    progreso.setTitle("Esperando ubicación");
                    progreso.setMessage("Si este proceso tarda demasiado compruebe sus ajustes de localización.");
                    progreso.setIndeterminate(true);
                    progreso.setCanceledOnTouchOutside(false);
                    progreso.setCancelable(false);
                    progreso.show();

                    startTimeOut();
                }
            }
        });

        txtPress = (TextView) findViewById(R.id.txtPress);

        scrollView = (ScrollView) findViewById(R.id.scrollView);

        handleIntent(getIntent());
    }

    @Override
    protected void onResume() {
        locationManager = (LocationManager) getSystemService(LOCATION_SERVICE);

        try {
            locationManager.requestLocationUpdates(LocationManager.GPS_PROVIDER, 1, 0, this);
            locationManager.requestLocationUpdates(LocationManager.NETWORK_PROVIDER, 1, 0, this);
        } catch (SecurityException e) {
            SecurityDialog.createDialogPrincipal(context);
            return;
        }
        super.onResume();
    }

    @Override
    public void onLocationChanged(Location location) {
        if (location != null) {
            this.location = location;
            currentLatitude = location.getLatitude();
            currentLongitude = location.getLongitude();
            if (doCheck){
                searchingLocation = false;
                progreso.dismiss();
                try {
                    locationManager.removeUpdates(locationListener);
                } catch (SecurityException e) {
                    SecurityDialog.createDialogPrincipal(context);
                    return;
                }
                fichar();
            }
        }
    }

    @Override
    public void onProviderDisabled(String provider) {}

    @Override
    public void onProviderEnabled(String provider) {}

    @Override
    public void onStatusChanged(String provider, int status, Bundle extras) {}

    @Override
    public void onBackPressed() {
        Intent i = new Intent(NFCCheck.this, Principal.class);
        startActivity(i);
        finish();
    }

    /*public void makeTextViewHyperlink(TextView txt) {
        SpannableStringBuilder ssb = new SpannableStringBuilder();
        ssb.append(txt.getText());
        ssb.setSpan(new URLSpan("#"), 0, ssb.length(), Spanned.SPAN_EXCLUSIVE_EXCLUSIVE);
        txt.setText(ssb, TextView.BufferType.SPANNABLE);
    }*/

    // Convirtiendo un binario a string
    private static String bin2hex(byte[] data) {
        return String.format("%0" + (data.length * 2) + "X", new BigInteger(1, data));
    }

    private void handleIntent(Intent intent) {
        NfcAdapter mNfcAdapter = NfcAdapter.getDefaultAdapter(this);

        if ( (mNfcAdapter != null) && (mNfcAdapter.isEnabled()) && NfcAdapter.ACTION_NDEF_DISCOVERED.equals(getIntent().getAction())) {
            Tag tag = intent.getParcelableExtra(NfcAdapter.EXTRA_TAG);

            ServerGet tarea = new ServerGet(this);
            tarea.delegate = this;
            tarea.execute(getResources().getString(R.string.apiServiceTagMobileGet) + bin2hex(tag.getId()));

            route = "Mobile/ControlPresence/v1/Check";
        } else if (intent.getStringExtra("tagId") != null){
            ServerGet tarea = new ServerGet(this);
            tarea.delegate = this;
            tarea.execute(getResources().getString(R.string.apiServiceTagMobileGet) + intent.getStringExtra("tagId"));

            route = "Mobile/ControlPresence/v1/Check";
        }
    }

    @Override
    public void onAsyncFinish(HashMap<String, String> map) {
        LayoutInflater inflater = (LayoutInflater) this.getSystemService(LAYOUT_INFLATER_SERVICE);
        TextView txtName;

        if (HandleServerError.Handle(map, this, this)) return ;

        if (map.get("json").equals("{\"data\":[]}")) {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);

            builder.setMessage("La etiqueta que ha escaneado no está registrada").setTitle("Lectura fallida");
            builder.setCancelable(false);
            builder.setPositiveButton("Ok", new DialogInterface.OnClickListener() {
                public void onClick(DialogInterface dialog, int id) {
                    dialog.dismiss();
                    Intent i = new Intent(NFCCheck.this, Principal.class);
                    startActivity(i);
                    finish();
                }
            });

            builder.show();
        } else if (map.get("route").contains(getResources().getString(R.string.apiControlPresenceMobileCheck))){
            ControlPresenceCheckResult res = CustomGson.getGson().fromJson(map.get("json"), ControlPresenceCheckResult.class);

            HandleCheck.Handle(saveTrack, this, this, res);
        } else if (map.get("route").contains(getResources().getString(R.string.apiServiceTagMobileGet))){
            TagResult = CustomGson.getGson().fromJson(map.get("json"), ServiceTagGetResult.class).Data.get(0);

            if (TagResult.Items.size() == 0) {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);

                builder.setMessage("La etiqueta que ha escaneado no está vinculada a ningún item").setTitle("Lectura fallida");
                builder.setCancelable(false);
                builder.setPositiveButton("Ok", new DialogInterface.OnClickListener() {
                    public void onClick(DialogInterface dialog, int id) {
                        dialog.dismiss();
                        Intent i = new Intent(NFCCheck.this, Principal.class);
                        startActivity(i);
                        finish();
                    }
                });

                builder.show();
            } else {
                btnFichar.setVisibility(View.VISIBLE);
                txtPress.setVisibility(View.VISIBLE);

                PresenceArguments = new ControlPresenceCheckArguments();
                PresenceArguments.Id = TagResult.Id;

                Forms = new ArrayList<>();

                for (ServiceTagGetResult.Item item : TagResult.Items) {
                    ControlPresenceCheckArguments.Item itemArgument = new ControlPresenceCheckArguments.Item();
                    itemArgument.Id = item.Id;
                    itemArgument.CheckPointId = item.CheckPointId;

                    if (item.Plannings.size() > 0) {
                        ServiceTagGetResult.Planning plan = item.Plannings.get(0);
                        itemArgument.CheckId = plan.CheckId;

                        ServiceTagGetResult.Item formItem = new ServiceTagGetResult.Item();

                        formItem.Id = item.Id;
                        formItem.Plannings.add(plan);

                        Forms.add(formItem);
                    }

                    PresenceArguments.Items.add(itemArgument);

                    if (item.SaveFacialRecognition) saveFacial = true;
                    if (item.SaveTrack) saveTrack = true;

                    View childLayout = inflater.inflate(R.layout.check_subview, (ViewGroup) findViewById(R.id.fichaje_subview), false);
                    scrollView.addView(childLayout);

                    txtName = (TextView) childLayout.findViewById(R.id.txtName);

                    txtName.setText(item.Name);

                    createItemSpinner(item, childLayout);
                }

                if (saveTrack) {
                    checkGPS();
                }
            }
        }
    }

    private void createItemSpinner(ServiceTagGetResult.Item item, View childLayout) {
        Spinner spiPlans = (Spinner) childLayout.findViewById(R.id.spiPlans);
        FichajeSpinnerAdapter adapter = new FichajeSpinnerAdapter(getApplication(), item);

        spiPlans.setAdapter(adapter);

        spiPlans.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parentView, View selectedItemView, int position, long id) {
                TextView txtItemId = (TextView) selectedItemView.findViewById(R.id.txtItemId);
                TextView txtCheckId = (TextView) selectedItemView.findViewById(R.id.txtCheckId);
                ServiceTagGetResult.Item resItem = null;
                ControlPresenceCheckArguments.Item argItem = null;

                int itemId = Integer.parseInt(txtItemId.getText().toString());

                for (ServiceTagGetResult.Item auxItem : TagResult.Items) {
                    if (itemId == auxItem.Id) {
                        resItem = auxItem;
                        break;
                    }
                }
                if (resItem == null) return;

                for (ControlPresenceCheckArguments.Item auxItem : PresenceArguments.Items) {
                    if (itemId == auxItem.Id) {
                        argItem = auxItem;
                        break;
                    }
                }
                if (argItem == null) return;

                for (ServiceTagGetResult.Item auxItem : Forms) {
                    if (itemId == auxItem.Id) {
                        Forms.remove(auxItem);
                        break;
                    }
                }

                Integer checkId = IntegerExpansion.getIntegerValueOf(txtCheckId.getText().toString());

                if (checkId == null) {
                    argItem.CheckId = null;
                } else {
                    for (ServiceTagGetResult.Planning plan : resItem.Plannings) {
                        if (plan.Id == checkId) {
                            argItem.CheckId = plan.CheckId;

                            ServiceTagGetResult.Item formItem = new ServiceTagGetResult.Item();

                            formItem.Id = resItem.Id;
                            formItem.Plannings.add(plan);

                            Forms.add(formItem);

                            break;
                        }
                    }
                }
            }

            @Override
            public void onNothingSelected(AdapterView<?> parentView) {
                // your code here
            }

        });
    }

    private void fichar(){
        boolean haveForms = false;

        CheckTrack.Check(this);

        TimeZone tz = TimeZone.getTimeZone("UTC");
        DateFormat df = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss'Z'", Locale.getDefault());
        df.setTimeZone(tz);

        for (ServiceTagGetResult.Item item : Forms) {
            for (ServiceTagGetResult.Planning plan : item.Plannings){
                if (plan.Assigns.size() > 0){
                    haveForms = true;
                }
            }
        }

        PresenceArguments.Latitude = currentLatitude;
        PresenceArguments.Longitude = currentLongitude;
        PresenceArguments.Date = df.format(Calendar.getInstance().getTime());

        if (haveForms) {
            Intent i = new Intent(NFCCheck.this, NFCFormulario.class);

            i.putExtra("route", route);
            i.putExtra("arguments", PresenceArguments);
            i.putExtra("forms", Forms);
            i.putExtra("saveFacial", saveFacial);
            i.putExtra("saveTrack", saveTrack);

            startActivity(i);
        } else if (saveFacial) {
            Intent i = new Intent(NFCCheck.this, NFCFoto.class);

            i.putExtra("route", route);
            i.putExtra("arguments", PresenceArguments);
            i.putExtra("saveTrack", saveTrack);

            startActivity(i);
        } else {
            ServerPost task = new ServerPost(context);
            task.delegate = delegate;
            task.execute(route, CustomGson.getGson().toJson(PresenceArguments));
        }
    }

    private void checkGPS(){
        if (! ((LocationManager) getSystemService(Context.LOCATION_SERVICE)).isProviderEnabled(LocationManager.GPS_PROVIDER) ){
            AlertDialog.Builder builder = new AlertDialog.Builder(this);

            builder.setMessage("Para poder hacer track necesitas tener el GPS activado").setTitle("Active el GPS");

            builder.setPositiveButton("Ir a ajustes", new DialogInterface.OnClickListener() {
                public void onClick(DialogInterface dialog, int id) {
                    startActivity(new Intent(Settings.ACTION_LOCATION_SOURCE_SETTINGS));
                    dialog.dismiss();
                }
            });

            builder.setNegativeButton("Cancelar", new DialogInterface.OnClickListener() {
                @Override
                public void onClick(DialogInterface dialog, int which) {
                    Intent i = new Intent(NFCCheck.this, Principal.class);
                    startActivity(i);
                    dialog.dismiss();
                    finish();
                }
            });

            builder.setCancelable(false);

            builder.show();
        }
    }

    private void startTimeOut(){
        searchingLocation = true;
        new AsyncTimeOut(this).execute();
    }

    private void onTimeOutEnd(){
        currentLatitude = null;
        currentLongitude = null;

        progreso.dismiss();
        try {
            locationManager.removeUpdates(locationListener);
        } catch (SecurityException e) {
            SecurityDialog.createDialogPrincipal(context);
            return;
        }

        AlertDialog.Builder builder = new AlertDialog.Builder(context);

        builder.setMessage("No ha sido posible determinar su localización, ¿desea fichar de todas formas?").setTitle("Fichar sin localización");

        builder.setPositiveButton(R.string.yes, new DialogInterface.OnClickListener() {
            public void onClick(DialogInterface dialog, int id) {
                fichar();
            }
        });
        builder.setNegativeButton(R.string.no, new DialogInterface.OnClickListener() {
            public void onClick(DialogInterface dialog, int id) {
                startActivity(new Intent(NFCCheck.this, Principal.class));
                finish();
            }
        });

        builder.create().show();
    }

    public static class AsyncTimeOut extends AsyncTask<Void, Void, Void> {
        private long antes;
        private final int SECOND = 1000;
        private NFCCheck Caller;

        public AsyncTimeOut(NFCCheck caller) {
            Caller = caller;
            antes = Calendar.getInstance().getTimeInMillis();
        }

        @Override
        protected Void doInBackground(Void... params) {
            long ahora;
            int tenSeconds = 10 * SECOND;
            while (true) {
                ahora = Calendar.getInstance().getTimeInMillis();

                if (antes + tenSeconds < ahora) {
                    break;
                }
            }

            return null;
        }

        @Override
        protected void onPostExecute(Void s) {
            if (Caller.searchingLocation)
                Caller.onTimeOutEnd();
        }
    }

    public static class FichajeSpinnerAdapter extends BaseAdapter {
        private static LayoutInflater inflador = null;
        private ServiceTagGetResult.Item item;

        public FichajeSpinnerAdapter(Context contexto, ServiceTagGetResult.Item item){
            if (inflador == null){
                inflador = (LayoutInflater) contexto.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
            }
            this.item = item;
        }

        public View getView(int posicion, View vistaReciclada, ViewGroup padre) {
            if (vistaReciclada == null) {
                vistaReciclada = inflador.inflate(R.layout.check_subview_spinner_items, padre, false);
            }

            TextView txtItemId = (TextView) vistaReciclada.findViewById(R.id.txtItemId);
            TextView txtCheckId = (TextView) vistaReciclada.findViewById(R.id.txtCheckId);
            TextView txtInfo = (TextView) vistaReciclada.findViewById(R.id.txtInfo);

            txtItemId.setVisibility(View.VISIBLE);
            txtItemId.setText("" + item.Id);
            txtItemId.setVisibility(View.GONE);

            if (posicion == this.getCount() - 1){
                if (item.PresenceType == PresenceType.ENTRANCE)
                    txtInfo.setText("Fichar entrada");
                else if (item.PresenceType == PresenceType.EXIT)
                    txtInfo.setText("Fichar salida");
                else if (item.PresenceType == PresenceType.ROUND)
                    txtInfo.setText("Fichar");
            } else {
                ServiceTagGetResult.Planning plan = item.Plannings.get(posicion);
                String date = "";
                Calendar cal = Calendar.getInstance();
                SimpleDateFormat sdfUTC = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss", Locale.getDefault()), sdfLocal = new SimpleDateFormat("EEE, d MMM yyyy HH:mm:ss", Locale.getDefault());
                sdfUTC.setTimeZone(TimeZone.getTimeZone("UTC"));

                txtCheckId.setVisibility(View.VISIBLE);
                txtCheckId.setText("" + plan.Id);
                txtCheckId.setVisibility(View.GONE);

                try {
                    cal.setTimeInMillis(sdfUTC.parse(plan.Date).getTime());
                    date = sdfLocal.format(cal.getTime());
                } catch (ParseException e) {
                    e.printStackTrace();
                }

                if (plan.PresenceType == PresenceType.ENTRANCE)
                    txtInfo.setText("Entrada " + date);
                else if (plan.PresenceType == PresenceType.EXIT)
                    txtInfo.setText("Salida " + date);
                else if (plan.PresenceType == PresenceType.ROUND)
                    txtInfo.setText("Fichar " + date);
            }

            return vistaReciclada;
        }

        public int getCount() {
            return item.Plannings.size() + 1;
        }

        public Object getItem(int posicion) {
            if (posicion == item.Plannings.size())
                return item;
            return item.Plannings.get(posicion);
        }

        public long getItemId(int posicion) {
            if (posicion == item.Plannings.size())
                return item.Id;
            return item.Plannings.get(posicion).Id;
        }
    }
}


