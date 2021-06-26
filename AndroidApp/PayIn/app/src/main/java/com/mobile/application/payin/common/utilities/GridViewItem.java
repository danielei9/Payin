package com.mobile.application.payin.common.utilities;

import android.graphics.drawable.Drawable;

public class GridViewItem {
    public final String title;
    public final Drawable icon;
    public final int type;

    public GridViewItem(Drawable icon, String title, int type) {
        this.icon  = icon;
        this.title = title;
        this.type  = type;
    }
}
