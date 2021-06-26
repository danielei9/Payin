package com.mobile.application.payin.common.utilities;

import android.content.Context;
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.ColorFilter;
import android.graphics.Paint;
import android.graphics.PixelFormat;
import android.graphics.Rect;
import android.graphics.Typeface;
import android.graphics.drawable.Drawable;
import android.graphics.drawable.LayerDrawable;

import com.android.application.payin.R;

public class IconsWithBadgeCount {

    public static void setDayBadgeCount(Context context, LayerDrawable icon, String count) {
        DayDrawable badge;

        Drawable reuse = icon.findDrawableByLayerId(R.id.ic_badge);
        if (reuse != null && reuse instanceof DayDrawable)
            badge = (DayDrawable) reuse;
        else
            badge = new DayDrawable(context);


        badge.setCount(count);
        icon.mutate();
        icon.setDrawableByLayerId(R.id.ic_badge, badge);
    }

    public static void setNotificationBadgeCount(Context context, LayerDrawable icon, String count) {
        NotificationDrawable badge;

        Drawable reuse = icon.findDrawableByLayerId(R.id.ic_badge);
        if (reuse != null && reuse instanceof NotificationDrawable)
            badge = (NotificationDrawable) reuse;
        else
            badge = new NotificationDrawable(context);


        badge.setCount(count);
        icon.mutate();
        icon.setDrawableByLayerId(R.id.ic_badge, badge);
    }

    public static class DayDrawable extends Drawable {
        private Paint mTextPaint;
        private Rect mTxtRect = new Rect();

        private String mCount = "";
        private boolean mWillDraw = false;

        public DayDrawable(Context context) {
            mTextPaint = new Paint();
            mTextPaint.setColor(Color.WHITE);
            mTextPaint.setTypeface(Typeface.DEFAULT_BOLD);
            mTextPaint.setTextSize(context.getResources().getDimension(R.dimen.day_text_size));
            mTextPaint.setAntiAlias(true);
            mTextPaint.setTextAlign(Paint.Align.CENTER);
        }

        @Override
        public void draw(Canvas canvas) {
            if (!mWillDraw) {
                return;
            }

            Rect bounds = getBounds();
            float width = bounds.right - bounds.left;
            float height = bounds.bottom - bounds.top + 2;

            float centerX = width / 2;
            float centerY = height / 2;

            mTextPaint.getTextBounds(mCount, 0, mCount.length(), mTxtRect);
            float textHeight = mTxtRect.bottom - mTxtRect.top;
            float textY = centerY + (textHeight / 2f);
            canvas.drawText(mCount, centerX, textY, mTextPaint);
        }

        public void setCount(String count) {
            mCount = count;

            mWillDraw = true;
            invalidateSelf();
        }

        @Override
        public void setAlpha(int alpha) {

        }

        @Override
        public void setColorFilter(ColorFilter cf) {

        }

        @Override
        public int getOpacity() {
            return PixelFormat.UNKNOWN;
        }
    }

    public static class NotificationDrawable extends Drawable {
        private Paint mTextPaint;
        private Rect mTxtRect = new Rect();

        private String mCount = "";
        private boolean mWillDraw = false;

        public NotificationDrawable(Context context) {
            mTextPaint = new Paint();
            mTextPaint.setColor(Color.WHITE);
            mTextPaint.setTypeface(Typeface.DEFAULT_BOLD);
            mTextPaint.setTextSize(context.getResources().getDimension(R.dimen.day_text_size));
            mTextPaint.setAntiAlias(true);
            mTextPaint.setTextAlign(Paint.Align.CENTER);
    }

        @Override
        public void draw(Canvas canvas) {
            if (!mWillDraw) {
                return;
            }

            Rect bounds = getBounds();
            float width = bounds.right - bounds.left;
            float height = bounds.bottom - bounds.top;

            float centerX = width * 0.90f;
            float centerY = height * 0.10f;

            mTextPaint.getTextBounds("00", 0, 2, mTxtRect);
            float textHeight = mTxtRect.bottom - mTxtRect.top;
            float textY = centerY + (textHeight / 2f);

            Paint circlePaint = new Paint();
            circlePaint.setColor(Color.parseColor("#3498DB"));
            circlePaint.setAntiAlias(true);
            circlePaint.setFilterBitmap(true);

            int x = Math.abs(mTxtRect.right)+ Math.abs(mTxtRect.left),
                    y = Math.abs(mTxtRect.bottom) + Math.abs(mTxtRect.top);

            if (y < x)
                canvas.drawCircle(centerX, centerY, x * 0.75f, circlePaint);
            else
                canvas.drawCircle(centerX, centerY, y * 0.75f, circlePaint);

            canvas.drawText(mCount, centerX, textY, mTextPaint);
        }

        public void setCount(String count) {
            mCount = count;

            mWillDraw = true;
            invalidateSelf();
        }

        @Override
        public void setAlpha(int alpha) {

        }

        @Override
        public void setColorFilter(ColorFilter cf) {

        }

        @Override
        public int getOpacity() {
            return PixelFormat.UNKNOWN;
        }
    }
}