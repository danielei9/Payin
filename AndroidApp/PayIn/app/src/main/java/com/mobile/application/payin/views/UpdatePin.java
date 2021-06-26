package com.mobile.application.payin.views;

import android.app.Activity;
import android.content.Context;
import android.os.Bundle;
import android.support.v4.content.res.ResourcesCompat;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import com.android.application.payin.R;
import com.mobile.application.payin.common.interfaces.AsyncResponse;
import com.mobile.application.payin.common.serverconnections.ServerPost;
import com.mobile.application.payin.common.utilities.HandleServerError;

import java.util.HashMap;

public class UpdatePin extends AppCompatActivity implements AsyncResponse {
    private EditText etOldPin;
    private EditText etNewPin;
    private EditText etNewPinCon;
    private Context context;
    private AsyncResponse delegate;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.update_pin);

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

        etOldPin = (EditText) findViewById(R.id.etOldPin);
        etNewPin = (EditText) findViewById(R.id.etNewPin);
        etNewPinCon = (EditText) findViewById(R.id.etNewPinCon);

        Button btnChangePin = (Button) findViewById(R.id.btnChangePin);

        btnChangePin.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String query = "{\"oldPin\":\"" + etOldPin.getText().toString() + "\",\"pin\":\"" +
                        etNewPin.getText().toString() + "\",\"confirmPin\":\"" + etNewPinCon.getText().toString() + "\"}";

                ServerPost task = new ServerPost(context);
                task.delegate = delegate;
                task.execute(getResources().getString(R.string.apiUserUpdatePin), query);
            }
        });
    }

    @Override
    public void onAsyncFinish(HashMap<String, String> map){
        if (HandleServerError.Handle(map, this, this)) return;

        if (map.containsKey("success")){
            Toast.makeText(this, "Cambio de PIN correcto", Toast.LENGTH_LONG).show();
            onBackPressed();
        } else {
            Toast.makeText(this, "Cambio de PIN incorrecto", Toast.LENGTH_LONG).show();
        }
    }
}
