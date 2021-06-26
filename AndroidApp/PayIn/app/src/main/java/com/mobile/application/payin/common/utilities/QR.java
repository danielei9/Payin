package com.mobile.application.payin.common.utilities;

import android.content.Context;
import android.graphics.Bitmap;
import android.graphics.Color;
import android.os.AsyncTask;
import android.util.DisplayMetrics;
import android.widget.ImageView;

import com.google.zxing.BarcodeFormat;
import com.google.zxing.WriterException;
import com.google.zxing.common.BitMatrix;
import com.google.zxing.qrcode.QRCodeWriter;

public class QR {
    public static void generate(Context context, ImageView iv, String domain, String json) {
        AsyncQR qr = new AsyncQR(context.getResources().getDisplayMetrics(), "pay[in]/" + domain + ":" + json, iv);
        qr.execute();
    }

    public static class AsyncQR extends AsyncTask<Void, Void, Bitmap> {
        private DisplayMetrics displayMetrics;
        private String content;
        private ImageView iv;

        public AsyncQR(DisplayMetrics displayMetrics, String content, ImageView iv) {
            this.displayMetrics = displayMetrics;
            this.content = content;
            this.iv = iv;
        }

        @Override
        protected void onPreExecute() {
            super.onPreExecute();
        }

        @Override
        protected Bitmap doInBackground(Void... params) {
            Bitmap bmp;
            QRCodeWriter writer = new QRCodeWriter();
            try {
                BitMatrix bitMatrix = writer.encode(content, BarcodeFormat.QR_CODE, displayMetrics.widthPixels - 32, displayMetrics.widthPixels - 32);
                int width = bitMatrix.getWidth();
                int height = bitMatrix.getHeight();
                bmp = Bitmap.createBitmap(width, height, Bitmap.Config.RGB_565);
                for (int x = 0; x < width; x++) {
                    for (int y = 0; y < height; y++) {
                        bmp.setPixel(x, y, bitMatrix.get(x, y) ? Color.BLACK : Color.WHITE);
                    }
                }
            } catch (WriterException e) {
                e.printStackTrace();
                return null;
            }

            return bmp;
        }

        @Override
        protected void onPostExecute(Bitmap bitmap) {
            if (bitmap != null)
                iv.setImageBitmap(bitmap);

            super.onPostExecute(bitmap);
        }
    }
}
