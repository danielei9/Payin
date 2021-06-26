package com.mobile.application.payin.domain;

public class ControlTrack {

    // Declaramos las variables
    private int id;
    private String latitud, longitud, fecha, calidadGPS, velocidadGPS;

    public ControlTrack() {}

    public ControlTrack(int id, String latitud, String longitud, String fecha, String calidadGPS, String velocidadGPS) {
        this.id = id;
        this.latitud = latitud;
        this.longitud = longitud;
        this.fecha = fecha;
        this.calidadGPS = calidadGPS;
        this.velocidadGPS = velocidadGPS;
    }

    public ControlTrack(String latitud, String longitud, String fecha, String calidadGPS, String velocidadGPS) {
        this.latitud = latitud;
        this.longitud = longitud;
        this.fecha = fecha;
        this.calidadGPS = calidadGPS;
        this.velocidadGPS = velocidadGPS;
    }

    public int getID(){
        return this.id;
    }

    public void setID(int id){
        this.id = id;
    }

    public String getLatitud(){
        return this.latitud;
    }

    public void setLatitud(String latitud){
        this.latitud = latitud;
    }

    public String getLongitud(){
        return this.longitud;
    }

    public void setLongitud(String longitud){
        this.longitud = longitud;
    }

    public String getFecha(){
        return this.fecha;
    }

    public void setFecha(String fecha){ this.fecha = fecha; }

    public String getCalidadGPS(){
        return this.calidadGPS;
    }

    public void setCalidadGPS(String calidadGPS){
        this.calidadGPS = calidadGPS;
    }

    public String getVelocidadGPS(){
        return this.velocidadGPS;
    }

    public void setVelocidadGPS(String velocidadGPS){
        this.velocidadGPS = velocidadGPS;
    }

}
