package com.mobile.application.payin.views;

import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.os.Bundle;
import android.support.v4.content.ContextCompat;
import android.support.v4.content.res.ResourcesCompat;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.support.v7.widget.Toolbar;
import android.text.InputFilter;
import android.text.InputType;
import android.util.TypedValue;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;

import com.android.application.payin.R;
import com.mobile.application.payin.common.interfaces.AsyncResponse;
import com.mobile.application.payin.common.serverconnections.ServerGet;
import com.mobile.application.payin.common.serverconnections.ServerPost;
import com.mobile.application.payin.common.utilities.CustomGson;
import com.mobile.application.payin.common.utilities.HandleServerError;
import com.mobile.application.payin.common.utilities.QR;
import com.mobile.application.payin.dto.arguments.PaymentRefundArguments;
import com.mobile.application.payin.dto.results.TicketMobileGetResult;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Locale;
import java.util.TimeZone;

public class TicketCute extends AppCompatActivity implements AsyncResponse {
    private TextView txtAmount, txtTitle, txtDate, txtPrice,
            txtSupplierName, txtSupplierAddr, txtSupplierNumber, txtSupplierPhone, txtWorkerName;
    private ImageView ivQr;
    private static RecyclerView RecyclerViewPayments = null;
    private static RecyclerView.Adapter mAdapter;

