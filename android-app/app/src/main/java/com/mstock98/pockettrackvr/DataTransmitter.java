package com.mstock98.pockettrackvr;

import android.os.AsyncTask;

import java.io.PrintWriter;
import java.net.Socket;

public class DataTransmitter {
    protected String _address;
    protected int _port;

    public DataTransmitter(String address, int port) {
        _address = address;
        _port = port;
    }

    private class TransmitTask extends AsyncTask<Integer, Void, Void> {
        @Override
        protected Void doInBackground(Integer... ints) {
            PrintWriter out;
            Socket socketToVRClient;

            try {
                socketToVRClient = new Socket(_address, _port);
            } catch (Exception e) {
                System.out.println("Socket establishment failed: " + e.toString());
                return null;
            }

            try {
                out = new PrintWriter(socketToVRClient.getOutputStream(), true);
            } catch (Exception e) {
                System.out.println("PrintWriter establishment failed: " + e.toString());
                return null;
            }

            out.print(ints[0]);
            out.flush();
            out.print("exit");
            out.flush();

            return null;
        }
    }

    public Void transmitStepCount(int stepCount) {
        TransmitTask task = new TransmitTask();
        task.execute(stepCount);
        return null;
    }
}
