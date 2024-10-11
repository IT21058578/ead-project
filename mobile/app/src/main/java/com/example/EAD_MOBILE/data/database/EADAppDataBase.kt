package com.example.EAD_MOBILE.data.database

import android.content.Context
import androidx.room.Database
import androidx.room.Room
import androidx.room.RoomDatabase
import androidx.sqlite.db.SupportSQLiteDatabase
import com.example.EAD_MOBILE.data.database.dao.CartDao
import com.example.EAD_MOBILE.data.database.entities.Cart
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch

@Database(entities = [Cart::class], version = 1,exportSchema = true)

abstract class EADAppDataBase: RoomDatabase() {
    abstract fun cartDao(): CartDao


    companion object {
        @Volatile
        private var INSTANCE: EADAppDataBase? = null

        fun getDatabase(
            context: Context,
            scope: CoroutineScope
        ): EADAppDataBase {
            // if the INSTANCE is not null, then return it,
            // if it is, then create the database
            return INSTANCE ?: synchronized(this) {
                val instance = Room.databaseBuilder(
                    context.applicationContext,
                    EADAppDataBase::class.java,
                    "metro_app_database"
                )
                    // Wipes and rebuilds instead of migrating if no Migration object.
                    // Migration is not part of this
                    .fallbackToDestructiveMigration()
                    .addCallback(EADAppDatabaseCallback(scope))
                    // .allowMainThreadQueries()
                    .build()
                INSTANCE = instance
                // return instance
                instance
            }
        }

        private class EADAppDatabaseCallback(
            private val scope: CoroutineScope
        ) : RoomDatabase.Callback() {
            /**
             * Override the onCreate method to populate the database.
             */
            override fun onCreate(db: SupportSQLiteDatabase) {
                super.onCreate(db)
                INSTANCE?.let { database ->
                    scope.launch(Dispatchers.IO) {
                        //populateDatabase(database.jobDao())
                    }
                }
            }
        }

        }


}