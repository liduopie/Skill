using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Skill.Models
{
    public class PersonUseLabel
    {
        public int ID { get; set; }

        public int PersonID { get; set; }

        public int LabelID { get; set; }

        [Display(Name = "使用日期")]
        [DataType(DataType.Date)]
        [Required(AllowEmptyStrings = true, ErrorMessage = "请输入使用日期")]
        public DateTime UseTime { get; set; }
        [Display(Name = "使用次数")]
        public int UseCount { get; set; }

        public Person Person { get; set; }
        public Label Label { get; set; }

    }
}
