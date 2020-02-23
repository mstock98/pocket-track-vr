package com.mstock98.pockettrackvr;

import android.hardware.Sensor;
import android.hardware.SensorManager;
import android.content.Context;

import static android.content.Context.SENSOR_SERVICE;

public class SensorDriver {
    private int stepCount; // number of steps that have been taken since the start of recording
    private final SensorManager mSensorManager;
    private final Sensor mAccelermeter;

    private Sensor sensor;

    /**
     * Initiates a sensor suite for measuring orientation and footfalls
     * @param mContext - State of the application. If calling from an activity, pass in "this"
     */
    public SensorDriver(Context mContext) {
        this.mSensorManager = (SensorManager) mContext.getSystemService(SENSOR_SERVICE);
        this.mAccelermeter = mSensorManager.getDefaultSensor(Sensor.TYPE_ACCELEROMETER);

        stepCount = 0;
    }

    /**
     * Call this method to start monitoring sensors
     */
    public void resumeRecording() {
        mSensorManager.registerListener(this, mAccelermeter, SensorManager.SENSOR_DELAY_NORMAL);
    }

    /**
     * Call this method when sensors aren't needed/app is minimized - to save on battery
     */
    public void pauseRecording() {
        mSensorManager.unregisterListener(this);
    }

    // Returns acceleration (m/s^2) along the x, y, and z axes - respectively
    public float[] getAccelerometerData() {
        return null;
    }

    // Returns rate of rotation (rad/s) along the x, y, and z axes - respectively
    public float[] getGyroscopeData() {
        return null;
    }

    // Returns number of steps that have been taken since the start of recording
    public int getStepCount() {
        return stepCount;
    }
}
