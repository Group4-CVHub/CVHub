using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CVHub.Models
{
    public class Cv
    {
        [Key]
        public int CvId { get; set; }
        public string Title { get; set; }
        public string AboutMe { get; set; }
        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile Picture { get; set; }
        [ForeignKey("TemplateIDRef")]
        public int TemplateId { get; set; }
        [ForeignKey("UserIdRef")]
        public int UserId { get; set; }
        public Template Template { get; set; }
        public User User { get; set; }
        public List<Education> Educations { get; set; }
        public List<Work> WorkPlaces { get; set; }

  
    }
}