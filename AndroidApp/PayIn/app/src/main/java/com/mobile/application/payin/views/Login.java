package com.mobile.application.payin.views;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.net.Uri;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.view.View;
import android.widget.EditText;
import android.widget.TextView;

import com.android.application.payin.R;
import com.mobile.application.payin.common.serverconnections.ServerAuth;
import com.mobile.application.payin.common.interfaces.AsyncResponse;
import com.mobile.application.payin.common.serverconnections.ServerPut;

import java.util.HashMap;

public class Login extends Activity implements AsyncResponse {
    private EditText etUserName, etPass;
    private TextView txtSignIn, txtRegistro, txtTerms;

    private Context context;
    private AsyncResponse delegate;

    private ServerAuth auth;
    private String email;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.login);

        context = this;
        delegate = this;

        String login;
        final SharedPreferences pref = getSharedPreferences(getResources().getString(R.string.prefs), MODE_PRIVATE);

        final Uri data = getIntent().getData();
        if (data != null) {
            new AlertDialog.Builder(this)
                    .setTitle("Confirmar email")
                    .setMessage("¿Desea confirmar la dirección de correo electrónico: " + data.getQueryParameter("email") + "?")
                    .setPositiveButton("Confirmar", new DialogInterface.OnClickListener() {
                        @Override
                        public void onClick(DialogInterface dialog, int which) {
                            email = data.getQueryParameter("email");
                            ServerPut tarea = new ServerPut(context);
                            tarea.delegate = delegate;
                            tarea.execute(getString(R.string.apiAccountConfirmMail),
                                    "{\"userId\":\"" + data.getQueryParameter("userid") + "\",\"code\":\"" + data.getQueryParameter("code") + "\"}");
                        }
                    }).setNegativeButton("Cancelar", new DialogInterface.OnClickListener() {
                @Override
                public void onClick(DialogInterface dialog, int which) {
                    dialog.dismiss();
                }
            }).show();
        } else if (pref.contains("access_token")){
            Intent i = new Intent(Login.this, Principal.class);
            startActivity(i);
            finish();
        }

        if (pref.getBoolean("debug", false)) {
            findViewById(R.id.imageView).setOnLongClickListener(new View.OnLongClickListener() {
                @Override
                public boolean onLongClick(View v) {
                    SharedPreferences.Editor edit = pref.edit();
                    edit.remove("debug");
                    edit.remove("access_token");
                    edit.remove("refresh_token");
                    edit.remove("IP");
                    edit.remove("notificationId");
                    edit.remove("pushToken");
                    edit.apply();

                    recreate();

                    return true;
                }
            });
        }

        etUserName = (EditText) findViewById(R.id.etUserName);
        etPass = (EditText) findViewById(R.id.etPass);
        txtSignIn = (TextView) findViewById(R.id.txtSingIn);
        txtRegistro = (TextView) findViewById(R.id.txtRegistro);
        txtTerms = (TextView) findViewById(R.id.txtTerms);

        if (pref.contains("login")){
            login = pref.getString("login", "");
            etUserName.setText(login);
        }

        txtSignIn.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                String user = etUserName.getText().toString();
                String password = etPass.getText().toString();

                SharedPreferences pref = getSharedPreferences(getResources().getString(R.string.prefs), MODE_PRIVATE);

                SharedPreferences.Editor editor = pref.edit();

                editor.putString("login", user);

                editor.apply();

                auth = new ServerAuth(context);
                auth.delegate = delegate;
                auth.execute(user, password);
            }
        });

        txtRegistro.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent i = new Intent(Login.this, Register.class);
                startActivity(i);
            }
        });

        txtTerms.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent i = new Intent(Intent.ACTION_VIEW);
                i.setData(Uri.parse("http://www.pay-in.es/condiciones-de-servicio/"));
                startActivity(i);
            }
        });
        /*
        txtRegistro.setOnClickListener((View v) -> {
            Intent i = new Intent(Login.this, Register.class);
            startActivity(i);
        });
        */
    }

    @Override
    protected void onPause() {
        if (auth != null){
            auth.dismissProgress();
            auth.cancel(true);
        }
        super.onPause();
    }

    @Override
    protected void onSaveInstanceState(@NonNull Bundle outState) {
        outState.putString("pass", etPass.getText().toString());

        super.onSaveInstanceState(outState);
    }

    @Override
    protected void onRestoreInstanceState(@NonNull Bundle savedInstanceState) {
        if (savedInstanceState.containsKey("pass")) etPass.setText(savedInstanceState.getString("pass"));

        super.onRestoreInstanceState(savedInstanceState);
    }

    @Override
    public void onAsyncFinish(HashMap<String, String> map){
        if (map == null) {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.setTitle("Error")
                    .setMessage("El usuario o la contraseña son erroneos")
                    .setCancelable(false)
                    .setPositiveButton("OK", new DialogInterface.OnClickListener() {
                        public void onClick(DialogInterface dialog, int id) {
                            dialog.dismiss();
                        }
                    });
            AlertDialog alert = builder.create();
            alert.show();
        } else if (map.containsKey("error") && map.containsKey("route") && map.get("route").contains(getString(R.string.apiAccountConfirmMail))) {
            new AlertDialog.Builder(this)
                    .setTitle("Confirmar email")
                    .setMessage("Ha ocurrido un problema al verificar el email, por favor póngase en contacto con system@pay-in.es.")
                    .setPositiveButton("Continuar", new DialogInterface.OnClickListener() {
                        @Override
                        public void onClick(DialogInterface dialog, int which) {
                            dialog.dismiss();
                        }
                    })
                    .show();
        } else if (map.containsKey("route") && map.get("route").contains(getString(R.string.apiAccountConfirmMail))) {
            new AlertDialog.Builder(this)
                    .setTitle("Confirmar email")
                    .setMessage("Confirmación de email correcta")
                    .setPositiveButton("Continuar", new DialogInterface.OnClickListener() {
                        @Override
                        public void onClick(DialogInterface dialog, int which) {
                            etUserName.setText(email);
                            dialog.dismiss();
                        }
                    })
                    .show();
        } else if (map.containsKey("error")) {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.setTitle("Error")
                    .setMessage("El usuario o la contraseña son erroneos")
                    .setCancelable(false)
                    .setPositiveButton("OK", new DialogInterface.OnClickListener() {
                        public void onClick(DialogInterface dialog, int id) {
                            dialog.dismiss();
                        }
                    });
            AlertDialog alert = builder.create();
            alert.show();
        } else if (map.containsKey("success")){
            SharedPreferences pref = getSharedPreferences(getResources().getString(R.string.prefs), MODE_PRIVATE);

            SharedPreferences.Editor editor = pref.edit();

            editor.putString("access_token", map.get("access_token"));
            editor.putString("refresh_token", map.get("refresh_token"));
            editor.putString("user_name", map.get("as:name"));

            String roles[] = map.get("as:roles").split(",");

            editor.remove("role_user");
            editor.remove("role_operator");
            editor.remove("role_superadmin");
            editor.remove("role_admin");
            editor.remove("role_commerce");
            editor.remove("role_commercePayment");
            editor.remove("role_tester");

            for (String role : roles){
                switch (role.trim().toLowerCase()){
                    case "user":
                        editor.putBoolean("role_user", true);
                        break;
                    case "operator":
                        editor.putBoolean("role_operator", true);
                        break;
                    case "superadministrator":
                        editor.putBoolean("role_superadmin", true);
                        break;
                    case "administrator":
                        editor.putBoolean("role_admin", true);
                        break;
                    case "commerce":
                        editor.putBoolean("role_commerce", true);
                        break;
                    case "commercepayment":
                        editor.putBoolean("role_commercePayment", true);
                        break;
                    case "tester":
                        editor.putBoolean("role_tester", true);
                        break;
                }
            }

            editor.apply();

            Intent i = new Intent(Login.this, Principal.class);
            startActivity(i);
            finish();
        }
    }
}

