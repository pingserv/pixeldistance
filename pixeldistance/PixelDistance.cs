using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pixeldistance
{
    public class PixelDistance
    {
        PixelEditor pe;
        public List<PDPoint> Map;
        public Vector2 Observer;
        POV pov;

        public PixelDistance(PixelEditor pixelEditor)
        {
            Map = new List<PDPoint>();
            pe = pixelEditor;
            Observer = new Vector2(0, 0);
            pov = new POV(11, 11);

            CalculatePoints();
            RepaintMap();
        }

        private void ObserverPositionChanged(Vector2 position, Vector2 oldPosition)
        {
            Bitmap bmp = (Bitmap)pe.APBox.Image;
            var currPoint = Map.Where(p => p.Point.X == oldPosition.X && p.Point.Y == oldPosition.Y);
            var exists = currPoint.Count() > 0;
            if (exists)
            {
                bmp.SetPixel((int)oldPosition.X, (int)oldPosition.Y, getColorByPointType(currPoint.First().Type));
            } else
            {
                bmp.SetPixel((int)oldPosition.X, (int)oldPosition.Y, Color.White);
            }

            bmp.SetPixel((int)position.X, (int)position.Y, Color.Black);
            pe.APBox.Image = bmp;
            pe.Invalidate();
        }

        public void MoveObserver(Vector2 direction)
        {
            Vector2 newPosition = Observer + direction;
            if (!Enumerable.Range(0, pe.Width / pe.PixelSize).Contains((int)newPosition.X) ||
                !Enumerable.Range(0, pe.Height / pe.PixelSize).Contains((int)newPosition.Y) ||
                Map.Any(p => p.Point == newPosition && p.Type == PointType.Wall))
                return;

            Observer = newPosition;

            CalculatePoints();
            RepaintMap();
        }

        public void RepaintMap(bool allPixel = false)
        {
            int cols = pe.ClientSize.Width / pe.PixelSize;
            int rows = pe.ClientSize.Height / pe.PixelSize;
            Bitmap bmp = (Bitmap)pe.APBox.Image;

            if (allPixel)
            {
                pov = new POV(155, 105);
            }
            else
            {
                pov = new POV(11, 11);
            }

            for (var y = (int)Math.Max(Observer.Y - (pov.Y - 1) / 2, 0); y <= Math.Min(Observer.Y + (pov.Y - 1) / 2, pe.Height / pe.PixelSize); y++)
            {
                for (var x = (int)Math.Max(Observer.X - (pov.X - 1) / 2, 0); x <= Math.Min(Observer.X + (pov.X - 1) / 2, pe.Width / pe.PixelSize); x++)
                {
                    var currpoint = Map.Where(p => p.Point.X == x && p.Point.Y == y);
                    if (currpoint.Count() == 0)
                        continue;

                    Color col = getColorByPointType(currpoint.First().Type);

                    bmp.SetPixel(x, y, col);
                }
            }

            pe.APBox.Image = bmp;
            pe.Invalidate();
        }

        public void CalculatePoints(bool allPixel = false)
        {
            List<PDPoint> wallpoints = new List<PDPoint>();
            Bitmap bmp = (Bitmap)pe.APBox.Image;

            if (allPixel)
            {
                pov = new POV(155, 105);
            } else
            {
                pov = new POV(11, 11);
            }

            for (var y = (int)Math.Max(Observer.Y - (pov.Y - 1) / 2, 0); y <= Math.Min(Observer.Y + (pov.Y - 1) / 2, pe.Height / pe.PixelSize); y++)
            {
                for (var x = (int)Math.Max(Observer.X - (pov.X - 1) / 2, 0); x <= Math.Min(Observer.X + (pov.X - 1) / 2, pe.Width / pe.PixelSize); x++)
                {
                    var currPoint = Map.getPoint(x, y);
                    if (currPoint == null)
                    {
                        currPoint = new PDPoint()
                        {
                            Point = new Vector2(x, y),
                            Type = PointType.None
                        };

                        Map.Add(currPoint);
                    }

                    Color pointColor = bmp.GetPixel(x, y);
                    PointType pointType = getPointTypeByColor(pointColor);

                    // Falpontok megjelölése
                    if (pointType == PointType.Wall)
                    {
                        Map.getPoint(x, y).Type = PointType.Wall;

                        wallpoints.Add(new PDPoint()
                        {
                            Point = new Vector2(x, y),
                            Type = PointType.Wall
                        });

                        // Kontúrpontok távolságának meghatározása
                        var siblings = getSiblings(new Vector2(x, y));
                        foreach (Vector2 sibling in siblings)
                        {
                            var currpoint = Map.Where(p => p.Point.X == sibling.X && p.Point.Y == sibling.Y);
                            if (currpoint.Count() > 0 &&
                                currpoint.First().Type != PointType.Wall &&
                                currpoint.First().Type != PointType.Unknow)
                            {
                                pe.Distances[(int)currpoint.First().Point.X, (int)currpoint.First().Point.Y] = 0;
                            }
                        }
                    }

                    // Padló és ismeretlen pontok megjelölése
                    else
                    {
                        bool hits = PixelRayCast.RaycastHitRectangle(Map, Observer, new Vector2(x, y));
                        if (hits && pointType != PointType.Floor && pointType != PointType.Contour)
                        {
                            Map.getPoint(x, y).Type = PointType.Unknow;
                        } else if (pointType != PointType.Contour)
                        {
                            Map.getPoint(x, y).Type = PointType.Floor;

                            // padló pontok távolságának megjelölése
                            List<Vector2> siblings = getSiblings(new Vector2(x, y));
                            int nearest = int.MaxValue;
                            bool found = false;
                            foreach (Vector2 sibling in siblings)
                            {
                                if (pe.Distances[(int)sibling.X, (int)sibling.Y] != int.MaxValue && nearest > pe.Distances[(int)sibling.X, (int)sibling.Y])
                                {
                                    nearest = pe.Distances[(int)sibling.X, (int)sibling.Y];
                                    found = true;
                                }
                            }
                            if (found)
                                pe.Distances[x, y] = nearest + 1;
                        }
                    }
                }
            }

            // Konturpontok megjelölése
            foreach (PDPoint point in wallpoints)
            {
                var siblings = getSiblings(point.Point);

                foreach (Vector2 sibling in siblings)
                {
                    var currpoint = Map.Where(p => p.Point.X == sibling.X && p.Point.Y == sibling.Y);
                    if (currpoint.Count() > 0 &&
                        currpoint.First().Type != PointType.Wall &&
                        currpoint.First().Type != PointType.Unknow)
                    {
                        currpoint.First().Type = PointType.Contour;
                        pe.Distances[(int)currpoint.First().Point.X, (int)currpoint.First().Point.Y] = 0;
                    }
                }
            }

            // Observer megjelölése
            var obpoint = Map.Where(p => p.Point.X == Observer.X && p.Point.Y == Observer.Y).FirstOrDefault();
            obpoint.Type = PointType.Observer;
        }

        public PointType getPointTypeByColor(Color color)
        {
            return color == Color.FromArgb(0, 0, 0, 0) ? PointType.None :
                   color == Color.FromArgb(255, 255, 0, 0) ? PointType.Wall :
                   color == Color.FromArgb(255, 173, 216, 230) ? PointType.Floor :
                   color == Color.FromArgb(255, 255, 255, 0) ? PointType.Contour :
                   color == Color.FromArgb(255, 211, 211, 211) ? PointType.Unknow :
                   color == Color.FromArgb(255, 0, 0, 0) ? PointType.Observer :
                   PointType.None;
        }

        public Color getColorByPointType(PointType type)
        {
            return type == PointType.None ? Color.FromArgb(0, 0, 0, 0) : 
                   type == PointType.Wall ? Color.FromArgb(255, 255, 0, 0) :
                   type == PointType.Floor ? Color.FromArgb(255, 173, 216, 230) :
                   type == PointType.Contour ? Color.FromArgb(255, 255, 255, 0) :
                   type == PointType.Unknow ? Color.FromArgb(255, 211, 211, 211) :
                   type == PointType.Observer ? Color.FromArgb(255, 0, 0, 0) :
                   Color.White;
        }

        List<Vector2> getSiblings(Vector2 point)
        {
            List<Vector2> siblings = new List<Vector2>();
            
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x==0 && y==0)
                        continue;

                    if (point.X + x < 0 || point.Y + y < 0)
                        continue;

                    if (PixelRayCast.RaycastHitRectangle(Map, Observer, new Vector2(point.X + x, point.Y + y)))
                        continue;

                    siblings.Add(new Vector2(point.X + x, point.Y + y));
                }
            }

            return siblings;
        }
    }

    public static class Tools {
        public static PDPoint getPoint(this List<PDPoint> map, int x, int y)
        {
            var result = map.Where(p => p.Point.X == x && p.Point.Y == y);
            if (result.Count() > 0)
                return result.First();

            return null;
        }

        public static T[,] Populate<T>(this T[,] arr, T value)
        {
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    arr[i,j] = value;
                }
            }

            return arr;
        }

        public static Color Darken(this Color color, int factor)
        {
            int R = (color.R - factor < 0) ? 0 : color.R - factor;
            int G = (color.G + factor > 255) ? 255 : color.G + factor;
            int B = (color.B - factor < 0) ? 0 : color.B - factor;

            return Color.FromArgb(R, G, B);
        }
    }

    public class PDPoint
    {
        public Vector2 Point { get; set; }
        public PointType Type { get; set; }
        public decimal Distance { get; set; }
        public Vector2 Parent { get; set; }
    }

    public class POV
    {
        public POV(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }
    }

    public class DistancePoint
    {
        public int X { get; set; }
        public int Y { get; set; }
        public decimal Distance { get; set; }
    }

    public enum PointType
    {
        None,
        Wall,
        Floor,
        Contour,
        Unknow,
        Observer
    }
}
