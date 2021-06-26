package com.mobile.application.payin.views;

import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import com.android.application.payin.R;
import com.mobile.application.payin.dto.results.MainMobileGetAllResult;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Locale;
import java.util.TimeZone;

public class FragmentPagos extends Fragment {
    private static RecyclerView RecyclerViewCharges = null;
    private static RecyclerView.Adapter mAdapter;

    private static Context context;

    private static ArrayList<MainMobileGetAllResult.Ticket> lista = null;

    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View fragmentView = inflater.inflate(R.layout.fragment_pagos,container,false);

        context = getActivity();

        RecyclerViewCharges = (RecyclerView) fragmentView.findViewById(R.id.RecyclerViewCharges);
        RecyclerViewCharges.setHasFixedSize(true);
        RecyclerView.LayoutManager mLayoutManager = new LinearLayoutManager(context);
        RecyclerViewCharges.setLayoutManager(mLayoutManager);

        if (lista != null) {
            mAdapter = new Adaptador(lista);
            RecyclerViewCharges.setAdapter(mAdapter);
        }

        return fragmentView;
    }

    public static void info(ArrayList<MainMobileGetAllResult.Ticket> l, Context cont){
        lista = l;
        context = cont;
        if (RecyclerViewCharges != null) {
            mAdapter = new Adaptador(lista);
            RecyclerViewCharges.setAdapter(mAdapter);
        }
    }

    public static class Adaptador extends RecyclerView.Adapter<Adaptador.ViewHolder> {
        private final ArrayList<MainMobileGetAllResult.Ticket> lista;
        private final SimpleDateFormat sdfUTC = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss'Z'", Locale.getDefault());
        private final SimpleDateFormat sdfLocal = new SimpleDateFormat("dd/MM/yyyy HH:mm", Locale.getDefault());

        public Adaptador(ArrayList<MainMobileGetAllResult.Ticket> lista) {
            this.lista = lista;
            sdfUTC.setTimeZone(TimeZone.getTimeZone("UTC"));
        }

        @Override
        public Adaptador.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
            View v = LayoutInflater.from(parent.getContext()).inflate(R.layout.fragment_pagos_list_items, parent, false);

            return new ViewHolder(v);
        }

        public void onBindViewHolder(ViewHolder holder, int position) {
            holder.payment = lista.get(position);

            holder.txtPayedAmount.setText(String.format("%.02f€", lista.get(position).PayedAmount));
            holder.txtAmount.setText(String.format("de %.02f€", lista.get(position).Amount));
            holder.txtSupplierName.setText(lista.get(position).SupplierName);
            holder.txtTitle.setText(lista.get(position).Title);
            try {
                holder.txtDate.setText(sdfLocal.format(sdfUTC.parse(lista.get(position).Date).getTime()));
            } catch (ParseException e) {
                holder.txtDate.setText(lista.get(position).Date);
            }
        }

        public int getItemCount() {
            return lista.size();
        }

        public static class ViewHolder extends RecyclerView.ViewHolder {
            public final TextView txtPayedAmount;
            public final TextView txtAmount;
            public final TextView txtSupplierName;
            public final TextView txtTitle;
            public final TextView txtDate;

            public MainMobileGetAllResult.Ticket payment;

            public ViewHolder(View v) {
                super(v);
                txtPayedAmount = (TextView) v.findViewById(R.id.txtPayedAmount);
                txtAmount = (TextView) v.findViewById(R.id.txtAmount);
                txtSupplierName = (TextView) v.findViewById(R.id.txtSupplierName);
                txtTitle = (TextView) v.findViewById(R.id.txtTitle);
                txtDate = (TextView) v.findViewById(R.id.txtDate);

                v.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View v) {
                        final int ticketId = payment.Id;
                        AlertDialog.Builder builder = new AlertDialog.Builder(context);
                        builder.setTitle("Vista detallada")
                                .setMessage("¿Desea ver más detalles del ticket?")
                                .setPositiveButton(context.getResources().getString(R.string.yes), new DialogInterface.OnClickListener() {
                                    public void onClick(DialogInterface dialog, int id) {
                                        Intent i = new Intent(context, TicketCute.class);
                                        i.putExtra("id", ticketId);
                                        context.startActivity(i);
                                        dialog.dismiss();
                                    }
                                })
                                .setNegativeButton(R.string.no, new DialogInterface.OnClickListener() {
                                    @Override
                                    public void onClick(DialogInterface dialog, int which) {
                                        dialog.dismiss();
                                    }
                                })
                                .setCancelable(true);
                        AlertDialog alert = builder.create();
                        alert.setCanceledOnTouchOutside(true);
                        alert.show();
                    }
                });
            }
        }
    }
}
