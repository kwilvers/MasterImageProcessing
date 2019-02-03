# Regression Linéaire Simple

# Importer les librairies
import numpy as np
import matplotlib.pyplot as plt
import pandas as pd

# Importer le dataset
dataset = pd.read_csv('Salary_Data.csv')
#créer les variable indépendantes : années d'experience
X = dataset.iloc[:, :-1].values
#créer la variable dépendante : salaire
y = dataset.iloc[:, -1].values


#pas de données manquante
#pas de données cathégorique


# Diviser le dataset entre le Training set et le Test set
from sklearn.model_selection import train_test_split
X_train, X_test, y_train, y_test = train_test_split(X, y, test_size = 1.0/3, random_state = 0)


# pas de feature scaling car une seule variable indépendante


# Construction du modèle
from sklearn.linear_model import LinearRegression
# création du regresseur linéaire
regressor = LinearRegression()
# Fit, lie la variable dépendante avec les variables indépendantes ==> effectue la méthode des carré ordianaire
regressor.fit(X_train, y_train)


# Faire de nouvelles prédictions pour la variable indépendante expérience les salaires prédits
y_pred = regressor.predict(X_test)
regressor.predict(15)

# Visualiser les résultats
plt.scatter(X_test, y_test, color = 'red')
plt.plot(X_train, regressor.predict(X_train), color = 'blue')
plt.scatter(X_test, y_pred, color = 'orange')
plt.scatter(7, regressor.predict(7), color = 'black')
plt.title('Salaire vs Experience')
plt.xlabel('Experience')
plt.ylabel('Salaire')
plt.show()

