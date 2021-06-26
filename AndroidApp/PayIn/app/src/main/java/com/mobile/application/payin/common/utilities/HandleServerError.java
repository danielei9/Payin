package com.mobile.application.payin.common.utilities;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;

import com.android.application.payin.R;
import com.mobile.application.payin.common.serverconnections.ServerPut;
import com.mobile.application.payin.views.Login;
import com.mobile.application.payin.views.Principal;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.HashMap;
import java.util.Iterator;
import java.util.Map;

public class HandleServerError {
    public static boolean Handle(Map<String, String> map, final Activity activity, final Context context) {
        boolean error = false;

        if (map == null) {
            error = true;
        } else if (map.containsKey("error") && map.get("error").equals("host")) {
            networkErrorDialog(context, activity);

            error = true;
        } else if (map.containsKey("error") && map.get("error").equals("401")) {
            logout(activity, context);

            error = true;
        } else if (map.containsKey("error")) {
            serverError(map, context, activity);

            error = true;
        }

        return error;
    }

    private static void networkErrorDialog(Context context, final Activity activity) {
        AlertDialog.Builder builder = new AlertDialog.Builder(context);
        builder.setTitle("Error de conexion")
                .setMessage("No ha sido posible conectar con el servidor. Consulte sus ajustes de red.")
                .setCancelable(false)
                .setPositiveButton("Reintentar", new DialogInterface.OnClickListener() {
                    public void onClick(DialogInterface dialog, int id) {
                        dialog.dismiss();
                        activity.recreate();
                    }
                });
        AlertDialog alert = builder.create();
        alert.show();
    }

    private static void serverError(Map<String, String> map, Context context, Activity activity) {
        if (map.get("type").equals("get")) {
            serverErrorRecreateDialog(map, context, activity);
        } else {
            Map<String, String> errorMap = new HashMap<>();

            try {
                JSONObject jsonObj = new JSONObject(map.get("json"));

                Iterator<String> keys = jsonObj.keys();
                while (keys.hasNext()) {
                    String key = keys.next();
                    String value = jsonObj.getString(key);

                    errorMap.put(key, value);
                }

                serverErrorInfoDialog(errorMap, context);
            } catch (JSONException e) {
                serverErrorRecreateDialog(map, context, activity);
            }
        }
    }

    private static void serverErrorRecreateDialog(Map<String, String> map, final Context context, final Activity activity) {
        SharedPreferences pref = context.getSharedPreferences(context.getResources().getString(R.string.prefs), Context.MODE_PRIVATE);
        AlertDialog.Builder builder = new AlertDialog.Builder(context);

        builder.setTitle("Ha ocurrido un error")
                .setMessage(pref.getBoolean("debug", false) ? map.get("json") : "Ha ocurrido un error en la conexión con el servidor. A continuación serás redirigido a la pantalla principal.")
                .setCancelable(false)
                .setPositiveButton(R.string.button_ok, new DialogInterface.OnClickListener() {
                    public void onClick(DialogInterface dialog, int id) {
                        Intent i = new Intent(activity, Principal.class);
                        i.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                        context.startActivity(i);
                        activity.finish();
                    }
                });
        AlertDialog alert = builder.create();
        alert.setCanceledOnTouchOutside(false);
        alert.show();
    }

    private static void serverErrorInfoDialog(Map<String, String> errorMap, Context context) {
        AlertDialog.Builder builder = new AlertDialog.Builder(context);
        String title = errorMap.get("message"), body = "";

        try {
            JSONObject jsonObj = new JSONObject(errorMap.get("modelState"));

            Iterator<String> keys = jsonObj.keys();
            while (keys.hasNext()) {
                String key = keys.next();
                if (jsonObj.get(key) instanceof JSONArray) {
                    body += jsonObj.getJSONArray(key).join("\n").replace("\"", "");
                } else if (jsonObj.get(key) instanceof String)
                    body += jsonObj.getString(key) + "\n";
            }
        } catch (JSONException e) {
            body += "\n";
        } catch (NullPointerException e) {
            String[] s = title.split("\n");
            if (s.length == 2) {
                body = s[1];
                title = s[0];
            }
        }

        builder.setTitle(title)
                .setMessage(body)
                .setCancelable(false)
                .setPositiveButton(R.string.button_ok, new DialogInterface.OnClickListener() {
                    public void onClick(DialogInterface dialog, int id) {
                        dialog.dismiss();
                    }
                });
        AlertDialog alert = builder.create();
        alert.setCanceledOnTouchOutside(false);
        alert.show();
    }

    private static void logout(Activity activity, Context context) {
        SharedPreferences pref = context.getSharedPreferences(context.getResources().getString(R.string.prefs), Context.MODE_PRIVATE);
        SharedPreferences.Editor editor = pref.edit();

        ServerPut task = new ServerPut(context);
        task.delegate = null;
        task.showProgress = false;
        task.execute(context.getResources().getString(R.string.apiAccountLogout), "{}");

        editor.remove("access_token");
        editor.remove("refresh_token");
        editor.remove("notificationId");
        editor.remove("pushToken");

        editor.apply();

        if (activity != null) {
            Intent i = new Intent(activity, Login.class);
            i.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
            context.startActivity(i);
            activity.finish();
        }
    }
}
