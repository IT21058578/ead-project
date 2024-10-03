using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Annotations.Validation;

namespace api.DTOs.Requests
{
	public class CreateReviewRequestDto
	{
		[Required]
		[ValidObjectId]
		public string VendorId { get; set; } = string.Empty!;
		[Required]
		[ValidObjectId]
		public string ProductId { get; set; } = string.Empty!;
		[Required]
		[ValidObjectId]
		public string UserId { get; set; } = string.Empty!;
		[Required]
		[MinLength(1)]
		[MaxLength(500)]
		public string Message { get; set; } = "";
		[Required]
		[Range(0, 5)]
		public double Rating { get; set; } = 0;
	}
}