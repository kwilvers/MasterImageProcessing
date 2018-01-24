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

namespace VS2013_07_ContourFreeman
{
    /// <summary>
    /// Logique d'interaction pour Representation12x12.xaml
    /// </summary>
    public partial class Representation12x12 : UserControl
    {
        //donnees
        private List<Rectangle> v_liste_rectangle = null;

        //constructeur
        public Representation12x12()
        {
            InitializeComponent();
            v_liste_rectangle = new List<Rectangle>();
            //positionnement des rectangles
            double deplac_h = 0;
            double deplac_v = 0;
            for (int lig = 0; lig < 12; lig++)
            {
                for (int col = 0; col < 12; col++)
                {
                    Rectangle rect = new Rectangle();
                    rect.Name = "x_rect_" + lig.ToString("00") + "_" + col.ToString("00"); //x_rect_00_00
                    rect.Width = 20;
                    rect.Height = 20;
                    rect.Fill = new SolidColorBrush(Colors.White);
                    rect.Stroke = new SolidColorBrush(Colors.Transparent);
                    rect.StrokeThickness = 0;
                    Canvas.SetLeft(rect, deplac_h);
                    Canvas.SetTop(rect, deplac_v);
                    x_cnv_grille.Children.Add(rect);
                    v_liste_rectangle.Add(rect);
                    deplac_h += 20;
                }
                deplac_h = 0;
                deplac_v += 20;
            }
            //positionnement du lignage
            deplac_h = 0;
            deplac_v = 0;
            for (int lig = 1; lig <= 13; lig++)
            {
                Line ligne = new Line();
                ligne.Width = 1;
                ligne.Height = 240;
                ligne.X1 = 0;
                ligne.Y1 = 0;
                ligne.X2 = 0;
                ligne.Y2 = 240;
                ligne.Stroke = new SolidColorBrush(Colors.Black);
                ligne.StrokeThickness = 1;
                ligne.StrokeDashArray = new DoubleCollection() {2, 2};
                Canvas.SetLeft(ligne, deplac_h);
                Canvas.SetTop(ligne, deplac_v);
                x_cnv_grille.Children.Add(ligne);
                deplac_h += 20;
            }
            deplac_h = 0;
            deplac_v = 0;
            for (int col = 1; col <= 13; col++)
            {
                Line ligne = new Line();
                ligne.Width = 240;
                ligne.Height = 1;
                ligne.X1 = 0;
                ligne.Y1 = 0;
                ligne.X2 = 240;
                ligne.Y2 = 0;
                ligne.Stroke = new SolidColorBrush(Colors.Black);
                ligne.StrokeThickness = 1;
                ligne.StrokeDashArray = new DoubleCollection() {2, 2};
                Canvas.SetLeft(ligne, deplac_h);
                Canvas.SetTop(ligne, deplac_v);
                x_cnv_grille.Children.Add(ligne);
                deplac_v += 20;
            }
        }

