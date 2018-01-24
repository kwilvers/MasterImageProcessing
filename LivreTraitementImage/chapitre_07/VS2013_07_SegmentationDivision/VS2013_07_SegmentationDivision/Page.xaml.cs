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

namespace VS2013_07_SegmentationDivision
{
    /// <summary>
    /// Logique d'interaction pour Page.xaml
    /// </summary>
    public partial class Page : UserControl
    {
        //donnees
        private string RC = Environment.NewLine;

        private bool v_fen_charge = false;
        private int v_nb_larg_essai;
        private int v_nb_haut_essai;
        private byte[,] v_tab_pixels_essai_LH;
        private WriteableBitmap v_wab_essai = null;

        public Page()
        {
            InitializeComponent();
        }

        //controle Loaded
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            v_fen_charge = true;
            x_text_infos.Height = 5000;
            v_wab_essai = new WriteableBitmap(12, 12, 96.0, 96.0, PixelFormats.Gray8, null);
            v_nb_larg_essai = v_wab_essai.PixelWidth;
            v_nb_haut_essai = v_wab_essai.PixelHeight;
            byte[] tab_pixels = new byte[12 * 12];
            v_wab_essai.CopyPixels(tab_pixels, 12, 0);
            v_tab_pixels_essai_LH = TransposerTableauPixelEnLH_8bit(tab_pixels, 12, 12);
            for (int lig = 0; lig < 12; lig++)
            {
                for (int col = 0; col < 12; col++)
                {
                    v_tab_pixels_essai_LH[lig, col] = 255;
                }
            }
            for (int lig = 1; lig <= 2; lig++)
            {
                for (int col = 1; col <= 10; col++)
                {
                    v_tab_pixels_essai_LH[lig, col] = 40;
                }
            }
            for (int lig = 3; lig <= 9; lig++)
            {
                for (int col = 2; col <= 3; col++)
                {
                    v_tab_pixels_essai_LH[lig, col] = 120;
                }
            }
            for (int lig = 5; lig <= 10; lig++)
            {
                for (int col = 5; col <= 10; col++)
                {
                    v_tab_pixels_essai_LH[lig, col] = 200;
                }
            }
            //tab_pixels = TransposerTableauPixelEnUnique_8bit(v_tab_pixels_essai_LH, 12, 12);
            x_visuel_12x12_ini.FixerLeMotif(v_tab_pixels_essai_LH);
        }

