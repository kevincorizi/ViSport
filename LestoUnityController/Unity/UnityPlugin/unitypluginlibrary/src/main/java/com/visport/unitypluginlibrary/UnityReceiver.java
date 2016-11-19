package com.visport.unitypluginlibrary;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.util.Log;

/**
 * Created by kevin on 18/11/2016.
 */

public class UnityReceiver extends BroadcastReceiver {
    private static UnityReceiver instance;

    // Text to be read by Unity
    public static String message = "";

    @Override
    public void onReceive(Context context, Intent intent){
        // We get the intent data
        Log.d("onReceive", "TRIGGERED");
        String sentIntent = intent.getStringExtra(Intent.EXTRA_TEXT);
        if(sentIntent != null){
            Log.d("SentIntent", sentIntent);
            message = sentIntent;
        } else{
            Log.d("SentIntent", "NULL");
        }
    }

    public static void createInstance(){
        if(instance != null){
            instance = new UnityReceiver();
        }
    }
}
