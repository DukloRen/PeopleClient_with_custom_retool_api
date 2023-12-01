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
using System.Windows.Shapes;

namespace PeopleClient
{
    /// <summary>
    /// Interaction logic for PersonForm.xaml
    /// </summary>
    public partial class PersonForm : Window
    {
        PeopleService service = new PeopleService();
        Person person;
        public PersonForm()
        {
            InitializeComponent();
        }
        public PersonForm(Person person)
        {
            InitializeComponent();
            this.person = person;
            LoadPerson();
            addButton.Visibility = Visibility.Collapsed;
            updateButton.Visibility = Visibility.Visible;
        }

        private void LoadPerson()
        {
            nameInput.Text = this.person.FullName;
            fatherInput.IsChecked = this.person.FatherLeft;
            childrenInput.Text = this.person.NumberOfChildren.ToString();
            emailInput.Text = this.person.Email;
            phoneInput.Text = this.person.PhoneNumber;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Person person = CreatePersonFromInputFields();
                Person newPerson = service.Add(person);
                if (newPerson.Id != 0)
                {
                    MessageBox.Show("Sikeres felvétel");
                    nameInput.Text = "";
                    fatherInput.IsChecked = false;
                    childrenInput.Text = "";
                    emailInput.Text = "";
                    phoneInput.Text = "";
                }
                else
                {
                    MessageBox.Show("Hiba történt a felvétel során!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Person person = CreatePersonFromInputFields();
                Person updated = service.Update(this.person.Id, person);
                if (updated.Id != 0)
                {
                    MessageBox.Show("Sikeres módosítás!");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Hiba történt a módosítás során!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private Person CreatePersonFromInputFields()
        {
            string name = nameInput.Text.Trim();
            bool father = (bool)fatherInput.IsChecked;
            string childrenText = childrenInput.Text.Trim();
            string email = emailInput.Text.Trim();
            string phone = phoneInput.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("Név mező kitöltése kötelező!");
            }

            if (string.IsNullOrEmpty(email))
            {
                throw new Exception("Email mező kitöltése kötelező!");
            }

            if (string.IsNullOrEmpty(phone))
            {
                throw new Exception("Telefonszám mező kitöltése kötelező!");
            }

            if (string.IsNullOrEmpty(childrenText))
            {
                throw new Exception("Gyerekek száma kitöltése kötelező!");
            }

            if (!int.TryParse(childrenText, out int children))
            {
                throw new Exception("A gyerekek száma csak szám lehet!");
            }

            if (children < 0 || children > 15)
            {
                throw new Exception("A gyerekek száma 0 és 15 közötti szám lehet!");
            }

            Person person = new Person();
            person.FullName = name;
            person.FatherLeft = father;
            person.NumberOfChildren = children;
            person.Email = email;
            person.PhoneNumber = phone;
            return person;
        }
    }
}
