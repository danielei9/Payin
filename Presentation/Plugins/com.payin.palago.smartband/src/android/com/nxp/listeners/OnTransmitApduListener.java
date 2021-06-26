package com.nxp.listeners;

public interface OnTransmitApduListener {
    public abstract void sendApduToSE(byte[] apdu, int timeout);
    public abstract void setOnOperationListener(OnOperationListener listener);
}
