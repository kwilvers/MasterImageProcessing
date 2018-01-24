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

namespace VS2013_02_TransPuissance
{
    /// <summary>
    /// Logique d'interaction pour TableLut.xaml
    /// </summary>
    public partial class TableLut : UserControl
    {
        //
        public delegate double FonctionCalcul(double x);

        //constructeur
        public TableLut()
        {
            InitializeComponent();
        }

        //usercontrol evenement Loaded
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.UpdateLayout();
        }

        //ajouter les points de la courbe en fonction d'une équation
        public void ModeliserCourbe(FonctionCalcul fonction)
        {
            Polyline courbe = new Polyline();
            courbe.Stroke = new SolidColorBrush(Colors.Black);
            courbe.StrokeThickness = 3;
            courbe.Fill = new SolidColorBrush(Colors.Transparent);
            PointCollection collect = new PointCollection();
            for (double xx = 0; xx <= 255; xx += 0.1)
            {
                Point pt = new Point();
                pt.X = xx;
                pt.Y = fonction(xx);
                collect.Add(pt);
            }
            courbe.Points = collect;
            x_cnv_courbe.Children.Add(courbe);
        }
    } //end class
}