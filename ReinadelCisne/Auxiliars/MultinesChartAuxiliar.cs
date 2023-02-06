using System;
using System.Collections.Generic;
using System.Text;
using Microcharts;
using Microcharts.Forms;
using Microcharts.Abstracts;
using SkiaSharp;
using System.Linq;
using Xamarin.Forms.Internals;

namespace ReinadelCisne.Auxiliars
{
    public class MultinesChartAuxiliar: LineChart
    {
        public IEnumerable<IEnumerable<ChartEntry>> MultiLineEntries { get; set; }
        public List<string> LegendNames { get; set; }
        private bool initiated = false;
        private float multiLineMax;
        private float multiLineMin;
        private List<SKPoint> points = new List<SKPoint>();

        private void Init()
        {
            if (initiated)
            {
                return;
            }

            initiated = true;

            foreach (List<ChartEntry> line in MultiLineEntries)
            {
                foreach (ChartEntry entry in line)
                {
                    if (entry.Value > multiLineMax)
                    {
                        multiLineMax = entry.Value;
                    }
                    if (entry.Value < multiLineMin)
                    {
                        multiLineMin = entry.Value;
                    }
                }
            }
        }

        public override void DrawContent(SKCanvas canvas, int width, int height)
        {
            Init();
            #region MyRegion
            /*
                foreach (List<ChartEntry> line in MultiLineEntries)
                {
                    foreach (ChartEntry entry in line)
                    {
                        if (entry.Value > multiLineMax)
                        {
                            multiLineMax = entry.Value;
                        }
                        if (entry.Value < multiLineMin)
                        {
                            multiLineMin = entry.Value;
                        }
                    }
                }
                */ 
            #endregion
            Entries = MultiLineEntries.ElementAt(0);
            
            var valueLableSizes = MeasureLabels(MultiLineEntries.ElementAt(0).Select(t => t.Label).ToArray());
            var footerHight = CalculateFooterHeaderHeight(valueLableSizes, Orientation.Horizontal);
            var itemsSize = CalculateItemSize(width, height, footerHight, 100);
            var origin = CalculateYOrigin(itemsSize.Height, 100);

            foreach (IEnumerable<ChartEntry> l in MultiLineEntries)
            {
                Entries = l;
                var tempPoints = CalculateMultiLinePoints(itemsSize, origin, 100);
                points.AddRange(tempPoints);
                DrawLine(canvas, tempPoints, itemsSize);
                DrawPoints(canvas, tempPoints);

                //if(MultiLineEntries.IndexOf(l) == 0)
                //{
                    DrawArea(canvas, tempPoints, itemsSize, origin);
                    DrawFooter(canvas, l.Select(x => x.Label).ToArray(), valueLableSizes, tempPoints, itemsSize, height, footerHight);
                //}
            }

            DrawLegends(canvas, width, height);
        }
        private void DrawLegends(SKCanvas canvas, int width, int height)
        {
            if (!LegendNames.Any()) { return; }

            List<SKColor> colors = new List<SKColor>();

            foreach (List<ChartEntry> l in MultiLineEntries)
            {
                colors.Add(l[0].Color);
            }

            int radius_size = 20;

            using (var paint = new SKPaint())
            {
                paint.TextSize = LabelTextSize;
                paint.IsAntialias = false;
                paint.IsStroke = false;

                float x = 200 + radius_size * 2;
                float y = 50;

                foreach (string legend in LegendNames)
                {
                    paint.Color = SKColor.Parse("#000000");
                    canvas.DrawText(legend, x + radius_size + 10, y, paint);

                    paint.Color = colors.ElementAt(LegendNames.IndexOf(legend));
                    canvas.DrawCircle(x, y - radius_size / 2 - radius_size / 4, radius_size, paint);

                    x += radius_size * 2 + LabelTextSize * (legend.Length / 2 + 2);
                }

                var minPoint = points.Min(p => p.Y);
                var maxPoint = points.Max(p => p.Y);

                paint.Color = SKColor.Parse("#000000");
                paint.TextSize = 20;
                canvas.DrawText(NumbersToolsAuxiliar.GetNember(multiLineMax), 0, minPoint - 20, paint);
                canvas.DrawCircle(12, maxPoint, 5, paint);
                canvas.DrawText(NumbersToolsAuxiliar.GetNember(multiLineMin), 0, maxPoint - 20, paint);
                canvas.DrawCircle(12, minPoint, 5, paint);

                #region MyRegion
                /*var step = maxPoint / 8;
                        var valueStep = multiLineMax / 6;
                        for (int i = 1; i < 6; i++)
                        {
                            var tt = (maxPoint - (step * i));
                            if (minPoint < tt && Math.Abs(minPoint - tt) >= step)
                            {
                                canvas.DrawCircle(12, tt, 5, paint);
                                canvas.DrawText(NumbersToolsAuxiliar.GetNember(valueStep * i), 0, (maxPoint - step * i) - 20, paint);
                            }
                        }*/ 
                #endregion
            }
        }

        private SKPoint[] CalculateMultiLinePoints(SKSize itemsSize, float origin, int headerHeight)
        {
            var result = new List<SKPoint>();

            for (int i = 0; i < Entries.Count(); i++)
            {
                var entry = Entries.ElementAt(i);

                var x = Margin + (itemsSize.Width / 2) + (i * (itemsSize.Width + Margin));
                var y = headerHeight + (((multiLineMax - entry.Value) / (multiLineMax - multiLineMin)) * itemsSize.Height);
                var point = new SKPoint(x, y);
                result.Add(point);
            }

            return result.ToArray();
        }
    }
}
