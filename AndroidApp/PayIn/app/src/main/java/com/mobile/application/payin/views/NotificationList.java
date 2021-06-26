package com.mobile.application.payin.views;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.os.Bundle;
import android.support.v4.content.res.ResourcesCompat;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.support.v7.widget.Toolbar;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;

import com.android.application.payin.R;
import com.mobile.application.payin.common.interfaces.AsyncResponse;
import com.mobile.application.payin.common.serverconnections.ServerGet;
import com.mobile.application.payin.common.types.NotificationType;
import com.mobile.application.payin.common.utilities.CustomGson;
import com.mobile.application.payin.common.utilities.HandleServerError;
import com.mobile.application.payin.dto.results.NotificationGetAllResult;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Locale;
import java.util.TimeZone;

public class NotificationList extends AppCompatActivity implements AsyncResponse{
    private static RecyclerView rvNotifications = null;
    private static RecyclerView.Adapter mAdapter;

    private Context context;
    private AsyncResponse delegate;

    private ArrayList<NotificationGetAllResult.Notification> list;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.notification_list);

        context = this;
        delegate = this;

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

        rvNotifications = (RecyclerView) findViewById(R.id.RecyclerViewCards);

        rvNotifications.setHasFixedSize(true);
        RecyclerView.LayoutManager mLayoutManager = new LinearLayoutManager(context);
        rvNotifications.setLayoutManager(mLayoutManager);

        ServerGet tarea = new ServerGet(context);
        tarea.delegate = delegate;
        tarea.execute(getString(R.string.apiNotificationGetAll));
    }

    @Override
    public void onAsyncFinish(HashMap<String, String> map) {
        if (HandleServerError.Handle(map, this, this)) return ;

        if (map.get("route").contains(getString(R.string.apiNotificationGetAll))) {
            list = CustomGson.getGson().fromJson(map.get("json"), NotificationGetAllResult.class).Data;

            mAdapter = new Adaptador(context, delegate, list);
            rvNotifications.setAdapter(mAdapter);
        }
    }

    public static class Adaptador extends RecyclerView.Adapter<Adaptador.ViewHolder>  {
        private final Context context;
        private final AsyncResponse delegate;
        private final ArrayList<NotificationGetAllResult.Notification> lista;

        public Adaptador(Context context, AsyncResponse delegate, ArrayList<NotificationGetAllResult.Notification> lista) {
            this.context = context;
            this.delegate = delegate;
            this.lista = lista;
        }

        @Override
        public Adaptador.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
            View v = LayoutInflater.from(parent.getContext()).inflate(R.layout.notification_list_card, parent, false);

            return new ViewHolder(v);
        }

        public void onBindViewHolder(ViewHolder holder, int position) {
            holder.notification = lista.get(position);

            holder.context = context;
            holder.delegate = delegate;

            SimpleDateFormat sdfUTC = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss'Z'", Locale.getDefault()), sdfLocal = new SimpleDateFormat("dd/MM/yyyy HH:mm", Locale.getDefault());
            sdfUTC.setTimeZone(TimeZone.getTimeZone("UTC"));

            try {
                holder.txtDate.setText(sdfLocal.format(sdfUTC.parse(lista.get(position).Date).getTime()));
            } catch (ParseException e) {
                holder.txtDate.setText(lista.get(position).Date);
            }
            holder.txtInfo.setText(lista.get(position).Message);

            if (lista.get(position).IsRead) holder.ivBell.setVisibility(View.INVISIBLE);
            else holder.ivBellGrey.setVisibility(View.INVISIBLE);
        }

        public int getItemCount() {
            return lista.size();
        }

        public static class ViewHolder extends RecyclerView.ViewHolder {
            public final TextView txtDate;
            public final TextView txtInfo;
            public final ImageView ivBell;
            public final ImageView ivBellGrey;

            public Context context;
            public AsyncResponse delegate;

            public NotificationGetAllResult.Notification notification;

            public ViewHolder(View v) {
                super(v);
                txtDate = (TextView) v.findViewById(R.id.txtDate);
                txtInfo = (TextView) v.findViewById(R.id.txtInfo);
                ivBell = (ImageView) v.findViewById(R.id.ivBell);
                ivBellGrey = (ImageView) v.findViewById(R.id.ivBellGrey);

                v.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View v) {
                        Intent i;
                        switch (notification.Type) {
                            case NotificationType.PaymentMediaCreateSucceed:
                            case NotificationType.PaymentMediaCreateError:
                                i = new Intent(context, Principal.class);
                                i.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                                context.startActivity(i);
                                break;
                            case NotificationType.PaymentSucceed:
                            case NotificationType.PaymentFail:
                            case NotificationType.RefundSucceed:
                            case NotificationType.RefundFail:
                                i = new Intent(context, TicketCute.class);
                                i.putExtra("id", notification.ReferenceId);
                                i.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                                context.startActivity(i);
                                break;
                            case NotificationType.ConcessionVinculation:
                            case NotificationType.ConcessionVinculationAccepted:
                            case NotificationType.ConcessionVinculationRefused:
                            case NotificationType.ConcessionDissociation:
                                i = new Intent(context, EnterpriseList.class);
                                i.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                                context.startActivity(i);
                                break;
                        }
                    }
                });
            }
        }
    }
}
