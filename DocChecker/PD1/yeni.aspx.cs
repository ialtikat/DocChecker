using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PD1
{
    public partial class yeni : System.Web.UI.Page
    {
        private SqlConnection baglanti = new SqlConnection(@"Data Source=.;Initial Catalog=TezKontrol;Integrated Security=True;MultipleActiveResultSets=True");
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from kullanicilar where kAdi='" + k_adi.Text + "'", baglanti);
            SqlDataReader oku = komut.ExecuteReader();
            if (oku.Read())
            {

                Label6.Text = k_adi.Text + " Kullanıcı Adı Zaten var...";
            }
            else if (k_adi.Text == "" || sifre.Text == "" || sifre_tekrar.Text == "")
            {
                Label6.Text = "Boş girilemez...";
            }
            else
            {
                if (sifre.Text == sifre_tekrar.Text)
                {
                    SqlDataAdapter sorgu = new SqlDataAdapter("insert into kullanicilar (kAdi,sifre) values (@kadi,@sifre)", baglanti);
                    sorgu.SelectCommand.Parameters.AddWithValue("@kadi", k_adi.Text);
                    sorgu.SelectCommand.Parameters.AddWithValue("@sifre", sifre.Text);

                    DataSet ds = new DataSet();
                    sorgu.Fill(ds);
                    sorgu.Dispose();
                    ds.Dispose();
                    baglanti.Close();
                    Response.Redirect("Anasayfa.aspx");
                }
                else
                {
                    Label6.Text = "Şifreler birbiri ile uyuşmamaktadır...";
                }
            }
        }
    }
}