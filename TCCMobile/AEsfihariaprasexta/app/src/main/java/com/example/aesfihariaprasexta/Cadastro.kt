package com.example.aesfihariaprasexta

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle

class Cadastro : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_cadastro)
        if(supportActionBar != null){
            supportActionBar!!.hide()
        }
    }
}