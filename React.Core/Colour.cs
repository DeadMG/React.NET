namespace React.Core
{
    public class Colour
    {
        public Colour(float r, float g, float b, float a)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;
        }

        public float R { get; }
        public float G { get; }
        public float B { get; }
        public float A { get; }
    }
}
