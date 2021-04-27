using CvHub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CVHub.Models
{
    public class Cv
    {
        [Key]
        public int CvId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        [ForeignKey("TemplateIDRef")]
        public int TemplateId { get; set; }
        [ForeignKey("UserIdRef")]
        public int UserId { get; set; }
        public Template Template { get; set; }
        public User User { get; set; }
    }
}