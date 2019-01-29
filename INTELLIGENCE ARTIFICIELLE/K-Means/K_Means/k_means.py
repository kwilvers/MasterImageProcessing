# K-Means
#identifier les clusters d'observation

# Importer les librairies
import numpy as np
import matplotlib.pyplot as plt
import pandas as pd

# Importer le dataset
dataset = pd.read_csv('Mall_Customers.csv')
#que des variables indépendantes dans le dataset, 
#On conserve deux colonnes pour rechercher les clusters
#intuitivement on cponserve l'annual income et le pending score
X = dataset.iloc[:, [3, 4]].values
#Pas de variable indépendante dasn le dataset
#Y = dataset.iloc[:, -1].values

#pas de données manquantes
#pas de variables cathégoriques
#pas besoin de faire un training set et test set
#pas de feature scalling pour le kmeans


# Utiliser la méthode elbow pour trouver le nombre optimal de clusters
from sklearn.cluster import KMeans

#within-cluster sum of squares (somme des carrés des distance au sein d'un cluster)
wcss = []
#Calcul de 1 à 10 clusters 
for i in range(1, 11):  #la borne suppérieur du range est exclue
    #construre l'objet kmeans avec le nombre de cluster désiré et l'algorithme ++ pour éviter le piège kmeans 
    kmeans = KMeans(n_clusters = i, init = 'k-means++', random_state = 0)
    #fit avec le dataset
    kmeans.fit(X)
    #récupère la somme des distance
    wcss.append(kmeans.inertia_)
    

#trace le nombre de cluister a l'aide de range et les valeurs de wcss    
plt.plot(range(1, 11), wcss)
plt.title('La méthode Elbow')
plt.xlabel('Nombre de clusters')
plt.ylabel('WCSS')
plt.show()

# Construction du modèle
from sklearn.cluster import KMeans
#D'après la courbe précédente 5 est le nombre de cluster optimal
kmeans = KMeans(n_clusters = 5, init = 'k-means++', random_state = 0)
#Récupère les clusters
y_kmeans = kmeans.fit_predict(X)

# Visualiser les résultats
#prendre tout les point qui appartiennent au cluster 1
#X[y_kmeans == 1, 0] tout les point qui appartiennt au cluster 1 dont l'absisse est la collonne 0 de X (salaire annuel)
#X[y_kmeans == 1, 1] tout les point qui appartiennt au cluster 1 dont l'ordonnée est la collonne 1 de X (spending score)
plt.scatter(X[y_kmeans == 1, 0], X[y_kmeans == 1, 1], c = 'red', label = 'Cluster 1')
plt.scatter(X[y_kmeans == 2, 0], X[y_kmeans == 2, 1], c = 'blue', label = 'Cluster 2')
plt.scatter(X[y_kmeans == 3, 0], X[y_kmeans == 3, 1], c = 'green', label = 'Cluster 3')
plt.scatter(X[y_kmeans == 4, 0], X[y_kmeans == 4, 1], c = 'cyan', label = 'Cluster 4')
plt.scatter(X[y_kmeans == 0, 0], X[y_kmeans == 0, 1], c = 'magenta', label = 'Cluster 5')

plt.title('Clusters de clients')
plt.xlabel('Salaire annuel')
plt.ylabel('Spending Score')
plt.legend()


#♦en ajoutant la variable cluster au dataset, on créée une variable dépendante permetant de 
#créer un problème de classification, avec la variable dépendante, on peut obtenir des corélations entre les 
#variables indépendantes et la variable dépendante et prédir pour chaque nouveau client le cluster auquel il appartient







