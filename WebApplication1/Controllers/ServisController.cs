using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers
{
    public class ServisController : ApiController
    {
        DB01Entities db = new DB01Entities();
        SonucModel sonuc = new SonucModel();

        #region Kategoriler

        [HttpGet]
        [Route("api/kategorilistele")]
        public List<KategorilerModel> KategorilerListe()
        {
            List<KategorilerModel> liste = db.Kategoriler.Select(x => new KategorilerModel()
            {
                Katid = x.Katid,
                KatAd = x.KatAd,
              
            }).ToList();
            return liste;
        }
        [HttpGet]
        [Route("api/kategoribyid/{Katid}")]
        public KategorilerModel KategoriById(int Katid)
        {
            KategorilerModel kayit = db.Kategoriler.Where(s => s.Katid == Katid).Select(x => new KategorilerModel()
            {
                Katid = x.Katid,
                KatAd = x.KatAd,
                KatDersSayi = x.Dersler.Count()

            }).FirstOrDefault();

            return kayit;
        }
        [HttpPost]
        [Route("api/kategoriekle")]
        public SonucModel KategoriEkle(KategorilerModel model)
        {
            if (db.Kategoriler.Count(s => s.KatAd == model.KatAd) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen Kategori Adı Kayıtlıdır!";
                return sonuc;
            }
            Kategoriler yeni = new Kategoriler();
            yeni.KatAd = model.KatAd;
            db.Kategoriler.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kategori Eklendi";
            return sonuc;
        }
        [HttpPut]
        [Route("api/kategoriduzenle")]
        public SonucModel KategoriDuzenle(KategorilerModel model)
        {
            Kategoriler kayit = db.Kategoriler.Where(s => s.Katid == model.Katid).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kategori Bulunamadı!";
                return sonuc;
            }
            kayit.KatAd = model.KatAd;
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Kategori Düzenlendi";
            return sonuc;
        }
        [HttpDelete]
        [Route("api/kategorisil/{Katid}")]

        public SonucModel KategoriSil(int Katid)
        {
            Kategoriler Kayit = db.Kategoriler.Where(s => s.Katid == Katid).FirstOrDefault();
            if (Kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kategori Bulunamadı!";
                return sonuc;
            }

            if (db.Dersler.Count(s => s.dKatid == Katid) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Üzerinde Anket Bulunan Kategori Silinemez";
                return sonuc;
            }
            db.Kategoriler.Remove(Kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kategori Silindi";
            return sonuc;
        }
        #endregion

        #region Dersler


        [HttpGet]
        [Route("api/dersliste")]
        public List<DerslerModel> DersListe()
        {
            List<DerslerModel> liste = db.Dersler.Select(x => new DerslerModel()
            {
                Dersid = x.Dersid,
                DersAd = x.DersAd,
                Dersicerik = x.Dersicerik,
                DersDurum = x.DersDurum,
                dKatid = x.Kategoriler.Katid,
                dUyeid = x.dUyeid,
                dyuklenmetarihi = x.dyuklenmetarihi



            }).ToList();
            return liste;
        }


        [HttpGet]
        [Route("api/derslistebykatid/{Katid}")]
        public List<DerslerModel> DersListeleByKatid(int Katid)
        {
            List<DerslerModel> liste = db.Dersler.Where(s => s.dKatid == Katid).Select(x =>
           new DerslerModel()
           {
               Dersid = x.Dersid,
               DersAd = x.DersAd,
               Dersicerik = x.Dersicerik,
               DersDurum = x.DersDurum,
               dKatid = x.dKatid,
               dyuklenmetarihi = x.dyuklenmetarihi
           }).ToList();
            return liste;
        }







        [HttpGet]
        [Route("api/dersbyid/{Dersid}")]
        public DerslerModel DersById(int Dersid)
        {
            DerslerModel kayit = db.Dersler.Where(s => s.Dersid == Dersid).Select(x => new DerslerModel()
            {
                Dersid = x.Dersid,
                DersAd = x.DersAd,
                Dersicerik = x.Dersicerik,
                DersDurum = x.DersDurum,
                dKatid = x.Kategoriler.Katid,
                dUyeid = x.dUyeid,
                dyuklenmetarihi = x.dyuklenmetarihi
            }).FirstOrDefault();
            return kayit;
        }


        [HttpPost]
        [Route("api/dersekle")]
        public SonucModel DersEkle(DerslerModel model)
        {
            if (db.Dersler.Count(s => s.DersAd == model.DersAd && s.dKatid == model.dKatid) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen Ders İlgili Kategoride Kayıtlıdır!";
                return sonuc;
            }
            Dersler yeni = new Dersler();
            yeni.DersAd = model.DersAd;
            yeni.Dersicerik = model.Dersicerik;
            yeni.DersDurum = model.DersDurum;
            yeni.dKatid = model.dKatid;
            yeni.dUyeid = model.dUyeid;
            yeni.dyuklenmetarihi = model.dyuklenmetarihi;
          
            db.Dersler.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Ders Eklendi";

            return sonuc;
        }



        [HttpPut]
        [Route("api/dersduzenle")]
        public SonucModel DersDuzenle(DerslerModel model)
        {
            Dersler kayit = db.Dersler.Where(s => s.Dersid == model.Dersid).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }
            kayit.DersAd = model.DersAd;
            kayit.Dersicerik = model.Dersicerik;
            kayit.DersDurum = model.DersDurum;
            kayit.dKatid = model.dKatid;
            kayit.dUyeid = model.dUyeid;
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kayıt Düzenlendi";
            return sonuc;
        }



        [HttpDelete]
        [Route("api/derssil/{Dersid}")]
        public SonucModel DersSil(int Dersid)
        {
            Dersler kayit = db.Dersler.Where(s => s.Dersid == Dersid).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }
            db.Dersler.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Ders Silindi";
            return sonuc;
        }

        #endregion


        #region Egitimler
        [HttpGet]
        [Route("api/egitimliste")]
        public List<EgitimlerModel> EğitimListe()
        {
            List<EgitimlerModel> liste = db.Egitimler.Select(x => new EgitimlerModel()
            {
                Egitimid = x.Egitimid,
                Egitimicerik = x.Egitimicerik,
                eUyeid = x.eUyeid,
                eDersid = x.eDersid
            }).ToList();
            return liste;
        }


        [HttpGet]
        [Route("api/egitimlistebyuyeid/{Uyeid}")]
        public List<EgitimlerModel> EgitimListeByUyeId(int Uyeid)
        {
            List<EgitimlerModel> liste = db.Egitimler.Where(s => s.eUyeid == Uyeid).Select(x => new EgitimlerModel()
         {
                Egitimid = x.Egitimid,
                Egitimicerik = x.Egitimicerik,
                eUyeid = x.eUyeid,
                eDersid = x.eDersid
                }).ToList();
                return liste;
        }


        [HttpGet]
        [Route("api/egitimlistebyegitim/{Egitimid}")]
        public List<EgitimlerModel> EgitimListeBymakaleId(int Egitimid)
        {
            List<EgitimlerModel> liste = db.Egitimler.Where(s => s.Egitimid == Egitimid).Select(x => new EgitimlerModel()
            {
                Egitimid = x.Egitimid,
                Egitimicerik = x.Egitimicerik,
                eUyeid = x.eUyeid,
                eDersid = x.eDersid
            }).ToList();
            return liste;
        }



        [HttpGet]
        [Route("api/egitimbyid/{Egitimid}")]
        public EgitimlerModel EgitimById(int Egitimid)
        {
            EgitimlerModel kayit = db.Egitimler.Where(s => s.Egitimid == Egitimid).Select(x => new EgitimlerModel()
            {
                Egitimid = x.Egitimid,
                Egitimicerik = x.Egitimicerik,
                eUyeid = x.eUyeid,
                eDersid = x.eDersid
            }).SingleOrDefault();
            return kayit;
        }

        [HttpPost]
        [Route("api/egitimekle")]
        public SonucModel EgitimEkle(EgitimlerModel model)
        {
            if (db.Egitimler.Count(s => s.Egitimicerik == model.Egitimicerik && s.eDersid == model.eDersid) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen Eğitim  Kayıtlıdır!";
                return sonuc;
            }
            Egitimler yeni = new Egitimler();
            yeni.Egitimicerik = model.Egitimicerik;
            yeni.eUyeid = model.eUyeid;
            yeni.eDersid = model.eDersid;
            db.Egitimler.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Eğitim Eklendi";

            return sonuc;
        }

        [HttpPut]
        [Route("api/egitimduzenle")]
        public SonucModel EgitimDuzenle(EgitimlerModel model)
        {
            Egitimler kayit = db.Egitimler.Where(s => s.Egitimid == model.Egitimid).SingleOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }
            kayit.Egitimid = model.Egitimid;
            kayit.Egitimicerik = model.Egitimicerik;
            kayit.eUyeid = model.eUyeid;
            kayit.eDersid = model.eDersid;
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Egitim Düzenlendi";
            return sonuc;
        }



        [HttpDelete]
        [Route("api/egitimsil/{Egitimid}")]
        public SonucModel EgitimSil(int Egitimid)
        {
            Egitimler kayit = db.Egitimler.Where(s => s.Egitimid == Egitimid).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }
            db.Egitimler.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Ders Silindi";
            return sonuc;
        }
        #endregion

        #region Uyeler
        [HttpGet]
        [Route("api/uyelistele")]

        public List<UyelerModel> UyeListe()
        {
            List<UyelerModel> liste = db.Uyeler.Select(x => new UyelerModel()
            {
                Uyeid = x.Uyeid,
                UyeAdSoyad = x.UyeAdSoyad,
                UyeMail = x.UyeMail,
                UyeYas = x.UyeYas,
                UyeParola = x.UyeParola,
                UyeTarih = x.UyeTarih,
                UyeYetki = x.UyeYetki,


            }).ToList();
            return liste;
        }


        [HttpGet]
        [Route("api/uyebyid/{uyeId}")]
        public UyelerModel UyeById(int Uyeid)
        {
            UyelerModel kayit = db.Uyeler.Where(s => s.Uyeid == Uyeid).Select(x => new UyelerModel()
            {
                Uyeid = x.Uyeid,
                UyeAdSoyad = x.UyeAdSoyad,
                UyeMail = x.UyeMail,
                UyeYas = x.UyeYas,
                UyeParola = x.UyeParola,
                UyeTarih = x.UyeTarih,
                UyeYetki = x.UyeYetki
            }).SingleOrDefault();
            return kayit;
        }



        [HttpPost]
        [Route("api/uyeekle")]
        public SonucModel UyeEkle(UyelerModel model)
        {
            if (db.Uyeler.Count(s => s.UyeMail == model.UyeMail) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "E-Posta Adresi Zaten Kayıtlıdır!";
                return sonuc;
            }
            Uyeler yeni = new Uyeler();
            yeni.UyeAdSoyad = model.UyeAdSoyad;
            yeni.UyeMail = model.UyeMail;
            yeni.UyeYas = model.UyeYas;
            yeni.UyeParola = model.UyeParola;
            yeni.UyeTarih = model.UyeTarih;
            yeni.UyeYetki = model.UyeYetki;
            db.Uyeler.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Üye Eklendi";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/uyesil/{Uyeid}")]
        public SonucModel UyeSil(int Uyeid)
        {
            Uyeler kayit = db.Uyeler.Where(s => s.Uyeid == Uyeid).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Üye Bulunamadı!";
                return sonuc;
            }

            db.Uyeler.Remove(kayit);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Üye Silindi";
            return sonuc;
        }
        #endregion
    }
}
