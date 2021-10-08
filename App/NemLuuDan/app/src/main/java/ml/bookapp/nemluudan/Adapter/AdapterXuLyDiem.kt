package ml.bookapp.nemluudan.Adapter

import android.app.Activity
import android.content.Context
import android.view.View
import android.view.ViewGroup
import android.widget.ArrayAdapter
import android.widget.TextView
import ml.bookapp.nemluudan.Model.ObjectClass.DuLieuDiem
import ml.bookapp.nemluudan.R

/**
 * Created by HOANGTHUAN on 1/15/2019.
 */
class AdapterXuLyDiem : ArrayAdapter<DuLieuDiem> {
    var context : Activity
    var list : ArrayList<DuLieuDiem>
    var layout : Int

    constructor(context: Activity?, resource: Int, list: ArrayList<DuLieuDiem>) : super(
        context,
        resource,
        list
    ) {
        this.context = context!!
        this.layout = resource
        this.list = list
    }

    override fun getView(position: Int, convertView: View?, parent: ViewGroup?): View {

        var view = context.layoutInflater.inflate(layout,parent,false);
        var txtLanNem = view.findViewById<TextView>(R.id.txtCustomLan)
        var txtDiem = view.findViewById<TextView>(R.id.txtCustomDiem)
        if(list[position].diem != 10) {
            txtDiem.text = "0" + list[position].diem.toString()
        }
        txtLanNem.text = "Điểm lần " + list[position].lanNem.toString()
        return view
    }
}
