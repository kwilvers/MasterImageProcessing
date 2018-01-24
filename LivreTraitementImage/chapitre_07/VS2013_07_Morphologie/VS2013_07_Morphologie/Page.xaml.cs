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

namespace VS2013_07_Morphologie
{
    /// <summary>
    /// Logique d'interaction pour Page.xaml
    /// </summary>
    public partial class Page : UserControl
    {
        //donnees
        private string RC = Environment.NewLine;

        private bool v_fen_charge = false;
        private byte v_seuillage = 0;
        private WriteableBitmap v_wb_nvg = null;
        private WriteableBitmap v_wb_seuillee = null;

        public Page()
        {
            InitializeComponent();
        }

        //controle Loaded
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                v_fen_charge = true;
                Uri uri = new Uri(
                    "pack://application:,,,/VS2013_07_Morphologie;component/collection_images/image_pour_structurant_32bit.jpg",
                    UriKind.Absolute);
                BitmapImage bti = new BitmapImage();
                bti.BeginInit();
                bti.UriSource = uri;
                bti.EndInit();
                x_img_couleur.Stretch = Stretch.None;
                x_img_couleur.Width = bti.PixelWidth;
                x_img_couleur.Height = bti.PixelHeight;
                x_img_couleur.Source = bti;
                WriteableBitmap wb_cl = new WriteableBitmap(bti);
                string infos_couleur = "LxH: " + wb_cl.PixelWidth.ToString() + " par " + wb_cl.PixelHeight.ToString() +
                                       " pixels ";
                infos_couleur += "avec " + wb_cl.Format.BitsPerPixel.ToString() + " bits par pixel ";
                infos_couleur += "et avec une résolution de " + wb_cl.DpiX.ToString() + " dpi ";
                x_tbl_infos_img_couleur.Text = infos_couleur;
                byte[] tab_pixel_couleur =
                    new byte[(wb_cl.Format.BitsPerPixel / 8) * wb_cl.PixelWidth * wb_cl.PixelHeight];
                wb_cl.CopyPixels(tab_pixel_couleur, (wb_cl.Format.BitsPerPixel / 8) * wb_cl.PixelWidth, 0);
                v_wb_nvg = new WriteableBitmap(wb_cl.PixelWidth, wb_cl.PixelHeight, 96.0, 96.0, PixelFormats.Gray8,
                    null);
                string infos_niveau_gris = "LxH: " + v_wb_nvg.PixelWidth.ToString() + " par " +
                                           v_wb_nvg.PixelHeight.ToString() + " pixels ";
                infos_niveau_gris += "avec " + v_wb_nvg.Format.BitsPerPixel.ToString() + " bits par pixel ";
                infos_niveau_gris += "et avec une résolution de " + v_wb_nvg.DpiX.ToString() + " dpi ";
                x_tbl_infos_img_niv_gris.Text = infos_niveau_gris;
                byte[] tab_pixel_nv_gris =
                    new byte[(v_wb_nvg.Format.BitsPerPixel / 8) * v_wb_nvg.PixelWidth * v_wb_nvg.PixelHeight];
                v_wb_nvg.CopyPixels(tab_pixel_nv_gris, (v_wb_nvg.Format.BitsPerPixel / 8) * v_wb_nvg.PixelWidth, 0);
                int cpt = 0;
                for (int xx = 0; xx < tab_pixel_nv_gris.Length; xx++)
                {
                    //on recupere un pixel BGRA couleur
                    byte comp_b = tab_pixel_couleur[cpt + 0];
                    byte comp_g = tab_pixel_couleur[cpt + 1];
                    byte comp_r = tab_pixel_couleur[cpt + 2];
                    byte comp_a = tab_pixel_couleur[cpt + 3];
                    Color couleur = Color.FromArgb(comp_a, comp_r, comp_g, comp_b);
                    tab_pixel_nv_gris[xx] = ConvertirCouleurNiveauDeGrisAvec709(couleur);
                    cpt += 4;
                }
                v_wb_nvg.Lock();
                v_wb_nvg.WritePixels(new Int32Rect(0, 0, v_wb_nvg.PixelWidth, v_wb_nvg.PixelHeight), tab_pixel_nv_gris,
                    (v_wb_nvg.Format.BitsPerPixel / 8) * v_wb_nvg.PixelWidth, 0);
                v_wb_nvg.Unlock();
                x_img_niveau_gris.Stretch = Stretch.None;
                x_img_niveau_gris.Width = v_wb_nvg.PixelWidth;
                x_img_niveau_gris.Height = v_wb_nvg.PixelHeight;
                x_img_niveau_gris.Source = v_wb_nvg;
                x_slider_seuillage.IsEnabled = true;
                v_seuillage = (byte) x_slider_seuillage.Value;
                x_tbl_seuillage.Text = "seuillage à " + v_seuillage.ToString();
                v_wb_seuillee = new WriteableBitmap(wb_cl.PixelWidth, wb_cl.PixelHeight, 96.0, 96.0, PixelFormats.Gray8,
                    null);
                v_wb_seuillee.Lock();
                v_wb_seuillee.WritePixels(new Int32Rect(0, 0, v_wb_nvg.PixelWidth, v_wb_nvg.PixelHeight),
                    tab_pixel_nv_gris,
                    (v_wb_nvg.Format.BitsPerPixel / 8) * v_wb_nvg.PixelWidth, 0);
                v_wb_seuillee.Unlock();
                BinariserImage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //
        private byte ConvertirCouleurNiveauDeGrisAvec709(Color couleur)
        {
            byte niveau_gris = 0;
            //recommandation CIE 709
            //Gris = 0.2125 * R + 0.7154 * G + 0.0721 * B
            niveau_gris = (byte) (0.2125 * (double) couleur.R + 0.7154 * (double) couleur.G +
                                  0.0721 * (double) couleur.B);
            return niveau_gris;
        }

