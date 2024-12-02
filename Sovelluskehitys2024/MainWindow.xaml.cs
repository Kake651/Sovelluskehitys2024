using System.Collections.Generic;
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
using ControlzEx.Standard;
using ControlzEx.Theming;
using MahApps.Metro.Controls;

using Microsoft.Data.SqlClient;

namespace Sovelluskehitys2024
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        string polku = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\k2202274\\Documents\\testitietokanta.mdf;Integrated Security=True;Connect Timeout=30";
        public MainWindow()
        {
            InitializeComponent();

            //ThemeManager.Current.ChangeTheme(this,"Light.Teal");

            try
            {
                PäivitäDataGrid("SELECT * FROM tuotteet", "tuotteet", tuotelista);
                PäivitäDataGrid("SELECT * FROM asiakkaat", "asiakkaat", asiakaslista);
                PäivitäDataGrid("SELECT * FROM Asentajat", "Asentajat", Asentajalista);
                PäivitäDataGrid("SELECT * FROM huoltopalvelut", "huoltopalvelut", huoltopalvelulista);
                PäivitäDataGrid("SELECT ti.id as id, a.nimi as asiakas, tu.nimi as tuote FROM tilaukset ti, asiakkaat a, tuotteet tu where a.id=ti.asiakas_id and tu.id=ti.tuote_id and ti.toimitettu='0'", "tilaukset", Tilauslista);
                PäivitäDataGrid("SELECT ti.id as id, a.nimi as asiakas, tu.nimi as tuote FROM tilaukset ti, asiakkaat a, tuotteet tu where a.id=ti.asiakas_id and tu.id=ti.tuote_id and ti.toimitettu='1'", "tilaukset", Toimitetutlista);

                PäivitäDataGrid("SELECT hti.id as id, an.nimi as asentaja, hp.nimi as huoltopalvelu, a.nimi as asiakas FROM huoltotilaukset hti, Asentajat an, huoltopalvelut hp, asiakkaat a where an.id=hti.asentaja_id and hp.id=hti.huoltopalvelu_id and a.id=hti.asiakas_id and hti.valmis='0'", "huoltotilaukset", Huoltotilauslista);
                /*PäivitäDataGrid("SELECT hti.id as id, an.nimi as asentaja, hp.nimi as huoltopalvelu, a.nimi as asiakas FROM huoltotilaukset hti, Asentajat an, huoltopalvelut hp, asiakkaat a where an.id=hti.asentaja_id and hp.id=hti.huoltopalvelu_id and a.id=hti.asiakas_id and hti.valmis='1'", "", );*/

                PäivitäComboBox(tuotelista_cb, tuotelista_cb_2);
                PäivitäComboBox_2(huoltopalvelulista_cb, huoltopalvelulista_cb_2);
                PäivitäAsiakasComboBox();
                PäivitäAsiakasComboBox2();
                PäivitäAsentajaComboBox();

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

        private void PäivitäAsiakasComboBox2()
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
            asiakaslista_cb_2.ItemsSource = taulu.DefaultView;
            asiakaslista_cb_2.DisplayMemberPath = "NIMI";
            asiakaslista_cb_2.SelectedValuePath = "ID";


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

        private void PäivitäAsentajaComboBox()
        {
            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();

            SqlCommand komento = new SqlCommand("SELECT * FROM Asentajat", yhteys);
            SqlDataReader lukija = komento.ExecuteReader();

            DataTable taulu = new DataTable();
            taulu.Columns.Add("ID", typeof(string));
            taulu.Columns.Add("NIMI", typeof(string));

            /* tehdään disokset että comboboxissa näytetään datataulua */
            asentajalista_cb.ItemsSource = taulu.DefaultView;
            asentajalista_cb.DisplayMemberPath = "NIMI";
            asentajalista_cb.SelectedValuePath = "ID";


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

            asiakasnimi.Clear();
            asiakaspuhelin.Clear();
            asiakasosoite.Clear();
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

        private void huoltotilaus_vamis_click(object sender, RoutedEventArgs e)
        {
            DataRowView rivinakyma = (DataRowView)((Button)e.Source).DataContext;
            String huoltotilaus_id = rivinakyma[0].ToString();

            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();
            string sql = "UPDATE huoltotilaukset set valmis = 1 WHERE id ='" + huoltotilaus_id + "';";

            SqlCommand komento = new SqlCommand(sql, yhteys);
            komento.ExecuteNonQuery();

            yhteys.Close();

            PäivitäDataGrid("SELECT hti.id as id, an.nimi as asentaja, hp.nimi as huoltopalvelu, a.nimi as asiakas FROM huoltotilaukset hti, Asentajat an, huoltopalvelut hp, asiakkaat a where an.id=hti.asentaja_id and hp.id=hti.huoltopalvelu_id and a.id=hti.asiakas_id and hti.valmis='0'", "huoltotilaukset", Huoltotilauslista);

            /*PäivitäDataGrid("SELECT hti.id as id, an.nimi as asentaja, hp.nimi as huoltopalvelu, a.nimi as asiakas FROM huoltotilaukset hti, Asentajat an, huoltopalvelut hp, asiakkaat a where an.id=hti.asentaja_id and hp.id=hti.huoltopalvelu_id and a.id=hti.asiakas_id and hti.valmis='1'", "", );*/
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();

            string kysely = "INSERT INTO Asentajat (nimi, puhelin) VALUES ('" + asentajanimi.Text + "','" + asentajapuhelin.Text + "'); ";
            SqlCommand komento = new SqlCommand(kysely, yhteys);
            komento.ExecuteNonQuery();

            yhteys.Close();

            asentajanimi.Clear();
            asentajapuhelin.Clear();
            PäivitäDataGrid("SELECT * FROM Asentajat", "Asentajat", Asentajalista);
            PäivitäAsentajaComboBox();

        }

        private void PäivitäComboBox_2(ComboBox kombo1, ComboBox kombo2)
        {
            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();

            SqlCommand komento = new SqlCommand("SELECT * FROM huoltopalvelut", yhteys);
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

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();

            string kysely = "INSERT INTO huoltopalvelut (nimi, hinta) VALUES ('" + Huoltopalvelu.Text + "'," + Huoltopalveluhinta.Text + ");";
            SqlCommand komento = new SqlCommand(kysely, yhteys);
            komento.ExecuteNonQuery();

            yhteys.Close();

            PäivitäDataGrid("SELECT * FROM huoltopalvelut", "huoltopalvelut", huoltopalvelulista);
            PäivitäComboBox_2(huoltopalvelulista_cb, huoltopalvelulista_cb_2);
            Huoltopalveluhinta.Clear();
            Huoltopalvelu.Clear();

        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();

            string id = huoltopalvelulista_cb.SelectedValue.ToString();
            string kysely = "DELETE FROM huoltopalvelut WHERE id='" + id + "';";
            SqlCommand komento = new SqlCommand(kysely, yhteys);
            komento.ExecuteNonQuery();
            yhteys.Close();

            PäivitäDataGrid("SELECT * FROM huoltopalvelut", "huoltopalvelut", huoltopalvelulista);
            PäivitäComboBox_2(huoltopalvelulista_cb, huoltopalvelulista_cb_2);
        }
        
        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            SqlConnection yhteys = new SqlConnection(polku);
            yhteys.Open();

            string asentajaID = asentajalista_cb.SelectedValue.ToString();
            string huoltopalveluID = huoltopalvelulista_cb_2.SelectedValue.ToString();
            string asiakasID = asiakaslista_cb_2.SelectedValue.ToString();

            string sql = "INSERT INTO huoltotilaukset (asentaja_id, huoltopalvelu_id, asiakas_id) VALUES ('" + asentajaID + "','" + huoltopalveluID + "','" + asiakasID +"');";

            SqlCommand komento = new SqlCommand(sql, yhteys);
            komento.ExecuteNonQuery();

            yhteys.Close();

            PäivitäDataGrid("SELECT hti.id as id, an.nimi as asentaja, hp.nimi as huoltopalvelu, a.nimi as asiakas FROM huoltotilaukset hti, Asentajat an, huoltopalvelut hp, asiakkaat a where an.id=hti.asentaja_id and hp.id=hti.huoltopalvelu_id and a.id=hti.asiakas_id and hti.valmis='0'", "huoltotilaukset", Huoltotilauslista);
        }
        
    }
}