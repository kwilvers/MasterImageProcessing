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

namespace VS2013_05_BicubiqueCouleur
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
                "pack://application:,,,/VS2013_05_BicubiqueCouleur;component/collection_images/louis_de_funes_32bit_1000x771_96dpi.jpg",
                UriKind.Absolute);
            BitmapImage bti = new BitmapImage();
            bti.BeginInit();
            bti.UriSource = uri;
            bti.EndInit();
            x_img.Width = bti.PixelWidth;
            x_img.Height = bti.PixelHeight;
            x_img.Source = bti;
            InterpolationBicubique(uri);
        }

        //interpolation bicubique
        private void InterpolationBicubique(Uri uri)
        {
            //image 1 avec 32 bits couleur
            BitmapImage bti = new BitmapImage();
            bti.BeginInit();
            bti.UriSource = uri;
            bti.EndInit();
            WriteableBitmap wb = new WriteableBitmap(bti);
            int largeur_numerisation = (wb.Format.BitsPerPixel / 8) * wb.PixelWidth;
            byte[] tab_pixel = new byte[largeur_numerisation * wb.PixelHeight];
            wb.CopyPixels(tab_pixel, largeur_numerisation, 0);
            int[,] tab_pixel_int_LH = ConvertirTableauPixelEnLH_32bit(tab_pixel, wb.PixelWidth, wb.PixelHeight);
            //initialise avec du blanc
            int largeur_agrandie = 2 * wb.PixelWidth - 1;
            int hauteur_agrandie = 2 * wb.PixelHeight - 1;
            int[,] tab_pixel_int_LH_agrandi = new int[hauteur_agrandie, largeur_agrandie];
            for (int lig = 0; lig < hauteur_agrandie; lig++)
            {
                for (int col = 0; col < largeur_agrandie; col++)
                {
                    tab_pixel_int_LH_agrandi[lig, col] = EncoderCouleurEnInt(Color.FromArgb(255, 255, 255, 255));
                }
            }
            //etape 1 : on copie les pixels d'origine dans leur emplacement dans l'image agrandie
            for (int lig = 0; lig < wb.PixelHeight; lig++)
            {
                for (int col = 0; col < wb.PixelWidth; col++)
                {
                    tab_pixel_int_LH_agrandi[2 * lig, 2 * col] = tab_pixel_int_LH[lig, col];
                }
            }
            //etape2 : on effectue le cacul bilineaire sur chaque ligne
            for (int lig = 0; lig < hauteur_agrandie; lig += 2)
            {
                for (int col = 1; col < (largeur_agrandie - 1); col += 2)
                {
                    //bordure gauche V=(v1+v2)/2
                    if (col == 1)
                    {
                        int coul_v1 = tab_pixel_int_LH_agrandi[lig, col - 1];
                        int coul_v2 = tab_pixel_int_LH_agrandi[lig, col + 1];
                        tab_pixel_int_LH_agrandi[lig, col] = MoyenneDeuxCouleurs(coul_v1, coul_v2);
                    }
                    //cubique V=(-V1+9*V2+9*V3-V4)/16
                    if (col > 1 && col < largeur_agrandie - 2)
                    {
                        int coul_v1 = tab_pixel_int_LH_agrandi[lig, col - 3];
                        int coul_v2 = tab_pixel_int_LH_agrandi[lig, col - 1];
                        int coul_v3 = tab_pixel_int_LH_agrandi[lig, col + 1];
                        int coul_v4 = tab_pixel_int_LH_agrandi[lig, col + 3];
                        tab_pixel_int_LH_agrandi[lig, col] = CubiqueCouleurs(coul_v1, coul_v2, coul_v3, coul_v4);
                    }
                    //bordure droite V=(v1+v2)/2
                    if (col == largeur_agrandie - 2)
                    {
                        int coul_v1 = tab_pixel_int_LH_agrandi[lig, col - 1];
                        int coul_v2 = tab_pixel_int_LH_agrandi[lig, col + 1];
                        tab_pixel_int_LH_agrandi[lig, col] = MoyenneDeuxCouleurs(coul_v1, coul_v2);
                    }
                }
            }
            //etape 3 : on effectue le cacul bicubique sur chaque colonne
            for (int col = 0; col < largeur_agrandie; col++)
            {
                for (int lig = 1; lig < (hauteur_agrandie - 1); lig += 2)
                {
                    //bordure gauche V=(v1+v2)/2
                    if (lig == 1)
                    {
                        int coul_v1 = tab_pixel_int_LH_agrandi[lig - 1, col];
                        int coul_v2 = tab_pixel_int_LH_agrandi[lig + 1, col];
                        tab_pixel_int_LH_agrandi[lig, col] = MoyenneDeuxCouleurs(coul_v1, coul_v2);
                    }
                    //cubique V=(-V1+9*V2+9*V3-V4)/16
                    if (lig > 1 && lig < hauteur_agrandie - 2)
                    {
                        int coul_v1 = tab_pixel_int_LH_agrandi[lig - 3, col];
                        int coul_v2 = tab_pixel_int_LH_agrandi[lig - 1, col];
                        int coul_v3 = tab_pixel_int_LH_agrandi[lig + 1, col];
                        int coul_v4 = tab_pixel_int_LH_agrandi[lig + 3, col];
                        tab_pixel_int_LH_agrandi[lig, col] = CubiqueCouleurs(coul_v1, coul_v2, coul_v3, coul_v4);
                    }
                    //bordure droite V=(v1+v2)/2
                    if (lig == hauteur_agrandie - 2)
                    {
                        int coul_v1 = tab_pixel_int_LH_agrandi[lig - 1, col];
                        int coul_v2 = tab_pixel_int_LH_agrandi[lig + 1, col];
                        tab_pixel_int_LH_agrandi[lig, col] = MoyenneDeuxCouleurs(coul_v1, coul_v2);
                    }
                }
            }
            //on genere l'image resultante
            byte[] tab_pixel_agrandi =
                ConvertirTableauPixelEnUnique_32bit(tab_pixel_int_LH_agrandi, largeur_agrandie, hauteur_agrandie);
            BitmapSource bti_agrandi = BitmapSource.Create(largeur_agrandie, hauteur_agrandie, 96.0, 96.0,
                PixelFormats.Bgra32, null, tab_pixel_agrandi, largeur_agrandie * 4);
            x_img_agrandie.Width = bti_agrandi.PixelWidth;
            x_img_agrandie.Height = bti_agrandi.PixelHeight;
            x_img_agrandie.Source = bti_agrandi;
        }

        //trouver la moyenne de 2 pixels couleur
        private int MoyenneDeuxCouleurs(int coul_v1, int coul_v2)
        {
            int couleur_moyenne = 0;
            byte comp_a_v1 = (byte) (coul_v1 >> 24);
            byte comp_r_v1 = (byte) (coul_v1 >> 16);
            byte comp_g_v1 = (byte) (coul_v1 >> 8);
            byte comp_b_v1 = (byte) (coul_v1 >> 0);
            byte comp_a_v2 = (byte) (coul_v2 >> 24);
            byte comp_r_v2 = (byte) (coul_v2 >> 16);
            byte comp_g_v2 = (byte) (coul_v2 >> 8);
            byte comp_b_v2 = (byte) (coul_v2 >> 0);
            int comp_a_moy = (comp_a_v1 + comp_a_v2) / 2;
            int comp_r_moy = (comp_r_v1 + comp_r_v2) / 2;
            int comp_g_moy = (comp_g_v1 + comp_g_v2) / 2;
            int comp_b_moy = (comp_b_v1 + comp_b_v2) / 2;
            couleur_moyenne = EncoderCouleurEnInt(Color.FromArgb((byte) comp_a_moy, (byte) comp_r_moy,
                (byte) comp_g_moy, (byte) comp_b_moy));
            return couleur_moyenne;
        }

        //trouver la cubique pour 4 pixels couleurs
        private int CubiqueCouleurs(int coul_v1, int coul_v2, int coul_v3, int coul_v4)
        {
            int cubique = 0;
            byte comp_a_v1 = (byte) (coul_v1 >> 24);
            byte comp_r_v1 = (byte) (coul_v1 >> 16);
            byte comp_g_v1 = (byte) (coul_v1 >> 8);
            byte comp_b_v1 = (byte) (coul_v1 >> 0);
            byte comp_a_v2 = (byte) (coul_v2 >> 24);
            byte comp_r_v2 = (byte) (coul_v2 >> 16);
            byte comp_g_v2 = (byte) (coul_v2 >> 8);
            byte comp_b_v2 = (byte) (coul_v2 >> 0);
            byte comp_a_v3 = (byte) (coul_v3 >> 24);
            byte comp_r_v3 = (byte) (coul_v3 >> 16);
            byte comp_g_v3 = (byte) (coul_v3 >> 8);
            byte comp_b_v3 = (byte) (coul_v3 >> 0);
            byte comp_a_v4 = (byte) (coul_v4 >> 24);
            byte comp_r_v4 = (byte) (coul_v4 >> 16);
            byte comp_g_v4 = (byte) (coul_v4 >> 8);
            byte comp_b_v4 = (byte) (coul_v4 >> 0);
            double cubique_r = (-comp_r_v1 + 9 * comp_r_v2 + 9 * comp_r_v3 - comp_r_v4) / 16.0;
            int cubique_r_borne = (int) Math.Min(Math.Abs(cubique_r), 255.0);
            double cubique_g = (-comp_g_v1 + 9 * comp_g_v2 + 9 * comp_g_v3 - comp_g_v4) / 16.0;
            int cubique_g_borne = (int) Math.Min(Math.Abs(cubique_g), 255.0);
            double cubique_b = (-comp_b_v1 + 9 * comp_b_v2 + 9 * comp_b_v3 - comp_b_v4) / 16.0;
            int cubique_b_borne = (int) Math.Min(Math.Abs(cubique_b), 255.0);
            int comp_a_moy = (comp_a_v1 + comp_a_v2 + comp_a_v3 + comp_a_v4) / 4;
            cubique = EncoderCouleurEnInt(Color.FromArgb((byte) comp_a_moy, (byte) cubique_r_borne,
                (byte) cubique_g_borne, (byte) cubique_b_borne));
            return cubique;
        }

        //encoder objet Color en Int
        private int EncoderCouleurEnInt(Color couleur)
        {
            int couleur_int = couleur.A << 24 | couleur.R << 16 | couleur.G << 8 | couleur.B;
            return couleur_int;
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