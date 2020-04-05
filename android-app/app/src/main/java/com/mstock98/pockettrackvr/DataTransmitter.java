package com.mstock98.pockettrackvr;

import android.os.AsyncTask;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.net.Socket;

public class DataTransmitter extends AsyncTask<Integer, Void, Void> {
    private Socket _socketToVRClient;
    private PrintWriter _out;
    private BufferedReader _in;

    public DataTransmitter(String address, int port) {
        try {
            _socketToVRClient = new Socket(address, port);
        } catch (Exception e) {
            System.out.println(e.toString());
        }

        try {
            _out = new PrintWriter(_socketToVRClient.getOutputStream(), true);
        } catch (Exception e) {
            System.out.println(e.toString());
        }

        try {
            _in = new BufferedReader(new InputStreamReader(_socketToVRClient.getInputStream()));
        } catch (Exception e) {
            System.out.println(e.toString());
        }
    }

    @Override
    protected Void doInBackground(Integer... ints) {
        _out.print(ints[0]);
        return null;
    }
}
