using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSPD.Domain.Models.EntityModels
{
    public class Excercise
    {
        public int Id { get; set; }
        public int excerciseNumber { get; set; }
        
        public Excercise()
        {

        }

        public Excercise(int number)
        {
            this.excerciseNumber = number;
        }
    }
}
