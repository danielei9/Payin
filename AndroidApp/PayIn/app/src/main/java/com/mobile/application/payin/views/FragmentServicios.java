package com.mobile.application.payin.views;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.res.Resources;
import android.os.AsyncTask;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v4.content.ContextCompat;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.GridView;
import android.widget.TextView;

import com.android.application.payin.R;
import com.mobile.application.payin.common.utilities.GridViewAdapter;
import com.mobile.application.payin.common.utilities.GridViewItem;
import com.mobile.application.payin.dto.results.MainMobileGetAllResult;

import java.util.ArrayList;
import java.util.Calendar;
import java.util.List;

public class FragmentServicios extends Fragment implements AdapterView.OnItemClickListener {
    private static GridViewAdapter mAdapter;
    private static GridView gridView = null;
    private static TextView txtWorkHours;

    private static Context context;
    private static Resources resources;

    private static final List<GridViewItem> mItems = new ArrayList<>();
    private static boolean update = false;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        resources = getResources();
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View fragmentView = inflater.inflate(R.layout.fragment_servicios, container, false);

        mAdapter = new GridViewAdapter(getActivity(), mItems);

        gridView = (GridView) fragmentView.findViewById(R.id.gridView);
        gridView.setAdapter(mAdapter);
        gridView.setOnItemClickListener(this);

        return fragmentView;
    }

    @Override
    public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
        GridViewItem item = mItems.get(position);

        if (item.type == MainMobileGetAllResult.PinType.CONTROLPRESENCE) {
            Intent intent = new Intent(getActivity().getApplicationContext(), Calendario.class);
            startActivity(intent);
        /*} else if (item.title.equals("Pagar Ticket")) {
            AlertDialog.Builder alertDialog = new AlertDialog.Builder(context);
            alertDialog.setTitle("Seleccione el ticket");
            alertDialog.setMessage("Introduzca el Id del ticket que quiere pagar.");

            LinearLayout layout = new LinearLayout(context);
            layout.setOrientation(LinearLayout.VERTICAL);
            final EditText input = new EditText(context);
            input.setInputType(InputType.TYPE_NUMBER_VARIATION_PASSWORD);
            input.setFilters(new InputFilter[]{new InputFilter.LengthFilter(4)});

            LinearLayout.LayoutParams lp = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MATCH_PARENT, LinearLayout.LayoutParams.MATCH_PARENT);
            lp.setMargins(16, 0, 16, 0);

            layout.addView(input, lp);
            alertDialog.setView(layout);

            alertDialog.setPositiveButton(getString(R.string.btnSend), new DialogInterface.OnClickListener() {
                public void onClick(DialogInterface dialog, int which) {
                    Intent intent = new Intent(getActivity().getApplicationContext(), TicketReception.class);
                    intent.putExtra("id", Integer.parseInt(input.getText().toString()));

                    startActivity(intent);
                }
            });

            AlertDialog dialog = alertDialog.create();
            dialog.setCanceledOnTouchOutside(true);
            dialog.show();*/
        } else if (item.type == MainMobileGetAllResult.PinType.TICKET) {
            Intent intent = new Intent(getActivity().getApplicationContext(), TicketCreation.class);
            startActivity(intent);
        }
    }

    @Override
    public void onPause() {
        update = false;
        super.onPause();
    }

    public static void info(MainMobileGetAllResult res, TextView txtHours, Context cont){
        mItems.clear();

        context = cont;
        txtWorkHours = txtHours;

        SharedPreferences pref = context.getSharedPreferences(context.getResources().getString(R.string.prefs), Context.MODE_PRIVATE);

        for (MainMobileGetAllResult.Favorite item : res.Favorites) {
            if (item.Type == MainMobileGetAllResult.PinType.CONTROLPRESENCE)
                mItems.add(new GridViewItem(ContextCompat.getDrawable(context, R.drawable.ic_presence_calendar), resources.getString(R.string.control_presence), MainMobileGetAllResult.PinType.CONTROLPRESENCE));
            if (item.Type == MainMobileGetAllResult.PinType.TICKET)
                mItems.add(new GridViewItem(ContextCompat.getDrawable(context, R.drawable.ic_gen_ticket), "Generar Ticket", MainMobileGetAllResult.PinType.TICKET));
        }

        //mItems.add(new GridViewItem(ContextCompat.getDrawable(context, R.drawable.ic_gen_ticket), "Pagar Ticket", MainMobileGetAllResult.PinType.TICKET));

        if (pref.getBoolean("pref_show_hours", true)) {
            if (res.SumChecks.equals("00:00:00") && res.CheckDuration.equals("00:00:00")) {
                update = false;
                txtWorkHours.setVisibility(View.GONE);
            } else {
                if (res.Working) update = true;
                changeTxt(res.SumChecks, res.CheckDuration);
                txtWorkHours.setVisibility(View.VISIBLE);
            }
        } else {
            update = false;
            txtWorkHours.setVisibility(View.GONE);
        }

        if (gridView != null) {
            mAdapter.notifyDataSetChanged();
        }
    }

    private static void changeTxt(String presence, String planning){
        if (txtWorkHours != null) {
            txtWorkHours.setText(text(presence, planning));
            if (update) new AsyncHours(presence, planning).execute();
        }
    }

    private static String text(String presence, String planning){
        String res = "Trabajadas " + presence;
        if (!planning.equals("00:00:00"))
            res += " de " + planning;
        res += " horas.";

        return res;
    }

    public static class AsyncHours extends AsyncTask<Void, Void, Void>{
        private int hours, minutes, seconds;
        private String planning;
        private long antes;
        private final int SECOND = 1000;

        public AsyncHours(String presence, String planning) {
            String[] t = presence.split(":");
            hours = Integer.parseInt(t[0]);
            minutes = Integer.parseInt(t[1]);
            seconds = Integer.parseInt(t[2]);

            this.planning = planning;

            antes = Calendar.getInstance().getTimeInMillis();
        }

        @Override
        protected Void doInBackground(Void... params) {
            while (true) {
                long ahora = Calendar.getInstance().getTimeInMillis();

                if (antes + SECOND < ahora) {
                    antes = ahora;
                    seconds++;
                    if (seconds == 60) {
                        minutes++;
                        seconds = 0;
                    }
                    if (minutes == 60) {
                        hours++;
                        minutes = 0;
                    }
                    return null;
                }
            }
        }

        @Override
        protected void onPostExecute(Void s) {
            String presence = ((hours<10)?"0":"") + hours + ":" + ((minutes<10)?"0":"") + minutes + ":" + ((seconds<10)?"0":"") + seconds;
            changeTxt(presence, planning);
        }
    }
}
