using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlgebraApp1._1.Models
{
    public class PredbiljezbeSeminariModel
    {
        public AlgebraAppDBEntities _db = new AlgebraAppDBEntities();

        public List<Predbiljezba> PredbiljezbaModel()
        {
            var predbiljezba = _db.Predbiljezbas.ToList();

            return predbiljezba;
        }

        public List<Seminar> SeminarModel()
        {
            var seminar = _db.Seminars.ToList();

            return seminar;
        }

    }
}