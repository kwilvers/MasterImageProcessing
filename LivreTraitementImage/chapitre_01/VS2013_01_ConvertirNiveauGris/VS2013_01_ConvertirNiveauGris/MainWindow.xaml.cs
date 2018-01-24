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

namespace VS2013_01_ConvertirNiveauGris
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
            BitmapImage bti = new BitmapImage();
            bti.BeginInit();
            bti.UriSource =
                new Uri(
                    "pack://application:,,,/VS2013_01_ConvertirNiveauGris;component/collection_images/aff_rabbi_jacob_800x1022_96dpi.jpg",
                    UriKind.Absolute);
            bti.EndInit();
            x_img.Width = bti.PixelWidth;
            x_img.Height = bti.PixelHeight;
            x_img.Source = bti;
        }

        //selection methode conversion
        private void x_cbx_select_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (v_fen_charge == true)
            {
                if (x_cbx_select.SelectedIndex == 0)
                {
                    BitmapImage bti = new BitmapImage();
                    bti.BeginInit();
                    bti.UriSource =
                        new Uri(
                            "pack://application:,,,/VS2013_01_ConvertirNiveauGris;component/contenu/image/fond_damier.png",
                            UriKind.Absolute);
                    bti.EndInit();
                    x_img_conversion.Width = 770;
                    x_img_conversion.Height = 401;
                    x_img_conversion.Source = bti;
                }
                if (x_cbx_select.SelectedIndex == 1)
                {
                    Convertir256NiveauxGrisMoyenne();
                }
                if (x_cbx_select.SelectedIndex == 2)
                {
                    Convertir256NiveauxGrisRec709();
                }
            }
        }

        //convertir selon moyenne composante
        private void Convertir256NiveauxGrisMoyenne()
        {
            BitmapImage bti = new BitmapImage();
            bti.BeginInit();
            bti.UriSource =
                new Uri(
                    "pack://application:,,,/VS2013_01_ConvertirNiveauGris;component/collection_images/aff_rabbi_jacob_800x1022_96dpi.jpg",
                    UriKind.Absolute);
            bti.EndInit();
            WriteableBitmap wb = new WriteableBitmap(bti);
            int largeur_numerisation = (wb.Format.BitsPerPixel / 8) * wb.PixelWidth;
            byte[] tab_pixel = new byte[largeur_numerisation * wb.PixelHeight]; //codage bgra
            wb.CopyPixels(tab_pixel, largeur_numerisation, 0);
            int[,] tab_pixel_int_LH = ConvertirTableauPixelEnLH_32bit(tab_pixel, wb.PixelWidth, wb.PixelHeight);
            int[,] tab_pixel_int_LH_modif = new int[wb.PixelHeight, wb.PixelWidth];
            for (int lig = 0; lig < wb.PixelHeight; lig++)
            {
                for (int col = 0; col < wb.PixelWidth; col++)
                {
                    int couleur_int = tab_pixel_int_LH[lig, col];
                    Color couleur = new Color();
                    couleur.A = (byte) (couleur_int >> 24);
                    couleur.R = (byte) (couleur_int >> 16);
                    couleur.G = (byte) (couleur_int >> 8);
                    couleur.B = (byte) (couleur_int);
                    double moyenne = (couleur.R + couleur.G + couleur.B) / 3;
                    couleur.R = (byte) moyenne;
                    couleur.G = (byte) moyenne;
                    couleur.B = (byte) moyenne;
                    int couleur_int_modif = couleur.A << 24 | couleur.R << 16 | couleur.G << 8 | couleur.B;
                    tab_pixel_int_LH_modif[lig, col] = couleur_int_modif;
                }
            }
            byte[] tab_pixel_modif =
                ConvertirTableauPixelEnUnique_32bit(tab_pixel_int_LH_modif, wb.PixelWidth, wb.PixelHeight);
            BitmapSource bti_modif = BitmapSource.Create(wb.PixelWidth, wb.PixelHeight, 96.0, 96.0,
                PixelFormats.Bgra32, null, tab_pixel_modif, largeur_numerisation);
            x_img_conversion.Width = bti_modif.PixelWidth;
            x_img_conversion.Height = bti_modif.PixelHeight;
            x_img_conversion.Source = bti_modif;
        }

        //methode recommandation 709
        private void Convertir256NiveauxGrisRec709()
        {
            BitmapImage bti = new BitmapImage();
            bti.BeginInit();
            bti.UriSource =
                new Uri(
                    "pack://application:,,,/VS2013_01_ConvertirNiveauGris;component/collection_images/aff_rabbi_jacob_800x1022_96dpi.jpg",
                    UriKind.Absolute);
            bti.EndInit();
            WriteableBitmap wb = new WriteableBitmap(bti);
            int largeur_numerisation = (wb.Format.BitsPerPixel / 8) * wb.PixelWidth;
            byte[] tab_pixel = new byte[largeur_numerisation * wb.PixelHeight]; //codage bgra
            wb.CopyPixels(tab_pixel, largeur_numerisation, 0);
            int[,] tab_pixel_int_LH = ConvertirTableauPixelEnLH_32bit(tab_pixel, wb.PixelWidth, wb.PixelHeight);
            int[,] tab_pixel_int_LH_modif = new int[wb.PixelHeight, wb.PixelWidth];
            for (int lig = 0; lig < wb.PixelHeight; lig++)
            {
                for (int col = 0; col < wb.PixelWidth; col++)
                {
                    int couleur_int = tab_pixel_int_LH[lig, col];
                    Color couleur = new Color();
                    couleur.A = (byte) (couleur_int >> 24);
                    couleur.R = (byte) (couleur_int >> 16);
                    couleur.G = (byte) (couleur_int >> 8);
                    couleur.B = (byte) (couleur_int);
                    double rec_709 = 0.2125d * couleur.R + 0.7154d * couleur.G + 0.0721d * couleur.G;
                    couleur.R = (byte) rec_709;
                    couleur.G = (byte) rec_709;
                    couleur.B = (byte) rec_709;
                    int couleur_int_modif = couleur.A << 24 | couleur.R << 16 | couleur.G << 8 | couleur.B;
                    tab_pixel_int_LH_modif[lig, col] = couleur_int_modif;
                }
            }
            byte[] tab_pixel_modif =
                ConvertirTableauPixelEnUnique_32bit(tab_pixel_int_LH_modif, wb.PixelWidth, wb.PixelHeight);
            BitmapSource bti_modif = BitmapSource.Create(wb.PixelWidth, wb.PixelHeight, 96.0, 96.0,
                PixelFormats.Bgra32, null, tab_pixel_modif, largeur_numerisation);
            x_img_conversion.Width = bti_modif.PixelWidth;
            x_img_conversion.Height = bti_modif.PixelHeight;
            x_img_conversion.Source = bti_modif;
        }

        //
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

        //
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
    } //end class
}