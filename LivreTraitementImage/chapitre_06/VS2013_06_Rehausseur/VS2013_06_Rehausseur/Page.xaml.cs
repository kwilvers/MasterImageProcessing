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

namespace VS2013_06_Rehausseur
{
    /// <summary>
    /// Logique d'interaction pour Page.xaml
    /// </summary>
    public partial class Page : UserControl
    {
        //donnees
        private bool v_fen_charge = false;

        public Page()
        {
            InitializeComponent();
        }

        //controle Loaded
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            v_fen_charge = true;
        }

        //selecteur de modele de bruit
        private void x_cbx_modele_bruit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (v_fen_charge == true)
            {
                ViderControle(x_img, 761, 429);
                ViderControle(x_img_filtree, 761, 429);
                //filtre rehausseur 3x3 image 8 bits motif_test
                if (x_cbx_filtre.SelectedIndex == 1)
                {
                    Uri uri = new Uri(
                        "pack://application:,,,/VS2013_06_Rehausseur;component/collection_images/motif_test_8bit_800x852_96dpi.png",
                        UriKind.Absolute);
                    BitmapImage bti = new BitmapImage();
                    bti.BeginInit();
                    bti.UriSource = uri;
                    bti.EndInit();
                    x_img.Width = bti.PixelWidth;
                    x_img.Height = bti.PixelHeight;
                    x_img.Source = bti;
                    WriteableBitmap wb = new WriteableBitmap(bti);
                    int largeur_numerisation = (wb.Format.BitsPerPixel / 8) * wb.PixelWidth;
                    byte[] tab_pixel = new byte[largeur_numerisation * wb.PixelHeight];
                    wb.CopyPixels(tab_pixel, largeur_numerisation, 0);
                    byte[,] tab_pixel_LH = TransposerTableauPixelEnLH_8bit(tab_pixel, wb.PixelWidth, wb.PixelHeight);
                    byte[,] tab_pixel_LH_filtre = new byte[wb.PixelHeight, wb.PixelWidth];
                    for (int lig = 0; lig < wb.PixelHeight; lig++)
                    {
                        for (int col = 0; col < wb.PixelWidth; col++)
                        {
                            tab_pixel_LH_filtre[lig, col] = 255;
                        }
                    }
                    //filtrage
                    Filtre filtre = new Filtre(3, new int[]
                    {
                        0, -1, 0,
                        -1, 5, -1,
                        0, -1, 0
                    });
                    for (int lig = 1; lig < wb.PixelHeight - 1; lig++)
                    {
                        for (int col = 1; col < wb.PixelWidth - 1; col++)
                        {
                            int[] voisins = new int[9];
                            voisins[0] = tab_pixel_LH[lig - 1, col - 1];
                            voisins[1] = tab_pixel_LH[lig - 1, col];
                            voisins[2] = tab_pixel_LH[lig - 1, col + 1];
                            voisins[3] = tab_pixel_LH[lig, col - 1];
                            voisins[4] = tab_pixel_LH[lig, col];
                            voisins[5] = tab_pixel_LH[lig, col + 1];
                            voisins[6] = tab_pixel_LH[lig + 1, col - 1];
                            voisins[7] = tab_pixel_LH[lig + 1, col];
                            voisins[8] = tab_pixel_LH[lig + 1, col + 1];
                            byte niveau_filtre = filtre.PixelFiltre(voisins);
                            tab_pixel_LH_filtre[lig, col] = BornerNiveauGris(niveau_filtre);
                        }
                    }
                    //on genere l'image resultante
                    byte[] tab_pixel_res =
                        TransposerTableauPixelEnUnique_8bit(tab_pixel_LH_filtre, wb.PixelWidth, wb.PixelHeight);
                    BitmapSource bti_res = BitmapSource.Create(wb.PixelWidth, wb.PixelHeight, 96.0, 96.0,
                        PixelFormats.Gray8, null, tab_pixel_res, largeur_numerisation);
                    x_img_filtree.Width = bti_res.PixelWidth;
                    x_img_filtree.Height = bti_res.PixelHeight;
                    x_img_filtree.Source = bti_res;
                }
                //filtre rehausseur 3x3 image 8 bits motif_test
                if (x_cbx_filtre.SelectedIndex == 2)
                {
                    Uri uri = new Uri(
                        "pack://application:,,,/VS2013_06_Rehausseur;component/collection_images/motif_test_8bit_800x852_96dpi.png",
                        UriKind.Absolute);
                    BitmapImage bti = new BitmapImage();
                    bti.BeginInit();
                    bti.UriSource = uri;
                    bti.EndInit();
                    x_img.Width = bti.PixelWidth;
                    x_img.Height = bti.PixelHeight;
                    x_img.Source = bti;
                    WriteableBitmap wb = new WriteableBitmap(bti);
                    int largeur_numerisation = (wb.Format.BitsPerPixel / 8) * wb.PixelWidth;
                    byte[] tab_pixel = new byte[largeur_numerisation * wb.PixelHeight];
                    wb.CopyPixels(tab_pixel, largeur_numerisation, 0);
                    byte[,] tab_pixel_LH = TransposerTableauPixelEnLH_8bit(tab_pixel, wb.PixelWidth, wb.PixelHeight);
                    byte[,] tab_pixel_LH_filtre = new byte[wb.PixelHeight, wb.PixelWidth];
                    for (int lig = 0; lig < wb.PixelHeight; lig++)
                    {
                        for (int col = 0; col < wb.PixelWidth; col++)
                        {
                            tab_pixel_LH_filtre[lig, col] = 255;
                        }
                    }
                    //filtrage
                    Filtre filtre = new Filtre(3, new int[]
                    {
                        -1, -1, -1,
                        -1, 9, -1,
                        -1, -1, -1
                    });
                    for (int lig = 1; lig < wb.PixelHeight - 1; lig++)
                    {
                        for (int col = 1; col < wb.PixelWidth - 1; col++)
                        {
                            int[] voisins = new int[9];
                            voisins[0] = tab_pixel_LH[lig - 1, col - 1];
                            voisins[1] = tab_pixel_LH[lig - 1, col];
                            voisins[2] = tab_pixel_LH[lig - 1, col + 1];
                            voisins[3] = tab_pixel_LH[lig, col - 1];
                            voisins[4] = tab_pixel_LH[lig, col];
                            voisins[5] = tab_pixel_LH[lig, col + 1];
                            voisins[6] = tab_pixel_LH[lig + 1, col - 1];
                            voisins[7] = tab_pixel_LH[lig + 1, col];
                            voisins[8] = tab_pixel_LH[lig + 1, col + 1];
                            byte niveau_filtre = filtre.PixelFiltre(voisins);
                            tab_pixel_LH_filtre[lig, col] = BornerNiveauGris(niveau_filtre);
                        }
                    }
                    //on genere l'image resultante
                    byte[] tab_pixel_res =
                        TransposerTableauPixelEnUnique_8bit(tab_pixel_LH_filtre, wb.PixelWidth, wb.PixelHeight);
                    BitmapSource bti_res = BitmapSource.Create(wb.PixelWidth, wb.PixelHeight, 96.0, 96.0,
                        PixelFormats.Gray8, null, tab_pixel_res, largeur_numerisation);
                    x_img_filtree.Width = bti_res.PixelWidth;
                    x_img_filtree.Height = bti_res.PixelHeight;
                    x_img_filtree.Source = bti_res;
                }
                //filtre rehausseur 3x3 image 8 bits motif_test
                if (x_cbx_filtre.SelectedIndex == 3)
                {
                    Uri uri = new Uri(
                        "pack://application:,,,/VS2013_06_Rehausseur;component/collection_images/motif_test_8bit_800x852_96dpi.png",
                        UriKind.Absolute);
                    BitmapImage bti = new BitmapImage();
                    bti.BeginInit();
                    bti.UriSource = uri;
                    bti.EndInit();
                    x_img.Width = bti.PixelWidth;
                    x_img.Height = bti.PixelHeight;
                    x_img.Source = bti;
                    WriteableBitmap wb = new WriteableBitmap(bti);
                    int largeur_numerisation = (wb.Format.BitsPerPixel / 8) * wb.PixelWidth;
                    byte[] tab_pixel = new byte[largeur_numerisation * wb.PixelHeight];
                    wb.CopyPixels(tab_pixel, largeur_numerisation, 0);
                    byte[,] tab_pixel_LH = TransposerTableauPixelEnLH_8bit(tab_pixel, wb.PixelWidth, wb.PixelHeight);
                    byte[,] tab_pixel_LH_filtre = new byte[wb.PixelHeight, wb.PixelWidth];
                    for (int lig = 0; lig < wb.PixelHeight; lig++)
                    {
                        for (int col = 0; col < wb.PixelWidth; col++)
                        {
                            tab_pixel_LH_filtre[lig, col] = 255;
                        }
                    }
                    //filtrage
                    Filtre filtre = new Filtre(3, new int[]
                    {
                        1, -2, 1,
                        -2, 5, -2,
                        1, -2, 1
                    });
                    for (int lig = 1; lig < wb.PixelHeight - 1; lig++)
                    {
                        for (int col = 1; col < wb.PixelWidth - 1; col++)
                        {
                            int[] voisins = new int[9];
                            voisins[0] = tab_pixel_LH[lig - 1, col - 1];
                            voisins[1] = tab_pixel_LH[lig - 1, col];
                            voisins[2] = tab_pixel_LH[lig - 1, col + 1];
                            voisins[3] = tab_pixel_LH[lig, col - 1];
                            voisins[4] = tab_pixel_LH[lig, col];
                            voisins[5] = tab_pixel_LH[lig, col + 1];
                            voisins[6] = tab_pixel_LH[lig + 1, col - 1];
                            voisins[7] = tab_pixel_LH[lig + 1, col];
                            voisins[8] = tab_pixel_LH[lig + 1, col + 1];
                            byte niveau_filtre = filtre.PixelFiltre(voisins);
                            tab_pixel_LH_filtre[lig, col] = BornerNiveauGris(niveau_filtre);
                        }
                    }
                    //on genere l'image resultante
                    byte[] tab_pixel_res =
                        TransposerTableauPixelEnUnique_8bit(tab_pixel_LH_filtre, wb.PixelWidth, wb.PixelHeight);
                    BitmapSource bti_res = BitmapSource.Create(wb.PixelWidth, wb.PixelHeight, 96.0, 96.0,
                        PixelFormats.Gray8, null, tab_pixel_res, largeur_numerisation);
                    x_img_filtree.Width = bti_res.PixelWidth;
                    x_img_filtree.Height = bti_res.PixelHeight;
                    x_img_filtree.Source = bti_res;
                }
                //filtre rehausseur 3x3 image 8 bits 
                if (x_cbx_filtre.SelectedIndex == 4)
                {
                    Uri uri = new Uri(
                        "pack://application:,,,/VS2013_06_Rehausseur;component/collection_images/louis_de_funes_8bit_700x992_96dpi.jpg",
                        UriKind.Absolute);
                    BitmapImage bti = new BitmapImage();
                    bti.BeginInit();
                    bti.UriSource = uri;
                    bti.EndInit();
                    x_img.Width = bti.PixelWidth;
                    x_img.Height = bti.PixelHeight;
                    x_img.Source = bti;
                    WriteableBitmap wb = new WriteableBitmap(bti);
                    int largeur_numerisation = (wb.Format.BitsPerPixel / 8) * wb.PixelWidth;
                    byte[] tab_pixel = new byte[largeur_numerisation * wb.PixelHeight];
                    wb.CopyPixels(tab_pixel, largeur_numerisation, 0);
                    byte[,] tab_pixel_LH = TransposerTableauPixelEnLH_8bit(tab_pixel, wb.PixelWidth, wb.PixelHeight);
                    byte[,] tab_pixel_LH_filtre = new byte[wb.PixelHeight, wb.PixelWidth];
                    for (int lig = 0; lig < wb.PixelHeight; lig++)
                    {
                        for (int col = 0; col < wb.PixelWidth; col++)
                        {
                            tab_pixel_LH_filtre[lig, col] = 255; // tab_pixel_LH[lig, col];
                        }
                    }
                    //filtrage
                    Filtre filtre = new Filtre(3, new int[]
                    {
                        0, -1, 0,
                        -1, 5, -1,
                        0, -1, 0
                    });
                    for (int lig = 1; lig < wb.PixelHeight - 1; lig++)
                    {
                        for (int col = 1; col < wb.PixelWidth - 1; col++)
                        {
                            int[] voisins = new int[9];
                            voisins[0] = tab_pixel_LH[lig - 1, col - 1];
                            voisins[1] = tab_pixel_LH[lig - 1, col];
                            voisins[2] = tab_pixel_LH[lig - 1, col + 1];
                            voisins[3] = tab_pixel_LH[lig, col - 1];
                            voisins[4] = tab_pixel_LH[lig, col];
                            voisins[5] = tab_pixel_LH[lig, col + 1];
                            voisins[6] = tab_pixel_LH[lig + 1, col - 1];
                            voisins[7] = tab_pixel_LH[lig + 1, col];
                            voisins[8] = tab_pixel_LH[lig + 1, col + 1];
                            byte niveau_filtre = filtre.PixelFiltre(voisins);
                            tab_pixel_LH_filtre[lig, col] = BornerNiveauGris(niveau_filtre);
                        }
                    }
                    //on genere l'image resultante
                    byte[] tab_pixel_res =
                        TransposerTableauPixelEnUnique_8bit(tab_pixel_LH_filtre, wb.PixelWidth, wb.PixelHeight);
                    BitmapSource bti_res = BitmapSource.Create(wb.PixelWidth, wb.PixelHeight, 96.0, 96.0,
                        PixelFormats.Gray8, null, tab_pixel_res, largeur_numerisation);
                    x_img_filtree.Width = bti_res.PixelWidth;
                    x_img_filtree.Height = bti_res.PixelHeight;
                    x_img_filtree.Source = bti_res;
                }
                //filtre rehausseur 3x3 pratt image 8 bits floue
                if (x_cbx_filtre.SelectedIndex == 5)
                {
                    Uri uri = new Uri(
                        "pack://application:,,,/VS2013_06_Rehausseur;component/collection_images/louis_de_funes_8bit_700x992_96dpi_floue.jpg",
                        UriKind.Absolute);
                    BitmapImage bti = new BitmapImage();
                    bti.BeginInit();
                    bti.UriSource = uri;
                    bti.EndInit();
                    x_img.Width = bti.PixelWidth;
                    x_img.Height = bti.PixelHeight;
                    x_img.Source = bti;
                    WriteableBitmap wb = new WriteableBitmap(bti);
                    int largeur_numerisation = (wb.Format.BitsPerPixel / 8) * wb.PixelWidth;
                    byte[] tab_pixel = new byte[largeur_numerisation * wb.PixelHeight];
                    wb.CopyPixels(tab_pixel, largeur_numerisation, 0);
                    byte[,] tab_pixel_LH = TransposerTableauPixelEnLH_8bit(tab_pixel, wb.PixelWidth, wb.PixelHeight);
                    byte[,] tab_pixel_LH_filtre = new byte[wb.PixelHeight, wb.PixelWidth];
                    for (int lig = 0; lig < wb.PixelHeight; lig++)
                    {
                        for (int col = 0; col < wb.PixelWidth; col++)
                        {
                            tab_pixel_LH_filtre[lig, col] = 255;
                        }
                    }
                    //filtrage
                    Filtre filtre = new Filtre(3, new int[]
                    {
                        -1, -1, -1,
                        -1, 9, -1,
                        -1, -1, -1
                    });
                    for (int lig = 1; lig < wb.PixelHeight - 1; lig++)
                    {
                        for (int col = 1; col < wb.PixelWidth - 1; col++)
                        {
                            int[] voisins = new int[9];
                            voisins[0] = tab_pixel_LH[lig - 1, col - 1];
                            voisins[1] = tab_pixel_LH[lig - 1, col];
                            voisins[2] = tab_pixel_LH[lig - 1, col + 1];
                            voisins[3] = tab_pixel_LH[lig, col - 1];
                            voisins[4] = tab_pixel_LH[lig, col];
                            voisins[5] = tab_pixel_LH[lig, col + 1];
                            voisins[6] = tab_pixel_LH[lig + 1, col - 1];
                            voisins[7] = tab_pixel_LH[lig + 1, col];
                            voisins[8] = tab_pixel_LH[lig + 1, col + 1];
                            byte niveau_filtre = filtre.PixelFiltre(voisins);
                            tab_pixel_LH_filtre[lig, col] = BornerNiveauGris(niveau_filtre);
                        }
                    }
                    //on genere l'image resultante
                    byte[] tab_pixel_res =
                        TransposerTableauPixelEnUnique_8bit(tab_pixel_LH_filtre, wb.PixelWidth, wb.PixelHeight);
                    BitmapSource bti_res = BitmapSource.Create(wb.PixelWidth, wb.PixelHeight, 96.0, 96.0,
                        PixelFormats.Gray8, null, tab_pixel_res, largeur_numerisation);
                    x_img_filtree.Width = bti_res.PixelWidth;
                    x_img_filtree.Height = bti_res.PixelHeight;
                    x_img_filtree.Source = bti_res;
                }
            }
        }

        //
        private void ViderControle(Image controle_image, int largeur, int hauteur)
        {
            Uri uri = new Uri("pack://application:,,,/VS2013_06_Rehausseur;component/contenu/image/fond_damier.png",
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

        //borner un niveau de gris entre 0 et 255
        private byte BornerNiveauGris(byte niveau)
        {
            byte niveau_borne = niveau;
            if (niveau > 255)
            {
                niveau_borne = 255;
            }
            if (niveau < 0)
            {
                niveau_borne = 0;
            }
            return niveau_borne;
        }
    } //end class
}