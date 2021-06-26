package com.mobile.application.payin.views;

import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.pm.PackageManager;
import android.graphics.Color;
import android.graphics.drawable.LayerDrawable;
import android.net.Uri;
import android.os.Bundle;
import android.support.v4.content.ContextCompat;
import android.support.v4.view.ViewPager;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.KeyEvent;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.TextView;
import android.widget.Toast;

import com.google.android.gms.common.ConnectionResult;
import com.google.android.gms.common.GooglePlayServicesUtil;
import com.google.zxing.integration.android.IntentIntegrator;
import com.google.zxing.integration.android.IntentResult;
import com.android.application.payin.BuildConfig;
import com.android.application.payin.R;
import com.mobile.application.payin.common.serverconnections.ServerGet;
import com.mobile.application.payin.common.serverconnections.ServerPut;
import com.mobile.application.payin.common.interfaces.AsyncResponse;
import com.mobile.application.payin.common.utilities.CustomGson;
import com.mobile.application.payin.common.utilities.HandleServerError;
import com.mobile.application.payin.common.utilities.IconsWithBadgeCount;
import com.mobile.application.payin.common.utilities.IsMyServiceRunning;
import com.mobile.application.payin.common.utilities.SecurityDialog;
import com.mobile.application.payin.common.utilities.SlidingTabLayout;
import com.mobile.application.payin.common.utilities.ViewPagerAdapter;
import com.mobile.application.payin.dto.results.MainMobileGetAllResult;
import com.mobile.application.payin.services.PushRegisterService;
import com.mobile.application.payin.services.TrackService;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.Arrays;
import java.util.Calendar;
import java.util.HashMap;
import java.util.HashSet;

public class Principal extends AppCompatActivity implements AsyncResponse {
    private Toolbar toolbar;
    private TextView txtHours;
    private ViewPager pager;
    private SlidingTabLayout tabs;

    private static Context context;

    private final CharSequence[] Titles = {"Servicios", "Tickets", "Tarjetas"};

