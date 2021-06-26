package com.nxp.ltsm.ltsmclient;

import android.content.ComponentName;
import android.content.Context;
import android.content.Intent;
import android.content.ServiceConnection;
import android.os.IBinder;

public class ServiceConnector implements ServiceConnection {
	private static final String APPSOLUT_INTENT_ACTION_BIND_MESSAGE_SERVICE = "com.nxp.ltsm.ltsmclient";

	private ILTSMClient service;
	private final Context context;

	public ServiceConnector(Context context) {
		this.context = context;
		connect();
	}

	@Override
	public void onServiceConnected(ComponentName name, IBinder binder) {
		this.service = ILTSMClient.Stub.asInterface(binder);

	}

	@Override
	public void onServiceDisconnected(ComponentName name) {
		service = null;
	}

	public boolean isConnected() {
		if (service != null) {
			return true;
		} else {
			return false;
		}
	}

	public ILTSMClient getInterface() {
		return service;
	}

	public void disConnect() {
		if (service != null) {
			context.unbindService(this);
		}

	}

	public void connect() {
		if (service == null) {
			Intent intent = new Intent(APPSOLUT_INTENT_ACTION_BIND_MESSAGE_SERVICE);
			intent.setPackage(APPSOLUT_INTENT_ACTION_BIND_MESSAGE_SERVICE);
			context.bindService(intent, this, Context.BIND_AUTO_CREATE);
			//context.start(intent);
		}
	}
}