        //valeur changee du cuurseur de la glissiere
        private void x_slider_seuillage_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (v_fen_charge == true)
            {
                v_seuillage = (byte) x_slider_seuillage.Value;
                x_tbl_seuillage.Text = "seuillage à " + v_seuillage.ToString();
                BinariserImage();
            }
        }

        //on binarise l'image en noir et blanc
        private void BinariserImage()
        {
            byte[] tab_pixel_nv_gris =
                new byte[(v_wb_nvg.Format.BitsPerPixel / 8) * v_wb_nvg.PixelWidth * v_wb_nvg.PixelHeight];
            v_wb_nvg.CopyPixels(tab_pixel_nv_gris, (v_wb_nvg.Format.BitsPerPixel / 8) * v_wb_nvg.PixelWidth, 0);
            int cpt_pixel_noir = 0;
            for (int xx = 0; xx < tab_pixel_nv_gris.Length; xx++)
            {
                if (tab_pixel_nv_gris[xx] >= v_seuillage)
                {
                    tab_pixel_nv_gris[xx] = 255;
                }
                else
                {
                    tab_pixel_nv_gris[xx] = 0;
                    cpt_pixel_noir++;
                }
            }
            x_tbl_seuillee_pixel_noir.Text = "pixels noirs = " + cpt_pixel_noir.ToString();
            v_wb_seuillee.Lock();
            v_wb_seuillee.WritePixels(new Int32Rect(0, 0, v_wb_seuillee.PixelWidth, v_wb_seuillee.PixelHeight),
                tab_pixel_nv_gris,
                (v_wb_seuillee.Format.BitsPerPixel / 8) * v_wb_seuillee.PixelWidth, 0);
            v_wb_seuillee.Unlock();
            x_img_seuillee.Stretch = Stretch.None;
            x_img_seuillee.Width = v_wb_seuillee.PixelWidth;
            x_img_seuillee.Height = v_wb_seuillee.PixelHeight;
            x_img_seuillee.Source = v_wb_seuillee;
        }

        //------------------------------------------------------------------------------------------
        //DILATATION structurant CROIX 
        //------------------------------------------------------------------------------------------
        //btn dilatation avec le structurant CROIX
        private void x_btn_dilatation_croix_Click(object sender, RoutedEventArgs e)
        {
            byte[] tab_pixel_binaire = new byte[(v_wb_seuillee.Format.BitsPerPixel / 8) * v_wb_seuillee.PixelWidth *
                                                v_wb_seuillee.PixelHeight];
            v_wb_seuillee.CopyPixels(tab_pixel_binaire,
                (v_wb_seuillee.Format.BitsPerPixel / 8) * v_wb_seuillee.PixelWidth, 0);
            //on passe le tableau binaire en LH
            byte[,] tab_pixel_binaire_LH = TransposerTableauPixelEnLH_8bit(tab_pixel_binaire, v_wb_seuillee.PixelWidth,
                v_wb_seuillee.PixelHeight);
            //on initialise le tableau binaire LH resultant avec des pixels blancs
            byte[,] tab_pixel_binaire_LH_res = new byte[v_wb_seuillee.PixelHeight, v_wb_seuillee.PixelWidth];
            for (int lig = 0; lig < v_wb_seuillee.PixelHeight; lig++)
            {
                for (int col = 0; col < v_wb_seuillee.PixelWidth; col++)
                {
                    tab_pixel_binaire_LH_res[lig, col] = 255;
                }
            }
            //on dilate
            ElementStructurant structurant = new ElementStructurant(ElementStructurant.TypeStructurant.croix);
            int pixel_noir = 0;
            for (int lig = 1; lig < v_wb_seuillee.PixelHeight - 1; lig++)
            {
                for (int col = 1; col < v_wb_seuillee.PixelWidth - 1; col++)
                {
                    byte[,] tab_3x3 = new byte[3, 3];
                    tab_3x3[0, 0] = tab_pixel_binaire_LH[lig - 1, col - 1];
                    tab_3x3[0, 1] = tab_pixel_binaire_LH[lig - 1, col];
                    tab_3x3[0, 2] = tab_pixel_binaire_LH[lig - 1, col + 1];
                    tab_3x3[1, 0] = tab_pixel_binaire_LH[lig, col - 1];
                    tab_3x3[1, 1] = tab_pixel_binaire_LH[lig, col];
                    tab_3x3[1, 2] = tab_pixel_binaire_LH[lig, col + 1];
                    tab_3x3[2, 0] = tab_pixel_binaire_LH[lig + 1, col - 1];
                    tab_3x3[2, 1] = tab_pixel_binaire_LH[lig + 1, col];
                    tab_3x3[2, 2] = tab_pixel_binaire_LH[lig + 1, col + 1];
                    byte nouv_niv = structurant.AppliquerDilatation(tab_3x3);
                    if (nouv_niv == 0)
                    {
                        pixel_noir++;
                    }
                    tab_pixel_binaire_LH_res[lig, col] = nouv_niv;
                }
            }
            x_tbl_img_dilatation_pxn.Text = "pixels noirs = " + pixel_noir.ToString();
            byte[] tab_pixel_binaire_res = TransposerTableauPixelEnUnique_8bit(tab_pixel_binaire_LH_res,
                v_wb_seuillee.PixelWidth, v_wb_seuillee.PixelHeight);
            BitmapSource bti_img_resultante = BitmapSource.Create(v_wb_seuillee.PixelWidth, v_wb_seuillee.PixelHeight,
                96.0, 96.0,
                PixelFormats.Gray8, null, tab_pixel_binaire_res, v_wb_seuillee.PixelWidth);
            x_img_dilatation_croix.Stretch = Stretch.None;
            x_img_dilatation_croix.Width = bti_img_resultante.PixelWidth;
            x_img_dilatation_croix.Height = bti_img_resultante.PixelHeight;
            x_img_dilatation_croix.Source = bti_img_resultante;
        }

