using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ImageManipulation;
using Prism.Commands;
using Prism.Mvvm;

namespace WpfTestLibrary
{
    public class MainViewModel : BindableBase
    {
        public ObservableCollection<ImageItem> Items { get; set; }
        public DelegateCommand IsolateCommand { get; set; }
        public DelegateCommand GrayCommand { get; set; }
        public BitmapImage Original { get; set; }
        public ImageItem OriginalItem { get; set; }

        public MainViewModel()
        {
            Items = new ObservableCollection<ImageItem>();
            IsolateCommand = new DelegateCommand(DoIsolation);
            GrayCommand = new DelegateCommand(DoGray);
            Original = new BitmapImage(new Uri("./echantillon.png", UriKind.Relative));
            OriginalItem = new ImageItem
            {
                Image = Original,
                Description = "Image originale"
            };
            Items.Add(OriginalItem);
        }

        private void DoGray()
        {
            throw new NotImplementedException();
        }

        private void DoIsolation()
        {
            var v = Original.BitmapImage2PixelMatrix();
            for (var index = 0; index < v.Length; index++)
            {
                var rowPixel = v[index];
                for (var i = 0; i < rowPixel.Length; i++)
                {
                    var pixel = rowPixel[i];
                    rowPixel[i] = PixelTransform.IsolateRed(pixel);
                }
            }
            var w = v.PixelMatrix2BitmapImage();

            Items.Add(new ImageItem(){Image = w, Description = "Red"});

            GC.Collect();
        }
    }
}
