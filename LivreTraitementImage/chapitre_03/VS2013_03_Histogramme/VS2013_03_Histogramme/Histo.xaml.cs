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

namespace VS2013_03_Histogramme
{
    /// <summary>
    /// Logique d'interaction pour Histo.xaml
    /// </summary>
    public partial class Histo : UserControl
    {
        //enumeration
        public enum TypeComposante
        {
            niveau_de_gris,
            composante_R,
            composante_G,
            composante_B
        };

        //champs
        private int v_nbre_pix_maxi = 0;

        //proprietes
        public int[] RepartitionPixel { set; private get; }

        public string Titre { set; private get; }

        public TypeComposante Composante { set; private get; }

        //constructeur
        public Histo()
        {
            InitializeComponent();
        }

        //usercontrol evenement Loaded
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.UpdateLayout();
            x_text_titre.Text = this.Titre;
            InscrireEtiquetteNbPixel();
            AjouterBarreVerticale();
            EtalonnageBarreCouleur();
        }

        //
        private void InscrireEtiquetteNbPixel()
        {
            for (int lig = 0; lig <= 255; lig++)
            {
                if (this.RepartitionPixel[lig] > v_nbre_pix_maxi)
                {
                    v_nbre_pix_maxi = this.RepartitionPixel[lig];
                }
            }
            x_text_nb_pix_max.Text = v_nbre_pix_maxi.ToString("### ### ###");
            x_text_nb_pix_1tiers.Text = (v_nbre_pix_maxi / 3).ToString("### ### ###");
            x_text_nb_pix_2tiers.Text = (2 * v_nbre_pix_maxi / 3).ToString("### ### ###");
        }

        //
        private void AjouterBarreVerticale()
        {
            double hauteur_stack = x_stack_barre.ActualHeight;
            for (int lig = 0; lig <= 255; lig++)
            {
                int nbre = this.RepartitionPixel[lig];
                Rectangle barre = new Rectangle();
                barre.Width = 2;
                barre.Height = (nbre * hauteur_stack) / v_nbre_pix_maxi;
                barre.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                barre.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                barre.StrokeThickness = 0;
                barre.Fill = new SolidColorBrush(Colors.Black);
                x_stack_barre.Children.Add(barre);
            }
        }

        //
        private void EtalonnageBarreCouleur()
        {
            LinearGradientBrush degrade = new LinearGradientBrush();
            degrade.StartPoint = new Point(0, 0.5);
            degrade.EndPoint = new Point(1, 0.5);
            GradientStop couleur_debut = new GradientStop();
            couleur_debut.Color = Colors.Black;
            couleur_debut.Offset = 0;
            degrade.GradientStops.Add(couleur_debut);
            GradientStop couleur_fin = new GradientStop();
            couleur_fin.Offset = 1;
            if (this.Composante == TypeComposante.niveau_de_gris)
            {
                couleur_fin.Color = Colors.White;
            }
            if (this.Composante == TypeComposante.composante_R)
            {
                couleur_fin.Color = Color.FromArgb(255, 255, 0, 0);
            }
            if (this.Composante == TypeComposante.composante_G)
            {
                couleur_fin.Color = Color.FromArgb(255, 0, 255, 0);
            }
            if (this.Composante == TypeComposante.composante_B)
            {
                couleur_fin.Color = Color.FromArgb(255, 0, 0, 255);
            }
            degrade.GradientStops.Add(couleur_fin);
            x_rect_etalonnage.Fill = degrade;
        }
    } //end class
}