        //btn resulat de la difference dilatation et image binaire
        private void x_btn_dilatation_croix_diff_Click(object sender, RoutedEventArgs e)
        {
            byte[] tab_pixel_binaire = new byte[(v_wb_seuillee.Format.BitsPerPixel / 8) * v_wb_seuillee.PixelWidth *
                                                v_wb_seuillee.PixelHeight];
            v_wb_seuillee.CopyPixels(tab_pixel_binaire,
                (v_wb_seuillee.Format.BitsPerPixel / 8) * v_wb_seuillee.PixelWidth, 0);
            //on passe le tableau binaire en LH
            byte[,] tab_pixel_binaire_LH = TransposerTableauPixelEnLH_8bit(tab_pixel_binaire, v_wb_seuillee.PixelWidth,
                v_wb_seuillee.PixelHeight);
            //on initialise le tableau binaire LH resultant avec des pixels blancs
            byte[,] tab_pixel_binaire_LH_res = new byte[v_wb_seuillee.PixelHeight, v_wb_seuillee.PixelWidth];
            for (int lig = 0; lig < v_wb_seuillee.PixelHeight; lig++)
            {
                for (int col = 0; col < v_wb_seuillee.PixelWidth; col++)
                {
                    tab_pixel_binaire_LH_res[lig, col] = 255;
                }
            }
            //on dilate
            ElementStructurant structurant = new ElementStructurant(ElementStructurant.TypeStructurant.croix);
            for (int lig = 1; lig < v_wb_seuillee.PixelHeight - 1; lig++)
            {
                for (int col = 1; col < v_wb_seuillee.PixelWidth - 1; col++)
                {
                    byte[,] tab_3x3 = new byte[3, 3];
                    tab_3x3[0, 0] = tab_pixel_binaire_LH[lig - 1, col - 1];
                    tab_3x3[0, 1] = tab_pixel_binaire_LH[lig - 1, col];
                    tab_3x3[0, 2] = tab_pixel_binaire_LH[lig - 1, col + 1];
                    tab_3x3[1, 0] = tab_pixel_binaire_LH[lig, col - 1];
                    tab_3x3[1, 1] = tab_pixel_binaire_LH[lig, col];
                    tab_3x3[1, 2] = tab_pixel_binaire_LH[lig, col + 1];
                    tab_3x3[2, 0] = tab_pixel_binaire_LH[lig + 1, col - 1];
                    tab_3x3[2, 1] = tab_pixel_binaire_LH[lig + 1, col];
                    tab_3x3[2, 2] = tab_pixel_binaire_LH[lig + 1, col + 1];
                    byte nouv_niv = structurant.AppliquerDilatation(tab_3x3);
                    tab_pixel_binaire_LH_res[lig, col] = nouv_niv;
                }
            }
            //difference entre image dilatee et image binaire
            byte[,] tab_diff = new byte[v_wb_seuillee.PixelHeight, v_wb_seuillee.PixelWidth];
            Array.Copy(tab_pixel_binaire_LH_res, tab_diff, tab_pixel_binaire_LH_res.Length);
            for (int lig = 1; lig < v_wb_seuillee.PixelHeight - 1; lig++)
            {
                for (int col = 1; col < v_wb_seuillee.PixelWidth - 1; col++)
                {
                    if (tab_pixel_binaire_LH_res[lig, col] == 0 && tab_pixel_binaire_LH[lig, col] == 0)
                    {
                        tab_diff[lig, col] = 255;
                    }
                }
            }
            int pixel_noir = 0;
            for (int lig = 1; lig < v_wb_seuillee.PixelHeight - 1; lig++)
            {
                for (int col = 1; col < v_wb_seuillee.PixelWidth - 1; col++)
                {
                    if (tab_diff[lig, col] == 0)
                    {
                        pixel_noir++;
                    }
                }
            }
            x_tbl_img_dilatation_diff_pxn.Text = "pixels noirs = " + pixel_noir.ToString();
            byte[] tab_pixel_binaire_res =
                TransposerTableauPixelEnUnique_8bit(tab_diff, v_wb_seuillee.PixelWidth, v_wb_seuillee.PixelHeight);
            BitmapSource bti_img_resultante = BitmapSource.Create(v_wb_seuillee.PixelWidth, v_wb_seuillee.PixelHeight,
                96.0, 96.0,
                PixelFormats.Gray8, null, tab_pixel_binaire_res, v_wb_seuillee.PixelWidth);
            x_img_dilatation_croix_diff.Stretch = Stretch.None;
            x_img_dilatation_croix_diff.Width = bti_img_resultante.PixelWidth;
            x_img_dilatation_croix_diff.Height = bti_img_resultante.PixelHeight;
            x_img_dilatation_croix_diff.Source = bti_img_resultante;
        }

