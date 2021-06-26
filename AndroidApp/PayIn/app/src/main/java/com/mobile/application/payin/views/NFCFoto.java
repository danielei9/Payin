package com.mobile.application.payin.views;

import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.SharedPreferences;
import android.content.pm.PackageManager;
import android.graphics.Bitmap;
import android.graphics.Bitmap.CompressFormat;
import android.graphics.BitmapFactory;
import android.graphics.Color;
import android.graphics.Matrix;
import android.hardware.Camera;
import android.hardware.Camera.CameraInfo;
import android.hardware.Camera.ErrorCallback;
import android.hardware.Camera.Parameters;
import android.hardware.Camera.PictureCallback;
import android.os.Bundle;
import android.support.v4.content.res.ResourcesCompat;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.util.Base64;
import android.util.Log;
import android.view.Surface;
import android.view.SurfaceHolder;
import android.view.SurfaceHolder.Callback;
import android.view.SurfaceView;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.WindowManager;
import android.widget.ImageButton;

import com.android.application.payin.R;
import com.mobile.application.payin.common.utilities.SecurityDialog;
import com.mobile.application.payin.dto.arguments.ControlPresenceCheckArguments;
import com.mobile.application.payin.dto.results.ControlPresenceCheckResult;
import com.mobile.application.payin.common.serverconnections.ServerPost;
import com.mobile.application.payin.common.interfaces.AsyncResponse;
import com.mobile.application.payin.common.utilities.CustomGson;
import com.mobile.application.payin.common.utilities.HandleCheck;
import com.mobile.application.payin.common.utilities.HandleServerError;

import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.util.HashMap;

public class NFCFoto extends AppCompatActivity implements Callback, OnClickListener, AsyncResponse {
    private SurfaceHolder surfaceHolder;
    private Camera camera;
    private int cameraId;

    private ControlPresenceCheckArguments PresenceArguments;

    private boolean takingFoto = false, saveTrack = false;
    private String route;

