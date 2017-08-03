using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Skill.Models
{
    public class PersonMasterLabel
    {
        /// <summary>
        /// 人员ID
        /// </summary>
        [Key]
        public int PersonID { get; set; }
        /// <summary>
        /// 标签ID
        /// </summary>
        [Key]
        public int LabelID { get; set; }

        /// <summary>
        /// 人员的引用属性
        /// </summary>
        public Person Person { get; set; }
        /// <summary>
        /// 标签的引用属性
        /// </summary>
        public Label Label { get; set; }
    }
}
