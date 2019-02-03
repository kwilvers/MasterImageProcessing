# Data preprocessing

#Imports
import numpy as np
import matplotlib.pyplot as plt
import pandas as pd

# lire le dataset
dataset = pd.read_csv('data.csv')

# Créer la matrice des var indépendante et le vecteur de la var dépendante
x = dataset.iloc[:,:-1].values #charge les var indépendantes :toute les lignes et :-1 les colonne la dernière
y = dataset.iloc[:, -1].values #charge la var dépendantes :toute les lignes et :-1 la dernière colonne


# gérer les données manquante
#importe un librairie
from sklearn.preprocessing import Imputer   
#remplace les données manquantes par la moyenne de la colonne
imputer = Imputer(missing_values='NaN', strategy='mean', axis=0)  
# calcule les moyenne aux données manquantes pour toutes les lignes des colonne 1 et 2
imputer.fit(x[:,[1,2]])    
# applique la moyenne en remplacant les données dans x
x[:, [1,2]] = imputer.transform(x[:, [1,2]])  


#gérer les variable cathégoriques (country et purchased)
from sklearn.preprocessing import LabelEncoder, OneHotEncoder

#encoder les country par dummy variable ==> créer une colonne par variable
#ici country n'a pas de relation d'ordre, un pays n'est pas avant un autre ==> variable nominale
labelencoder_X = LabelEncoder()
#Effectue le fit et le transform simultanément entransformant les textes en nombre
x[:,0] = labelencoder_X.fit_transform(x[:,0])
#demande d'encoder la colonne 0 en dummy variable
onehotencoder = OneHotEncoder(categorical_features = [0])
#effectue le fit et le transform pour créer les trois colonne
x = onehotencoder.fit_transform(x).toarray()

#traite le vecteur purchased par 0 et 1
labelencoder_y = LabelEncoder()
#effectue le fit et le transform pour modifier no=0 et yes=1
y = labelencoder_y.fit_transform(y)


# Diviser le dataset entre le Training set et le Test set
from sklearn.model_selection import train_test_split
# divise le datatset en trainig set et en test set ==> retourne un training set pour x et y et un test set pour x et y
x_train, x_test, y_train, y_test = train_test_split(x, y, test_size=0.2, random_state=0)


# Mise à l'echelle des variable pour que age et salaire aient la même pondération
#pour ne pas que salaire écrase age ou coountry
from sklearn.preprocessing import StandardScaler
#on va standardiser les variables : Xstand = (X-moyenne(x))/ecart type(x)
sc = StandardScaler()
#lié le feature scalling sur les variables indépendante pour chaque variable indépendante de x_train et le x_test
x_train = sc.fit_transform(x_train)
#on ne fit pas le test puisqu'il est fitter sur le training set on fait juste le transform
x_test = sc.transform(x_test)




