using SearchExplorer.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchExplorer.Core.Entities
{
    public class ApiResponse<T>
    {

        [Required]
        public StatusCode StatusCode { get; set; }
        [Required]
        [StringLength(500)]
        public string Message { get; set; }
        public T Data { get; set; }
        public ApiResponse(StatusCode status, string message, T data = default)
        {
            StatusCode = status;
            Message = message;
            Data = data;
        }

        public ApiResponse() { }
    }


}
