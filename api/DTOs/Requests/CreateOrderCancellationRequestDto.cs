using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Annotations.Validation;

namespace api.DTOs.Requests
{
    public class CreateOrderCancellationRequestDto
    {
        [Required]
        [ValidObjectId]
        public string OrderId { get; set; } = "";
        [Required]
        [ValidObjectId]
        public string UserId { get; set; } = "";
        [Required]
        public string Reason { get; set; } = "";
    }
}