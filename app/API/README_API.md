# API REST - Bibliothèque

Cette API ASP.NET Core permet de gérer une bibliothèque avec deux entités principales : **Livres** et **Emprunts**.

## Endpoints principaux

### Livres
- `GET /api/Livres` : Liste tous les livres
- `GET /api/Livres/{id}` : Détail d'un livre
- `POST /api/Livres` : Ajoute un livre
- `PUT /api/Livres/{id}` : Modifie un livre
- `DELETE /api/Livres/{id}` : Supprime un livre

#### Exemple de JSON pour un livre
```json
{
  "id": 1,
  "titre": "Le Petit Prince",
  "auteur": "Antoine de Saint-Exupéry",
  "isbn": "978-2070612758",
  "exemplairesDisponibles": 3
}
```

### Emprunts
- `GET /api/Emprunts` : Liste tous les emprunts
- `GET /api/Emprunts/{id}` : Détail d'un emprunt
- `POST /api/Emprunts` : Ajoute un emprunt
- `DELETE /api/Emprunts/{id}` : Supprime un emprunt

#### Exemple de JSON pour un emprunt
```json
{
  "id": 1,
  "nom": "Dupont",
  "prenom": "Luc",
  "livreEmprunte": "Le Petit Prince"
}
```

## Configuration
- Base de données : SQL Server (connexion dans `appsettings.json`)
- Migrations Entity Framework pour la création des tables
- Lancement :
  - `dotnet build`
  - `dotnet run`

## Remarques
- L'API est utilisée par le front-end WPF (voir README_WPF.md)
- Respect du CRUD complet pour les livres, et gestion simple (ajout/suppression) pour les emprunts
- Les contrôleurs sont dans le dossier `Controllers/`
- Les modèles sont dans le dossier `Models/` 