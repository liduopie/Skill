using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Skill.Models
{
    public class Project
    {
        public int ID { get; set; }
        [Display(Name = "项目名称")]
        [Required(ErrorMessage = "必填")]
        [StringLength(100)]
        public String Name { get; set; }
        [Display(Name = "项目简介")]
        [Required(ErrorMessage = "必填")]
        [DataType(DataType.MultilineText)]
        [StringLength(1024)]
        public String Synopsis { get; set; }
        [Display(Name = "项目状态")]
        [Required(ErrorMessage = "必填")]
        [StringLength(100)]
        public String State { get; set; }

        /// <summary>
        /// 项目参与人员表导航属性
        /// </summary>
        public ICollection<ProjectUseLabel> ProjectUseLabel { get; set; }

        /// <summary>
        /// 项目参与人员表导航属性
        /// </summary>
        public ICollection<ProjectPartakePerson> ProjectPartakePerson { get; set; }
    }
}
