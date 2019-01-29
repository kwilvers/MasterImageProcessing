# Regression Logistique

# Importer les librairies
import numpy as np
import matplotlib.pyplot as plt
import pandas as pd

# Importer le dataset
dataset = pd.read_csv('Social_Network_Ads.csv')
X = dataset.iloc[:, [2,3]].values
y = dataset.iloc[:, -1].values

# Diviser le dataset entre le Training set et le Test set
from sklearn.model_selection import train_test_split
X_train, X_test, y_train, y_test = train_test_split(X, y, test_size = 0.25, random_state = 0)


# Mise a l'échelle des valeurs en utiliqsant l'ecart type 
from sklearn.preprocessing import StandardScaler
sc = StandardScaler()
X_train = sc.fit_transform(X_train)
X_test = sc.transform(X_test)
kk = np.matrix([[42, 55000]])   #création d'une matrice pour faire une prédiction de karl :)
k = sc.transform(kk)            #standardisation des valeurs 42, 55000 --> 0.383585, -0.422817

# Construction du modèle
from sklearn.linear_model import LogisticRegression
classifier = LogisticRegression(random_state = 0)
classifier.fit(X_train, y_train)

# Faire de nouvelles prédictions
y_pred = classifier.predict(X_test)     #prédiction du test set
k_pred = classifier.predict(k)          #réalisation de la prédiction

# Matrice de confusion
from sklearn.metrics import confusion_matrix
cm = confusion_matrix(y_test, y_pred)

# Visualiser les résultats
#les points vert on acheté la voiture et les points rouge n'ont pas acheté la voitue
#la région verte prédit l'achat de la voiture la région rouge prédit l'inverse
from matplotlib.colors import ListedColormap

#a choisir le training set ou le test set
#X_set, y_set = X_train, y_train   # Affichage du training set
X_set, y_set = X_test, y_test     # affichage du test set
# creation de la grid age/salaire
X1, X2 = np.meshgrid(np.arange(start = X_set[:, 0].min() - 1, stop = X_set[:, 0].max() + 1, step = 0.01),
                     np.arange(start = X_set[:, 1].min() - 1, stop = X_set[:, 1].max() + 1, step = 0.01))
#tracer la frontière de prédiction et les régions dont le alha permet d'atténuer le rouge et le vert (contraste)
plt.contourf(X1, X2, classifier.predict(np.array([X1.ravel(), X2.ravel()]).T).reshape(X1.shape),
             alpha = 0.4, cmap = ListedColormap(('red', 'green')))
# tracer les utilisateur, les point rouge et vert
plt.xlim(X1.min(), X1.max())
plt.ylim(X2.min(), X2.max())
for i, j in enumerate(np.unique(y_set)):
    plt.scatter(X_set[y_set == j, 0], X_set[y_set == j, 1],
                c = ListedColormap(('red', 'green'))(i), label = j)
    
#titres et légendes
plt.title('Résultats du Training set')
plt.xlabel('Age')
plt.ylabel('Salaire Estimé')
plt.legend()
plt.show()