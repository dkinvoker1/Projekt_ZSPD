using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSPD.Domain.Models.EntityModels
{
    public class SubjectGraph
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Exercises { get; set; }
        public string PreviousConcepts { get; set; }
        public string NextConcepts { get; set; }
    }
}
