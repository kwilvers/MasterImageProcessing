
#Recurrent Neural Neworks
# Part 1 - Data Preprocessing

# Importing the libraries
import numpy as np
import matplotlib.pyplot as plt
import pandas as pd

''' AJOUT DE LA VALEUR DE SORTIE DE L ACTION '''
''' DONC deux valriable pour le régresseur '''

#''' Importer jeu d'entrainement '''
dataset_train = pd.read_csv("google_stock_price_train.csv")
''' change le nombre de valeur que l'on sélectionne open et hight'''
training_set = dataset_train.iloc[:, 1:3].values


#''' Feature scalling '''
# normalisation pour les RNN et les fonctions sigmoïd
from sklearn.preprocessing import MinMaxScaler
# Les valeurs seront comprisent entre 0 et 1
sc = MinMaxScaler(feature_range=(0,1))
# mise à l'échelle
training_set_scaled = sc.fit_transform(training_set)


#''' Création de la structure avec 60 timestamps soit 60 jours et 1 sortie de prédiction'''
# 60 timestamp
X_train = []
# 1 sortie
y_train = []

''' Pour y ça change pas'''
for i in range(60, 1258):
    # prendre le 61 jour pour la prédiction
    y_train.append(training_set_scaled[i, 0])
# convertion en array
y_train = np.array(y_train)


''' pour X on va traiter chacunes des variables du regresseur ''' 
for variable in range(0, 2):
    X = []
    for i in range(60, 1258):
        X.append(training_set_scaled[i-60:i, variable])
    X, np.array(X)
    X_train.append(X)
# Convertion en array : 1097 lignes de 60 colonnes
# 60 valeurs des 60 derniers jours donc les valeurs se retouvent dans 1 à 60 lignes
X_train = np.array(X_train)


#''' Reshaping pour obtenir un tableau a trois dimensions '''
#   batch_size : Le nombre de lignes, 
#   timesteps : Le nombres de colonne soit les timestep,
#   input_dim : Le nombre d'action à coréler, ici 1 (google) mais elle pourait dépendre  
#    par exemple de youtube il faudrait alors mettre 2
''' La troisième dimension existe mais à la mauvaisse place (2,1198,60) --> (2,1198,60) '''
X_train = np.swapaxes(np.swapaxes(X_train, 0, 1), 1, 2)


#''' Librairies'''
from keras.models import Sequential
from keras.layers import Dense
from keras.layers import LSTM
from keras.layers import Dropout


#''' Model de regression '''
regressor = Sequential()


#''' Couche 1 : LSTM '''
# units=50 nombre de neuronnes (arbitraire)
# return_sequences=true on veut faire plusieur couches
# input_shape=(X_train.shape[1], 1) nombre de neuronne en entrée, les mêmes que dans le reshape sans les lignes
''' input_shape=(X_train.shape[1], 2)  deux variables d'entrée '''
regressor.add(LSTM(units=50, return_sequences=True, input_shape=(X_train.shape[1], 2)))

# Couche de désactivation des neuronnes pour réduire le sur entrainement
regressor.add(Dropout(0.2))


#''' Couche 2 : LSTM '''
# units=50 nombre de neuronnes (arbitraire)
# return_sequences=true on veut faire plusieur couches
# input_shape= Seulement dans la 1er couche !!!
regressor.add(LSTM(units=50, return_sequences=True))

# Couche de désactivation des neuronnes pour réduire le sur entrainement
regressor.add(Dropout(0.2))


#''' Couche 3 : LSTM '''
# units=50 nombre de neuronnes (arbitraire)
# return_sequences=true on veut faire plusieur couches
regressor.add(LSTM(units=50, return_sequences=True))

# Couche de désactivation des neuronnes pour réduire le sur entrainement
regressor.add(Dropout(0.2))


#''' Couche 4 : LSTM '''
# units=50 nombre de neuronnes (arbitraire)
# return_sequences=False plus de couche, c'est la dernière couches
regressor.add(LSTM(units=50, return_sequences=False))

