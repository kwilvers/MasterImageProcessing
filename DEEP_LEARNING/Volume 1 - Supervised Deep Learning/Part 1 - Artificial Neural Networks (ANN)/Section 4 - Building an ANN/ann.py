
#----------------------------------
#Partie 1 : Préparation des données
#----------------------------------

import numpy as np
import matplotlib.pyplot as plt
import pandas as pd


#Importation dataset
dataset = pd.read_csv('.\Churn_Modelling.csv')
#Création de l'ensemble des variables indépendantes
X = dataset.iloc[:, 3:13].values
#Création de la variable dépendante
Y = dataset.iloc[:, [13]].values


#Catégorisé les variables indépendantes (france, germany, ..., Male, Female)
from sklearn.preprocessing import LabelEncoder, OneHotEncoder
#Encoder de pays colonne 1 dans X
labelencoder_x_1 = LabelEncoder()
X[:, 1] = labelencoder_x_1.fit_transform(X[:, 1])
#Encoder de genre colonne 2 dans X
labelencoder_x_2 = LabelEncoder()
X[:, 2] = labelencoder_x_2.fit_transform(X[:, 2])
#Créer les variables indépendantes à partir des valeurs cathégorisée des colonnes
onehotencoder = OneHotEncoder(categorical_features=[1])
X = onehotencoder.fit_transform(X).toarray()
X = X[:, 1:]

#Division  de dataset en training set  et test  set
from sklearn.model_selection import train_test_split
X_train, X_test, Y_train, Y_test = train_test_split(X, Y, test_size = 0.2, random_state = 0)


#Changer l'échelle des variables (feature scaling)
from sklearn.preprocessing import StandardScaler
sc = StandardScaler()
X_train = sc.fit_transform(X_train)
X_test = sc.fit_transform(X_test)


#----------------------------------
#Partie 2 : Préparation des données
#----------------------------------
import keras
from keras.models import Sequential     #module initialisation du reseau de neurone
from keras.layers import Dense          #module des couches du réseau de neurones


# Entrainement
#1.  initialiser les poids avec des valeurs proche de 0
#2.  Envoyer les valeurs dans la couche d'entree (une variable par neurone)
#3.  Propagation de gauche a droite des neurones, activation des neurones avec la fonction d'activation
#    Fontion redresseur pour les couches et sigmoide pour la couche de sortie
#4.  Comparer prédiction et valeur réel ==> calculer la fonction de coût
#5.  Propagation arrière de droite a gauche, maj des poids selon leur responsabilité dans l'erreur
#6.  Repeter étape 1 à 5 et ajuster les poids après chaque observation (chaque ligne du training set)
#7.  Fin du traitement d'une époque, rejouer plusieur époque

#INITIALISER ET CEER LE RESEAU DE NEURONES
#-----------------------------------------
#Initialiser le reseau de neurone
classifier = Sequential()


#Définir les couches 
#La couche d'entrée est ajoutée automatiquement lors de la première couche cachée

#Ajouter la première couche cachée
#Units = nombre de neurone des couche cachées : nombre de variable d'entre +variable de sortie /2 = 6
#Activation = fonction d'activation relu:redresseur
#kernel_initializer = initialisation des poids d'entrée
#input_dim = nombre de variable d'entree, la couche d'entrée
classifier.add(Dense(units=6, activation="relu", 
                     kernel_initializer="uniform", input_dim=11))

#Ajouter deuxième couche cachée
classifier.add(Dense(units=6, activation="relu", 
                     kernel_initializer="uniform"))

#Ajouter la couche de sortie
#Units = 1 neuronne de sortie
#Activation sigmoide pour une cathégorie a deux valeur 1 ou 0 (le client reste ou part) 
#           softmax si plus de deux neurone de sortie
classifier.add(Dense(units=1, activation="sigmoid", 
                     kernel_initializer="uniform"))


#Compiler le reseau de neurone a l'aide de l'algorithme de gradient stochastique
#optimizer="adam" : algorithme de gradient stochastique
#loss="binary_crossentropy" : regression logistique (classification) fonction de cout logarithmique 
#      categorical_crossentropy si plus de deux cathégories
#metrics="accuracy" : mesure la performance du modèle époque par époque (ATTENTION liste)
classifier.compile(optimizer="adam", loss="binary_crossentropy", metrics=["accuracy"])


#ENTRAINER
#----------
#Entrainer le réseau de neurones avec les données de training X_train
#X_train, Y_train sont les jeux de données d'entrainement : d'entrée X et de sortie Y
#batch_size=10 : apprentissage par lot de 10 ligne du training set plutôt que renforcé (ligne par ligne)
#epochs=100 : entrainement des 8000 lignes sur 100 epoques
#             au final ==> loss(fct de cout): 0.3990 - acc(précission): 0.8331
classifier.fit(X_train, Y_train, batch_size=10, epochs=100)


#PREDICTION
#----------
#Réaliser des prédictions sur base du dataset de test
Y_pred = classifier.predict(X_test)


#MATRICE DE CONFUSION
#--------------------
#Classifier les prédictions qui sont en % de chance de quiter la banque en 
#valeurs bool puisque la matrice de confusion attend des 1 ou 0
#si Y_pred < 0.5 (50%) alors 0 (reste) sinon 1 (part)
#le pourcentage n'est pas nécessairement 50 %
Y_pred_matrice = (Y_pred > 0.5)

#Réaliser la matrice de confusion avec l'aide des résultats des prédictions 
#et des données réel
from sklearn.metrics import confusion_matrix
cm = confusion_matrix(Y_test, Y_pred_matrice)

#Calcul de la précision du modèle : ex. (1556+134)/2000=0.845 soit 84.5% de précision
#L'indice (0,0) représente les prédiction 0 pour des résultats 0
#L'indice (1,1) représente les prédiction 1 pour des résultats 1
#Soit les bonnes prédictions de ceux qui quittent la banque et ceux qui restent 
(cm[0,0]+cm[1,1])/2000







