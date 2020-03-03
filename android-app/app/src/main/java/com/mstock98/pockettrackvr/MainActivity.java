package com.mstock98.pockettrackvr;

import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;
import android.view.View;
import android.widget.Button;

// Data binding? - https://www.vogella.com/tutorials/AndroidDatabinding/article.html

public class MainActivity extends AppCompatActivity {
    Button btnToggleTracking;
    private SensorDriver _sensorDriver;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        _sensorDriver = new SensorDriver(this);

        btnToggleTracking = findViewById(R.id.btnToggleTracking);

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
