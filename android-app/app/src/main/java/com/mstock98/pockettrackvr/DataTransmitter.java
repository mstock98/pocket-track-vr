package com.mstock98.pockettrackvr;

import android.os.AsyncTask;
import android.util.Log;

import java.io.PrintWriter;
import java.net.InetSocketAddress;
import java.net.Socket;

/**
 * Handles data transmission to the VR client
 */
public class DataTransmitter {
    protected String _address;
    protected int _port;
    protected int _socketTimeout;

    /**
     * Constructor for the DataTransmitter class
     * @param address - the IP address of the VR client
     * @param port - the port of the VR client
     * @param socketTimeout - the time (milliseconds) that the DataTransmitter should wait before a
     *                        step transmission is deemed unsuccessful
     */
    public DataTransmitter(String address, int port, int socketTimeout) {
        _address = address;
        _port = port;
        _socketTimeout = socketTimeout;

        Log.v("DataTransmitter", "DataTransmitter created with address: " + address + " and port: " + port);
    }

    /**
     * Asynchronous transmission task that gets created with every call of transmitStepCount()
     */
    private class TransmitTask extends AsyncTask<Integer, Void, String> {

        /**
         * Main background transmission task. This gets called on execute()
         * @param ints - step count to send to the VR client
         * @return success status for logging
         */
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

        /**
         * Logs the result of the TransmitTask
         * @param result - TransmitTask result
         */
        @Override
        protected void onPostExecute(String result) {
            if (result.equals("Success")) {
                Log.v("TransmitTask", "Step successfully transmitted");
            } else {
                Log.e("TransmitTask", result);
            }
        }
    }

    /**
     * Main method to call in order to send a step to the VR client
     * @param stepCount - step count to send to the VR client
     */
    public Void transmitStepCount(int stepCount) {
        TransmitTask task = new TransmitTask();
        task.execute(stepCount);
        return null;
    }
}
