using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using BibliothequeWPF.Models;
using System.Windows.Input;

namespace BibliothequeWPF.ViewModels
{
    public class EmpruntsViewModel : INotifyPropertyChanged
    {
        private readonly HttpClient _httpClient = new();
        private const string apiUrl = "http://localhost:5194/api/Emprunts";
        private const string livresUrl = "http://localhost:5194/api/Livres";

        public ObservableCollection<Emprunt> Emprunts { get; set; } = new();
        public ObservableCollection<string> Livres { get; set; } = new();

        private string _nom = string.Empty;
        public string Nom { get => _nom; set { _nom = value; OnPropertyChanged(); } }
        private string _prenom = string.Empty;
        public string Prenom { get => _prenom; set { _prenom = value; OnPropertyChanged(); } }
        private string _livreSelectionne = string.Empty;
        public string LivreSelectionne { get => _livreSelectionne; set { _livreSelectionne = value; OnPropertyChanged(); } }

        public ICommand ValiderEmpruntCommand { get; }
        public ICommand SupprimerEmpruntCommand { get; }

        public EmpruntsViewModel()
        {
            ValiderEmpruntCommand = new RelayCommand(async () => await ValiderAjoutEmprunt());
            SupprimerEmpruntCommand = new RelayCommand<Emprunt>(async (emprunt) => await SupprimerEmprunt(emprunt));
            _ = ChargerEmprunts();
            _ = ChargerLivres();
        }

        public async Task ChargerEmprunts()
        {
            try
            {
                var emprunts = await _httpClient.GetFromJsonAsync<List<Emprunt>>(apiUrl);
                Emprunts.Clear();
                if (emprunts != null)
                    foreach (var e in emprunts)
                        Emprunts.Add(e);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Erreur lors du chargement des emprunts : {ex.Message}", "Erreur", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        public async Task ChargerLivres()
        {
            try
            {
                var livres = await _httpClient.GetFromJsonAsync<List<Livre>>(livresUrl);
                Livres.Clear();
                if (livres != null)
                    foreach (var l in livres)
                        Livres.Add(l.titre);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Erreur lors du chargement des livres : {ex.Message}", "Erreur", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        public void AfficherFormulaireAjout()
        {
            Nom = string.Empty;
            Prenom = string.Empty;
            LivreSelectionne = string.Empty;
        }

        public async Task ValiderAjoutEmprunt()
        {
            if (string.IsNullOrWhiteSpace(Nom) || string.IsNullOrWhiteSpace(Prenom) || string.IsNullOrWhiteSpace(LivreSelectionne))
            {
                System.Windows.MessageBox.Show("Veuillez remplir tous les champs", "Erreur", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            try
            {
                var emprunt = new Emprunt { Nom = Nom, Prenom = Prenom, LivreEmprunte = LivreSelectionne };
                var response = await _httpClient.PostAsJsonAsync(apiUrl, emprunt);
                if (response.IsSuccessStatusCode)
                {
                    await ChargerEmprunts();
                    AfficherFormulaireAjout();
                }
                else
                {
                    System.Windows.MessageBox.Show("Erreur lors de l'ajout de l'emprunt", "Erreur", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Erreur lors de l'ajout de l'emprunt : {ex.Message}", "Erreur", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        public async Task SupprimerEmprunt(Emprunt emprunt)
        {
            if (emprunt == null) return;

            var result = System.Windows.MessageBox.Show(
                $"Voulez-vous vraiment supprimer l'emprunt de {emprunt.Nom} {emprunt.Prenom} ?",
                "Confirmation",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Question);

            if (result == System.Windows.MessageBoxResult.Yes)
            {
                try
                {
                    var response = await _httpClient.DeleteAsync($"{apiUrl}/{emprunt.Id}");
                    if (response.IsSuccessStatusCode)
                    {
                        await ChargerEmprunts();
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Erreur lors de la suppression de l'emprunt", "Erreur", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Erreur lors de la suppression de l'emprunt : {ex.Message}", "Erreur", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
} 