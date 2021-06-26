package com.mobile.application.payin.views;

import android.app.DatePickerDialog;
import android.app.Dialog;
import android.app.TimePickerDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.Bitmap;
import android.graphics.Color;
import android.os.Bundle;
import android.provider.MediaStore;
import android.support.annotation.NonNull;
import android.support.v4.app.DialogFragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.content.res.ResourcesCompat;
import android.app.AlertDialog;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.text.InputType;
import android.util.Base64;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.DatePicker;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.ScrollView;
import android.widget.TextView;
import android.widget.TimePicker;

import com.android.application.payin.R;
import com.mobile.application.payin.common.interfaces.AsyncResponse;
import com.mobile.application.payin.common.serverconnections.ServerPost;
import com.mobile.application.payin.common.types.FormValueTargetType;
import com.mobile.application.payin.common.types.FormValueType;
import com.mobile.application.payin.common.utilities.CustomGson;
import com.mobile.application.payin.common.utilities.DoubleExpansion;
import com.mobile.application.payin.common.utilities.HandleCheck;
import com.mobile.application.payin.common.utilities.HandleServerError;
import com.mobile.application.payin.common.utilities.Pair;
import com.mobile.application.payin.dto.arguments.ControlIncidentCreateManualCheckArguments;
import com.mobile.application.payin.dto.results.ControlIncidentCreateManualCheckResult;
import com.mobile.application.payin.dto.results.ControlItemMobileGetAllResult;

import java.io.ByteArrayOutputStream;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.HashMap;
import java.util.Locale;
import java.util.TimeZone;

public class ManualFormulario extends AppCompatActivity implements AsyncResponse {
    private static Context context;
    private AsyncResponse delegate;
    private ScrollView scrollForms;
    private LayoutInflater inflater;
    private static FragmentManager fm;
    private static DialogFragment df;

    private ControlItemMobileGetAllResult.Item Form;
    private ControlIncidentCreateManualCheckArguments IncidentArguments;
    private static HashMap<Integer, Pair<ControlItemMobileGetAllResult.Value, ControlIncidentCreateManualCheckArguments.Value>> FormValues = new HashMap<>();
    private static Pair<ControlItemMobileGetAllResult.Value, ControlIncidentCreateManualCheckArguments.Value> pairImage;
    private static Button auxBtn;
    private int countFormValues = 0;

    private String route;
    private boolean saveFacial, saveTrack;

