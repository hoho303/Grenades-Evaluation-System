package ml.bookapp.nemluudan.Model.XuLy

import android.content.Context
import android.util.Log
import com.android.volley.Request
import com.android.volley.Response
import com.android.volley.toolbox.StringRequest
import com.android.volley.toolbox.Volley
import ml.bookapp.nemluudan.Model.ObjectClass.DuLieuDiem

/**
 * Created by HOANGTHUAN on 1/15/2019.
 */
class XuLyServer {
    var context : Context

    constructor(context: Context) {
        this.context = context
    }

    fun getDataFromServer(link : String,callback : (String)->Unit)
    {
        Log.d("AAA",link)
        var queue = Volley.newRequestQueue(context)
        var stringRequest = StringRequest(Request.Method.GET,link,Response.Listener<String>{
            Log.d("BBB",it)
            callback(it)
        },Response.ErrorListener {
        })
        queue.add(stringRequest)
    }
}