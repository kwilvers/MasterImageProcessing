
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

#OPTIMISATION 
#-------------
#Le training set est divisé en 10 training set dont 10% serviront au test set
#Il faut pour ça créer une fonction qui sera utilisée par KerasClassifier
from keras.wrappers.scikit_learn import KerasClassifier
from sklearn.model_selection import GridSearchCV

'''Optimiser les hypersparamètres : epochs, batch_size, units (nombre de neurones par couche)'''
#Fonction de création du reseau de neurone
def build_classifier(optimizer):
  classifier = Sequential()

  #Définir les couches 
  classifier.add(Dense(units=6, activation="relu", 
                       kernel_initializer="uniform", input_dim=11))

  #Ajouter deuxième couche cachée
  classifier.add(Dense(units=6, activation="relu", 
                       kernel_initializer="uniform"))

  #Ajouter la couche de sortie
  classifier.add(Dense(units=1, activation="sigmoid", 
                       kernel_initializer="uniform"))

  #Compiler le reseau de neurone a l'aide de l'algorithme de gradient stochastique
  classifier.compile(optimizer=optimizer, loss="binary_crossentropy", metrics=["accuracy"])
  return classifier


#ENTRAINER
#----------
#Creation du classifier KerasClassifier dont les paramètre sont identique 
#au fit du classifier Sequencial  
classifier = KerasClassifier(build_fn=build_classifier)

#création d'un dictionaire de paramètres 
#batchsize:[25,32] : diviseur du total de row ou puissance de 2
#epochs: [100, 500] : être sure que le modèle a fini de converger
#optimizer:["adam","rmsprop"]} : algo de gradient
parameters = {"batch_size":[25,32], "epochs": [100, 500], "optimizer":["adam","rmsprop"]}


#Liaison des parametres au classifier
#scoring="accuracy" : Mesurer la performance
#cv=10 : nombre de lot, il divise le dataset en 10 et utilise une pour le test
#        et 9 pour l'entrainement
grid_search = GridSearchCV(estimator=classifier, 
                           param_grid=parameters,
                           scoring="accuracy",
                           cv=10)


#Lance le test des 8 combinaisons possible de parametre
grid_search.fit(X_train, Y_train)


#Récdupère les meilleurs paramètres et précisions
best_parameters = grid_search.best_params_
best_precision = grid_search.best_score_

















