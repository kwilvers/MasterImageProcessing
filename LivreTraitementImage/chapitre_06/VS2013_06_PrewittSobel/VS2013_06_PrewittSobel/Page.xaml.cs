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

namespace VS2013_06_PrewittSobel
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
                //filtre prewitt horizontal 3x3 motif test 
                if (x_cbx_filtre.SelectedIndex == 1)
                {
                    Uri uri = new Uri(
                        "pack://application:,,,/VS2013_06_PrewittSobel;component/collection_images/motif_test_8bit_800x852_96dpi.png",
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
                    x_img.Width = bti.PixelWidth;
                    x_img.Height = bti.PixelHeight;
                    x_img.Source = bti;
                    byte[,] tab_pixel_img_filtree_LH = new byte[wb.PixelHeight, wb.PixelWidth];
                    for (int lig = 0; lig < wb.PixelHeight; lig++)
                    {
                        for (int col = 0; col < wb.PixelWidth; col++)
                        {
                            tab_pixel_img_filtree_LH[lig, col] = tab_pixel_LH[lig, col];
                        }
                    }
                    //filtre prewitt horizontal
                    Filtre filtre = new Filtre(3, new int[]
                    {
                        -1, 0, 1,
                        -1, 0, 1,
                        -1, 0, 1
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
                            tab_pixel_img_filtree_LH[lig, col] = InverserNiveau(niveau_filtre);
                        }
                    }
                    //on genere l'image resultante
                    byte[] tab_pixel_img_filtree =
                        TransposerTableauPixelEnUnique_8bit(tab_pixel_img_filtree_LH, wb.PixelWidth, wb.PixelHeight);
                    BitmapSource bti_res = BitmapSource.Create(wb.PixelWidth, wb.PixelHeight, 96.0, 96.0,
                        PixelFormats.Gray8, null, tab_pixel_img_filtree, largeur_numerisation);
                    x_img_filtree.Width = bti_res.PixelWidth;
                    x_img_filtree.Height = bti_res.PixelHeight;
                    x_img_filtree.Source = bti_res;
                }
                //filtre prewitt vertical 3x3 motif test 
                if (x_cbx_filtre.SelectedIndex == 2)
                {
                    Uri uri = new Uri(
                        "pack://application:,,,/VS2013_06_PrewittSobel;component/collection_images/motif_test_8bit_800x852_96dpi.png",
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
                    x_img.Width = bti.PixelWidth;
                    x_img.Height = bti.PixelHeight;
                    x_img.Source = bti;
                    byte[,] tab_pixel_img_filtree_LH = new byte[wb.PixelHeight, wb.PixelWidth];
                    for (int lig = 0; lig < wb.PixelHeight; lig++)
                    {
                        for (int col = 0; col < wb.PixelWidth; col++)
                        {
                            tab_pixel_img_filtree_LH[lig, col] = tab_pixel_LH[lig, col];
                        }
                    }
                    //filtre prewitt vetical
                    Filtre filtre = new Filtre(3, new int[]
                    {
                        -1, -1, -1,
                        0, 0, 0,
                        1, 1, 1
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
                            tab_pixel_img_filtree_LH[lig, col] = InverserNiveau(niveau_filtre);
                        }
                    }
                    //on genere l'image resultante
                    byte[] tab_pixel_img_filtree =
                        TransposerTableauPixelEnUnique_8bit(tab_pixel_img_filtree_LH, wb.PixelWidth, wb.PixelHeight);
                    BitmapSource bti_res = BitmapSource.Create(wb.PixelWidth, wb.PixelHeight, 96.0, 96.0,
                        PixelFormats.Gray8, null, tab_pixel_img_filtree, largeur_numerisation);
                    x_img_filtree.Width = bti_res.PixelWidth;
                    x_img_filtree.Height = bti_res.PixelHeight;
                    x_img_filtree.Source = bti_res;
                }
                //filtre prewitt complet 3x3 motif test 
                if (x_cbx_filtre.SelectedIndex == 3)
                {
                    Uri uri = new Uri(
                        "pack://application:,,,/VS2013_06_PrewittSobel;component/collection_images/motif_test_8bit_800x852_96dpi.png",
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
                    x_img.Width = bti.PixelWidth;
                    x_img.Height = bti.PixelHeight;
                    x_img.Source = bti;
                    byte[,] tab_pixel_img_filtree_LH = new byte[wb.PixelHeight, wb.PixelWidth];
                    for (int lig = 0; lig < wb.PixelHeight; lig++)
                    {
                        for (int col = 0; col < wb.PixelWidth; col++)
                        {
                            tab_pixel_img_filtree_LH[lig, col] = tab_pixel_LH[lig, col];
                        }
                    }
                    //filtre prewitt complet
                    byte[,] tab_pixels_res_h1 = new byte[wb.PixelHeight, wb.PixelWidth];
                    Filtre filtre_h = new Filtre(3, new int[] {-1, 0, 1, -1, 0, 1, -1, 0, 1});
                    AppliquerFiltre(tab_pixel_LH, filtre_h, tab_pixels_res_h1, wb.PixelWidth, wb.PixelHeight);
                    byte[,] tab_pixels_res_h2 = new byte[wb.PixelHeight, wb.PixelWidth];
                    Filtre filtre_v = new Filtre(3, new int[] {-1, -1, -1, 0, 0, 0, 1, 1, 1});
                    AppliquerFiltre(tab_pixel_LH, filtre_v, tab_pixels_res_h2, wb.PixelWidth, wb.PixelHeight);
                    for (int lig = 1; lig < wb.PixelHeight - 1; lig++)
                    {
                        for (int col = 1; col < wb.PixelWidth - 1; col++)
                        {
                            byte module = (byte) Math.Sqrt(
                                Math.Pow(tab_pixels_res_h1[lig, col], 2) +
                                Math.Pow(tab_pixels_res_h2[lig, col], 2));
                            tab_pixel_img_filtree_LH[lig, col] = InverserNiveau(module);
                        }
                    }
                    //on genere l'image resultante
                    byte[] tab_pixel_img_filtree =
                        TransposerTableauPixelEnUnique_8bit(tab_pixel_img_filtree_LH, wb.PixelWidth, wb.PixelHeight);
                    BitmapSource bti_res = BitmapSource.Create(wb.PixelWidth, wb.PixelHeight, 96.0, 96.0,
                        PixelFormats.Gray8, null, tab_pixel_img_filtree, largeur_numerisation);
                    x_img_filtree.Width = bti_res.PixelWidth;
                    x_img_filtree.Height = bti_res.PixelHeight;
                    x_img_filtree.Source = bti_res;
                }
                //filtre sobel complet 3x3 motif test 
                if (x_cbx_filtre.SelectedIndex == 4)
                {
                    Uri uri = new Uri(
                        "pack://application:,,,/VS2013_06_PrewittSobel;component/collection_images/motif_test_8bit_800x852_96dpi.png",
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
                    x_img.Width = bti.PixelWidth;
                    x_img.Height = bti.PixelHeight;
                    x_img.Source = bti;
                    byte[,] tab_pixel_img_filtree_LH = new byte[wb.PixelHeight, wb.PixelWidth];
                    for (int lig = 0; lig < wb.PixelHeight; lig++)
                    {
                        for (int col = 0; col < wb.PixelWidth; col++)
                        {
                            tab_pixel_img_filtree_LH[lig, col] = tab_pixel_LH[lig, col];
                        }
                    }
                    //filtre sobel complet
                    byte[,] tab_pixels_res_h1 = new byte[wb.PixelHeight, wb.PixelWidth];
                    Filtre filtre_h = new Filtre(3, new int[] {-1, 0, 1, -2, 0, 2, -1, 0, 1});
                    AppliquerFiltre(tab_pixel_LH, filtre_h, tab_pixels_res_h1, wb.PixelWidth, wb.PixelHeight);
                    byte[,] tab_pixels_res_h2 = new byte[wb.PixelHeight, wb.PixelWidth];
                    Filtre filtre_v = new Filtre(3, new int[] {-1, -2, -1, 0, 0, 0, 1, 2, 1});
                    AppliquerFiltre(tab_pixel_LH, filtre_v, tab_pixels_res_h2, wb.PixelWidth, wb.PixelHeight);
                    for (int lig = 1; lig < wb.PixelHeight - 1; lig++)
                    {
                        for (int col = 1; col < wb.PixelWidth - 1; col++)
                        {
                            byte module = (byte) Math.Sqrt(
                                Math.Pow(tab_pixels_res_h1[lig, col], 2) +
                                Math.Pow(tab_pixels_res_h2[lig, col], 2));
                            tab_pixel_img_filtree_LH[lig, col] = InverserNiveau(module);
                        }
                    }
                    //on genere l'image resultante
                    byte[] tab_pixel_img_filtree =
                        TransposerTableauPixelEnUnique_8bit(tab_pixel_img_filtree_LH, wb.PixelWidth, wb.PixelHeight);
                    BitmapSource bti_res = BitmapSource.Create(wb.PixelWidth, wb.PixelHeight, 96.0, 96.0,
                        PixelFormats.Gray8, null, tab_pixel_img_filtree, largeur_numerisation);
                    x_img_filtree.Width = bti_res.PixelWidth;
                    x_img_filtree.Height = bti_res.PixelHeight;
                    x_img_filtree.Source = bti_res;
                }
                //filtre sobel complet 3x3 sur image
                if (x_cbx_filtre.SelectedIndex == 5)
                {
                    Uri uri = new Uri(
                        "pack://application:,,,/VS2013_06_PrewittSobel;component/collection_images/louis_de_funes_site_8bit_800x428_96dpi.jpg",
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
                    x_img.Width = bti.PixelWidth;
                    x_img.Height = bti.PixelHeight;
                    x_img.Source = bti;
                    byte[,] tab_pixel_img_filtree_LH = new byte[wb.PixelHeight, wb.PixelWidth];
                    for (int lig = 0; lig < wb.PixelHeight; lig++)
                    {
                        for (int col = 0; col < wb.PixelWidth; col++)
                        {
                            tab_pixel_img_filtree_LH[lig, col] = tab_pixel_LH[lig, col];
                        }
                    }
                    //filtre sobel complet
                    byte[,] tab_pixels_res_h1 = new byte[wb.PixelHeight, wb.PixelWidth];
                    Filtre filtre_h1 = new Filtre(3, new int[] {-1, 0, 1, -2, 0, 2, -1, 0, 1});
                    AppliquerFiltre(tab_pixel_LH, filtre_h1, tab_pixels_res_h1, wb.PixelWidth, wb.PixelHeight);
                    byte[,] tab_pixels_res_h2 = new byte[wb.PixelHeight, wb.PixelWidth];
                    Filtre filtre_h2 = new Filtre(3, new int[] {1, 0, -1, 2, 0, -2, 1, 0, -1});
                    AppliquerFiltre(tab_pixel_LH, filtre_h2, tab_pixels_res_h2, wb.PixelWidth, wb.PixelHeight);

                    byte[,] tab_pixels_res_v1 = new byte[wb.PixelHeight, wb.PixelWidth];
                    Filtre filtre_v1 = new Filtre(3, new int[] {-1, -2, -1, 0, 0, 0, 1, 2, 1});
                    AppliquerFiltre(tab_pixel_LH, filtre_v1, tab_pixels_res_v1, wb.PixelWidth, wb.PixelHeight);
                    byte[,] tab_pixels_res_v2 = new byte[wb.PixelHeight, wb.PixelWidth];
                    Filtre filtre_v2 = new Filtre(3, new int[] {1, 2, 1, 0, 0, 0, -1, -2, -1});
                    AppliquerFiltre(tab_pixel_LH, filtre_v2, tab_pixels_res_v2, wb.PixelWidth, wb.PixelHeight);

                    for (int lig = 1; lig < wb.PixelHeight - 1; lig++)
                    {
                        for (int col = 1; col < wb.PixelWidth - 1; col++)
                        {
                            byte module = (byte) Math.Sqrt(
                                Math.Pow(tab_pixels_res_h1[lig, col], 2) +
                                Math.Pow(tab_pixels_res_h2[lig, col], 2) +
                                Math.Pow(tab_pixels_res_v1[lig, col], 2) +
                                Math.Pow(tab_pixels_res_v2[lig, col], 2)
                            );
                            tab_pixel_img_filtree_LH[lig, col] = InverserNiveau(module);
                        }
                    }
                    //on genere l'image resultante
                    byte[] tab_pixel_img_filtree =
                        TransposerTableauPixelEnUnique_8bit(tab_pixel_img_filtree_LH, wb.PixelWidth, wb.PixelHeight);
                    BitmapSource bti_res = BitmapSource.Create(wb.PixelWidth, wb.PixelHeight, 96.0, 96.0,
                        PixelFormats.Gray8, null, tab_pixel_img_filtree, largeur_numerisation);
                    x_img_filtree.Width = bti_res.PixelWidth;
                    x_img_filtree.Height = bti_res.PixelHeight;
                    x_img_filtree.Source = bti_res;
                }
            }
        }

        //
        private void ViderControle(Image controle_image, int largeur, int hauteur)
        {
            Uri uri = new Uri("pack://application:,,,/VS2013_06_PrewittSobel;component/contenu/image/fond_damier.png",
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