        //------------------------------------------------------------------------------------------
        //DILATATION structurant CARRE 
        //------------------------------------------------------------------------------------------
        //btn dilatation avec le structurant CARRE
        private void x_btn_dilatation_carre_Click(object sender, RoutedEventArgs e)
        {
            byte[] tab_pixel_binaire = new byte[(v_wb_seuillee.Format.BitsPerPixel / 8) * v_wb_seuillee.PixelWidth *
                                                v_wb_seuillee.PixelHeight];
            v_wb_seuillee.CopyPixels(tab_pixel_binaire,
                (v_wb_seuillee.Format.BitsPerPixel / 8) * v_wb_seuillee.PixelWidth, 0);
            //on passe le tableau binaire en LH
            byte[,] tab_pixel_binaire_LH = TransposerTableauPixelEnLH_8bit(tab_pixel_binaire, v_wb_seuillee.PixelWidth,
                v_wb_seuillee.PixelHeight);
            //on initialise le tableau binaire LH resultant avec des pixels blancs
            byte[,] tab_pixel_binaire_LH_res = new byte[v_wb_seuillee.PixelHeight, v_wb_seuillee.PixelWidth];
            for (int lig = 0; lig < v_wb_seuillee.PixelHeight; lig++)
            {
                for (int col = 0; col < v_wb_seuillee.PixelWidth; col++)
                {
                    tab_pixel_binaire_LH_res[lig, col] = 255;
                }
            }
            //on dilate
            ElementStructurant structurant = new ElementStructurant(ElementStructurant.TypeStructurant.carre);
            int pixel_noir = 0;
            for (int lig = 1; lig < v_wb_seuillee.PixelHeight - 1; lig++)
            {
                for (int col = 1; col < v_wb_seuillee.PixelWidth - 1; col++)
                {
                    byte[,] tab_3x3 = new byte[3, 3];
                    tab_3x3[0, 0] = tab_pixel_binaire_LH[lig - 1, col - 1];
                    tab_3x3[0, 1] = tab_pixel_binaire_LH[lig - 1, col];
                    tab_3x3[0, 2] = tab_pixel_binaire_LH[lig - 1, col + 1];
                    tab_3x3[1, 0] = tab_pixel_binaire_LH[lig, col - 1];
                    tab_3x3[1, 1] = tab_pixel_binaire_LH[lig, col];
                    tab_3x3[1, 2] = tab_pixel_binaire_LH[lig, col + 1];
                    tab_3x3[2, 0] = tab_pixel_binaire_LH[lig + 1, col - 1];
                    tab_3x3[2, 1] = tab_pixel_binaire_LH[lig + 1, col];
                    tab_3x3[2, 2] = tab_pixel_binaire_LH[lig + 1, col + 1];
                    byte nouv_niv = structurant.AppliquerDilatation(tab_3x3);
                    if (nouv_niv == 0)
                    {
                        pixel_noir++;
                    }
                    tab_pixel_binaire_LH_res[lig, col] = nouv_niv;
                }
            }
            x_tbl_img_dilatation_carre_pxn.Text = "pixels noirs = " + pixel_noir.ToString();
            byte[] tab_pixel_binaire_res = TransposerTableauPixelEnUnique_8bit(tab_pixel_binaire_LH_res,
                v_wb_seuillee.PixelWidth, v_wb_seuillee.PixelHeight);
            BitmapSource bti_img_resultante = BitmapSource.Create(v_wb_seuillee.PixelWidth, v_wb_seuillee.PixelHeight,
                96.0, 96.0,
                PixelFormats.Gray8, null, tab_pixel_binaire_res, v_wb_seuillee.PixelWidth);
            x_img_dilatation_carre.Stretch = Stretch.None;
            x_img_dilatation_carre.Width = bti_img_resultante.PixelWidth;
            x_img_dilatation_carre.Height = bti_img_resultante.PixelHeight;
            x_img_dilatation_carre.Source = bti_img_resultante;
        }

