package com.mobile.application.payin.views;

import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.v4.app.FragmentActivity;
import android.view.View;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.EditText;

import com.android.application.payin.R;
import com.mobile.application.payin.common.serverconnections.ServerPost;
import com.mobile.application.payin.common.interfaces.AsyncResponse;
import com.mobile.application.payin.common.utilities.CustomGson;
import com.mobile.application.payin.dto.arguments.AccountRegisterArguments;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.HashMap;

public class Register extends FragmentActivity implements AsyncResponse {
    private EditText etName, etMail, etPhone, etPass, etPassCh;
    private CheckBox checkBox;

    private Context context;
    private AsyncResponse delegate;
    private ServerPost task = null;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.register);

        context = this;
        delegate = this;

        etName = (EditText) findViewById(R.id.etName);
        etMail = (EditText) findViewById(R.id.etEmail);
        etPhone = (EditText) findViewById(R.id.etPhone);
        etPass = (EditText) findViewById(R.id.etPassword);
        etPassCh = (EditText) findViewById(R.id.etPasswordCheck);
        checkBox = (CheckBox) findViewById(R.id.checkBox);

        Button btnReg = (Button) findViewById(R.id.btnReg);

        btnReg.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                AccountRegisterArguments args = new AccountRegisterArguments();

                args.Name = etName.getText().toString();
                args.UserName = etMail.getText().toString();
                args.Mobile = etPhone.getText().toString();
                args.Password = etPass.getText().toString();
                args.ConfirmPassword = etPassCh.getText().toString();
                args.AcceptTerms = checkBox.isChecked();

                String query = CustomGson.getGson().toJson(args);

                task = new ServerPost(context);
                task.delegate = delegate;
                task.execute(getResources().getString(R.string.apiAccount), query);
            }
        });

        Button btnCancel = (Button) findViewById(R.id.btnCancel);

        btnCancel.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                finish();
            }
        });
    }

    @Override
    protected void onPause() {
        if (task != null){
            task.dismissProgress();
            task.cancel(false);
        }
        super.onPause();
    }

    @Override
    protected void onSaveInstanceState(@NonNull Bundle outState) {
        outState.putString("name", etName.getText().toString());
        outState.putString("mail", etMail.getText().toString());
        outState.putString("phone", etPhone.getText().toString());
        outState.putString("pass", etPass.getText().toString());
        outState.putString("passC", etPassCh.getText().toString());

        super.onSaveInstanceState(outState);
    }

    @Override
    protected void onRestoreInstanceState(@NonNull Bundle savedInstanceState) {
        try {
            if (savedInstanceState.containsKey("name"))
                etName.setText(savedInstanceState.getString("name"));
            if (savedInstanceState.containsKey("mail"))
                etMail.setText(savedInstanceState.getString("mail"));
            if (savedInstanceState.containsKey("phone"))
                etPhone.setText(savedInstanceState.getString("phone"));
            if (savedInstanceState.containsKey("pass"))
                etPass.setText(savedInstanceState.getString("pass"));
            if (savedInstanceState.containsKey("passC"))
                etPassCh.setText(savedInstanceState.getString("passC"));
        } catch (NullPointerException e) {
            e.printStackTrace();
        }

        super.onRestoreInstanceState(savedInstanceState);
    }

    @Override
    public void onAsyncFinish(HashMap<String, String> map) {
        if (map.containsKey("success")) {
            SharedPreferences pref = getSharedPreferences(getResources().getString(R.string.prefs), MODE_PRIVATE);
            SharedPreferences.Editor editor = pref.edit();

            editor.putString("user_name", etMail.getText().toString());

            editor.apply();

            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.setTitle("Gracias por registrarse")
                    .setMessage("En breve recibir?? un correo electr??nico inform??ndole de como continuar el registro, si no encuentra este correo en su buz??n de entrada verifique la carpeta de correo no deado. " +
                            "Si no lo recibe, puede volver a registrarte para que se le reenv??e el correo de confirmaci??n o ponerse en contacto con system@pay-in.es")
                    .setCancelable(false)
                    .setPositiveButton("Ok", new DialogInterface.OnClickListener() {
                        public void onClick(DialogInterface dialog, int id) {
                            dialog.dismiss();
                            finish();
                        }
                    });
            AlertDialog alert = builder.create();
            alert.show();
        } else {
            try {
                JSONObject jsonObj = new JSONObject(map.get("json"));

                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                if (jsonObj.has("message")) {
                    builder.setTitle("Creaci??n de usuario err??nea")
                            .setMessage(jsonObj.get("message").toString())
                            .setPositiveButton("Ok", new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialog, int id) {
                                    dialog.dismiss();
                                }
                            });
                } else {
                    builder.setTitle("Creaci??n de usuario err??nea")
                            .setMessage("Revise la informaci??n introducida en los campos y vuevla a intentarlo.")
                            .setPositiveButton("Ok", new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialog, int id) {
                                    dialog.dismiss();
                                }
                            });
                }
                AlertDialog alert = builder.create();
                alert.show();
            } catch (JSONException e) {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.setTitle("Creaci??n de usuario err??nea")
                        .setMessage("Revise la informaci??n introducida en los campos y vuevla a intentarlo.")
                        .setPositiveButton("Ok", new DialogInterface.OnClickListener() {
                            public void onClick(DialogInterface dialog, int id) {
                                dialog.dismiss();
                            }
                        });
                AlertDialog alert = builder.create();
                alert.show();
            }
        }
    }
}
