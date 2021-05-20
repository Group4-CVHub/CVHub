using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CVHub.Models
{
    public class CvTemp
    {
        public int CvId { get; set; }
        public string Title { get; set; }
        public string AboutMe { get; set; }
        public string Picture { get; set; }
        public int TemplateId { get; set; }
        public int UserId { get; set; }
        public Template Template { get; set; }
        public User User { get; set; }
        public Education Education { get; set; }
        public List<Education> Educations { get; set; }
        public List<Work> WorkPlaces { get; set; }

        public CvTemp()
        {
            Educations = new List<Education>();
            WorkPlaces = new List<Work>();
        }
    }
}
