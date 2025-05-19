using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using BibliothequeWPF.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;

namespace BibliothequeWPF.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly HttpClient _httpClient = new();
        private const string apiUrl = "http://localhost:5194/api/Livres";

        private Livre _livreSelectionne;
        public ObservableCollection<Livre> Livres { get; set; } = new();
        public Livre LivreSelectionne
        {
            get => _livreSelectionne;
            set { _livreSelectionne = value; OnPropertyChanged(); }
        }

        // Champs de saisie bindés
        private string _titre;
        public string Titre
        {
            get => _titre;
            set { _titre = value; OnPropertyChanged(); }
        }
        private string _auteur;
        public string Auteur
        {
            get => _auteur;
            set { _auteur = value; OnPropertyChanged(); }
        }
        private string _isbn;
        public string Isbn
        {
            get => _isbn;
            set { _isbn = value; OnPropertyChanged(); }
        }
        private string _nbEx;
        public string NbEx
        {
            get => _nbEx;
            set { _nbEx = value; OnPropertyChanged(); }
        }

        // Commandes pour ajouter, rafraîchir, éditer, supprimer
        public ICommand AjouterLivreCommand { get; }
        public ICommand RafraichirCommand { get; }
        public ICommand EditerLivreCommand { get; }
        public ICommand SupprimerLivreCommand { get; }

        public MainViewModel()
        {
            AjouterLivreCommand = new RelayCommand(async () => await AjouterLivreAsync());
            RafraichirCommand = new RelayCommand(async () => await LoadLivresAsync());
            EditerLivreCommand = new RelayCommand<Livre>(async (livre) => await EditerLivreAsync(livre));
            SupprimerLivreCommand = new RelayCommand<Livre>(async (livre) => await SupprimerLivreAsync(livre));
            _ = LoadLivresAsync(); // Chargement initial (fire and forget)
        }

        public async Task LoadLivresAsync()
        {
            try
            {
                var livres = await _httpClient.GetFromJsonAsync<List<Livre>>(apiUrl);
                Livres.Clear();
                if (livres != null)
                {
                    foreach (var l in livres)
                        Livres.Add(l);
                }
            }
            catch
            {
                // Optionnel : gestion d'erreur (MessageBox, etc.)
            }
        }

        public async Task AjouterLivreAsync()
        {
            if (string.IsNullOrWhiteSpace(Titre) || string.IsNullOrWhiteSpace(Auteur))
                return;
            int nb = 1;
            int.TryParse(NbEx, out nb);
            var livre = new Livre
            {
                titre = Titre,
                auteur = Auteur,
                isbn = Isbn,
                exemplairesDisponibles = nb
            };
            try
            {
                var response = await _httpClient.PostAsJsonAsync(apiUrl, livre);
                if (response.IsSuccessStatusCode)
                {
                    await LoadLivresAsync();
                    Titre = Auteur = Isbn = NbEx = string.Empty;
                }
            }
            catch
            {
            }
        }

        public async Task EditerLivreAsync(Livre livre)
        {
            if (livre == null) return;
            // Ouvre la fenêtre d'édition
            var editWindow = new BibliothequeWPF.EditLivreWindow(new Livre
            {
                id = livre.id,
                titre = livre.titre,
                auteur = livre.auteur,
                isbn = livre.isbn,
                exemplairesDisponibles = livre.exemplairesDisponibles
            });
            if (editWindow.ShowDialog() == true)
            {
                // Met à jour le livre via l'API (PUT)
                var edited = editWindow.Livre;
                edited.titre = editWindow.TitreTextBox.Text;
                edited.auteur = editWindow.AuteurTextBox.Text;
                edited.isbn = editWindow.IsbnTextBox.Text;
                edited.exemplairesDisponibles = int.TryParse(editWindow.NbExTextBox.Text, out var nb) ? nb : 1;
                try
                {
                    var response = await _httpClient.PutAsJsonAsync($"{apiUrl}/{edited.id}", edited);
                    if (response.IsSuccessStatusCode)
                        await LoadLivresAsync();
                }
                catch { }
            }
        }

        public async Task SupprimerLivreAsync(Livre livre)
        {
            if (livre == null) return;
            // Ouvre la fenêtre d'édition pour confirmation (ou supprime direct)
            var result = MessageBox.Show($"Supprimer le livre '{livre.titre}' ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var response = await _httpClient.DeleteAsync($"{apiUrl}/{livre.id}");
                    if (response.IsSuccessStatusCode)
                        await LoadLivresAsync();
                }
                catch { }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    // Commande de base pour MVVM (sans paramètre)
    public class RelayCommand : ICommand
    {
        private readonly Func<Task>? _executeAsync;
        private readonly Action? _execute;
        private readonly Func<bool>? _canExecute;
        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
        public RelayCommand(Func<Task> executeAsync, Func<bool>? canExecute = null)
        {
            _executeAsync = executeAsync;
            _canExecute = canExecute;
        }
        public bool CanExecute(object? parameter) => _canExecute == null || _canExecute();
        public async void Execute(object? parameter)
        {
            if (_execute != null) _execute();
            else if (_executeAsync != null) await _executeAsync();
        }
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }

    // Commande de base pour MVVM (générique)
    public class RelayCommand<T> : ICommand
    {
        private readonly Func<T?, Task>? _executeAsync;
        private readonly Action<T?>? _execute;
        private readonly Func<T?, bool>? _canExecute;
        public RelayCommand(Action<T?> execute, Func<T?, bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
        public RelayCommand(Func<T?, Task> executeAsync, Func<T?, bool>? canExecute = null)
        {
            _executeAsync = executeAsync;
            _canExecute = canExecute;
        }
        public bool CanExecute(object? parameter) => _canExecute == null || _canExecute((T?)parameter);
        public async void Execute(object? parameter)
        {
            if (_execute != null) _execute((T?)parameter);
            else if (_executeAsync != null) await _executeAsync((T?)parameter);
        }
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
} 