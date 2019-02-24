
#Recurrent Neural Neworks
# Part 1 - Data Preprocessing

# Importing the libraries
import numpy as np
import matplotlib.pyplot as plt
import pandas as pd


''' Importer jeu d'entrainement '''
dataset_train = pd.read_csv("google_stock_price_train.csv")
training_set = dataset_train[["Open"]].values


''' Feature scalling '''
# normalisation pour les RNN et les fonctions sigmoïd
from sklearn.preprocessing import MinMaxScaler
# Les valeurs seront comprisent entre 0 et 1
sc = MinMaxScaler(feature_range=(0,1))
# mise à l'échelle
training_set_scaled = sc.fit_transform(training_set)


''' Création de la structure avec 60 timestamps soit 60 jours et 1 sortie de prédiction'''
# 60 timestamp
X_train = []
# 1 sortie
y_train = []

for i in range(60, 1258):
    # prendre les 60 jours pour l'entrainement
    X_train.append(training_set_scaled[(i-60):i, 0])
    # prendre le 61 jour pour la prédiction
    y_train.append(training_set_scaled[i, 0])
    
# Convertion en array : 1097 lignes de 60 colonnes
# 60 valeurs des 60 derniers jours donc les valeurs se retouvent dans 1 à 60 lignes
X_train = np.array(X_train)
# convertion en array
y_train = np.array(y_train)


''' Reshaping pour obtenir un tableau a trois dimensions '''
#   batch_size : Le nombre de lignes, 
#   timesteps : Le nombres de colonne soit les timestep,
#   input_dim : Le nombre d'action à coréler, ici 1 (google) mais elle pourait dépendre  
#    par exemple de youtube il faudrait alors mettre 2
X_train = np.reshape(X_train,(X_train.shape[0], X_train.shape[1], 1))


''' Librairies'''
from keras.models import Sequential
from keras.layers import Dense
from keras.layers import LSTM
from keras.layers import Dropout


''' Model de regression '''
regressor = Sequential()


''' Couche 1 : LSTM '''
# units=50 nombre de neuronnes (arbitraire)
# return_sequences=true on veut faire plusieur couches
# input_shape=(X_train.shape[1], 1) nombre de neuronne en entrée, les mêmes que dans le reshape sans les lignes
regressor.add(LSTM(units=50, return_sequences=True, input_shape=(X_train.shape[1], 1)))

# Couche de désactivation des neuronnes pour réduire le sur entrainement
regressor.add(Dropout(0.2))


''' Couche 2 : LSTM '''
# units=50 nombre de neuronnes (arbitraire)
# return_sequences=true on veut faire plusieur couches
# input_shape= Seulement dans la 1er couche !!!
regressor.add(LSTM(units=50, return_sequences=True))

# Couche de désactivation des neuronnes pour réduire le sur entrainement
regressor.add(Dropout(0.2))


''' Couche 3 : LSTM '''
# units=50 nombre de neuronnes (arbitraire)
# return_sequences=true on veut faire plusieur couches
regressor.add(LSTM(units=50, return_sequences=True))

# Couche de désactivation des neuronnes pour réduire le sur entrainement
regressor.add(Dropout(0.2))


''' Couche 4 : LSTM '''
# units=50 nombre de neuronnes (arbitraire)
# return_sequences=False plus de couche, c'est la dernière couches
regressor.add(LSTM(units=50, return_sequences=False))

# Couche de désactivation des neuronnes pour réduire le sur entrainement
regressor.add(Dropout(0.2))


''' Couche de sortie '''
# units=1 un neuronne de sortie qui est  le prix de l'action
regressor.add(Dense(units=1))


''' Compilation du réseau'''
# optimizer='Adam' fonctionne bien par défaut mais on pourait utiliser d'autres (a essayer)
# loss='' fonction de cout erreur quadratique moyenne (Y-Y')2
regressor.compile(optimizer='Adam', loss='mean_squared_error')


''' Entrainement '''
# epochs=100 le nombre d'époque d'entrainement : arbitraire
# batch_size=32 calcul la fonction de cout et retropropage toutes les 32 observations
regressor.fit(X_train, y_train, epochs=100, batch_size=32)



''' PREDICTION '''
''' Chargement données de test'''
# Chargement des données 2017
dataset_test = pd.read_csv("google_stock_price_test.csv")
real_stock_price = dataset_test[["Open"]].values


''' Création du jeu de test '''
# Pour prédire les jours de janvier, il faut les 60 jours précédents
# Il va falior concaténer les données d'entrainement et de test 
# puisque les données de test commence le 3 janvier et le training 
# sont les données de 2012 à 2016 contenant le 60 jours précédents 
# (dataset_train['Open'], dataset_test['Open']) la liste des dataset à concaténer
# axis=0 axe des lignes
dataset_total = pd.concat((dataset_train['Open'], dataset_test['Open']), axis=0)

# On garde les 60 jours précédent
# len(dataset_total)-len(dataset_test)-60 est l'index de départ
# : représente les données jusqu'a la fin
# .values pour obtenir une liste de 80 valeurs, les 60 jours avant et les 20 jours du jeu de test
inputs = dataset_total[len(dataset_total)-len(dataset_test)-60:].values
# mise en 2d pour le scaling
inputs = inputs.reshape(-1,1)

''' Feature scaling '''
# Le même objet que pour les données d'entrainement mais 
# ATTENTION transfort et pas fit_transform pour ne pas qu'il change de méthode
inputs = sc.transform(inputs)


''' Reshaping pour obtenir un tableau a trois dimensions '''
# 60 timestamp
X_test = []

for i in range(60, 80):
    # prendre les 60 jours pour l'entrainement
    X_test.append(inputs[(i-60):i, 0])
X_test = np.array(X_test)

#   batch_size : Le nombre de lignes, 
#   timesteps : Le nombres de colonne soit les timestep,
#   input_dim : Le nombre d'action à coréler, ici 1 (google) mais elle pourait dépendre  
#    par exemple de youtube il faudrait alors mettre 2
X_test = np.reshape(X_test,(X_test.shape[0], X_test.shape[1], 1))


''' Prédiction '''
predictive = regressor.predict(X_test)
# Transformation inverse de mise a l'echele
predictive = sc.inverse_transform(predictive)


''' Visualisation '''
plt.plot(real_stock_price, color='red', label='Prix réel')
plt.plot(predictive, color='green', label='Prix prédit')
plt.title('Google Stock Price Prediction')
plt.xlabel('Time')
plt.ylabel('Google Stock Price')
plt.legend()
plt.show()





















