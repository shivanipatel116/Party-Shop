using myPartyShop.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace myPartyShop.Controllers
{
    public class HomeController : Controller
    {
        private PartyShopEntities db = new PartyShopEntities();

        // GET: Home
        public ActionResult Index(string partyThemeType, string searchString)
        {         
            //Theme wise search
            var ThemeLst = new List<string>();
            var ThemeQry = from p in db.partyTables
                           orderby p.partyTheme
                           select p.partyTheme;

            ThemeLst.AddRange(ThemeQry.Distinct());
            ViewBag.partyThemeType = new SelectList(ThemeLst);

            //title search
            var partyType = from p in db.partyTables
                            select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                partyType = partyType.Where(p => p.partyDecorationName.Contains(searchString));
            }

            //last bit of theme search
            if (!String.IsNullOrEmpty(partyThemeType))
            {
                partyType = partyType.Where(p => p.partyTheme == partyThemeType);
            }

            return View(partyType);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            partyTable party = db.partyTables.Find(id);

            if (party == null)
            {
                return HttpNotFound();
            }
            return View(party);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "Id,partyTheme,partyDecorationName,partyDecorationImage,partyDecorationDescription,partyDecorationPrice")] partyTable party)
        {
            if (ModelState.IsValid)
            {
                db.partyTables.Add(party);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(party);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            partyTable party = db.partyTables.Find(id);

            if (party == null)
            {
                return HttpNotFound();
            }
            return View(party);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,partyTheme,partyDecorationName,partyDecorationImage,partyDecorationDescription,partyDecorationPrice")] partyTable party)
        {
            if (ModelState.IsValid)
            {
                db.Entry(party).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(party);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            partyTable party = db.partyTables.Find(id);

            if (party == null)
            {
                return HttpNotFound();
            }
            return View(party);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            partyTable party = db.partyTables.Find(id);
            db.partyTables.Remove(party);
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
    }
}