using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Prism.Mvvm;

namespace WpfTestLibrary
{
    public class ImageItem : BindableBase
    {
        private string description;
        private BitmapImage image;

        public string Description
        {
            get => description;
            set
            {
                description = value; 
                RaisePropertyChanged();
            }
        }

        public BitmapImage Image
        {
            get => image;
            set
            {
                image = value; 
                RaisePropertyChanged();
            }
        }
    }
}
