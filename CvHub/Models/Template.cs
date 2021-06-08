using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CVHub.Models
{
    public class Template
    {
        [Key]
        public int TemplateId { get; set; }
        public string Description { get; set; }
        public ICollection<Cv> Cvs { get; set; }
    }
}
