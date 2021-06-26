package com.mobile.application.payin.views;

import android.app.AlertDialog;
import android.app.Dialog;
import android.app.ProgressDialog;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.res.Configuration;
import android.graphics.Color;
import android.graphics.RectF;
import android.graphics.drawable.LayerDrawable;
import android.net.Uri;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.v4.content.ContextCompat;
import android.support.v4.content.res.ResourcesCompat;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.util.Log;
import android.util.TypedValue;
import android.view.KeyEvent;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuItem;
import android.view.MotionEvent;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.BaseAdapter;
import android.widget.Button;
import android.widget.ListView;
import android.widget.Spinner;
import android.widget.TextView;

import com.alamkanak.weekview.DateTimeInterpreter;
import com.alamkanak.weekview.WeekView;
import com.alamkanak.weekview.WeekViewEvent;
import com.android.application.payin.R;
import com.mobile.application.payin.common.serverconnections.ServerGet;
import com.mobile.application.payin.common.interfaces.AsyncResponse;
import com.mobile.application.payin.common.types.CalendarItemType;
import com.mobile.application.payin.common.utilities.CustomGson;
import com.mobile.application.payin.common.utilities.HandleServerError;
import com.mobile.application.payin.common.utilities.IntegerExpansion;
import com.mobile.application.payin.domain.CustomWeekViewEvent;
import com.mobile.application.payin.common.utilities.IconsWithBadgeCount;
import com.mobile.application.payin.dto.results.ControlItemMobileGetAllResult;
import com.mobile.application.payin.dto.results.ControlItemMobileGetSelectorResult;
import com.mobile.application.payin.dto.results.ControlPresenceMobileGetTimetableResult;

import java.lang.reflect.Method;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.HashMap;
import java.util.List;
import java.util.Locale;
import java.util.TimeZone;

