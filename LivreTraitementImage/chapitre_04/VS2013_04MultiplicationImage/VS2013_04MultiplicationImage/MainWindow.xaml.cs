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

namespace VS2013_04MultiplicationImage
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //donnees
        private string RC = Environment.NewLine;

        private string doss_exe = Environment.CurrentDirectory;

        private bool v_fen_charge = false;

        //constructeur
        public MainWindow()
        {
            InitializeComponent();
        }

        //menu fichier -> quitter
        private void x_men_fichier_quitter_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //fenetre evenement Loaded
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            v_fen_charge = true;
            Uri uri_1 = new Uri(
                "pack://application:,,,/VS2013_04MultiplicationImage;component/collection_images/lune_8bit_388x418_96dpi.jpg",
                UriKind.Absolute);
            BitmapImage bti_1 = new BitmapImage();
            bti_1.BeginInit();
            bti_1.UriSource = uri_1;
            bti_1.EndInit();
            x_img_origine.Width = bti_1.PixelWidth;
            x_img_origine.Height = bti_1.PixelHeight;
            x_img_origine.Source = bti_1;
            MultiplicationImage(x_slider_1, x_img_mult_1);
            MultiplicationImage(x_slider_2, x_img_mult_2);
        }

        //
        private void x_slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (v_fen_charge == true)
            {
                Slider glissiere = (Slider) sender;
                if (glissiere.Name == "x_slider_1")
                {
                    MultiplicationImage(glissiere, x_img_mult_1);
                }
                if (glissiere.Name == "x_slider_2")
                {
                    MultiplicationImage(glissiere, x_img_mult_2);
                }
            }
        }

        //
        private void MultiplicationImage(Slider glissiere, Image controle_img)
        {
            //image 8 bit 256 gris
            Uri uri_1 = new Uri(
                "pack://application:,,,/VS2013_04MultiplicationImage;component/collection_images/lune_8bit_388x418_96dpi.jpg",
                UriKind.Absolute);
            BitmapImage bti_1 = new BitmapImage();
            bti_1.BeginInit();
            bti_1.UriSource = uri_1;
            bti_1.EndInit();
            WriteableBitmap wb_1 = new WriteableBitmap(bti_1);
            int largeur_numerisation = (wb_1.Format.BitsPerPixel / 8) * wb_1.PixelWidth;
            byte[] tab_pixel = new byte[largeur_numerisation * wb_1.PixelHeight];
            wb_1.CopyPixels(tab_pixel, largeur_numerisation, 0);
            int[,] tab_pixel_int_LH = ConvertirTableauPixelEnLH_8bit(tab_pixel, wb_1.PixelWidth, wb_1.PixelHeight);
            int[,] tab_pixel_int_LH_mult = new int[wb_1.PixelHeight, wb_1.PixelWidth];
            for (int lig = 0; lig < wb_1.PixelHeight; lig++)
            {
                for (int col = 0; col < wb_1.PixelWidth; col++)
                {
                    int niveau_gris_int = tab_pixel_int_LH[lig, col];
                    int niveau_gris_int_mult = (int) Math.Min((double) (niveau_gris_int * glissiere.Value), 255.0);
                    tab_pixel_int_LH_mult[lig, col] = niveau_gris_int_mult;
                }
            }
            byte[] tab_pixel_mult =
                ConvertirTableauPixelEnUnique_8bit(tab_pixel_int_LH_mult, wb_1.PixelWidth, wb_1.PixelHeight);
            BitmapSource bti_mult = BitmapSource.Create(wb_1.PixelWidth, wb_1.PixelHeight, 96.0, 96.0,
                PixelFormats.Gray8, null, tab_pixel_mult, largeur_numerisation);
            controle_img.Width = bti_mult.PixelWidth;
            controle_img.Height = bti_mult.PixelHeight;
            controle_img.Source = bti_mult;
        }

        //transposition tableau pixel dimension 1 vers 2 avec codage 32 bits
        private int[,] ConvertirTableauPixelEnLH_32bit(byte[] tab_pixel, int pixel_larg, int pixel_haut)
        {
            int[,] tab_LH = new int[pixel_haut, pixel_larg];
            int lig = 0;
            int col = 0;
            for (int xx = 0; xx < tab_pixel.Length; xx += 4)
            {
                byte comp_b = tab_pixel[xx];
                byte comp_g = tab_pixel[xx + 1];
                byte comp_r = tab_pixel[xx + 2];
                byte comp_a = tab_pixel[xx + 3];
                int couleur_int = comp_a << 24 | comp_r << 16 | comp_g << 8 | comp_b;
                tab_LH[lig, col] = couleur_int;
                col++;
                if (col == pixel_larg)
                {
                    col = 0;
                    lig++;
                }
            }
            return tab_LH;
        }

        //transposition tableau pixel dimension 2 vers 1 avec codage 32 bits
        private byte[] ConvertirTableauPixelEnUnique_32bit(int[,] tab_pixel_int_LH_modif, int pixel_largeur,
            int pixel_hauteur)
        {
            byte[] tab = new byte[pixel_largeur * 4 * pixel_hauteur];
            int cpt = 0;
            for (int lig = 0; lig < pixel_hauteur; lig++)
            {
                for (int col = 0; col < pixel_largeur; col++)
                {
                    int couleur_int = tab_pixel_int_LH_modif[lig, col]; //code en argb
                    Color couleur = new Color();
                    couleur.A = (byte) (couleur_int >> 24);
                    couleur.R = (byte) (couleur_int >> 16);
                    couleur.G = (byte) (couleur_int >> 8);
                    couleur.B = (byte) (couleur_int);
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

        //transposition tableau pixel dimension 1 vers 2 avec codage 8 bits
        private int[,] ConvertirTableauPixelEnLH_8bit(byte[] tab_pixel, int pixel_larg, int pixel_haut)
        {
            int[,] tab_LH = new int[pixel_haut, pixel_larg];
            int lig = 0;
            int col = 0;
            for (int xx = 0; xx < tab_pixel.Length; xx++)
            {
                byte comp = tab_pixel[xx];
                int couleur_int = (byte) comp;
                tab_LH[lig, col] = couleur_int;
                col++;
                if (col == pixel_larg)
                {
                    col = 0;
                    lig++;
                }
            }
            return tab_LH;
        }

        //transposition tableau pixel dimension 2 vers 1 avec codage 8 bits
        private byte[] ConvertirTableauPixelEnUnique_8bit(int[,] tab_pixel_int_LH_modif, int pixel_largeur,
            int pixel_hauteur)
        {
            byte[] tab = new byte[pixel_largeur * pixel_hauteur];
            int cpt = 0;
            for (int lig = 0; lig < pixel_hauteur; lig++)
            {
                for (int col = 0; col < pixel_largeur; col++)
                {
                    tab[cpt] = (byte) tab_pixel_int_LH_modif[lig, col];
                    cpt++;
                }
            }
            return tab;
        }
    } //end class
}