using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.Requests
{
	public class CreateReviewRequestDto
	{
		[Required]
		public string VendorId { get; set; } = string.Empty!;
		[Required]
		public string ProductId { get; set; } = string.Empty!;
		[Required]
		public string UserId { get; set; } = string.Empty!;
		[Required]
		[MinLength(1)]
		[MaxLength(500)]
		public string Message { get; set; } = null!;
		[Required]
		[Range(0, 5)]
		public double Rating { get; set; } = 0;
	}
}