        //btn resulat de la difference dilatation et image binaire
        private void x_btn_dilatation_carre_diff_Click(object sender, RoutedEventArgs e)
        {
            byte[] tab_pixel_binaire = new byte[(v_wb_seuillee.Format.BitsPerPixel / 8) * v_wb_seuillee.PixelWidth *
                                                v_wb_seuillee.PixelHeight];
            v_wb_seuillee.CopyPixels(tab_pixel_binaire,
                (v_wb_seuillee.Format.BitsPerPixel / 8) * v_wb_seuillee.PixelWidth, 0);
            //on passe le tableau binaire en LH
            byte[,] tab_pixel_binaire_LH = TransposerTableauPixelEnLH_8bit(tab_pixel_binaire, v_wb_seuillee.PixelWidth,
                v_wb_seuillee.PixelHeight);
            //on initialise le tableau binaire LH resultant avec des pixels blancs
            byte[,] tab_pixel_binaire_LH_res = new byte[v_wb_seuillee.PixelHeight, v_wb_seuillee.PixelWidth];
            for (int lig = 0; lig < v_wb_seuillee.PixelHeight; lig++)
            {
                for (int col = 0; col < v_wb_seuillee.PixelWidth; col++)
                {
                    tab_pixel_binaire_LH_res[lig, col] = 255;
                }
            }
            //on dilate
            ElementStructurant structurant = new ElementStructurant(ElementStructurant.TypeStructurant.carre);
            for (int lig = 1; lig < v_wb_seuillee.PixelHeight - 1; lig++)
            {
                for (int col = 1; col < v_wb_seuillee.PixelWidth - 1; col++)
                {
                    byte[,] tab_3x3 = new byte[3, 3];
                    tab_3x3[0, 0] = tab_pixel_binaire_LH[lig - 1, col - 1];
                    tab_3x3[0, 1] = tab_pixel_binaire_LH[lig - 1, col];
                    tab_3x3[0, 2] = tab_pixel_binaire_LH[lig - 1, col + 1];
                    tab_3x3[1, 0] = tab_pixel_binaire_LH[lig, col - 1];
                    tab_3x3[1, 1] = tab_pixel_binaire_LH[lig, col];
                    tab_3x3[1, 2] = tab_pixel_binaire_LH[lig, col + 1];
                    tab_3x3[2, 0] = tab_pixel_binaire_LH[lig + 1, col - 1];
                    tab_3x3[2, 1] = tab_pixel_binaire_LH[lig + 1, col];
                    tab_3x3[2, 2] = tab_pixel_binaire_LH[lig + 1, col + 1];
                    byte nouv_niv = structurant.AppliquerDilatation(tab_3x3);
                    tab_pixel_binaire_LH_res[lig, col] = nouv_niv;
                }
            }
            //difference entre image dilatee et image binaire
            byte[,] tab_diff = new byte[v_wb_seuillee.PixelHeight, v_wb_seuillee.PixelWidth];
            Array.Copy(tab_pixel_binaire_LH_res, tab_diff, tab_pixel_binaire_LH_res.Length);
            for (int lig = 1; lig < v_wb_seuillee.PixelHeight - 1; lig++)
            {
                for (int col = 1; col < v_wb_seuillee.PixelWidth - 1; col++)
                {
                    if (tab_pixel_binaire_LH_res[lig, col] == 0 && tab_pixel_binaire_LH[lig, col] == 0)
                    {
                        tab_diff[lig, col] = 255;
                    }
                }
            }
            int pixel_noir = 0;
            for (int lig = 1; lig < v_wb_seuillee.PixelHeight - 1; lig++)
            {
                for (int col = 1; col < v_wb_seuillee.PixelWidth - 1; col++)
                {
                    if (tab_diff[lig, col] == 0)
                    {
                        pixel_noir++;
                    }
                }
            }
            x_tbl_img_dilatation_carre_pxn_diff.Text = "pixels noirs = " + pixel_noir.ToString();
            byte[] tab_pixel_binaire_res =
                TransposerTableauPixelEnUnique_8bit(tab_diff, v_wb_seuillee.PixelWidth, v_wb_seuillee.PixelHeight);
            BitmapSource bti_img_resultante = BitmapSource.Create(v_wb_seuillee.PixelWidth, v_wb_seuillee.PixelHeight,
                96.0, 96.0,
                PixelFormats.Gray8, null, tab_pixel_binaire_res, v_wb_seuillee.PixelWidth);
            x_img_dilatation_carre_diff.Stretch = Stretch.None;
            x_img_dilatation_carre_diff.Width = bti_img_resultante.PixelWidth;
            x_img_dilatation_carre_diff.Height = bti_img_resultante.PixelHeight;
            x_img_dilatation_carre_diff.Source = bti_img_resultante;
        }