    private Context context;
    private AsyncResponse delegate;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.foto);

        context = this;
        delegate = this;

        Toolbar toolbar = (Toolbar) findViewById(R.id.tool_bar);
        setSupportActionBar(toolbar);
        toolbar.setNavigationIcon(ResourcesCompat.getDrawable(getResources(), R.drawable.ic_action_back, null));
        toolbar.setNavigationOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                finish();
            }
        });

        // Comprobamos las preferencias del modo debug
        SharedPreferences pref = getSharedPreferences(getResources().getString(R.string.prefs), MODE_PRIVATE);

        Boolean debug = pref.getBoolean("debug", false);

        // Con el modo debug activado ponemos el color de fondo en rojo
        if (debug){
            toolbar.setBackgroundColor(Color.parseColor("#ff1a1a"));
        } else {
            toolbar.setBackgroundColor(Color.parseColor("#e9af30"));
        }

        PresenceArguments = (ControlPresenceCheckArguments) getIntent().getSerializableExtra("arguments");
        route = getIntent().getStringExtra("route");
        saveTrack = getIntent().getBooleanExtra("saveTrack", false);

        // Asociamos las vistas
        ImageButton captureImage = (ImageButton) findViewById(R.id.captureImage);
        SurfaceView surfaceView = (SurfaceView) findViewById(R.id.surfaceView);

        String permission = "android.permission.CAMERA";
        if (checkCallingOrSelfPermission(permission) == PackageManager.PERMISSION_GRANTED) {
            surfaceHolder = surfaceView.getHolder();
            surfaceHolder.addCallback(this);
            captureImage.setOnClickListener(this);
            getWindow().addFlags(WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON);
        } else {
            SecurityDialog.createDialogPrincipal(this);
        }
    }

    @Override
    public void onBackPressed() {
        finish();
    }

    @Override
    public void surfaceCreated(SurfaceHolder holder) {
        openCamera(CameraInfo.CAMERA_FACING_FRONT);
    }

    @Override
    public void surfaceChanged(SurfaceHolder holder, int format, int width, int height) {}

    @Override
    public void surfaceDestroyed(SurfaceHolder holder) {releaseCamera();}

    private void alertCameraDialog() {
        AlertDialog.Builder builder = new AlertDialog.Builder(this);
        builder.setTitle("Error")
                .setMessage("Fallo al abrir la cámara frontal")
                .setCancelable(false)
                .setPositiveButton("OK", new DialogInterface.OnClickListener() {
                    public void onClick(DialogInterface dialog, int id) {
                        dialog.dismiss();
                    }
                });
        AlertDialog alert = builder.create();
        alert.show();
    }

    private void openCamera(int id) {
        cameraId = id;
        releaseCamera();

        try {
            camera = Camera.open(cameraId);
        } catch (Exception e) {
            e.printStackTrace();
        }

        if (camera != null) {
            try {
                setUpCamera(camera);

                camera.setErrorCallback(new ErrorCallback() {
                    @Override
                    public void onError(int error, Camera camera) {
                        // Aquí mostrariamos un error
                    }
                });

                camera.setPreviewDisplay(surfaceHolder);
                camera.startPreview();
            } catch (IOException e) {
                e.printStackTrace();
                releaseCamera();
            }
        } else {
            releaseCamera();
            openCamera(CameraInfo.CAMERA_FACING_BACK);
        }
    }

    private void releaseCamera() {
        try {
            if (camera != null) {
                camera.setPreviewCallback(null);
                camera.setErrorCallback(null);
                camera.stopPreview();
                camera.release();
                camera = null;
            }
        } catch (Exception e) {
            e.printStackTrace();
            Log.e("error", e.toString());
            camera = null;
        }
    }

    private void setUpCamera(Camera c) {
        Camera.CameraInfo info = new Camera.CameraInfo();
        Camera.getCameraInfo(cameraId, info);
        int rotation = getWindowManager().getDefaultDisplay().getRotation();
        int degree = 0;
        switch (rotation) {
            case Surface.ROTATION_0:
                degree = 0;
                break;
            case Surface.ROTATION_90:
                degree = 90;
                break;
            case Surface.ROTATION_180:
                degree = 180;
                break;
            case Surface.ROTATION_270:
                degree = 270;
                break;

            default:
                break;
        }

        if (info.facing == Camera.CameraInfo.CAMERA_FACING_FRONT) {
            // Para la camara frontal
            rotation = (info.orientation + degree) % 330;
            rotation = (360 - rotation) % 360;
        }

        if (info.facing == CameraInfo.CAMERA_FACING_BACK) {
            // Para la camara frontal
            rotation = (info.orientation + 180) % 330;
            rotation = (360 - rotation) % 360;
        }

        c.setDisplayOrientation(rotation);
        Parameters params = c.getParameters();

        params.setRotation(rotation);
    }

    @Override
    public void onClick(View v) {
        switch (v.getId()) {
            case R.id.captureImage:
                if (!takingFoto) {
                    takingFoto = true;
                    takeImage();
                }
                break;
            default:
                break;
        }
    }

    private void takeImage() {
        camera.takePicture(null, null, new PictureCallback() {
            @Override
            public void onPictureTaken(byte[] data, Camera camera) {
                try {
                    // Se convierte el Array de Bytes a un Bitmap
                    Bitmap loadedImage = BitmapFactory.decodeByteArray(data, 0, data.length);

                    Bitmap rotatedBitmap;

                    if (cameraId == CameraInfo.CAMERA_FACING_FRONT) {
                        // Rotamos la imagen 270 grados o los que requiera en la galeria
                        Matrix rotateMatrix = new Matrix();
                        rotateMatrix.postRotate(270);
                        rotatedBitmap = Bitmap.createBitmap(loadedImage, 0, 0, loadedImage.getWidth(), loadedImage.getHeight(), rotateMatrix, false);
                    } else {
                        Matrix rotateMatrix = new Matrix();
                        rotateMatrix.postRotate(90);
                        rotatedBitmap = Bitmap.createBitmap(loadedImage, 0, 0, loadedImage.getWidth(), loadedImage.getHeight(), rotateMatrix, false);
                    }

                    ByteArrayOutputStream ostream = new ByteArrayOutputStream();

                    // Guardamos la imagen de momento en la galeria
                    rotatedBitmap.compress(CompressFormat.JPEG, 100, ostream);

                    Bitmap compressImage = Bitmap.createScaledBitmap(rotatedBitmap, 240, 320, false);
                    ByteArrayOutputStream ostream2 = new ByteArrayOutputStream();
                    compressImage.compress(CompressFormat.JPEG, 100, ostream2);

                    byte[] imageArray = ostream2.toByteArray();

                    // Query de la foto
                    PresenceArguments.Image = Base64.encodeToString(imageArray, 0);

                    ServerPost task = new ServerPost(context);
                    task.delegate = delegate;
                    task.execute(route, CustomGson.getGson().toJson(PresenceArguments));
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
        });
    }

    @Override
    public void onAsyncFinish(HashMap<String, String> map) {
        if (HandleServerError.Handle(map, this, this)) return ;

        if (map.get("route").contains(getResources().getString(R.string.apiControlPresenceMobileCheck))){
            ControlPresenceCheckResult res = CustomGson.getGson().fromJson(map.get("json"), ControlPresenceCheckResult.class);

            HandleCheck.Handle(saveTrack, this, this, res);
        }
    }
}