public class Calendario extends AppCompatActivity implements WeekView.MonthChangeListener, WeekView.EventClickListener,
        WeekView.EventLongPressListener, WeekView.EmptyViewLongPressListener, AsyncResponse {

    private static final int TYPE_DAY_VIEW = 1;
    private static final int TYPE_THREE_DAY_VIEW = 2;
    private static final int TYPE_WEEK_VIEW = 3;
    private int mWeekViewType = TYPE_THREE_DAY_VIEW;

    private static boolean calendarVisible = true;
    private static boolean planningVisible = true;
    private static boolean presenceVisible = false;
    private static int filterItem = 0;
    private static int scheduleYear, scheduleMonth, scheduleDay;

    private Toolbar toolbar;
    private Menu mMenu;
    private WeekView mWeekView;
    private Spinner spinner;
    private TextView txtDay;
    private View linea;
    private ListView lvItems;
    private static  Context context;
    private static AsyncResponse delegate;
    private ProgressDialog progreso = null;

    private DateTimeInterpreter oDateInterpreter;
    private final HashMap<String, ArrayList<CustomWeekViewEvent>> monthsDown = new HashMap<>();
    private HashMap<Integer, ControlItemMobileGetSelectorResult.Item> items = new HashMap<>();

    private boolean error = false;
    private float mLastX;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.calendario);

        context = this;
        delegate = this;

        toolbar = (Toolbar) findViewById(R.id.tool_bar);
        setSupportActionBar(toolbar);
        toolbar.setNavigationIcon(ResourcesCompat.getDrawable(getResources(), R.drawable.ic_action_back, null));
        toolbar.setNavigationOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                finish();
            }
        });

        mWeekView = (WeekView) findViewById(R.id.weekView);
        linea = findViewById(R.id.lineaSeparacion);
        spinner = (Spinner) findViewById(R.id.spinner);
        txtDay = (TextView) findViewById(R.id.txtDay);
        lvItems = (ListView) findViewById(R.id.lvItems);

        SharedPreferences pref = getSharedPreferences(getResources().getString(R.string.prefs), MODE_PRIVATE);
        boolean debug = pref.getBoolean("debug", false);

        if (debug){
            toolbar.setBackgroundColor(Color.parseColor("#ff1a1a"));
            txtDay.setBackgroundColor(Color.parseColor("#ff1a1a"));
        } else {
            toolbar.setBackgroundColor(Color.parseColor("#e9af30"));
            txtDay.setBackgroundColor(Color.parseColor("#e9af30"));
        }

        configureCalendar();
        configureListView();
        configureSpinner();

        progreso = new ProgressDialog(context);

        scheduleYear = Calendar.getInstance().get(Calendar.YEAR);
        scheduleMonth = Calendar.getInstance().get(Calendar.MONTH) + 1;
        scheduleDay = Calendar.getInstance().get(Calendar.DAY_OF_MONTH);
    }

    private void configureCalendar(){
        oDateInterpreter = mWeekView.getDateTimeInterpreter();

        mWeekView.setDateTimeInterpreter(new DateTimeInterpreter() {
            @Override
            public String interpretDate(Calendar date) {
                String fecha = oDateInterpreter.interpretDate(date);

                String[] aux = fecha.split(" ");

                fecha = aux[0] + " ";

                aux = aux[1].split("/");

                fecha += aux[1] + "/" + aux[0];

                return fecha;
            }

            @Override
            public String interpretTime(int hour) {
                return String.format("%02d:00", hour);
            }
        });

        Calendar today = Calendar.getInstance();
        today.add(Calendar.DAY_OF_MONTH, -1);
        mWeekView.goToDate(today);
        int hour = Calendar.getInstance().get(Calendar.HOUR_OF_DAY);
        mWeekView.goToHour((hour - 2 > 17) ? 17 : (hour - 2 < 0) ? 0 : (hour - 2));

        mWeekView.setOnEventClickListener(this);

        mWeekView.setMonthChangeListener(this);

        mWeekView.setEmptyViewLongPressListener(this);

        mWeekView.setEventLongPressListener(this);

        mWeekView.setScrollListener(new WeekView.ScrollListener() {
            @Override
            public void onFirstVisibleDayChanged(Calendar newDate, Calendar oldDate) {
                scheduleYear = newDate.get(Calendar.YEAR);
                scheduleMonth = newDate.get(Calendar.MONTH) + 1;
                scheduleDay = newDate.get(Calendar.DAY_OF_MONTH);
            }
        });
    }

    private void configureSpinner(){
        spinner.setOnItemSelectedListener(
                new AdapterView.OnItemSelectedListener() {
                    @Override
                    public void onItemSelected(AdapterView<?> arg0, View arg1, int arg2, long arg3) {
                        int position = spinner.getSelectedItemPosition();
                        String name = spinner.getSelectedItem().toString();

                        if (position == 0) {
                            planningVisible = true;
                            presenceVisible = false;

                            mMenu.findItem(R.id.itemToDo).setVisible(true);
                            mMenu.findItem(R.id.itemDone).setVisible(false);

                            filterItem = 0;
                        } else {
                            planningVisible = true;
                            presenceVisible = true;

                            mMenu.findItem(R.id.itemToDo).setVisible(false);
                            mMenu.findItem(R.id.itemDone).setVisible(false);
                            for (ControlItemMobileGetSelectorResult.Item item : items.values()) {
                                if (item.Value.equals(name)) {
                                    filterItem = item.Id;
                                }
                            }
                        }

                        mWeekView.notifyDatasetChanged();
                    }

                    @Override
                    public void onNothingSelected(AdapterView<?> arg0) {
                    }
                }
        );
    }

    private void configureListView(){
        lvItems.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                SimpleDateFormat sdfUTC = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss'Z'", Locale.getDefault()), sdf = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss", Locale.getDefault());
                sdfUTC.setTimeZone(TimeZone.getTimeZone("UTC"));
                Calendar start = Calendar.getInstance(), end = (Calendar) start.clone();

                TextView txt;
                Button dialogButton;
                final Dialog dialog = new Dialog(Calendario.this);

                TextView tv = (TextView) dialog.findViewById(android.R.id.title);
                if (tv != null)
                    tv.setSingleLine(false);

                int eventId = Integer.parseInt(((TextView) view.findViewById(R.id.txtId)).getText().toString());
                int eventType = Integer.parseInt(((TextView) view.findViewById(R.id.txtType)).getText().toString());
                CustomWeekViewEvent event = null;

                if (monthsDown.containsKey(String.format("%04d-%02d", scheduleYear, scheduleMonth)))
                    for (CustomWeekViewEvent eventAux : monthsDown.get(String.format("%04d-%02d", scheduleYear, scheduleMonth))) {
                        if (eventId == eventAux.getId() && eventType == eventAux.getType()) {
                            event = eventAux;
                            break;
                        }
                    }

                if (event == null) return;

                dialog.setTitle(event.getName());

                dialog.setContentView(R.layout.calendar_dialog);

                txt = (TextView) dialog.findViewById(R.id.txtDate);
                try {
                    if (event.getType() == CalendarItemType.RoundPresence || event.getType() == CalendarItemType.RoundPlanning) {
                        Calendar cal = Calendar.getInstance();
                        cal.setTimeInMillis(start.getTimeInMillis() + 10 * 60 * 1000);

                        txt.setText("Paso a las : " + sdf.format(cal.getTime()));
                    } else if (event.getType() == CalendarItemType.OpenPresence) {
                        start.setTimeInMillis(sdfUTC.parse(event.getStart()).getTime());
                        txt.setText("En curso desde: " + sdf.format(start.getTime()));
                    } else {
                        start.setTimeInMillis(sdfUTC.parse(event.getStart()).getTime());
                        end.setTimeInMillis(sdfUTC.parse(event.getEnd()).getTime());
                        txt.setText("Comienzo: " + sdf.format(start.getTime()) +
                                "\nFinalización: " + sdf.format(end.getTime()));
                    }
                } catch (ParseException e) {
                    System.err.println(e.getMessage());
                }

                txt = (TextView) dialog.findViewById(R.id.txtObservations);
                txt.setVisibility(View.GONE);

                txt = (TextView) dialog.findViewById(R.id.txtValues);
                txt.setVisibility(View.GONE);

                final int Id = event.getId();
                final int ItemId = event.getItemId();
                final String Location = event.getLocation();

                Calendar yesterday = Calendar.getInstance(), tomorrow = (Calendar) yesterday.clone(), startTime = (Calendar) yesterday.clone();
                yesterday.add(Calendar.DAY_OF_MONTH, -1);
                tomorrow.add(Calendar.DAY_OF_MONTH, 1);

                try {
                    startTime.setTimeInMillis(sdfUTC.parse(event.getStart()).getTime());
                } catch (ParseException e) {
                    dialog.show();
                    return;
                }

                switch (event.getType()) {
                    case CalendarItemType.Planning:
                        if (startTime.after(yesterday) && startTime.before(tomorrow)) {
                            dialogButton = (Button) dialog.findViewById(R.id.btnCheck);
                            dialogButton.setVisibility(View.VISIBLE);
                            dialogButton.setOnClickListener(new View.OnClickListener() {
                                @Override
                                public void onClick(View v) {
                                    String query = "?ItemId=" + ItemId + "&PlanningItemId=" + Id;

                                    ServerGet tarea = new ServerGet(context);
                                    tarea.delegate = delegate;
                                    tarea.execute(getResources().getString(R.string.apiControlPlanningCheckMobileGet) + query);

                                    dialog.dismiss();
                                }
                            });
                        }
                        break;
                    case CalendarItemType.RoundPlanning:
                        if (startTime.after(yesterday) && startTime.before(tomorrow)) {
                            dialogButton = (Button) dialog.findViewById(R.id.btnCheck);
                            dialogButton.setVisibility(View.VISIBLE);
                            dialogButton.setOnClickListener(new View.OnClickListener() {
                                @Override
                                public void onClick(View v) {
                                    String query = "?ItemId=" + ItemId + "&PlanningCheckId=" + Id;

                                    ServerGet tarea = new ServerGet(context);
                                    tarea.delegate = delegate;
                                    tarea.execute(getResources().getString(R.string.apiControlPlanningCheckMobileGet) + query);

                                    dialog.dismiss();
                                }
                            });
                        }

                        if (event.getLocation() != null && !event.getLocation().equals("")) {
                            dialogButton = (Button) dialog.findViewById(R.id.btnLocation);
                            dialogButton.setVisibility(View.VISIBLE);
                            dialogButton.setOnClickListener(new View.OnClickListener() {
                                @Override
                                public void onClick(View v) {
                                    Uri gmIntentUri = Uri.parse("geo:0,0?q=" + Location);
                                    Intent mapIntent = new Intent(Intent.ACTION_VIEW, gmIntentUri);
                                    mapIntent.setPackage("com.google.android.apps.maps");
                                    if (mapIntent.resolveActivity(getPackageManager()) != null) {
                                        startActivity(mapIntent);
                                    }
                                }
                            });
                        }

                        break;
                }

                dialog.show();
            }
        });
    }

    @Override
    protected void onResume() {
        if (items.isEmpty() || monthsDown.isEmpty()) {
            progreso.setProgressStyle(ProgressDialog.STYLE_SPINNER);
            progreso.setTitle("Descargando datos");
            progreso.setMessage("Descargando...");
            progreso.setIndeterminate(true);
            progreso.setCanceledOnTouchOutside(false);
            progreso.setCancelable(false);
            progreso.show();
        }

        if (items.isEmpty()) {
            ServerGet tarea = new ServerGet(this);
            tarea.delegate = this;
            tarea.showProgress = false;
            tarea.execute(getResources().getString(R.string.apiControlItemMobileGetSelector));
        }

        if (getResources().getConfiguration().orientation == Configuration.ORIENTATION_LANDSCAPE) {
            mWeekViewType = TYPE_WEEK_VIEW;
            mWeekView.setNumberOfVisibleDays(7);

            // Lets change some dimensions to best fit the view.
            mWeekView.setColumnGap((int) TypedValue.applyDimension(TypedValue.COMPLEX_UNIT_DIP, 2, getResources().getDisplayMetrics()));
            mWeekView.setTextSize((int) TypedValue.applyDimension(TypedValue.COMPLEX_UNIT_SP, 10, getResources().getDisplayMetrics()));
            mWeekView.setEventTextSize((int) TypedValue.applyDimension(TypedValue.COMPLEX_UNIT_SP, 10, getResources().getDisplayMetrics()));
        } else if (mWeekView.getNumberOfVisibleDays() == 7) {
            mWeekViewType = TYPE_THREE_DAY_VIEW;
            mWeekView.setNumberOfVisibleDays(3);

            // Lets change some dimensions to best fit the view.
            mWeekView.setColumnGap((int) TypedValue.applyDimension(TypedValue.COMPLEX_UNIT_DIP, 8, getResources().getDisplayMetrics()));
            mWeekView.setTextSize((int) TypedValue.applyDimension(TypedValue.COMPLEX_UNIT_SP, 12, getResources().getDisplayMetrics()));
            mWeekView.setEventTextSize((int) TypedValue.applyDimension(TypedValue.COMPLEX_UNIT_SP, 12, getResources().getDisplayMetrics()));
        }
        super.onResume();
    }

    @Override
    public void onBackPressed() {
        finish();
    }

    @Override
    protected void onSaveInstanceState(Bundle outState) {
        super.onSaveInstanceState(outState);

        outState.putSerializable("items", items);

        outState.putBoolean("calendarVisible", calendarVisible);
        outState.putString("date", String.format("%04d-%02d-%02d", scheduleYear, scheduleMonth, scheduleDay));
    }

    @SuppressWarnings("unchecked")
    @Override
    protected void onRestoreInstanceState(@NonNull Bundle savedInstanceState) {
        ArrayList<CustomWeekViewEvent> events = new ArrayList<>();
        Adaptador adap;
        Calendar now = Calendar.getInstance();
        SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd", Locale.getDefault());

        super.onRestoreInstanceState(savedInstanceState);

        items = (HashMap<Integer, ControlItemMobileGetSelectorResult.Item>) savedInstanceState.get("items");

        if (savedInstanceState.getBoolean("calendarVisible")) {
            try {
                now.setTimeInMillis(sdf.parse(savedInstanceState.getString("date")).getTime());
            } catch (ParseException e) {
                now = Calendar.getInstance();
            }
            mWeekView.goToDate(now);
        } else {
            calendarVisible = false;

            mWeekView.setVisibility(View.GONE);
            spinner.setVisibility(View.GONE);
            linea.setVisibility(View.GONE);

            txtDay.setVisibility(View.VISIBLE);
            lvItems.setVisibility(View.VISIBLE);

            try {
                now.setTimeInMillis(sdf.parse(savedInstanceState.getString("date")).getTime());
            } catch (ParseException e) {
                now = Calendar.getInstance();
            }

            scheduleYear = now.get(Calendar.YEAR);
            scheduleMonth = now.get(Calendar.MONTH) + 1;
            scheduleDay = now.get(Calendar.DAY_OF_MONTH);

            txtDay.setText(String.format("%02d-%02d-%04d", scheduleDay, scheduleMonth, scheduleYear));

            if (monthsDown.containsKey(String.format("%04d-%02d", scheduleYear, scheduleMonth)))
                for (CustomWeekViewEvent event : monthsDown.get(String.format("%04d-%02d", scheduleYear, scheduleMonth))){
                    int startDay = Integer.parseInt(event.getStart().substring(8, 10)), endDay = Integer.parseInt(event.getEnd().substring(8, 10));
                    if (startDay == scheduleDay || endDay == scheduleDay){
                        events.add(event);
                    }
                }

            adap = new Adaptador(context, events);

            lvItems.setAdapter(adap);

            String since = String.format("%04d-%02d-01", scheduleYear, scheduleMonth);
            String until = String.format("%04d-%02d-%02d", scheduleYear, scheduleMonth, lastDayOfMonth(scheduleYear, scheduleMonth));

            ServerGet tarea = new ServerGet(this);
            tarea.delegate = this;
            tarea.showProgress = false;
            tarea.execute(getResources().getString(R.string.apiControlPresenceMobileTimetable) + "?since=" + since + "&until=" + until);
        }
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        mMenu = menu;

        MenuItem item;

        getMenuInflater().inflate(R.menu.menu_calendario, menu);

        try {
            item = menu.findItem(R.id.itemToday);
            LayerDrawable icon = (LayerDrawable) item.getIcon();

            // Update LayerDrawable's BadgeDrawable
            IconsWithBadgeCount.setDayBadgeCount(this, icon, Integer.toString(Calendar.getInstance().get(Calendar.DAY_OF_MONTH)));
        } catch (NullPointerException e) {
            System.err.println(e.getMessage() == null ? "" : e.getMessage());
        }

        if (!calendarVisible) {
            mMenu.findItem(R.id.itemToDo).setVisible(false);
            mMenu.findItem(R.id.itemDone).setVisible(false);

            mMenu.findItem(R.id.itemAgenda).setVisible(false);
            mMenu.findItem(R.id.itemCalendar).setVisible(true);

            mMenu.findItem(R.id.itemArrowLeft).setVisible(true);
            mMenu.findItem(R.id.itemArrowRight).setVisible(true);
        }

        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        int id = item.getItemId(); Intent i;
        ArrayList<CustomWeekViewEvent> events = new ArrayList<>();
        Adaptador adap;
        Calendar now = Calendar.getInstance();
        SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd", Locale.getDefault());

        switch (id){
            case R.id.itemToday:
                try {
                    Calendar today = Calendar.getInstance();
                    today.add(Calendar.DAY_OF_MONTH, -1);
                    mWeekView.goToDate(today);
                    int hour = Calendar.getInstance().get(Calendar.HOUR_OF_DAY);
                    mWeekView.goToHour( (hour - 2 > 17)?17:(hour - 2) );
                }catch (IllegalArgumentException e){
                    Log.e("Calendario", e.getLocalizedMessage());
                }
                return true;
            case R.id.itemDayView:
                if (mWeekViewType != TYPE_DAY_VIEW) {
                    mWeekViewType = TYPE_DAY_VIEW;
                    mWeekView.setNumberOfVisibleDays(1);

                    // Lets change some dimensions to best fit the view.
                    mWeekView.setColumnGap((int) TypedValue.applyDimension(TypedValue.COMPLEX_UNIT_DIP, 8, getResources().getDisplayMetrics()));
                    mWeekView.setTextSize((int) TypedValue.applyDimension(TypedValue.COMPLEX_UNIT_SP, 12, getResources().getDisplayMetrics()));
                    mWeekView.setEventTextSize((int) TypedValue.applyDimension(TypedValue.COMPLEX_UNIT_SP, 12, getResources().getDisplayMetrics()));
                }
                return true;
            case R.id.itemThreeDayView:
                if (mWeekViewType != TYPE_THREE_DAY_VIEW) {
                    mWeekViewType = TYPE_THREE_DAY_VIEW;
                    mWeekView.setNumberOfVisibleDays(3);

                    // Lets change some dimensions to best fit the view.
                    mWeekView.setColumnGap((int) TypedValue.applyDimension(TypedValue.COMPLEX_UNIT_DIP, 8, getResources().getDisplayMetrics()));
                    mWeekView.setTextSize((int) TypedValue.applyDimension(TypedValue.COMPLEX_UNIT_SP, 12, getResources().getDisplayMetrics()));
                    mWeekView.setEventTextSize((int) TypedValue.applyDimension(TypedValue.COMPLEX_UNIT_SP, 12, getResources().getDisplayMetrics()));
                }
                return true;
            case R.id.itemRefresh:
                recreate();
                return true;
            case R.id.itemCheck:
                i = new Intent(this, ManualItems.class);
                i.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TASK);
                i.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                startActivity(i);
                return true;
            case R.id.itemAgenda:
                calendarVisible = false;

                mMenu.findItem(R.id.itemToDo).setVisible(false);
                mMenu.findItem(R.id.itemDone).setVisible(false);

                mMenu.findItem(R.id.itemAgenda).setVisible(false);
                mMenu.findItem(R.id.itemCalendar).setVisible(true);

                mMenu.findItem(R.id.itemArrowLeft).setVisible(true);
                mMenu.findItem(R.id.itemArrowRight).setVisible(true);

                mWeekView.setVisibility(View.GONE);
                spinner.setVisibility(View.GONE);
                linea.setVisibility(View.GONE);

                txtDay.setVisibility(View.VISIBLE);
                lvItems.setVisibility(View.VISIBLE);

                scheduleYear = now.get(Calendar.YEAR);
                scheduleMonth = now.get(Calendar.MONTH) + 1;
                scheduleDay = now.get(Calendar.DAY_OF_MONTH);

                txtDay.setText(String.format("%02d-%02d-%04d", scheduleDay, scheduleMonth, scheduleYear));

                if (monthsDown.containsKey(String.format("%04d-%02d", scheduleYear, scheduleMonth)))
                    for (CustomWeekViewEvent event : monthsDown.get(String.format("%04d-%02d", scheduleYear, scheduleMonth))){
                        int startDay = Integer.parseInt(event.getStart().substring(8, 10)), endDay = Integer.parseInt(event.getEnd().substring(8, 10));
                        if (startDay == scheduleDay || endDay == scheduleDay){
                            events.add(event);
                        }
                    }

                adap = new Adaptador(context, events);

                lvItems.setAdapter(adap);

                return true;
            case R.id.itemCalendar:
                calendarVisible = true;

                if (planningVisible) mMenu.findItem(R.id.itemToDo).setVisible(true);
                if (presenceVisible) mMenu.findItem(R.id.itemDone).setVisible(true);

                mMenu.findItem(R.id.itemAgenda).setVisible(true);
                mMenu.findItem(R.id.itemCalendar).setVisible(false);

                mMenu.findItem(R.id.itemArrowLeft).setVisible(false);
                mMenu.findItem(R.id.itemArrowRight).setVisible(false);

                mWeekView.setVisibility(View.VISIBLE);
                spinner.setVisibility(View.VISIBLE);
                linea.setVisibility(View.VISIBLE);

                txtDay.setVisibility(View.GONE);
                lvItems.setVisibility(View.GONE);

                try {
                    now.setTimeInMillis(sdf.parse(String.format("%04d-%02d-%02d", scheduleYear, scheduleMonth, scheduleDay)).getTime());
                    mWeekView.goToDate(now);
                } catch (ParseException e) {
                    mWeekView.goToDate(Calendar.getInstance());
                }

                return true;
            case R.id.itemToDo:
                planningVisible = false;
                presenceVisible = true;

                mMenu.findItem(R.id.itemToDo).setVisible(false);
                mMenu.findItem(R.id.itemDone).setVisible(true);

                if (calendarVisible){
                    mWeekView.notifyDatasetChanged();
                }

                return true;
            case R.id.itemDone:
                planningVisible = true;
                presenceVisible = false;

                mMenu.findItem(R.id.itemToDo).setVisible(true);
                mMenu.findItem(R.id.itemDone).setVisible(false);

                if (calendarVisible){
                    mWeekView.notifyDatasetChanged();
                }

                return true;
            case R.id.itemArrowLeft:
                try {
                    now.setTimeInMillis(sdf.parse(String.format("%04d-%02d-%02d", scheduleYear, scheduleMonth, scheduleDay)).getTime());
                } catch (ParseException e) {
                    System.err.println(e.getMessage());
                }
                now.add(Calendar.DAY_OF_MONTH, -1);

                mWeekView.goToDate(now);

                scheduleYear = now.get(Calendar.YEAR);
                scheduleMonth = now.get(Calendar.MONTH) + 1;
                scheduleDay = now.get(Calendar.DAY_OF_MONTH);

                txtDay.setText(String.format("%02d-%02d-%04d", scheduleDay, scheduleMonth, scheduleYear));

                if (monthsDown.containsKey(String.format("%04d-%02d", scheduleYear, scheduleMonth)))
                    for (CustomWeekViewEvent event : monthsDown.get(String.format("%04d-%02d", scheduleYear, scheduleMonth))){
                        int startDay = Integer.parseInt(event.getStart().substring(8, 10)), endDay = Integer.parseInt(event.getEnd().substring(8, 10));
                        if (startDay == scheduleDay || endDay == scheduleDay){
                            events.add(event);
                        }
                    }

                adap = new Adaptador(context, events);

                lvItems.setAdapter(adap);

                return true;
            case R.id.itemArrowRight:
                try {
                    now.setTimeInMillis(sdf.parse(String.format("%04d-%02d-%02d", scheduleYear, scheduleMonth, scheduleDay)).getTime());
                } catch (ParseException e) {
                    System.err.println(e.getMessage());
                }
                now.add(Calendar.DAY_OF_MONTH, 1);

                mWeekView.goToDate(now);

                scheduleYear = now.get(Calendar.YEAR);
                scheduleMonth = now.get(Calendar.MONTH) + 1;
                scheduleDay = now.get(Calendar.DAY_OF_MONTH);

                txtDay.setText(String.format("%02d-%02d-%04d", scheduleDay, scheduleMonth, scheduleYear));

                if (monthsDown.containsKey(String.format("%04d-%02d", scheduleYear, scheduleMonth)))
                    for (CustomWeekViewEvent event : monthsDown.get(String.format("%04d-%02d", scheduleYear, scheduleMonth))){
                        int startDay = Integer.parseInt(event.getStart().substring(8, 10)), endDay = Integer.parseInt(event.getEnd().substring(8, 10));
                        if (startDay == scheduleDay || endDay == scheduleDay){
                            events.add(event);
                        }
                    }

                adap = new Adaptador(context, events);

                lvItems.setAdapter(adap);

                return true;
        }
        return super.onOptionsItemSelected(item);
    }

    @Override
    public boolean onMenuOpened(int featureId, Menu menu) {
        if(featureId == Window.FEATURE_ACTION_BAR && menu != null){
            if(menu.getClass().getSimpleName().equals("MenuBuilder")){
                try{
                    Method m = menu.getClass().getDeclaredMethod("setOptionalIconsVisible", Boolean.TYPE);
                    m.setAccessible(true);
                    m.invoke(menu, true);
                }
                catch(Exception e){
                    throw new RuntimeException(e);
                }
            }
        }
        return super.onMenuOpened(featureId, menu);
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
    public boolean dispatchTouchEvent(@NonNull MotionEvent ev) {
        if (calendarVisible) return super.dispatchTouchEvent(ev);

        ArrayList<CustomWeekViewEvent> events = new ArrayList<>();
        Adaptador adap;
        Calendar now = Calendar.getInstance();
        SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd", Locale.getDefault());

        switch (ev.getAction()) {
            case MotionEvent.ACTION_DOWN:
                mLastX = ev.getX();
                break;

            case MotionEvent.ACTION_UP:
                float curX = ev.getX();
                float mDiffX = curX - mLastX;

                if (mDiffX < -20) {
                    try {
                        now.setTimeInMillis(sdf.parse(String.format("%04d-%02d-%02d", scheduleYear, scheduleMonth, scheduleDay)).getTime());
                    } catch (ParseException e) {
                        System.err.println(e.getMessage());
                    }
                    now.add(Calendar.DAY_OF_MONTH, 1);

                    mWeekView.goToDate(now);

                    scheduleYear = now.get(Calendar.YEAR);
                    scheduleMonth = now.get(Calendar.MONTH) + 1;
                    scheduleDay = now.get(Calendar.DAY_OF_MONTH);

                    txtDay.setText(String.format("%02d-%02d-%04d", scheduleDay, scheduleMonth, scheduleYear));

                    if (monthsDown.containsKey(String.format("%04d-%02d", scheduleYear, scheduleMonth)))
                        for (CustomWeekViewEvent event : monthsDown.get(String.format("%04d-%02d", scheduleYear, scheduleMonth))){
                            int startDay = Integer.parseInt(event.getStart().substring(8, 10)), endDay = Integer.parseInt(event.getEnd().substring(8, 10));
                            if (startDay == scheduleDay || endDay == scheduleDay){
                                events.add(event);
                            }
                        }

                    adap = new Adaptador(context, events);

                    lvItems.setAdapter(adap);
                } else if (mDiffX > 20) {
                    try {
                        now.setTimeInMillis(sdf.parse(String.format("%04d-%02d-%02d", scheduleYear, scheduleMonth, scheduleDay)).getTime());
                    } catch (ParseException e) {
                        System.err.println(e.getMessage());
                    }
                    now.add(Calendar.DAY_OF_MONTH, -1);

                    mWeekView.goToDate(now);

                    scheduleYear = now.get(Calendar.YEAR);
                    scheduleMonth = now.get(Calendar.MONTH) + 1;
                    scheduleDay = now.get(Calendar.DAY_OF_MONTH);

                    txtDay.setText(String.format("%02d-%02d-%04d", scheduleDay, scheduleMonth, scheduleYear));

                    if (monthsDown.containsKey(String.format("%04d-%02d", scheduleYear, scheduleMonth)))
                        for (CustomWeekViewEvent event : monthsDown.get(String.format("%04d-%02d", scheduleYear, scheduleMonth))){
                            int startDay = Integer.parseInt(event.getStart().substring(8, 10)), endDay = Integer.parseInt(event.getEnd().substring(8, 10));
                            if (startDay == scheduleDay || endDay == scheduleDay){
                                events.add(event);
                            }
                        }

                    adap = new Adaptador(context, events);

                    lvItems.setAdapter(adap);
                }
        }

        return super.dispatchTouchEvent(ev);
    }

    @Override
    public List<WeekViewEvent> onMonthChange(int newYear, int newMonth) {
        String fecha = String.format("%04d-%02d", newYear, newMonth);

        if (monthsDown.containsKey(fecha)){
            ArrayList<WeekViewEvent> list = new ArrayList<>();
            for (CustomWeekViewEvent event : monthsDown.get(fecha)){
                if (filterItem == 0) {
                    if (planningVisible && !(event.getType() == CalendarItemType.Planning || event.getType() == CalendarItemType.RoundPlanning))
                        continue;
                    else if (presenceVisible && !(event.getType() == CalendarItemType.Presence || event.getType() == CalendarItemType.RoundPresence || event.getType() == CalendarItemType.OpenPresence))
                        continue;
                } else if (!(filterItem == event.getItemId()))
                    continue;

                int durHour = Integer.parseInt(event.getDuration().substring(0, 2)),
                    durMin = Integer.parseInt(event.getDuration().substring(3, 5));
                if (durHour == 0 && durMin == 0) continue;

                for (WeekViewEvent weekViewEvent : event.getEvents())
                    list.add(weekViewEvent);
            }
            return list;
        }

        monthsDown.put(fecha, new ArrayList<CustomWeekViewEvent>());

        ServerGet tarea = new ServerGet(this);
        tarea.delegate = this;
        tarea.showProgress = false;
        tarea.execute(getResources().getString(R.string.apiControlPresenceMobileTimetable) + "?since=" + fecha + "-01&until=" + fecha + String.format("-%02d", lastDayOfMonth(newYear, newMonth)));
        return new ArrayList<>();
    }

    @Override
    public void onEventClick(WeekViewEvent event, RectF eventRect) {
        int itemIdAux = 0; String location = null, eventName = "";
        SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss", Locale.getDefault());
        TextView txt; Button dialogButton;
        final Dialog dialog = new Dialog(Calendario.this);

        TextView tv = (TextView) dialog.findViewById(android.R.id.title);
        if (tv != null)
            tv.setSingleLine(false);

        dialog.setTitle(event.getName());

        dialog.setContentView(R.layout.calendar_dialog);

        txt = (TextView) dialog.findViewById(R.id.txtDate);
        if (event.getColor() == ContextCompat.getColor(context, R.color.ColorGreenRoundPlanning) || event.getColor() == ContextCompat.getColor(context, R.color.ColorAccentRoundPresence)) {
            Calendar cal = Calendar.getInstance();
            cal.setTimeInMillis(event.getStartTime().getTimeInMillis() + 10 * 60 * 1000);

            txt.setText("Paso a las : " + sdf.format(cal.getTime()));
        } else if (event.getColor() == ContextCompat.getColor(context, R.color.ColorPrimary) || event.getColor() == ContextCompat.getColor(context, R.color.ColorAccentPresence)) {
            txt.setText("Comienzo: " + sdf.format(event.getStartTime().getTime()) + "\nFinalización: " + sdf.format(event.getEndTime().getTime()));
        } else if (event.getColor() == ContextCompat.getColor(context, R.color.ColorAccentOpenPresence)) {
            txt.setText("En curso desde: " + sdf.format(event.getStartTime().getTime()));
        }

        txt = (TextView) dialog.findViewById(R.id.txtObservations);
        txt.setVisibility(View.GONE);

        txt = (TextView) dialog.findViewById(R.id.txtValues);
        txt.setVisibility(View.GONE);

        final int Id = (int) event.getId();

        {
            boolean find = false;
            for (ArrayList<CustomWeekViewEvent> list : monthsDown.values()) {
                for (CustomWeekViewEvent customEvent : list) {
                    if (customEvent.getId() == Id) {
                        itemIdAux = customEvent.getItemId();
                        location = customEvent.getLocation();
                        eventName = customEvent.getName();
                        find = true;
                        break;
                    }
                }
                if (find) break;
            }
        }

        final int ItemId = itemIdAux;
        final String Location = location;

        if (event.getName().equals("")) {
            dialog.setTitle(eventName);
        }

        Calendar yesterday = Calendar.getInstance(), tomorrow = (Calendar) yesterday.clone();
        yesterday.add(Calendar.DAY_OF_MONTH, -1);
        tomorrow.add(Calendar.DAY_OF_MONTH, 1);

        if (event.getColor() == ContextCompat.getColor(context, R.color.ColorPrimaryPlanning)) {
            if (event.getStartTime().after(yesterday) && event.getStartTime().before(tomorrow)) {
                dialogButton = (Button) dialog.findViewById(R.id.btnCheck);
                dialogButton.setVisibility(View.VISIBLE);
                dialogButton.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View v) {
                        String query = "?ItemId=" + ItemId + "&PlanningItemId=" + Id;

                        ServerGet tarea = new ServerGet(context);
                        tarea.delegate = delegate;
                        tarea.execute(getResources().getString(R.string.apiControlPlanningCheckMobileGet) + query);

                        dialog.dismiss();
                    }
                });
            }
        } else if (event.getColor() == ContextCompat.getColor(context, R.color.ColorGreenRoundPlanning)) {
            if (event.getStartTime().after(yesterday) && event.getStartTime().before(tomorrow)) {
                dialogButton = (Button) dialog.findViewById(R.id.btnCheck);
                dialogButton.setVisibility(View.VISIBLE);
                dialogButton.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View v) {
                        String query = "?ItemId=" + ItemId + "&PlanningCheckId=" + Id;

                        ServerGet tarea = new ServerGet(context);
                        tarea.delegate = delegate;
                        tarea.execute(getResources().getString(R.string.apiControlPlanningCheckMobileGet) + query);

                        dialog.dismiss();
                    }
                });
            }

            if (Location != null && !Location.equals("")) {
                dialogButton = (Button) dialog.findViewById(R.id.btnLocation);
                dialogButton.setVisibility(View.VISIBLE);
                dialogButton.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View v) {
                        Uri gmIntentUri = Uri.parse("geo:0,0?q=" + Location);
                        Intent mapIntent = new Intent(Intent.ACTION_VIEW, gmIntentUri);
                        mapIntent.setPackage("com.google.android.apps.maps");
                        if (mapIntent.resolveActivity(getPackageManager()) != null) {
                            startActivity(mapIntent);
                        }
                    }
                });
            }
        }

        dialog.show();
    }

    @Override
    public void onEmptyViewLongPress(Calendar calendar) {
        showDayInfo(calendar);
    }

    @Override
    public void onEventLongPress(WeekViewEvent weekViewEvent, RectF rectF) {
        showDayInfo(weekViewEvent.getStartTime());
    }

    private void showDayInfo(Calendar calendar){
        int planning = 0, presence = 0;
        SimpleDateFormat sdfYearMonth = new SimpleDateFormat("yyyy-MM", Locale.getDefault()),
                sdfDay = new SimpleDateFormat("dd", Locale.getDefault()),
                sdfDisplay = new SimpleDateFormat("EEE, d MMM yyyy", Locale.getDefault());
        String yearMonth = sdfYearMonth.format(calendar.getTime());
        int day = IntegerExpansion.getIntValueOf(sdfDay.format(calendar.getTime()));

        ArrayList<CustomWeekViewEvent> list = monthsDown.get(yearMonth);

        for (CustomWeekViewEvent event : list){
            int eventDay = IntegerExpansion.getIntValueOf(event.getStart().substring(8, 10));

            if (eventDay > day) break;
            else if (eventDay < day) continue;

            String[] tokens = event.getDuration().split(":");
            if (event.getType() == CalendarItemType.Planning && (filterItem == 0 || event.getItemId() == filterItem)) {
                planning += 3600 * IntegerExpansion.getIntValueOf(tokens[0]) + 60 * IntegerExpansion.getIntValueOf(tokens[1]) + IntegerExpansion.getIntValueOf(tokens[2]);
            } else if (event.getType() == CalendarItemType.Presence || event.getType() == CalendarItemType.OpenPresence && (filterItem == 0 || event.getItemId() == filterItem)) {
                presence += 3600 * IntegerExpansion.getIntValueOf(tokens[0]) + 60 * IntegerExpansion.getIntValueOf(tokens[1]) + IntegerExpansion.getIntValueOf(tokens[2]);
            }
        }

        final Dialog dialog = new Dialog(Calendario.this);
        TextView tv = (TextView) dialog.findViewById(android.R.id.title);
        dialog.setTitle("Resumen " + sdfDisplay.format(calendar.getTime()));
        if (tv != null)
            tv.setSingleLine(false);
        dialog.setContentView(R.layout.calendar_dialog);

        String text = "Trabajadas " + String.format("%02d:%02d:%02d", presence / 3600, (presence / 60) % 60, presence % 60) + " horas";
        if (planning != 0) text += " de " + String.format("%02d:%02d:%02d", planning / 3600, (planning / 60) % 60, planning % 60) + " horas";

        ((TextView) dialog.findViewById(R.id.txtDate)).setText(text);

        dialog.findViewById(R.id.txtObservations).setVisibility(View.GONE);
        dialog.findViewById(R.id.txtValues).setVisibility(View.GONE);

        dialog.show();
    }

    @Override
    public void onAsyncFinish(HashMap<String, String> map){

        if (error) return ;

        if (HandleServerError.Handle(map, this, this)) {
            error = true;
            return ;
        }

        if (map.get("route").contains(getResources().getString(R.string.apiControlPresenceMobileTimetable))) {
            ControlPresenceMobileGetTimetableResult res = CustomGson.getGson().fromJson(map.get("json"), ControlPresenceMobileGetTimetableResult.class);

            String[] since = (map.get("route")).split("[=&]")[1].split("-");
            String fecha = since[0] + "-" + since[1];
            ArrayList<CustomWeekViewEvent> list = new ArrayList<>();

            for (ControlPresenceMobileGetTimetableResult.Item item : res.Data) {
                try {
                    if (item.Type == CalendarItemType.Planning) {
                        añadirTurno(item, list);
                    } else if (item.Type == CalendarItemType.Presence) {
                        if (item.Start != null && item.Start.equals(item.End)) añadirRoundPresence(item, list);
                        else if (! (item.End == null)) añadirFichaje(item, list);
                        else añadirFichajeInc(item, list);
                    } else if (item.Type == CalendarItemType.RoundPlanning) {
                        añadirRoundPlan(item, list);
                    }
                } catch (NullPointerException e) {
                    e.printStackTrace();
                }
            }

            monthsDown.put(fecha, list);
            mWeekView.notifyDatasetChanged();

            Adaptador adap = new Adaptador(context, list);
            lvItems.setAdapter(adap);

            progreso.dismiss();
        } else if (map.get("route").contains(getResources().getString(R.string.apiControlItemMobileGetSelector))) {
            ControlItemMobileGetSelectorResult res = CustomGson.getGson().fromJson(map.get("json"), ControlItemMobileGetSelectorResult.class);
            String[] spinnerItems = new String[res.Data.size() + 1];

            spinnerItems[0] = "Mostrar todos los Items";

            for (int i = 0; i < res.Data.size(); i++){
                ControlItemMobileGetSelectorResult.Item item = res.Data.get(i);

                items.put(item.Id, item);
                spinnerItems[i+1] = item.Value;
            }

            ArrayAdapter<String> adapter = new ArrayAdapter<>(this, android.R.layout.simple_spinner_item, spinnerItems);

            spinner.setAdapter(adapter);
        } else if (map.get("route").contains(getResources().getString(R.string.apiControlPlanningCheckMobileGet))) {
            ControlItemMobileGetAllResult res = CustomGson.getGson().fromJson(map.get("json"), ControlItemMobileGetAllResult.class);

            Intent i = new Intent(Calendario.this, ManualCheck.class);
            i.putExtra("result", res.Data.get(0));
            i.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TASK);
            startActivity(i);
        }
    }

    private void añadirTurno(ControlPresenceMobileGetTimetableResult.Item item, ArrayList<CustomWeekViewEvent> list){
        String date;

        // Coger instancia actual del calendario
        Calendar startTime = Calendar.getInstance(),
                endTime = (Calendar) startTime.clone(),
                middleTime;

        SimpleDateFormat sdfUTC = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss", Locale.getDefault()), sdfLocal = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss", Locale.getDefault());
        sdfUTC.setTimeZone(TimeZone.getTimeZone("UTC"));

        CustomWeekViewEvent event = new CustomWeekViewEvent(item.Id, item.ItemId, item.Title, item.Start, item.End, item.Duration, item.Info, item.Type);

        try {
            startTime.setTimeInMillis(sdfUTC.parse(item.Start).getTime());
            endTime.setTimeInMillis(sdfUTC.parse(item.End).getTime());

            while (startTime.get(Calendar.DAY_OF_MONTH) != endTime.get(Calendar.DAY_OF_MONTH)){
                middleTime = Calendar.getInstance();

                date = startTime.get(Calendar.YEAR) + "-"
                        + ((startTime.get(Calendar.MONTH) < 9) ? "0" + (startTime.get(Calendar.MONTH) +1 ) : (startTime.get(Calendar.MONTH)) + 1) + "-"
                        + ((startTime.get(Calendar.DAY_OF_MONTH) < 10) ? "0" + startTime.get(Calendar.DAY_OF_MONTH) : startTime.get(Calendar.DAY_OF_MONTH))
                        + "T23:59:59";

                middleTime.setTimeInMillis(sdfLocal.parse(date).getTime());

                event.addEvent(item.Id, (Calendar) startTime.clone(), (Calendar) middleTime.clone());

                startTime.add(Calendar.DAY_OF_MONTH, 1);

                date = startTime.get(Calendar.YEAR) + "-"
                        + ((startTime.get(Calendar.MONTH) < 9) ? "0" + (startTime.get(Calendar.MONTH) +1 ) : (startTime.get(Calendar.MONTH)) + 1) + "-"
                        + ((startTime.get(Calendar.DAY_OF_MONTH) < 10) ? "0" + startTime.get(Calendar.DAY_OF_MONTH) : startTime.get(Calendar.DAY_OF_MONTH))
                        + "T00:00:00";

                startTime.setTimeInMillis(sdfLocal.parse(date).getTime());
            }

            event.addEvent(item.Id, startTime, endTime);

            for ( WeekViewEvent weekViewEvent : event.getEvents()) {
                // Asignarle un color al evento
                weekViewEvent.setColor(ContextCompat.getColor(context, R.color.ColorPrimaryPlanning));

                // Asignarle un nombre al evento
                if (endTime.getTimeInMillis() - startTime.getTimeInMillis() > 20 * 60 * 1000) {
                    weekViewEvent.setName(item.Title);
                } else {
                    weekViewEvent.setName("");
                }
            }

            list.add(event);
        } catch (ParseException e) {
            e.printStackTrace();
        }
    }

    private void añadirFichaje(ControlPresenceMobileGetTimetableResult.Item item, ArrayList<CustomWeekViewEvent> list){
        String date;

        // Coger instancia actual del calendario
        Calendar startTime = Calendar.getInstance(),
                endTime = (Calendar) startTime.clone(),
                middleTime;

        SimpleDateFormat sdfUTC = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss", Locale.getDefault()), sdfLocal = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss", Locale.getDefault());
        sdfUTC.setTimeZone(TimeZone.getTimeZone("UTC"));

        // Crear el evento (id, titulo, inicio, fin)
        CustomWeekViewEvent event = new CustomWeekViewEvent(item.Id, item.ItemId, item.Title, item.Start, item.End, item.Duration, item.Info, item.Type);

        try {
            startTime.setTimeInMillis(sdfUTC.parse(item.Start).getTime());
            endTime.setTimeInMillis(sdfUTC.parse(item.End).getTime());

            while (startTime.get(Calendar.DAY_OF_MONTH) != endTime.get(Calendar.DAY_OF_MONTH)){
                middleTime = Calendar.getInstance();

                date = startTime.get(Calendar.YEAR) + "-"
                        + ((startTime.get(Calendar.MONTH) < 9) ? "0" + (startTime.get(Calendar.MONTH) +1 ) : (startTime.get(Calendar.MONTH)) + 1) + "-"
                        + ((startTime.get(Calendar.DAY_OF_MONTH) < 10) ? "0" + startTime.get(Calendar.DAY_OF_MONTH) : startTime.get(Calendar.DAY_OF_MONTH))
                        + "T23:59:59";

                middleTime.setTimeInMillis(sdfLocal.parse(date).getTime());

                event.addEvent(item.Id, (Calendar) startTime.clone(), (Calendar) middleTime.clone());

                startTime.add(Calendar.DAY_OF_MONTH, 1);

                date = startTime.get(Calendar.YEAR) + "-"
                        + ((startTime.get(Calendar.MONTH) < 9) ? "0" + (startTime.get(Calendar.MONTH) +1 ) : (startTime.get(Calendar.MONTH)) + 1) + "-"
                        + ((startTime.get(Calendar.DAY_OF_MONTH) < 10) ? "0" + startTime.get(Calendar.DAY_OF_MONTH) : startTime.get(Calendar.DAY_OF_MONTH))
                        + "T00:00:00";

                startTime.setTimeInMillis(sdfLocal.parse(date).getTime());
            }

            event.addEvent(item.Id, startTime, endTime);

            for ( WeekViewEvent weekViewEvent : event.getEvents()) {
                // Asignarle un color al evento
                weekViewEvent.setColor(ContextCompat.getColor(context, R.color.ColorAccentPresence));

                if (endTime.getTimeInMillis() - startTime.getTimeInMillis() > 20 * 60 * 1000) {
                    weekViewEvent.setName(item.Title);
                } else {
                    weekViewEvent.setName("");
                }
            }

            list.add(event);
        } catch (ParseException e) {
            e.printStackTrace();
        }
    }

    private void añadirFichajeInc(ControlPresenceMobileGetTimetableResult.Item item, ArrayList<CustomWeekViewEvent> list){
        String date;

        // Coger instancia actual del calendario
        Calendar yesterday = Calendar.getInstance(); yesterday.add(Calendar.DAY_OF_MONTH, -1);
        Calendar startTime = Calendar.getInstance(), endTime = Calendar.getInstance(), middleTime;

        SimpleDateFormat sdfUTC = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss'Z'", Locale.getDefault()), sdfLocal = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss", Locale.getDefault());
        sdfUTC.setTimeZone(TimeZone.getTimeZone("UTC"));

        // Crear el evento (id, titulo, inicio, fin)
        CustomWeekViewEvent event = new CustomWeekViewEvent(item.Id, item.ItemId, item.Title, item.Start, sdfUTC.format(endTime.getTime()), item.Duration, item.Info, CalendarItemType.OpenPresence);

        try {
            startTime.setTimeInMillis(sdfUTC.parse(item.Start).getTime());

            if (startTime.before(yesterday)){
                return ;
            }

            long duration = (long) ((endTime.getTimeInMillis() - startTime.getTimeInMillis()) / 1E3);
            event.setDuration(String.format("%02d:%02d:%02d", duration / 3600, (duration / 60) % 60, duration % 60));

            while (startTime.get(Calendar.DAY_OF_MONTH) != endTime.get(Calendar.DAY_OF_MONTH)){
                middleTime = Calendar.getInstance();

                date = String.format("%04d-%02d-%02dT23:59:59", startTime.get(Calendar.YEAR), startTime.get(Calendar.MONTH) + 1, startTime.get(Calendar.DAY_OF_MONTH));

                middleTime.setTimeInMillis(sdfLocal.parse(date).getTime());

                event.addEvent(item.Id, (Calendar) startTime.clone(), (Calendar) middleTime.clone());

                startTime.add(Calendar.DAY_OF_MONTH, 1);

                date = String.format("%04d-%02d-%02dT23:59:59", startTime.get(Calendar.YEAR), startTime.get(Calendar.MONTH) + 1, startTime.get(Calendar.DAY_OF_MONTH));

                startTime.setTimeInMillis(sdfLocal.parse(date).getTime());
            }

            event.addEvent(item.Id, startTime, endTime);

            for ( WeekViewEvent weekViewEvent : event.getEvents()) {
                weekViewEvent.setColor(ContextCompat.getColor(context, R.color.ColorAccentOpenPresence));

                if (endTime.getTimeInMillis() - startTime.getTimeInMillis() > 20 * 60 * 1000) {
                    weekViewEvent.setName(item.Title);
                } else {
                    weekViewEvent.setName("");
                }
            }

            list.add(event);
        } catch (ParseException e) {
            e.printStackTrace();
        }
    }

    private void añadirRoundPlan(ControlPresenceMobileGetTimetableResult.Item item, ArrayList<CustomWeekViewEvent> list){
        // Coger instancia actual del calendario
        Calendar startTime = Calendar.getInstance(),
                endTime = (Calendar) startTime.clone();

        SimpleDateFormat sdfUTC = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss", Locale.getDefault());
        sdfUTC.setTimeZone(TimeZone.getTimeZone("UTC"));

        CustomWeekViewEvent event = new CustomWeekViewEvent(item.Id, item.ItemId, item.Title, item.Start, item.Start, "00:20:00", item.Info, item.Type);

        event.setLocation(item.Location);

        try {
            startTime.setTimeInMillis(sdfUTC.parse(item.Start).getTime());

            endTime.setTimeInMillis(startTime.getTimeInMillis() + 10 * 60 * 1000);

            startTime.setTimeInMillis(startTime.getTimeInMillis() - 10 * 60 * 1000);

            event.addEvent(item.Id, startTime, endTime);

            for ( WeekViewEvent weekViewEvent : event.getEvents()) {
                // Asignarle un color al evento
                weekViewEvent.setColor(ContextCompat.getColor(context, R.color.ColorGreenRoundPlanning));

                // Asignarle un nombre al evento
                weekViewEvent.setName(item.Title);
            }

            list.add(event);
        } catch (ParseException e) {
            e.printStackTrace();
        }
    }

    private void añadirRoundPresence(ControlPresenceMobileGetTimetableResult.Item item, ArrayList<CustomWeekViewEvent> list){
        // Coger instancia actual del calendario
        Calendar startTime = Calendar.getInstance(),
                endTime = (Calendar) startTime.clone();

        SimpleDateFormat sdfUTC = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss", Locale.getDefault());
        sdfUTC.setTimeZone(TimeZone.getTimeZone("UTC"));

        CustomWeekViewEvent event = new CustomWeekViewEvent(item.Id, item.ItemId, item.Title, item.Start, item.Start, "00:20:00", item.Info, CalendarItemType.RoundPresence);

        try {
            startTime.setTimeInMillis(sdfUTC.parse(item.Start).getTime());

            endTime.setTimeInMillis(startTime.getTimeInMillis() + 10 * 60 * 1000);

            startTime.setTimeInMillis(startTime.getTimeInMillis() - 10 * 60 * 1000);

            event.addEvent(item.Id, startTime, endTime);

            for ( WeekViewEvent weekViewEvent : event.getEvents()) {
                // Asignarle un color al evento
                weekViewEvent.setColor(ContextCompat.getColor(context, R.color.ColorAccentRoundPresence));

                // Asignarle un nombre al evento
                weekViewEvent.setName(item.Title);
            }

            list.add(event);
        } catch (ParseException e) {
            e.printStackTrace();
        }
    }

    public static class Adaptador extends BaseAdapter {
        private static LayoutInflater inflador = null;
        private final ArrayList<CustomWeekViewEvent> lista;

        public Adaptador(Context contexto, ArrayList<CustomWeekViewEvent> list){
            if (inflador == null){
                inflador = (LayoutInflater) contexto.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
            }
            this.lista = list;
        }

        public View getView(int posicion, View vistaReciclada, ViewGroup padre) {
            if (vistaReciclada == null) {
                vistaReciclada = inflador.inflate(R.layout.calendario_list_items, padre, false);
            }

            String startInfo = "", endInfo = "";

            // Coger instancia actual del calendario
            Calendar startTime = Calendar.getInstance(), endTime = (Calendar) startTime.clone();

            SimpleDateFormat sdfUTC = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss'Z'", Locale.getDefault()), sdfLocal = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss", Locale.getDefault());
            sdfUTC.setTimeZone(TimeZone.getTimeZone("UTC"));

            CustomWeekViewEvent event = lista.get(posicion);

            try {
                startTime.setTimeInMillis(sdfUTC.parse(event.getStart()).getTime());
                endTime.setTimeInMillis(sdfUTC.parse(event.getEnd()).getTime());
            } catch (ParseException e) {
                e.printStackTrace();
            }

            // Crear el evento (id, titulo, inicio, fin)
            TextView txtId = (TextView) vistaReciclada.findViewById(R.id.txtId);
            TextView txtType = (TextView) vistaReciclada.findViewById(R.id.txtType);
            TextView txtTitle = (TextView) vistaReciclada.findViewById(R.id.txtTitle);
            TextView txtStart = (TextView) vistaReciclada.findViewById(R.id.txtStart);
            TextView txtEnd = (TextView) vistaReciclada.findViewById(R.id.txtEnd);
            TextView txtInfo = (TextView) vistaReciclada.findViewById(R.id.txtInfo);

            txtId.setText(String.valueOf(event.getId()));
            txtType.setText(String.valueOf(event.getType()));

            txtTitle.setText(event.getName());
            if (event.getType() == CalendarItemType.Planning){
                txtTitle.setTextColor(ContextCompat.getColor(context, R.color.ColorPrimaryPlanning));
                startInfo = "Comienzo: ";
                endInfo = "Finalización: ";
                txtEnd.setVisibility(View.VISIBLE);
            } else if (event.getType() == CalendarItemType.Presence) {
                txtTitle.setTextColor(ContextCompat.getColor(context, R.color.ColorAccentPresence));
                startInfo = "Comienzo: ";
                endInfo = "Finalización: ";
                txtEnd.setVisibility(View.VISIBLE);
            } else if (event.getType() == CalendarItemType.RoundPlanning) {
                txtTitle.setTextColor(ContextCompat.getColor(context, R.color.ColorGreenRoundPlanning));
                startInfo = "Paso a: ";
                txtEnd.setVisibility(View.GONE);
            } else if (event.getType() == CalendarItemType.RoundPresence) {
                txtTitle.setTextColor(ContextCompat.getColor(context, R.color.ColorAccentRoundPresence));
                startInfo = "Paso a: ";
                txtEnd.setVisibility(View.GONE);
            } else if (event.getType() == CalendarItemType.OpenPresence) {
                txtTitle.setTextColor(ContextCompat.getColor(context, R.color.ColorAccentOpenPresence));
                startInfo = "En curso desde: ";
                txtEnd.setVisibility(View.GONE);
            }

            try {
                txtStart.setText(startInfo + sdfLocal.format(startTime.getTime()));
                txtEnd.setText(endInfo + sdfLocal.format(endTime.getTime()));
                txtInfo.setText(event.getObservations());
            } catch (NullPointerException e) {
                e.printStackTrace();
            }

            return vistaReciclada;
        }

        public int getCount() {
            if (lista != null)
                return lista.size();
            else
                return 0;
        }

        public Object getItem(int posicion) {
            return lista.get(posicion);
        }

        public long getItemId(int posicion) {
            return posicion;
        }
    }

    private int lastDayOfMonth(int year, int month){
        switch (month)
        {
            case 1: // fall through
            case 3: // fall through
            case 5: // fall through
            case 7: // fall through
            case 8: // fall through
            case 10: // fall through
            case 12:
                return  31;
            case 4: // fall through
            case 6: // fall through
            case 9: // fall through
            case 11:
                return  30;
            case 2:
                if (0 == year % 4 && 0 != year % 100 || 0 == year % 400)
                    return 29;
                else
                    return 28;
        }

        return -1;
    }
}
