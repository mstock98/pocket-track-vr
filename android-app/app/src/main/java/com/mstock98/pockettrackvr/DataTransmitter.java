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
    private String _address;
    private int _port;

    public DataTransmitter(String address, int port) {
        _address = address;
        _port = port;
    }

    @Override
    protected Void doInBackground(Integer... ints) {
        try {
            _socketToVRClient = new Socket(_address, _port);
        } catch (Exception e) {
            System.out.println("Socket establishment failed: " + e.toString());
        }

        try {
            _out = new PrintWriter(_socketToVRClient.getOutputStream(), true);
        } catch (Exception e) {
            System.out.println("PrintWriter establishment failed: " + e.toString());
        }

        try {
            _in = new BufferedReader(new InputStreamReader(_socketToVRClient.getInputStream()));
        } catch (Exception e) {
            System.out.println("BufferedReader establishment failed: " + e.toString());
        }

        _out.print(ints[0]);
        return null;
    }
}
