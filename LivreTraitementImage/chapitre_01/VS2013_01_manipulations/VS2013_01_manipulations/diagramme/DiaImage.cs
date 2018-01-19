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
using System.Net;
using System.IO;
using System.Windows.Resources;
using System.IO.Compression;

namespace VS2013_01_manipulations.diagramme {
  class DiaImage {
    BitmapImage bti;
    BitmapFrame bt2;
    WriteableBitmap w1;
    RenderTargetBitmap rt;
    CachedBitmap cc1;
    ColorConvertedBitmap cc3;
    CroppedBitmap cc2;
    FormatConvertedBitmap ff1;
    TransformedBitmap tb1;
    WebClient wc1;
  }
}