        //------------------------------------------------------------------------------------------
        //EROSION structurant CROIX 
        //------------------------------------------------------------------------------------------
        //btn erosion avec le structurant CROIX
        private void x_btn_erosion_croix_Click(object sender, RoutedEventArgs e)
        {
            byte[] tab_pixel_binaire = new byte[(v_wb_seuillee.Format.BitsPerPixel / 8) * v_wb_seuillee.PixelWidth *
                                                v_wb_seuillee.PixelHeight];
            v_wb_seuillee.CopyPixels(tab_pixel_binaire,
                (v_wb_seuillee.Format.BitsPerPixel / 8) * v_wb_seuillee.PixelWidth, 0);
            //on passe le tableau binaire en LH
            byte[,] tab_pixel_binaire_LH = TransposerTableauPixelEnLH_8bit(tab_pixel_binaire, v_wb_seuillee.PixelWidth,
                v_wb_seuillee.PixelHeight);
            //on initialise le tableau binaire LH resultant avec des pixels blancs
            byte[,] tab_pixel_binaire_LH_res = new byte[v_wb_seuillee.PixelHeight, v_wb_seuillee.PixelWidth];
            for (int lig = 0; lig < v_wb_seuillee.PixelHeight; lig++)
            {
                for (int col = 0; col < v_wb_seuillee.PixelWidth; col++)
                {
                    tab_pixel_binaire_LH_res[lig, col] = 255;
                }
            }
            //on erode
            ElementStructurant structurant = new ElementStructurant(ElementStructurant.TypeStructurant.croix);
            int pixel_noir = 0;
            for (int lig = 1; lig < v_wb_seuillee.PixelHeight - 1; lig++)
            {
                for (int col = 1; col < v_wb_seuillee.PixelWidth - 1; col++)
                {
                    byte[,] tab_3x3 = new byte[3, 3];
                    tab_3x3[0, 0] = tab_pixel_binaire_LH[lig - 1, col - 1];
                    tab_3x3[0, 1] = tab_pixel_binaire_LH[lig - 1, col];
                    tab_3x3[0, 2] = tab_pixel_binaire_LH[lig - 1, col + 1];
                    tab_3x3[1, 0] = tab_pixel_binaire_LH[lig, col - 1];
                    tab_3x3[1, 1] = tab_pixel_binaire_LH[lig, col];
                    tab_3x3[1, 2] = tab_pixel_binaire_LH[lig, col + 1];
                    tab_3x3[2, 0] = tab_pixel_binaire_LH[lig + 1, col - 1];
                    tab_3x3[2, 1] = tab_pixel_binaire_LH[lig + 1, col];
                    tab_3x3[2, 2] = tab_pixel_binaire_LH[lig + 1, col + 1];
                    byte nouv_niv = structurant.AppliquerErosion(tab_3x3);
                    if (nouv_niv == 0)
                    {
                        pixel_noir++;
                    }
                    tab_pixel_binaire_LH_res[lig, col] = nouv_niv;
                }
            }
            x_tbl_img_erosion_pxn.Text = "pixels noirs = " + pixel_noir.ToString();
            byte[] tab_pixel_binaire_res = TransposerTableauPixelEnUnique_8bit(tab_pixel_binaire_LH_res,
                v_wb_seuillee.PixelWidth, v_wb_seuillee.PixelHeight);
            BitmapSource bti_img_resultante = BitmapSource.Create(v_wb_seuillee.PixelWidth, v_wb_seuillee.PixelHeight,
                96.0, 96.0,
                PixelFormats.Gray8, null, tab_pixel_binaire_res, v_wb_seuillee.PixelWidth);
            x_img_erosion_croix.Stretch = Stretch.None;
            x_img_erosion_croix.Width = bti_img_resultante.PixelWidth;
            x_img_erosion_croix.Height = bti_img_resultante.PixelHeight;
            x_img_erosion_croix.Source = bti_img_resultante;
        }

        //btn difference entre erosion croix et image binaire
        private void x_btn_erosion_croix_diff_Click(object sender, RoutedEventArgs e)
        {
            byte[] tab_pixel_binaire = new byte[(v_wb_seuillee.Format.BitsPerPixel / 8) * v_wb_seuillee.PixelWidth *
                                                v_wb_seuillee.PixelHeight];
            v_wb_seuillee.CopyPixels(tab_pixel_binaire,
                (v_wb_seuillee.Format.BitsPerPixel / 8) * v_wb_seuillee.PixelWidth, 0);
            //on passe le tableau binaire en LH
            byte[,] tab_pixel_binaire_LH = TransposerTableauPixelEnLH_8bit(tab_pixel_binaire, v_wb_seuillee.PixelWidth,
                v_wb_seuillee.PixelHeight);
            //on initialise le tableau binaire LH resultant avec des pixels blancs
            byte[,] tab_pixel_binaire_LH_res = new byte[v_wb_seuillee.PixelHeight, v_wb_seuillee.PixelWidth];
            for (int lig = 0; lig < v_wb_seuillee.PixelHeight; lig++)
            {
                for (int col = 0; col < v_wb_seuillee.PixelWidth; col++)
                {
                    tab_pixel_binaire_LH_res[lig, col] = 255;
                }
            }
            //on erode
            ElementStructurant structurant = new ElementStructurant(ElementStructurant.TypeStructurant.croix);
            int pixel_noir = 0;
            for (int lig = 1; lig < v_wb_seuillee.PixelHeight - 1; lig++)
            {
                for (int col = 1; col < v_wb_seuillee.PixelWidth - 1; col++)
                {
                    byte[,] tab_3x3 = new byte[3, 3];
                    tab_3x3[0, 0] = tab_pixel_binaire_LH[lig - 1, col - 1];
                    tab_3x3[0, 1] = tab_pixel_binaire_LH[lig - 1, col];
                    tab_3x3[0, 2] = tab_pixel_binaire_LH[lig - 1, col + 1];
                    tab_3x3[1, 0] = tab_pixel_binaire_LH[lig, col - 1];
                    tab_3x3[1, 1] = tab_pixel_binaire_LH[lig, col];
                    tab_3x3[1, 2] = tab_pixel_binaire_LH[lig, col + 1];
                    tab_3x3[2, 0] = tab_pixel_binaire_LH[lig + 1, col - 1];
                    tab_3x3[2, 1] = tab_pixel_binaire_LH[lig + 1, col];
                    tab_3x3[2, 2] = tab_pixel_binaire_LH[lig + 1, col + 1];
                    byte nouv_niv = structurant.AppliquerErosion(tab_3x3);
                    tab_pixel_binaire_LH_res[lig, col] = nouv_niv;
                }
            }
            //difference entre image erodee et image binaire
            byte[,] tab_diff = new byte[v_wb_seuillee.PixelHeight, v_wb_seuillee.PixelWidth];
            Array.Copy(tab_pixel_binaire_LH_res, tab_diff, tab_pixel_binaire_LH_res.Length);
            for (int lig = 1; lig < v_wb_seuillee.PixelHeight - 1; lig++)
            {
                for (int col = 1; col < v_wb_seuillee.PixelWidth - 1; col++)
                {
                    if (tab_pixel_binaire_LH_res[lig, col] == 255 && tab_pixel_binaire_LH[lig, col] == 0)
                    {
                        tab_diff[lig, col] = 0;
                    }
                    if (tab_pixel_binaire_LH_res[lig, col] == 0 && tab_pixel_binaire_LH[lig, col] == 0)
                    {
                        tab_diff[lig, col] = 255;
                    }
                }
            }
            for (int lig = 1; lig < v_wb_seuillee.PixelHeight - 1; lig++)
            {
                for (int col = 1; col < v_wb_seuillee.PixelWidth - 1; col++)
                {
                    if (tab_diff[lig, col] == 0)
                    {
                        pixel_noir++;
                    }
                }
            }
            x_tbl_img_erosion_diff_pxn.Text = "pixels noirs = " + pixel_noir.ToString();
            byte[] tab_pixel_binaire_res =
                TransposerTableauPixelEnUnique_8bit(tab_diff, v_wb_seuillee.PixelWidth, v_wb_seuillee.PixelHeight);
            BitmapSource bti_img_resultante = BitmapSource.Create(v_wb_seuillee.PixelWidth, v_wb_seuillee.PixelHeight,
                96.0, 96.0,
                PixelFormats.Gray8, null, tab_pixel_binaire_res, v_wb_seuillee.PixelWidth);
            x_img_erosion_croix_diff.Stretch = Stretch.None;
            x_img_erosion_croix_diff.Width = bti_img_resultante.PixelWidth;
            x_img_erosion_croix_diff.Height = bti_img_resultante.PixelHeight;
            x_img_erosion_croix_diff.Source = bti_img_resultante;
        }

