using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.EntityFrameworkCore;

namespace api.Models
{
	[Collection("reviews")]
	public class Review : BaseModel
	{
		public ObjectId VendorId { get; set; } = ObjectId.Empty!;
		public ObjectId ProductId { get; set; } = ObjectId.Empty!;
		public ObjectId UserId { get; set; } = ObjectId.Empty!;
		public string Message { get; set; } = "";
		public double Rating { get; set; } = 0;
	}
}