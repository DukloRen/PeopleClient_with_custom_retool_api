using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PeopleClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PeopleService service = new PeopleService();
        public MainWindow()
        {
            InitializeComponent();
            peopleTable.ItemsSource = service.GetAll();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            PersonForm personForm = new PersonForm();
            //personForm.Closed += (obj, args) =>
            // _ karakter változónévnek lambdában -> discard -> nem használjuk fel a változót ezért nem nevezzük el.
            personForm.Closed += (_, _) =>
            {
                peopleTable.ItemsSource = service.GetAll();
            };
            personForm.ShowDialog();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Person selected = peopleTable.SelectedItem as Person;
            if (selected == null)
            {
                MessageBox.Show("Törléshez előbb válasszon ki elemet!");
                return;
            }

            MessageBoxResult clickedButton = 
                MessageBox.Show($"Biztos, hogy törölni szeretné az alábbi embert: {selected.FullName}", "Törlés", MessageBoxButton.YesNo);
            if (clickedButton == MessageBoxResult.Yes)
            {
                if (service.Delete(selected))
                {
                    MessageBox.Show("Sikeres törlés!");
                }
                else
                {
                    MessageBox.Show("Hiba történt a törlés során!");
                }
                peopleTable.ItemsSource = service.GetAll();
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            Person selected = peopleTable.SelectedItem as Person;
            if (selected == null)
            {
                MessageBox.Show("Módosításhoz előbb válasszon ki elemet!");
                return;
            }

            PersonForm personForm = new PersonForm(selected);
            personForm.Closed += (_, _) =>
            {
                peopleTable.ItemsSource = service.GetAll();
            };
            personForm.ShowDialog();
        }
    }
}