        //------------------------------------------------------------------------------------------
        //EROSION structurant CARRE 
        //------------------------------------------------------------------------------------------
        //btn erosion avec le structurant CARRE
        private void x_btn_erosion_carre_Click(object sender, RoutedEventArgs e)
        {
            byte[] tab_pixel_binaire = new byte[(v_wb_seuillee.Format.BitsPerPixel / 8) * v_wb_seuillee.PixelWidth *
                                                v_wb_seuillee.PixelHeight];
            v_wb_seuillee.CopyPixels(tab_pixel_binaire,
                (v_wb_seuillee.Format.BitsPerPixel / 8) * v_wb_seuillee.PixelWidth, 0);
            //on passe le tableau binaire en LH
            byte[,] tab_pixel_binaire_LH = TransposerTableauPixelEnLH_8bit(tab_pixel_binaire, v_wb_seuillee.PixelWidth,
                v_wb_seuillee.PixelHeight);
            //on initialise le tableau binaire LH resultant avec des pixels blancs
            byte[,] tab_pixel_binaire_LH_res = new byte[v_wb_seuillee.PixelHeight, v_wb_seuillee.PixelWidth];
            for (int lig = 0; lig < v_wb_seuillee.PixelHeight; lig++)
            {
                for (int col = 0; col < v_wb_seuillee.PixelWidth; col++)
                {
                    tab_pixel_binaire_LH_res[lig, col] = 255;
                }
            }
            //on erode
            ElementStructurant structurant = new ElementStructurant(ElementStructurant.TypeStructurant.carre);
            int pixel_noir = 0;
            for (int lig = 1; lig < v_wb_seuillee.PixelHeight - 1; lig++)
            {
                for (int col = 1; col < v_wb_seuillee.PixelWidth - 1; col++)
                {
                    byte[,] tab_3x3 = new byte[3, 3];
                    tab_3x3[0, 0] = tab_pixel_binaire_LH[lig - 1, col - 1];
                    tab_3x3[0, 1] = tab_pixel_binaire_LH[lig - 1, col];
                    tab_3x3[0, 2] = tab_pixel_binaire_LH[lig - 1, col + 1];
                    tab_3x3[1, 0] = tab_pixel_binaire_LH[lig, col - 1];
                    tab_3x3[1, 1] = tab_pixel_binaire_LH[lig, col];
                    tab_3x3[1, 2] = tab_pixel_binaire_LH[lig, col + 1];
                    tab_3x3[2, 0] = tab_pixel_binaire_LH[lig + 1, col - 1];
                    tab_3x3[2, 1] = tab_pixel_binaire_LH[lig + 1, col];
                    tab_3x3[2, 2] = tab_pixel_binaire_LH[lig + 1, col + 1];
                    byte nouv_niv = structurant.AppliquerErosion(tab_3x3);
                    if (nouv_niv == 0)
                    {
                        pixel_noir++;
                    }
                    tab_pixel_binaire_LH_res[lig, col] = nouv_niv;
                }
            }
            x_tbl_img_erosion_carre_pxn.Text = "pixels noirs = " + pixel_noir.ToString();
            byte[] tab_pixel_binaire_res = TransposerTableauPixelEnUnique_8bit(tab_pixel_binaire_LH_res,
                v_wb_seuillee.PixelWidth, v_wb_seuillee.PixelHeight);
            BitmapSource bti_img_resultante = BitmapSource.Create(v_wb_seuillee.PixelWidth, v_wb_seuillee.PixelHeight,
                96.0, 96.0,
                PixelFormats.Gray8, null, tab_pixel_binaire_res, v_wb_seuillee.PixelWidth);
            x_img_erosion_carre.Stretch = Stretch.None;
            x_img_erosion_carre.Width = bti_img_resultante.PixelWidth;
            x_img_erosion_carre.Height = bti_img_resultante.PixelHeight;
            x_img_erosion_carre.Source = bti_img_resultante;
        }

