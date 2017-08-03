using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Skill.Models
{
    public class Person
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "姓名")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "请输入姓名")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "用户名长度必须在{2}和{1}位之间")]
        //[Remote("PersonCreate", "Person", ErrorMessage = "用户名已经存在")]
        public string Name { get; set; }

        [Display(Name = "年龄")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "请输入年龄")]
        [Range(10, 120, ErrorMessage = "您输入的年龄不符合规范，年龄应该在{1}-{2}之间")]
        public int Age { get; set; }

        [Display(Name = "出生日期")]
        [DataType(DataType.DateTime)]
        //[DisplayFormat(ApplyFormatInEditMode=true,DataFormatString="yyyy/MM/dd")]  
        [Required(AllowEmptyStrings = true, ErrorMessage = "请输入出生日期")]
        public DateTime Birthday { get; set; }

        [Display(Name = "爱好")]
        [DataType(DataType.Text)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "请输入爱好")]
        public string Hobby { get; set; }
        [Display(Name = "手机号码")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"(^1[3,4,5,7,8]\d{9}$)", ErrorMessage = "格式不正确")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "请输入手机号")]
        public string Phone { get; set; }

        [Display(Name = "住址")]
        [DataType(DataType.Text)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "请输入住址")]
        public string Address { get; set; }

        public ICollection<PersonUseLabel> PersonUserLable { get; set; }
    }
}
