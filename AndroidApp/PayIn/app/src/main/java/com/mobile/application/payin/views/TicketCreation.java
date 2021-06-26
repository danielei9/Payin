package com.mobile.application.payin.views;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.os.Bundle;
import android.support.v4.content.res.ResourcesCompat;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Spinner;

import com.android.application.payin.R;
import com.mobile.application.payin.common.interfaces.AsyncResponse;
import com.mobile.application.payin.common.serverconnections.ServerGet;
import com.mobile.application.payin.common.serverconnections.ServerPost;
import com.mobile.application.payin.common.utilities.CustomGson;
import com.mobile.application.payin.common.utilities.DoubleExpansion;
import com.mobile.application.payin.common.utilities.HandleServerError;
import com.mobile.application.payin.dto.arguments.TicketMobileCreateArguments;
import com.mobile.application.payin.dto.results.PaymentConcessionGetAll;

import org.json.JSONException;
import org.json.JSONObject;

import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.HashMap;
import java.util.Locale;
import java.util.TimeZone;

public class TicketCreation extends AppCompatActivity implements AsyncResponse{
    private EditText etReference, etTitle, etAmount;
    private Spinner spiCommerce;
    private Context context;
    private AsyncResponse delegate;

    private TicketMobileCreateArguments arg;
    private PaymentConcessionGetAll.PaymentConcession paymentConcession;

    private final HashMap<Integer, PaymentConcessionGetAll.PaymentConcession> items = new HashMap<>();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.ticket_creation);

        SimpleDateFormat sdf = new SimpleDateFormat("dd/MM/yyyy HH:mm", Locale.getDefault()),
                sdfUTC = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss'Z'", Locale.getDefault());
        sdfUTC.setTimeZone(TimeZone.getTimeZone("UTC"));

        arg = new TicketMobileCreateArguments();
        context = this;
        delegate = this;

        Toolbar toolbar = (Toolbar) findViewById(R.id.tool_bar);
        setSupportActionBar(toolbar);
        toolbar.setNavigationIcon(ResourcesCompat.getDrawable(getResources(), R.drawable.ic_action_back, null));
        toolbar.setNavigationOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(TicketCreation.this, Principal.class);
                startActivity(intent);
                finish();
            }
        });

        SharedPreferences pref = getSharedPreferences(getResources().getString(R.string.prefs), MODE_PRIVATE);

        Boolean debug = pref.getBoolean("debug", false);

        // Con el modo debug activado ponemos el color de fondo en rojo
        if (debug) toolbar.setBackgroundColor(Color.parseColor("#ff1a1a"));
        else toolbar.setBackgroundColor(Color.parseColor("#e9af30"));

        EditText etDate = (EditText) findViewById(R.id.etDate);
        spiCommerce = (Spinner) findViewById(R.id.spiCommerce);
        etReference = (EditText) findViewById(R.id.etReference);
        etTitle = (EditText) findViewById(R.id.etTitle);
        etAmount = (EditText) findViewById(R.id.etAmount);

        etDate.setText(sdf.format(Calendar.getInstance().getTime()));
        arg.Date = sdfUTC.format(Calendar.getInstance().getTime());
        spiCommerce.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                arg.ConcessionId = items.get(position).Id;
                paymentConcession = items.get(position);
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {

            }
        });

        Button btnSend = (Button) findViewById(R.id.btnSend),
            btnCancel = (Button) findViewById(R.id.btnCancel);

        btnSend.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                arg.Title = etTitle.getText().toString();
                if (arg.Title.equals("")) arg.Title = getResources().getString(R.string.varios);
                arg.Reference = etReference.getText().toString();
                arg.Amount = DoubleExpansion.getDoubleValueOf(etAmount.getText().toString());

                ServerPost tarea = new ServerPost(context);
                tarea.delegate = delegate;
                tarea.execute(getResources().getString(R.string.apiTicketCreate), CustomGson.getGson().toJson(arg));
            }
        });

        btnCancel.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent i = new Intent(TicketCreation.this, Principal.class);
                startActivity(i);
                finish();
            }
        });

        ServerGet tarea = new ServerGet(this);
        tarea.delegate = this;
        tarea.execute(getResources().getString(R.string.apiPaymentConcessionGetAll));
    }

    @Override
    public void onAsyncFinish(HashMap<String, String> map) {
        if (HandleServerError.Handle(map, this, this)) return ;

        if (map.get("route").contains(getResources().getString(R.string.apiPaymentConcessionGetAll))) {
            PaymentConcessionGetAll res = CustomGson.getGson().fromJson(map.get("json"), PaymentConcessionGetAll.class);

            String[] spinnerItems = new String[res.Data.size()];

            if (res.Data.size() == 0) return;

            PaymentConcessionGetAll.PaymentConcession item = res.Data.get(0);

            arg.ConcessionId = item.Id;
            paymentConcession = item;

            items.put(0, item);
            spinnerItems[0] = item.Name;

            for (int i = 1; i < res.Data.size(); i++){
                item = res.Data.get(i);

                items.put(i, item);
                spinnerItems[i] = item.Name;
            }

            ArrayAdapter<String> adapter = new ArrayAdapter<>(this, android.R.layout.simple_spinner_item, spinnerItems);

            spiCommerce.setAdapter(adapter);
        } else if (map.get("route").contains(getResources().getString(R.string.apiTicketCreate))){
            try {
                JSONObject jsonObj = new JSONObject(map.get("json"));

                Intent i = new Intent(TicketCreation.this, TicketBroadcast.class);
                i.putExtra("ticketId", jsonObj.getInt("id"));
                i.putExtra("ticket", arg);
                i.putExtra("concession", paymentConcession);
                startActivity(i);
            } catch (JSONException e) {
                e.printStackTrace();
            }
        }
    }
}
