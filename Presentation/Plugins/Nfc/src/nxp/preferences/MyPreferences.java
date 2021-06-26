package com.nxp.ltsm.mymifareapp.preferences;

import android.content.Context;
import android.content.SharedPreferences;

public class MyPreferences {
	// Preferences name
	private final static String PREF_FILE_NAME = "MyMifareAppPref";
	private final static String VCENTRYINDEX = "vcentryindex";

	private static SharedPreferences getPreferences(Context ctx) {
		SharedPreferences preferences = ctx.getSharedPreferences(PREF_FILE_NAME, 0);
		return preferences;
	}
	
	////////////////////////////////////////////////////////////////////////
	/////// GET
	////////////////////////////////////////////////////////////////////////
	
	public static int getEntryIndex(final Context ctx) {
		SharedPreferences prefer = getPreferences(ctx);
		return prefer.getInt(VCENTRYINDEX, 1);
	}
	
	////////////////////////////////////////////////////////////////////////
	/////// SET
	////////////////////////////////////////////////////////////////////////
	
	public static void setEntryIndex(final Context ctx, final int index) {
		SharedPreferences.Editor editor = getPreferences(ctx).edit();
		editor.putInt(VCENTRYINDEX, index);
		editor.commit();
	}
}
