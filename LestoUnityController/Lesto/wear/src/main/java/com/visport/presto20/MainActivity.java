package com.visport.presto20;

import android.hardware.Sensor;
import android.hardware.SensorEvent;
import android.hardware.SensorEventListener;
import android.hardware.SensorManager;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.wearable.activity.WearableActivity;
import android.util.Log;

import com.google.android.gms.common.api.GoogleApiClient;
import com.google.android.gms.common.api.ResultCallback;
import com.google.android.gms.wearable.MessageApi;
import com.google.android.gms.wearable.Node;
import com.google.android.gms.wearable.NodeApi;
import com.google.android.gms.wearable.Wearable;

import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.TimeUnit;

public class MainActivity extends WearableActivity implements SensorEventListener{
    private SensorManager sensorManager;
    private static final String TAG = "WearActivity";
    private GoogleApiClient mGoogleApiClient;
    private static final String WEAR_ORIENTATION_MESSAGE_PATH = "/wear_orientation_path";
    private static final long CONNECTION_TIME_OUT_MS = 2500;
    private static final long MAX_MILLIS_BETWEEN_UPDATES = 3;
    private static long mLastOrientationSent = 0;
    private static String mOrientationReceiverNodeId = null;
    private float[] mGravity;
    private float[] mMagnetic;
    // TO DO
    private float[] mLinearAccelerometer;
    private float[] mGyroscope;
    private List<Node> myNodes = new ArrayList<>();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        setAmbientEnabled();
        mGoogleApiClient = new GoogleApiClient.Builder(this)
                .addApi(Wearable.API)
                .build();
        getNodes();
        sensorManager = (SensorManager)getSystemService(SENSOR_SERVICE);
        sensorManager.registerListener(this,
                sensorManager.getDefaultSensor(Sensor.TYPE_ACCELEROMETER),
                SensorManager.SENSOR_DELAY_GAME);
        sensorManager.registerListener(this,
                sensorManager.getDefaultSensor(Sensor.TYPE_GYROSCOPE),
                SensorManager.SENSOR_DELAY_GAME);
        sensorManager.registerListener(this,
                sensorManager.getDefaultSensor(Sensor.TYPE_MAGNETIC_FIELD),
                SensorManager.SENSOR_DELAY_GAME);
    }

    private void getNodes(){
        new Thread(new Runnable() {
            @Override
            public void run() {
                Log.d(TAG,"Getting nodes...");

                mGoogleApiClient.blockingConnect(CONNECTION_TIME_OUT_MS, TimeUnit.MILLISECONDS);

                NodeApi.GetConnectedNodesResult result = Wearable.NodeApi.getConnectedNodes(mGoogleApiClient).await();
                List<Node> nodes = result.getNodes();

                for(Node n:nodes){
                    Log.d(TAG,"Adding Node: "+n.getDisplayName());
                    myNodes.add(n);
                    mOrientationReceiverNodeId = n.getId();
                }

                Log.d(TAG,"Getting nodes DONE!");
            }
        }).start();
    }

    @Override
    public void onSensorChanged(SensorEvent event){
        String orientationReceiverNodeId = mOrientationReceiverNodeId;
        if(orientationReceiverNodeId == null){
            Log.d("NODE IS NULL", "A");
            return;
        }

        if(System.currentTimeMillis() - mLastOrientationSent < MAX_MILLIS_BETWEEN_UPDATES){
            //Log.d("TOO SOON", "A");
            return;
        }

        if(event.sensor.getType() == Sensor.TYPE_ACCELEROMETER){
            //Log.d("SENSOR", "ACCEL");
            mGravity = event.values;
        }
        if(event.sensor.getType() == Sensor.TYPE_MAGNETIC_FIELD){
            //Log.d("SENSOR", "MAGNETIC");
            mMagnetic = event.values;
        }
        if(event.sensor.getType() == Sensor.TYPE_GYROSCOPE){
            //Log.d("SENSOR", "GYRO");
            mGyroscope = event.values;
        }
        if(mGravity != null && mMagnetic != null && mGyroscope != null){
            Log.d("READY TO SEND", "a");
            int totalValues = 4 * 3;
            float R[] = new float[totalValues];
            boolean success = SensorManager.getRotationMatrix(R, null, mGravity, mMagnetic);

            if (success) {
                float orientation[] = new float[3];
                SensorManager.getOrientation(R, orientation);
                sendOrientation(orientationReceiverNodeId, orientation[0], orientation[1], orientation[2]);
            } else {
                Log.e(TAG, "Couldn't get rotation matrix");
            }
        }
    }

    private void sendOrientation(String node, final float azimuth, final float pitch, final float roll){
        Log.d("SENDING TO ", node);
        mLastOrientationSent = System.currentTimeMillis();
        ByteBuffer byteBuffer = ByteBuffer.allocate(12);
        byteBuffer.putFloat(azimuth);
        byteBuffer.putFloat(pitch);
        byteBuffer.putFloat(roll);
        final byte[] data = byteBuffer.array();
        Wearable.MessageApi.sendMessage(mGoogleApiClient, node,
                WEAR_ORIENTATION_MESSAGE_PATH, data).setResultCallback(new ResultCallback<MessageApi.SendMessageResult>() {
            @Override
            public void onResult(@NonNull MessageApi.SendMessageResult sendMessageResult) {
                if(!sendMessageResult.getStatus().isSuccess()){
                    Log.e(TAG, "Couldn't send message: " + sendMessageResult);
                } else{
                    Log.d(TAG, "MESSAGE SENT");
                }
            }
        });
        Log.d("BYTE ARRAY SENT", "a");
    }

    @Override
    public void onAccuracyChanged(Sensor s, int i){

    }
}
