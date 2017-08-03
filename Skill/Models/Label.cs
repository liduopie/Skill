using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Skill.Models
{
    public class Label
    {
        enum LabelCategory { technology, component };//标签类别，技术，组件

        [Key]
        public int Id { get; set; }
        [Display (Name ="标签名")]
        [Required(AllowEmptyStrings =false,ErrorMessage = "请输入标签名")]
        public string Name { get; set; }

        [Display(Name = "标签简介")]
        [DataType(DataType.Text)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "请输入标签简介")]
        public string Synopsis { get; set; }

        [Display(Name = "标签使用情况")]
        [DataType(DataType.Text)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "请输入标签使用情况")]
        public string Applicable { get; set; }

        [Display(Name = "标签类别")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "请输入标签类别")]
        public string Category { get; set; }

        public ICollection<PersonUseLabel> PersonUseLable { get; set; }
    }
}