# Couche de désactivation des neuronnes pour réduire le sur entrainement
regressor.add(Dropout(0.2))


#''' Couche de sortie '''
# units=1 un neuronne de sortie qui est  le prix de l'action
regressor.add(Dense(units=1))


#''' Compilation du réseau'''
# optimizer='Adam' fonctionne bien par défaut mais on pourait utiliser d'autres (a essayer)
# loss='' fonction de cout erreur quadratique moyenne (Y-Y')2
regressor.compile(optimizer='Adam', loss='mean_squared_error')


#''' Entrainement '''
# epochs=100 le nombre d'époque d'entrainement : arbitraire
# batch_size=32 calcul la fonction de cout et retropropage toutes les 32 observations
regressor.fit(X_train, y_train, epochs=100, batch_size=32)



''' PREDICTION '''
#''' Chargement données de test'''
# Chargement des données 2017
dataset_test = pd.read_csv("google_stock_price_test.csv")
''' Chargement des données open et hight '''
real_stock_price = dataset_test.iloc[:, 1:3].values

#''' Création du jeu de test '''
# Pour prédire les jours de janvier, il faut les 60 jours précédents
# Il va falior concaténer les données d'entrainement et de test 
# puisque les données de test commence le 3 janvier et le training 
# sont les données de 2012 à 2016 contenant le 60 jours précédents 
# (dataset_train['Open'], dataset_test['Open']) la liste des dataset à concaténer
# axis=0 axe des lignes
''' Concaténation des données des colonnes open et hight '''
dataset_total = pd.concat((dataset_train.iloc[:, 1:3], dataset_test.iloc[:, 1:3]), axis=0)

# On garde les 60 jours précédent
# len(dataset_total)-len(dataset_test)-60 est l'index de départ
# : représente les données jusqu'a la fin
# .values pour obtenir une liste de 80 valeurs, les 60 jours avant et les 20 jours du jeu de test
inputs = dataset_total[len(dataset_total)-len(dataset_test)-60:].values
# mise en 2d pour le scaling
#inputs = inputs.reshape(-1,1)

#''' Feature scaling '''
# Le même objet que pour les données d'entrainement mais 
# ATTENTION transfort et pas fit_transform pour ne pas qu'il change de méthode
inputs = sc.transform(inputs)


#''' Reshaping pour obtenir un tableau a trois dimensions '''
# 60 timestamp
X_test = []

'''for i in range(60, 80):
    # prendre les 60 jours pour l'entrainement
    X_test.append(inputs[(i-60):i, 0])
X_test = np.array(X_test)
#   batch_size : Le nombre de lignes, 
#   timesteps : Le nombres de colonne soit les timestep,
#   input_dim : Le nombre d'action à coréler, ici 1 (google) mais elle pourait dépendre  
#    par exemple de youtube il faudrait alors mettre 2
X_test = np.reshape(X_test,(X_test.shape[0], X_test.shape[1], 1))'''

for variable in range(0, 2):
    X = []
    for i in range(60, 80):
        X.append(inputs[i-60:i, variable])
    X, np.array(X)
    X_test.append(X)

X_test=np.array(X_test)

X_test = np.swapaxes(np.swapaxes(X_test, 0, 1), 1, 2)


''' Prédiction '''
predictive = regressor.predict(X_test)

trainPredict_dataset_like = np.zeros(shape=(len(predictive), 2) )
# rajout en première colonne du vecteur de résultats prédits
trainPredict_dataset_like[:,0] = predictive[:,0]
# inversion et retour vers les valeurs non scalées
predictive = sc.inverse_transform(trainPredict_dataset_like)[:,0]



''' Visualisation '''
plt.plot(real_stock_price[:,0], color='red', label='Prix réel')
plt.plot(real_stock_price[:,1], color='blue', label='Prix réel')
plt.plot(predictive, color='green', label='Prix prédit')
plt.title('Google Stock Price Prediction')
plt.xlabel('Time')
plt.ylabel('Google Stock Price')
plt.legend()
plt.show()





















