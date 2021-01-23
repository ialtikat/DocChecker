using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Word = Microsoft.Office.Interop.Word;
using System.Xml;
using Microsoft.Office.Interop.Word;


namespace PD1
{
    public partial class index : System.Web.UI.Page
    {
        List<string> data1 = new List<string>();
        List<string> fontFamly = new List<string>();
        List<string> data = new List<string>();
        public void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                WordFileToRead.SaveAs(Server.MapPath(WordFileToRead.FileName));
                object filename = Server.MapPath(WordFileToRead.FileName);
                Word.Application AC = new Word.Application();
                Word.Document doc = new Word.Document();
                object missing = System.Reflection.Missing.Value;
                doc = AC.Documents.Open(ref filename);
                foreach (Word.Paragraph objParagraph in doc.Paragraphs)
                {
                    data1.Add(objParagraph.Range.Text.Trim());
                    //Tez yazı stili
                    fontFamly.Add(objParagraph.Range.Font.Name);
                }
                ListBox1.Items.Add(SayfaBoyut(doc));
                ListBox1.Items.Add(sayfaKenarBosluklari(doc));
                ((Word.Document)doc).Close();
                ((Word.Application)AC).Quit();
                for (int i = 0; i < data1.Count; i++)
                {
                    if (data1[i].ToString() != "" && data1[i].ToString() != " " && data1[i].ToString() != "" && data1[i].ToString() != "/" && data1[i].ToString() != "/r")
                    {
                        data.Add(data1[i].ToString().Trim());
                    }
                }
                //Fontfamly listboxa yazdırma
                string[] font = fontFamly.Distinct().ToArray();
                for (int i = 0; i < font.Length; i++)
                {
                    ListBox1.Items.Add("Tezde bu yazı fontları kullanılmıştır: " + font[i]);
                }
                string karakter = null;
                string tektirnak = null;
                string[] duzen = { "ÖNSÖZ", "İÇİNDEKİLER", "ÖZET", "ABSTRACT", "1.GİRİŞ",  "5.BULGULAR VE TARTIŞMA", "6.SONUÇLAR", "KAYNAKLAR", "ÖZGEÇMİŞ" };
                //İçindekiler tablosu
                ListBox1.Items.Add(Icindekiler(data));
                ListBox1.Items.Add(KaynakcaSiralamaKontrolu(data).ToString());
                ListBox1.Items.Add(OnaylananTezBaslikKontrolu(data));
                ListBox1.Items.Add(TezJuriCheck(data));
                ListBox1.Items.Add(TablolarListesi(data, "ŞEKİLLER LİSTESİ", "TABLOLAR LİSTESİ", "Şekil"));
                ListBox1.Items.Add(TablolarListesi(data, "TABLOLAR LİSTESİ", "EKLER LİSTESİ", "Tablo"));
                ListBox1.Items.Add(TablolarListesi(data, "EKLER LİSTESİ", "SİMGELER VE KISALTMALAR", "Ek"));
                ListBox1.Items.Add(tezDuzenKontrol(data, duzen));
                ListBox1.Items.Add(AnahtarKelimeAra(data));
                ListBox1.Items.Add(KeywordsCheck(data));
                //Karakter ara
                char[] chr = { '{', '}', '(', ')', '[', ']' };
                for (int a = 0; a < chr.Length; a += 2)
                {
                    for (int i = 0; i < data.Count; i++)
                    {
                        karakter = karakterAra(data[i], chr[a], chr[a + 1]);
                        if (karakter != null)
                        {
                            ListBox1.Items.Add(karakter);
                        }
                    }
                }
                //Tektırnak ara
                for (int i = 0; i < data.Count; i++)
                {
                    tektirnak = tirnakKontrolu(data[i], '"');
                    if (tektirnak != null)
                    {
                        ListBox1.Items.Add(tektirnak);
                    }
                }
                ListBox1.Items.Add(KeywordsCheck(data));
                ListBox1.Items.Add(onsozKontrol(data));
                ListBox1.Items.Add(tirnakKelime(data));
                ListBox1.Items.Add(kaynakAtıf(data));
            }
            catch (Exception)
            {
                ListBox1.Items.Add("Bir hata meydana geldi sayfayı yenileyip  tekrar deneyiniz!!!");
            }
        }
        // içindekiler metodu
        //İçindekiler kısmını listeye çekip, bütün tez ile karşılaştırma yapıp bize değer döndürmektedir.
        private string Icindekiler(List<string> data)
        {
            try
            {
                int sayac = 0;
                //Değerlerimizin bulunduğu list.
                List<string> deger = new List<string>();
                //İçindekiler tablosunu çekebilmemiz için gerekli şartlar.
                int index = data.FindIndex(a => a.Contains("İÇİNDEKİLER"));
                if (index == -1)
                {
                    index = data.FindIndex(a => a.Contains("İçindekiler"));
                }
                var newList = data.Where(x => x.Contains("ÖZET")).ToList();
                //deger listemizin içerisini düzenlemek için kullandığımız split şartları.
                char[] ayirici = { '.', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
                //Tezden içindekiler kısmını çektiğimiz bölüm.
                for (int i = index; i < data.Count; i++)
                {
                    if (data[i] == newList[1])
                    {
                        goto a;
                    }
                    else
                    {
                        string[] parca = data[i].Split(ayirici);
                        for (int k = 0; k < parca.Length; k++)
                        {
                            deger.Add(parca[k]);
                        }
                    }
                }
            a:
                deger.Remove("Sayfa");
                //Karşılaştırma işleminin gerçekleştiği kısım.
                //Burada tez ile deger listimizi karşılaştırıp sonucu gönderiyoruz.
                for (int s = 0; s < deger.Count; s++)
                {
                    if (data.Contains(deger[s]))
                    {
                        sayac = sayac + 1;
                    }
                    if (sayac < 2)
                    {
                        return "İçindekiler tablosu hatalıdır.";
                    }
                    else
                    {
                        sayac = 0;
                    }
                }
                return "İçindekiler tablosunda hata yoktur.";
            }
            catch (Exception)
            {

                return null;
            }

        }
        /// <summary>
        /// Tez kontrolü yaparken kaynakların alfabetik sıralı olması gerekmektedir.
        /// Kaynakca yaziminda oncelikle Soyadı, Yazarın adının baş harfi kullanılmaktadır.
        /// Elde edilen veri ushort türünde geri döndürülmekte gelen değere göre hata mesajı verilecektir.
        /// Eğer bir hata ile karşılaşılırsa -1 değeri geriye döndürülecektir.
        /// </summary>
        /// <param name="paragraf"></param>
        /// <returns></returns>
        public string KaynakcaSiralamaKontrolu(List<string> paragraf)
        {
            try
            {
                short say = 0;

                List<string> kaynaklarlistesi = new List<string>();

                char[] seperators = { ' ', ',' };

                int kaynaakca = paragraf.FindLastIndex(x => x.Contains("KAYNAKÇA"));
                if (kaynaakca == -1)
                {
                    kaynaakca = paragraf.FindLastIndex(x => x.Contains("KAYNAKLAR"));
                }

                int ekler = paragraf.FindLastIndex(x => x.Contains("EKLER"));


                for (int i = kaynaakca + 1; i < ekler; i++)
                {
                    string[] veri = paragraf[i].Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                    kaynaklarlistesi.Add(veri[0]);
                }

                List<string> clonedList = new List<string>(kaynaklarlistesi);

                clonedList.Sort();

                for (int i = 0; i < kaynaklarlistesi.Count; i++)
                {
                    if (kaynaklarlistesi[i] != clonedList[i])
                    {
                        say += 1;
                        if (say == 10)
                        {
                            break;
                        }
                    }
                }
                if (say > 0)
                {
                    return "Kaynak sıralaması alfabetik yapılmamıştır.";
                }
                else
                {
                    return "Kaynak sıralaması alfabetik yapılmıştır.";
                }
            }
            catch (Exception)
            {
                return "Kaynakça sıralamsı kontrol edilirken bir hata ile kakrşılaşıldı.";
            }

        }


        /// <summary>
        /// Sayfa boyunun tez yazım kurallarına göre A4 olması gerekmektedir. Bunun kontrolünün sağlanması için kullanılmaktadır.
        /// </summary>
        /// <param name="doc">doc parametresi Word.Document türünde okunan tez dosyasının değişkenini ifade eder.</param>
        /// <returns></returns>
        public string SayfaBoyut(Word.Document doc)
        {  /*
         * Sayfa boyutu wdPaperA4 olmalı. Tez kontrolüne göre tüm sayfa boyutu A4 ile yazılmalı.
         */
            try
            {
                if (doc.PageSetup.PaperSize.ToString() == "wdPaperA4")
                {
                    return "Tezde bulunan sayfalar doğru bir şekilde A4 olarak hazırlanmış.";
                }
                else
                {
                    return "Tezde bulunan sayfalar doğru bir şekilde hazırlanmamış. Sayfa boyutu A4 olarak düzeltilmelidir.";
                }
            }
            catch (Exception)
            {

                return "Sayfa Boyutu kontrol edilirken bir hata ile karşılaşıldı.";

            }





        }


        /// <summary>
        /// Sayfa Kenar bosluklarinin kontrolu amaciyla kullanilmaktadir. Varsayilan degerler
        /// Top/Ust = 84.95, Left/Sol = 91.45, Bottom/Alt = 70.55, Right/Sag = 70.55
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public string sayfaKenarBosluklari(Word.Document doc)
        {


            /* 3 centimeters = 85.0393701 PostScript points üst kenar boşluğu
             * 3,25 centimeters = 92,1259843 PostScript points sol kenar boşluğu
             * 2,5 centimeters = 70,8661417 PostScript points sağ ve alt kenar boşluğu
             * top = üst, bottom = alt, left = sol kenar, right = sag kenar
             */
            //top, left, bottom, right
            try
            {
                var sonuc = "";
                string[] kenar = { "84.95", "91.45", "70.55", "70.55" };
                string[] docKenar = new string[4];

                foreach (Word.Paragraph objParagraph in doc.Paragraphs)
                {
                    docKenar[0] = objParagraph.Range.PageSetup.TopMargin.ToString();
                    docKenar[0] = objParagraph.Range.PageSetup.LeftMargin.ToString();
                    docKenar[0] = objParagraph.Range.PageSetup.BottomMargin.ToString();
                    docKenar[0] = objParagraph.Range.PageSetup.RightMargin.ToString();
                    break;
                }

                sonuc += docKenar[0] != kenar[0] ? "Sayfa üst kenar boşluğu hatalı. " : "Sayfa üst kenar boşluğu doğru hazırlanmış. ";
                sonuc += docKenar[1] != kenar[1] ? "Sayfa sol kenar boşluğu hatalı. " : "Sayfa sol kenar boşluğu doğru hazırlanmış. ";
                sonuc += docKenar[2] != kenar[2] ? "Sayfa alt kenar boşluğu hatalı. " : "Sayfa alt kenar boşluğu doğru hazırlanmış. ";
                sonuc += docKenar[3] != kenar[3] ? "Sayfa sağ kenar boşluğu hatalı. " : "Sayfa sağ kenar boşluğu doğru hazırlanmış. ";

                return sonuc;
            }
            catch (Exception)
            {

                return "Sayfa kenar boşlukları kontrol edilirken bir hata ile karşılaşıldı.";
            }


        }


        /// <summary>
        /// Onaylanan tezin başlığı ile hazırlanan tezin başlığını kontrol eder.
        /// </summary>
        /// <param name="paragraf">Word dokümanının paragraf olarak eklendiği listenin gönderilmesi gerekmektedir.</param>
        /// <returns></returns>
        public string OnaylananTezBaslikKontrolu(List<string> paragraf)
        {

            // Tez Yazarı ifadesi de kullanılabilir index-1 olmalı

            try
            {
                sbyte p1_index = (sbyte)paragraf.FindIndex(a => a.Contains("Yüksek Lisans Tezi"));
                int p2_index = paragraf.FindIndex(a => a.Contains("Başlığı:"));
                string baslik = paragraf[p2_index - 2];

                if (baslik.Trim().ToUpper() != paragraf[p2_index].Split(':')[1].Trim().ToUpper())
                {
                    return "Onaylanan tezin başlık kontrolü tutarlı değildir.";
                }
                else
                {
                    return "Onaylanan tezin başlık kontrolü tutarlıdır.";
                }

            }
            catch (Exception)
            {
                return "Tezin başlık kontrolü yapılırken hata ile karşılaşıldı.";
            }

        }


        /// <summary>
        /// Tez juri kontrolu
        /// İkinci danışman yoksa Başkana ek olarak Yüksek Lisans için 1 Üye Doktora için 3 Üye
        ///  İkinci danışmanın jüride bulunduğu durumlarda Doktora için 4 Üye, Yüksek Lisans için 2 Üye ve Başkan olmalıdır.
        /// </summary>
        /// <param name="paragraf">Word dokümanının paragraf olarak eklendiği listenin gönderilmesi gerekmektedir.</param>
        /// <returns></returns>
        public string TezJuriCheck(List<string> paragraf)
        {
            // İkinci danışman yoksa Başkana ek olarak Yüksek Lisans için 1 Üye Doktora için 3 Üye
            // İkinci danışmanın jüride bulunduğu durumlarda Doktora için 4 Üye, Yüksek Lisans için 2 Üye ve Başkan olmalıdır.
            try
            {
                sbyte uyeSay = 0;
                sbyte baskanSay = 0;
                int p1_index = paragraf.FindIndex(a => a.Contains("TEZ ONAYI"));
                int p2_index = paragraf.FindIndex(a => a.Contains("BEYAN"));
                int p3_index = paragraf.FindIndex(a => a.Contains("İkinci Danışman:"));


                if (p3_index != -1)
                {
                    if (p1_index < p3_index && p3_index < p2_index)
                    {
                        for (int i = p1_index + 1; i < p2_index; i++)
                        {
                            if (paragraf[i].Contains("Üye"))
                            {
                                uyeSay += 1;
                            }
                            if (paragraf[i].Contains("Başkan"))
                            {
                                baskanSay += 1;
                            }
                        }
                        if (uyeSay != 2 && baskanSay != 1)
                        {
                            return "İkinci Danışman jüride bulunduğu durumda 1 Başkan ve 2 Üye bulunmalıdır.";
                        }
                        else
                        {
                            return "Jüri düzgün hazırlanmış.";
                        }
                    }
                    else
                    {
                        return "Tez Onayı sıralaması düzgün hazırlanmamış.";
                    }
                }
                else
                {

                    for (int i = p1_index + 1; i < p2_index; i++)
                    {
                        if (paragraf[i].Contains("Üye"))
                        {
                            uyeSay += 1;
                        }
                        if (paragraf[i].Contains("Başkan"))
                        {
                            baskanSay += 1;
                        }
                    }
                    if (uyeSay != 1 && baskanSay != 1)
                    {
                        return "Jüri düzgün hazırlanmamış. İkinci Danışmanın jüride bulunmadığı durumda 1 Başkan ve 1 Üye bulunmalıdır.";
                    }
                    else
                    {
                        return "Jüri düzgün hazırlanmış.";
                    }
                }
            }
            catch (Exception)
            {

                return "Jüri kontrolü yapılırken bir hata ile karşılaşıldı.";
            }


        }









        /// <summary>
        ///  Word dokümanı içerisinde Şekil, Tablo, Dipnot aramak için kullanılır.
        /// </summary>
        /// <param name="paragraf">Paragraf liste halindeki dokümanı belirtir.</param>
        /// <param name="baslangicListesi">baslangicListesi doküman içerisinde aranması istenen listenin stringini belirtir.</param>
        /// <param name="bitisListesi">bitisListesi doküman içerisinde aranması istenen listeden sonra gelen ilk string ifadedir.</param>
        /// <param name="arananKelime">arananKelime aranacak string ifadeyi belirtir. "Dipnot", "Tablo", "Ek-" parametreleri ile kullanılmalıdır. </param>
        /// <returns></returns>
        public string TablolarListesi(List<string> paragraf, string baslangicListesi, string bitisListesi, string arananKelime)
        {



            // Başlangıç Listesi metin içerisinde aranması istenen listenin başlangıç stringini belirtir.
            // Bitiş listesi aranması istenen listeden sonra gelen ilk stringi belirtir.
            // arananKelime Dipnot, Çizelge, Tablo, Şekil gibi türü ifade eder.


            /* Neden liste? Array ile arasındaki fark 60000e kadar int ekleme için.
             * Memory usage for arrays and Lists:
                List generic:  6.172 MB
                Integer array: 5.554 MB
              Lookup performance for arrays and Lists:
                List generic:  1043.4 ms
                Integer array:  980.2 ms
            */

            // int p1_index = paragraf.FindIndex(a => a.Contains("TABLOLAR LİSTESİ"));

            try
            {
                List<string> tabloIsim = new List<string>();


                int p1_index = paragraf.LastIndexOf(baslangicListesi);
                int p2_index = paragraf.LastIndexOf(bitisListesi);

                for (int i = p1_index + 2; i < p2_index - 1; i++)
                {

                    if (paragraf[i].Count() >= 20 && paragraf[i].Contains(arananKelime.Insert(arananKelime.Length, " ")))
                    {
                        //İki kere split etmeye gerek yok buna bakılmalı
                        tabloIsim.Add(paragraf[i].Split(' ')[0] + " " + paragraf[i].Split(' ')[1]);
                    }
                    else if (paragraf[i].Count() == 1)
                    {
                        continue;
                    }
                    else
                    {
                        return $"{arananKelime} doğru hazırlanmamış.";
                    }


                    //Tablo 2.1.

                    // Eğer sayfa numarası ekleniyorsa içerisine buna dikkat edilmeli

                }


                for (int i = p2_index + 1; i < paragraf.Count; i++)
                {
                    for (int a = 0; a < tabloIsim.Count; a++)
                    {
                        if (paragraf[i].Contains(tabloIsim[a]))
                        {
                            tabloIsim.Remove(tabloIsim[a]);
                            if (tabloIsim.Count == 0)
                            {
                                break;
                            }
                        }
                    }
                }


                if (tabloIsim.Count > 0)
                {
                    return $"{arananKelime} atıflarında hata bulunmaktadır. {tabloIsim.Count.ToString()} adet tabloya metin içerisinde atıf yapılmamış.";
                    // {0} tabloIsim.Count()
                }
                else if (tabloIsim.Count == 0)
                {
                    // 
                    return $"{arananKelime} atıflarında hata bulunmamaktadır.";
                }
                else
                {
                    return $"{arananKelime} hata ile kaşılaşıldı.";
                }
            }
            catch (Exception)
            {

                return "Atıf kontrolü yapılırken bir hata ile karşılaşıldı.";
            }

        }


        /// <summary>
        /// Eger icindekiler kisminda belirtilen sira bir duzen icerisinde ise bunu dizi olarak gönderilmeli ve kontrol işlemi gerçekleştirilmektedir.
        /// Kontrol edilmesi gereken tez stilinin farklılıklar gösterebilme durumu sebebiyle bir dizi şeklinde verilmesi daha doğru sonuç elde edilmesine katkı sağlayacaktır.
        /// </summary>
        /// <param name="paragraf">Word dokümanının paragraf olarak eklendiği listenin gönderilmesi gerekmektedir.</param>
        /// <param name="duzen">Exam: string[] duzen = {ÖNSÖZ, İÇİNDEKİLER, ÖZET ABSTRACT 1. GİRİŞ, 4. MATERYAL VE METOT, 5. BULGULAR VE TARTIŞMA, 6. SONUÇLAR, KAYNAKLAR,  ÖZGEÇMİŞ} </param>
        /// <returns></returns>
        public static string tezDuzenKontrol(List<string> paragraf, string[] duzen)
        {
            // Eğer sayi değişkenimizdeki değer 0 ise tez düzgün sıralı şekilde yazılmıştır.
            // Ancak değilse ve hatta 0 dan daha büyük bir değer ise tez içerik sıralamasında hata meydana gelmiş demektir.

            try
            {
                sbyte sayi = 0;
                //ÖNSÖZ, İÇİNDEKİLER, ÖZET ABSTRACT 1. GİRİŞ, 4. MATERYAL VE METOT, 5. BULGULAR VE TARTIŞMA, 6. SONUÇLAR, KAYNAKLAR,  ÖZGEÇMİŞ, 

                List<int> sira = new List<int>();

                for (int i = 0; i < duzen.Length; i++)
                {
                    sira.Add(paragraf.IndexOf(duzen[i]));
                }

                for (int i = 1; i < sira.Count; i++)
                {

                    if (sira[i - 1] < sira[i])
                    {
                        continue;
                    }
                    else
                    {
                        sayi += 1;
                    }
                }

                //return "Tez düzeninde adet hata bulundu." + sayi.ToString();

                return "Tez düzeninde " + sayi.ToString() + " adet hata bulundu.";
            }
            catch (Exception)
            {

                return "Tez düzeni kontrol edilirken bir hata ile karşılaşıldı.";
            }

        }


        /// <summary>
        /// Turkce hazirlanan anahtar kelimelerin sayilarini hesaplar. Kural anahtar kelime 5'ten fazla olmamalidir.
        /// </summary>
        /// <param name="paragraf">Word dokümanının paragraf olarak eklendiği listenin gönderilmesi gerekmektedir.</param>
        /// <returns></returns>
        public string AnahtarKelimeAra(List<string> paragraf)
        {
            try
            {
                int index = paragraf.FindIndex(a => a.Contains("Anahtar Kelimeler:"));

                string[] arrays = paragraf[index].Split(',');
                if (arrays.Length < 6)
                {
                    return "Anahtar kelime kullanımı doğru hazırlanmış";
                }
                else
                {
                    //return $"Anahtar kelime kullanımı doğru hazırlanmış. {arrays.Length} adet kullanım hatalıdır.";
                    return "Anahtar kelime kullanımı doğru hazırlanmamış";
                }
            }
            catch (Exception)
            {

                return "Anahtar kelime kontrol edilirken bir hata ile karşılaşıldı.";
            }


        }


        /// <summary>
        /// Calculates the numbers of keywords prepared in English. The rule keyword should not be more than 5.
        /// </summary>
        /// <param name="paragraf">Word dokümanının paragraf olarak eklendiği listenin gönderilmesi gerekmektedir.</param>
        /// <returns></returns>
        public string KeywordsCheck(List<string> paragraf)
        {
            try
            {
                int index = paragraf.FindIndex(a => a.Contains("Keywords:"));

                string[] arrays = paragraf[index].Split(',');
                if (arrays.Length < 6)
                {
                    return "Keywords kullanımı doğru hazırlanmış.";
                }
                else
                {
                    //return $"Keywords kullanımı doğru hazırlanmış. {arrays.Length} adet kullanım hatalıdır.";
                    return "Keywords kullanımı doğru hazırlanmamış.";
                }
            }
            catch (Exception)
            {

                return "Keywords kullanımı kontrol edilirken bir hata ile karşılaşıldı.";
            }

        }


        /// <summary>
        /// Belirlenen karakteri dokuman icerisinde arar. Ornegin [ ifadesi kac kere kullanildiysa ] ifadesi de ayni sayida kullanilmalidir.
        /// </summary>
        /// <param name="metin">Dokümanın listeye eklenmiş her verisinin tek tek kontrol edilmesi için string olarak gönderilmesi gerekmektedir.</param>
        /// <param name="chr1">Kontrol edilmesi istenen karakterin girilmesi istenmektedir. Ex: '(', '{', '[' </param>
        /// <param name="chr2">Kontrol edilmesi istenen karakterin girilmesi istenmektedir. Ex: ')', '}', ']' </param>
        /// <returns></returns>
        public static string karakterAra(string metin, char chr1, char chr2)
        {


            try
            {
                int r1 = metin.ToCharArray().Count(c => c == chr1);
                int r6 = metin.ToCharArray().Count(c => c == chr2);

                if (r1 == r6)
                {
                    return null;
                }
                else if (r1 > r6)
                {
                    //return $"{metin} de {chr2} karakteri eşit sayıda kullanılmamış";
                    return metin + "Paragrafta " + chr2.ToString() + " karakteri eşit sayıda kullanılmamış";
                }
                else
                {
                    //return $"{metin} de {chr1} karakteri eşit sayıda kullanılmamış";
                    return metin + "Paragrafta " + chr1.ToString() + " karakteri eşit sayıda kullanılmamış";
                }

            }
            catch (Exception)
            {

                return "Karakter kontrolü yapılırken hata ile karşılaşıldı"; ;
            }

        }


        public string onsozKontrol(List<string> data)
        {
            try
            {
                //sonuç değerinin tutulacağı değişken
                string donut = null;

                //paragraf indisinin tutulacağı değişken
                int sayac = 0;

                //gelen paragrafların dolaşılması için foreach döngüsü
                foreach (var item in data)
                {
                    //ÖNSÖZ başlığının bulunması
                    if (item == "ÖNSÖZ")
                    {
                        //önsöz başlığında bir sonraki paragrafla iş yapılacağından bir
                        //sonraki paragrafın alınarak boşluklara göre bölümlenmesi
                        string[] dizi = data[sayac + 1].Split(' ');

                        //dizinin tüm elemanlarının dolaşılması
                        for (int i = 0; i < dizi.Length; i++)
                        {
                            //teşekkür ibaresi geçmesi durumunda yapılacak işlem
                            if (dizi[i] == "Teşekkür" || dizi[i] == "teşekkür")
                            {
                                donut = "ilk paragrafta teşekkür ibaresi vardır...";
                                break;
                            }

                            //else durumda yapılacak işlem
                            else
                            {
                                donut = "ilk paragrafta teşekkür ibaresi yoktur...";
                            }

                        }
                    }

                    //ÖNSÖZ başlığının bulunmasının ardından bir sonraki paragrafa gidilmesi için mevcut indisin belirlenmesi
                    else
                    {
                        sayac++;
                    }
                }

                //sonuç değerinin döndürülmesi
                return donut;
            }
            catch (Exception)
            {

                throw;
            }

        }

        //----------------------------------------------------

        public string tirnakKelime(List<string> paragraf)
        {
            try
            {
                //split(bölme) işleminin yapılacağı tırnak işaretlerinin char dizisine atanması 
                char[] karakterler = { '“', '”', '"' };

                //geri dönecek değerin tutulacağı değişken
                string deger = null;

                //gelen paragrafların dolaşılması için foreach döngüsü
                foreach (var item in paragraf)
                {
                    //paragrafın belirtilen karakterlere göre bölümlenmesi
                    string[] b = item.Split(karakterler);

                    //gelen bölüm değerlerine göre koşulların belirlenmesi 
                    //eğer uzunluk 1 ise karakterlerden herhangi biri yok demektir 
                    if (b.Length == 1)
                    {
                        deger = null;
                    }

                    //eğer uzunluk 3 ün altında ise tırnak işareti eksik demektir. yani tırnak işareti açılmış ama kapanmamıştır.
                    else if (b.Length < 3)
                    {
                        deger = "Tırnak işareti eksik...";
                    }

                    //eğer uzunluk 3 e eşit veya 4 ten küçük ise paragrafta çif tınak içerisinde 1 ifade var demektir.
                    else if (b.Length >= 3 && b.Length < 4)
                    {
                        //gelen veri dizisinin 1. indisteki değeri tırnak içini ifade etmektedir.
                        string[] c = b[1].Split(' ');

                        //koşullara göre gerçekleştirilecek işlemler mevcuttur.
                        if (c.Length >= 50)
                            deger = "Tırnak içinde " + c.Length + " kadar kelime vardır...(kural ihlali)";

                        else
                            deger = "Tırnak içinde " + c.Length + " kadar kelime vardır.(kural ihlali yoktur...)";
                    }

                    //eğer uzunluk 4 ise tırnak işareti eksik demektir. yani ifadede 3 tırnak işareti var demektir. olaması gereken 4 tür.
                    else if (b.Length == 4) deger = "Tırnak işareti eksik...";

                    //eğer uzunluk 5 ise problem yok demektir. 
                    else if (b.Length > 4 && b.Length <= 5)
                    {
                        //gelen veri dizisinin 1. indisteki değeri tırnak içini ifade etmektedir.
                        string[] c1 = b[1].Split(' ');

                        //gelen veri dizisinin 3. indisteki değeri tırnak içini ifade etmektedir.
                        string[] c = b[3].Split(' ');

                        //koşullara göre gerçekleştirilecek işlemler mevcuttur.
                        if (c.Length >= 50)
                            deger = "Tırnak içinde " + c.Length + " kadar kelime vardır...(kural ihlali)";

                        else if (c1.Length >= 50)
                            deger = "Tırnak içinde " + c1.Length + " kadar kelime vardır...(kural ihlali)";

                        else
                            deger = "Tırnak içinde " + c.Length + " kadar kelime vardır.(kural ihlali yoktur...)";
                    }
                    //diğer durumlar else olarak ifade edilmiştir.
                    else
                    {
                        deger = "Tırnak işareti eksik...";
                    }

                    //geri dönütlerin tutulduğu listbox 
                }
                return deger;
            }
            catch (Exception)
            {

                return "Beklenmeyen bir durum ile karşılaşıldı...";
            }
           
        }

        //----------------------------------------------------

        public string kaynakAtıf(List<String> data)
        {
            try
            {
                var liste2 = data.Where(x => x.Contains("].")).ToList();

                foreach (var item in liste2)
                {

                    string p = item.Substring(item.LastIndexOf('['), (item.LastIndexOf(']') - item.LastIndexOf('[')));

                    char[] dizi = { ',', '-' };

                    string[] dizi1 = p.Split(dizi);

                    if (dizi1.Length > 2)
                    {
                        return "Hata kaynak atıfta hata vardır.";
                    }

                    else
                    {
                        return "Kaynak atıfda hata yoktur.";
                    }
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }


        // Rapor dökümanı indir
        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment; filename=testfile.txt");
            Response.AddHeader("content-type", "text/plain");

            using (StreamWriter writer = new StreamWriter(Response.OutputStream))
            {
                foreach (var item in ListBox1.Items)
                {
                    writer.WriteLine(item.ToString());
                }

            }
            Response.End();
        }

        /// <summary>
        /// Tez içerisinde tırnak ifadesi iki ve ikinin katları şeklinde kullanılmalıdır. Bu da kullanımda hataya sebep olmaktadır. Bunun kontrolü amaçlanmaktadır.
        /// </summary>
        /// <param name="metin">String veri türündeki metni ifade eder.</param>
        /// <param name="chr">Tek tırnak haricinde kontrol edilmek istenen ifadeler içinde kullanılabilwecektir. Ex: '"' şeklinde kullanılmalıdır.</param>
        /// <returns></returns>
        public string tirnakKontrolu(string metin, char chr)
        {
            try
            {
                int r1 = metin.ToCharArray().Count(c => c == chr);

                if (r1 % 2 == 0)
                {
                    return null;

                }
                else
                {
                    return $"{metin} ifadesinde tırnak işareti hatalı kullanılmış.";

                }

            }
            catch (Exception)
            {
                return "Tırnak kontrolü yapılırken bir hata ile karşılaşıldı.";
            }

        }
    }
}