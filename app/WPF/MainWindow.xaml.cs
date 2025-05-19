using System.Windows;
using BibliothequeWPF.ViewModels;

namespace BibliothequeWPF;

public partial class MainWindow : Window
{
    public MainViewModel ViewModel { get; set; }

    public MainWindow()
    {
        InitializeComponent();
        ViewModel = new MainViewModel();
        DataContext = ViewModel;
    }

    private void GererEmprunts_Click(object sender, RoutedEventArgs e)
    {
        var fenetre = new EmpruntsWindow();
        fenetre.ShowDialog();
    }
} 