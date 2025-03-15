package com.example.aesfihariaprasexta

import android.content.Intent
import android.media.midi.MidiManager
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.view.View
import kotlinx.android.synthetic.main.activity_main.*
import kotlinx.android.synthetic.main.activity_splash_loading_login.*

class MainActivity : AppCompatActivity(), View.OnClickListener {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)
        if (supportActionBar != null) {
            supportActionBar!!.hide()
        }
        MainRegisterButton.setOnClickListener(this)
        MainLoginButton.setOnClickListener(this)
    }

    override fun onClick( view : View) {
        val id = view.id
        if(id == MainLoginButton.id){

        }
        if(id == MainRegisterButton.id){
            val intentRegister = Intent(this, Cadastro::class.java)
            startActivity(intentRegister)
        }
    }
}