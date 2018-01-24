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

namespace VS2013_07_SeuillageTronque
{
    /// <summary>
    /// Logique d'interaction pour Page.xaml
    /// </summary>
    public partial class Page : UserControl
    {
        //donnees
        private bool v_fen_charge = false;

        private bool v_inversion = false;
        private byte v_glissiere_seuil = 0;

        public Page()
        {
            InitializeComponent();
        }

        //controle Loaded
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            v_fen_charge = true;
            Uri uri = new Uri(
                "pack://application:,,,/VS2013_07_SeuillageTronque;component/collection_images/seuillage_louis_de_funes_8bit_388x446_96dpi.jpg",
                UriKind.Absolute);
            BitmapImage bti = new BitmapImage();
            bti.BeginInit();
            bti.UriSource = uri;
            bti.EndInit();
            x_img.Width = bti.PixelWidth;
            x_img.Height = bti.PixelHeight;
            x_img.Source = bti;
            v_glissiere_seuil = (byte) x_slider_seuil.Value;
            x_tbl_seuil.Text = "Seuillage = " + v_glissiere_seuil.ToString();
            SeuillageTronque();
        }

        //
        private void x_slider_seuil_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (v_fen_charge == true)
            {
                v_glissiere_seuil = (byte) x_slider_seuil.Value;
                x_tbl_seuil.Text = "Seuillage = " + v_glissiere_seuil.ToString();
                SeuillageTronque();
            }
        }

        //
        private void SeuillageTronque()
        {
            Uri uri = new Uri(
                "pack://application:,,,/VS2013_07_SeuillageTronque;component/collection_images/seuillage_louis_de_funes_8bit_388x446_96dpi.jpg",
                UriKind.Absolute);
            BitmapImage bti = new BitmapImage();
            bti.BeginInit();
            bti.UriSource = uri;
            bti.EndInit();
            WriteableBitmap wb = new WriteableBitmap(bti);
            int largeur_numerisation = (wb.Format.BitsPerPixel / 8) * wb.PixelWidth;
            byte[] tab_pixel = new byte[largeur_numerisation * wb.PixelHeight];
            wb.CopyPixels(tab_pixel, largeur_numerisation, 0);
            byte[,] tab_pixel_LH = TransposerTableauPixelEnLH_8bit(tab_pixel, wb.PixelWidth, wb.PixelHeight);
            //visualise l'histogramme des gris de l'image initiale
            x_histo_seuillage.VisualiserHistoGrisImgInitiale(tab_pixel, wb.PixelWidth, wb.PixelHeight);
            x_histo_seuillage.PositionnerLigneVertiSeuillage((int) v_glissiere_seuil);
            //on effectue le seuillage tronque
            byte niv_seuillage = v_glissiere_seuil;
            for (int lig = 0; lig < wb.PixelHeight; lig++)
            {
                for (int col = 0; col < wb.PixelWidth; col++)
                {
                    byte niveau = tab_pixel_LH[lig, col];
                    if (v_inversion == false)
                    {
                        if (niveau >= niv_seuillage)
                        {
                            niveau = niv_seuillage;
                        }
                    }
                    if (v_inversion == true)
                    {
                        if (niveau < niv_seuillage)
                        {
                            niveau = niv_seuillage;
                        }
                    }
                    tab_pixel_LH[lig, col] = niveau;
                }
            }
            //on genere l'image resultante
            byte[] tab_pixel_seuillee =
                TransposerTableauPixelEnUnique_8bit(tab_pixel_LH, wb.PixelWidth, wb.PixelHeight);
            BitmapSource bti_res = BitmapSource.Create(wb.PixelWidth, wb.PixelHeight, 96.0, 96.0,
                PixelFormats.Gray8, null, tab_pixel_seuillee, largeur_numerisation);
            x_img_seuillee.Width = bti_res.PixelWidth;
            x_img_seuillee.Height = bti_res.PixelHeight;
            x_img_seuillee.Source = bti_res;
        }

        //case inversion passe de non cochée à cochée
        private void x_check_seuil_Checked(object sender, RoutedEventArgs e)
        {
            v_inversion = !v_inversion;
            SeuillageTronque();
        }

        //case inversion passe de cochée à non cochée
        private void x_check_seuil_Unchecked(object sender, RoutedEventArgs e)
        {
            v_inversion = !v_inversion;
            SeuillageTronque();
        }

        //
        private void ViderControle(Image controle_image, int largeur, int hauteur)
        {
            Uri uri = new Uri(
                "pack://application:,,,/VS2013_07_SeuillageTronque;component/contenu/image/fond_damier.png",
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

        //trouver le niveau median dans un voisinage
        private byte TrouverNiveauMedian(int[] voisinage)
        {
            byte mediane = 0;
            List<int> liste = voisinage.ToList();
            liste.Sort();
            mediane = (byte) liste[liste.Count / 2];
            return mediane;
        }

        //inverser un niveau
        private byte InverserNiveau(byte niveau)
        {
            return (byte) (255 - niveau);
        }

        //borner le niveau
        public byte BornerNiveau(byte niveau)
        {
            byte niveau_borne = niveau;
            if (niveau < 0)
            {
                niveau_borne = 0;
            }
            if (niveau > 255)
            {
                niveau_borne = 255;
            }
            return niveau_borne;
        }

        //appliquer un filtre sur un tableau de pixels
        private void AppliquerFiltre(byte[,] tab_pixels_LH, Filtre filtre, byte[,] tab_pixels_res_LH, int largeur,
            int hauteur)
        {
            for (int lig = 1; lig < hauteur - 1; lig++)
            {
                for (int col = 1; col < largeur - 1; col++)
                {
                    int[] voisins = new int[9];
                    voisins[0] = tab_pixels_LH[lig - 1, col - 1];
                    voisins[1] = tab_pixels_LH[lig - 1, col];
                    voisins[2] = tab_pixels_LH[lig - 1, col + 1];
                    voisins[3] = tab_pixels_LH[lig, col - 1];
                    voisins[4] = tab_pixels_LH[lig, col];
                    voisins[5] = tab_pixels_LH[lig, col + 1];
                    voisins[6] = tab_pixels_LH[lig + 1, col - 1];
                    voisins[7] = tab_pixels_LH[lig + 1, col];
                    voisins[8] = tab_pixels_LH[lig + 1, col + 1];
                    byte niveau_filtre = filtre.PixelFiltre(voisins);
                    BornerNiveau(niveau_filtre);
                    InverserNiveau(niveau_filtre);
                    tab_pixels_res_LH[lig, col] = niveau_filtre;
                }
            }
        }
    } //end class
}