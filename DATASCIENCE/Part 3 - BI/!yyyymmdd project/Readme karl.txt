Pour valider les données : 

- Les données sont elle dans le bon type : ISNUMERIC, ISDATE
- Trouver les minimums et maximums
- Trouver les moyennes
- Mettre en corrélation les min, max et moyenne, 
  -  Le max n'est il pas trop élévé? 
  -  Le min peut il être négatif, n'est il pas trop éloigné?
- Quel est le range valide des données?
- Quel est le range autorisé pour une date?  Y a t'il des date en dehors du range?
- Y a t'il une ou plusieurs colonnes suplémentaire créée dans le CSV après l'import dans excel?
  -  Si oui, peut-on identifier les dignes décalées vers la gauche ou vers la droite?
- Y a t'il des données dupliquées?

Pour chaque colonne du dataset, 
- Définir le range des données
- Définir les données inconsistentes
- Définir les données valides (ex.: Female, Male) --> Vérifier qu'il n'y a pas d'autre valeurs
- Définir la taille des données


En somme, il faut définir les pré et post condition