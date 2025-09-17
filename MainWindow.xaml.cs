using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.Json;

namespace Assignment11._3_ClientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HttpClient petsClient = new HttpClient();
        public MainWindow()
        {
            InitializeComponent();
            petsClient.BaseAddress = new Uri("http://localhost:5027/api/Pets");
            PetInfoGrid.IsEnabled = false;
            btnSaveAddPet.IsEnabled = false;
            btnSaveUpdate.IsEnabled = false;
        }

        private async void GetPetList (object sender , RoutedEventArgs e)
        {
            var pets = await petsClient.GetAsync(petsClient.BaseAddress);
            if (pets.IsSuccessStatusCode)
            {
                var petList = await pets.Content.ReadFromJsonAsync<List<Pet>>();
                PetListDG.ItemsSource = petList;
            }
            else
            {
                MessageBox.Show("Error: " + pets.StatusCode);
            }
        }
        private async void SaveUpdate(object sender, RoutedEventArgs e)
        {
            var pet = PetInfoGrid.DataContext as Pet;
            int id = pet.PetId;
            using StringContent content = new(JsonSerializer.Serialize(pet),
                System.Text.Encoding.UTF8, "application/json");
            using var response = await petsClient.PutAsync($"{petsClient.BaseAddress}/{id}", content);
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Update Successful");
                GetPetList(sender, e);
            }
            else
            {
                MessageBox.Show($"Error: " + response.StatusCode);
            }
            btnStartAddPet.IsEnabled = true;
            btnSaveUpdate.IsEnabled = false;
            PetInfoGrid.IsEnabled = false;
        }

        private async void StartAddPet(object sender, RoutedEventArgs e)
        {
            PetInfoGrid.IsEnabled = true;
            btnSaveAddPet.IsEnabled = true;
            btnStartAddPet.IsEnabled = false;
            Pet newPet = new Pet();
            PetInfoGrid.DataContext = newPet;

        }
        private async void SaveAddPet(object sender, RoutedEventArgs e)
        {
            var newPet = PetInfoGrid.DataContext; //getting new pet info into an object
            using StringContent content = new(JsonSerializer.Serialize(newPet), 
                System.Text.Encoding.UTF8, "application/json");

            using var response = petsClient.PostAsync(petsClient.BaseAddress, content).Result;
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Pet added successfully");
                
            }
            else
            {
                MessageBox.Show("Error: " + response.StatusCode);
            }
            PetInfoGrid.IsEnabled = false;
            btnSaveAddPet.IsEnabled = false;
            btnStartAddPet.IsEnabled = true;
            GetPetList(sender, e);
        }
        private async void DeletePet(object sender, RoutedEventArgs e)
        {
            var pet = (sender as FrameworkElement).DataContext as Pet;
            var id = pet.PetId;

            using var response = petsClient.DeleteAsync($"{petsClient.BaseAddress}/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Delete Successful");
                GetPetList(sender, e);
            }
            else
            {
                MessageBox.Show($"Error: " + response.StatusCode);
            }

        }
        private async void SendToEdit(object sender, RoutedEventArgs e)
        {
            btnStartAddPet.IsEnabled = false;
            btnSaveUpdate.IsEnabled = true;
            PetInfoGrid.IsEnabled = true;
            PetInfoGrid.DataContext = (sender as FrameworkElement).DataContext as Pet;
            
        }
    }
}