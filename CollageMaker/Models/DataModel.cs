using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CollageMaker.Models
{
    public class DataModel
    {
        [Display(Name="Twitter Name")]
        [Required(ErrorMessage ="Twitter name is required field!")]
        public string UserName { get; set; }
        [Display(Name ="Number of Columns")]
        [Required(ErrorMessage ="Number of columns is required field!")]
        [Range(1,150,ErrorMessage ="Invalid value")]
        public int NumberOfColumns { get; set; }
        [Display(Name = "Number of Rows")]
        [Required(ErrorMessage = "Number of rows is required field!")]
        [Range(1, 150, ErrorMessage = "Invalid value")]
        public int NumberOfRows { get; set; }
    }
}