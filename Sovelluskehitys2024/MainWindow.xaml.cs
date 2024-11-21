using System.Data;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
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

            try
            {
                PäivitäDataGrid("SELECT * FROM tuotteet", "tuotteet", tuotelista);
                PäivitäDataGrid("SELECT * FROM asiakkaat", "asiakkaat", asiakaslista);
                PäivitäDataGrid("SELECT ti.id as id, a.nimi as asiakas, tu.nimi as tuote FROM tilaukset ti, asiakkaat a, tuotteet tu where a.id=ti.asiakas_id and tu.id=ti.tuote_id and ti.toimitettu='0'", "tilaukset", Tilauslista);
                PäivitäDataGrid("SELECT ti.id as id, a.nimi as asiakas, tu.nimi as tuote FROM tilaukset ti, asiakkaat a, tuotteet tu where a.id=ti.asiakas_id and tu.id=ti.tuote_id and ti.toimitettu='1'", "tilaukset", Toimitetutlista);
                PäivitäComboBox(tuotelista_cb, tuotelista_cb_2);
                PäivitäAsiakasComboBox();

            }
            catch
            {
                tilaviesti.Text = "Ei tietokantayhteyttä";
            }

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

        private void PäivitäComboBox(ComboBox kombo1, ComboBox kombo2)
        {
            //tuotelista_cb.Items.Clear();

            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();

            SqlCommand komento = new SqlCommand("SELECT * FROM tuotteet", yhteys);
            SqlDataReader lukija = komento.ExecuteReader();

            DataTable taulu = new DataTable();
            taulu.Columns.Add("ID", typeof(string));
            taulu.Columns.Add("NIMI", typeof(string));

            /* tehdään disokset että comboboxissa näytetään datataulua */
            kombo1.ItemsSource = taulu.DefaultView;
            kombo1.DisplayMemberPath = "NIMI";
            kombo1.SelectedValuePath = "ID";

            kombo2.ItemsSource = taulu.DefaultView;
            kombo2.DisplayMemberPath = "NIMI";
            kombo2.SelectedValuePath = "ID";

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

        private void PäivitäAsiakasComboBox()
        {
            //asiakaslista_cb.Items.Clear();

            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();

            SqlCommand komento = new SqlCommand("SELECT * FROM asiakkaat", yhteys);
            SqlDataReader lukija = komento.ExecuteReader();

            DataTable taulu = new DataTable();
            taulu.Columns.Add("ID", typeof(string));
            taulu.Columns.Add("NIMI", typeof(string));

            /* tehdään disokset että comboboxissa näytetään datataulua */
            asiakaslista_cb.ItemsSource = taulu.DefaultView;
            asiakaslista_cb.DisplayMemberPath = "NIMI";
            asiakaslista_cb.SelectedValuePath = "ID";


            while (lukija.Read()) // käsitellään kyselytulos rivi riviltä
            {
                int id = lukija.GetInt32(0);
                string nimi = lukija.GetString(1);
                taulu.Rows.Add(id, nimi); // lisätään datatauluun rivi tietoineen
                //asiakaslista_cb.Items.Add(lukija.GetString(1));
            }
            lukija.Close();

            yhteys.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PäivitäDataGrid("SELECT * FROM tuotteet", "tuotteet", tuotelista);
            PäivitäComboBox(tuotelista_cb, tuotelista_cb_2);
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
            PäivitäComboBox(tuotelista_cb, tuotelista_cb_2);
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
            PäivitäComboBox(tuotelista_cb, tuotelista_cb_2);
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
            PäivitäAsiakasComboBox();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();

            string asiakasID = asiakaslista_cb.SelectedValue.ToString();
            string tuoteID = tuotelista_cb_2.SelectedValue.ToString();

            string sql = "INSERT INTO tilaukset (asiakas_id, tuote_id) VALUES ('" + asiakasID + "','" + tuoteID + "');";

            SqlCommand komento = new SqlCommand(sql, yhteys);
            komento.ExecuteNonQuery();

            yhteys.Close();

            PäivitäDataGrid("SELECT ti.id as id, a.nimi as asiakas, tu.nimi as tuote FROM tilaukset ti, asiakkaat a, tuotteet tu where a.id=ti.asiakas_id and tu.id=ti.tuote_id and ti.toimitettu='0'", "tilaukset", Tilauslista);
        }
        private void toimita_tilaus_Click(object sender, RoutedEventArgs e)
        {
            DataRowView rivinakyma = (DataRowView)((Button)e.Source).DataContext;
            String tilaus_id = rivinakyma[0].ToString();

            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();
            string sql = "UPDATE tilaukset set toimitettu = 1 WHERE id ='" + tilaus_id + "';";

            SqlCommand komento = new SqlCommand(sql, yhteys);
            komento.ExecuteNonQuery();

            yhteys.Close();

            PäivitäDataGrid("SELECT ti.id as id, a.nimi as asiakas, tu.nimi as tuote FROM tilaukset ti, asiakkaat a, tuotteet tu where a.id=ti.asiakas_id and tu.id=ti.tuote_id and ti.toimitettu='0'", "tilaukset", Tilauslista);
            PäivitäDataGrid("SELECT ti.id as id, a.nimi as asiakas, tu.nimi as tuote FROM tilaukset ti, asiakkaat a, tuotteet tu where a.id=ti.asiakas_id and tu.id=ti.tuote_id and ti.toimitettu='1'", "tilaukset", Toimitetutlista);
        }
    }
}