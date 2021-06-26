package com.mobile.application.payin.views;

import android.app.DatePickerDialog;
import android.app.Dialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
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
import android.widget.Toast;

import com.android.application.payin.R;
import com.mobile.application.payin.common.interfaces.AsyncResponse;
import com.mobile.application.payin.common.serverconnections.ServerPost;
import com.mobile.application.payin.common.utilities.CustomGson;
import com.mobile.application.payin.common.utilities.HandleServerError;
import com.mobile.application.payin.dto.arguments.UserMobileCreateArguments;

import java.util.Calendar;
import java.util.HashMap;

public class PaymentsUserCreate extends AppCompatActivity implements AsyncResponse {
    private LinearLayout ll;
    private EditText etTaxName, etTaxAddress, etTaxNumber, etPin, etPinConfirm;
    private static EditText etBirth;
    private DialogFragment birthDialog;

    private FragmentManager fm;

    private static Context context;
    private AsyncResponse delegate;

    private static int year, month, day;
    private static String birthS;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.payments_user_create);

        context = this;
        delegate = this;

        fm = getSupportFragmentManager();

        Calendar c = Calendar.getInstance();
        year = c.get(Calendar.YEAR);
        month = c.get(Calendar.MONTH);
        day = c.get(Calendar.DAY_OF_MONTH);

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

        boolean debug = pref.getBoolean("debug", false);

        if (debug) toolbar.setBackgroundColor(Color.parseColor("#ff1a1a"));
        else toolbar.setBackgroundColor(Color.parseColor("#e9af30"));

        ll = (LinearLayout) findViewById(R.id.linearLayout);
        etTaxName = (EditText) findViewById(R.id.etTaxName);
        etTaxAddress = (EditText) findViewById(R.id.etTaxAddress);
        etTaxNumber = (EditText) findViewById(R.id.etTaxNumber);
        etBirth = (EditText) findViewById(R.id.etBirthday);
        etPin = (EditText) findViewById(R.id.etPin);
        etPinConfirm = (EditText) findViewById(R.id.etPinConfirm);

        etBirth.setOnTouchListener(new View.OnTouchListener() {
            private boolean pressed;
            private float x = 0.0f, y = 0.0f;

            @Override
            public boolean onTouch(View v, MotionEvent event) {
                if (event.getAction() == MotionEvent.ACTION_DOWN) {
                    pressed = true;
                    x = event.getX();
                    y = event.getY();
                } else if (event.getAction() == MotionEvent.ACTION_UP && pressed) {
                    birthDialog = new DatePickerFragment();
                    birthDialog.show(fm, "datePicker");
                    etBirth.clearFocus();
                    x = 0.0f; y = 0.0f;
                } else if (Math.abs(x - event.getX()) > 1 || Math.abs(y - event.getY()) > 1)
                    pressed = false;
                return true;
            }
        });

        etBirth.setOnFocusChangeListener(new View.OnFocusChangeListener() {
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

        Button btnCancel = (Button) findViewById(R.id.btnCancel);
        Button btnSend = (Button) findViewById(R.id.btnSend);

        btnCancel.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                finish();
            }
        });

        btnSend.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (!etPin.getText().toString().matches("\\d{4}")) {
                    android.app.AlertDialog.Builder builder = new android.app.AlertDialog.Builder(context);
                    builder.setTitle("Error en la introducción de datos")
                            .setMessage("El código Pin ha de consistir en cuatro caracteres numéricos.")
                            .setPositiveButton(getString(R.string.button_ok), new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialog, int id) {
                                    dialog.dismiss();
                                }
                            });
                    builder.create().show();
                } else if (!etPin.getText().toString().matches("\\d{4}") && !etPin.getText().toString().equals(etPinConfirm.getText().toString())) {
                    android.app.AlertDialog.Builder builder = new android.app.AlertDialog.Builder(context);
                    builder.setTitle("Error en la introducción de datos")
                            .setMessage("Los códigos PIN introducidos no coinciden.")
                            .setPositiveButton(getString(R.string.button_ok), new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialog, int id) {
                                    dialog.dismiss();
                                }
                            });
                    builder.create().show();
                } else {
                    UserMobileCreateArguments args = new UserMobileCreateArguments();

                    args.TaxName = etTaxName.getText().toString();
                    args.TaxAddress = etTaxAddress.getText().toString();
                    args.TaxNumber = etTaxNumber.getText().toString();
                    args.Birthday = birthS;
                    args.Pin = etPin.getText().toString();

                    ServerPost tarea = new ServerPost(context);
                    tarea.delegate = delegate;
                    tarea.refresh = true;
                    tarea.execute(getString(R.string.apiUser), CustomGson.getGson().toJson(args));
                }
            }
        });
    }

    @Override
    public void onAsyncFinish(HashMap<String, String> map) {
        if (HandleServerError.Handle(map, this, this)) return ;

        if (map.get("route").contains(getString(R.string.apiUser))) {
            Toast.makeText(this, "Creación de usuario correcta", Toast.LENGTH_LONG).show();
            Intent i = new Intent(PaymentsUserCreate.this, CreateTarjeta.class);
            startActivity(i);
            finish();
        }
    }

    public static class DatePickerFragment extends DialogFragment implements DatePickerDialog.OnDateSetListener {
        @Override @NonNull
        public Dialog onCreateDialog( Bundle savedInstanceState) {
            return new DatePickerDialog(context, this, year, month, day);
        }

        public void onDateSet(DatePicker view, int dialogYear, int dialogMonth, int dialogDay) {
            year = dialogYear;
            month = dialogMonth;
            day = dialogDay;

            etBirth.setText(day + "/" + (month + 1) + "/" + year);

            birthS = (month + 1) + "/" + day + "/" + year;
        }
    }
}