        //fixer le motif a partir d'un tableau de pixel codés en int
        public void FixerLeMotif(byte[,] tab_pixels_essai_LH)
        {
            try
            {
                //MessageBox.Show(v_liste_rectangle.Count.ToString());
                for (int lig = 0; lig < 12; lig++)
                {
                    for (int col = 0; col < 12; col++)
                    {
                        string nom = "x_rect_" + lig.ToString("00") + "_" + col.ToString("00");
                        byte niveau = tab_pixels_essai_LH[lig, col];
                        Color couleur = Color.FromArgb(255, niveau, niveau, niveau);
                        for (int xx = 0; xx < v_liste_rectangle.Count; xx++)
                        {
                            Rectangle rect = v_liste_rectangle[xx];
                            if (rect.Name == nom)
                            {
                                rect.Fill = new SolidColorBrush(couleur);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //visualiser les divisions obtenues
        public void AfficherLesDivisions(List<Noeud4Fils> liste_feuille, Color couleur_fond)
        {
            x_cnv_division.Visibility = Visibility.Visible;
            x_cnv_division.Children.Clear();
            for (int xx = 0; xx < liste_feuille.Count; xx++)
            {
                Noeud4Fils feuille = liste_feuille[xx];
                Point pixel = new Point(feuille.PosX, feuille.PosY);
                int cote_h = feuille.CoteX;
                int cote_v = feuille.CoteY;
                Rectangle rect = new Rectangle();
                rect.Name = "x_rect_feuille_" + xx.ToString("00");
                rect.Width = cote_h * 20;
                rect.Height = cote_v * 20;
                rect.Fill = new SolidColorBrush(couleur_fond);
                rect.Stroke = new SolidColorBrush(Colors.Black);
                rect.StrokeThickness = 1;
                Canvas.SetLeft(rect, pixel.X * 20);
                Canvas.SetTop(rect, pixel.Y * 20);
                x_cnv_division.Children.Add(rect);
            }
        }

        //visualiser les regions obtenues avec leurs valeurs de gris moyen
        public void AfficherLesRegionsAvecValeurGris(List<Noeud4Fils> liste_feuille)
        {
            x_cnv_region_gris.Visibility = Visibility.Visible;
            x_cnv_region_gris.Children.Clear();
            for (int xx = 0; xx < liste_feuille.Count; xx++)
            {
                Noeud4Fils feuille = liste_feuille[xx];
                Point pixel = new Point(feuille.PosX, feuille.PosY);
                int cote_h = feuille.CoteX;
                int cote_v = feuille.CoteY;
                TextBlock tb = new TextBlock();
                tb.Name = "x_text_feuille_gris_" + xx.ToString("00");
                tb.Width = 20; // cote_h * 20;
                tb.Height = 20; // cote_v * 20;
                tb.Text = feuille.ValeurGris.ToString();
                tb.FontSize = 7;
                tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                Canvas.SetLeft(tb, pixel.X * 20 + 3);
                Canvas.SetTop(tb, pixel.Y * 20 + 3);
                x_cnv_region_gris.Children.Add(tb);
            }
        }

        //visualiser les regions obtenues avec leurs etiquettes
        public void AfficherLesRegionsAvecEtiquettes(List<Noeud4Fils> liste_feuille)
        {
            x_cnv_region_etiquettes.Visibility = Visibility.Visible;
            x_cnv_region_etiquettes.Children.Clear();
            for (int xx = 0; xx < liste_feuille.Count; xx++)
            {
                Noeud4Fils feuille = liste_feuille[xx];
                Point pixel = new Point(feuille.PosX, feuille.PosY);
                int cote_h = feuille.CoteX;
                int cote_v = feuille.CoteY;
                TextBlock tb = new TextBlock();
                tb.Name = "x_text_feuille_etiquette_" + xx.ToString("00");
                tb.Width = 20; // cote_h * 20;
                tb.Height = 20; // cote_v * 20;
                tb.Text = feuille.Etiquette.ToString();
                tb.FontSize = 11;
                tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                Canvas.SetLeft(tb, pixel.X * 20 + 3);
                Canvas.SetTop(tb, pixel.Y * 20 + 3);
                x_cnv_region_etiquettes.Children.Add(tb);
            }
        }

        //visualiser les regions obtenues avec leurs couleurs segmentées
        public void AfficherLesRegionsAvecCouleurSegmentee(List<Noeud4Fils> liste_feuille)
        {
            x_cnv_region_couleur.Visibility = Visibility.Visible;
            x_cnv_region_couleur.Children.Clear();
            for (int xx = 0; xx < liste_feuille.Count; xx++)
            {
                Noeud4Fils feuille = liste_feuille[xx];
                Point pixel = new Point(feuille.PosX, feuille.PosY);
                int cote_h = feuille.CoteX;
                int cote_v = feuille.CoteY;
                Rectangle rect = new Rectangle();
                rect.Name = "x_rect_feuille_couleur_" + xx.ToString("00");
                rect.Width = cote_h * 20;
                rect.Height = cote_v * 20;
                Color couleur = Color.FromArgb(255, (byte) feuille.ValeurGris, (byte) feuille.ValeurGris,
                    (byte) feuille.ValeurGris);
                rect.Fill = new SolidColorBrush(couleur);
                rect.Stroke = new SolidColorBrush(Colors.Black);
                rect.StrokeThickness = 0;
                Canvas.SetLeft(rect, pixel.X * 20);
                Canvas.SetTop(rect, pixel.Y * 20);
                x_cnv_region_couleur.Children.Add(rect);
            }
        }

        //convertir un int en objet color
        private Color CorrespondanceIntVersColor(int couleur_int)
        {
            Color couleur = new Color();
            couleur.A = (byte) (couleur_int >> 24);
            couleur.R = (byte) (couleur_int >> 16);
            couleur.G = (byte) (couleur_int >> 8);
            couleur.B = (byte) (couleur_int);
            return couleur;
        }

        //convertir un color en int
        private int CorrespondanceColorVersInt(Color couleur)
        {
            int couleur_int = 0;
            couleur_int = (couleur.A << 24) | (couleur.R << 16) | (couleur.G << 8) | couleur.B;
            return couleur_int;
        }
    } //end class
}