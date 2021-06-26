package com.mobile.application.payin.views;

import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.nfc.NdefMessage;
import android.nfc.NfcAdapter;
import android.os.Bundle;
import android.os.Parcelable;
import android.support.v4.content.res.ResourcesCompat;
import android.app.AlertDialog;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.support.v7.widget.Toolbar;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;

import com.android.application.payin.R;
import com.mobile.application.payin.common.interfaces.AsyncResponse;
import com.mobile.application.payin.common.serverconnections.ServerGet;
import com.mobile.application.payin.common.utilities.CustomGson;
import com.mobile.application.payin.common.utilities.HandleServerError;
import com.mobile.application.payin.dto.results.TicketMobileGetResult;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.HashMap;
import java.util.Locale;
import java.util.TimeZone;

public class TicketReception extends AppCompatActivity implements AsyncResponse {
    private TextView txtAmount, txtTitle, txtDate, txtPrice,
            txtSupplierName, txtSupplierAddr, txtSupplierNumber, txtSupplierPhone, txtWorkerName;
    private Button btnPay;

    private Context context;
    private AsyncResponse delegate;

    private int id;
    private TicketMobileGetResult paymentMedias;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.ticket_qr);

        context = this;
        delegate = this;

        Toolbar toolbar = (Toolbar) findViewById(R.id.tool_bar);
        setSupportActionBar(toolbar);
        toolbar.setNavigationIcon(ResourcesCompat.getDrawable(getResources(), R.drawable.ic_action_back, null));
        toolbar.setNavigationOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(TicketReception.this, Principal.class);
                startActivity(intent);
                finish();
            }
        });

        SharedPreferences pref = getSharedPreferences(getResources().getString(R.string.prefs), MODE_PRIVATE);

        Boolean debug = pref.getBoolean("debug", false);
        if (debug) toolbar.setBackgroundColor(Color.parseColor("#ff1a1a"));
        else toolbar.setBackgroundColor(Color.parseColor("#e9af30"));

        handleIntent();

        RecyclerView RecyclerViewPayments = (RecyclerView) findViewById(R.id.RecyclerViewPayments);
        RecyclerViewPayments.setHasFixedSize(true);
        RecyclerView.LayoutManager mLayoutManager = new LinearLayoutManager(this);
        RecyclerViewPayments.setLayoutManager(mLayoutManager);
        RecyclerViewPayments.setFocusable(false);
        RecyclerViewPayments.setVisibility(View.GONE);

        txtAmount = (TextView) findViewById(R.id.txtAmount);

        txtSupplierName = (TextView) findViewById(R.id.txtSupplierName);
        txtSupplierAddr = (TextView) findViewById(R.id.txtSupplierAddr);
        txtSupplierNumber = (TextView) findViewById(R.id.txtSupplierNumber);
        txtSupplierPhone = (TextView) findViewById(R.id.txtSupplierPhone);
        txtDate = (TextView) findViewById(R.id.txtDate);
        txtTitle = (TextView) findViewById(R.id.txtTitle);
        txtAmount = (TextView) findViewById(R.id.txtAmount);
        txtPrice = (TextView) findViewById(R.id.txtPrice);
        txtWorkerName = (TextView) findViewById(R.id.txtWorkerName);

        btnPay = (Button) findViewById(R.id.btnPay);
        btnPay.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent i = new Intent(TicketReception.this, CreditCardList.class);
                i.putExtra("id", id);
                i.putExtra("paymentMedias", CustomGson.getGson().toJson(paymentMedias, TicketMobileGetResult.class));
                startActivity(i);
            }
        });
    }

    private void handleIntent(){
        Intent intent = getIntent();
        int id;
        if (NfcAdapter.ACTION_NDEF_DISCOVERED.equals(intent.getAction())) {
            Parcelable[] rawMsgs = intent.getParcelableArrayExtra(NfcAdapter.EXTRA_NDEF_MESSAGES);

            NdefMessage msg = (NdefMessage) rawMsgs[0];

            id = Integer.parseInt(new String(msg.getRecords()[0].getPayload()));
        } else {
            id = intent.getIntExtra("id", 0);
        }

        ServerGet tarea = new ServerGet(this);
        tarea.delegate = delegate;
        tarea.execute(getResources().getString(R.string.apiTicketGet) + "/" + id);
    }

    @Override
    public void onAsyncFinish(HashMap<String, String> map) {
        if (HandleServerError.Handle(map, this, this)) return ;

        if (map.get("route").contains(getResources().getString(R.string.apiTicketPay))){
            AlertDialog.Builder builder = new AlertDialog.Builder(context);
            builder.setTitle("Operación correcta:");
            builder.setMessage("El ticket se ha abonado correctamente");
            builder.setPositiveButton(R.string.ok, new DialogInterface.OnClickListener() {
                @Override
                public void onClick(DialogInterface dialog, int which) {
                    dialog.dismiss();
                    startActivity(new Intent(TicketReception.this, Principal.class));
                    finish();
                }
            });
            builder.create().show();
        } else if (map.get("route").contains(getResources().getString(R.string.apiTicketGet))){
            paymentMedias = CustomGson.getGson().fromJson(map.get("json"), TicketMobileGetResult.class);
            SimpleDateFormat sdfUTC = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss'Z'", Locale.getDefault()), sdfLocal = new SimpleDateFormat("dd/MM/yyyy HH:mm", Locale.getDefault());
            sdfUTC.setTimeZone(TimeZone.getTimeZone("UTC"));

            TicketMobileGetResult.Ticket ticket = paymentMedias.Data.get(0);

            id = ticket.Id;

            txtSupplierName.setText(ticket.SupplierName);
            txtSupplierAddr.setText(ticket.SupplierAddress);
            txtSupplierNumber.setText(ticket.SupplierNumber);
            txtSupplierPhone.setText(ticket.SupplierPhone);
            try {
                txtDate.setText(sdfLocal.format(sdfUTC.parse(ticket.Date).getTime()));
            } catch (ParseException e) {
                txtDate.setText(ticket.Date);
            }
            txtAmount.setText(String.format("%.02f€", ticket.Amount));
            txtPrice.setText(String.format("%.02f€", ticket.Amount));
            txtTitle.setText(ticket.Title);

            if (ticket.PayedAmount >= ticket.Amount) {
                findViewById(R.id.txtPayed).setVisibility(View.VISIBLE);
            } else if (ticket.PayedAmount < ticket.Amount) {
                findViewById(R.id.btnPay).setVisibility(View.VISIBLE);
            }
            if (ticket.WorkerName != null)
                txtWorkerName.setText(String.format(getString(R.string.worker) , ticket.WorkerName));
            else
                txtWorkerName.setVisibility(View.GONE);

            if (!paymentMedias.HasPayment) {
                android.app.AlertDialog.Builder builder = new android.app.AlertDialog.Builder(context);
                builder.setTitle("Se requiere información")
                        .setMessage("Previamente a poder crear tarjetas debe definir su usuario para utilizar pagos.")
                        .setPositiveButton("Confirmar", new DialogInterface.OnClickListener() {
                            public void onClick(DialogInterface dialog, int id) {
                                startActivity(new Intent(TicketReception.this, PaymentsUserCreate.class));
                                dialog.dismiss();
                            }
                        })
                        .setNegativeButton("Cerrar", new DialogInterface.OnClickListener() {
                            @Override
                            public void onClick(DialogInterface dialog, int which) {
                                Intent i = new Intent(TicketReception.this, Principal.class);
                                i.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                                startActivity(i);
                                dialog.dismiss();
                            }
                        });
                android.app.AlertDialog alert = builder.create();
                alert.show();
            }
        }
    }
}
