package com.mobile.application.payin.views;

import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.os.Bundle;
import android.support.v4.content.res.ResourcesCompat;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.util.Log;
import android.view.View;
import android.webkit.WebView;
import android.widget.ImageView;

import com.android.application.payin.R;

import java.io.IOException;
import java.io.InputStream;

public class About extends AppCompatActivity {
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.about);

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

        ImageView imageView = (ImageView) findViewById(R.id.logo);
        imageView.setOnLongClickListener(new View.OnLongClickListener() {
            @Override
            public boolean onLongClick(View v) {

                SharedPreferences pref = getSharedPreferences(getResources().getString(R.string.prefs), MODE_PRIVATE);
                SharedPreferences.Editor editor = pref.edit();
                boolean debug = pref.getBoolean("debug", false);

                if (debug) {
                    editor.putBoolean("debug", false);
                    editor.remove("access_token");
                    editor.remove("refresh_token");
                    editor.remove("IP");
                    editor.remove("notificationId");
                    editor.remove("pushToken");

                    editor.apply();

                    Intent intent = new Intent(About.this, Login.class);
                    startActivity(intent);
                    finish();

                    return true;
                }

                if (pref.getBoolean("role_tester", false)){
                    editor.putBoolean("debug", true);
                    editor.remove("access_token");
                    editor.remove("refresh_token");

                    editor.apply();

                    Intent intent = new Intent(About.this, Login.class);
                    startActivity(intent);
                    finish();

                    return true;
                }

                return false;
            }
        });

        WebView infoPayin = (WebView) findViewById(R.id.WV_about);

        infoPayin.setVerticalScrollBarEnabled(false);
        infoPayin.setHorizontalScrollBarEnabled(false);

        infoPayin.setBackgroundColor(Color.TRANSPARENT);

        try {
            InputStream info = getApplication().getAssets().open("about.txt");
            int tam = info.available(), result;
            byte[] buffer = new byte[tam];

            result = info.read(buffer);
            if (result == -1){
                Intent i = new Intent(About.this, Principal.class);
                startActivity(i);
                finish();
            }
            info.close();

            infoPayin.loadDataWithBaseURL("", (new String(buffer)), "text/html", "utf-8", "");

        } catch (IOException e) {
            Log.e("Error fichero assets: ", e.getMessage());
        }
    }
}
