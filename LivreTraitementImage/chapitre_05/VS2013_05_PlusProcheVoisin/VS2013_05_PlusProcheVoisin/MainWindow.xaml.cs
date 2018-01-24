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

namespace VS2013_05_PlusProcheVoisin
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
        private double v_facteur_zoom = 100;

        private double v_facteur_zoom_agrandi = 100;

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
            Uri uri = new Uri(
                "pack://application:,,,/VS2013_05_PlusProcheVoisin;component/collection_images/louis_de_funes_8bit_800x564_96dpi.jpg",
                UriKind.Absolute);
            BitmapImage bti = new BitmapImage();
            bti.BeginInit();
            bti.UriSource = uri;
            bti.EndInit();
            x_img.Width = bti.PixelWidth;
            x_img.Height = bti.PixelHeight;
            x_img.Source = bti;
            InterpolationLePlusProcheVoisin(uri);
        }

        //interpolation du plus proche voisin
        private void InterpolationLePlusProcheVoisin(Uri uri)
        {
            //image 1 avec 8 bits 256 gris
            BitmapImage bti = new BitmapImage();
            bti.BeginInit();
            bti.UriSource = uri;
            bti.EndInit();
            WriteableBitmap wb = new WriteableBitmap(bti);
            int largeur_numerisation = (wb.Format.BitsPerPixel / 8) * wb.PixelWidth;
            byte[] tab_pixel = new byte[largeur_numerisation * wb.PixelHeight];
            wb.CopyPixels(tab_pixel, largeur_numerisation, 0);
            int[,] tab_pixel_int_LH = ConvertirTableauPixelEnLH_8bit(tab_pixel, wb.PixelWidth, wb.PixelHeight);
            //initialise avec du blanc
            int[,] tab_pixel_int_LH_agrandi = new int[2 * wb.PixelHeight, 2 * wb.PixelWidth];
            for (int lig = 0; lig < 2 * wb.PixelHeight; lig++)
            {
                for (int col = 0; col < 2 * wb.PixelWidth; col++)
                {
                    tab_pixel_int_LH_agrandi[lig, col] = 255;
                }
            }
            //on copie les pixels d'origine dans leur emplacement dans l'image agrandie
            for (int lig = 0; lig < wb.PixelHeight; lig++)
            {
                for (int col = 0; col < wb.PixelWidth; col++)
                {
                    tab_pixel_int_LH_agrandi[2 * lig, 2 * col] = tab_pixel_int_LH[lig, col];
                }
            }
            //on recopie le pixel a droite sur chaque ligne
            for (int lig = 0; lig < 2 * wb.PixelHeight; lig++)
            {
                for (int col = 0; col < 2 * wb.PixelWidth; col += 2)
                {
                    tab_pixel_int_LH_agrandi[lig, col + 1] = tab_pixel_int_LH_agrandi[lig, col];
                }
            }
            //on recopie le pixel a bas sur chaque colonne
            for (int col = 0; col < 2 * wb.PixelWidth; col++)
            {
                for (int lig = 0; lig < 2 * wb.PixelHeight; lig += 2)
                {
                    tab_pixel_int_LH_agrandi[lig + 1, col] = tab_pixel_int_LH_agrandi[lig, col];
                }
            }
            //on genere l'image resultante
            byte[] tab_pixel_agrandi =
                ConvertirTableauPixelEnUnique_8bit(tab_pixel_int_LH_agrandi, 2 * wb.PixelWidth, 2 * wb.PixelHeight);
            BitmapSource bti_agrandi = BitmapSource.Create(2 * wb.PixelWidth, 2 * wb.PixelHeight, 96.0, 96.0,
                PixelFormats.Gray8, null, tab_pixel_agrandi, 2 * largeur_numerisation);
            x_img_agrandie.Width = bti_agrandi.PixelWidth;
            x_img_agrandie.Height = bti_agrandi.PixelHeight;
            x_img_agrandie.Source = bti_agrandi;
        }

        //
        private void btn_zoom_plus_Click(object sender, RoutedEventArgs e)
        {
            x_img.Stretch = Stretch.Fill;
            x_img.Width = 2 * x_img.ActualWidth;
            x_img.Height = 2 * x_img.ActualHeight;
            v_facteur_zoom *= 2;
            x_tbl_zoom_ini.Text = v_facteur_zoom.ToString() + " %";
        }

        //selecteur zoom sur l'image initiale
        private void btn_zoom_moins_Click(object sender, RoutedEventArgs e)
        {
            x_img.Stretch = Stretch.Fill;
            x_img.Width = x_img.ActualWidth / 2;
            x_img.Height = x_img.ActualHeight / 2;
            v_facteur_zoom /= 2;
            x_tbl_zoom_ini.Text = v_facteur_zoom.ToString() + " %";
        }

        private void btn_zoom_plus_agrandi_Click(object sender, RoutedEventArgs e)
        {
            x_img_agrandie.Stretch = Stretch.Fill;
            x_img_agrandie.Width = 2 * x_img_agrandie.ActualWidth;
            x_img_agrandie.Height = 2 * x_img_agrandie.ActualHeight;
            v_facteur_zoom_agrandi *= 2;
            x_tbl_zoom_agrandi.Text = v_facteur_zoom_agrandi.ToString() + " %";
        }

        private void btn_zoom_moins_agrandi_Click(object sender, RoutedEventArgs e)
        {
            x_img_agrandie.Stretch = Stretch.Fill;
            x_img_agrandie.Width = x_img_agrandie.ActualWidth / 2;
            x_img_agrandie.Height = x_img_agrandie.ActualHeight / 2;
            v_facteur_zoom_agrandi /= 2;
            x_tbl_zoom_agrandi.Text = v_facteur_zoom_agrandi.ToString() + " %";
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

        ////transposition tableau pixel dimension 1 vers 2 avec codage 8 bits
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