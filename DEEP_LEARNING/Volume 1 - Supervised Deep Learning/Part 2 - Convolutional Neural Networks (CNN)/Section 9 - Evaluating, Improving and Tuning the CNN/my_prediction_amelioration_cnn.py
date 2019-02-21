#REPERTOIRE DU DATASET
#C:\Users\karl\Pictures\Convolutional_Neural_Networks\Convolutional_Neural_Networks

# Importing the Keras libraries and packages
from keras.models import Sequential
from keras.layers import Conv2D
from keras.layers import MaxPooling2D
from keras.layers import Flatten
from keras.layers import Dense
from keras.layers import Dropout

#---------------------------
#  CREATION DU CNN
#---------------------------

#iEtape 0 : nitialisatin du CNN
classifier = Sequential()


#Etape 1 : Ajouter la couche de convolution
#filters=32     : 32 features detector 32 pour une couche intermédiare
#kernel_size=3  : la taille du kernel (3,5,7,...)
#strides=1      : la foulée un pixel 
#input_shape=(128, 128, 3)  : RGB = 3 , GRAY = 1, 64x64 taille de l'image que l'on veut traiter
#activation="relu" : fonction redresseur
''' Utilise des images plus grande '''
conv = Conv2D(filters=32, kernel_size=3, strides=1, input_shape=(200, 200, 3), activation="relu")
classifier.add(conv)


#Etape 2 : Max pooling (convolution avec une matrice de 2x2 et on retient le nombre maximum)
#pool_size=(2,2)  : la taille de la matrice 
maxpool = MaxPooling2D(pool_size=(2,2))
classifier.add(maxpool)
dropout = Dropout(0.25)
classifier.add(dropout)


'''Ajout d une deuxieme couche de convolution '''
conv = Conv2D(filters=32, kernel_size=3, strides=1, activation="relu")
classifier.add(conv)
classifier.add(MaxPooling2D(pool_size=(2,2)))
classifier.add(Dropout(0.25))
'''Ajout d une troisieme couche de convolution '''
conv = Conv2D(filters=64, kernel_size=3, strides=1, activation="relu")
classifier.add(conv)
classifier.add(MaxPooling2D(pool_size=(2,2)))
classifier.add(Dropout(0.25))
'''Ajout d une quatrieme couche de convolution '''
conv = Conv2D(filters=64, kernel_size=3, strides=1, activation="relu")
classifier.add(conv)
classifier.add(MaxPooling2D(pool_size=(2,2)))
classifier.add(Dropout(0.25))



#Etape 3 : Flatening - mise a plat des matrice en vecteur
classifier.add(Flatten())


#Etape 4 : Fully connected - Ajout d'une couche cachée
#units=128  : nombre de neuronnes, ici, c'est dificile de prendre la moyenne entre le nombre 
#          de neuronne d'entrée et de sortie, alors on prend une puisssance de 2
#activation="relu"  : fonction redresseur, on laisse passé ou pas le signal
from keras import backend as K
dense = Dense(units=128, activation=K.relu)
classifier.add(dense)
dropout = Dropout(0.3)
classifier.add(dropout)

''' Ajoute une deuxieme couche '''
classifier.add(Dense(units=128, activation=K.relu))
classifier.add(Dropout(0.5))

''' Ajoute une troisieme couche '''
classifier.add(Dense(units=128, activation=K.relu))
classifier.add(Dropout(0.3))


#Etape 5 : Ajout de la couche de sortie
#units=128  : nombre de neuronnes, un seul neurone de sortie 
#activation="sigmoid"  : fonction sigmoid qui permet de faire de la classification, des probabilités
#           "softmax"  : si plus de deux cathégories
dense = Dense(units=1, activation=K.sigmoid)
classifier.add(dense)


#Etape 6 : Compilation du reseau de neuronne
#optimizer=adam  : algorithme de classification
#loss="binary_crossentropy"  : fonction de cout, ici catégorisation binaire 
#                              plus de deux categorie : categorical_crossentropy
#metrics=["accuracy"] :  Calcul des estimation  par la précission
from keras import losses
classifier.compile(optimizer="adam", loss=losses.binary_crossentropy, metrics=["accuracy"])


#Etape 7 : Entrainement du CNN
#Eviter le sur-entrainement des images en générant plus d'images à partir 
#des images d'entrée en retournant les image, les déformant, ...
from keras.preprocessing.image import ImageDataGenerator


#Augmente le jeu de donnée
#rescale = 1./255  : Change la valeur des pixels, redimensione les pixel entre 0 et 1
#shear_range = 0.2  : transvection changer l'angle d'observation, point de vue (https://fr.wikipedia.org/wiki/Transvection)
#zoom_range = 0.2,  : Zoomer les images
#horizontal_flip = True : retournement
train_datagen = ImageDataGenerator(rescale = 1./255,
                                   shear_range = 0.2,
                                   zoom_range = 0.2,
                                   horizontal_flip = True)


#rescale = 1./255 : Changer l'échelle du jeu de test pour être = au training
test_datagen = ImageDataGenerator(rescale = 1./255)


#Utilise le générator sur les dossiers image
#training_set = 'dataset/training_set'
training_path = 'C:/Users/karl/Pictures/Convolutional_Neural_Networks/Convolutional_Neural_Networks/dataset/training_set'
training_set = train_datagen.flow_from_directory(training_path,
                                                 target_size = (200, 200),
                                                 batch_size = 32,
                                                 class_mode = 'binary')


#Utilise le générator sur les dossiers image
test_path = 'C:/Users/karl/Pictures/Convolutional_Neural_Networks/Convolutional_Neural_Networks/dataset/test_set'
test_set = test_datagen.flow_from_directory(test_path,
                                            target_size = (200, 200),
                                            batch_size = 32,
                                            class_mode = 'binary')

#Compulse les images
#steps_per_epoch = 250   : 8000/32 nombre d'image divisé par la taille du batch_size
#epochs = 25
#validation_data = test_set,
#validation_steps = 63   : 2000/32
'''classifier.fit_generator(training_set,
                         steps_per_epoch = 250,
                         epochs = 2,
                         validation_data = test_set,
                         validation_steps = 63)
'''
classifier.load_weights('my_weight.h5')
classifier.weights

#loss: 0.4208 - acc: 0.8074 - val_loss: 0.4181 - val_acc: 0.8120
import numpy as np
from keras.preprocessing.image import load_img
from keras.preprocessing.image import img_to_array


#DOG
predict_path = "C:/Users/karl/Pictures/Convolutional_Neural_Networks/Convolutional_Neural_Networks/dataset/single_prediction/cat_or_dog_4.jpg"
im = load_img(predict_path, target_size=(200, 200))
#convertir l'image en array
im = img_to_array(im)
#Ajoute une dimension pour que le format de données soit correcte
im = np.expand_dims(im, axis=0)
#effectue la prédiction
res = classifier.predict(im)
print(res)

#Affiche les classes {'cats': 0, 'dogs': 1}
training_set.class_indices

if res[0][0] == 1:
    print("chien")
else:
    print("chat")


classifier.save_weights('my_weight.h5')
classifier.weights

classifier.weights.clear
classifier.weights
classifier.load_weights('my_weight.h5')
classifier.weights
#effectue la prédiction
res = classifier.predict(im)
print(res)













