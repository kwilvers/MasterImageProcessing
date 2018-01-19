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

namespace VS2013_07_ContourFreeman {
  /// <summary>
  /// Logique d'interaction pour Page.xaml
  /// </summary>
  public partial class Page : UserControl {
    //donnees
    private string RC = Environment.NewLine;
    private bool v_fen_charge = false;
    private int[,] v_tab_pixels_LH;
    private int v_lig_etiq_trouve;
    private int v_col_etiq_trouve;
    private string v_etiq_trouve;
    private string[,] v_tab_etiq_LH;
    private int v_indice_pt = 1;
    private int v_lig_pt_ini;
    private int v_col_pt_ini;
    private int v_direction = 7;
    public Page() {
      InitializeComponent();
    }
    //controle Loaded
    private void UserControl_Loaded(object sender, RoutedEventArgs e) {
      v_fen_charge = true;
      AjouterMotif();
      ViderZoneTexte();
      x_text_code.Text = "Code de Freeman : ";
    }
    //ajouter le motif de la forme
    private void AjouterMotif() {
      string[] tab_motif = new string[12];
      tab_motif[0] = "000000000000";
      tab_motif[1] = "000011100000";
      tab_motif[2] = "000111110000";
      tab_motif[3] = "001111111000";
      tab_motif[4] = "000011111000";
      tab_motif[5] = "000001100000";
      tab_motif[6] = "000001110000";
      tab_motif[7] = "000011110000";
      tab_motif[8] = "000000000000";
      tab_motif[9] = "000000000000";
      tab_motif[10] = "000000000000";
      tab_motif[11] = "000000000000";
      v_tab_pixels_LH = new int[12, 12];
      for (int lig = 0; lig < 12; lig++) {
        for (int col = 0; col < 12; col++) {
          if (tab_motif[lig].Substring(col, 1) == "0") {
            v_tab_pixels_LH[lig, col] = 255;
          }
          else {
            v_tab_pixels_LH[lig, col] = 100;
          }
        }
      }
      x_grille_12x12.FixerLeMotif(v_tab_pixels_LH);
      v_tab_etiq_LH = new string[12, 12];
      for (int lig = 0; lig < 12; lig++) {
        for (int col = 0; col < 12; col++) {
          v_tab_etiq_LH[lig, col] = "-";
        }
      }
    }
    //
    private void ViderZoneTexte() {
      x_text_infos.Text = "";
      x_text_infos.Height = 5000;
    }
    //bouton demarrer la detection
    private void x_btn_lancer_Click(object sender, RoutedEventArgs e) {
      x_btn_lancer.IsEnabled = false;
      bool pixel_trouve = false;
      for (int lig = 0; lig < 12; lig++) {
        for (int col = 0; col < 12; col++) {
          if (v_tab_pixels_LH[lig, col] == 100) {
            pixel_trouve = true;
            v_etiq_trouve = "P" + v_indice_pt.ToString();
            v_lig_etiq_trouve = lig;
            v_col_etiq_trouve = col;
            x_grille_12x12.AfficherEtiquette(v_etiq_trouve, v_lig_etiq_trouve, v_col_etiq_trouve);
            AfficherInfos("1er pixel de bordure trouvé (lig,col) = (" + v_lig_etiq_trouve.ToString("00") + ","
              + v_col_etiq_trouve.ToString("00") + ")" + RC);
            v_tab_etiq_LH[v_lig_etiq_trouve, v_col_etiq_trouve] = v_etiq_trouve;
            break;
          }
        }
        if (pixel_trouve == true) {
          break;
        }
      }
      x_grille_voisins.AfficherVoisinage(v_lig_etiq_trouve, v_col_etiq_trouve, v_tab_pixels_LH, v_tab_etiq_LH);
      v_lig_pt_ini = v_lig_etiq_trouve;
      v_col_pt_ini = v_col_etiq_trouve;
      x_btn_point_suiv.IsEnabled = true;
      x_btn_point_suiv.Content = "Détecter point P" + (v_indice_pt + 1).ToString();
    }
    //
    private void AfficherInfos(string texte) {
      x_text_infos.Text += texte;
    }
    //bouton detecter le point suivant
    private void x_btn_point_suiv_Click(object sender, RoutedEventArgs e) {
      AfficherInfos("on a direction=" + v_direction.ToString() + RC);
      if (v_direction % 2 != 0) {
        //direction impaire
        v_direction = (v_direction + 6) % 8;
        AfficherInfos("on cherche à partir de dir=" + v_direction.ToString() + " sens contraire aiguille" + RC);
      }
      else {
        //direction paire
        v_direction = (v_direction + 7) % 8;
        AfficherInfos("on cherche à partir de dir=" + v_direction.ToString() + " sens contraire aiguille" + RC);
      }
      int[,] voisins_pixel = new int[3, 3];
      string[,] voisins_etiq = new string[3, 3];
      int deplac_lig = 0, deplac_col = 0;
      for (int lig = v_lig_etiq_trouve - 1; lig <= v_lig_etiq_trouve + 1; lig++) {
        for (int col = v_col_etiq_trouve - 1; col <= v_col_etiq_trouve + 1; col++) {
          voisins_pixel[deplac_lig, deplac_col] = v_tab_pixels_LH[lig, col];
          voisins_etiq[deplac_lig, deplac_col] = v_tab_etiq_LH[lig, col];
          deplac_col++;
        }
        deplac_lig++;
        deplac_col = 0;
      }
      int dir_trouvee = ChercherPoint(voisins_pixel, voisins_etiq, v_direction);
      AfficherInfos("direction trouvée= " + dir_trouvee.ToString() + RC);
      switch (dir_trouvee) {
        case 0: v_col_etiq_trouve++; break;
        case 1: v_lig_etiq_trouve--; v_col_etiq_trouve++; break;
        case 2: v_lig_etiq_trouve--; break;
        case 3: v_lig_etiq_trouve--; v_col_etiq_trouve--; break;
        case 4: v_col_etiq_trouve--; break;
        case 5: v_lig_etiq_trouve++; v_col_etiq_trouve--; break;
        case 6: v_lig_etiq_trouve++; break;
        case 7: v_lig_etiq_trouve++; v_col_etiq_trouve++; break;
      }
      v_indice_pt++;
      v_etiq_trouve = "P" + v_indice_pt.ToString();
      AfficherInfos(v_etiq_trouve + " pixel de bordure = (" + v_lig_etiq_trouve.ToString("00") + ","
              + v_col_etiq_trouve.ToString("00") + ")" + RC);
      v_tab_etiq_LH[v_lig_etiq_trouve, v_col_etiq_trouve] = v_etiq_trouve;
      x_grille_12x12.AfficherEtiquette(v_etiq_trouve, v_lig_etiq_trouve, v_col_etiq_trouve);
      MettreAJourCodeFreeman(dir_trouvee);
      if (ContourTerminer() == false) {
        v_direction = (v_direction + 1) % 8;
        AfficherInfos("la direction est mise à jour =" + v_direction.ToString() + RC);
        x_btn_point_suiv.Content = "Détecter point P" + (v_indice_pt + 1).ToString();
        x_grille_voisins.AfficherVoisinage(v_lig_etiq_trouve, v_col_etiq_trouve, v_tab_pixels_LH, v_tab_etiq_LH);
      }
      else {
        x_btn_point_suiv.Content = "Contour fermé";
        x_btn_point_suiv.IsEnabled = false;
        x_btn_raz.IsEnabled = true;
      }
    }
    //chercher le point dans le voisinage 3x3
    private int ChercherPoint(int[,] voisins_pixel, string[,] voisins_etiq, int direction) {
      int[] voisins = new int[8]{voisins_pixel[1,2],voisins_pixel[0,2],voisins_pixel[0,1],voisins_pixel[0,0],
        voisins_pixel[1,0],voisins_pixel[2,0],voisins_pixel[2,1],voisins_pixel[2,2]};
      string[] etiqs = new string[8]{voisins_etiq[1,2],voisins_etiq[0,2],voisins_etiq[0,1],voisins_etiq[0,0],
        voisins_etiq[1,0],voisins_etiq[2,0],voisins_etiq[2,1],voisins_etiq[2,2]};
      string parcours = "";
      switch (direction) {
        case 0: parcours = "012345670"; break;
        case 1: parcours = "123456701"; break;
        case 2: parcours = "234567012"; break;
        case 3: parcours = "345670123"; break;
        case 4: parcours = "456701234"; break;
        case 5: parcours = "567012345"; break;
        case 6: parcours = "670123456"; break;
        case 7: parcours = "701234567"; break;
      }
      int direction_trouvee = -1;
      for (int tour = 0; tour <= 7; tour++) {
        int comp_1 = voisins[int.Parse(parcours.Substring(tour, 1))];
        int comp_2 = voisins[int.Parse(parcours.Substring(tour + 1, 1))];
        string comp_2_etiq = etiqs[int.Parse(parcours.Substring(tour + 1, 1))];
        if (comp_1 == 255 && comp_2 == 100 && comp_2_etiq == "-") {
          direction_trouvee = int.Parse(parcours.Substring(tour + 1, 1));
          break;
        }
      }
      return direction_trouvee;
    }
    //mettre a jour le code de freeman trouvé
    private void MettreAJourCodeFreeman(int direction) {
      x_text_code.Text += direction.ToString() + " ";
    }
    //regarder si le contour est fermé
    private bool ContourTerminer() {
      bool termine = false;
      if (v_indice_pt > 2) {
        if (v_lig_pt_ini == v_lig_etiq_trouve && v_col_pt_ini == v_col_etiq_trouve - 1) {
          termine = true;
        }
        if (v_lig_pt_ini == v_lig_etiq_trouve && v_col_pt_ini == v_col_etiq_trouve + 1) {
          termine = true;
        }
        if (v_lig_pt_ini == v_lig_etiq_trouve + 1 && v_col_pt_ini == v_col_etiq_trouve) {
          termine = true;
        }
        if (v_lig_pt_ini == v_lig_etiq_trouve - 1 && v_col_pt_ini == v_col_etiq_trouve) {
          termine = true;
        }
        if (v_lig_pt_ini == v_lig_etiq_trouve + 1 && v_col_pt_ini == v_col_etiq_trouve + 1) {
          termine = true;
        }
        if (v_lig_pt_ini == v_lig_etiq_trouve + 1 && v_col_pt_ini == v_col_etiq_trouve - 1) {
          termine = true;
        }
        if (v_lig_pt_ini == v_lig_etiq_trouve - 1 && v_col_pt_ini == v_col_etiq_trouve - 1) {
          termine = true;
        }
        if (v_lig_pt_ini == v_lig_etiq_trouve - 1 && v_col_pt_ini == v_col_etiq_trouve + 1) {
          termine = true;
        }
      }
      return termine;
    }
    //bouton de remise a zero
    private void x_btn_raz_Click(object sender, RoutedEventArgs e) {
      x_btn_lancer.IsEnabled = true;
      x_btn_raz.IsEnabled = false;
      ViderZoneTexte();
      x_text_code.Text = "Code de Freeman : ";
      x_grille_12x12.ViderPourRemiseAZero();
      AjouterMotif();
      v_lig_etiq_trouve = 0;
      v_col_etiq_trouve = 0;
      v_etiq_trouve = "";
      v_indice_pt = 1;
      v_lig_pt_ini = 0;
      v_col_pt_ini = 0;
      v_direction = 7;
      x_grille_voisins.ViderLaGrille();
    }
    //--------------------------------------------------------------------------
    //DETECTION sur une forme libre
    //--------------------------------------------------------------------------
    //bouton detection
    private void x_btn_detect_essai_Click(object sender, RoutedEventArgs e) {
      try {
        //chargement image
        Uri uri = new Uri("pack://application:,,,/VS2013_07_ContourFreeman;component/collection_images/image pour freeman.bmp", UriKind.Absolute);
        BitmapImage bti = new BitmapImage();
        bti.BeginInit();
        bti.UriSource = uri;
        bti.EndInit();
        WriteableBitmap wb = new WriteableBitmap(bti);
        int largeur_numerisation = (wb.Format.BitsPerPixel / 8) * wb.PixelWidth;
        byte[] tab_pixel = new byte[largeur_numerisation * wb.PixelHeight];
        wb.CopyPixels(tab_pixel, largeur_numerisation, 0);
        byte[,] tab_pixel_LH = TransposerTableauPixelEnLH_8bit(tab_pixel, wb.PixelWidth, wb.PixelHeight);
        int[,] tab_pixels_noirblanc_LH = new int[wb.PixelHeight, wb.PixelWidth];
        for (int lig = 0; lig < wb.PixelHeight; lig++) {
          for (int col = 0; col < wb.PixelWidth; col++) {
            if (tab_pixel_LH[lig, col] > 100) {
              tab_pixels_noirblanc_LH[lig, col] = 255;
            }
            else {
              tab_pixels_noirblanc_LH[lig, col] = 0;
            }
          }
        }
        //initialisation des variables
        int lig_etiq_trouve = 0;
        int col_etiq_trouve = 0;
        string etiq_trouve = "";
        string[,] tab_etiq_noirblanc_LH = new string[wb.PixelHeight, wb.PixelWidth];
        for (int lig = 0; lig < wb.PixelHeight; lig++) {
          for (int col = 0; col < wb.PixelWidth; col++) {
            tab_etiq_noirblanc_LH[lig, col] = "-";
          }
        }
        int indice_pt = 1;
        int lig_pt_ini = 0;
        int col_pt_ini = 0;
        int direction = 7;
        List<int> liste_freeman = new List<int>();
        //on detecte le point de depart
        bool pixel_trouve = false;
        for (int lig = 0; lig < wb.PixelHeight; lig++) {
          for (int col = 0; col < wb.PixelWidth; col++) {
            if (tab_pixels_noirblanc_LH[lig, col] == 0) {
              pixel_trouve = true;
              etiq_trouve = "P" + indice_pt.ToString();
              lig_etiq_trouve = lig;
              col_etiq_trouve = col;
              tab_etiq_noirblanc_LH[lig_etiq_trouve, col_etiq_trouve] = etiq_trouve;
              break;
            }
          }
          if (pixel_trouve == true) {
            break;
          }
        }
        lig_pt_ini = lig_etiq_trouve;
        col_pt_ini = col_etiq_trouve;
        //MessageBox.Show(lig_pt_ini.ToString() + " " + col_pt_ini.ToString());
        //detection du contour
        bool contour_termine = false;
        while (contour_termine == false) {
          if (direction % 2 != 0) {
            //direction impaire
            direction = (direction + 6) % 8;
          }
          else {
            //direction paire
            direction = (direction + 7) % 8;
          }
          int[,] voisins_pixel = new int[3, 3];
          string[,] voisins_etiq = new string[3, 3];
          int deplac_lig = 0, deplac_col = 0;
          for (int lig = lig_etiq_trouve - 1; lig <= lig_etiq_trouve + 1; lig++) {
            for (int col = col_etiq_trouve - 1; col <= col_etiq_trouve + 1; col++) {
              voisins_pixel[deplac_lig, deplac_col] = tab_pixels_noirblanc_LH[lig, col];
              voisins_etiq[deplac_lig, deplac_col] = tab_etiq_noirblanc_LH[lig, col];
              deplac_col++;
            }
            deplac_lig++;
            deplac_col = 0;
          }
          int dir_trouvee = ChercherPointEssai(voisins_pixel, voisins_etiq, direction);
          if (dir_trouvee == -1) {
            MessageBox.Show(dir_trouvee.ToString());
            break;
          }
          switch (dir_trouvee) {
            case 0: col_etiq_trouve++; break;
            case 1: lig_etiq_trouve--; col_etiq_trouve++; break;
            case 2: lig_etiq_trouve--; break;
            case 3: lig_etiq_trouve--; col_etiq_trouve--; break;
            case 4: col_etiq_trouve--; break;
            case 5: lig_etiq_trouve++; col_etiq_trouve--; break;
            case 6: lig_etiq_trouve++; break;
            case 7: lig_etiq_trouve++; col_etiq_trouve++; break;
          }
          indice_pt++;
          etiq_trouve = "P" + indice_pt.ToString();
          tab_etiq_noirblanc_LH[lig_etiq_trouve, col_etiq_trouve] = etiq_trouve;
          liste_freeman.Add(dir_trouvee);
          //
          if (ContourEstIlfermer(indice_pt, lig_pt_ini, col_pt_ini, lig_etiq_trouve, col_etiq_trouve) == false) {
            direction = (direction + 1) % 8;
          }
          else {
            contour_termine = true;
          }
        }//fin while
        //afficher le code trouvé
        string aff_code = "Code de Freeman généré comportant " + liste_freeman.Count.ToString() + " directions" + RC;
        for (int xx = 0; xx < liste_freeman.Count; xx++) {
          aff_code += liste_freeman[xx].ToString();
        }
          //MessageBox.Show(liste_freeman.Count.ToString());//1370 trouvés
          x_text_forme_code.Text = aff_code;
        //convertir le code en forme
        WriteableBitmap wab_res = new WriteableBitmap(wb.PixelWidth, wb.PixelHeight, 96.0, 96.0, PixelFormats.Gray8, null);
        byte[] tab_pixels_res = new byte[wb.PixelWidth * wb.PixelHeight];
        wab_res.CopyPixels(tab_pixels_res, wb.PixelWidth, 0);
        byte[,] tab_pixels_res_LH = TransposerTableauPixelEnLH_8bit(tab_pixels_res, wb.PixelWidth, wb.PixelHeight);
        //PasserTab_1dim_vers_LH(tab_pixels_res, tab_pixels_res_LH, nb_largeur, nb_hauteur);
        for (int lig = 0; lig < wb.PixelHeight; lig++) {
          for (int col = 0; col < wb.PixelWidth; col++) {
            tab_pixels_res_LH[lig, col] = 255;
          }
        }
        int nouv_lig_pt = lig_pt_ini;
        int nouv_col_pt = col_pt_ini;
        tab_pixels_res_LH[nouv_lig_pt, nouv_col_pt] = 0;
        for (int xx = 0; xx < liste_freeman.Count; xx++) {
          int code = liste_freeman[xx];
          switch (code) {
            case 0: nouv_col_pt++; break;
            case 1: nouv_lig_pt--; nouv_col_pt++; break;
            case 2: nouv_lig_pt--; break;
            case 3: nouv_lig_pt--; nouv_col_pt--; break;
            case 4: nouv_col_pt--; break;
            case 5: nouv_lig_pt++; nouv_col_pt--; break;
            case 6: nouv_lig_pt++; break;
            case 7: nouv_lig_pt++; nouv_col_pt++; break;
          }
          tab_pixels_res_LH[nouv_lig_pt, nouv_col_pt] = 0;
        }
        byte[] tab_pixel_contour = TransposerTableauPixelEnUnique_8bit(tab_pixels_res_LH, wb.PixelWidth, wb.PixelHeight);
        BitmapSource bti_res = BitmapSource.Create(wb.PixelWidth, wb.PixelHeight, 96.0, 96.0,
          PixelFormats.Gray8, null, tab_pixel_contour, largeur_numerisation);
        x_img_res.Width = bti_res.PixelWidth;
        x_img_res.Height = bti_res.PixelHeight;
        x_img_res.Source = bti_res;
      }
      catch (Exception ex) {
        MessageBox.Show(ex.Message);
      }
    }
    //chercher le point dans le voisinage 3x3
    private int ChercherPointEssai(int[,] voisins_pixel, string[,] voisins_etiq, int direction) {
      int[] voisins = new int[8]{voisins_pixel[1,2],voisins_pixel[0,2],voisins_pixel[0,1],voisins_pixel[0,0],
        voisins_pixel[1,0],voisins_pixel[2,0],voisins_pixel[2,1],voisins_pixel[2,2]};
      string[] etiqs = new string[8]{voisins_etiq[1,2],voisins_etiq[0,2],voisins_etiq[0,1],voisins_etiq[0,0],
        voisins_etiq[1,0],voisins_etiq[2,0],voisins_etiq[2,1],voisins_etiq[2,2]};
      string parcours = "";
      switch (direction) {
        case 0: parcours = "012345670"; break;
        case 1: parcours = "123456701"; break;
        case 2: parcours = "234567012"; break;
        case 3: parcours = "345670123"; break;
        case 4: parcours = "456701234"; break;
        case 5: parcours = "567012345"; break;
        case 6: parcours = "670123456"; break;
        case 7: parcours = "701234567"; break;
      }
      int direction_trouvee = -1;
      for (int tour = 0; tour <= 7; tour++) {
        int comp_1 = voisins[int.Parse(parcours.Substring(tour, 1))];
        int comp_2 = voisins[int.Parse(parcours.Substring(tour + 1, 1))];
        string comp_2_etiq = etiqs[int.Parse(parcours.Substring(tour + 1, 1))];
        if (comp_1 == 255 && comp_2 == 0 && comp_2_etiq == "-") {
          direction_trouvee = int.Parse(parcours.Substring(tour + 1, 1));
          break;
        }
      }
      return direction_trouvee;
    }
    //regarder si le contour est fermé
    private bool ContourEstIlfermer(int indice_pt, int lig_pt_ini, int col_pt_ini, int lig_etiq_trouve, int col_etiq_trouve) {
      bool termine = false;
      if (indice_pt > 2) {
        if (lig_pt_ini == lig_etiq_trouve && col_pt_ini == col_etiq_trouve - 1) {
          termine = true;
        }
        if (lig_pt_ini == lig_etiq_trouve && col_pt_ini == col_etiq_trouve + 1) {
          termine = true;
        }
        if (lig_pt_ini == lig_etiq_trouve + 1 && col_pt_ini == col_etiq_trouve) {
          termine = true;
        }
        if (lig_pt_ini == lig_etiq_trouve - 1 && col_pt_ini == col_etiq_trouve) {
          termine = true;
        }
        if (lig_pt_ini == lig_etiq_trouve + 1 && col_pt_ini == col_etiq_trouve + 1) {
          termine = true;
        }
        if (lig_pt_ini == lig_etiq_trouve + 1 && col_pt_ini == col_etiq_trouve - 1) {
          termine = true;
        }
        if (lig_pt_ini == lig_etiq_trouve - 1 && col_pt_ini == col_etiq_trouve - 1) {
          termine = true;
        }
        if (lig_pt_ini == lig_etiq_trouve - 1 && col_pt_ini == col_etiq_trouve + 1) {
          termine = true;
        }
      }
      return termine;
    }



    //************************************************************************
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

   


  }//end class
}
