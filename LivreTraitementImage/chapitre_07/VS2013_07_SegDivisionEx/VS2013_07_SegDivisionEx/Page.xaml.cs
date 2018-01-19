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

namespace VS2013_07_SegDivisionEx {
  /// <summary>
  /// Logique d'interaction pour Page.xaml
  /// </summary>
  public partial class Page : UserControl {
    //donnees
    private string RC = Environment.NewLine;
    private bool v_fen_charge = false;
    private byte v_detection = 0;
    private List<Noeud4Fils> v_liste_feuille = null;
    public Page() {
      InitializeComponent();
    }
    //controle Loaded
    private void UserControl_Loaded(object sender, RoutedEventArgs e) {
      v_fen_charge = true;
      Uri uri = new Uri("pack://application:,,,/VS2013_07_SegDivisionEx;component/collection_images/hibernatus_8bit_388x446_96dpi.jpg", UriKind.Absolute);
      BitmapImage bti = new BitmapImage();
      bti.BeginInit();
      bti.UriSource = uri;
      bti.EndInit();
      x_img.Width = bti.PixelWidth;
      x_img.Height = bti.PixelHeight;
      x_img.Source = bti;
      v_detection = (byte)x_slider_detect.Value;
      x_tbl_detect.Text = v_detection.ToString();
      SegmentationParDivision();
    }
    //valeur changee du cuurseur de la glissiere
    private void x_slider_detect_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
      if (v_fen_charge == true) {
        x_text_infos.Text = "";
        v_detection = (byte)x_slider_detect.Value;
        x_tbl_detect.Text = v_detection.ToString();
        SegmentationParDivision();
      }
    }
    //effectuer la segmentation par division
    private void SegmentationParDivision() {
      Uri uri = new Uri("pack://application:,,,/VS2013_07_SegDivisionEx;component/collection_images/hibernatus_8bit_388x446_96dpi.jpg", UriKind.Absolute);
      BitmapImage bti = new BitmapImage();
      bti.BeginInit();
      bti.UriSource = uri;
      bti.EndInit();
      WriteableBitmap wb = new WriteableBitmap(bti);
      int largeur_numerisation = (wb.Format.BitsPerPixel / 8) * wb.PixelWidth;
      byte[] tab_pixel = new byte[largeur_numerisation * wb.PixelHeight];
      wb.CopyPixels(tab_pixel, largeur_numerisation, 0);
      byte[,] tab_pixel_LH = TransposerTableauPixelEnLH_8bit(tab_pixel, wb.PixelWidth, wb.PixelHeight);
      int[,] tab_pixel_LH_int = new int[wb.PixelHeight, wb.PixelWidth];
      for (int lig = 0; lig < wb.PixelHeight; lig++) {
        for (int col = 0; col < wb.PixelWidth; col++) {
          tab_pixel_LH_int[lig, col] = tab_pixel_LH[lig, col];
        }
      }
      //on instancie l'arbre et on segmente et on récupère la liste des feuilles
      Arbre4Fils arbre = new Arbre4Fils(tab_pixel_LH_int, wb.PixelWidth, (int)v_detection);
      arbre.SegmenterArbre();
      List<Noeud4Fils> liste_noeud = arbre.ListeNoeud4Fils;
      v_liste_feuille = new List<Noeud4Fils>();
      for (int xx = 0; xx < liste_noeud.Count; xx++) {
        Noeud4Fils noeud = liste_noeud[xx];
        if (noeud.FilsNordOuest == null) {
          if (noeud.FilsNordEst == null) {
            if (noeud.FilsSudOuest == null) {
              if (noeud.FilsSudEst == null) {
                v_liste_feuille.Add(liste_noeud[xx]);
              }
            }
          }
        }
      }
      //on reconstruit les regions
      for (int xx = 0; xx < v_liste_feuille.Count; xx++) {
        Noeud4Fils region = v_liste_feuille[xx];
        int valeur_gris = region.ValeurGris;
        List<Point> liste_pixel = ObtenirListePixel(region);
        for (int yy = 0; yy < liste_pixel.Count; yy++) {
          Point pixel = liste_pixel[yy];
          tab_pixel_LH_int[(int)pixel.X, (int)pixel.Y] = valeur_gris;
        }
      }
      //on genere l'image resultante
      for (int lig = 0; lig < wb.PixelHeight; lig++) {
        for (int col = 0; col < wb.PixelWidth; col++) {
          tab_pixel_LH[lig, col] = (byte)tab_pixel_LH_int[lig, col];
        }
      }
      byte[] tab_pixel_segment = TransposerTableauPixelEnUnique_8bit(tab_pixel_LH, wb.PixelWidth, wb.PixelHeight);
      BitmapSource bti_res = BitmapSource.Create(wb.PixelWidth, wb.PixelHeight, 96.0, 96.0,
        PixelFormats.Gray8, null, tab_pixel_segment, largeur_numerisation);
      x_img_segment.Width = bti_res.PixelWidth;
      x_img_segment.Height = bti_res.PixelHeight;
      x_img_segment.Source = bti_res;
    }
    //clic sur le bouton determiner les regions
    private void x_btn_regions_Click(object sender, RoutedEventArgs e) {
      x_slider_detect.Value = 255;
      v_detection = (byte)x_slider_detect.Value;
      x_tbl_detect.Text = v_detection.ToString();
      SegmentationParDivision();
      EtiquetageRegionMinimum255();
    }
    //
    private void EtiquetageRegionMinimum255() {
      AfficherInfos("liste des régions:" + RC);
      for (int xx = 0; xx < v_liste_feuille.Count; xx++) {
        AfficherInfos(v_liste_feuille[xx].ToString() + RC);
      }
      AfficherInfos("étiquetage des régions:" + RC);
      int compteur_etiquette = 0;
      for (int xx = 0; xx < v_liste_feuille.Count; xx++) {
        Noeud4Fils region = v_liste_feuille[xx];
        AfficherInfos("région: " + region.ToString() + RC);
        List<Noeud4Fils> liste_region_connexe = ObtenirRegionConnexe(region, v_liste_feuille);
        for (int yy = 0; yy < liste_region_connexe.Count; yy++) {
          AfficherInfos("connexe= " + liste_region_connexe[yy].ToString() + RC);
        }
        //si cette region n'est pas étiquetée
        if (region.Etiquette == -1) {
          int gris_region = region.ValeurGris;
          List<Noeud4Fils> liste_region_connexe_gris = ObtenirRegionConnexeGris(region, v_liste_feuille);
          for (int yy = 0; yy < liste_region_connexe_gris.Count; yy++) {
            AfficherInfos("connexe= " + liste_region_connexe_gris[yy].ToString() + RC);
          }
          //on regarde si ces regions connexes possèdent au moins une étiquette
          int etiq_presente = -1;
          for (int yy = 0; yy < liste_region_connexe_gris.Count; yy++) {
            if (liste_region_connexe_gris[yy].Etiquette != -1) {
              etiq_presente = liste_region_connexe_gris[yy].Etiquette;
              break;
            }
          }
          //si aucune etiquette trouvée
          if (etiq_presente == -1) {
            //on cree une nouvelle étiquette
            compteur_etiquette++;
            //on donne cette nouvelle étiquette à la région
            region.Etiquette = compteur_etiquette;
            //on donne cette nouvelle étiquette au region connexe de meme gris
            for (int yy = 0; yy < liste_region_connexe_gris.Count; yy++) {
              liste_region_connexe_gris[yy].Etiquette = compteur_etiquette;
            }
          }
          else {
            //une des regions connexes de meme gris a une etiquette
            //on donne cette etiquette à la region
            region.Etiquette = etiq_presente;
            //on donne cette etiquette au regions connexes de meme gris
            //qui n'étaient pas étiquetés
            for (int yy = 0; yy < liste_region_connexe_gris.Count; yy++) {
              if (liste_region_connexe_gris[yy].Etiquette == -1) {
                liste_region_connexe_gris[yy].Etiquette = etiq_presente;
              }
            }
          }
        }
      }
      AfficherInfos("liste des régions étiquetées:" + RC);
      for (int xx = 0; xx < v_liste_feuille.Count; xx++) {
        AfficherInfos(v_liste_feuille[xx].AffichageRegionEtiquetee() + RC);
      }
    }
    //transposition tableau pixel dimension 1 vers 2 avec codage 8 bits
    private byte[,] TransposerTableauPixelEnLH_8bit(byte[] tab_pixel, int pixel_larg, int pixel_haut) {
      byte[,] tab_LH = new byte[pixel_haut, pixel_larg];
      int lig = 0;
      int col = 0;
      for (int xx = 0; xx < tab_pixel.Length; xx++) {
        byte niveau_gris = tab_pixel[xx];
        tab_LH[lig, col] = niveau_gris;
        col++;
        if (col == pixel_larg) {
          col = 0;
          lig++;
        }
      }
      return tab_LH;
    }
    //transposition tableau pixel dimension 2 vers 1 avec codage 8 bits
    private byte[] TransposerTableauPixelEnUnique_8bit(byte[,] tab_pixel_int_LH_modif, int pixel_largeur, int pixel_hauteur) {
      byte[] tab = new byte[pixel_largeur * pixel_hauteur];
      int cpt = 0;
      for (int lig = 0; lig < pixel_hauteur; lig++) {
        for (int col = 0; col < pixel_largeur; col++) {
          tab[cpt] = (byte)tab_pixel_int_LH_modif[lig, col];
          cpt++;
        }
      }
      return tab;
    }
    //trouver le niveau median dans un voisinage
    private byte TrouverNiveauMedian(int[] voisinage) {
      byte mediane = 0;
      List<int> liste = voisinage.ToList();
      liste.Sort();
      mediane = (byte)liste[liste.Count / 2];
      return mediane;
    }
    //inverser un niveau
    private byte InverserNiveau(byte niveau) {
      return (byte)(255 - niveau);
    }
    //borner le niveau
    public byte BornerNiveau(byte niveau) {
      byte niveau_borne = niveau;
      if (niveau < 0) {
        niveau_borne = 0;
      }
      if (niveau > 255) {
        niveau_borne = 255;
      }
      return niveau_borne;
    }
    //appliquer un filtre sur un tableau de pixels
    private void AppliquerFiltre(byte[,] tab_pixels_LH, Filtre filtre, byte[,] tab_pixels_res_LH, int largeur, int hauteur) {
      for (int lig = 1; lig < hauteur - 1; lig++) {
        for (int col = 1; col < largeur - 1; col++) {
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
    private void PasserTab_1dim_vers_LH(byte[] tab_1dim, byte[,] tab_LH, int largeur, int hauteur) {
      int compteur_indice = 0;
      for (int lig = 0; lig < hauteur; lig++) {
        for (int col = 0; col < largeur; col++) {
          tab_LH[lig, col] = tab_1dim[compteur_indice];
          compteur_indice++;
        }
      }
    }
    //passer du tableau LxH vers tableau 1 dimension
    private void PasserTab_LH_vers_1dim(byte[,] tab_LH, int largeur, int hauteur, byte[] tab_1_dim) {
      int deplac = 0;
      for (int lig = 0; lig < hauteur; lig++) {
        for (int col = 0; col < largeur; col++) {
          tab_1_dim[deplac] = tab_LH[lig, col];
          deplac++;
        }
      }
    }
    //trouver liste pixel d'un noeud
    private List<Point> ObtenirListePixel(Noeud4Fils noeud) {
      List<Point> liste = new List<Point>();
      for (int lig = noeud.PosY; lig <= noeud.PosY + noeud.CoteY - 1; lig++) {
        for (int col = noeud.PosX; col <= noeud.PosX + noeud.CoteX - 1; col++) {
          liste.Add(new Point(lig, col));
        }
      }
      return liste;
    }
    //trouver liste pixel connexe d'un noeud
    private List<Point> ObtenirListePixelConnexeNoeud(Noeud4Fils noeud) {
      List<Point> liste_connexe_noeud = new List<Point>();
      List<Point> liste_point_noeud = ObtenirListePixel(noeud);
      for (int xx = 0; xx < liste_point_noeud.Count; xx++) {
        Point pt = liste_point_noeud[xx];
        Point pt_connex_1 = new Point(pt.X - 1, pt.Y - 1);
        Point pt_connex_2 = new Point(pt.X, pt.Y - 1);
        Point pt_connex_3 = new Point(pt.X + 1, pt.Y - 1);
        Point pt_connex_4 = new Point(pt.X - 1, pt.Y);
        Point pt_connex_5 = new Point(pt.X + 1, pt.Y);
        Point pt_connex_6 = new Point(pt.X - 1, pt.Y + 1);
        Point pt_connex_7 = new Point(pt.X, pt.Y + 1);
        Point pt_connex_8 = new Point(pt.X + 1, pt.Y + 1);
        if (liste_connexe_noeud.Contains(pt_connex_1) == false && liste_point_noeud.Contains(pt_connex_1) == false) {
          liste_connexe_noeud.Add(pt_connex_1);
        }
        if (liste_connexe_noeud.Contains(pt_connex_2) == false && liste_point_noeud.Contains(pt_connex_2) == false) {
          liste_connexe_noeud.Add(pt_connex_2);
        }
        if (liste_connexe_noeud.Contains(pt_connex_3) == false && liste_point_noeud.Contains(pt_connex_3) == false) {
          liste_connexe_noeud.Add(pt_connex_3);
        }
        if (liste_connexe_noeud.Contains(pt_connex_4) == false && liste_point_noeud.Contains(pt_connex_4) == false) {
          liste_connexe_noeud.Add(pt_connex_4);
        }
        if (liste_connexe_noeud.Contains(pt_connex_5) == false && liste_point_noeud.Contains(pt_connex_5) == false) {
          liste_connexe_noeud.Add(pt_connex_5);
        }
        if (liste_connexe_noeud.Contains(pt_connex_6) == false && liste_point_noeud.Contains(pt_connex_6) == false) {
          liste_connexe_noeud.Add(pt_connex_6);
        }
        if (liste_connexe_noeud.Contains(pt_connex_7) == false && liste_point_noeud.Contains(pt_connex_7) == false) {
          liste_connexe_noeud.Add(pt_connex_7);
        }
        if (liste_connexe_noeud.Contains(pt_connex_8) == false && liste_point_noeud.Contains(pt_connex_8) == false) {
          liste_connexe_noeud.Add(pt_connex_8);
        }
      }
      return liste_connexe_noeud;
    }
    //
    private bool RegionsAdjacentes(List<Point> liste_pixel_connexe, List<Point> liste_pixel_region) {
      bool adjacente = false;
      for (int xx = 0; xx < liste_pixel_connexe.Count; xx++) {
        Point point_connex = liste_pixel_connexe[xx];
        if (liste_pixel_region.Contains(point_connex) == true) {
          adjacente = true;
          break;
        }
      }
      return adjacente;
    }
    //retourne la region d'appartenance pour un pixel donné
    private Noeud4Fils ObtenirRegionPourUnPixel(Point pixel, List<Noeud4Fils> liste_feuille) {
      Noeud4Fils noeud = null;
      for (int xx = 0; xx < liste_feuille.Count; xx++) {
        Noeud4Fils feuille = liste_feuille[xx];
        List<Point> liste_feuille_point = ObtenirListePixel(feuille);
        if (liste_feuille_point.Contains(pixel) == true) {
          noeud = feuille;
          break;
        }
      }
      return noeud;
    }
    //
    private void AfficherInfos(string texte) {
      x_text_infos.Text += texte;
      x_text_infos.Height += 18;
    }
    //trouver la liste des régions connexes à une région donnée
    private List<Noeud4Fils> ObtenirRegionConnexe(Noeud4Fils region_etudiee, List<Noeud4Fils> liste_region) {
      List<Noeud4Fils> liste_region_connexe = new List<Noeud4Fils>();
      Rect rect_region = new Rect(region_etudiee.PosX, region_etudiee.PosY, region_etudiee.CoteX, region_etudiee.CoteY);
      for (int xx = 0; xx < liste_region.Count; xx++) {
        if (Noeud4Fils.RegionIdentique(region_etudiee, liste_region[xx]) == false) {
          Rect rect = new Rect(liste_region[xx].PosX, liste_region[xx].PosY, liste_region[xx].CoteX, liste_region[xx].CoteY);
          rect.Intersect(rect_region);
          if (rect.IsEmpty == false) {
            liste_region_connexe.Add(liste_region[xx]);
          }
        }
      }
      return liste_region_connexe;
    }
    //trouver la liste des régions connexes à une région donnée ayant la meme valeur de gris
    private List<Noeud4Fils> ObtenirRegionConnexeGris(Noeud4Fils region_etudiee, List<Noeud4Fils> liste_region) {
      List<Noeud4Fils> liste_region_connexe = new List<Noeud4Fils>();
      Rect rect_region = new Rect(region_etudiee.PosX, region_etudiee.PosY, region_etudiee.CoteX, region_etudiee.CoteY);
      for (int xx = 0; xx < liste_region.Count; xx++) {
        if (Noeud4Fils.RegionIdentique(region_etudiee, liste_region[xx]) == false) {
          if (region_etudiee.ValeurGris == liste_region[xx].ValeurGris) {
            Rect rect = new Rect(liste_region[xx].PosX, liste_region[xx].PosY, liste_region[xx].CoteX, liste_region[xx].CoteY);
            rect.Intersect(rect_region);
            if (rect.IsEmpty == false) {
              liste_region_connexe.Add(liste_region[xx]);
            }
          }
        }
      }
      return liste_region_connexe;
    }
  }//end class
}
