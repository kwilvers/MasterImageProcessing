using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;

namespace ImageManipulation
{
    public static class BitmapHelper
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        private static Bitmap BitmapImage2Bitmap(this BitmapImage bitmapImage)
        {
            // BitmapImage bitmapImage = new BitmapImage(new Uri("../Images/test.png", UriKind.Relative));

            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }

        private static BitmapImage Bitmap2BitmapImage(this Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();
            BitmapImage retval;

            try
            {
                retval = (BitmapImage)Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(hBitmap);
            }

            return retval;
        }

        public static Color[][] BitMap2ColorMatrix(this Bitmap bmp)
        {
            int height = bmp.Height;
            int width = bmp.Width;

            //row and column
            Color[][] colorMatrix = new Color[width][];
            for (int i = 0; i < width; i++)
            {
                colorMatrix[i] = new Color[height];
                for (int j = 0; j < height; j++)
                {
                    colorMatrix[i][j] = bmp.GetPixel(i, j);
                }
            }
            return colorMatrix;
        }

        public static Bitmap BitMap2ColorMatrix(this Color[][] colorMatrix)
        {
            int height = colorMatrix[0].Length;
            int width = colorMatrix.Length;
            Bitmap bmp = new Bitmap(width, height);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    bmp.SetPixel(i, j, colorMatrix[i][j]);
                }
            }
            return bmp;
        }

        public static int[,] FourByteColor2Int(this byte[] pixelsColors, int width, int height)
        {
            int[,] tabLh = new int[height, width];
            int lig = 0;
            int col = 0;
            for (int xx = 0; xx < pixelsColors.Length; xx += 4)
            {
                byte compB = pixelsColors[xx];
                byte compG = pixelsColors[xx + 1];
                byte compR = pixelsColors[xx + 2];
                byte compA = pixelsColors[xx + 3];
                int couleurInt = compA << 24 | compR << 16 | compG << 8 | compB;
                tabLh[lig, col] = couleurInt;
                col++;
                if (col == width)
                {
                    col = 0;
                    lig++;
                }
            }
            return tabLh;
        }

        public static byte[] Int2FourByteColor(this int[,] pixels, int width, int height)
        {
            byte[] tab = new byte[width * 4 * height];
            int cpt = 0;
            for (int lig = 0; lig < height; lig++)
            {
                for (int col = 0; col < width; col++)
                {
                    int couleurInt = pixels[lig, col]; //code en argb
                    Color couleur = new Color();
                    Color.FromArgb((byte)(couleurInt >> 24),
                                    (byte)(couleurInt >> 16),
                                    (byte)(couleurInt >> 8),
                                    (byte)(couleurInt));
                    //code bgra pour tableau unique
                    tab[cpt] = couleur.B;
                    cpt++;
                    tab[cpt] = couleur.G;
                    cpt++;
                    tab[cpt] = couleur.R;
                    cpt++;
                    tab[cpt] = couleur.A;
                    cpt++;
                }
            }
            return tab;
        }

        public static Pixel[][] BitmapImage2PixelMatrix(this BitmapImage bmp)
        {
            double height = bmp.Height;
            double width = bmp.Width;

            WriteableBitmap wb = new WriteableBitmap(bmp);
            int largeurNumerisation = (wb.Format.BitsPerPixel / 8) * wb.PixelWidth;
            byte[] tabPixel = new byte[largeurNumerisation * wb.PixelHeight]; //codage bgra
            wb.CopyPixels(tabPixel, largeurNumerisation, 0);


            //row and column
            Pixel[][] colorMatrix = new Pixel[(int) height][];
            for (int i = 0; i < (int)height; i++)
            {
                colorMatrix[i] = new Pixel[(int)width];
            }

            int col = 0, row = 0;
            for (int xx = 0; row < (int)height; xx += 4)
            {
                colorMatrix[row][col] = new Pixel(tabPixel[xx + 3], tabPixel[xx + 2], tabPixel[xx + 1], tabPixel[xx]);

                col++;
                if (col == (int)width)
                {
                    row++;
                    col = 0;
                }
            }

            return colorMatrix;
        }

        public static BitmapImage PixelMatrix2BitmapImage(this Pixel[][]pixels)
        {
            double height = pixels.Length;
            double width = pixels[0].Length;

            byte[] tabPixel = new byte[(int) (width * 4 * height)];

            int i = 0;

            foreach (var rowPixel in pixels )
            {
                foreach (var pixel in rowPixel)
                {
                    tabPixel[i] = pixel.B;
                    tabPixel[i++] = pixel.G;
                    tabPixel[i++] = pixel.R;
                    tabPixel[i++] = pixel.A;
                }
            }

            BitmapSource btiModif = BitmapSource.Create((int) width, (int) height, 96.0, 96.0,
                PixelFormats.Bgra32, null, tabPixel, (int) width);
    
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            //bmp.StreamSource = btiModif.;
            
            /*WriteableBitmap wb = new WriteableBitmap(bmp);
            int largeurNumerisation = (wb.Format.BitsPerPixel / 8) * wb.PixelWidth;
            byte[] tabPixel = new byte[largeurNumerisation * wb.PixelHeight]; //codage bgra
            wb.CopyPixels(tabPixel, largeurNumerisation, 0);


            //row and column
            Pixel[][] colorMatrix = new Pixel[(int)width][];
            for (int i = 0; i < width; i++)
            {
                colorMatrix[i] = new Pixel[(int)height];
            }

            int col = 0, row = 0;
            for (int xx = 0; xx < tabPixel.Length; xx += 4)
            {
                colorMatrix[row][col].B = tabPixel[xx];
                colorMatrix[row][col].G = tabPixel[xx + 1];
                colorMatrix[row][col].R = tabPixel[xx + 2];
                colorMatrix[row][col].A = tabPixel[xx + 3];

                col++;
                if (col == width)
                {
                    row++;
                    col = 0;
                }
            }*/


            return btiModif as BitmapImage; ;
        }
    }
}
