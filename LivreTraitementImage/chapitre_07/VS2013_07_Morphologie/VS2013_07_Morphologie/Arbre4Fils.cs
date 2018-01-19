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

namespace VS2013_07_Morphologie {
  public class Arbre4Fils {
    //champs
    private Noeud4Fils v_racine = null;
    private int[,] v_tab_gris_LH;
    private int v_cote_x;
    private int v_cote_y;
    private int v_pos_x;
    private int v_pos_y;
    private List<Noeud4Fils> v_liste_noeud = null;
    private int v_prof_nord_ouest;
    private int v_prof_nord_est;
    private int v_prof_sud_ouest;
    private int v_prof_sud_est;
    private int v_plage_division;
    //proprietes
    public List<Noeud4Fils> ListeNoeud4Fils { get { return v_liste_noeud; } }
    public Noeud4Fils Racine { get { return v_racine; } }
    //constructeur
    public Arbre4Fils(int[,] tab_gris_LH, int cote, int plage_division) {
      v_tab_gris_LH = tab_gris_LH;
      v_cote_x = cote;
      v_cote_y = cote;
      v_pos_x = 0;
      v_pos_y = 0;
      v_liste_noeud = new List<Noeud4Fils>();
      v_prof_nord_ouest = 0;
      v_prof_nord_est = 0;
      v_prof_sud_ouest = 0;
      v_prof_sud_est = 0;
      v_plage_division = plage_division;
    }
    //segmenter l'arbre
    public void SegmenterArbre() {
      v_racine = new Noeud4Fils(v_pos_x, v_pos_y, v_cote_x, v_cote_y, "racine");
      int ecart = CalculerEcart(v_tab_gris_LH, v_racine);
      v_racine.Ecart = ecart;
      v_racine.ValeurGris = CalculerValeurGris(v_tab_gris_LH, v_racine);
      v_racine.Profondeur = 0;
      v_liste_noeud.Add(v_racine);
      if (ecart >= v_plage_division && v_cote_x > 1 && v_cote_y > 1) {
        v_prof_nord_ouest++;
        v_prof_nord_est++;
        v_prof_sud_ouest++;
        v_prof_sud_est++;
        SegmenterNoeud(v_racine);
      }
    }
    //segmenter un noeud de l'arbre
    private void SegmenterNoeud(Noeud4Fils noeud) {
      int cote_gauche = 0;
      int cote_droit = 0;
      if (noeud.CoteX % 2 == 0) {
        cote_gauche = noeud.CoteX / 2;
        cote_droit = noeud.CoteX / 2;
      }
      else {
        cote_gauche = noeud.CoteX / 2;
        cote_droit = (noeud.CoteX / 2) + 1;
      }
      int cote_haut = 0;
      int cote_bas = 0;
      if (noeud.CoteY % 2 == 0) {
        cote_haut = noeud.CoteY / 2;
        cote_bas = noeud.CoteY / 2;
      }
      else {
        cote_haut = noeud.CoteY / 2;
        cote_bas = (noeud.CoteY / 2) + 1;
      }
      //noeud nord ouest
      Noeud4Fils nord_ouest = new Noeud4Fils(noeud.PosX, noeud.PosY, cote_gauche, cote_haut, "nord-ouest");
      noeud.FilsNordOuest = nord_ouest;
      nord_ouest.Ecart = CalculerEcart(v_tab_gris_LH, nord_ouest);
      nord_ouest.ValeurGris = CalculerValeurGris(v_tab_gris_LH, nord_ouest);
      nord_ouest.Profondeur = v_prof_nord_ouest;
      v_liste_noeud.Add(nord_ouest);
      if (nord_ouest.Ecart >= v_plage_division && cote_gauche > 1 && cote_droit > 1 && cote_haut > 1 && cote_bas > 1) {
        v_prof_nord_ouest++;
        SegmenterNoeud(nord_ouest);
      }
      //noeud nord est
      Noeud4Fils nord_est = new Noeud4Fils(noeud.PosX + cote_gauche, noeud.PosY, cote_droit, cote_haut, "nord-est");
      noeud.FilsNordEst = nord_est;
      nord_est.Ecart = CalculerEcart(v_tab_gris_LH, nord_est);
      nord_est.ValeurGris = CalculerValeurGris(v_tab_gris_LH, nord_est);
      nord_est.Profondeur = v_prof_nord_est;
      v_liste_noeud.Add(nord_est);
      if (nord_est.Ecart >= v_plage_division && cote_gauche > 1 && cote_droit > 1 && cote_haut > 1 && cote_bas > 1) {
        v_prof_nord_est++;
        SegmenterNoeud(nord_est);
      }
      //noeud sud ouest
      Noeud4Fils sud_ouest = new Noeud4Fils(noeud.PosX, noeud.PosY + cote_haut, cote_gauche, cote_bas, "sud-ouest");
      noeud.FilsSudOuest = sud_ouest;
      sud_ouest.Ecart = CalculerEcart(v_tab_gris_LH, sud_ouest);
      sud_ouest.ValeurGris = CalculerValeurGris(v_tab_gris_LH, sud_ouest);
      sud_ouest.Profondeur = v_prof_sud_ouest;
      v_liste_noeud.Add(sud_ouest);
      if (sud_ouest.Ecart >= v_plage_division && cote_gauche > 1 && cote_droit > 1 && cote_haut > 1 && cote_bas > 1) {
        v_prof_sud_ouest++;
        SegmenterNoeud(sud_ouest);
      }
      //noeud sud est
      Noeud4Fils sud_est = new Noeud4Fils(noeud.PosX + cote_gauche, noeud.PosY + cote_haut, cote_droit, cote_bas, "sud-est");
      noeud.FilsSudEst = sud_est;
      sud_est.Ecart = CalculerEcart(v_tab_gris_LH, sud_est);
      sud_est.ValeurGris = CalculerValeurGris(v_tab_gris_LH, sud_est);
      sud_est.Profondeur = v_prof_sud_est;
      v_liste_noeud.Add(sud_est);
      if (sud_est.Ecart >= v_plage_division && cote_gauche > 1 && cote_droit > 1 && cote_haut > 1 && cote_bas > 1) {
        v_prof_sud_est++;
        SegmenterNoeud(sud_est);
      }
    }
    //
    private int CalculerEcart(int[,] tab_gris_LH, Noeud4Fils noeud) {
      int ecart_min_max = 999;
      List<int> liste = new List<int>();
      for (int lig = noeud.PosY; lig <= (noeud.PosY + noeud.CoteY - 1); lig++) {
        for (int col = noeud.PosX; col <= (noeud.PosX + noeud.CoteX - 1); col++) {
          liste.Add(tab_gris_LH[lig, col]);
        }
      }
      if (liste.Count != 0) {
        ecart_min_max = liste.Max() - liste.Min();
      }
      return ecart_min_max;
    }
    //
    private int CalculerValeurGris(int[,] tab_gris_LH, Noeud4Fils noeud) {
      int valeur_gris = -1;
      List<int> liste = new List<int>();
      for (int lig = noeud.PosY; lig <= (noeud.PosY + noeud.CoteY - 1); lig++) {
        for (int col = noeud.PosX; col <= (noeud.PosX + noeud.CoteX - 1); col++) {
          liste.Add(tab_gris_LH[lig, col]);
        }
      }
      int somme = 0;
      for (int xx = 0; xx < liste.Count; xx++) {
        somme += liste[xx];
      }
      valeur_gris = somme / liste.Count;
      return valeur_gris;
    }
  }//end class
}

