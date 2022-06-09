using System.ComponentModel.DataAnnotations;

namespace Hastnama.Solico.Common.Enums
{
    public enum OrderStatus
    {
        
        [Display(Name = "لغو شده", ShortName = "Canceled")]
        Canceled = 1,

        [Display(Name = "در انتظار تایید", ShortName = "Pending")]
        Pending = 2,

        [Display(Name = "تایید شده", ShortName = "Approved")]
        Approved = 3,

        [Display(Name = "در حال ارسال", ShortName = "Posted")]
        Posted = 4,

        [Display(Name = "تحویل داده شده", ShortName = "Delivered")]
        Delivered = 5,

        [Display(Name = "در سبد خرید", ShortName = "InBasket")]
        InBasket = 6,

        [Display(Name = "آماده پرداخت", ShortName = "ReadyForPay")]
        ReadyForPay = 7
    }
}