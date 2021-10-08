package ml.bookapp.nemluudan.Model.ConnectInternet

/**
 * Created by HOANGTHUAN on 1/15/2019.
 */


import android.os.AsyncTask

import java.io.BufferedReader
import java.io.IOException
import java.io.InputStream
import java.io.InputStreamReader
import java.net.HttpURLConnection
import java.net.MalformedURLException
import java.net.URL
import java.util.ArrayList

/**
 * Created by ADMIN on 13/02/2018.
 */

class DownloadDiem : AsyncTask<String, Void, String>() {
    override fun doInBackground(vararg strings: String): String {
        var dulieu = ""
        try {
            val url = URL(strings[0])
            val connection = url.openConnection() as HttpURLConnection
            val inputStreamReader = InputStreamReader(connection.inputStream)
            val reader = BufferedReader(inputStreamReader)
            val builder = StringBuilder()
            var line: String? = reader.readLine()
            while (line != null) {
                builder.append(line)
                line = reader.readLine()
            }
            dulieu = builder.toString()
        } catch (e: IOException) {
            e.printStackTrace()
        }
        return dulieu
    }
}