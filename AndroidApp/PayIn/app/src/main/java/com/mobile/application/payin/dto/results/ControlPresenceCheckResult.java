package com.mobile.application.payin.dto.results;

import java.io.Serializable;
import java.util.ArrayList;

public class ControlPresenceCheckResult implements Serializable {
    public ArrayList<Track> Entrances;
    public ArrayList<Track> Exits;
    public String           TrackFrecuency;

    public ControlPresenceCheckResult() {
        Entrances = new ArrayList<>();
        Exits = new ArrayList<>();
    }

    public static class Track implements Serializable {
        public int Id;

        public Track() {}
    }
}
