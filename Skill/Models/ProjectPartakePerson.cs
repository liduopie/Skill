using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Skill.Models
{
    public class ProjectPartakePerson
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        [Key]
        public int ProjectID { get; set; }
        /// <summary>
        /// 人员ID
        /// </summary>
        [Key]
        public int PersonID { get; set; }

        /// <summary>
        /// 人员的引用属性
        /// </summary>
        public Person Person { get; set; }
        /// <summary>
        /// 标签的引用属性
        /// </summary>
        public Project Project { get; set; }
    }
}
