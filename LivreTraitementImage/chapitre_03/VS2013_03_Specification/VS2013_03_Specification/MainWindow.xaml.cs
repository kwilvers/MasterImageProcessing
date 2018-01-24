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

namespace VS2013_03_Specification
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
            AjouterImageInitialeEtHistogramme();
            AjouterHistogrammeReference();
            SpecificationHistogramme();
        }

        //
        private void AjouterImageInitialeEtHistogramme()
        {
            Uri uri = new Uri(
                "pack://application:,,,/VS2013_03_Specification;component/collection_images/gendarme_newyork_8bit_800x797_96dpi.jpg",
                UriKind.Absolute);
            BitmapImage bti = new BitmapImage();
            bti.BeginInit();
            bti.UriSource = uri;
            bti.EndInit();
            x_page.x_img.Width = bti.PixelWidth;
            x_page.x_img.Height = bti.PixelHeight;
            x_page.x_img.Source = bti;
            //image 8 bits contenant des niveaux 256 gris 
            WriteableBitmap wb = new WriteableBitmap(bti);
            int largeur_numerisation = (wb.Format.BitsPerPixel / 8) * wb.PixelWidth;
            byte[] tab_pixel = new byte[largeur_numerisation * wb.PixelHeight];
            wb.CopyPixels(tab_pixel, largeur_numerisation, 0);
            int[,] tab_pixel_int_LH = ConvertirTableauPixelEnLH_8bit(tab_pixel, wb.PixelWidth, wb.PixelHeight);
            byte[,] tab_pixel_gris_LH = new byte[wb.PixelHeight, wb.PixelWidth];
            for (int lig = 0; lig < wb.PixelHeight; lig++)
            {
                for (int col = 0; col < wb.PixelWidth; col++)
                {
                    int gris_int = tab_pixel_int_LH[lig, col];
                    tab_pixel_gris_LH[lig, col] = (byte) gris_int;
                }
            }
            HistoNormaliseNg visuel_histo = new HistoNormaliseNg();
            visuel_histo.Titre = "Histogramme normalisé pour les niveaux de gris";
            visuel_histo.PixelImage_LH = tab_pixel_gris_LH;
            visuel_histo.PixelLargeur = wb.PixelWidth;
            visuel_histo.PixelHauteur = wb.PixelHeight;
            visuel_histo.AfficherCourbeCumul = true;
            x_page.x_border_histo_initial.Child = visuel_histo;
        }

        //
        private void AjouterHistogrammeReference()
        {
            Uri uri = new Uri(
                "pack://application:,,,/VS2013_03_Specification;component/collection_images/louis_de_funes_8bit_600x900_96dpi_reference.jpg",
                UriKind.Absolute);
            BitmapImage bti = new BitmapImage();
            bti.BeginInit();
            bti.UriSource = uri;
            bti.EndInit();
            //image 8 bits contenant des niveaux 256 gris 
            WriteableBitmap wb = new WriteableBitmap(bti);
            int largeur_numerisation = (wb.Format.BitsPerPixel / 8) * wb.PixelWidth;
            byte[] tab_pixel = new byte[largeur_numerisation * wb.PixelHeight];
            wb.CopyPixels(tab_pixel, largeur_numerisation, 0);
            int[,] tab_pixel_int_LH = ConvertirTableauPixelEnLH_8bit(tab_pixel, wb.PixelWidth, wb.PixelHeight);
            byte[,] tab_pixel_gris_LH = new byte[wb.PixelHeight, wb.PixelWidth];
            for (int lig = 0; lig < wb.PixelHeight; lig++)
            {
                for (int col = 0; col < wb.PixelWidth; col++)
                {
                    int gris_int = tab_pixel_int_LH[lig, col];
                    tab_pixel_gris_LH[lig, col] = (byte) gris_int;
                }
            }
            HistoNormaliseNg visuel_histo = new HistoNormaliseNg();
            visuel_histo.Titre = "Histogramme normalisé pour les niveaux de gris";
            visuel_histo.PixelImage_LH = tab_pixel_gris_LH;
            visuel_histo.PixelLargeur = wb.PixelWidth;
            visuel_histo.PixelHauteur = wb.PixelHeight;
            visuel_histo.AfficherCourbeCumul = true;
            x_page.x_border_histo_reference.Child = visuel_histo;
        }

        //specification d'histogramme
        private void SpecificationHistogramme()
        {
            Uri uri = new Uri(
                "pack://application:,,,/VS2013_03_Specification;component/collection_images/gendarme_newyork_8bit_800x797_96dpi.jpg",
                UriKind.Absolute);
            BitmapImage bti = new BitmapImage();
            bti.BeginInit();
            bti.UriSource = uri;
            bti.EndInit();
            //image 8 bits contenant des niveaux 256 gris de l'image initiale 
            WriteableBitmap wb = new WriteableBitmap(bti);
            int largeur_numerisation = (wb.Format.BitsPerPixel / 8) * wb.PixelWidth;
            byte[] tab_pixel = new byte[largeur_numerisation * wb.PixelHeight];
            wb.CopyPixels(tab_pixel, largeur_numerisation, 0);
            int[,] tab_pixel_int_LH = ConvertirTableauPixelEnLH_8bit(tab_pixel, wb.PixelWidth, wb.PixelHeight);
            //calcul de l'histogramme de l'image initiale
            int[] histo = new int[256];
            for (int lig = 0; lig < wb.PixelHeight; lig++)
            {
                for (int col = 0; col < wb.PixelWidth; col++)
                {
                    int niveau_gris_int = tab_pixel_int_LH[lig, col];
                    histo[niveau_gris_int] += 1;
                }
            }
            //calcul de l'histogramme normalisé de l'image initiale
            double[] histo_normalise = new double[256];
            int total_pixel = wb.PixelWidth * wb.PixelHeight;
            for (int xx = 0; xx < histo.Length; xx++)
            {
                histo_normalise[xx] = (double) histo[xx] / (double) (total_pixel);
            }
            //calcul de l'histogramme normalisé cumulé de l'image initiale
            double[] histo_normalise_cumule = new double[256];
            double somme_proba = 0d;
            for (int xx = 0; xx < histo_normalise.Length; xx++)
            {
                somme_proba += histo_normalise[xx];
                histo_normalise_cumule[xx] = somme_proba;
            }
            //image 8 bits contenant des niveaux 256 gris de l'image de reference 
            Uri uri_reference =
                new Uri(
                    "pack://application:,,,/VS2013_03_Specification;component/collection_images/louis_de_funes_8bit_600x900_96dpi_reference.jpg",
                    UriKind.Absolute);
            BitmapImage bti_reference = new BitmapImage();
            bti_reference.BeginInit();
            bti_reference.UriSource = uri_reference;
            bti_reference.EndInit();
            WriteableBitmap wb_reference = new WriteableBitmap(bti_reference);
            int largeur_numerisation_reference = (wb_reference.Format.BitsPerPixel / 8) * wb_reference.PixelWidth;
            byte[] tab_pixel_reference = new byte[largeur_numerisation_reference * wb_reference.PixelHeight];
            wb_reference.CopyPixels(tab_pixel_reference, largeur_numerisation_reference, 0);
            int[,] tab_pixel_reference_int_LH = ConvertirTableauPixelEnLH_8bit(tab_pixel_reference,
                wb_reference.PixelWidth, wb_reference.PixelHeight);
            //calcul de l'histogramme de l'image de reference
            int[] histo_reference = new int[256];
            for (int lig = 0; lig < wb_reference.PixelHeight; lig++)
            {
                for (int col = 0; col < wb_reference.PixelWidth; col++)
                {
                    int niveau_gris_int = tab_pixel_reference_int_LH[lig, col];
                    histo_reference[niveau_gris_int] += 1;
                }
            }
            //calcul de l'histogramme normalisé de l'image de reference
            double[] histo_normalise_reference = new double[256];
            int total_pixel_reference = wb_reference.PixelWidth * wb_reference.PixelHeight;
            for (int xx = 0; xx < histo_normalise_reference.Length; xx++)
            {
                histo_normalise_reference[xx] = (double) histo[xx] / (double) (total_pixel_reference);
            }
            //calcul de l'histogramme normalisé cumulé de l'image de reference
            double[] histo_normalise_cumule_reference = new double[256];
            double somme_proba_reference = 0d;
            for (int xx = 0; xx < histo_normalise_reference.Length; xx++)
            {
                somme_proba_reference += histo_normalise_reference[xx];
                histo_normalise_cumule_reference[xx] = somme_proba_reference;
            }
            //calcul des correspondances de la specification
            int[] correspondance = new int[256];
            correspondance = AlgoSpecification(histo_normalise_cumule, histo_normalise_cumule_reference);
            //changement de pixels
            int[,] tab_pixel_int_LH_specifie = new int[wb.PixelHeight, wb.PixelWidth];
            for (int lig = 0; lig < wb.PixelHeight; lig++)
            {
                for (int col = 0; col < wb.PixelWidth; col++)
                {
                    int niveau_gris_int = tab_pixel_int_LH[lig, col];
                    int niveau_gris_specifie = correspondance[niveau_gris_int];
                    tab_pixel_int_LH_specifie[lig, col] = niveau_gris_specifie;
                }
            }
            byte[] tab_pixel_modif = new byte[largeur_numerisation * wb.PixelHeight];
            tab_pixel_modif =
                ConvertirTableauPixelEnUnique_8bit(tab_pixel_int_LH_specifie, wb.PixelWidth, wb.PixelHeight);
            BitmapSource bti_modif = BitmapSource.Create(wb.PixelWidth, wb.PixelHeight, 96.0, 96.0, PixelFormats.Gray8,
                null, tab_pixel_modif, largeur_numerisation);
            x_page.x_img_traitee.Width = bti_modif.PixelWidth;
            x_page.x_img_traitee.Height = bti_modif.PixelHeight;
            x_page.x_img_traitee.Source = bti_modif;
            HistoNormaliseNg visuel_histo = new HistoNormaliseNg();
            visuel_histo.Titre = "Histogramme normalisé pour les niveaux de gris";
            byte[,] tab_pixel_byte_LH_specifie = new byte[wb.PixelHeight, wb.PixelWidth];
            for (int lig = 0; lig < wb.PixelHeight; lig++)
            {
                for (int col = 0; col < wb.PixelWidth; col++)
                {
                    tab_pixel_byte_LH_specifie[lig, col] = (byte) tab_pixel_int_LH_specifie[lig, col];
                }
            }
            visuel_histo.PixelImage_LH = tab_pixel_byte_LH_specifie;
            visuel_histo.PixelLargeur = wb.PixelWidth;
            visuel_histo.PixelHauteur = wb.PixelHeight;
            visuel_histo.AfficherCourbeCumul = true;
            x_page.x_border_histo_final.Child = visuel_histo;
        }

        //algorithme de la specification d'histogramme
        private int[] AlgoSpecification(double[] Hx, double[] Hz)
        {
            int[] nouv_niv = new int[256];
            int j = 0;
            for (int i = 0; i <= 255; i++)
            {
                if (Hx[i] <= Hz[j])
                {
                    nouv_niv[i] = j;
                }
                else
                {
                    while (Hx[i] > Hz[j])
                    {
                        j++;
                    }
                    if (Hz[j] - Hx[i] > Hx[i] - Hz[j - 1])
                    {
                        nouv_niv[i] = --j;
                    }
                    else
                    {
                        nouv_niv[i] = j;
                    }
                }
            }
            return nouv_niv;
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