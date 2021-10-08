package ml.bookapp.nemluudan.View

import android.os.Bundle
import android.support.v7.app.AppCompatActivity
import kotlinx.android.synthetic.main.activity_main.*
import ml.bookapp.nemluudan.Adapter.AdapterXuLyDiem
import ml.bookapp.nemluudan.Model.ObjectClass.DuLieuDiem
import ml.bookapp.nemluudan.Model.XuLy.XuLyServer
import ml.bookapp.nemluudan.R
import java.util.*
import android.media.MediaPlayer


class MainActivity : AppCompatActivity() {

    var adapterXuLyDiem:AdapterXuLyDiem? = null
    private var timeCount = 0
    var list = ArrayList<DuLieuDiem>()
    var lan = 1
    var lanNem = 1
    var timer: Timer? = Timer()
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)
        addControls()
    }

    private fun addControls() {
        adapterXuLyDiem = AdapterXuLyDiem(this, R.layout.custom_layout_diem, list)
        lvDiem.adapter = adapterXuLyDiem
        timer = Timer()
        timer!!.schedule(object : TimerTask() {
            override fun run() {
                runOnUiThread { getDiem() }
            }
        }, 5000, 5000)
    }

    private fun getDiem() {
        var xuLyServer = XuLyServer(this)
        xuLyServer.getDataFromServer(getString(R.string.server) + "getData" + lan) {
            if (it.toInt() > 0) {
                when (it.toInt()) {
                    10 -> Play("DIEM10.WAV")
                    9 -> Play("DIEM9.WAV")
                    8 -> Play("DIEM8.WAV")
                    7 -> Play("DIEM7.WAV")
                    6 -> Play("DIEM6.WAV")
                    5 -> Play("DIEM5.WAV")
                    4 -> Play("DIEM4.WAV")
                }
                txtDiem.text = it
                if (list.size < 5) {
                    list.add(DuLieuDiem(lanNem, it.toInt()))
                } else {
                    list.clear()
                    list.add(DuLieuDiem(lanNem, it.toInt()))
                }
                adapterXuLyDiem!!.notifyDataSetChanged()
                if (lan < 5) {
                    lan++
                    lanNem++
                } else {
                    lan = 1
                    lanNem = 1
                }
            } else {
                if(timeCount < 30)
                {
                    timeCount++
                }
                else
                {
                    xuLyServer.getDataFromServer(getString(R.string.server) + "getZero") {}
                    txtDiem.text = "" + 0
                    timeCount = 0
                    Play("DIEM0.WAV")
                    if (list.size < 5) {
                        list.add(DuLieuDiem(lanNem, 0))
                    } else {
                        list.clear()
                        list.add(DuLieuDiem(lanNem, 0))
                    }
                    lanNem++
                    adapterXuLyDiem!!.notifyDataSetChanged()
                }
            }
        }
    }

    fun Play(fileName: String) {
        val descriptor = assets.openFd(fileName)
        val start = descriptor.startOffset
        val end = descriptor.length
        val mediaPlayer = MediaPlayer()
        mediaPlayer.setDataSource(descriptor.fileDescriptor, start, end)
        mediaPlayer.prepare()
        mediaPlayer.start()
    }
}
