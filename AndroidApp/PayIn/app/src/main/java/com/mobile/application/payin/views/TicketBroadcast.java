package com.mobile.application.payin.views;

import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.nfc.NdefMessage;
import android.nfc.NdefRecord;
import android.nfc.NfcAdapter;
import android.nfc.NfcEvent;
import android.os.Bundle;
import android.support.v4.content.res.ResourcesCompat;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.support.v7.widget.Toolbar;
import android.view.View;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import com.android.application.payin.R;
import com.mobile.application.payin.common.utilities.QR;
import com.mobile.application.payin.dto.arguments.TicketMobileCreateArguments;
import com.mobile.application.payin.dto.results.PaymentConcessionGetAll;

import java.nio.charset.Charset;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Locale;
import java.util.TimeZone;

public class TicketBroadcast extends AppCompatActivity implements NfcAdapter.CreateNdefMessageCallback {

    private int ticketId;
    private TicketMobileCreateArguments ticket;
    private PaymentConcessionGetAll.PaymentConcession paymentConcession;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        SimpleDateFormat sdf = new SimpleDateFormat("dd/MM/yyyy HH:mm", Locale.getDefault()),
                sdfUTC = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss'Z'", Locale.getDefault());
        sdfUTC.setTimeZone(TimeZone.getTimeZone("UTC"));
        Calendar now = Calendar.getInstance();

        super.onCreate(savedInstanceState);
        setContentView(R.layout.ticket_qr);

        Toolbar toolbar = (Toolbar) findViewById(R.id.tool_bar);
        setSupportActionBar(toolbar);
        toolbar.setNavigationIcon(ResourcesCompat.getDrawable(getResources(), R.drawable.ic_action_back, null));
        toolbar.setNavigationOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(TicketBroadcast.this, Principal.class);
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

        ImageView ivQR = (ImageView) findViewById(R.id.iv_qr);

        TextView txtAmount = (TextView) findViewById(R.id.txtAmount);
        TextView txtSupplierName = (TextView) findViewById(R.id.txtSupplierName);
        TextView txtSupplierAddr = (TextView) findViewById(R.id.txtSupplierAddr);
        TextView txtSupplierNumber = (TextView) findViewById(R.id.txtSupplierNumber);
        TextView txtSupplierPhone = (TextView) findViewById(R.id.txtSupplierPhone);
        TextView txtDate = (TextView) findViewById(R.id.txtDate);
        TextView txtTitle = (TextView) findViewById(R.id.txtTitle);
        TextView txtPrice = (TextView) findViewById(R.id.txtPrice);

        txtAmount.setText(String.format("%.02f€", ticket.Amount));
        txtPrice.setText(String.format("%.02f€", ticket.Amount));
        txtTitle.setText(ticket.Title);
        txtSupplierName.setText(paymentConcession.Name);
        txtSupplierAddr.setText(paymentConcession.Address);
        txtSupplierNumber.setText(paymentConcession.Cif);
        txtSupplierPhone.setText(paymentConcession.Phone);

        try {
            now.setTimeInMillis(sdfUTC.parse(ticket.Date).getTime());
            txtDate.setText(sdf.format(now.getTime()));
        } catch (ParseException e) {
            txtDate.setText(ticket.Date);
        }

        QR.generate(this, ivQR, "ticket", "{\"id\":" + ticketId + "}");
        broadcastNFC();
    }

    private void handleIntent(){
        Intent i = getIntent();

        ticketId = i.getIntExtra("ticketId", 0);
        ticket = (TicketMobileCreateArguments) i.getSerializableExtra("ticket");
        paymentConcession = (PaymentConcessionGetAll.PaymentConcession) i.getSerializableExtra("concession");
    }

    private void broadcastNFC(){
        NfcAdapter mNfcAdapter = NfcAdapter.getDefaultAdapter(this);
        if (mNfcAdapter == null) {
            Toast.makeText(this, "NFC is not available", Toast.LENGTH_LONG).show();
            return;
        }

        mNfcAdapter.setNdefPushMessageCallback(this, this);
    }

    @Override
    public NdefMessage createNdefMessage(NfcEvent event) {
        String text = "" + ticketId;
        return new NdefMessage(
                new NdefRecord[] {
                        createMimeRecord(text.getBytes())
                }
        );
    }

    private NdefRecord createMimeRecord(byte[] payload) {
        byte[] mimeBytes = "pay[in]/ticket:".getBytes(Charset.forName("US-ASCII"));
        return new NdefRecord(NdefRecord.TNF_MIME_MEDIA, mimeBytes, new byte[0], payload);
    }
}