    private static final int REQUEST_IMAGE_CAPTURE = 1;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.formulario);

        Toolbar toolbar = (Toolbar) findViewById(R.id.tool_bar);
        SharedPreferences pref = getSharedPreferences(getResources().getString(R.string.prefs), MODE_PRIVATE);

        inflater = (LayoutInflater) this.getSystemService(LAYOUT_INFLATER_SERVICE);
        Form = (ControlItemMobileGetAllResult.Item) getIntent().getSerializableExtra("forms");
        IncidentArguments = (ControlIncidentCreateManualCheckArguments) getIntent().getSerializableExtra("arguments");
        saveFacial = getIntent().getBooleanExtra("saveFacial", false);
        saveTrack = getIntent().getBooleanExtra("saveTrack", false);
        route = getIntent().getStringExtra("route");

        context = this;
        delegate = this;
        fm = getSupportFragmentManager();

        scrollForms = (ScrollView) findViewById(R.id.scrollView);
        Button btnSend = (Button) findViewById(R.id.btnSend);

        setSupportActionBar(toolbar);
        toolbar.setNavigationIcon(ResourcesCompat.getDrawable(getResources(), R.drawable.ic_action_back, null));
        toolbar.setNavigationOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                finish();
            }
        });

        Boolean debug = pref.getBoolean("debug", false);

        if (debug) toolbar.setBackgroundColor(Color.parseColor("#ff1a1a"));
        else toolbar.setBackgroundColor(Color.parseColor("#e9af30"));

        btnSend.setFocusableInTouchMode(true);
        btnSend.setOnFocusChangeListener(new View.OnFocusChangeListener() {
            @Override
            public void onFocusChange(View v, boolean hasFocus) {
                if (hasFocus) {
                    fichar();
                }
            }
        });

        createForms();
    }

    @Override
    public void onBackPressed() {
        finish();
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        if (requestCode == REQUEST_IMAGE_CAPTURE && resultCode == RESULT_OK) {
            Bundle extras = data.getExtras();
            Bitmap imageBitmap = (Bitmap) extras.get("data");

            ByteArrayOutputStream ostream = new ByteArrayOutputStream();

            if (imageBitmap != null)
                imageBitmap.compress(Bitmap.CompressFormat.JPEG, 100, ostream);

            Bitmap compressImage = Bitmap.createScaledBitmap(imageBitmap, 240, 320, false);
            ByteArrayOutputStream ostream2 = new ByteArrayOutputStream();
            compressImage.compress(Bitmap.CompressFormat.JPEG, 100, ostream2);

            byte[] imageArray = ostream2.toByteArray();

            pairImage.arg.ValueImage = Base64.encodeToString(imageArray, 0);
        }
    }

    @Override
    public void onAsyncFinish(HashMap<String, String> map) {
        if (HandleServerError.Handle(map, this, this)) return ;

        if (map.get("route").contains(getResources().getString(R.string.apiControlIncidentMobileManualCheck))){
            ControlIncidentCreateManualCheckResult res = CustomGson.getGson().fromJson(map.get("json"), ControlIncidentCreateManualCheckResult.class);

            HandleCheck.Handle(saveTrack, this, this, res);
        }
    }

    private void createForms(){
        ControlIncidentCreateManualCheckArguments.Assign argAssing;
        ControlIncidentCreateManualCheckArguments.Value argValue;
        LinearLayout.LayoutParams txtViewParams = new  LinearLayout.LayoutParams(LinearLayout.LayoutParams.MATCH_PARENT, LinearLayout.LayoutParams.WRAP_CONTENT);
        txtViewParams.setMargins(0, 20, 0, 0);

        ControlItemMobileGetAllResult.Planning plan = Form.Plannings.get(0);
        if (plan == null) return ;

        for (ControlItemMobileGetAllResult.Assign resAssign : plan.Assigns) {
            argAssing = new ControlIncidentCreateManualCheckArguments.Assign();
            argAssing.Id = resAssign.Id;

            View childLayout = inflater.inflate(R.layout.formulario_subview, (ViewGroup) findViewById(R.id.llFormSubview), false);

            TextView txtFormName = (TextView) childLayout.findViewById(R.id.txtFormName);
            TextView txtFormObs = (TextView) childLayout.findViewById(R.id.txtFormObservatios);
            LinearLayout llFormValue = (LinearLayout) childLayout.findViewById(R.id.llFormValue);

            txtFormName.setText(resAssign.FormName);
            if (resAssign.FormObservations.equals("")) txtFormObs.setVisibility(View.GONE);
            else txtFormObs.setText(resAssign.FormObservations);

            for (ControlItemMobileGetAllResult.Value resValue : resAssign.Values) {
                TextView txt = new TextView(context);

                txt.setLayoutParams(txtViewParams);

                if (resValue.IsRequired) txt.setText(resValue.Name + "*");
                else txt.setText(resValue.Name);

                if (resValue.Target == FormValueTargetType.CREATION) {
                    if (resValue.ValueString != null) {
                        llFormValue.addView(txt);

                        txt = new TextView(context);
                        txt.setText(resValue.ValueString);
                    } else if (resValue.ValueBool != null) {
                        llFormValue.addView(txt);

                        txt = new TextView(context);
                        if (resValue.ValueBool) txt.setText(getResources().getText(R.string.yes));
                        else txt.setText(getResources().getText(R.string.no));
                    } else if (resValue.ValueNumeric != null) {
                        llFormValue.addView(txt);

                        txt = new TextView(context);
                        txt.setText("" + resValue.ValueNumeric);
                    } else if (resValue.ValueDateTime != null) {
                        llFormValue.addView(txt);

                        txt = new TextView(context);
                        txt.setText(resValue.ValueDateTime);
                    }

                    llFormValue.addView(txt);
                } else if (resValue.Target == FormValueTargetType.CHECK) {
                    llFormValue.addView(txt);

                    argValue = new ControlIncidentCreateManualCheckArguments.Value();
                    argValue.Id = resValue.Id;

                    if (resValue.Type == FormValueType.String)        createStringTxt(resValue, argValue, llFormValue);
                    else if (resValue.Type == FormValueType.Int)      createNumericTxt(resValue, argValue, llFormValue);
                    else if (resValue.Type == FormValueType.Double)   createNumericTxt(resValue, argValue, llFormValue);
                    else if (resValue.Type == FormValueType.Bool)     createCheckBox(resValue, argValue, llFormValue);
                    else if (resValue.Type == FormValueType.Datetime) createDateTimeBtn(resValue, argValue, llFormValue);
                    else if (resValue.Type == FormValueType.Date)     createDateBtn(resValue, argValue, llFormValue);
                    else if (resValue.Type == FormValueType.Time)     createTimeBtn(resValue, argValue, llFormValue);
                    else if (resValue.Type == FormValueType.Duration) createDurationBtn(resValue, argValue, llFormValue);
                    else if (resValue.Type == FormValueType.Image)    createImgBtn(resValue, argValue, llFormValue);

                    argAssing.Values.add(argValue);
                }
            }

            IncidentArguments.Item.Assigns.add(argAssing);
            scrollForms.addView(childLayout);
        }
    }

    private void createStringTxt(ControlItemMobileGetAllResult.Value resValue, ControlIncidentCreateManualCheckArguments.Value argValue, LinearLayout ll) {
        EditText et = new EditText(context);

        et.setId(countFormValues);
        et.setInputType(InputType.TYPE_CLASS_TEXT);
        if (resValue.ValueString != null) {
            et.setText(resValue.ValueString);
            argValue.ValueString = resValue.ValueString;
        }

        et.setOnFocusChangeListener(new View.OnFocusChangeListener() {
            @Override
            public void onFocusChange(View v, boolean hasFocus) {
                if (!hasFocus) {
                    EditText et = (EditText) v;


                    FormValues.get(et.getId()).arg.ValueString = et.getText().toString();
                }
            }
        });

        FormValues.put(countFormValues++, new Pair<>(resValue, argValue));

        ll.addView(et);
    }

    private void createNumericTxt(ControlItemMobileGetAllResult.Value resValue, ControlIncidentCreateManualCheckArguments.Value argValue, LinearLayout ll) {
        EditText et = new EditText(context);

        et.setId(countFormValues);
        if (resValue.Type == FormValueType.Int) et.setInputType(InputType.TYPE_CLASS_NUMBER);
        else if (resValue.Type == FormValueType.Double) et.setInputType(InputType.TYPE_NUMBER_FLAG_DECIMAL);

        if (resValue.ValueNumeric != null) {
            et.setText("" + resValue.ValueNumeric);
            argValue.ValueNumeric = resValue.ValueNumeric;
        }

        et.setOnFocusChangeListener(new View.OnFocusChangeListener() {
            @Override
            public void onFocusChange(View v, boolean hasFocus) {
                if (!hasFocus) {
                    EditText et = (EditText) v;

                    FormValues.get(et.getId()).arg.ValueNumeric = DoubleExpansion.getDoubleValueOf(et.getText().toString());
                }
            }
        });

        FormValues.put(countFormValues++, new Pair<>(resValue, argValue));

        ll.addView(et);
    }

    private void createDateTimeBtn(ControlItemMobileGetAllResult.Value resValue, ControlIncidentCreateManualCheckArguments.Value argValue, LinearLayout ll) {
        Button btn = new Button(context);

        btn.setId(countFormValues);

        if (resValue.ValueDateTime == null)
            btn.setText("Seleccionar una fecha");
        else {
            TimeZone tz = TimeZone.getTimeZone("UTC");
            SimpleDateFormat sdfUTC = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss'Z'", Locale.getDefault()),
                    sdfLocal = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss", Locale.getDefault());
            sdfUTC.setTimeZone(tz);

            String fechaLocal;

            Calendar cal = Calendar.getInstance();
            try {
                cal.setTimeInMillis(sdfUTC.parse(resValue.ValueDateTime).getTime());
                fechaLocal = sdfLocal.format(cal.getTime());
                btn.setText(fechaLocal);
            } catch (ParseException e) {
                btn.setText(resValue.ValueDateTime);
            }
            argValue.ValueDateTime = resValue.ValueDateTime;
        }

        btn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                auxBtn = (Button) v;

                df = new DateTimeDatePickerFragment();
                df.show(fm, "dateTimeDatePicker");
            }
        });

        FormValues.put(countFormValues++, new Pair<>(resValue, argValue));

        ll.addView(btn);
    }

    private void createDateBtn(ControlItemMobileGetAllResult.Value resValue, ControlIncidentCreateManualCheckArguments.Value argValue, LinearLayout ll) {
        Button btn = new Button(context);

        btn.setId(countFormValues);

        if (resValue.ValueDateTime == null)
            btn.setText("Seleccionar una fecha");
        else {
            TimeZone tz = TimeZone.getTimeZone("UTC");
            SimpleDateFormat sdfUTC = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss", Locale.getDefault()),
                    sdfLocal = new SimpleDateFormat("yyyy-MM-dd", Locale.getDefault());
            sdfUTC.setTimeZone(tz);

            String fechaLocal;

            Calendar cal = Calendar.getInstance();
            try {
                cal.setTimeInMillis(sdfUTC.parse(resValue.ValueDateTime).getTime());
                fechaLocal = sdfLocal.format(cal.getTime());
                btn.setText(fechaLocal);
            } catch (ParseException e) {
                btn.setText(resValue.ValueDateTime.substring(0, 10));
            }

            argValue.ValueDateTime = resValue.ValueDateTime;
        }

        btn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                auxBtn = (Button) v;

                df = new DatePickerFragment();
                df.show(fm, "datePicker");
            }
        });

        FormValues.put(countFormValues++, new Pair<>(resValue, argValue));

        ll.addView(btn);
    }

    private void createTimeBtn(ControlItemMobileGetAllResult.Value resValue, ControlIncidentCreateManualCheckArguments.Value argValue, LinearLayout ll) {
        Button btn = new Button(context);

        btn.setId(countFormValues);

        if (resValue.ValueDateTime == null)
            btn.setText("Seleccionar una hora");
        else {
            TimeZone tz = TimeZone.getTimeZone("UTC");
            SimpleDateFormat sdfUTC = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss'Z'", Locale.getDefault()),
                    sdfLocal = new SimpleDateFormat("HH:mm:ss", Locale.getDefault());
            sdfUTC.setTimeZone(tz);

            String fechaLocal;

            Calendar cal = Calendar.getInstance();
            try {
                cal.setTimeInMillis(sdfUTC.parse(resValue.ValueDateTime).getTime());
                fechaLocal = sdfLocal.format(cal.getTime());
                btn.setText(fechaLocal);
            } catch (ParseException e) {
                btn.setText(resValue.ValueDateTime);
            }

            argValue.ValueDateTime = resValue.ValueDateTime;
        }

        btn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                auxBtn = (Button) v;

                df = new TimePickerFragment();
                df.show(fm, "timePicker");
            }
        });

        FormValues.put(countFormValues++, new Pair<>(resValue, argValue));

        ll.addView(btn);
    }

    private void createDurationBtn(ControlItemMobileGetAllResult.Value resValue, ControlIncidentCreateManualCheckArguments.Value argValue, LinearLayout ll) {
        Button btn = new Button(context);

        btn.setId(countFormValues);

        if (resValue.ValueDateTime == null)
            btn.setText("Seleccionar una duracion");
        else {
            TimeZone tz = TimeZone.getTimeZone("UTC");
            SimpleDateFormat sdfUTC = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss", Locale.getDefault()),
                    sdfLocal = new SimpleDateFormat("HH:mm:ss", Locale.getDefault());
            sdfUTC.setTimeZone(tz);

            String fechaLocal;

            Calendar cal = Calendar.getInstance();
            try {
                cal.setTimeInMillis(sdfUTC.parse(resValue.ValueDateTime).getTime());
                fechaLocal = sdfLocal.format(cal.getTime());
            } catch (ParseException e) {
                return ;
            }

            btn.setText(fechaLocal);

            argValue.ValueDateTime = resValue.ValueDateTime;
        }

        btn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                auxBtn = (Button) v;

                df = new DurationPickerFragment();
                df.show(fm, "durationPicker");
            }
        });

        FormValues.put(countFormValues++, new Pair<>(resValue, argValue));

        ll.addView(btn);
    }

    private void createCheckBox(ControlItemMobileGetAllResult.Value resValue, ControlIncidentCreateManualCheckArguments.Value argValue, LinearLayout ll) {
        CheckBox ch = new CheckBox(context);

        ch.setId(countFormValues);

        if (resValue.ValueBool != null) {
            ch.setChecked(resValue.ValueBool);
            argValue.ValueBool = resValue.ValueBool;
        } else {
            argValue.ValueBool = false;
        }

        ch.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                CheckBox ch = (CheckBox) v;
                FormValues.get(ch.getId()).arg.ValueBool = ch.isChecked();
            }
        });

        FormValues.put(countFormValues++, new Pair<>(resValue, argValue));

        ll.addView(ch);
    }

    private void createImgBtn(ControlItemMobileGetAllResult.Value resValue, ControlIncidentCreateManualCheckArguments.Value argValue, LinearLayout ll) {
        Button btn = new Button(context);

        btn.setId(countFormValues);

        btn.setText("Hacer foto");

        btn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Button ch = (Button) v;

                dispatchTakePictureIntent(FormValues.get(ch.getId()));
            }
        });

        FormValues.put(countFormValues++, new Pair<>(resValue, argValue));

        ll.addView(btn);
    }

    private void dispatchTakePictureIntent(Pair<ControlItemMobileGetAllResult.Value, ControlIncidentCreateManualCheckArguments.Value> values) {
        Intent takePictureIntent = new Intent(MediaStore.ACTION_IMAGE_CAPTURE);
        pairImage = values;

        if (takePictureIntent.resolveActivity(getPackageManager()) != null) {
            startActivityForResult(takePictureIntent, REQUEST_IMAGE_CAPTURE);
        }
    }

    private void fichar() {
        boolean falta = false;

        for (Pair<ControlItemMobileGetAllResult.Value, ControlIncidentCreateManualCheckArguments.Value> pair : FormValues.values()){
            if (pair.res.IsRequired){
                ControlIncidentCreateManualCheckArguments.Value argValue = pair.arg;

                if (argValue.ValueString == null && argValue.ValueNumeric == null && argValue.ValueBool == null
                        && argValue.ValueDateTime == null && argValue.ValueImage == null){
                    falta = true; break;
                } else if (argValue.ValueString != null && argValue.ValueString.equals("")){
                    falta = true; break;
                } else if (argValue.ValueDateTime != null && argValue.ValueDateTime.equals("")){
                    falta = true; break;
                } else if (argValue.ValueImage != null && argValue.ValueImage.equals("")){
                    falta = true; break;
                }
            }
        }

        if (falta){
            AlertDialog.Builder builder = new AlertDialog.Builder(context);

            builder.setMessage("Hay campos requeridos sin completar.").setTitle("Se ha producido un error");

            builder.setPositiveButton(R.string.button_ok, new DialogInterface.OnClickListener() {
                public void onClick(DialogInterface dialog, int id) {
                    dialog.dismiss();
                }
            });

            builder.create().show();
        } else {
            if (saveFacial) {
                Intent i = new Intent(ManualFormulario.this, ManualFoto.class);

                i.putExtra("route", route);
                i.putExtra("arguments", IncidentArguments);
                i.putExtra("saveTrack", saveTrack);

                i.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TASK);

                startActivity(i);
            } else {
                ServerPost task = new ServerPost(context);
                task.delegate = delegate;
                task.execute(route, CustomGson.getGson().toJson(IncidentArguments));
            }
        }
    }

    public static class DateTimeDatePickerFragment extends DialogFragment implements DatePickerDialog.OnDateSetListener {
        @Override @NonNull
        public Dialog onCreateDialog( Bundle savedInstanceState) {
            Calendar cal = Calendar.getInstance();
            return new DatePickerDialog(getActivity(), this, cal.get(Calendar.YEAR), cal.get(Calendar.MONTH), cal.get(Calendar.DAY_OF_MONTH));
        }

        public void onDateSet(DatePicker view, int dialogYear, int dialogMonth, int dialogDay) {
            Bundle bundle = new Bundle();
            bundle.putInt("year", dialogYear);
            bundle.putInt("month", dialogMonth);
            bundle.putInt("day", dialogDay);

            df = new DateTimeTimePickerFragment();
            df.setArguments(bundle);
            df.show(fm, "dateTimeDatePicker");
        }
    }

    public static class DateTimeTimePickerFragment extends DialogFragment implements TimePickerDialog.OnTimeSetListener {
        private int year, month, day;

        @Override @NonNull
        public Dialog onCreateDialog(Bundle savedInstanceState) {
            Calendar cal = Calendar.getInstance();

            Bundle bundle = this.getArguments();

            year = bundle.getInt("year", cal.get(Calendar.YEAR));
            month = bundle.getInt("month", cal.get(Calendar.MONTH));
            day = bundle.getInt("day", cal.get(Calendar.DAY_OF_MONTH));

            return new TimePickerDialog(getActivity(), this, cal.get(Calendar.HOUR_OF_DAY), cal.get(Calendar.MINUTE), true);
        }

        public void onTimeSet(TimePicker view, int dialogHour, int dialogMinute) {
            TimeZone tz = TimeZone.getTimeZone("UTC");
            SimpleDateFormat sdfUTC = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss'Z'", Locale.getDefault()),
                    sdfLocal = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss", Locale.getDefault());
            sdfUTC.setTimeZone(tz);

            String fechaLocal = String.format("%04d-%02d-%02d %02d:%02d:00", year, month, day, dialogHour, dialogMinute),
                    fechaServer;

            Calendar cal = Calendar.getInstance();
            try {
                cal.setTimeInMillis(sdfLocal.parse(fechaLocal).getTime());
                fechaServer = sdfUTC.format(cal.getTime());
            } catch (ParseException e) {
                return ;
            }

            int id = auxBtn.getId();

            auxBtn.setText(fechaLocal);

            FormValues.get(id).arg.ValueDateTime = fechaServer;
        }
    }

    public static class DatePickerFragment extends DialogFragment implements DatePickerDialog.OnDateSetListener {
        @Override @NonNull
        public Dialog onCreateDialog( Bundle savedInstanceState) {
            Calendar cal = Calendar.getInstance();
            return new DatePickerDialog(getActivity(), this, cal.get(Calendar.YEAR), cal.get(Calendar.MONTH), cal.get(Calendar.DAY_OF_MONTH));
        }

        public void onDateSet(DatePicker view, int dialogYear, int dialogMonth, int dialogDay) {
            String fecha = String.format("%04d-%02d-%02d", dialogYear, dialogMonth + 1, dialogDay);

            int id = auxBtn.getId();

            auxBtn.setText(fecha);

            fecha = fecha.concat("T00:00:00Z");

            FormValues.get(id).arg.ValueDateTime = fecha;
        }
    }

    public static class TimePickerFragment extends DialogFragment implements TimePickerDialog.OnTimeSetListener {
        @Override @NonNull
        public Dialog onCreateDialog(Bundle savedInstanceState) {
            Calendar cal = Calendar.getInstance();
            return new TimePickerDialog(getActivity(), this, cal.get(Calendar.HOUR_OF_DAY), cal.get(Calendar.MINUTE), true);
        }

        public void onTimeSet(TimePicker view, int dialogHour, int dialogMinute) {
            TimeZone tz = TimeZone.getTimeZone("UTC");
            SimpleDateFormat sdfUTC = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss'Z'", Locale.getDefault()),
                    sdfLocal = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss", Locale.getDefault());
            sdfUTC.setTimeZone(tz);

            Calendar cal = Calendar.getInstance();
            String fechaServer,
                    fechaLocal = String.format("%04d-%02d-%02dT%02d:%02d:00", cal.get(Calendar.YEAR), cal.get(Calendar.MONTH) + 1,
                        cal.get(Calendar.DAY_OF_MONTH), dialogHour, dialogMinute);

            try {
                cal.setTimeInMillis(sdfLocal.parse(fechaLocal).getTime());
                fechaServer = sdfUTC.format(cal.getTime());
            } catch (ParseException e) {
                return ;
            }

            int id = auxBtn.getId();

            auxBtn.setText(fechaLocal);

            FormValues.get(id).arg.ValueDateTime = fechaServer;
        }
    }

    public static class DurationPickerFragment extends DialogFragment implements TimePickerDialog.OnTimeSetListener {
        @Override @NonNull
        public Dialog onCreateDialog(Bundle savedInstanceState) {
            Calendar cal = Calendar.getInstance();
            return new TimePickerDialog(getActivity(), this, cal.get(Calendar.HOUR_OF_DAY), cal.get(Calendar.MINUTE), true);
        }

        public void onTimeSet(TimePicker view, int dialogHour, int dialogMinute) {
            String duration = String.format("%02d:%02d:00", dialogHour, dialogMinute);

            int id = auxBtn.getId();
            auxBtn.setText(duration);

            FormValues.get(id).arg.ValueDateTime = duration;
        }
    }
}
