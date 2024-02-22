using System.Text;

namespace PaintOnlineClient.Models
{
    public struct BoardPoint
    {
        public double X; public double Y;
        public BoardPoint(double X,double Y)
        {
            this.X = X;
            this.Y = Y;
        }
        public BoardPoint()
        {
            X = 0;
            Y = 0;
        }
        public override string ToString()
        {
            return X + "'" + Y;
        }
    }


    public class LineData 
    {
        public List<BoardPoint> Points { get; private set; }
        public string Color;
        public int Width;
        public LineData()
        {
            Points = new List<BoardPoint>();
        }

        public void AppendPoint(BoardPoint point)
        {
            Points.Add(point);
        }
        public void AppendPoint(double x, double y)
        {
            Points.Add(new BoardPoint(x,y));
        }

        public void ClearPoints()
        {
            Points.Clear();
        }
        public static LineData GetFromString(string data)
        {
            LineData line = new LineData();
            if (data != null)
            {
                var lineData = data.Split(new char[] { '>' }, StringSplitOptions.RemoveEmptyEntries);
                line.Color = lineData[0];  
                line.Width = int.Parse(lineData[1]);
                for (int i = 2; i < lineData.Length; i++)
                {
                    var pointData = lineData[i].Split(new char[] { '\'' }, StringSplitOptions.RemoveEmptyEntries);
                    line.AppendPoint(double.Parse(pointData[0]), double.Parse(pointData[1]));
                }
            }
            return line;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Color + ">" + Width + ">");
            foreach (BoardPoint point in Points)
            {
                sb.Append(point.ToString() + ">");
            }
            return sb.ToString();
        }
    }
}
