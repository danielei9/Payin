package com.mobile.application.payin.views;

import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.os.Bundle;
import android.support.v4.content.res.ResourcesCompat;
import android.app.AlertDialog;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;

import com.android.application.payin.R;
import com.mobile.application.payin.common.interfaces.AsyncResponse;
import com.mobile.application.payin.common.serverconnections.ServerPost;
import com.mobile.application.payin.common.types.PaymentMediaType;
import com.mobile.application.payin.common.utilities.CustomGson;
import com.mobile.application.payin.common.utilities.HandleServerError;
import com.mobile.application.payin.dto.arguments.TicketPayArguments;
import com.mobile.application.payin.dto.results.TicketMobileGetResult;

import java.util.HashMap;

public class TicketPay extends AppCompatActivity implements AsyncResponse {
    private EditText etPin;

    private Context context = null;
    private AsyncResponse delegate;

    private static TicketPayArguments args;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.ticket_pay);

        context = this;
        delegate = this;

        args = CustomGson.getGson().fromJson(getIntent().getStringExtra("args"), TicketPayArguments.class);
        TicketMobileGetResult.PaymentMedia paymentMedia = CustomGson.getGson().fromJson(getIntent().getStringExtra("paymentMedia"), TicketMobileGetResult.PaymentMedia.class);

        Toolbar toolbar = (Toolbar) findViewById(R.id.tool_bar);
        setSupportActionBar(toolbar);
        toolbar.setNavigationIcon(ResourcesCompat.getDrawable(getResources(), R.drawable.ic_action_back, null));
        toolbar.setNavigationOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                finish();
            }
        });

        SharedPreferences pref = getSharedPreferences(getResources().getString(R.string.prefs), MODE_PRIVATE);

        Boolean debug = pref.getBoolean("debug", false);
        if (debug) toolbar.setBackgroundColor(Color.parseColor("#ff1a1a"));
        else toolbar.setBackgroundColor(Color.parseColor("#e9af30"));

        TextView txtCardType = (TextView) findViewById(R.id.txtCardType),
                txtCardName = (TextView) findViewById(R.id.txtCardName),
                txtCardNumber = (TextView) findViewById(R.id.txtCardNumber),
                txtCardBank = (TextView) findViewById(R.id.txtCardBank),
                txtCardExpire = (TextView) findViewById(R.id.txtCardExpire);
        etPin = (EditText) findViewById(R.id.etPin);
        Button btnSend = (Button) findViewById(R.id.btnSend);

        txtCardName.setText(paymentMedia.Title);
        switch (paymentMedia.Type) {
            case PaymentMediaType.VISA:
                txtCardType.setText("VISA");
                break;
            case PaymentMediaType.MASTERCARD:
                txtCardType.setText("MasterCard");
                break;
            case PaymentMediaType.AMERICANEXPRESS:
                txtCardType.setText("American Express");
                break;
            case PaymentMediaType.WEBCARD:
                txtCardType.setText("WebCard");
                break;
        }
        txtCardBank.setText(paymentMedia.BankEntity);
        txtCardNumber.setText(paymentMedia.NumberHash);
        if (paymentMedia.ExpirationMonth != null && paymentMedia.ExpirationYear != null) {
            txtCardExpire.setText(String.format("CAD %02d/%02d", paymentMedia.ExpirationMonth, paymentMedia.ExpirationYear));
        }

        btnSend.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                args.Pin = etPin.getText().toString();

                ServerPost tarea = new ServerPost(context);
                tarea.delegate = delegate;
                tarea.execute(getString(R.string.apiTicketPay), CustomGson.getGson().toJson(args, TicketPayArguments.class));
            }
        });
    }

    @Override
    public void onAsyncFinish(HashMap<String, String> map) {
        if (HandleServerError.Handle(map, this, this)) return;

        if (map.get("route").contains(getResources().getString(R.string.apiTicketPay))){
            AlertDialog.Builder builder = new AlertDialog.Builder(context);
            builder.setTitle("Operación correcta");
            builder.setMessage("El ticket se ha abonado correctamente");
            builder.setPositiveButton(getResources().getString(R.string.button_ok), new DialogInterface.OnClickListener() {
                @Override
                public void onClick(DialogInterface dialog, int which) {
                    dialog.dismiss();
                    Intent i =  new Intent(TicketPay.this, Principal.class);
                    i.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                    i.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TASK);
                    startActivity(i);
                    finish();
                }
            });
            builder.setCancelable(false);
            builder.create().show();
        }
    }
}
