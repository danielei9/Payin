package com.mobile.application.payin.views;

import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.os.Bundle;
import android.preference.Preference;
import android.preference.PreferenceFragment;
import android.preference.PreferenceScreen;
import android.support.v4.content.ContextCompat;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.text.InputFilter;
import android.text.InputType;
import android.view.View;
import android.widget.EditText;
import android.widget.LinearLayout;

import com.android.application.payin.BuildConfig;
import com.android.application.payin.R;

public class MainSettings extends AppCompatActivity {
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.settings_main);

        Toolbar toolbar = (Toolbar) findViewById(R.id.tool_bar);
        setSupportActionBar(toolbar);
        toolbar.setNavigationIcon(ContextCompat.getDrawable(this, R.drawable.ic_action_back));
        toolbar.setNavigationOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                finish();
            }
        });

        PreferenceFragment settingsFragment = (PreferenceFragment) getFragmentManager().findFragmentById(R.id.prefFragment);
        PreferenceScreen rootPrefScreen = ((PreferenceScreen) settingsFragment.findPreference("prefRootScreen"));

        final SharedPreferences pref = getSharedPreferences(getResources().getString(R.string.prefs), MODE_PRIVATE);
        Boolean debug = pref.getBoolean("debug", false);

        if (debug) toolbar.setBackgroundColor(Color.parseColor("#ff1a1a"));
        else {
            toolbar.setBackgroundColor(Color.parseColor("#e9af30"));
            rootPrefScreen.removePreference(rootPrefScreen.findPreference("prefVersion"));
            rootPrefScreen.removePreference(rootPrefScreen.findPreference("prefIP"));
        }
    }

    public static class SettingsFragment extends PreferenceFragment {
        SharedPreferences.Editor edit;

        @Override
        public void onCreate(Bundle savedInstanceState) {
            super.onCreate(savedInstanceState);

            getPreferenceManager().setSharedPreferencesName(getResources().getString(R.string.prefs));

            addPreferencesFromResource(R.xml.main_preferences);

            final SharedPreferences preferences = this.getActivity().getSharedPreferences(getResources().getString(R.string.prefs), MODE_PRIVATE);
            edit = preferences.edit();

            Preference versionCodePref = findPreference("prefVersion");
            versionCodePref.setSummary("" + BuildConfig.VERSION_CODE);

            Preference showHoursPref = findPreference("prefShowHours");
            showHoursPref.setOnPreferenceChangeListener(new Preference.OnPreferenceChangeListener() {
                @Override
                public boolean onPreferenceChange(Preference preference, Object newValue) {
                    edit.putBoolean("pref_show_hours", Boolean.valueOf(newValue.toString()));
                    edit.apply();
                    return true;
                }
            });

            Preference IPPref = findPreference("prefIP");
            IPPref.setOnPreferenceClickListener(new Preference.OnPreferenceClickListener() {
                @Override
                public boolean onPreferenceClick(Preference preference) {
                    AlertDialog.Builder builder = new AlertDialog.Builder(getActivity());
                    builder.setTitle("Cambiar IP de la server");
                    builder.setMessage("Introduzca la IP en formato 255.255.255.255:8080");

                    final EditText input = new EditText(getActivity());
                    input.setInputType(InputType.TYPE_NUMBER_FLAG_DECIMAL);
                    LinearLayout.LayoutParams lp = new LinearLayout.LayoutParams(
                            LinearLayout.LayoutParams.MATCH_PARENT,
                            LinearLayout.LayoutParams.MATCH_PARENT);

                    lp.setMargins(14, 0, 14, 0);

                    input.setLayoutParams(lp);
                    builder.setView(input);

                    builder.setPositiveButton("Ok",
                            new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialog, int id) {
                                    preferences.edit().putString("IP", "http://" + input.getText().toString() + "/").apply();
                                    dialog.dismiss();
                                }
                            });

                    builder.setNeutralButton("Eliminar",
                            new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialog, int id) {
                                    preferences.edit().remove("IP").apply();
                                    dialog.dismiss();
                                }
                            });

                    builder.setNegativeButton("Cancelar",
                            new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialog, int id) {
                                    dialog.dismiss();
                                }
                            });

                    builder.create().show();

                    return true;
                }
            });
        }
    }
}


