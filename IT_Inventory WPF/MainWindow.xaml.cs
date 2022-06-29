using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
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
using IT_inventory_EF;


namespace IT_Inventory_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Itt létrehozok egy cnIT_Inventory tipusú változót, ami ugyanerre a névre hallgat.
        /// </summary>
        private cnIT_Inventory cnIT_Inventory;
        
        /// <summary>
        /// Ez a rész inicializálja a komponenseket, majd mindegyik mezőköz hozzáad egy KeyEventHandler eseményekezelőt,
        /// aminek az a célja, hogy egy billentyű lenyomásra reagáljon a mező a t_KeyDown metódusnak megfelelően. Majd példányosítom a cnIT_Inventory-t.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();


            this.tbNev.KeyDown += new KeyEventHandler(t_KeyDown);
            this.cbHely.KeyDown += new KeyEventHandler(t_KeyDown);
            this.cbFelhasznalo.KeyDown += new KeyEventHandler(t_KeyDown);
            this.cbCsoport.KeyDown += new KeyEventHandler(t_KeyDown);
            this.cbStatus.KeyDown += new KeyEventHandler(t_KeyDown);
            this.cbTipusok.KeyDown += new KeyEventHandler(t_KeyDown);
            //this.tbTipusok.KeyDown += new KeyEventHandler(t_KeyDown);
            this.cbModell.KeyDown += new KeyEventHandler(t_KeyDown); 
            this.cbGyarto.KeyDown += new KeyEventHandler(t_KeyDown);
            this.tbSorozatszam.KeyDown += new KeyEventHandler(t_KeyDown);
            this.tbLeltariszam.KeyDown += new KeyEventHandler(t_KeyDown);
            cnIT_Inventory = new cnIT_Inventory();
        }

        /// <summary>
        /// Itt az if-ben ha Enter billenytű lenyomása történik akkor a focus a következő mezőre ugrik.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        void t_KeyDown(Object sender, KeyEventArgs e)
        {                                        
            if (e.Key == Key.Enter)
            {
               
                FocusNavigationDirection focusDirection = FocusNavigationDirection.Next;
                TraversalRequest request = new TraversalRequest(focusDirection);
                UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

                if (elementWithFocus != null)
                    elementWithFocus.MoveFocus(request);
            }
        }

        /// <summary>
        /// Uj tétel menüpontra nyomva aktiválódik a metódus. Helyek, csoport, gyártó, modell mezőket, feltöltöm adattal az adatbázisból.
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void miUjTetel_Click(object sender, RoutedEventArgs e)
        {
            dgAdatracs.Visibility = Visibility.Collapsed;
            grItemek.Visibility = Visibility.Visible;
            // grItemek.DataContext = cnIT_Inventory.TE_Leltar.ToList();
            // cbFelhasznalo.SelectedItem = 0;
            // var helyek = new List<KeyValuePair<string, string>>();
            var helyek = cnIT_Inventory.TE_Helysegek.ToDictionary(t => t.Helyseg_Azon, t => t.Helyseg_Nev);
            cbHely.DataContext = helyek; 
           

            var csoport = (from y in cnIT_Inventory.AD_Users select new { y.physicalDeliveryOfficeName }).Distinct().ToList();
            cbCsoport.DataContext = csoport; //cnIT_Inventory.AD_Users.ToList();
                                             // var felhasznalofilter = (from x in cnIT_Inventory.AD_Users select new {x.cn, x. physicalDeliveryOfficeName})

            var gyarto = (from z in cnIT_Inventory.TE_Leltar select new { z.Gyarto }).Distinct().ToList();
            cbGyarto.DataContext = gyarto;

            var modell = (from s in cnIT_Inventory.TE_Leltar select new { s.Modell }).Distinct().ToList();
            cbModell.DataContext = modell;

            cbCsoport.Text = "";
            cbFelhasznalo.Text = "";

            // cbHely.SelectedItem = null;
            // cbFelhasznalo.SelectedItem = null;
            //if (cbHely.SelectedIndex == -1)
            //{
            //    var felh = (from u in cnIT_Inventory.AD_Users select new { u.cn }).ToList();
            //    cbFelhasznalo.DataContext = felh;
            //    cbFelhasznalo.Text = "";

            //}
            //cbStatus.ItemsSource = new List<string> { "Használatban", "Használaton kívül" };



            //cbStatus.Items.Add("Használatban");
            //cbStatus.Items.Add("Használaton kívül");


            // var criteria = ;
            //foreach (var cItem in cbHely.ItemsSource )
            //{
            //    if (cbHely.SelectedItem != null)
            //    {
            //        //string selectedItemName = this.cbHely.GetItemText(this.cbHely.SelectedItem);
            //        criteria.Add(new Predicate<AD_Users>(x => x.cn == cbHely.SelectedItem.ToString()));
            //        break;
            //    }
            //}
            // var felh = (from y in cnIT_Inventory.AD_Users select new { y.cn}).ToList();
            // cbFelhasznalo.ItemsSource = felh;


            //cnIT_Inventory.AD_Users.ToList();

        }

        /// <summary>
        /// Kilépés menüpontra kattintva bezáródik az alkalmazás.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void miKilepes_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Létrehozok egy TE_Leltar tipusú _leltar változót, ahol az adatbázis mező neveket, behelyettesítem a text/combobox .Text paraméterével.
        /// Majd ezt a listát, illetve a tartalmát, hozzáadom az adatbázishoz és elmentem. Ha sikeres a mentés akkor felugrik egy ablak ami értesít erről és
        /// a mezők tartalmát kiürítem.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btMentes_Click(object sender, RoutedEventArgs e)
        {
            var _leltar = new TE_Leltar
            {
                Nev = tbNev.Text.ToString(),
                Hely = cbHely.Text.ToString() /*+ tbHely.Text.ToString()*/,
                Felhasznalo = cbFelhasznalo.Text.ToString()/* + tbFelhasznalo.Text.ToString()*/,
                Csoport = cbCsoport.Text.ToString()/* + tbCsoport.Text.ToString()*/,
                Statusz = cbStatus.Text.ToString(),
                Tipusok = cbTipusok.Text.ToString(),
                //Tipusok = tbTipusok.Text.ToString(),
                Gyarto = cbGyarto.Text.ToString(),
                Modell = cbModell.Text.ToString(),
                Sorozatszam = tbSorozatszam.Text.ToString(),
                Leltari_Szam = tbLeltariszam.Text.ToString()
            };
            cnIT_Inventory.TE_Leltar.Add(_leltar);
            cnIT_Inventory.SaveChanges();




            //try
            //{
                
            //    cnIT_Inventory.SaveChanges();
            //}
            //catch (DbEntityValidationException err)
            //{
            //    foreach (var eve in err.EntityValidationErrors)
            //    {
            //        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
            //            eve.Entry.Entity.GetType().Name, eve.Entry.State);
            //        foreach (var ve in eve.ValidationErrors)
            //        {
            //            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
            //                ve.PropertyName, ve.ErrorMessage);
            //        }
            //    }
            //    throw;
            //}


                if (MessageBox.Show("Sikeres mentés", "IT_Inventory", MessageBoxButton.OK, MessageBoxImage.Information) == MessageBoxResult.OK)
            {
                tbNev.Text = "";
                cbHely.Text = "";
                cbFelhasznalo.Text = "";
                cbCsoport.Text = "";
                cbStatus.SelectedIndex = -1;
                cbTipusok.SelectedIndex = -1;
                cbGyarto.Text = "";
                cbModell.Text = "";
                tbSorozatszam.Text = "";
                tbLeltariszam.Text = "";
                cbFelhasznalo.Items.Clear();

            }
        }

        /// <summary>
        /// A lekérdezés menüpontra kattintva aktiválódik. Megjelenik a dgAdatracs és lekérem az adatbázisból egy er nevű listába az adatokat.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void miLekerdezes_Click(object sender, RoutedEventArgs e)
        {
            grItemek.Visibility = Visibility.Collapsed;
            dgAdatracs.Visibility = Visibility.Visible;

            var er = (from x in cnIT_Inventory.TE_Leltar select new { x.Nev, x.Hely, x.Felhasznalo, x.Csoport, 
                x.Statusz, x.Tipusok, x.Gyarto, x.Modell, x.Sorozatszam, x.Leltari_Szam}).ToList();
            
            dgAdatracs.ItemsSource = er;
        }

        /// <summary>
        /// Ide az AD_Users tábla adatait kérdezem le, ennek csak ellenőrzési célja van.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void miADLekerdezes_Click(object sender, RoutedEventArgs e)
        {
            grItemek.Visibility = Visibility.Collapsed;
            dgAdatracs.Visibility = Visibility.Visible;
            var er = (from x in cnIT_Inventory.AD_Users select new { x.sAMAccountName, x.sn, x.cn, x.givenName, x.title, x.displayName, x.physicalDeliveryOfficeName,
                x.objectGUID, x.objectSid }).ToList();

            dgAdatracs.ItemsSource = er;
        }


        //private void cbHely_DropDownClosed(object sender, EventArgs e)
        //{
        //    // MessageBox.Show(cbHely.Text);

        //    //IEnumerable<AD_Users> aD_Users = cnIT_Inventory.AD_Users.Where(x => x.physicalDeliveryOfficeName == cbHely.Text);

        //    //cbFelhasznalo.Items.Clear();
        //    //foreach (var x in aD_Users)
        //    //{
        //    //    cbFelhasznalo.Items.Add(x);
        //    //}
        //}

        /// <summary>
        /// Az új tétel menüpontnál feljövő mezők alatt a Mégse gombra kattintva aktiválódik a metódus.
        /// Ez nem menti l az adatokat, hanem csak kitörli a mezőket.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btMegse_Click(object sender, RoutedEventArgs e)
        {
            tbNev.Text = "";
            cbHely.Text = "";
            cbFelhasznalo.Text = "";
            cbCsoport.Text = "";
            cbStatus.SelectedIndex = -1;
            cbTipusok.SelectedIndex = -1;
            //tbTipusok.Text = "";
            cbGyarto.Text = "";
            cbModell.Text = "";
            tbSorozatszam.Text = "";
            tbLeltariszam.Text = "";
            cbFelhasznalo.Items.Clear();

        }

        /// <summary>
        /// Ha a csoport mezőről ellépünk (tehát elveszti a focus-t) akkor a felhasználók mezőbe betölti azokat a felhasználókat akik a kiválasztott csoporthoz tartoznak.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbCsoport_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (cbFelhasznalo.Text == "")
            //{
                IEnumerable<AD_Users> aD_Users = cnIT_Inventory.AD_Users.Where(x => x.physicalDeliveryOfficeName == cbCsoport.Text);

                cbFelhasznalo.Items.Clear();
                foreach (var x in aD_Users)
                {
                    cbFelhasznalo.Items.Add(x);
                }
            //}
        }

        /// <summary>
        /// Ha a sorozatszám mező elveszti a focus-t akkor leellenőrzöm, hogy a beírt sorozatszámra van e találat az adazbázisban, és ha van,
        /// akkor adatbázisból kitölti az adott sorotaszámú gép nevét, gyártóját és modelljét. Ha nincs egyezés a sorozatszámra az adatbázisban, akkor nem történik semmi.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbSorozatszam_LostFocus(object sender, RoutedEventArgs e)
        {
            IEnumerable<TE_Gepek> tE_Gepek = cnIT_Inventory.TE_Gepek;

           
                var q = (from a in tE_Gepek
                         where a.Serial_number == tbSorozatszam.Text
                         select a).SingleOrDefault();
            if(q != null)
            {
                if (tbSorozatszam.Text == q.Serial_number.ToString())
                {
                    tbNev.Text = q.Computer_name;
                    cbGyarto.Text = q.Device_manufacturer;
                    cbModell.Text = q.Device_model;
                }
            }
            

        }

        //private void cbHely_GotFocus(object sender, RoutedEventArgs e)
        //{
        //    if (cbFelhasznalo.Text == "")
        //    {
        //        var hely = (from y in cnIT_Inventory.AD_Users select new { y.physicalDeliveryOfficeName }).Distinct().ToList();
        //        cbHely.DataContext = hely;
        //        //IEnumerable<AD_Users> aD_Users = cnIT_Inventory.AD_Users.Where(x => x.physicalDeliveryOfficeName == cbHely.Text);

        //        //cbFelhasznalo.Items.Clear();
        //        //foreach (var x in aD_Users)
        //        //{
        //        //    cbFelhasznalo.Items.Add(x);
        //        //}
        //    }
        //}

        //private void cbFelhasznalo_GotFocus(object sender, RoutedEventArgs e)
        //{

        //    if (cbHely.Text == "")
        //    {
        //        var felh = (from y in cnIT_Inventory.AD_Users select new { y.cn }).ToList();
        //        cbFelhasznalo.DataContext = felh;
        //        //IEnumerable<AD_Users> aD_Users = cnIT_Inventory.AD_Users.Where(x => x.cn == cbFelhasznalo.Text);

        //        //cbHely.Items.Clear();
        //        //foreach (var k in aD_Users)
        //        //{
        //        //    cbHely.Items.Add(k);
        //        //}
        //    }
        //}


        //private void cbFelhasznalo_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    if (cbHely.Text == "")
        //    {
        //        IEnumerable<AD_Users> aD_Users = cnIT_Inventory.AD_Users.Where(x => x.cn == cbFelhasznalo.Text);

        //        cbHely.Items.Clear();
        //        foreach (var k in aD_Users)
        //        {
        //            cbHely.Items.Add(k);
        //        }
        //    }
        //}

    }

    //private void cbHely_KeyUp(object sender, KeyEventArgs e)
    //{
    //    IEnumerable<AD_Users> aD_Users = cnIT_Inventory.AD_Users.Where(x => x.physicalDeliveryOfficeName == cbHely.Text);

    //    cbFelhasznalo.Items.Clear();
    //    foreach (var x in aD_Users)
    //    {
    //        cbFelhasznalo.Items.Add(x);
    //    }
    //}



    /*  private void cbHely_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
          //foreach (var item in cnIT_Inventory.AD_Users)
          //{
          //    cbFelhasznalo.Items.Add(item.cn.ToString());
          //}


          IEnumerable<AD_Users> aD_Users = cnIT_Inventory.AD_Users.Where(x => x.physicalDeliveryOfficeName == x.cn);


          foreach (var x in aD_Users)
          {
              cbFelhasznalo.Items.Add(x);
          }
          //var felhasznalo = (from x in cnIT_Inventory.AD_Users where cbHely.SelectedItem.ToString() == x.cn.ToString() select x).ToList();
          //cbFelhasznalo.ItemsSource = felhasznalo;


          //var enAktualis = ((ComboBox)sender).SelectedItem as AD_Users;
          //cbFelhasznalo.SelectedItem = enAktualis;

      }*/

    //private void tbSorozatszam_KeyUp(object sender, KeyEventArgs e)
    //{
    //    if (e.Key == Key.Enter)
    //    {
    //        TextBox txtBox = e.Source as TextBox;
    //        if (txtBox != null)
    //        {
    //            this.tbSorozatszam.Text = txtBox.Text;
    //            this.tbSorozatszam.Background = new SolidColorBrush(Colors.LightGray);
    //        }
    //    }
    //}
}

