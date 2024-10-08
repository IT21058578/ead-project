package com.example.EAD_MOBILE.dummies

import com.example.EAD_MOBILE.R

class Product (
    val productName: String,
    val imageId: Int,
    val origin: String,
    val price: Float
)

val productListFruits = listOf(
    Product(
        productName = "Kiwi",
        imageId = R.drawable.kiwi, // Replace with the actual image resource ID for kiwi
        origin = "New Zealand",
        price = 2.50f
    ),
    Product(
        productName = "Green Apple",
        imageId = R.drawable.green_apple, // Replace with the actual image resource ID for green apple
        origin = "United States",
        price = 1.75f
    ),
    Product(
        productName = "Coconut",
        imageId = R.drawable.cocunut, // Replace with the actual image resource ID for coconut
        origin = "Tropical regions",
        price = 3.00f
    ),
    Product(
        productName = "Lime",
        imageId = R.drawable.lime, // Replace with the actual image resource ID for lime
        origin = "Mexico",
        price = 1.25f
    ),
    Product(
        productName = "Pear",
        imageId = R.drawable.pear, // Replace with the actual image resource ID for pear
        origin = "United States",
        price = 1.50f
    ),
    Product(
        productName = "Pineapple",
        imageId = R.drawable.pineapple, // Replace with the actual image resource ID for pineapple
        origin = "Tropical regions",
        price = 2.75f
    )
)
val productListVegetables = listOf(
    Product(
        productName = "Spinach",
        imageId = R.drawable.spinach, // Replace with the actual image resource ID for spinach
        origin = "Various",
        price = 1.50f
    ),
    Product(
        productName = "Tomato",
        imageId = R.drawable.tomato, // Replace with the actual image resource ID for tomato
        origin = "Various",
        price = 0.75f
    ),
    Product(
        productName = "Carrot",
        imageId = R.drawable.carrot, // Replace with the actual image resource ID for carrot
        origin = "Various",
        price = 1.00f
    ),
    Product(
        productName = "Brinjal",
        imageId = R.drawable.brinjal, // Replace with the actual image resource ID for brinjal (eggplant)
        origin = "Various",
        price = 1.25f
    ),
    Product(
        productName = "Potato",
        imageId = R.drawable.potato, // Replace with the actual image resource ID for potato
        origin = "Various",
        price = 0.80f
    )
)
val productListSeafood = listOf(
    Product(
        productName = "Lobster",
        imageId = R.drawable.lobster, // Replace with the actual image resource ID for lobster
        origin = "Various",
        price = 25.00f
    ),
    Product(
        productName = "Shrimp",
        imageId = R.drawable.shrimp, // Replace with the actual image resource ID for shrimp
        origin = "Various",
        price = 15.00f
    ),
    Product(
        productName = "Salmon",
        imageId = R.drawable.salmon, // Replace with the actual image resource ID for salmon
        origin = "Various",
        price = 20.00f
    )
)
val productListMeat = listOf(
    Product(
        productName = "Pork",
        imageId = R.drawable.pork, // Replace with the actual image resource ID for pork
        origin = "Various",
        price = 12.00f
    ),
    Product(
        productName = "Chicken",
        imageId = R.drawable.chicken, // Replace with the actual image resource ID for chicken
        origin = "Various",
        price = 8.50f
    ),
    Product(
        productName = "Beef",
        imageId = R.drawable.beef, // Replace with the actual image resource ID for beef
        origin = "Various",
        price = 15.00f
    ),
    Product(
        productName = "Goat Meat",
        imageId = R.drawable.goat_meat, // Replace with the actual image resource ID for goat meat
        origin = "Various",
        price = 18.00f
    )
)
