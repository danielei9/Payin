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
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.support.v7.widget.Toolbar;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import com.android.application.payin.R;
import com.mobile.application.payin.common.interfaces.AsyncResponse;
import com.mobile.application.payin.common.serverconnections.ServerGet;
import com.mobile.application.payin.common.serverconnections.ServerPut;
import com.mobile.application.payin.common.utilities.CustomGson;
import com.mobile.application.payin.common.utilities.HandleServerError;
import com.mobile.application.payin.dto.results.WorkerGetAllResult;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.HashMap;

public class EnterpriseList extends AppCompatActivity implements AsyncResponse {
    private static RecyclerView RecyclerViewEnterprises = null;
    private static RecyclerView.Adapter mAdapter;

    private static  Context context;
    private static AsyncResponse delegate;

    private ArrayList<WorkerGetAllResult.Worker> list;
    private static AlertDialog.Builder builder;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.enterprise_list);

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

        RecyclerViewEnterprises = (RecyclerView) findViewById(R.id.RecyclerViewEnterprises);
        RecyclerViewEnterprises.setHasFixedSize(true);
        RecyclerView.LayoutManager mLayoutManager = new LinearLayoutManager(this);
        RecyclerViewEnterprises.setLayoutManager(mLayoutManager);

        ServerGet tarea = new ServerGet(this);
        tarea.delegate = this;
        tarea.execute(getResources().getString(R.string.apiWorker));
    }

    @Override
    public void onAsyncFinish(HashMap<String, String> map) {
        if (HandleServerError.Handle(map, this, this)) return ;

        if (map.get("route").contains(getString(R.string.apiUserHasPayment))) {
            boolean hasPayment;

            try {
                JSONObject json = new JSONObject(map.get("json"));
                hasPayment = json.getBoolean("hasPayment");
            } catch (JSONException e) {
                hasPayment = false;
            }

            if (hasPayment) {
                android.app.AlertDialog alert = builder.create();
                alert.setCanceledOnTouchOutside(true);
                alert.show();
            } else {
                android.app.AlertDialog.Builder builder = new android.app.AlertDialog.Builder(context);
                builder.setTitle("Se requiere información")
                        .setMessage("Previamente a poder vincularse a una empresa debe definir su usuario para utilizar pagos.")
                        .setPositiveButton("Confirmar", new DialogInterface.OnClickListener() {
                            public void onClick(DialogInterface dialog, int id) {
                                startActivity(new Intent(context, PaymentsUserCreate.class));
                                dialog.dismiss();
                            }
                        });
                android.app.AlertDialog alert = builder.create();
                alert.setCanceledOnTouchOutside(true);
                alert.show();
            }
        } else if (!map.get("route").equals(getResources().getString(R.string.apiWorker))) {
            recreate();
        } else if (map.get("route").contains(getResources().getString(R.string.apiWorker))) {
            list = CustomGson.getGson().fromJson(map.get("json"), WorkerGetAllResult.class).Data;
            mAdapter = new Adaptador(list);
            RecyclerViewEnterprises.setAdapter(mAdapter);
        }
    }

    public static class Adaptador extends RecyclerView.Adapter<Adaptador.ViewHolder> {
        private final ArrayList<WorkerGetAllResult.Worker> lista;

        public Adaptador(ArrayList<WorkerGetAllResult.Worker> lista) {
            this.lista = lista;
        }

        @Override
        public Adaptador.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
            View v = LayoutInflater.from(parent.getContext()).inflate(R.layout.enterprise_list_card, parent, false);

            return new ViewHolder(v);
        }

        public void onBindViewHolder(ViewHolder holder, int position) {
            holder.worker = lista.get(position);

            String state;
            int color;

            switch (lista.get(position).State) {
                case 1:
                    state = "Vinculada";
                    color = context.getResources().getColor(R.color.ColorGreen);
                    break;
                case 2:
                    state = "Pendiente";
                    color = context.getResources().getColor(R.color.ColorYellow);
                    break;
                case 3:
                    state = "Desvinculada";
                    color = context.getResources().getColor(R.color.ColorRed);
                    break;
                default:
                    state = "Eliminada";
                    color = context.getResources().getColor(R.color.ColorRed);
                    break;
            }

            holder.txtName.setText(lista.get(position).ConcessionName);
            holder.txtState.setText(state);
            holder.txtState.setTextColor(color);
        }

        public int getItemCount() {
            return lista.size();
        }

        public static class ViewHolder extends RecyclerView.ViewHolder {
            public final TextView txtName;
            public final TextView txtState;

            public WorkerGetAllResult.Worker worker;

            public ViewHolder(View v) {
                super(v);

                txtName = (TextView) v.findViewById(R.id.txtName);
                txtState = (TextView) v.findViewById(R.id.txtState);

                v.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View v) {
                        if (worker.Type == 2) {
                            ServerGet tarea = new ServerGet(context);
                            tarea.delegate = delegate;
                            tarea.execute(context.getString(R.string.apiUserHasPayment));
                        }

                        String state = worker.State == 1 ? "activa" : worker.State == 2 ? "pendiente" : "desactivada";

                        builder = new AlertDialog.Builder(context);

                        builder.setMessage("La vinculacion con la empresa " + worker.ConcessionName + " se encuentra " + state +".").setTitle("Vinculación");

                        if (worker.State != 1)
                            builder.setPositiveButton("Vincularse", new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialog, int id) {
                                        ServerPut tarea = new ServerPut(context);
                                        tarea.delegate = delegate;
                                        String url = "";

                                        if (worker.Type == 1)
                                            url = context.getString(R.string.apiServiceWorkerAccept);
                                        else if (worker.Type == 2)
                                            url = context.getString(R.string.apiPaymentWorkerAccept);

                                        tarea.execute(url + "/" + worker.Id, "{}");

                                        dialog.dismiss();
                                }
                            });

                        if (worker.State != 3 && worker.State != 0)
                            builder.setNeutralButton("Desvincularse", new DialogInterface.OnClickListener() {
                                @Override
                                public void onClick(DialogInterface dialog, int which) {
                                    ServerPut tarea = new ServerPut(context);
                                    tarea.delegate = delegate;
                                    String url = "";

                                    if      (worker.Type == 1) url = context.getString(R.string.apiServiceWorkerReject);
                                    else if (worker.Type == 2) url = context.getString(R.string.apiPaymentWorkerReject);

                                    tarea.execute(url + "/" + worker.Id, "{}");

                                    dialog.dismiss();
                                }
                            });

                        builder.setNegativeButton("Cancelar", new DialogInterface.OnClickListener() {
                            @Override
                            public void onClick(DialogInterface dialog, int which) {
                                dialog.dismiss();
                            }
                        });

                        if (worker.Type != 2) {
                            android.app.AlertDialog alert = builder.create();
                            alert.setCanceledOnTouchOutside(true);
                            alert.show();
                        }
                    }
                });
            }
        }
    }
}