        //clic sur bouton segmentation
        private void x_btn_essai_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                x_text_infos.Text = "";
                //on créé un tableau contenant les valeurs de la composante gris
                int[,] tab_gris_LH = new int[12, 12];
                for (int lig = 0; lig < 12; lig++)
                {
                    for (int col = 0; col < 12; col++)
                    {
                        tab_gris_LH[lig, col] = (int) v_tab_pixels_essai_LH[lig, col];
                    }
                }
                //on instancie l'arbre et on segmente et on récupère la liste des feuilles
                Arbre4Fils arbre = new Arbre4Fils(tab_gris_LH, v_nb_larg_essai, 0);
                arbre.SegmenterArbre();
                List<Noeud4Fils> liste_noeud = arbre.ListeNoeud4Fils;
                //quelques manipulations
                x_text_infos.Text += "la racine est:" + RC;
                x_text_infos.Text += arbre.Racine.ToString() + RC;
                x_text_infos.Text += "------------------------------------------" + RC;
                x_text_infos.Text += "les feuilles de l'arbre sont:" + RC;
                List<Noeud4Fils> liste_feuille = new List<Noeud4Fils>();
                for (int xx = 0; xx < liste_noeud.Count; xx++)
                {
                    Noeud4Fils noeud = liste_noeud[xx];
                    if (noeud.FilsNordOuest == null)
                    {
                        if (noeud.FilsNordEst == null)
                        {
                            if (noeud.FilsSudOuest == null)
                            {
                                if (noeud.FilsSudEst == null)
                                {
                                    liste_feuille.Add(liste_noeud[xx]);
                                }
                            }
                        }
                    }
                }
                for (int xx = 0; xx < liste_feuille.Count; xx++)
                {
                    Noeud4Fils feuille = liste_feuille[xx];
                    x_text_infos.Text += xx.ToString("00") + ": " + feuille.ToString() + RC;
                }
                //visualiser les divisions sur le motif
                x_visuel_12x12_division.FixerLeMotif(v_tab_pixels_essai_LH);
                x_visuel_12x12_division.AfficherLesDivisions(liste_feuille, Color.FromArgb(75, 255, 255, 0));
                //quelques manipulations
                x_text_infos.Text += "------------------------------------------" + RC;
                Noeud4Fils noeud_objet3 = liste_feuille[3];
                x_text_infos.Text += noeud_objet3.ToString() + RC;
                x_text_infos.Text += "pixels du noeud:" + RC;
                List<Point> liste_pts_objet3 = ObtenirListePixel(noeud_objet3);
                for (int xx = 0; xx < liste_pts_objet3.Count; xx++)
                {
                    x_text_infos.Text += "pixels " + liste_pts_objet3[xx].ToString() + RC;
                }
                x_text_infos.Text += "pixels connexes du noeud:" + RC;
                List<Point> liste_pts_objet3_connexe = ObtenirListePixelConnexeNoeud(noeud_objet3);
                for (int xx = 0; xx < liste_pts_objet3_connexe.Count; xx++)
                {
                    x_text_infos.Text += "pixels " + liste_pts_objet3_connexe[xx].ToString() + RC;
                }
                x_text_infos.Text += "les régions adjacentes du noeud:" + RC;
                for (int xx = 0; xx < liste_feuille.Count; xx++)
                {
                    Noeud4Fils feuille = liste_feuille[xx];
                    if (RegionsAdjacentes(ObtenirListePixelConnexeNoeud(noeud_objet3), ObtenirListePixel(feuille)) ==
                        true)
                    {
                        x_text_infos.Text += xx.ToString("00") + ": " + feuille.ToString() + RC;
                    }
                }
                //visualisation des regions avec la valeur moyenne de leur gris
                x_visuel_12x12_region_gris.AfficherLesDivisions(liste_feuille, Color.FromArgb(75, 255, 255, 255));
                x_visuel_12x12_region_gris.AfficherLesRegionsAvecValeurGris(liste_feuille);
                //visualiser les regions avec leurs couleurs
                x_visuel_12x12_region_couleur.AfficherLesRegionsAvecCouleurSegmentee(liste_feuille);
                //etiquetage des regions
                x_text_infos.Text += "---------------------------------" + RC;
                x_text_infos.Text += "étiquetage des régions:" + RC;
                int compteur_etiquette = 0;
                for (int lig = 0; lig < 12; lig++)
                {
                    for (int col = 0; col < 12; col++)
                    {
                        Point pixel = new Point(lig, col);
                        Noeud4Fils region = ObtenirRegionPourUnPixel(pixel, liste_feuille);
                        //si la region n'est pas etiquete
                        if (region.Etiquette == -1)
                        {
                            int gris_region = region.ValeurGris;
                            List<Noeud4Fils> liste_region_connex_gris = new List<Noeud4Fils>();
                            for (int xx = 0; xx < liste_feuille.Count; xx++)
                            {
                                Noeud4Fils feuille = liste_feuille[xx];
                                if (RegionsAdjacentes(ObtenirListePixelConnexeNoeud(region),
                                        ObtenirListePixel(feuille)) == true)
                                {
                                    if (feuille.ValeurGris == gris_region)
                                    {
                                        liste_region_connex_gris.Add(feuille);
                                    }
                                }
                            }
                            //on cherche si ces regions adjacentes avec le meme gris
                            //ont une etiquette
                            int etiquette_presente = -1;
                            for (int xx = 0; xx < liste_region_connex_gris.Count; xx++)
                            {
                                if (liste_region_connex_gris[xx].Etiquette != -1)
                                {
                                    etiquette_presente = liste_region_connex_gris[xx].Etiquette;
                                    break;
                                }
                            }
                            //si aucune etiquette trouvée
                            if (etiquette_presente == -1)
                            {
                                //on cree une nouvelle étiquette
                                compteur_etiquette++;
                                //on donne cette etiquette à la region du pixel
                                region.Etiquette = compteur_etiquette;
                                //on donne cette etiquette au region adjacente de meme gris
                                for (int xx = 0; xx < liste_region_connex_gris.Count; xx++)
                                {
                                    liste_region_connex_gris[xx].Etiquette = compteur_etiquette;
                                }
                            }
                            else
                            {
                                //une des regions adjacentes de meme gris a une etiquette
                                //on donne cette etiquette à la region du pixel
                                region.Etiquette = etiquette_presente;
                                //on donne cette etiquette au region adjacente de meme gris
                                //qui n'était pas étiqueté
                                for (int xx = 0; xx < liste_region_connex_gris.Count; xx++)
                                {
                                    if (liste_region_connex_gris[xx].Etiquette == -1)
                                    {
                                        liste_region_connex_gris[xx].Etiquette = etiquette_presente;
                                    }
                                }
                            }
                        }
                    }
                }
                string contenu_etiquette = "";
                for (int lig = 0; lig < 12; lig++)
                {
                    for (int col = 0; col < 12; col++)
                    {
                        Point pixel = new Point(lig, col);
                        Noeud4Fils region = ObtenirRegionPourUnPixel(pixel, liste_feuille);
                        if (region.Etiquette == -1)
                        {
                            contenu_etiquette += "**" + " ";
                        }
                        else
                        {
                            contenu_etiquette += region.Etiquette.ToString("00") + " ";
                        }
                    }
                    contenu_etiquette += RC;
                }
                contenu_etiquette += RC;
                x_text_infos.Text += contenu_etiquette;
                //visualiser les regions avec leurs etiquettes
                x_visuel_12x12_region_etiquette.AfficherLesDivisions(liste_feuille, Color.FromArgb(75, 255, 255, 255));
                x_visuel_12x12_region_etiquette.AfficherLesRegionsAvecEtiquettes(liste_feuille);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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

