using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Skill.Models
{
    public class ProjectUseLabel
    {
        [Key]
        public int ProjectID { get; set; }
        [Key]
        public int LabelID { get; set; }

        public Project Project { get; set; }
        public Label Label { get; set; }
    }
}
