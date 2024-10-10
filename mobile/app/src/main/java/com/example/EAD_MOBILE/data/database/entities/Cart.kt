package com.example.EAD_MOBILE.data.database.entities

import android.os.Parcelable
import androidx.room.ColumnInfo
import androidx.room.Entity
import androidx.room.PrimaryKey
import kotlinx.android.parcel.Parcelize

@Parcelize
@Entity(tableName = "cart")
data class Cart (

    @PrimaryKey(autoGenerate = true)
    val id: Long = 0L,

    @ColumnInfo(name = "productId")
    var productId: String,

    @ColumnInfo(name = "vendorId")
    val vendorId: String,

    @ColumnInfo(name = "quantity")
    val quantity: Int?,

    @ColumnInfo(name = "name")
    val name: String,

    @ColumnInfo(name = "price")
    val price: Double?

) : Parcelable