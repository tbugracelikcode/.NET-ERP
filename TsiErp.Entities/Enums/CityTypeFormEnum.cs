using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Enums
{
    public enum CityTypeFormEnum
    {
        [Display(Name = "BigCityEnum")]
        Büyükşehir = 1,
        [Display(Name = "SmallTownEnum")]
        Küçükşehir = 2,
        [Display(Name = "MetropolitanCityEnum")]
        Metropol = 3,
    }
}

