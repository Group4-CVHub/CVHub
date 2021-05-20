using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CVHub.Models
{
    public class Work
    {
        [Key]
        public int WorkId { get; set; }
        [ForeignKey("CvIdRef")]
        public int CvId { get; set; }
        public string Name { get; set; }
        public DateTime WorkStart { get; set; }
        public DateTime WorkStop { get; set; }
        public string Description { get; set; }
        public Cv Cv { get; set; }
    }
}