    private Context context;
    private AsyncResponse delegate;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.ticket_qr);

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
        delegate = this;

        SharedPreferences pref = getSharedPreferences(getResources().getString(R.string.prefs), MODE_PRIVATE);

        Boolean debug = pref.getBoolean("debug", false);
        if (debug) toolbar.setBackgroundColor(Color.parseColor("#ff1a1a"));
        else toolbar.setBackgroundColor(Color.parseColor("#e9af30"));

        ServerGet tarea = new ServerGet(this);
        tarea.delegate = this;
        tarea.execute(getResources().getString(R.string.apiTicketGet) + "/" + getIntent().getIntExtra("id", 0));

        View ticketSubView = findViewById(R.id.ticket_subview);
        ivQr = (ImageView) findViewById(R.id.iv_qr);

        txtSupplierName = (TextView) ticketSubView.findViewById(R.id.txtSupplierName);
        txtSupplierAddr = (TextView) ticketSubView.findViewById(R.id.txtSupplierAddr);
        txtSupplierNumber = (TextView) ticketSubView.findViewById(R.id.txtSupplierNumber);
        txtSupplierPhone = (TextView) ticketSubView.findViewById(R.id.txtSupplierPhone);
        txtDate = (TextView) ticketSubView.findViewById(R.id.txtDate);
        txtTitle = (TextView) ticketSubView.findViewById(R.id.txtTitle);
        txtAmount = (TextView) ticketSubView.findViewById(R.id.txtAmount);
        txtPrice = (TextView) ticketSubView.findViewById(R.id.txtPrice);
        txtWorkerName = (TextView) findViewById(R.id.txtWorkerName);

        RecyclerViewPayments = (RecyclerView) findViewById(R.id.RecyclerViewPayments);
        RecyclerViewPayments.setHasFixedSize(true);
        RecyclerView.LayoutManager mLayoutManager = new LinearLayoutManager(this);
        RecyclerViewPayments.setLayoutManager(mLayoutManager);
        RecyclerViewPayments.setFocusable(false);
    }

    @Override
    public void onAsyncFinish(HashMap<String, String> map) {
        if (HandleServerError.Handle(map, this, this)) return ;

        if (map.get("route").contains(getResources().getString(R.string.apiPaymentRefund))) {
            AlertDialog.Builder alert = new AlertDialog.Builder(context)
                    .setTitle("Devolución")
                    .setMessage("Devolución realizada con éxito.")
                    .setPositiveButton(R.string.button_ok, new DialogInterface.OnClickListener() {
                        @Override
                        public void onClick(DialogInterface dialog, int which) {
                            dialog.dismiss();
                            finish();
                        }
                    });
            alert.show();
        } if (map.get("route").contains(getResources().getString(R.string.apiTicketGet))) {
            final TicketMobileGetResult.Ticket ticket = CustomGson.getGson().fromJson(map.get("json"), TicketMobileGetResult.class).Data.get(0);
            SimpleDateFormat sdfUTC = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss'Z'", Locale.getDefault()), sdfLocal = new SimpleDateFormat("dd/MM/yyyy HH:mm", Locale.getDefault());
            sdfUTC.setTimeZone(TimeZone.getTimeZone("UTC"));

            txtSupplierName.setText(ticket.SupplierName);
            txtSupplierAddr.setText(ticket.SupplierAddress);
            txtSupplierNumber.setText(ticket.SupplierNumber);
            txtSupplierPhone.setText(ticket.SupplierPhone);
            try {
                txtDate.setText("Id. " + ticket.Id + "-" + sdfLocal.format(sdfUTC.parse(ticket.Date).getTime()));
            } catch (ParseException e) {
                txtDate.setText("Id. " + ticket.Id + "-" + ticket.Date);
            }
            txtTitle.setText(ticket.Title);
            txtAmount.setText(String.format("%.02f€", ticket.Amount));
            txtPrice.setText(String.format("%.02f€", ticket.Amount));

            if (ticket.WorkerName != null)
                txtWorkerName.setText(String.format(getString(R.string.worker) , ticket.WorkerName));
            else
                txtWorkerName.setVisibility(View.GONE);

            QR.generate(this, ivQr, "ticket", "{\"id\":" + ticket.Id + "}");

            if (ticket.Payments.size() == 0) {
                RecyclerViewPayments.setVisibility(View.GONE);
            } else {
                float px = TypedValue.applyDimension(TypedValue.COMPLEX_UNIT_DIP, 70, getResources().getDisplayMetrics());
                RecyclerViewPayments.getLayoutParams().height = (int) (px * ticket.Payments.size());
                mAdapter = new Adaptador(ticket.Payments, ticket.CanReturn, context, delegate);
                RecyclerViewPayments.setAdapter(mAdapter);
            }

            if (ticket.PayedAmount >= ticket.Amount) {
                findViewById(R.id.txtPayed).setVisibility(View.VISIBLE);
            } else if (!ticket.CanReturn && ticket.PayedAmount < ticket.Amount) {
                findViewById(R.id.btnPay).setVisibility(View.VISIBLE);
                ((Button) findViewById(R.id.btnPay)).setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View v) {
                        Intent i = new Intent(TicketCute.this, CreditCardList.class);
                        i.putExtra("id", ticket.Id);
                        startActivity(i);
                    }
                });
            }
        }
    }

    public static class Adaptador extends RecyclerView.Adapter<Adaptador.ViewHolder> {
        private final Context context;
        private final AsyncResponse delegate;
        private final ArrayList<TicketMobileGetResult.Payment> lista;
        private final boolean canReturn;

        private final SimpleDateFormat sdfUTC = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss'Z'", Locale.getDefault());
        private final SimpleDateFormat sdfLocal = new SimpleDateFormat("dd/MM/yyyy HH:mm", Locale.getDefault());

        public Adaptador(ArrayList<TicketMobileGetResult.Payment> lista, boolean canReturn, Context context, AsyncResponse delegate) {
            this.context = context;
            this.delegate = delegate;
            this.lista = lista;
            this.canReturn = canReturn;

            sdfUTC.setTimeZone(TimeZone.getTimeZone("UTC"));
        }

        @Override
        public Adaptador.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
            View v = LayoutInflater.from(parent.getContext()).inflate(R.layout.ticket_payment_item, parent, false);

            return new ViewHolder(v);
        }

        public void onBindViewHolder(ViewHolder holder, int position) {
            TicketMobileGetResult.Payment payment = lista.get(position);

            holder.context = context;
            holder.delegate = delegate;
            holder.payment = payment;
            holder.canReturn = canReturn;

            holder.txtUserLogin.setText(payment.UserName);
            holder.txtCardName.setText(payment.PaymentMediaName);
            if (payment.State == 2) holder.txtAmount.setText("Error");
            else holder.txtAmount.setText(String.format("%.02f€", payment.Amount));
            try {
                holder.txtDate.setText("Id. " + payment.Id + "-" + sdfLocal.format(sdfUTC.parse(payment.Date).getTime()));
            } catch (ParseException e) {
                holder.txtDate.setText("Id. " + payment.Id + "-" + payment.Date);
            }

            //if (!(canReturn && payment.CanBeReturned)) holder.disable();
        }

        public int getItemCount() {
            return lista.size();
        }

        public static class ViewHolder extends RecyclerView.ViewHolder {
            public final View v;

            public final TextView txtUserLogin;
            public final TextView txtCardName;
            public final TextView txtAmount;
            public final TextView txtDate;

            public Context context;
            public AsyncResponse delegate;
            public TicketMobileGetResult.Payment payment;
            public boolean canReturn;

            public ViewHolder(View v) {
                super(v);

                this.v = v;

                txtUserLogin = (TextView) v.findViewById(R.id.txtUserLogin);
                txtCardName = (TextView) v.findViewById(R.id.txtCardName);
                txtAmount = (TextView) v.findViewById(R.id.txtAmount);
                txtDate = (TextView) v.findViewById(R.id.txtDate);

                v.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View v) {
                        if (canReturn && payment.CanBeReturned) {
                            AlertDialog.Builder alertDialog = new AlertDialog.Builder(context);
                            alertDialog.setTitle("Devolución");
                            alertDialog.setMessage(String.format(
                                    "Para proceder con la devolución del pago de %s con un importe de %.02f€ debe introducir su PIN de Pay[in]:",
                                    payment.UserName, payment.Amount));

                            LinearLayout layout = new LinearLayout(context);
                            layout.setOrientation(LinearLayout.VERTICAL);
                            final EditText input = new EditText(context);
                            input.setInputType(InputType.TYPE_CLASS_NUMBER|InputType.TYPE_NUMBER_VARIATION_PASSWORD);
                            input.setFilters(new InputFilter[]{new InputFilter.LengthFilter(4)});

                            LinearLayout.LayoutParams lp = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MATCH_PARENT, LinearLayout.LayoutParams.MATCH_PARENT);
                            lp.setMargins(16, 0, 16, 0);

                            layout.addView(input, lp);
                            alertDialog.setView(layout);

                            alertDialog.setPositiveButton(context.getString(R.string.btnSend), new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialog, int which) {
                                    PaymentRefundArguments args = new PaymentRefundArguments(context);

                                    args.Id = payment.Id;
                                    args.Pin = input.getText().toString();

                                    ServerPost tarea = new ServerPost(context);
                                    tarea.delegate = delegate;
                                    tarea.execute(context.getResources().getString(R.string.apiPaymentRefund), CustomGson.getGson().toJson(args, PaymentRefundArguments.class));
                                    dialog.dismiss();
                                }
                            });

                            alertDialog.setNegativeButton(R.string.button_cancel, new DialogInterface.OnClickListener() {
                                @Override
                                public void onClick(DialogInterface dialog, int which) {
                                    dialog.dismiss();
                                }
                            });

                            AlertDialog dialog = alertDialog.create();
                            dialog.setCanceledOnTouchOutside(true);
                            dialog.show();
                        }
                    }
                });
            }

            public void disable() {
                v.setBackgroundColor(ContextCompat.getColor(context, R.color.ColorPrimaryDark));
            }
        }
    }
}
