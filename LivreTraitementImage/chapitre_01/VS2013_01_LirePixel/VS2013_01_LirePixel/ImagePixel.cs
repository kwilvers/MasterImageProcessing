using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VS2013_01_LirePixel
{
    public class ImagePixel
    {
        //donnees
        private WriteableBitmap v_wb = null;

        private ImageType v_image_type = ImageType.couleur_argb;

        public enum ImageType
        {
            couleur_argb,
            niveaux_gris_256
        };

        public ImageType P_ImageType
        {
            get { return v_image_type; }
        }

        public ImagePixel(BitmapImage bti, ImageType type_image)
        {
            if (type_image == ImageType.couleur_argb)
            {
                v_wb = new WriteableBitmap(bti.PixelWidth, bti.PixelHeight, 96, 96, PixelFormats.Bgra32, null);
            }
            if (type_image == ImageType.niveaux_gris_256)
            {
                v_wb = new WriteableBitmap(bti.PixelWidth, bti.PixelHeight, 96, 96, PixelFormats.Gray8, null);
            }
        }
    }
}