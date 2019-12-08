using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using CLUB.Models;

namespace CLUB.Controllers
{
    public class SOCIOSController : Controller
    {
        private CLUBEntities db = new CLUBEntities();

        // GET: SOCIOS


        public ActionResult Login()
        {
            return View();
        }


        public ActionResult Inicio()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View(db.SOCIOS.ToList());
        }

        // GET: SOCIOS/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SOCIOS sOCIOS = db.SOCIOS.Find(id);
            if (sOCIOS == null)
            {
                return HttpNotFound();
            }
            return View(sOCIOS);
        }

        // GET: SOCIOS/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SOCIOS/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NOMBRE,APELLIDOS,CEDULA,FOTO,DIRECCION,TELEFONO,SEXO,EDAD,FECHADENAC,AFILIADOS,DATOSMEMBRESIA,LUGARDETRABAJO,DIRECCIONOFICINA,TELOFOCINA,ESTADODEMEMBRESIA,FECHAINGRESO,FECHASALIDA")] SOCIOS sOCIOS)
        {
            HttpPostedFileBase FileBase = Request.Files[0];

            if(FileBase.ContentLength == 0)
            {
                ModelState.AddModelError("FOTO", "Es necesario seleccionar una imagen.");
            }
            else
            {
                if (FileBase.FileName.EndsWith(".jpg"))
                {
                    WebImage image = new WebImage(FileBase.InputStream);
                    sOCIOS.FOTO = image.GetBytes();
                }

                else { ModelState.AddModelError("FOTO", "El sistema solo acepta imagenes con formato JPG. "); }
              
            }

            if (ModelState.IsValid)
            {
                db.SOCIOS.Add(sOCIOS);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sOCIOS);
        }

        // GET: SOCIOS/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SOCIOS sOCIOS = db.SOCIOS.Find(id);
            if (sOCIOS == null)
            {
                return HttpNotFound();
            }
            return View(sOCIOS);
        }

        // POST: SOCIOS/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NOMBRE,APELLIDOS,CEDULA,FOTO,DIRECCION,TELEFONO,SEXO,EDAD,FECHADENAC,AFILIADOS,DATOSMEMBRESIA,LUGARDETRABAJO,DIRECCIONOFICINA,TELOFOCINA,ESTADODEMEMBRESIA,FECHAINGRESO,FECHASALIDA")] SOCIOS sOCIOS)
        {
            byte[] ImagenActual = null;
            HttpPostedFileBase FileBase = Request.Files[0];

            if (FileBase.ContentLength == 0)
            {
                ImagenActual = db.SOCIOS.SingleOrDefault(t=>t.CEDULA==sOCIOS.CEDULA).FOTO;
            }
            else
            {
                if (FileBase.FileName.EndsWith(".jpg"))
                {
                    WebImage image = new WebImage(FileBase.InputStream);
                    sOCIOS.FOTO = image.GetBytes();
                }

                else { ModelState.AddModelError("FOTO", "El sistema solo acepta imagenes con formato JPG. "); }

            }

            if (ModelState.IsValid)
            {
                db.Entry(sOCIOS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sOCIOS);
        }

        // GET: SOCIOS/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SOCIOS sOCIOS = db.SOCIOS.Find(id);
            if (sOCIOS == null)
            {
                return HttpNotFound();
            }
            return View(sOCIOS);
        }

        // POST: SOCIOS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            SOCIOS sOCIOS = db.SOCIOS.Find(id);
            db.SOCIOS.Remove(sOCIOS);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult getImage(String id)
        {
            SOCIOS img = db.SOCIOS.Find(id);
            byte[] byteImage = img.FOTO;

            MemoryStream memoryStream = new MemoryStream(byteImage);
            Image image = Image.FromStream(memoryStream);

            memoryStream = new MemoryStream();
            image.Save(memoryStream, ImageFormat.Jpeg);
            memoryStream.Position = 0;

            return File(memoryStream,"image/jpg");
        }

    
    }
}
