package com.mobile.application.payin.views;

import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.graphics.Point;
import android.os.Bundle;
import android.support.v4.content.res.ResourcesCompat;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.CardView;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.support.v7.widget.Toolbar;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.WindowManager;
import android.widget.LinearLayout;
import android.widget.TextView;

import com.android.application.payin.R;
import com.mobile.application.payin.common.interfaces.AsyncResponse;
import com.mobile.application.payin.common.states.PaymentMediaState;
import com.mobile.application.payin.common.types.PaymentMediaType;
import com.mobile.application.payin.common.utilities.CustomGson;
import com.mobile.application.payin.dto.arguments.TicketPayArguments;
import com.mobile.application.payin.dto.results.TicketMobileGetResult;

import java.util.ArrayList;

public class CreditCardList extends AppCompatActivity  {
    private static RecyclerView RecyclerViewCards = null;

    private static Context context = null;

    private static TicketPayArguments args;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.creditcard_list);

        context = this;

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

        TextView txtInfo = (TextView) findViewById(R.id.txtTotalCards);
        RecyclerViewCards = (RecyclerView) findViewById(R.id.RecyclerViewCards);

        txtInfo.setText(getResources().getString(R.string.step1));

        RecyclerViewCards.setHasFixedSize(true);
        RecyclerView.LayoutManager mLayoutManager = new LinearLayoutManager(context);
        RecyclerViewCards.setLayoutManager(mLayoutManager);

        handleIntent();

        findViewById(R.id.fab_image_button_tarjetas).setVisibility(View.GONE);
    }

    private void handleIntent(){
        args = new TicketPayArguments(context);
        args.Id = getIntent().getIntExtra("id", 0);

        ArrayList<TicketMobileGetResult.PaymentMedia> lista = CustomGson.getGson().fromJson(getIntent().getStringExtra("paymentMedias"), TicketMobileGetResult.class).PaymentMedias;

        for (TicketMobileGetResult.PaymentMedia payment : lista) {
            if (payment.State != PaymentMediaState.Active)
                lista.remove(payment);
        }

        if (lista.size() == 0) {
            android.app.AlertDialog.Builder builder = new android.app.AlertDialog.Builder(context);
            builder.setTitle("Se requiere información")
                    .setMessage("Previamente a poder realizar un pago necesitas crear una tarjeta.")
                    .setPositiveButton("Confirmar", new DialogInterface.OnClickListener() {
                        public void onClick(DialogInterface dialog, int id) {
                            startActivity(new Intent(CreditCardList.this, CreateTarjeta.class));
                        }
                    })
                    .setNegativeButton("Cerrar", new DialogInterface.OnClickListener() {
                        @Override
                        public void onClick(DialogInterface dialog, int which) {
                            Intent i = new Intent(CreditCardList.this, Principal.class);
                            i.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                            startActivity(i);
                            dialog.dismiss();
                        }
                    });
            android.app.AlertDialog alert = builder.create();
            alert.show();
        } else {
            RecyclerView.Adapter mAdapter = new Adaptador(lista);
            RecyclerViewCards.setAdapter(mAdapter);
        }
    }

    public static class Adaptador extends RecyclerView.Adapter<Adaptador.ViewHolder>  {
        private final ArrayList<TicketMobileGetResult.PaymentMedia> lista;

        public Adaptador(ArrayList<TicketMobileGetResult.PaymentMedia> lista) {
            this.lista = lista;
        }

        @Override
        public Adaptador.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
            View v = LayoutInflater.from(parent.getContext()).inflate(R.layout.fragment_tarjetas_card, parent, false);

            CardView cv = (CardView) v.findViewById(R.id.card_view);

            LinearLayout.LayoutParams lp = (LinearLayout.LayoutParams) cv.getLayoutParams();
            Point point = new Point();
            ((WindowManager) context.getSystemService(Context.WINDOW_SERVICE)).getDefaultDisplay().getSize(point);
            lp.height = (point.x - 10) * 60/90;

            return new ViewHolder(v);
        }

        public void onBindViewHolder(ViewHolder holder, int position) {
            TicketMobileGetResult.PaymentMedia pm = lista.get(position);

            holder.paymentMedia = pm;
            holder.context = context;

            holder.txtCardName.setText(pm.Title);
            switch (pm.Type) {
                case PaymentMediaType.VISA:
                    holder.txtCardType.setText("VISA");
                    break;
                case PaymentMediaType.MASTERCARD:
                    holder.txtCardType.setText("MasterCard");
                    break;
                case PaymentMediaType.AMERICANEXPRESS:
                    holder.txtCardType.setText("American Express");
                    break;
                case PaymentMediaType.WEBCARD:
                    holder.txtCardType.setText("WebCard");
                    break;
            }
            holder.txtCardBank.setText(pm.BankEntity);
            holder.txtCardNumber.setText(pm.NumberHash);
            if (pm.ExpirationMonth != null && pm.ExpirationYear != null) {
                holder.txtCardExpire.setText(String.format("CAD %02d/%02d", pm.ExpirationMonth, pm.ExpirationYear));
            }
        }

        public int getItemCount() {
            return lista.size();
        }

        public static class ViewHolder extends RecyclerView.ViewHolder {
            public final TextView txtCardName;
            public final TextView txtCardBank;
            public final TextView txtCardExpire;
            public final TextView txtCardType;
            public final TextView txtCardState;
            public final TextView txtCardNumber;

            public TicketMobileGetResult.PaymentMedia paymentMedia;
            public Context context;
            public AsyncResponse delegate;

            public ViewHolder(View v) {
                super(v);
                txtCardName = (TextView) v.findViewById(R.id.txtCardName);
                txtCardType = (TextView) v.findViewById(R.id.txtCardType);
                txtCardBank = (TextView) v.findViewById(R.id.txtCardBank);
                txtCardExpire = (TextView) v.findViewById(R.id.txtCardExpire);
                txtCardState = (TextView) v.findViewById(R.id.txtCardState);
                txtCardNumber = (TextView) v.findViewById(R.id.txtCardNumber);

                v.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View v) {
                        args.PaymentMediaId = paymentMedia.Id;

                        Intent i = new Intent(context, TicketPay.class);
                        i.putExtra("args", CustomGson.getGson().toJson(args, TicketPayArguments.class));
                        i.putExtra("paymentMedia", CustomGson.getGson().toJson(paymentMedia, TicketMobileGetResult.PaymentMedia.class));
                        context.startActivity(i);
                    }
                });
            }
        }
    }
}
