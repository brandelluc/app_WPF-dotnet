# Dossier de Présentation - Bibliothèque

Ce dossier contient une version épurée et pédagogique du projet de gestion de bibliothèque : API ASP.NET + Application WPF (MVVM).

## Structure du dossier

- `API/` : Code source de l’API ASP.NET (contrôleurs, modèles, contexte, README)
- `WPF/` : Code source de l’application WPF (XAML, ViewModels, Models, README)
- `README_API.md` : Documentation de l’API
- `README_WPF.md` : Documentation de l’application WPF

## Lancement rapide

1. **API**
   - Ouvrir le dossier `API/` dans Visual Studio
   - Vérifier la chaîne de connexion dans `appsettings.json` (SQL Server)
   - Lancer : `dotnet build` puis `dotnet run`
2. **WPF**
   - Ouvrir le dossier `WPF/` dans Visual Studio
   - Lancer l’application (`F5` ou `dotnet run`)

## Sécurité & confidentialité
- Les fichiers de configuration sensibles (`appsettings.Development.json`, secrets, etc.) ont été retirés.
- Aucune donnée personnelle ou confidentielle n’est présente.

## Pour toute question, se référer aux README spécifiques de chaque partie. 