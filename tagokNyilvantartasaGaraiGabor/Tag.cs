using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tagokNyilvantartasaGaraiGabor
{
    internal class Tag
    {
        private int azon;
        private string nev;
        private int szulev;
        private int irszam;
        private string orsz;

        public Tag(int azon, string nev, int szulev, int irszam, string orsz)
        {
            this.azon = azon;
            this.nev = nev;
            this.szulev = szulev;
            this.irszam = irszam;
            this.orsz = orsz;
        }

        public int Azon { get => azon; set => azon = value; }
        public string Nev { get => nev; set => nev = value; }
        public int Szulev { get => szulev; set => szulev = value; }
        public int Irszam { get => irszam; set => irszam = value; }
        public string Orsz { get => orsz; set => orsz = value; }

        public override string ToString()
        {

            return  $"Azonosító: {Azon}\n" +
                    $"Név: {Nev}\n" +
                    $"Születési év: {Szulev}\n" +
                    $"Irányítószám: {Irszam}\n" +
                    $"Ország: {Orsz}\n" +
                    $"";
        }
    }
}
