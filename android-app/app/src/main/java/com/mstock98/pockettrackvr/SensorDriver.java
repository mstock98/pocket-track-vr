package com.mstock98.pockettrackvr;

import android.hardware.Sensor;
import android.hardware.SensorEvent;
import android.hardware.SensorEventListener;
import android.hardware.SensorManager;
import android.content.Context;

import java.util.Observable;

import static android.content.Context.SENSOR_SERVICE;

/**
 * Interfaces with the Android hardware sensors (pedometer for now)
 * Alerts observers when a step is detected
 */
public class SensorDriver extends Observable {
    private final SensorManager _mSensorManager;

    private final Sensor _mStepDetector;

    private final SensorEventListener _mStepDetectorListener;

    private final int _SENSOR_SAMPLING_PERIOD_US = 8333; // 8333 microseconds ~= 120 Hz sample rate

    private int _stepCount; // number of steps that have been taken since the start of recording

    private boolean _isRecording;

    /**
     * Initiates a sensor suite for measuring orientation and footfalls
     * @param mContext - State of the application. If calling from an activity, pass in "this"
     */
    public SensorDriver(Context mContext) {
        _stepCount = 0;
        _isRecording = false;

        this._mSensorManager = (SensorManager) mContext.getSystemService(SENSOR_SERVICE);
        this._mStepDetector = _mSensorManager.getDefaultSensor(Sensor.TYPE_STEP_DETECTOR);

        this._mStepDetectorListener = new SensorEventListener() {
            @Override
            public void onSensorChanged(SensorEvent event) {
                _stepCount++; // if a step is detected, increment the step count
                setChanged(); // Set the SensorDriver object's observer "changed" status to true
                notifyObservers();
            }

            @Override
            public void onAccuracyChanged(Sensor sensor, int accuracy) { } // Do nothing for now, here to satisfy interface
        };
    }

    /**
     * Call this method to start monitoring the pedometer
     */
    public void resumeRecording() {
        _mSensorManager.registerListener(_mStepDetectorListener, _mStepDetector, _SENSOR_SAMPLING_PERIOD_US);
        _isRecording = true;
    }

    /**
     * Call this method when the pedometer isn't needed/app is minimized - to save on battery
     */
    public void pauseRecording() {
        _mSensorManager.unregisterListener(_mStepDetectorListener);
        _isRecording = false;
    }

    /**
     * Checks if the SensorDriver is actively reading values from hardware sensors
     * @return true if the hardware sensors are being read, false otherwise
     */
    public boolean isRecording() { return _isRecording; }

    /**
     * Gets the number of steps that have been recorded since the object was created
     * @return step count
     */
    public int getStepCount() {
        return _stepCount;
    }
}
