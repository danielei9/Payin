package com.mobile.application.payin.views;

import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.os.Bundle;
import android.support.v4.content.res.ResourcesCompat;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.View;
import android.webkit.WebChromeClient;
import android.webkit.WebSettings;
import android.webkit.WebView;
import android.webkit.WebViewClient;
import android.widget.Button;
import android.widget.EditText;

import com.android.application.payin.R;
import com.mobile.application.payin.common.interfaces.AsyncResponse;
import com.mobile.application.payin.common.serverconnections.ServerPost;
import com.mobile.application.payin.common.utilities.CustomGson;
import com.mobile.application.payin.common.utilities.HandleServerError;
import com.mobile.application.payin.dto.arguments.PaymentMediaMobileCreateWebCardArguments;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.HashMap;

public class CreateTarjeta extends AppCompatActivity implements AsyncResponse {
    private EditText etBank, etCardName, etPin;

    private Context context;
    private AsyncResponse delegate;
    private SharedPreferences pref;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.create_tarjeta);

        context = this;
        delegate = this;

        Toolbar toolbar = (Toolbar) findViewById(R.id.tool_bar);
        setSupportActionBar(toolbar);
        toolbar.setNavigationIcon(ResourcesCompat.getDrawable(getResources(), R.drawable.ic_action_back, null));
        toolbar.setNavigationOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(CreateTarjeta.this, Principal.class);
                startActivity(intent);
                finish();
            }
        });

        pref = getSharedPreferences(getResources().getString(R.string.prefs), MODE_PRIVATE);
        if (pref.getBoolean("debug", false)) toolbar.setBackgroundColor(Color.parseColor("#ff1a1a"));
        else toolbar.setBackgroundColor(Color.parseColor("#e9af30"));

        etBank = (EditText) findViewById(R.id.etBank);
        etCardName = (EditText) findViewById(R.id.etCardName);
        etPin = (EditText) findViewById(R.id.etPin);

        Button btnCancel = (Button) findViewById(R.id.btnCancel),
            btnSend = (Button) findViewById(R.id.btnSend);

        btnCancel.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                finish();
            }
        });

        btnSend.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (!etPin.getText().toString().matches("\\d{4}")) {
                    AlertDialog.Builder builder = new android.app.AlertDialog.Builder(context);
                    builder.setTitle("Error en la introducción de datos")
                            .setMessage("El código PIN ha de consistir en cuatro caracteres numéricos.")
                            .setPositiveButton(getString(R.string.button_ok), new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialog, int id) {
                                    dialog.dismiss();
                                }
                            });
                    android.app.AlertDialog alert = builder.create();
                    alert.show();
                } else {
                    AlertDialog.Builder builder = new android.app.AlertDialog.Builder(context)
                            .setTitle("Confirmación")
                            .setMessage("Para comprobar la autenticidad de la tarjeta, Pay[in] procederá a realizar el cobro de 1€ que será devuelto de inmediato en el momento todo esté verificado.")
                            .setPositiveButton(getString(R.string.button_ok), new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialog, int id) {
                                    PaymentMediaMobileCreateWebCardArguments args = new PaymentMediaMobileCreateWebCardArguments(context);

                                    args.Name = etCardName.getText().toString();
                                    args.Pin = etPin.getText().toString();
                                    args.BankEntity = etBank.getText().toString();

                                    ServerPost tarea = new ServerPost(context);
                                    tarea.delegate = delegate;
                                    tarea.execute(getString(R.string.apiPaymentMediaCreate), CustomGson.getGson().toJson(args));
                                }
                            });
                    AlertDialog alert = builder.create();
                    alert.show();
                }
            }
        });
    }

    @Override
    public void onAsyncFinish(HashMap<String, String> map) {
        if (HandleServerError.Handle(map, this, this)) return ;

        String query;

        if (map.get("route").contains(getString(R.string.apiPaymentMediaCreate))) {
            try {
                JSONObject json = new JSONObject(map.get("json"));
                query = json.getString("request");
            } catch (JSONException e) {
                query = "";
            }

            findViewById(R.id.scrollView).setVisibility(View.GONE);

            WebView wWBank = (WebView) findViewById(R.id.wWBank);

            wWBank.clearCache(true);

            wWBank.setWebViewClient(new WebViewClient());
            WebSettings webSettings = wWBank.getSettings();
            webSettings.setJavaScriptEnabled(true);
            webSettings.setDomStorageEnabled(true);
            wWBank.setWebChromeClient(new WebChromeClient() {
                @Override
                public void onCloseWindow(WebView w) {
                    super.onCloseWindow(w);
                    finish();
                }
            });

            if (pref.getBoolean("debug", false)) wWBank.postUrl(getString(R.string.sabadell_url_debug), query.getBytes());
            else wWBank.postUrl(getString(R.string.sabadell_url), query.getBytes());

            wWBank.setVisibility(View.VISIBLE);
        }
    }
}
