package com.visport.presto20;

import android.content.Intent;
import android.os.Handler;
import android.util.Log;

import com.google.android.gms.wearable.MessageEvent;
import com.google.android.gms.wearable.WearableListenerService;

import java.nio.ByteBuffer;

public class UnitySenderService extends WearableListenerService {

    private final Handler handler = new Handler();

    private String wearMessage = "";

    private Runnable sendData = new Runnable() {
        public void run() {
            Log.d("Inside run()", "daje");
            // sendIntent is the object that will be broadcast outside our app
            Intent sendIntent = new Intent();

            // We add flags for example to work from background
            sendIntent.addFlags(Intent.FLAG_ACTIVITY_NO_ANIMATION|Intent.FLAG_FROM_BACKGROUND|Intent.FLAG_INCLUDE_STOPPED_PACKAGES  );

            // SetAction uses a string which is an important name as it identifies the sender of the itent and that we will give to the receiver to know what to listen.
            // By convention, it's suggested to use the current package name
            sendIntent.setAction("com.visport.unitypluginlibrary.UnityViSportIntent");

            // Here we fill the Intent with our data, here just a string with an incremented number in it.
            // Dobbiamo trovare il modo di metterci il messaggio
            sendIntent.putExtra(Intent.EXTRA_TEXT, wearMessage);
            Log.d("Sending...", sendIntent.getStringExtra(Intent.EXTRA_TEXT));
            // And here it goes ! our message is send to any other app that want to listen to it.
            sendBroadcast(sendIntent);
        }
    };

    @Override
    public void onMessageReceived(MessageEvent messageEvent){
        Log.d("MESSAGE", "RECEIVED");
        byte[] payload = messageEvent.getData();
        ByteBuffer byteBuffer = ByteBuffer.wrap(payload);
        float azimuth = byteBuffer.getFloat();
        float pitch = byteBuffer.getFloat();
        float roll = byteBuffer.getFloat();

        String message = azimuth + " " + pitch + " " + roll;
        wearMessage = message;

        Log.d("MESSAGE", wearMessage);
        handler.removeCallbacks(sendData);
        Log.d("MESSAGE", "removeCallbacks done");
        handler.post(sendData);
        //handler.postDelayed(sendData, 1000);
        Log.d("MESSAGE", "Sent");
    }

    // When service is started
    @Override
    public void onStart(Intent intent, int startid) {
        Log.d("UnitySenderService", "START");
        // We first start the Handler

    }

}