        //passer du tableau 1 dimension vers tableau LxH
        private void PasserTab_1dim_vers_LH(byte[] tab_1dim, byte[,] tab_LH, int largeur, int hauteur)
        {
            int compteur_indice = 0;
            for (int lig = 0; lig < hauteur; lig++)
            {
                for (int col = 0; col < largeur; col++)
                {
                    tab_LH[lig, col] = tab_1dim[compteur_indice];
                    compteur_indice++;
                }
            }
        }

        //passer du tableau LxH vers tableau 1 dimension
        private void PasserTab_LH_vers_1dim(byte[,] tab_LH, int largeur, int hauteur, byte[] tab_1_dim)
        {
            int deplac = 0;
            for (int lig = 0; lig < hauteur; lig++)
            {
                for (int col = 0; col < largeur; col++)
                {
                    tab_1_dim[deplac] = tab_LH[lig, col];
                    deplac++;
                }
            }
        }

        //trouver liste pixel d'un noeud
        private List<Point> ObtenirListePixel(Noeud4Fils noeud)
        {
            List<Point> liste = new List<Point>();
            for (int lig = noeud.PosY; lig <= noeud.PosY + noeud.CoteY - 1; lig++)
            {
                for (int col = noeud.PosX; col <= noeud.PosX + noeud.CoteX - 1; col++)
                {
                    liste.Add(new Point(lig, col));
                }
            }
            return liste;
        }

