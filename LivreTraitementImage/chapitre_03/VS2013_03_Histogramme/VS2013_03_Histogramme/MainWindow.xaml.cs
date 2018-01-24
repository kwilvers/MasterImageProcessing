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

namespace VS2013_03_Histogramme
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
        }

        //vider les controles
        private void ViderControleImage(Uri uri, Image controle_image, int largeur, int hauteur)
        {
            BitmapImage bti = new BitmapImage();
            bti.BeginInit();
            bti.UriSource = uri;
            bti.EndInit();
            controle_image.Width = largeur;
            controle_image.Height = hauteur;
            controle_image.Source = bti;
        }

        //selection methode conversion
        private void x_cbx_select_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (v_fen_charge == true)
            {
                Uri uri_vide =
                    new Uri("pack://application:,,,/VS2013_03_Histogramme;component/contenu/image/fond_damier.png",
                        UriKind.Absolute);
                ViderControleImage(uri_vide, x_img, 770, 399);
                x_stack_histo.Children.Clear();
                if (x_cbx_select.SelectedIndex == 1)
                {
                    Uri uri = new Uri(
                        "pack://application:,,,/VS2013_03_Histogramme;component/collection_images/le_petit_baigneur_8bit_600x817_96dpi.jpg",
                        UriKind.Absolute);
                    BitmapImage bti = new BitmapImage();
                    bti.BeginInit();
                    bti.UriSource = uri;
                    bti.EndInit();
                    x_img.Width = bti.PixelWidth;
                    x_img.Height = bti.PixelHeight;
                    x_img.Source = bti;
                    //image 256 gris 8 bits
                    WriteableBitmap wb = new WriteableBitmap(bti);
                    int largeur_numerisation = (wb.Format.BitsPerPixel / 8) * wb.PixelWidth;
                    byte[] tab_pixel = new byte[largeur_numerisation * wb.PixelHeight];
                    wb.CopyPixels(tab_pixel, largeur_numerisation, 0);
                    int[,] tab_pixel_int_LH = ConvertirTableauPixelEnLH_8bit(tab_pixel, wb.PixelWidth, wb.PixelHeight);
                    int[] tab_repartition_gris = new int[256];
                    for (int lig = 0; lig < 256; lig++)
                    {
                        tab_repartition_gris[lig] = 0;
                    }
                    for (int lig = 0; lig < wb.PixelHeight; lig++)
                    {
                        for (int col = 0; col < wb.PixelWidth; col++)
                        {
                            int niveau_gris_int = tab_pixel_int_LH[lig, col];
                            tab_repartition_gris[niveau_gris_int] += 1;
                        }
                    }
                    Histo visuel_histo = new Histo();
                    visuel_histo.Titre = "Histogramme pour les niveaux de gris";
                    visuel_histo.Composante = Histo.TypeComposante.niveau_de_gris;
                    visuel_histo.RepartitionPixel = tab_repartition_gris;
                    x_stack_histo.Children.Add(visuel_histo);
                }
                if (x_cbx_select.SelectedIndex == 2)
                {
                    Uri uri = new Uri(
                        "pack://application:,,,/VS2013_03_Histogramme;component/collection_images/le_petit_baigneur_32bit_600x817_96dpi.jpg",
                        UriKind.Absolute);
                    BitmapImage bti = new BitmapImage();
                    bti.BeginInit();
                    bti.UriSource = uri;
                    bti.EndInit();
                    x_img.Width = bti.PixelWidth;
                    x_img.Height = bti.PixelHeight;
                    x_img.Source = bti;
                    //image couleur 32 bits
                    WriteableBitmap wb = new WriteableBitmap(bti);
                    int largeur_numerisation = (wb.Format.BitsPerPixel / 8) * wb.PixelWidth;
                    byte[] tab_pixel = new byte[largeur_numerisation * wb.PixelHeight];
                    wb.CopyPixels(tab_pixel, largeur_numerisation, 0);
                    int[,] tab_pixel_int_LH = ConvertirTableauPixelEnLH_32bit(tab_pixel, wb.PixelWidth, wb.PixelHeight);
                    int[] tab_repartition_r = new int[256];
                    int[] tab_repartition_g = new int[256];
                    int[] tab_repartition_b = new int[256];
                    for (int lig = 0; lig < 256; lig++)
                    {
                        tab_repartition_r[lig] = 0;
                        tab_repartition_g[lig] = 0;
                        tab_repartition_b[lig] = 0;
                    }
                    for (int lig = 0; lig < wb.PixelHeight; lig++)
                    {
                        for (int col = 0; col < wb.PixelWidth; col++)
                        {
                            int couleur_int = tab_pixel_int_LH[lig, col];
                            Color couleur = new Color();
                            couleur.R = (byte) (couleur_int >> 16);
                            couleur.G = (byte) (couleur_int >> 8);
                            couleur.B = (byte) (couleur_int);
                            tab_repartition_r[couleur.R] += 1;
                            tab_repartition_g[couleur.G] += 1;
                            tab_repartition_b[couleur.B] += 1;
                        }
                    }
                    Histo visuel_histo_R = new Histo();
                    visuel_histo_R.Titre = "Histogramme pour la composante rouge";
                    visuel_histo_R.Composante = Histo.TypeComposante.composante_R;
                    visuel_histo_R.RepartitionPixel = tab_repartition_r;
                    visuel_histo_R.Margin = new Thickness(0, 0, 0, 2);
                    x_stack_histo.Children.Add(visuel_histo_R);
                    Histo visuel_histo_G = new Histo();
                    visuel_histo_G.Titre = "Histogramme pour la composante vert";
                    visuel_histo_G.Composante = Histo.TypeComposante.composante_G;
                    visuel_histo_G.RepartitionPixel = tab_repartition_g;
                    visuel_histo_G.Margin = new Thickness(0, 0, 0, 2);
                    x_stack_histo.Children.Add(visuel_histo_G);
                    Histo visuel_histo_B = new Histo();
                    visuel_histo_B.Titre = "Histogramme pour la composante bleu";
                    visuel_histo_B.Composante = Histo.TypeComposante.composante_B;
                    visuel_histo_B.RepartitionPixel = tab_repartition_b;
                    visuel_histo_B.Margin = new Thickness(0, 0, 0, 2);
                    x_stack_histo.Children.Add(visuel_histo_B);
                    x_stack_histo.Height = 3 * visuel_histo_R.ActualHeight + 50;
                }
            }
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