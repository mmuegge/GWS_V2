namespace GWS_Library.Loxone
{
    public class LoxoneTemperatures
    {
        private double aussentemperatur;
        private double innentemperatur;

        // auf null Nachkommastellen runden
        public double Aussentemperatur
        {
            get { return Math.Round(aussentemperatur, 0); }
            set { aussentemperatur = value; }
        }

        // auf null Nachkommastellen runden
        public double Innentemperatur
        {
            get { return Math.Round(innentemperatur, 0); }
            set { innentemperatur = value; }
        }

    }
}
