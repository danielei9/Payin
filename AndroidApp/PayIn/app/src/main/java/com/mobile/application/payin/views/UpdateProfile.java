package com.mobile.application.payin.views;

import android.app.DatePickerDialog;
import android.app.Dialog;
import android.content.Context;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.v4.app.DialogFragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.content.res.ResourcesCompat;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.MotionEvent;
import android.view.View;
import android.widget.Button;
import android.widget.DatePicker;
import android.widget.EditText;
import android.widget.LinearLayout;

import com.android.application.payin.R;
import com.mobile.application.payin.common.interfaces.AsyncResponse;
import com.mobile.application.payin.common.serverconnections.ServerPut;
import com.mobile.application.payin.common.utilities.CustomGson;
import com.mobile.application.payin.common.utilities.HandleServerError;
import com.mobile.application.payin.dto.results.AccountGetResult;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.HashMap;
import java.util.Locale;

public class UpdateProfile  extends AppCompatActivity implements AsyncResponse {
    private LinearLayout ll;
    private EditText etUserPublicName, etUserPublicPhone,
            etUserTaxName, etUserTaxNumber, etUserTaxAddr;
    private static EditText etUserTaxBirth;
    private DialogFragment birthDialog;

    private Context context;
    private AsyncResponse delegate;
    private FragmentManager fm;

    private static AccountGetResult.User res;
    private static int year, month, day;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.update_profile);

        context = this;
        delegate = this;
        fm = getSupportFragmentManager();

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

        ll = (LinearLayout) findViewById(R.id.linearLayout);
        etUserPublicName = (EditText) findViewById(R.id.etUserPublicName);
        etUserPublicPhone = (EditText) findViewById(R.id.etUserPublicPhone);
        etUserTaxName = (EditText) findViewById(R.id.etUserTaxName);
        etUserTaxNumber = (EditText) findViewById(R.id.etUserTaxNumber);
        etUserTaxBirth = (EditText) findViewById(R.id.etUserTaxBirth);
        etUserTaxAddr = (EditText) findViewById(R.id.etUserTaxAddr);

        year = Calendar.getInstance().get(Calendar.YEAR);
        month = Calendar.getInstance().get(Calendar.MONTH) + 1;
        day = Calendar.getInstance().get(Calendar.DAY_OF_MONTH);

        handleIntent();

        etUserTaxBirth.setOnTouchListener(new View.OnTouchListener() {
            private boolean pressed;

            @Override
            public boolean onTouch(View v, MotionEvent event) {
                if (event.getAction() == MotionEvent.ACTION_DOWN) {
                    pressed = true;
                } else if (event.getAction() == MotionEvent.ACTION_UP && pressed) {
                    birthDialog = new DatePickerFragment();
                    birthDialog.show(fm, "datePicker");
                    etUserTaxBirth.clearFocus();
                } else {
                    pressed = false;
                }
                return true;
            }
        });

        etUserTaxBirth.setOnFocusChangeListener(new View.OnFocusChangeListener() {
            @Override
            public void onFocusChange(View v, boolean hasFocus) {
                if (hasFocus) {
                    birthDialog = new DatePickerFragment();
                    birthDialog.show(fm, "datePicker");
                    v.clearFocus();
                    ll.requestFocus();
                }
            }
        });

        Button btnSend = (Button) findViewById(R.id.btnSend);
        Button btnCancel = (Button) findViewById(R.id.btnCancel);

        btnSend.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                res.Name = etUserPublicName.getText().toString();
                res.Mobile = etUserPublicPhone.getText().toString();
                res.TaxName = etUserTaxName.getText().toString();
                res.TaxNumber = etUserTaxNumber.getText().toString();
                res.TaxAddress = etUserTaxAddr.getText().toString();

                ServerPut tarea = new ServerPut(context);
                tarea.delegate = delegate;
                tarea.execute(getString(R.string.apiAccount), CustomGson.getGson().toJson(res));
            }
        });

        btnCancel.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                finish();
            }
        });
    }

    private void handleIntent() {
        SimpleDateFormat sdfServer = new SimpleDateFormat("yyyy-MM-dd", Locale.getDefault()), sdf = new SimpleDateFormat("d MMM yyyy", Locale.getDefault());
        Calendar now = Calendar.getInstance();

        res = (AccountGetResult.User) getIntent().getSerializableExtra("profile");

        etUserPublicName.setText(res.Name);
        etUserPublicPhone.setText(res.Mobile);
        etUserTaxName.setText(res.TaxName);
        etUserTaxNumber.setText(res.TaxNumber);
        etUserTaxAddr.setText(res.TaxAddress);

        try {
            now.setTimeInMillis(sdfServer.parse(res.Birthday).getTime());
            etUserTaxBirth.setText(sdf.format(now.getTime()));
            year = now.get(Calendar.YEAR);
            month = now.get(Calendar.MONTH) + 1;
            day = now.get(Calendar.DAY_OF_MONTH);
        } catch (ParseException e) {
            etUserTaxBirth.setText(res.Birthday);
        }
    }

    @Override
    public void onAsyncFinish(HashMap<String, String> map) {
        HandleServerError.Handle(map, this, this);
    }

    public static class DatePickerFragment extends DialogFragment implements DatePickerDialog.OnDateSetListener {
        SimpleDateFormat sdfServer, sdf;

        @Override @NonNull
        public Dialog onCreateDialog( Bundle savedInstanceState) {
            sdfServer = new SimpleDateFormat("yyyy-MM-dd", Locale.getDefault());
            sdf = new SimpleDateFormat("d MMM yyyy", Locale.getDefault());

            return new DatePickerDialog(getActivity(), this, year, month, day);
        }

        public void onDateSet(DatePicker view, int dialogYear, int dialogMonth, int dialogDay) {
            Calendar now = Calendar.getInstance();

            year = dialogYear;
            month = dialogMonth + 1;
            day = dialogDay;

            String date = String.format("%04d-%02d-%02d", year, month, day);

            try {
                now.setTimeInMillis(sdfServer.parse(date).getTime());
                etUserTaxBirth.setText(sdf.format(now.getTime()));
            } catch (ParseException e) {
                etUserTaxBirth.setText(res.Birthday);
            }

            res.Birthday = date;
        }
    }
}
