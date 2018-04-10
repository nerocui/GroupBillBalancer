using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Budget
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ObservableCollection<Person> people;
        List<Person> paying = new List<Person>();
        List<Person> receiving = new List<Person>();
        List<Person> none = new List<Person>();
        ObservableCollection<Transaction> transactions;
        List<string> imgs;
        int wtf_index;
        public MainPage()
        {
            wtf_index = 0;
            imgs = new List<string>();
            imgs.Add("ms-appx:Assets/wtf/wtf1.jpg");
            imgs.Add("ms-appx:Assets/wtf/wtf2.gif");
            imgs.Add("ms-appx:Assets/wtf/wtf3.jpg");
            imgs.Add("ms-appx:Assets/wtf/wtf4.jpg");
            imgs.Add("ms-appx:Assets/wtf/wtf5.png");
            imgs.Add("ms-appx:Assets/wtf/wtf6.jpg");
            imgs.Add("ms-appx:Assets/wtf/wtf7.gif");
            imgs.Add("ms-appx:Assets/wtf/wtf8.jpg");
            transactions = new ObservableCollection<Transaction>();
            people = new ObservableCollection<Person>();
            this.InitializeComponent();
            this.DataContext = this;
        }

        private void calculate_button_Click(object sender, RoutedEventArgs e)
        {
            transactions.Clear();
            double total = 0;
            int size = 0;
            foreach(Person p in people)
            {
                total += p.amount;
                size++;
            }

            double each = total / size;
            

            foreach(Person p in people)
            {
                if (p.amount > each)
                {
                    p.amount -= each;
                    receiving.Add(p);
                } 
                else if (p.amount < each)
                {
                    p.amount = each - p.amount;
                    paying.Add(p);
                }
                else if (p.amount == each)
                {
                    p.amount = 0;
                    none.Add(p);
                }
            }

            paying = paying.OrderBy(p => p.amount).ToList();
            receiving = receiving.OrderByDescending(p => p.amount).ToList();
            
            recurr();
            


        }

        

        private void recurr()
        {
            int p = 0;
            int r = 0;
            while(paying.Count != 0 && receiving.Count != 0)
            {
                Person pp = paying[p];
                Person rr = receiving[r];
                double actual_amount = 0;
                if(pp.amount == rr.amount)
                {
                    actual_amount = rr.amount;
                    pp.amount = 0;
                    rr.amount = 0;
                    none.Add(pp);
                    none.Add(rr);
                    paying.Remove(pp);
                    receiving.Remove(rr);
                    
                }else if(pp.amount > rr.amount)
                {
                    actual_amount = rr.amount;
                    pp.amount -= rr.amount;
                    rr.amount = 0;
                    none.Add(rr);
                    receiving.Remove(rr);
                    paying = paying.OrderBy(o => o.amount).ToList();
                    
                }else if(pp.amount < rr.amount)
                {
                    actual_amount = pp.amount;
                    rr.amount -= pp.amount;
                    pp.amount = 0;
                    none.Add(pp);
                    paying.Remove(pp);
                    receiving.OrderByDescending(o => o.amount).ToList();
                }

                Transaction tuple = new Transaction(pp.name, rr.name, actual_amount);
                transactions.Add(tuple);
            }
        }

        private void add_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = name_input.Text;
                foreach (Person pp in people)
                {
                    if (pp.name.ToLower().Equals(name.ToLower()))
                    {
                        double aa = pp.amount + Double.Parse(amount_input.Text);
                        Person ppp = new Person(pp.name, aa);
                        people.Remove(pp);
                        people.Add(ppp);
                        name_input.Text = "";
                        amount_input.Text = "";
                        wtf_img.Source = null;
                        wtf_grid.Visibility = Visibility.Collapsed;
                        return;
                    }
                }
                double amount = Double.Parse(amount_input.Text);
                Person p = new Person(name, amount);
                people.Add(p);
                name_input.Text = "";
                amount_input.Text = "";
                wtf_img.Source = null;
                wtf_grid.Visibility = Visibility.Collapsed;
            }
            catch(FormatException)
            {
                wtf_grid.Visibility = Visibility.Visible;
                Uri uri = new Uri(imgs[wtf_index]);
                BitmapImage bitmapimage = new BitmapImage(uri);
                wtf_img.Source = bitmapimage;
                name_input.Text = "";
                amount_input.Text = "";
                wtf_index++;
                wtf_index = wtf_index % imgs.Count;
            }
            
        }
        
        private void clear_button_Click(object sender, RoutedEventArgs e)
        {
            people.Clear();
            transactions.Clear();
        }

        private void info_Click(object sender, RoutedEventArgs e)
        {
            ExampleInAppNotification.Show("This is simply the best budget app ever created in all universes. Simply add all people in you group and how much they spend. Then the AI engine powered by state of the art quantum computer will calculate how much each person owe. You welcome. (Try enter a invalid amount format, I dare you.)");
        }
    }
}
