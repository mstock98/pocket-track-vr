package com.mstock98.pockettrackvr;

import androidx.appcompat.app.AppCompatActivity;

import android.media.MediaPlayer;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.net.Socket;
import java.util.Observable;
import java.util.Observer;

public class MainActivity extends AppCompatActivity {
    Button btnToggleTracking;
    TextView lblStepCount;
    private SensorDriver _sensorDriver;
    private MediaPlayer _stepMediaPlayer;
    private DataTransmitter _sendDataToVRClient;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        _stepMediaPlayer = MediaPlayer.create(this, R.raw.step);

        lblStepCount = findViewById(R.id.lblStepCount);

        btnToggleTracking = findViewById(R.id.btnToggleTracking);

        _sensorDriver = new SensorDriver(this);
        _sensorDriver.addObserver(new Observer() {
            @Override
            public void update(Observable o, Object arg) {
                int stepCount = ((SensorDriver) o).getStepCount();
                lblStepCount.setText("Step Count: " + stepCount);
                _stepMediaPlayer.start();
                _sendDataToVRClient.execute(stepCount);
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
