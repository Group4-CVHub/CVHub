using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CVHub.Models
{
    public class Education
    {
        [Key]
        public int EducationId { get; set; }
        [ForeignKey("CvIdRef")]
        public int CvId { get; set; }
        public string Name { get; set; }
        public DateTime EducationStart { get; set; }
        public DateTime EducationStop { get; set; }
        public string Degree { get; set; }
        public Cv Cv { get; set; }
    }
}
