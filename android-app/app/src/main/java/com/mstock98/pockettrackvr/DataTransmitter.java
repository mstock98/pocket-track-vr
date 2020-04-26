package com.mstock98.pockettrackvr;

import android.os.AsyncTask;
import android.util.Log;

import java.io.PrintWriter;
import java.net.InetSocketAddress;
import java.net.Socket;

public class DataTransmitter {
    protected String _address;
    protected int _port;
    protected int _socketTimeout;

    public DataTransmitter(String address, int port, int socketTimeout) {
        _address = address;
        _port = port;
        _socketTimeout = socketTimeout;

        Log.v("DataTransmitter", "DataTransmitter created with address: " + address + " and port: " + port);
    }

    private class TransmitTask extends AsyncTask<Integer, Void, String> {
        @Override
        protected String doInBackground(Integer... ints) {
            PrintWriter out;
            Socket socketToVRClient;

            try {
                socketToVRClient = new Socket();
                InetSocketAddress socketAddress = new InetSocketAddress(_address, _port);
                socketToVRClient.connect(socketAddress, _socketTimeout);
            } catch (Exception e) {
                return "Socket error: " + e.toString();
            }

            try {
                out = new PrintWriter(socketToVRClient.getOutputStream(), true);
            } catch (Exception e) {
                return "PrintWriter error: " + e.toString();
            }

            out.print(ints[0]);
            out.flush();
            out.print("exit");
            out.flush();

            return "Success";
        }

        @Override
        protected void onPostExecute(String result) {
            if (result.equals("Success")) {
                Log.v("TransmitTask", "Step successfully transmitted");
            } else {
                Log.e("TransmitTask", result);
            }
        }
    }

    public Void transmitStepCount(int stepCount) {
        TransmitTask task = new TransmitTask();
        task.execute(stepCount);
        return null;
    }
}
