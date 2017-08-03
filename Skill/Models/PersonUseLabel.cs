using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skill.Models
{
    public class PersonUseLabel
    {
        public int ID { get; set; }

        public int PersonID { get; set; }

        public int LabelID { get; set; }

        public string UserTime { get; set; }

        public Person Person { get; set; }
        public Label Lable { get; set; }

    }
}
