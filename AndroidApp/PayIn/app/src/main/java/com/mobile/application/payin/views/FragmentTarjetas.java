package com.mobile.application.payin.views;

import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.graphics.Point;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.support.v7.widget.CardView;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.MotionEvent;
import android.view.View;
import android.view.ViewGroup;
import android.view.WindowManager;
import android.widget.ImageButton;
import android.widget.LinearLayout;
import android.widget.TextView;

import com.android.application.payin.R;
import com.mobile.application.payin.common.interfaces.AsyncResponse;
import com.mobile.application.payin.common.serverconnections.ServerDelete;
import com.mobile.application.payin.common.serverconnections.ServerGet;
import com.mobile.application.payin.common.states.PaymentMediaState;
import com.mobile.application.payin.common.types.PaymentMediaType;
import com.mobile.application.payin.common.utilities.HandleServerError;
import com.mobile.application.payin.dto.results.MainMobileGetAllResult;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.HashMap;

public class FragmentTarjetas extends Fragment implements AsyncResponse {
    private static TextView txtTotalCards = null;
    private static RecyclerView RecyclerViewCards = null;
    private static RecyclerView.Adapter mAdapter;

    private static Context context = null;
    private static AsyncResponse delegate;

    private static ArrayList<MainMobileGetAllResult.PaymentMedia> lista = null;

    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View v = inflater.inflate(R.layout.fragment_tarjetas,container,false);

        context = getActivity();
        delegate = this;

        ImageButton imgButtonFAB = (ImageButton) v.findViewById(R.id.fab_image_button_tarjetas);
        imgButtonFAB.setOnClickListener(new View.OnClickListener()
        {
            @Override
            public void onClick(View v)
            {
                ServerGet tarea = new ServerGet(context);
                tarea.delegate = delegate;
                tarea.execute(getString(R.string.apiUserHasPayment));
            }
        });

        txtTotalCards = (TextView) v.findViewById(R.id.txtTotalCards);
        RecyclerViewCards = (RecyclerView) v.findViewById(R.id.RecyclerViewCards);

        if (lista != null) {
            txtTotalCards.setText(getString(R.string.total_cards) + " " + lista.size());
        }

        RecyclerViewCards.setHasFixedSize(true);
        RecyclerView.LayoutManager mLayoutManager = new LinearLayoutManager(context);
        RecyclerViewCards.setLayoutManager(mLayoutManager);

        if (lista != null) {
            mAdapter = new Adaptador(getActivity(), delegate, lista);
            RecyclerViewCards.setAdapter(mAdapter);
        }

        return v;
    }

    @Override
    public void onAsyncFinish(HashMap<String, String> map) {
        if (HandleServerError.Handle(map, getActivity(), context)) return ;

        boolean hasPayment;

        if (map.get("route").contains(getString(R.string.apiUserHasPayment))) {
            try {
                JSONObject json = new JSONObject(map.get("json"));
                hasPayment = json.getBoolean("hasPayment");
            } catch (JSONException e) {
                hasPayment = false;
            }

            if (hasPayment)
                startActivity(new Intent(getActivity(), CreateTarjeta.class));
            else {
                android.app.AlertDialog.Builder builder = new android.app.AlertDialog.Builder(context);
                builder.setTitle("Se requiere información")
                        .setMessage("Previamente a poder crear tarjetas debe definir su usuario para utilizar pagos.")
                        .setPositiveButton("Confirmar", new DialogInterface.OnClickListener() {
                            public void onClick(DialogInterface dialog, int id) {
                                startActivity(new Intent(getActivity(), PaymentsUserCreate.class));
                                dialog.dismiss();
                            }
                        });
                android.app.AlertDialog alert = builder.create();
                alert.show();
            }
        } else if (map.get("route").contains(getString(R.string.apiPaymentMediaDelete))){
            getActivity().recreate();
        }
    }

    public static void info(ArrayList<MainMobileGetAllResult.PaymentMedia> l, Context cont){
        lista = l;
        context = cont;
        if (txtTotalCards != null) {
            txtTotalCards.setText(context.getString(R.string.total_cards) + " " + lista.size());
        }
        if (RecyclerViewCards != null) {
            mAdapter = new Adaptador(context, delegate, lista);
            RecyclerViewCards.setAdapter(mAdapter);
        }
    }

    public static class Adaptador extends RecyclerView.Adapter<Adaptador.ViewHolder>  {
        private final Context context;
        private final AsyncResponse delegate;
        private final ArrayList<MainMobileGetAllResult.PaymentMedia> lista;

        public Adaptador(Context context, AsyncResponse delegate, ArrayList<MainMobileGetAllResult.PaymentMedia> lista) {
            this.context = context;
            this.delegate = delegate;
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
            MainMobileGetAllResult.PaymentMedia pm = lista.get(position);

            holder.paymentMediaId = pm.Id;
            holder.context = context;
            holder.delegate = delegate;

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
            switch (pm.State) {
                case PaymentMediaState.Pending:
                    holder.txtCardState.setText("No activada");
                    holder.txtCardState.setTextColor(context.getResources().getColor(R.color.ColorAmber));
                    break;
                case PaymentMediaState.Active:
                    holder.txtCardState.setText("Activa");
                    holder.txtCardState.setTextColor(context.getResources().getColor(R.color.ColorGreen));
                    break;
                default:
                    break;
            }
        }

        public int getItemCount() {
            return lista.size();
        }

        public static class ViewHolder extends RecyclerView.ViewHolder {
            public final TextView txtCardName;
            public final TextView txtCardType;
            public final TextView txtCardExpire;
            public final TextView txtCardBank;
            public final TextView txtCardState;
            public final TextView txtCardNumber;

            public int paymentMediaId;
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

                v.setOnLongClickListener(new View.OnLongClickListener() {
                    @Override
                    public boolean onLongClick(View v) {
                        AlertDialog.Builder builder = new AlertDialog.Builder(context);
                        builder.setTitle("Eliminar tarjeta")
                                .setMessage("¿Está seguro de que desea eliminar esta tarjeta?")
                                .setPositiveButton(context.getResources().getString(R.string.yes), new DialogInterface.OnClickListener() {
                                    public void onClick(DialogInterface dialog, int id) {
                                        ServerDelete tarea = new ServerDelete(context);
                                        tarea.delegate = delegate;
                                        tarea.execute(context.getResources().getString(R.string.apiPaymentMediaDelete) + "/" + paymentMediaId, "{}");
                                        dialog.dismiss();
                                    }
                                })
                                .setNegativeButton(context.getResources().getString(R.string.no), new DialogInterface.OnClickListener() {
                                    @Override
                                    public void onClick(DialogInterface dialog, int which) {
                                        dialog.dismiss();
                                    }
                                });
                        AlertDialog alert = builder.create();
                        alert.show();
                        return true;
                    }
                });
            }
        }
    }
}
