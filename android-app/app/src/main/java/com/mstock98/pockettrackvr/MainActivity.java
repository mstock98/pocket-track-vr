package com.mstock98.pockettrackvr;

import androidx.appcompat.app.AppCompatActivity;

import android.app.AlertDialog;
import android.content.DialogInterface;
import android.media.MediaPlayer;
import android.os.Bundle;
import android.text.InputType;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;

import java.util.Observable;
import java.util.Observer;

/**
 * Main screen of the PTVR Android app
 */
public class MainActivity extends AppCompatActivity {
    Button btnToggleTracking;
    Button btnChangeAddress;
    TextView lblStepCount;
    private SensorDriver _sensorDriver;
    private MediaPlayer _stepMediaPlayer;
    private DataTransmitter _dataTransmitter;
    private final int _STEP_TRANSMISSION_TIMEOUT = 2000;

    /**
     * Sets up the main UI of the app
     */
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        _stepMediaPlayer = MediaPlayer.create(this, R.raw.step);

        lblStepCount = findViewById(R.id.lblStepCount);
        lblStepCount.setText(getString(R.string.step_count, 0));

        btnToggleTracking = findViewById(R.id.btnToggleTracking);
        btnChangeAddress = findViewById(R.id.btnChangeAddress);

        createDataTransmitterFromUserInput();

        _sensorDriver = new SensorDriver(this);
        _sensorDriver.addObserver(new Observer() {
            @Override
                public void update(Observable o, Object arg) {
                int stepCount = ((SensorDriver) o).getStepCount();
                lblStepCount.setText(getString(R.string.step_count, stepCount));
                _stepMediaPlayer.start();
                _dataTransmitter.transmitStepCount(stepCount);
            }
        });

        btnToggleTracking.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (_sensorDriver.isRecording()) {
                    _sensorDriver.pauseRecording();
                    btnToggleTracking.setText(R.string.btn_toggle_tracking_start);
                } else {
                    _sensorDriver.resumeRecording();
                    btnToggleTracking.setText(R.string.btn_toggle_tracking_stop);
                }
            }
        });

        btnChangeAddress.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                createDataTransmitterFromUserInput();
            }
        });
    }

    // https://stackoverflow.com/questions/10903754/input-text-dialog-android

    /**
     * Displays an AlertDialog asking the user for the IP address and the port of the
     * VR client, then creates a DataTransmitter based on that.
     */
    private void createDataTransmitterFromUserInput() {
        final String[] address = new String[1];
        final String[] port = new String[1];

        final AlertDialog.Builder ipDialogBuilder = new AlertDialog.Builder(this);
        final AlertDialog.Builder portDialogBuilder = new AlertDialog.Builder(this);

        // Set up dialog to get IP address from user
        ipDialogBuilder.setTitle(R.string.ip_input_title);

        final EditText addressInput = new EditText(this);
        addressInput.setInputType(InputType.TYPE_CLASS_PHONE);
        ipDialogBuilder.setView(addressInput);

        ipDialogBuilder.setPositiveButton(R.string.btn_ok, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                address[0] = addressInput.getText().toString();
                portDialogBuilder.show();
            }
        });

        ipDialogBuilder.setCancelable(false);

        // Set up dialog to get port from user
        portDialogBuilder.setTitle(R.string.port_input_title);

        final EditText portInput = new EditText(this);
        portInput.setInputType(InputType.TYPE_CLASS_NUMBER);
        portDialogBuilder.setView(portInput);

        portDialogBuilder.setPositiveButton(R.string.btn_ok, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                port[0] = portInput.getText().toString();

                try {
                    _dataTransmitter = new DataTransmitter(address[0], Integer.parseInt(port[0]), _STEP_TRANSMISSION_TIMEOUT);
                } catch (NumberFormatException e) {
                    Log.e("MainActivity", e.toString());
                }

                Log.v("MainActivity", "Created DataTransmitter with address: " + address[0] + " on port: " + port[0]);
            }
        });

        portDialogBuilder.setCancelable(false);

        // Show the dialogs to the user
        ipDialogBuilder.show();
    }
}
