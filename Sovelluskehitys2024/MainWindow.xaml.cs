﻿using System.Data;
using System.Reflection;
using System.Reflection.Metadata;
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

using Microsoft.Data.SqlClient;

namespace Sovelluskehitys2024
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string polku = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\k2202274\\Documents\\testitietokanta.mdf;Integrated Security=True;Connect Timeout=30";
        public MainWindow()
        {
            InitializeComponent();

            PäivitäDataGrid("SELECT * FROM tuotteet","tuotteet", tuotelista);
            PäivitäDataGrid("SELECT * FROM asiakkaat", "asiakkaat", asiakaslista);
            PäivitäComboBox();
        }

        private void PäivitäDataGrid(string kysely, string taulu, DataGrid grid)
        {
            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();

            SqlCommand komento = yhteys.CreateCommand();
            komento.CommandText = kysely;

            SqlDataAdapter adapteri = new SqlDataAdapter(komento);
            DataTable dt = new DataTable(taulu);
            adapteri.Fill(dt);

            grid.ItemsSource = dt.DefaultView;

            yhteys.Close();
        }

        private void PäivitäComboBox()
        {
            //tuotelista_cb.Items.Clear();

            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();

            SqlCommand komento = new SqlCommand("SELECT * FROM tuotteet", yhteys);
            SqlDataReader lukija = komento.ExecuteReader();

            DataTable taulu = new DataTable();
            taulu.Columns.Add("ID",  typeof(string));
            taulu.Columns.Add("NIMI",  typeof(string));

            /* tehdään disokset että comboboxissa näytetään datataulua */
            tuotelista_cb.ItemsSource = taulu.DefaultView;
            tuotelista_cb.DisplayMemberPath = "NIMI";
            tuotelista_cb.SelectedValuePath = "ID";

            while (lukija.Read()) // käsitellään kyselytulos rivi riviltä
            {
                int id = lukija.GetInt32(0);
                string nimi = lukija.GetString(1);
                taulu.Rows.Add(id, nimi); // lisätään datatauluun rivi tietoineen
                //tuotelista_cb.Items.Add(lukija.GetString(1));
            }
            lukija.Close();

            yhteys.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PäivitäDataGrid("SELECT * FROM tuotteet", "tuotteet", tuotelista);
            PäivitäComboBox();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();

            string kysely = "INSERT INTO tuotteet (nimi, hinta) VALUES ('" + tuotenimi.Text + "'," + tuotehinta.Text + ");";
            SqlCommand komento = new SqlCommand(kysely, yhteys);
            komento.ExecuteNonQuery();

            yhteys.Close();

            PäivitäDataGrid("SELECT * FROM tuotteet", "tuotteet", tuotelista);
            PäivitäComboBox();
            tuotehinta.Clear();
            tuotenimi.Clear();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();

            string id = tuotelista_cb.SelectedValue.ToString();
            string kysely = "DELETE FROM tuotteet WHERE id='" + id + "';";
            SqlCommand komento = new SqlCommand(kysely, yhteys);
            komento.ExecuteNonQuery();
            yhteys.Close();

            PäivitäDataGrid("SELECT * FROM tuotteet", "tuotteet", tuotelista);
            PäivitäComboBox();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();

            string kysely = "INSERT INTO asiakkaat (nimi, osoite, puhelin) VALUES ('" + asiakasnimi.Text + "','" + asiakasosoite.Text + "','" + asiakaspuhelin.Text + "'); ";
            SqlCommand komento = new SqlCommand(kysely, yhteys);
            komento.ExecuteNonQuery();

            yhteys.Close();

            PäivitäDataGrid("SELECT * FROM asiakkaat", "asiakkaat", asiakaslista);
        }
    }
}