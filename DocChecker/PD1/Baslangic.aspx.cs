using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PD1
{
    public partial class Baslangic : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private SqlConnection baglanti = new SqlConnection(@"Data Source=.;Initial Catalog=TezKontrol;Integrated Security=True;MultipleActiveResultSets=True");
        protected void Button1_Click1(object sender, EventArgs e)
        {
            if (k_adi.Text == "" || sifre.Text == "")
            {
                Label7.Text = "Boş girilemez...";
            }
            else
            {
                string kullanici, sifre1;
                baglanti.Open();
                SqlCommand komut = new SqlCommand("select * from kullanicilar where kAdi='" + k_adi.Text + "'", baglanti);
                SqlDataReader oku = komut.ExecuteReader();
                if (oku.Read())
                {
                    kullanici = oku["kAdi"].ToString();
                    sifre1 = oku["sifre"].ToString();
                    if (k_adi.Text == kullanici && sifre.Text == sifre1)
                    {
                        Response.Redirect("Anasayfa.aspx");
                    }
                    else
                    {
                        Label7.Text = "Lütfen bilgileri kontrol ediniz...";
                    }
                }
            }
        }
    }
}