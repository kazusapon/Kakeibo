using System;
using Xamarin.Forms;
using Eri.Models;

namespace Eri.ViewModels
{
    public class SpendDetailViewModel : BaseViewModel
    {
        public Item Item { get; set; }
        public SpendDetailViewModel(Item item = null)
        {
            //Title = item?.Text;
            Item = item;
        }
    }
}