        //btn difference entre erosion carre et image binaire
        private void x_btn_erosion_carre_diff_Click(object sender, RoutedEventArgs e)
        {
            byte[] tab_pixel_binaire = new byte[(v_wb_seuillee.Format.BitsPerPixel / 8) * v_wb_seuillee.PixelWidth *
                                                v_wb_seuillee.PixelHeight];
            v_wb_seuillee.CopyPixels(tab_pixel_binaire,
                (v_wb_seuillee.Format.BitsPerPixel / 8) * v_wb_seuillee.PixelWidth, 0);
            //on passe le tableau binaire en LH
            byte[,] tab_pixel_binaire_LH = TransposerTableauPixelEnLH_8bit(tab_pixel_binaire, v_wb_seuillee.PixelWidth,
                v_wb_seuillee.PixelHeight);
            //on initialise le tableau binaire LH resultant avec des pixels blancs
            byte[,] tab_pixel_binaire_LH_res = new byte[v_wb_seuillee.PixelHeight, v_wb_seuillee.PixelWidth];
            for (int lig = 0; lig < v_wb_seuillee.PixelHeight; lig++)
            {
                for (int col = 0; col < v_wb_seuillee.PixelWidth; col++)
                {
                    tab_pixel_binaire_LH_res[lig, col] = 255;
                }
            }
            //on erode
            ElementStructurant structurant = new ElementStructurant(ElementStructurant.TypeStructurant.carre);
            int pixel_noir = 0;
            for (int lig = 1; lig < v_wb_seuillee.PixelHeight - 1; lig++)
            {
                for (int col = 1; col < v_wb_seuillee.PixelWidth - 1; col++)
                {
                    byte[,] tab_3x3 = new byte[3, 3];
                    tab_3x3[0, 0] = tab_pixel_binaire_LH[lig - 1, col - 1];
                    tab_3x3[0, 1] = tab_pixel_binaire_LH[lig - 1, col];
                    tab_3x3[0, 2] = tab_pixel_binaire_LH[lig - 1, col + 1];
                    tab_3x3[1, 0] = tab_pixel_binaire_LH[lig, col - 1];
                    tab_3x3[1, 1] = tab_pixel_binaire_LH[lig, col];
                    tab_3x3[1, 2] = tab_pixel_binaire_LH[lig, col + 1];
                    tab_3x3[2, 0] = tab_pixel_binaire_LH[lig + 1, col - 1];
                    tab_3x3[2, 1] = tab_pixel_binaire_LH[lig + 1, col];
                    tab_3x3[2, 2] = tab_pixel_binaire_LH[lig + 1, col + 1];
                    byte nouv_niv = structurant.AppliquerErosion(tab_3x3);
                    tab_pixel_binaire_LH_res[lig, col] = nouv_niv;
                }
            }
            //difference entre image erodee et image binaire
            byte[,] tab_diff = new byte[v_wb_seuillee.PixelHeight, v_wb_seuillee.PixelWidth];
            Array.Copy(tab_pixel_binaire_LH_res, tab_diff, tab_pixel_binaire_LH_res.Length);
            for (int lig = 1; lig < v_wb_seuillee.PixelHeight - 1; lig++)
            {
                for (int col = 1; col < v_wb_seuillee.PixelWidth - 1; col++)
                {
                    if (tab_pixel_binaire_LH_res[lig, col] == 255 && tab_pixel_binaire_LH[lig, col] == 0)
                    {
                        tab_diff[lig, col] = 0;
                    }
                    if (tab_pixel_binaire_LH_res[lig, col] == 0 && tab_pixel_binaire_LH[lig, col] == 0)
                    {
                        tab_diff[lig, col] = 255;
                    }
                }
            }
            for (int lig = 1; lig < v_wb_seuillee.PixelHeight - 1; lig++)
            {
                for (int col = 1; col < v_wb_seuillee.PixelWidth - 1; col++)
                {
                    if (tab_diff[lig, col] == 0)
                    {
                        pixel_noir++;
                    }
                }
            }
            x_tbl_img_erosion_carre_pxn_diff.Text = "pixels noirs = " + pixel_noir.ToString();
            byte[] tab_pixel_binaire_res =
                TransposerTableauPixelEnUnique_8bit(tab_diff, v_wb_seuillee.PixelWidth, v_wb_seuillee.PixelHeight);
            BitmapSource bti_img_resultante = BitmapSource.Create(v_wb_seuillee.PixelWidth, v_wb_seuillee.PixelHeight,
                96.0, 96.0,
                PixelFormats.Gray8, null, tab_pixel_binaire_res, v_wb_seuillee.PixelWidth);
            x_img_erosion_carre_diff.Stretch = Stretch.None;
            x_img_erosion_carre_diff.Width = bti_img_resultante.PixelWidth;
            x_img_erosion_carre_diff.Height = bti_img_resultante.PixelHeight;
            x_img_erosion_carre_diff.Source = bti_img_resultante;
        }

        //-----------------------------------------------------------------------------------------------
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