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

namespace VS2013_06_ModeleBruit
{
    /// <summary>
    /// Logique d'interaction pour Page.xaml
    /// </summary>
    public partial class Page : UserControl
    {
        //donnees
        private bool v_fen_charge = false;

        private int v_img_largeur = 0;
        private int v_img_hauteur = 0;

        public Page()
        {
            InitializeComponent();
        }

        //controle Loaded
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            v_fen_charge = true;
            Uri uri = new Uri(
                "pack://application:,,,/VS2013_06_ModeleBruit;component/collection_images/biographie_8bit_600x913_96dpi.jpg",
                UriKind.Absolute);
            BitmapImage bti = new BitmapImage();
            bti.BeginInit();
            bti.UriSource = uri;
            bti.EndInit();
            v_img_largeur = bti.PixelWidth;
            v_img_hauteur = bti.PixelHeight;
            x_img.Width = v_img_largeur;
            x_img.Height = v_img_hauteur;
            x_img.Source = bti;
        }

        //selecteur de modele de bruit
        private void x_cbx_modele_bruit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (v_fen_charge == true)
            {
                x_border_histo.Child = null;
                ViderControle(x_img_bruitee, 761, 401);
                ViderControle(x_img_modele_bruit, 761, 401);
                //modele bruit uniforme
                if (x_cbx_modele_bruit.SelectedIndex == 1)
                {
                    //modeliser un modele de bruit uniforme et l'afficher avec son histogramme
                    BruitImage8bit bruit = new BruitImage8bit(v_img_largeur, v_img_hauteur);
                    int bruit_niv_mini = 107;
                    int bruit_niv_maxi = 147;
                    BitmapSource bitmap_bruit = bruit.BitmapModeleUniforme(bruit_niv_mini, bruit_niv_maxi);
                    x_img_modele_bruit.Width = bitmap_bruit.PixelWidth;
                    x_img_modele_bruit.Height = bitmap_bruit.PixelHeight;
                    x_img_modele_bruit.Source = bitmap_bruit;
                    HistoNormaliseNg histo_normalise = new HistoNormaliseNg();
                    histo_normalise.Titre = "Histogramme normalisé du modèle bruit";
                    histo_normalise.PixelLargeur = v_img_largeur;
                    histo_normalise.PixelHauteur = v_img_hauteur;
                    histo_normalise.PixelImage_LH = bruit.P_Pixels_LH;
                    histo_normalise.AfficherCourbeCumul = false;
                    x_border_histo.Child = histo_normalise;
                    //appliquer le modele de bruit sur l'image initiale 8 bits 256 gris
                    Uri uri = new Uri(
                        "pack://application:,,,/VS2013_06_ModeleBruit;component/collection_images/biographie_8bit_600x913_96dpi.jpg",
                        UriKind.Absolute);
                    BitmapImage bti = new BitmapImage();
                    bti.BeginInit();
                    bti.UriSource = uri;
                    bti.EndInit();
                    WriteableBitmap wb = new WriteableBitmap(bti);
                    int largeur_numerisation = (wb.Format.BitsPerPixel / 8) * wb.PixelWidth;
                    byte[] tab_pixel = new byte[largeur_numerisation * wb.PixelHeight];
                    wb.CopyPixels(tab_pixel, largeur_numerisation, 0);
                    byte[,] tab_pixel_int_LH =
                        TransposerTableauPixelEnLH_8bit(tab_pixel, wb.PixelWidth, wb.PixelHeight);
                    byte[,] tab_pixel_bruit_LH = bruit.P_Pixels_LH;
                    double facteur = 0.90;
                    double borne_mini = 127.0 - (127.0 - bruit_niv_mini) * facteur;
                    double borne_maxi = 127.0 + (bruit_niv_maxi - 127.0) * facteur;
                    for (int lig = 0; lig < wb.PixelHeight; lig++)
                    {
                        for (int col = 0; col < wb.PixelWidth; col++)
                        {
                            if (tab_pixel_bruit_LH[lig, col] <= borne_mini ||
                                tab_pixel_bruit_LH[lig, col] >= borne_maxi)
                            {
                                tab_pixel_int_LH[lig, col] = tab_pixel_bruit_LH[lig, col];
                            }
                        }
                    }
                    //on genere l'image resultante
                    byte[] tab_pixel_res =
                        TransposerTableauPixelEnUnique_8bit(tab_pixel_int_LH, wb.PixelWidth, wb.PixelHeight);
                    BitmapSource bti_res = BitmapSource.Create(wb.PixelWidth, wb.PixelHeight, 96.0, 96.0,
                        PixelFormats.Gray8, null, tab_pixel_res, largeur_numerisation);
                    x_img_bruitee.Width = bti_res.PixelWidth;
                    x_img_bruitee.Height = bti_res.PixelHeight;
                    x_img_bruitee.Source = bti_res;
                }
                //modele bruit poivre et sel
                if (x_cbx_modele_bruit.SelectedIndex == 2)
                {
                    //modeliser un modele de bruit poivre et sel et l'afficher avec son histogramme
                    BruitImage8bit bruit = new BruitImage8bit(v_img_largeur, v_img_hauteur);
                    int bruit_niv_1 = 10;
                    int bruit_niv_2 = 240;
                    BitmapSource bitmap_bruit = bruit.BitmapModelePoivreEtSel(bruit_niv_1, bruit_niv_2);
                    x_img_modele_bruit.Width = bitmap_bruit.PixelWidth;
                    x_img_modele_bruit.Height = bitmap_bruit.PixelHeight;
                    x_img_modele_bruit.Source = bitmap_bruit;
                    HistoNormaliseNg histo_normalise = new HistoNormaliseNg();
                    histo_normalise.Titre = "Histogramme normalisé du modèle bruit";
                    histo_normalise.PixelLargeur = v_img_largeur;
                    histo_normalise.PixelHauteur = v_img_hauteur;
                    histo_normalise.PixelImage_LH = bruit.P_Pixels_LH;
                    histo_normalise.AfficherCourbeCumul = false;
                    x_border_histo.Child = histo_normalise;
                    //appliquer le modele de bruit sur l'image initiale 8 bits 256 gris
                    Uri uri = new Uri(
                        "pack://application:,,,/VS2013_06_ModeleBruit;component/collection_images/biographie_8bit_600x913_96dpi.jpg",
                        UriKind.Absolute);
                    BitmapImage bti = new BitmapImage();
                    bti.BeginInit();
                    bti.UriSource = uri;
                    bti.EndInit();
                    WriteableBitmap wb = new WriteableBitmap(bti);
                    int largeur_numerisation = (wb.Format.BitsPerPixel / 8) * wb.PixelWidth;
                    byte[] tab_pixel = new byte[largeur_numerisation * wb.PixelHeight];
                    wb.CopyPixels(tab_pixel, largeur_numerisation, 0);
                    byte[,] tab_pixel_int_LH =
                        TransposerTableauPixelEnLH_8bit(tab_pixel, wb.PixelWidth, wb.PixelHeight);
                    byte[,] tab_pixel_bruit_LH = bruit.P_Pixels_LH;
                    for (int lig = 0; lig < wb.PixelHeight; lig++)
                    {
                        for (int col = 0; col < wb.PixelWidth; col++)
                        {
                            if (tab_pixel_bruit_LH[lig, col] == bruit_niv_1 ||
                                tab_pixel_bruit_LH[lig, col] == bruit_niv_2)
                            {
                                tab_pixel_int_LH[lig, col] = tab_pixel_bruit_LH[lig, col];
                            }
                        }
                    }
                    //on genere l'image resultante
                    byte[] tab_pixel_res =
                        TransposerTableauPixelEnUnique_8bit(tab_pixel_int_LH, wb.PixelWidth, wb.PixelHeight);
                    BitmapSource bti_res = BitmapSource.Create(wb.PixelWidth, wb.PixelHeight, 96.0, 96.0,
                        PixelFormats.Gray8, null, tab_pixel_res, largeur_numerisation);
                    x_img_bruitee.Width = bti_res.PixelWidth;
                    x_img_bruitee.Height = bti_res.PixelHeight;
                    x_img_bruitee.Source = bti_res;
                }
            }
        }

        //
        private void ViderControle(Image controle_image, int largeur, int hauteur)
        {
            Uri uri = new Uri("pack://application:,,,/VS2013_06_ModeleBruit;component/contenu/image/fond_damier.png",
                UriKind.Absolute);
            BitmapImage bti = new BitmapImage();
            bti.BeginInit();
            bti.UriSource = uri;
            bti.EndInit();
            controle_image.Width = largeur;
            controle_image.Height = hauteur;
            controle_image.Source = bti;
        }

        //transposition tableau pixel dimension 1 vers 2 avec codage 8 bits
        private byte[,] TransposerTableauPixelEnLH_8bit(byte[] tab_pixel, int pixel_larg, int pixel_haut)
        {
            byte[,] tab_LH = new byte[pixel_haut, pixel_larg];
            int lig = 0;
            int col = 0;
            for (int xx = 0; xx < tab_pixel.Length; xx++)
            {
                byte niveau_gris = tab_pixel[xx];
                tab_LH[lig, col] = niveau_gris;
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
        private byte[] TransposerTableauPixelEnUnique_8bit(byte[,] tab_pixel_int_LH_modif, int pixel_largeur,
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