        //trouver liste pixel connexe d'un noeud
        private List<Point> ObtenirListePixelConnexeNoeud(Noeud4Fils noeud)
        {
            List<Point> liste_connexe_noeud = new List<Point>();
            List<Point> liste_point_noeud = ObtenirListePixel(noeud);
            for (int xx = 0; xx < liste_point_noeud.Count; xx++)
            {
                Point pt = liste_point_noeud[xx];
                Point pt_connex_1 = new Point(pt.X - 1, pt.Y - 1);
                Point pt_connex_2 = new Point(pt.X, pt.Y - 1);
                Point pt_connex_3 = new Point(pt.X + 1, pt.Y - 1);
                Point pt_connex_4 = new Point(pt.X - 1, pt.Y);
                Point pt_connex_5 = new Point(pt.X + 1, pt.Y);
                Point pt_connex_6 = new Point(pt.X - 1, pt.Y + 1);
                Point pt_connex_7 = new Point(pt.X, pt.Y + 1);
                Point pt_connex_8 = new Point(pt.X + 1, pt.Y + 1);
                if (liste_connexe_noeud.Contains(pt_connex_1) == false &&
                    liste_point_noeud.Contains(pt_connex_1) == false)
                {
                    liste_connexe_noeud.Add(pt_connex_1);
                }
                if (liste_connexe_noeud.Contains(pt_connex_2) == false &&
                    liste_point_noeud.Contains(pt_connex_2) == false)
                {
                    liste_connexe_noeud.Add(pt_connex_2);
                }
                if (liste_connexe_noeud.Contains(pt_connex_3) == false &&
                    liste_point_noeud.Contains(pt_connex_3) == false)
                {
                    liste_connexe_noeud.Add(pt_connex_3);
                }
                if (liste_connexe_noeud.Contains(pt_connex_4) == false &&
                    liste_point_noeud.Contains(pt_connex_4) == false)
                {
                    liste_connexe_noeud.Add(pt_connex_4);
                }
                if (liste_connexe_noeud.Contains(pt_connex_5) == false &&
                    liste_point_noeud.Contains(pt_connex_5) == false)
                {
                    liste_connexe_noeud.Add(pt_connex_5);
                }
                if (liste_connexe_noeud.Contains(pt_connex_6) == false &&
                    liste_point_noeud.Contains(pt_connex_6) == false)
                {
                    liste_connexe_noeud.Add(pt_connex_6);
                }
                if (liste_connexe_noeud.Contains(pt_connex_7) == false &&
                    liste_point_noeud.Contains(pt_connex_7) == false)
                {
                    liste_connexe_noeud.Add(pt_connex_7);
                }
                if (liste_connexe_noeud.Contains(pt_connex_8) == false &&
                    liste_point_noeud.Contains(pt_connex_8) == false)
                {
                    liste_connexe_noeud.Add(pt_connex_8);
                }
            }
            return liste_connexe_noeud;
        }

        //
        private bool RegionsAdjacentes(List<Point> liste_pixel_connexe, List<Point> liste_pixel_region)
        {
            bool adjacente = false;
            for (int xx = 0; xx < liste_pixel_connexe.Count; xx++)
            {
                Point point_connex = liste_pixel_connexe[xx];
                if (liste_pixel_region.Contains(point_connex) == true)
                {
                    adjacente = true;
                    break;
                }
            }
            return adjacente;
        }

        //retourne la region d'appartenance pour un pixel donné
        private Noeud4Fils ObtenirRegionPourUnPixel(Point pixel, List<Noeud4Fils> liste_feuille)
        {
            Noeud4Fils noeud = null;
            for (int xx = 0; xx < liste_feuille.Count; xx++)
            {
                Noeud4Fils feuille = liste_feuille[xx];
                List<Point> liste_feuille_point = ObtenirListePixel(feuille);
                if (liste_feuille_point.Contains(pixel) == true)
                {
                    noeud = feuille;
                    break;
                }
            }
            return noeud;
        }
    } //end class
}