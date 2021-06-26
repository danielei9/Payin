package com.mobile.application.payin.dto.arguments;

import java.util.ArrayList;

public class ControlPresenceMobileTrackArguments {
    public ArrayList<Track> Tracks;
    public ArrayList<TrackItem> TrackItems;

    public ControlPresenceMobileTrackArguments() {
        Tracks = new ArrayList<>();
        TrackItems = new ArrayList<>();
    }

    public static class Track
    {
        public int Id;

        public Track() {
        }
    }

    public static class TrackItem
    {
        public String Date;
        public double Latitude;
        public double Longitude;
        public int    Quality;
        public double Speed;

        public TrackItem() {
        }
    }
}
