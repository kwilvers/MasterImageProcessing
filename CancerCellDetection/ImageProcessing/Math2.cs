using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageProcessing
{
    /**
	* @overview brève description du type ainsi que sa mutabilité 
	* @specfields nom:type //éléments nommé repris dans un n-uplet
	* @derivedfields nom:type //élément dérivé des @specfields
	* @invariant description des invariants abstrait qui doivent être vérifié à tout moment
	*/
    public static class Math2
    {
        /**
        * @requires préconditions : values != null && values.length > 0
        * @return La racine carrée de la somme des carrés des valeurs
        */
        public static double SumOfAbsoluteValues(this IEnumerable<double> values)
        {
            return values.ToList().Select(Math.Abs).Sum();
        }

        /**
        * @requires préconditions : values != null && values.length > 0
        * @return La racine carrée de la somme des carrés des valeurs
        */
        public static double SquareRootOfSumOfSquares(this IEnumerable<double> values)
        {
            return Math.Sqrt(values.ToList().Select(d => d * d).Sum());
        }

        /**
        * @effect Calcul la racine carrée de la somme des carrés des valeurs
        * @requires préconditions : values != null && values.length > 0
        * @return racine carré(dx^2 + dy^2 )
        */
        public static double SquareRootOfSumOfSquares(double dx, double dy)
        {
            return Math.Sqrt(Math.Pow(dx,2) + Math.Pow(dy,2) );
        }

        /**
        * @effect Calcul la valeur maximum des valeurs absolues
        * @requires préconditions
        * @return Max(|dx|, |dy|)
        */
        public static double MaxAbs(double dx, double dy)
        {
            return Math.Max(Math.Abs(dx), Math.Abs(dy));
        }

        /**
		* @effects Obtient la direction du gradient à l'aide de la fonction arc tangente
        * @requires préconditions : gx > 0 && gy > 0
        * @return gy si gx est = 0, 90 si gy = 0 sinon Atan(gy/gx)
        */
        public static double GradientDirection(double gx, double gy)
        {
            if (Math.Abs(gx) < double.Epsilon)
                return gy;

            if (Math.Abs(gy) < double.Epsilon)
                return 90;

            return Math.Atan(gy/gx);
        }

        /**
		* @requires préconditions b != 0
		* @throws DivideByZeroException si b == 0
		* @effects divise a par b
		* @return le résultat de la division de a par b
		*/
        public static double Div(double a, double b)
        {
            if (b == 0) throw new DivideByZeroException();

            return a / b;
        }
    }
}