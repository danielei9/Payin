package com.mobile.application.payin.views;

import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.os.Bundle;
import android.support.v4.content.res.ResourcesCompat;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.Menu;
import android.view.MenuItem;
import android.view.MotionEvent;
import android.view.View;
import android.widget.TextView;

import com.android.application.payin.R;
import com.mobile.application.payin.common.interfaces.AsyncResponse;
import com.mobile.application.payin.common.serverconnections.ServerGet;
import com.mobile.application.payin.common.utilities.CustomGson;
import com.mobile.application.payin.common.utilities.HandleServerError;
import com.mobile.application.payin.dto.results.AccountGetResult;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.HashMap;
import java.util.Locale;

public class Profile extends AppCompatActivity implements AsyncResponse {
    private TextView txtUserPublicName, txtUserPublicMail, txtUserPublicPhone, txtUserPublicBirth,
            txtUserTaxName, txtUserTaxNumber, txtUserTaxBirth, txtUserTaxAddr;

    private AccountGetResult.User res;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.profile);

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

        txtUserPublicName = (TextView) findViewById(R.id.txtUserPublicName);
        txtUserPublicMail = (TextView) findViewById(R.id.txtUserPublicMail);
        txtUserPublicPhone = (TextView) findViewById(R.id.txtUserPublicPhone);
        txtUserPublicBirth = (TextView) findViewById(R.id.txtUserPublicBirth);
        txtUserTaxName = (TextView) findViewById(R.id.txtUserTaxName);
        txtUserTaxNumber = (TextView) findViewById(R.id.txtUserTaxNumber);
        txtUserTaxBirth = (TextView) findViewById(R.id.txtUserTaxBirth);
        txtUserTaxAddr = (TextView) findViewById(R.id.txtUserTaxAddr);
        TextView txtUserChangePass = (TextView) findViewById(R.id.txtUserChangePass);
        TextView txtUserChangePin = (TextView) findViewById(R.id.txtUserChangePin);

        txtUserChangePass.setOnTouchListener(new View.OnTouchListener() {
            @Override
            public boolean onTouch(View v, MotionEvent event) {
                if (event.getAction() == MotionEvent.ACTION_DOWN)
                    startActivity(new Intent(Profile.this, UpdatePass.class));
                return true;
            }
        });

        txtUserChangePin.setOnTouchListener(new View.OnTouchListener() {
            @Override
            public boolean onTouch(View v, MotionEvent event) {
                if (event.getAction() == MotionEvent.ACTION_DOWN)
                    startActivity(new Intent(Profile.this, UpdatePin.class));
                return true;
            }
        });
    }

    @Override
    protected void onResume() {
        ServerGet tarea = new ServerGet(this);
        tarea.delegate = this;
        tarea.execute(getString(R.string.apiAccountCurrent));

        super.onResume();
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        getMenuInflater().inflate(R.menu.menu_update, menu);

        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        int id = item.getItemId();

        if (id == R.id.itemUpdate) {
            Intent i = new Intent(Profile.this, UpdateProfile.class);
            i.putExtra("profile", res);
            startActivity(i);
            return true;
        }

        return false;
    }

    @Override
    public void onAsyncFinish(HashMap<String, String> map) {
        HandleServerError.Handle(map, this, this);

        SharedPreferences pref = getSharedPreferences(getResources().getString(R.string.prefs), MODE_PRIVATE);
        SimpleDateFormat sdfServer = new SimpleDateFormat("yyyy-MM-dd", Locale.getDefault()), sdf = new SimpleDateFormat("d MMM yyyy", Locale.getDefault());
        Calendar now = Calendar.getInstance();

        if (map.get("route").contains(getString(R.string.apiAccountCurrent))) {
            res = CustomGson.getGson().fromJson(map.get("json"), AccountGetResult.class).data.get(0);

            txtUserPublicName.setText(res.Name);
            txtUserPublicMail.setText(pref.getString("login", ""));
            txtUserPublicPhone.setText(res.Mobile);
            txtUserPublicBirth.setText(res.Birthday);
            txtUserTaxName.setText(res.TaxName);
            txtUserTaxNumber.setText(res.TaxNumber);
            txtUserTaxAddr.setText(res.TaxAddress);

            try {
                now.setTimeInMillis(sdfServer.parse(res.Birthday).getTime());
                txtUserTaxBirth.setText(sdf.format(now.getTime()));
            } catch (ParseException e) {
                txtUserTaxBirth.setText(res.Birthday);
            }
        }
    }
}
