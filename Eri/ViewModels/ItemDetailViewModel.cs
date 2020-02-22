using System;
using Xamarin.Forms;
using Eri.Models;

namespace Eri.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Item Item { get; set; }
        public ItemDetailViewModel(Item item = null)
        {
            //Title = item?.Text;
            Item = item;
        }
    }
}
