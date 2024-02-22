using System.Drawing;

namespace PaintOnlineClient.Models
{
    public class PaintBoard
    {
        public List<LineData> paintObjects = new List<LineData>();
        public double Width { get; private set; }
        public double Height { get; private set; }

        public string id;
        public void Resize(double width, double height) =>
            (Width, Height) = (width, height);
        public void Resize(Size size) =>
            (Width, Height) = (size.Width, size.Height);

        public void AppendObject(LineData paintObject)
        {
            paintObjects.Add(paintObject);
        }
    }
}
