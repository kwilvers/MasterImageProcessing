
# Regression Polynomiale

# Importer les librairies

import numpy as np
import pandas as pd
import matplotlib.pyplot as plt

dataset = pd.read_csv("Position_Salaries.csv")
X = dataset.iloc[:, 1:2].values
y = dataset.iloc[:, -1].values

from sklearn.linear_model import LinearRegression
from sklearn.preprocessing import PolynomialFeatures
#poly_reg = PolynomialFeatures(degree = 2)   #colonne niveau au carré pas top
#poly_reg = PolynomialFeatures(degree = 3)   #colonne niveau au carré c'est mieux
#poly_reg = PolynomialFeatures(degree = 4)   #colonne niveau au carré c'est encore mieux
poly_reg = PolynomialFeatures(degree = 5)   #colonne niveau au carré attention parfait mais aver fitting!!! le modele a trop bien apris
X_poly = poly_reg.fit_transform(X)          #ajoute la colonne (deux colonne sont ajoutée la constante 1 et les polynome)
regressor = LinearRegression()
regressor.fit(X_poly, y)


plt.scatter(X, y, color='red')
#plt.plot(X, regressor.predict(X), color='blue')
plt.plot(X, regressor.predict(X_poly), color='green')
plt.title('salaire vs experience')
plt.xlabel('experience')
plt.ylabel('salaire')
plt.show()