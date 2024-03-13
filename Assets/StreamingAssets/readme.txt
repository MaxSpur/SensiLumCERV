Le fichier de configuration des ambiances et des espèces se trouve dans le fichier BasSiamEspeces_Data\StreamingAssets\AnimalsData.csv

Ce fichier peut être ouvert dans un tableau comme LibreOffice. Lors de l'import dans LibreOffice, le seul séparateur coché dans le formulaire d'import doit être Point-virgule.
Sous Excel, il l'y a pas de paramètre d'import mais le fichier semble s'ouvrir correctement.

Les valeurs contenant une décimale doivent utiliser le point comme séparateur de décimale et non la virgule.
 Sous Excel, l'utilisation du point du pavé numérique est remplacé par une virgule. Il convient donc d'utiliser plutôt Maj+point-virgule

Les lignes précédées d'un # ou d'un point-virgule et les lignes vides sont ignorées
La première ligne non vide ou commentée contient la signification de chaque colonne.
Les noms de colonnes acceptés sont:
 - Bloom Intensity: intensité de "bloom" par défaut pour cette espèce
 - Bloom Scatter: dispertion du "bloom" par défaut pour cette espèce
 - Bloom Tint: couleur du "bloom" par défaut pour cette espèce
 - Exposure: exposition par défaut pour cette espèce (plus sombre avec des valeurs négatives, plus claire avec des valeurs positives)
 - Un de nom de colonne précédente suivie de l'index de l'ambiance entre 0 et 4 (par exemple Bloom Intensity 1 ou Exposure 4 en colonne supplémentaire)
    Cet index d'ambiance représente l'ambiance à laquelle s'appliquera le paramètre (0 pour Ciel étoilé, 1 pour Pleine lune, 2 pour Lumières rouges, 3 pour Lumières normales, 4 pour Lumières bleues)
    Il est donc possible d'avoir des paramètres de post process différents pour chaque espèce et pour chaque ambiance
 - Les dernières colonnes doivent être les longueurs d'ondes
Toutes ces colonnes sont optionnelles sauf la première (nom des espèces) et les longueurs d'ondes


Les lignes suivantes représentent les données de chaque espèce. La première colonne indique le nom de l'espèce (qui sera affichée dans l'application),
 les colonnes suivantes correspondent aux valeurs de chaque paramètre (en fonction du nom de la colonne spécifiée sur la première ligne)

Le fichier peut être rechargé dans l'application sans avoir à la relancer en appuyant sur "R".
A titre d'example, le fichier AnimalsData_exemple.csv contient l'ensemble des colonnes comprises par l'application.

