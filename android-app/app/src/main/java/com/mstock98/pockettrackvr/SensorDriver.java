package com.mstock98.pockettrackvr;

import android.hardware.Sensor;
import android.hardware.SensorEvent;
import android.hardware.SensorEventListener;
import android.hardware.SensorManager;
import android.content.Context;

import java.util.Observable;

import static android.content.Context.SENSOR_SERVICE;

public class SensorDriver extends Observable {
    private final SensorManager _mSensorManager;

    private final Sensor _mAccelerometer;

    private final SensorEventListener _mAccelerometerListener;

    private Sensor _sensor;

    private final int SENSOR_SAMPLING_PERIOD_US = 8333; // 8333 microseconds ~= 120 Hz sample rate

    private float[] _accelerationValues;
    //public ObservableFloat accelerationX;
    //public ObservableFloat accelerationY;
    //public ObservableFloat accelerationZ;

    private int _stepCount; // number of steps that have been taken since the start of recording

    private boolean _isRecording;

    /**
     * Initiates a sensor suite for measuring orientation and footfalls
     * @param mContext - State of the application. If calling from an activity, pass in "this"
     */
    public SensorDriver(Context mContext) {
        this._mSensorManager = (SensorManager) mContext.getSystemService(SENSOR_SERVICE);
        this._mAccelerometer = _mSensorManager.getDefaultSensor(Sensor.TYPE_ACCELEROMETER);

        this._mAccelerometerListener = new SensorEventListener() {
            @Override
            public void onSensorChanged(SensorEvent event) {
                _accelerationValues = event.values;
                setChanged(); // Set the SensorDriver object's observer "changed" status to true
            }

            @Override
            public void onAccuracyChanged(Sensor sensor, int accuracy) { } // Do nothing for now, here to satisfy interface
        };

        _stepCount = 0;
        _isRecording = false;
    }

    /**
     * Call this method to start monitoring sensors
     */
    public void resumeRecording() {
        _mSensorManager.registerListener(_mAccelerometerListener, _mAccelerometer, SENSOR_SAMPLING_PERIOD_US);
        _isRecording = true;
    }

    /**
     * Call this method when sensors aren't needed/app is minimized - to save on battery
     */
    public void pauseRecording() {
        _mSensorManager.unregisterListener(_mAccelerometerListener);
        _isRecording = false;
    }

    /**
     * Checks if the SensorDriver is actively reading values from hardware sensors
     * @return true if the hardware sensors are being read, false otherwise
     */
    public boolean isRecording() { return _isRecording; }

    // Returns acceleration (m/s^2) along the x, y, and z axes - respectively
    public float[] getAccelerometerData() {
        return _accelerationValues;
    }

    // Returns rate of rotation (rad/s) along the x, y, and z axes - respectively
    public float[] getGyroscopeData() {
        return null;
    }

    // Returns number of steps that have been taken since the start of recording
    public int getStepCount() {
        return _stepCount;
    }
}
