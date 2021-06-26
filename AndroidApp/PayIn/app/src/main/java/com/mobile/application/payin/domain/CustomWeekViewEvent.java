package com.mobile.application.payin.domain;

import com.alamkanak.weekview.WeekViewEvent;

import java.io.Serializable;
import java.util.ArrayList;
import java.util.Calendar;

public class CustomWeekViewEvent implements Serializable{
    private int Id;
    private int ItemId;
    private String Name;
    private String Start;
    private String End;
    private String Duration;
    private String Observations;
    private String Location = null;
    private int Type;

    private ArrayList<WeekViewEvent> events;

    public CustomWeekViewEvent(int id, int itemId, String name, String start, String end, String duration, String observations, int type){
        Id = id;
        ItemId = itemId;
        Name = name;
        Start = start;
        End = end;
        Duration = duration;
        Observations = observations;

        Type = type;

        events = new ArrayList<>();
    }

    public CustomWeekViewEvent(int id, int itemId, String name, String start, String end, String duration, String observations, int type, Calendar startTime, Calendar endTime){
        Id = id;
        ItemId = itemId;
        Name = name;
        Start = start;
        End = end;
        Duration = duration;
        Observations = observations;

        Type = type;

        events = new ArrayList<>();
        events.add(new WeekViewEvent(id, name, startTime, endTime));
    }

    public int getId() {
        return Id;
    }

    public void setId(int id) {
        Id = id;
    }

    public int getItemId() {
        return ItemId;
    }

    public void setItemId(int itemId) {
        ItemId = itemId;
    }

    public String getDuration() {
        return Duration == null ? "" : Duration;
    }

    public void setDuration(String duration) {
        Duration = duration;
    }

    public String getEnd() {
        return End == null ? "" : End;
    }

    public void setEnd(String end) {
        End = end;
    }

    public String getName() {
        return Name == null ? "" : Name;
    }

    public void setName(String name) {
        Name = name;
    }

    public String getObservations() {
        return Observations == null ? "" : Observations;
    }

    public void setObservations(String observations) {
        Observations = observations;
    }

    public String getLocation() {
        return Location;
    }

    public void setLocation(String location) {
        Location = location;
    }

    public String getStart() {
        return Start == null ? "" : Start;
    }

    public void setStart(String start) {
        Start = start;
    }

    public int getType() {
        return Type;
    }

    public void setType(int type) {
        Type = type;
    }

    public ArrayList<WeekViewEvent> getEvents(){
        return events;
    }

    public void setEvents(ArrayList<WeekViewEvent> events){ this.events = events; }

    public void addEvent(long eventId, Calendar startTime, Calendar endTime) {
        events.add(new WeekViewEvent(eventId, "", startTime, endTime));
    }
}
