# Application WPF - Gestion de Bibliothèque

Cette application WPF (C#, MVVM) permet de gérer une bibliothèque connectée à l'API ASP.NET (voir README_API.md).

## Fonctionnalités principales

### Gestion des livres
- Affichage de la liste des livres (titre, auteur, ISBN, nombre d'exemplaires)
- Ajout, modification, suppression de livres (CRUD complet)
- Rafraîchissement de la liste
- Animations modernes sur les boutons
- Interface sombre, moderne, avec data binding

### Gestion des emprunts
- Fenêtre dédiée accessible depuis la fenêtre principale
- Ajout d'un emprunt (nom, prénom, livre emprunté via ComboBox)
- Suppression d'un emprunt
- Affichage des emprunts sous forme de liste stylée
- Pas de modification d'emprunt (conformément au cahier des charges)

## Architecture
- **MVVM** (Model-View-ViewModel) strict
- Dossiers : `Models/`, `ViewModels/`, `Views/`
- Liaison des vues via DataContext et data binding
- Utilisation d'ObservableCollection pour la synchronisation UI
- Commandes (ICommand) pour toutes les actions utilisateur

## Lancement
- Ouvrir la solution dans Visual Studio
- S'assurer que l'API est lancée (`dotnet run` dans le dossier API)
- Lancer le projet WPF (`F5` ou `dotnet run` dans le dossier WPF)

## Remarques
- Les requêtes HTTP sont faites via `HttpClient` dans les ViewModels
- Les styles sont définis dans les fichiers XAML (boutons, DataGrid, ListBox, ComboBox, etc.)
- L'UI est responsive et moderne (fond noir, texte blanc, bordures rouges)
- Toute la logique métier est dans les ViewModels, les Views ne contiennent que du XAML

## Dépendances
- .NET 6 ou supérieur
- Aucune librairie tierce obligatoire (tout est natif WPF)

## Pour aller plus loin
- Possibilité d'ajouter des animations supplémentaires ou d'intégrer une librairie de styles (MahApps, MaterialDesign) si besoin
- L'API et le WPF sont découplés, donc facilement extensibles 