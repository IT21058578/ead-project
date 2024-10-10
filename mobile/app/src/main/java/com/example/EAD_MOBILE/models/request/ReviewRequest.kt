import com.google.gson.annotations.Expose
import com.google.gson.annotations.SerializedName

data class ReviewRequest(
    @SerializedName("vendorId")
    @Expose
    var vendorId: String? = null,

    @SerializedName("productId")
    @Expose
    var productId: String? = null,

    @SerializedName("message")
    @Expose
    var message: String? = null,

    @SerializedName("rating")
    @Expose
    var rating: Int? = null,

    @SerializedName("userId")
    @Expose
    var userId: String? = null
)