    private ServerGet tarea = null;
    private int numNotifications = 0;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.principal);

        toolbar = (Toolbar) findViewById(R.id.tool_bar);
        setSupportActionBar(toolbar);

        SharedPreferences pref = getSharedPreferences(getResources().getString(R.string.prefs), MODE_PRIVATE);

        boolean debug = pref.getBoolean("debug", false);

        pager = (ViewPager) findViewById(R.id.pager);
        tabs = (SlidingTabLayout) findViewById(R.id.tabs);

        txtHours = (TextView) findViewById(R.id.txtWorkHours);

        if (debug) {
            toolbar.setBackgroundColor(Color.parseColor("#ff1a1a"));
            txtHours.setBackgroundColor(Color.parseColor("#ff1a1a"));
        } else {
            toolbar.setBackgroundColor(Color.parseColor("#e9af30"));
            txtHours.setBackgroundColor(Color.parseColor("#e9af30"));
        }

        ViewPagerAdapter adapter = new ViewPagerAdapter(getSupportFragmentManager(), Titles, Titles.length);
        pager.setAdapter(adapter);

        tabs.setDistributeEvenly(true);

        tabs.setCustomTabColorizer(new SlidingTabLayout.TabColorizer() {
            @Override
            public int getIndicatorColor(int position) {
                return ContextCompat.getColor(context, R.color.tabsScrollColor);
            }
        });

        //tabs.setCustomTabColorizer((int position) -> getResources().getColor(R.color.tabsScrollColor));

        tabs.setViewPager(pager);
        pager.setVisibility(View.VISIBLE);
        tabs.setVisibility(View.VISIBLE);

        context = this;

        if (pref.getStringSet("trackIds", new HashSet<String>()).size() > 0 && !IsMyServiceRunning.isMyServiceRunning(TrackService.class, context)) {
            context.startService(new Intent(this, TrackService.class));
        }
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        getMenuInflater().inflate(R.menu.menu_principal, menu);

        try {
            MenuItem notificationsItem = menu.findItem(R.id.bell);
            LayerDrawable icon = (LayerDrawable) notificationsItem.getIcon();

            // Update LayerDrawable's BadgeDrawable
            if (numNotifications != 0)
                if (numNotifications >= 100)
                    IconsWithBadgeCount.setNotificationBadgeCount(this, icon, "...");
                else
                    IconsWithBadgeCount.setNotificationBadgeCount(this, icon, Integer.toString(numNotifications));
        } catch (NullPointerException e) {
            System.err.println(e.getMessage() == null ? "" : e.getMessage());
        } catch (Exception e) {
            e.printStackTrace();
        }

        return true;
    }

    @Override
    public boolean onPrepareOptionsMenu(Menu menu) {
        return super.onPrepareOptionsMenu(menu);
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        int id = item.getItemId();

        if (id == R.id.action_settings) {
            Intent i = new Intent(Principal.this, MainSettings.class);
            startActivity(i);
        } else if (id == R.id.bell) {
            Intent i = new Intent(Principal.this, NotificationList.class);
            startActivity(i);
        } else if (id == R.id.action_about) {
            Intent i = new Intent(Principal.this, About.class);
            startActivity(i);
        } else if (id == R.id.logout) {
            SharedPreferences pref = getSharedPreferences(getResources().getString(R.string.prefs), MODE_PRIVATE);

            ServerPut task = new ServerPut(context);
            task.delegate = null;
            task.showProgress = false;
            task.execute(getResources().getString(R.string.apiAccountLogout), "{}");

            SharedPreferences.Editor editor = pref.edit();

            editor.remove("access_token");
            editor.remove("refresh_token");
            editor.remove("notificationId");
            editor.remove("pushToken");

            editor.apply();

            Intent i = new Intent(Principal.this, Login.class);
            i.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
            i.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TASK);
            startActivity(i);
            finish();

            return true;
        }else if (id == R.id.qr_code) {
            String permission = "android.permission.CAMERA";
            if (checkCallingOrSelfPermission(permission) == PackageManager.PERMISSION_GRANTED) {
                IntentIntegrator.initiateScan(this);
            } else {
                SecurityDialog.createDialogDismiss(this);
            }

            return true;
        }

        return super.onOptionsItemSelected(item);
    }

    @Override
    public void onActivityResult(int requestCode, int resultCode, Intent intent) {
        IntentResult result = IntentIntegrator.parseActivityResult(requestCode, resultCode, intent);
        if (result != null) {
            String contents = result.getContents();
            if (contents != null) {
                final String capturedQrValue = intent.getStringExtra("SCAN_RESULT");

                if (capturedQrValue.startsWith("pay[in]/ticket")) {
                    int id;
                    try {
                        JSONObject jsonObj = new JSONObject(capturedQrValue.split(":", 2)[1]);
                        id = jsonObj.getInt("id");
                    } catch (JSONException e) {
                        return ;
                    }

                    Intent i = new Intent(Principal.this, TicketReception.class);
                    i.putExtra("id", id);
                    startActivity(i);
                    return ;
                }

                new AlertDialog.Builder(this)
                        .setTitle("Código QR leído:")
                        .setMessage("¿Visualizar el contenido?")
                        .setPositiveButton(R.string.yes, new DialogInterface.OnClickListener() {
                            public void onClick(DialogInterface dialog, int which) {
                                if (capturedQrValue.matches("(https?|ftp|file)://[-a-zA-Z0-9+&@#/%?=~_|!:,.;]*[-a-zA-Z0-9+&@#/%=~_|]")){
                                    Intent i = new Intent(Intent.ACTION_VIEW, Uri.parse(capturedQrValue));
                                    startActivity(i);
                                } else {
                                    Toast.makeText(getApplicationContext(), "Contenido del QR: " + capturedQrValue, Toast.LENGTH_LONG).show();
                                }
                            }
                        })
                        .setNegativeButton(R.string.no, null)
                        .show();
            }
        }
    }

    @Override
    protected void onResume() {
        tarea = new ServerGet(this);
        tarea.delegate = this;
        tarea.execute(getResources().getString(R.string.apiMainMobile));

        super.onResume();
    }

    @Override
    protected void onPause() {
        if (tarea != null && tarea.showProgress) {
            tarea.dismissProgress();
            tarea.showProgress = false;
        }
        super.onPause();
    }

    @Override
    public boolean onKeyDown(int keycode, KeyEvent e) {
        if (keycode == KeyEvent.KEYCODE_MENU) {
            toolbar.showOverflowMenu();
            return true;
        } else
            return super.onKeyDown(keycode, e);
    }

    @Override
    public void onAsyncFinish (HashMap<String, String> map) {
        if (HandleServerError.Handle(map, this, this)) return ;

        if (map.get("route").contains(getResources().getString(R.string.apiMainMobile))) {
            SharedPreferences pref = getSharedPreferences(getResources().getString(R.string.prefs), MODE_PRIVATE);

            if (checkPlayServices()) {
                Intent intent = new Intent(this, PushRegisterService.class);
                startService(intent);
            }

            MainMobileGetAllResult res = CustomGson.getGson().fromJson(map.get("json"), MainMobileGetAllResult.class);

            if (BuildConfig.VERSION_CODE < res.AppVersion && (!pref.contains("version_timeout") ||
                    (pref.getLong("version_timeout", 0) + 3600000 > Calendar.getInstance().getTimeInMillis()))) {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);

                builder.setMessage("Existe una nueva version de la aplicación, por favor descarguela.").setTitle("Actualización");

                builder.setPositiveButton("Actualizar", new DialogInterface.OnClickListener() {
                    public void onClick(DialogInterface dialog, int id) {
                        final String appPackageName = getPackageName(); // getPackageName() from Context or Activity object
                        try {
                            startActivity(new Intent(Intent.ACTION_VIEW, Uri.parse("market://details?id=" + appPackageName)));
                        } catch (android.content.ActivityNotFoundException anfe) {
                            startActivity(new Intent(Intent.ACTION_VIEW, Uri.parse("https://play.google.com/store/apps/details?id=" + appPackageName)));
                        }
                        dialog.dismiss();
                    }
                });

                builder.setNegativeButton("Más tarde", new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        dialog.dismiss();
                    }
                });

                builder.show();

                pref.edit().putLong("version_timeout", Calendar.getInstance().getTimeInMillis()).apply();
            }

            if (numNotifications != res.NumNotifications) {
                numNotifications = res.NumNotifications;
                invalidateOptionsMenu();
            }

            if (res.Favorites.size() == 0) {
                CharSequence[] TitlesShort = Arrays.copyOfRange(Titles, 1, 3);

                ViewPagerAdapter adapter = new ViewPagerAdapter(getSupportFragmentManager(), TitlesShort, TitlesShort.length);
                pager.setAdapter(adapter);

                tabs.setViewPager(pager);
            } else
                FragmentServicios.info(res, txtHours, this);

            FragmentPagos.info(res.Tickets, this);
            FragmentTarjetas.info(res.Data, this);
        }
    }

    private boolean checkPlayServices() {
        int resultCode = GooglePlayServicesUtil.isGooglePlayServicesAvailable(this);
        if (resultCode != ConnectionResult.SUCCESS) {
            if (GooglePlayServicesUtil.isUserRecoverableError(resultCode)) {
                GooglePlayServicesUtil.getErrorDialog(resultCode, this, 9000).show();
            } else {
                finish();
            }
            return false;
        }
        return true;
    }
}
