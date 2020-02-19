package com.mstock98.pockettrackvr;

import android.hardware.Sensor;
import android.hardware.SensorManager;
import android.content.Context;

import static android.content.Context.SENSOR_SERVICE;

public class SensorDriver {
    private int stepCount; // number of steps that have been taken since the start of recording
    private final SensorManager sensorManager;

    private Sensor sensor;

    public SensorDriver() {
        sensorManager = (SensorManager) getSystemService(SENSOR_SERVICE);

        stepCount = 0;
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
