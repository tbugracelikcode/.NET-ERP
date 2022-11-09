using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TsiErp.Entities.Enums
{
    public enum CalendarDateTypeEnum
    {
        [Display(Name = "Çalışma Var")]
        CalismaVar = 1,
        [Display(Name = "Çalışma Yok")]
        CalismaYok = 2,
        [Display(Name = "Resmi Tatil")]
        ResmiTatil = 3,
        [Display(Name = "Tatil")]
        Tatil = 4,
        [Display(Name = "Yarım Gün Çalışma")]
        YarimGun = 5,
        [Display(Name = "Yükleme Günü")]
        YuklemeGunu = 6,
        [Display(Name = "Bakım")]
        Bakim = 7
    }
}
