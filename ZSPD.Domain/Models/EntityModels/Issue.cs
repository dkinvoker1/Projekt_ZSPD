using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSPD.Domain.Models.EntityModels
{
    public class Issue
    {
        public int Id { get; set; }
        public int issueNumber { get; set; }
        public virtual SubjectGraph graph { get; set; }
        public Issue()
        {

        }

        public Issue(int number, SubjectGraph graph)
        {
            this.issueNumber = number;
            this.graph = graph;
        }
    }
}
