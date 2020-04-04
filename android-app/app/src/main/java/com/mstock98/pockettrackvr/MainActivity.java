package com.mstock98.pockettrackvr;

import androidx.appcompat.app.AppCompatActivity;

import android.hardware.Sensor;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;

import java.util.Observable;
import java.util.Observer;

public class MainActivity extends AppCompatActivity {
    Button btnToggleTracking;

    TextView lblStepCount;
    private SensorDriver _sensorDriver;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        lblStepCount = findViewById(R.id.lblStepCount);

        btnToggleTracking = findViewById(R.id.btnToggleTracking);

        _sensorDriver = new SensorDriver(this);
        _sensorDriver.addObserver(new Observer() {
            @Override
            public void update(Observable o, Object arg) {
                lblStepCount.setText("Step Count: " + ((SensorDriver) o).getStepCount());
            }
        });

        btnToggleTracking.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (_sensorDriver.isRecording()) {
                    _sensorDriver.pauseRecording();
                    btnToggleTracking.setText("Start Tracking");
                } else {
                    _sensorDriver.resumeRecording();
                    btnToggleTracking.setText("Stop Tracking");
                }
            }
        });
    }
}
