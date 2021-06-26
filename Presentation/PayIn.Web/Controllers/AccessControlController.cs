using System.Collections.Generic;
using System.Web.Mvc;

namespace PayIn.Web.Controllers
{
    public class AccessControlController : Controller
    {
        public partial class BeachDto
        {
            public string Name { get; set; }
            public string Capacity { get; set; }
            public string Zone { get; set; }
            public string URL { get; set; }

            public BeachDto(string name, string capacity, string zone, string url)
            {
                Name = name;
                Capacity = capacity;
                Zone = zone;
                URL = url;
            }
        };

        #region /

        public ActionResult Index()
        {
            return PartialView();
        }

        #endregion

        #region /Create

        public ActionResult Create()
        {
            return PartialView();
        }

        #endregion

        #region /Update

        public ActionResult Update()
        {
            return PartialView();
        }

        #endregion

        #region /Delete

        public ActionResult Delete()
        {
            return PartialView();
        }

        #endregion

        #region /Public

        [AllowAnonymous]
        public ActionResult Public()
        {
            ViewBag.Public = true;
            ViewBag.Beaches = new List<BeachDto>()
            {
                new BeachDto("Platja del Riu de la Sènia", "27", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.1"),
                new BeachDto("Cala de la Sunyera", "Cerrada", "Costa norte", ""),
                new BeachDto("Cala del Sòl del Riu", "29", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.14"),
                new BeachDto("Platja de les Deveses", "229", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.2"),
                new BeachDto("Cala de les Timbes", "92", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.9"),
                new BeachDto("Cala de les Llanetes", "34", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.15"),
                new BeachDto("Cala de la Roca Plana", "45", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.22"),
                new BeachDto("Cala de la Foradada", "73", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.21"),
                new BeachDto("Platja de les Cales", "13", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.3"),
                new BeachDto("Cala del Pastor", "68", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.10"),
                new BeachDto("Platja del Triador", "154", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.16"),
                new BeachDto("Cala del Pinar", "95", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.4"),
                new BeachDto("Platja de la Barbiguera", "104", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.11"),
                new BeachDto("Cala Saldonar", "20", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.17"),
                new BeachDto("Platja del Saldonar", "78", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.5"),
                new BeachDto("Cala dels Cossis", "123", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.8"),
                new BeachDto("Cala dels Pinets", "28", "Costa sur", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.18"),
                new BeachDto("Cala del Fondo de Bola", "43", "Costa sur", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.6"),
                new BeachDto("Cala de les Roques", "3", "Costa sur", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.12"),
                new BeachDto("Platja de les Salines", "72", "Costa sur", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.19"),
                new BeachDto("Cala del Puntal I", "94", "Costa sur", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.7"),
                new BeachDto("Cala del Puntal II", "144", "Costa sur", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.13"),
                new BeachDto("Platja d’Aiguadoliva", "186", "Costa sur", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.20"),
            };

            return View();
        }

        #endregion

        #region /Mupi

        [AllowAnonymous]
        public ActionResult Mupi()
        {
            ViewBag.Beaches = new List<BeachDto>()
            {
                new BeachDto("Platja del Riu de la Sènia", "27", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.1"),
                new BeachDto("Cala de la Sunyera", "Cerrada", "Costa norte", ""),
                new BeachDto("Cala del Sòl del Riu", "29", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.14"),
                new BeachDto("Platja de les Deveses", "229", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.2"),
                new BeachDto("Cala de les Timbes", "92", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.9"),
                new BeachDto("Cala de les Llanetes", "34", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.15"),
                new BeachDto("Cala de la Roca Plana", "45", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.22"),
                new BeachDto("Cala de la Foradada", "73", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.21"),
                new BeachDto("Platja de les Cales", "13", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.3"),
                new BeachDto("Cala del Pastor", "68", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.10"),
                new BeachDto("Platja del Triador", "154", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.16"),
                new BeachDto("Cala del Pinar", "95", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.4"),
                new BeachDto("Platja de la Barbiguera", "104", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.11"),
                new BeachDto("Cala Saldonar", "20", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.17"),
                new BeachDto("Platja del Saldonar", "78", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.5"),
                new BeachDto("Cala dels Cossis", "123", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.8"),
                new BeachDto("Cala dels Pinets", "28", "Costa sur", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.18"),
                new BeachDto("Cala del Fondo de Bola", "43", "Costa sur", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.6"),
                new BeachDto("Cala de les Roques", "3", "Costa sur", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.12"),
                new BeachDto("Platja de les Salines", "72", "Costa sur", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.19"),
                new BeachDto("Cala del Puntal I", "94", "Costa sur", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.7"),
                new BeachDto("Cala del Puntal II", "144", "Costa sur", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.13"),
                new BeachDto("Platja d’Aiguadoliva", "186", "Costa sur", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.20"),
            };

            return View();
        }

        #endregion

        #region /Places

        [AllowAnonymous]
        public ActionResult Places()
        {
            ViewBag.Public = false;
            ViewBag.Beaches = new List<BeachDto>()
            {
                new BeachDto("Platja del Riu de la Sènia", "27", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.1"),
                new BeachDto("Cala de la Sunyera", "Cerrada", "Costa norte", ""),
                new BeachDto("Cala del Sòl del Riu", "29", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.14"),
                new BeachDto("Platja de les Deveses", "229", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.2"),
                new BeachDto("Cala de les Timbes", "92", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.9"),
                new BeachDto("Cala de les Llanetes", "34", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.15"),
                new BeachDto("Cala de la Roca Plana", "45", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.22"),
                new BeachDto("Cala de la Foradada", "73", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.21"),
                new BeachDto("Platja de les Cales", "13", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.3"),
                new BeachDto("Cala del Pastor", "68", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.10"),
                new BeachDto("Platja del Triador", "154", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.16"),
                new BeachDto("Cala del Pinar", "95", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.4"),
                new BeachDto("Platja de la Barbiguera", "104", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.11"),
                new BeachDto("Cala Saldonar", "20", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.17"),
                new BeachDto("Platja del Saldonar", "78", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.5"),
                new BeachDto("Cala dels Cossis", "123", "Costa norte", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.8"),
                new BeachDto("Cala dels Pinets", "28", "Costa sur", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.18"),
                new BeachDto("Cala del Fondo de Bola", "43", "Costa sur", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.6"),
                new BeachDto("Cala de les Roques", "3", "Costa sur", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.12"),
                new BeachDto("Platja de les Salines", "72", "Costa sur", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.19"),
                new BeachDto("Cala del Puntal I", "94", "Costa sur", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.7"),
                new BeachDto("Cala del Puntal II", "144", "Costa sur", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.13"),
                new BeachDto("Platja d’Aiguadoliva", "186", "Costa sur", "https://geoportal.vinaros.es/visor#/mapa/platjes?elemento=platjes_cales.20"),
            };

            return PartialView();
        }

        #endregion

        #region /Entry

        public ActionResult Entry()
        {
            return PartialView();
        }

        #endregion

        #region /Reset

        public ActionResult Reset()
        {
            return PartialView();
        }

        #endregion

        #region /Weather

        [AllowAnonymous]
        public ActionResult Weather()
        {
            return PartialView();
        }

        #endregion

        #region /Info

        [AllowAnonymous]
        public ActionResult Info()
        {
            return PartialView();
        }

        #endregion

        #region /Language

        [AllowAnonymous]
        public ActionResult Language()
        {
            return PartialView();
        }

        #endregion
    }
}