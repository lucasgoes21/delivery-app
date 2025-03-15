package com.example.aesfihariaprasexta

import android.content.Intent
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.text.method.Touch
import android.view.View
import kotlinx.android.synthetic.main.activity_main.*
import kotlinx.android.synthetic.main.activity_splash_loading_login.*

class SplashLoadingLogin : AppCompatActivity(), View.OnClickListener {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_splash_loading_login)
        if (supportActionBar != null) {
            supportActionBar!!.hide()
        }
        buttonEntrar.setOnClickListener(this)
    }

    fun EntrarNaTelaMain() {
        val intent = Intent(this, MainActivity::class.java)
        startActivity(intent)
    }


    override fun onClick(view: View) {
        val id = view.id
        if (id == buttonEntrar.id) EntrarNaTelaMain()
        EntrarNaTelaMain()
    }
}