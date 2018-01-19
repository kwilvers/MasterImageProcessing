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

namespace VS2013_06_MinimumMaximum {
  public class Filtre {
    //donnees
    private int[] v_coefficients;
    private int v_taille;
    private int v_normalisation;
    //constructeur
    public Filtre(int taille, int[] coefficients) {
      v_taille = taille;
      v_coefficients = coefficients;
      for (int xx = 0; xx < coefficients.Length; xx++) {
        v_normalisation += coefficients[xx];
      }
    }
    //
    public byte PixelFiltre(int[] voisins) {
      int niveau_calcule = 0;
      for (int xx = 0; xx < v_coefficients.Length; xx++) {
        niveau_calcule += voisins[xx] * v_coefficients[xx];
      }
      if (v_normalisation != 0) {
        niveau_calcule = niveau_calcule / v_normalisation;
      }
      return (byte)niveau_calcule;
    }
  }//end class
}
