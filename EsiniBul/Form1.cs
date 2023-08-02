using EsiniBul.Properties;

namespace EsiniBul
{
    public partial class Form1 : Form
    {
        int boyut = 4;
        int w = 128;
        List<int> resimler;
        List<PictureBox> aciklar;
        int gizlenenAdet;
        public Form1()
        {
            InitializeComponent();
            // int x = 2;
            //panel1.BackgroundImage = (Bitmap)Resources.ResourceManager.GetObject( x.ToString());
            YeniOyunHazirla();
        }

        private void ResimleriBelirle()
        {
            int toplamKartAdet = boyut * boyut;
            int resimCesitAdet = toplamKartAdet / 2;

            for (int i = 0; i < 2; i++)
            {
                for (int j = 1; j <= resimCesitAdet; j++)
                {
                    resimler.Add(j);
                }
            }
            ListeKaristir(resimler);//shuffle algorithm
        }

        private void ListeKaristir(List<int> resimler)
        {
            Random rnd = new Random();
            int temp, talihliIndeks;

            for (int i = 0; i < resimler.Count - 1; i++)
            {
                talihliIndeks = rnd.Next(i, resimler.Count);
                temp = resimler[i];
                resimler[i] = resimler[talihliIndeks];
                resimler[talihliIndeks] = temp;

            }
        }

        private void KartlariOlustur()
        {
            int no = 0;
            for (int i = 0; i < boyut; i++)
            {
                for (int j = 0; j < boyut; j++)
                {
                    PictureBox pb = new PictureBox();
                    pb.Size = new Size(w, w);
                    pb.Location = new Point(j * w, i * w);
                    pb.SizeMode = PictureBoxSizeMode.Zoom;
                    pb.Image = Resim(0);
                    pb.Tag = no;
                    pb.Click += Pb_Click;
                    pnlKartlar.Controls.Add(pb);
                    no++;
                }
            }
            pnlKartlar.Size = new Size(boyut * w, boyut * w);
            ClientSize = new Size(boyut * w + 24, boyut * w + 24);
        }

        //hangi pb t�kland�ysa senderda o vard�r
        private void Pb_Click(object? sender, EventArgs e)//Event metodu
        {
            PictureBox tiklanan = (PictureBox)sender;
            int tiklananNo = (int)tiklanan.Tag;

            //E�er t�klanan kart zaten a��ksa hi�bir�ey yapmadan ��k
            if (aciklar.Contains(tiklanan))
            {
                return;
            }

            //MessageBox.Show(tiklananNo + ". karta t�kland�");

            //E�er zaten a��k 2 kart varsa �nce onlar� kapat

            if (aciklar.Count == 2)
            {
                AciklariKapat();
            }
            aciklar.Add(tiklanan);



            //E�er a��k kart say�s� 2 ise ve bunlar ayn�ysa onlar� gizle
            int oncekiNo = (int)aciklar[0].Tag;
            tiklanan.Image = Resim(resimler[tiklananNo]);
            
            if (aciklar.Count == 2 && resimler[oncekiNo]== resimler[tiklananNo])
            {
                //Gizlenmeden �nce yar�m saniye g�rme �ans� ver
                Update();
                Thread.Sleep(500);
                AciklariGizle();
                
            }

            //E�er t�m kartlar gizlendiyse oyunu bitir.
            if (gizlenenAdet == resimler.Count)
            {
                DialogResult cevap = MessageBox.Show("Oyun bitti! Tekrar oynamak ister misiniz?", "Tekrar Oyna?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                
                if (cevap == DialogResult.Yes)
                {
                    YeniOyunHazirla();
                }
                else 
                {
                    Close();//Pencereyi kapat
                }

            }
            
        }

        private void YeniOyunHazirla()
        {
            pnlKartlar.Controls.Clear();
            resimler = new List<int> ();
            aciklar = new List<PictureBox>();
            gizlenenAdet = 0;
            ResimleriBelirle();
            KartlariOlustur();
                
        }

        private void AciklariGizle()
        {
            gizlenenAdet += 2;
            foreach (PictureBox pb in aciklar)
            { 
                pb.Hide();
            }
            
            aciklar.Clear();
        }

        private void AciklariKapat()
        {
            foreach (PictureBox pb in aciklar)
            {
                pb.Image = Resim(0);
            }
            aciklar.Clear();
        }

        Bitmap Resim(int no) // bu metot belirtilen numaradaki resmi getirir
        {
            return (Bitmap)Resources.ResourceManager.GetObject(no.ToString());
        }
    }
}