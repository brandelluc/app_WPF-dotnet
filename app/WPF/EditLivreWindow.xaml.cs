using System.Windows;
using BibliothequeWPF.Models;

namespace BibliothequeWPF
{
    public partial class EditLivreWindow : Window
    {
        public Livre Livre { get; set; }
        public EditLivreWindow(Livre livre)
        {
            InitializeComponent();
            Livre = livre;
            TitreTextBox.Text = livre.titre;
            AuteurTextBox.Text = livre.auteur;
            IsbnTextBox.Text = livre.isbn;
            NbExTextBox.Text = livre.exemplairesDisponibles?.ToString() ?? "";
        }
    }
} 