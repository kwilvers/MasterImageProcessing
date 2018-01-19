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
  public class Noeud4Fils {
    //champs
    private string v_nom = "";
    private int v_pos_x;
    private int v_pos_y;
    private int v_cote_x;
    private int v_cote_y;
    private Noeud4Fils v_fils_nord_ouest = null;
    private Noeud4Fils v_fils_nord_est = null;
    private Noeud4Fils v_fils_sud_ouest = null;
    private Noeud4Fils v_fils_sud_est = null;
    private int v_profondeur = -1;
    private int v_ecart = -1;
    private int v_valeur_gris = -1;
    private int v_etiquette = -1;
    //proprietes
    public Noeud4Fils FilsNordOuest {
      get { return v_fils_nord_ouest; }
      set { v_fils_nord_ouest = value; }
    }
    public Noeud4Fils FilsNordEst {
      get { return v_fils_nord_est; }
      set { v_fils_nord_est = value; }
    }
    public Noeud4Fils FilsSudOuest {
      get { return v_fils_sud_ouest; }
      set { v_fils_sud_ouest = value; }
    }
    public Noeud4Fils FilsSudEst {
      get { return v_fils_sud_est; }
      set { v_fils_sud_est = value; }
    }
    public int Ecart {
      set { v_ecart = value; }
      get { return v_ecart; }
    }
    public int Profondeur {
      set { v_profondeur = value; }
      get { return v_profondeur; }
    }
    public int PosX { get { return v_pos_x; } }
    public int PosY { get { return v_pos_y; } }
    public int CoteX {
      get { return v_cote_x; }
      set { v_cote_x = value; }
    }
    public int CoteY {
      get { return v_cote_y; }
      set { v_cote_y = value; }
    }
    public string Nom {
      get { return v_nom; }
    }
    public int ValeurGris {
      get { return v_valeur_gris; }
      set { v_valeur_gris = value; }
    }
    public int Etiquette {
      get { return v_etiquette; }
      set { v_etiquette = value; }
    }
    //constructeur
    public Noeud4Fils(int pos_x, int pos_y, int cote_x, int cote_y, string nom) {
      v_pos_x = pos_x;
      v_pos_y = pos_y;
      v_cote_x = cote_x;
      v_cote_y = cote_y;
      v_fils_nord_ouest = null;
      v_fils_nord_est = null;
      v_fils_sud_ouest = null;
      v_fils_sud_est = null;
      v_nom = nom;
    }
    //
    public override string ToString() {
      string aff = "";
      aff += v_nom + " P=" + v_profondeur.ToString("00");
      aff += " -> xy (" + v_pos_x.ToString("00") + "," + v_pos_y.ToString("00") + ")";
      aff += " cx=" + v_cote_x.ToString("00") + " cy=" + v_cote_y.ToString("00");
      aff += " ec=" + v_ecart.ToString() + " vg=" + v_valeur_gris.ToString("000");
      return aff;
    }
    //regions identiques
    public static bool RegionIdentique(Noeud4Fils region_1, Noeud4Fils region_2) {
      bool identique = false;
      if (region_1.PosX == region_2.PosX) {
        if (region_1.PosY == region_2.PosY) {
          if (region_1.CoteX == region_2.CoteX) {
            if (region_1.CoteY == region_2.CoteY) {
              identique = true;
            }
          }
        }
      }
      return identique;
    }
    //affichage des regions etiquetees
    public string AffichageRegionEtiquetee() {
      string aff = "";
      aff += v_nom;
      aff += " -> xy (" + v_pos_x.ToString("00") + "," + v_pos_y.ToString("00") + ")";
      aff += " cx=" + v_cote_x.ToString("00") + " cy=" + v_cote_y.ToString("00");
      aff += " vg=" + v_valeur_gris.ToString("000") + " etiq=" + v_etiquette.ToString();
      return aff;
    }
  }//end class
}
