using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemManager
{
    /// <summary>
    /// This class implements the Bressenham Line Algorithm to rasterize the segment formed by one pair of indexs.
    /// </summary>
    public static class Rasterizer
    {
        /// <summary>
        /// Main Function. Decide which routine is going to rasterize line
        /// </summary>
        /// <param name="begin">The index of the begin of the line.</param>
        /// <param name="end">The index of the begin of the line.</param>
        /// <param name="dem">A list of dems.</param>
        /// <returns>A list with the rastered indexs</returns>
        public static List<Index> rasterizeLine(Index begin, Index end, double scaleFactor)
        {
            if (Math.Abs(end.getRow() - begin.getRow()) < Math.Abs(end.getCol() - begin.getCol()))
            {
                if (end.getCol() >= begin.getCol())
                    return rasterizeLineOrig(begin, end, scaleFactor);
                else
                    return rasterizeLineReverseOrig(begin, end, scaleFactor);
            }
            else
            {
                if (end.getRow() >= begin.getRow())
                    return rasterizeLineSteep(begin, end, scaleFactor);
                else
                    return rasterizeLineReverseSteep(begin, end, scaleFactor);
            }
        }

        /// <summary>
        /// A rasterizer.
        /// </summary>
        /// <remarks>
        /// For more info see http://www.codeproject.com/KB/graphics/bresenham_revisited.aspx
        /// </remarks>
        /// <param name="begin">The index of the begin of the line.</param>
        /// <param name="end">The index of the begin of the line.</param>
        /// <param name="dem">A list of dems.</param>
        /// <returns>A list with the rastered indexs</returns>
        private static List<Index> rasterizeLineOrig(Index begin, Index end, double scaleFactor)
        {
            List<Index> retIndex = new List<Index>();
            double totalDistance = Math.Sqrt(Math.Pow((begin.getRow() - end.getRow()), 2) + Math.Pow((begin.getCol() - end.getCol()), 2));
            double totalHeight = (double)(end.getAlt() - begin.getAlt())/scaleFactor;
            double angle = Math.Atan2(totalHeight,totalDistance);
            int deltaX = end.getCol() - begin.getCol();
            int deltaY = (int)Math.Abs(begin.getRow() - end.getRow());
            int error = (int)(deltaX / 2.0);
            int yStep = 1;
            if (end.getRow() < begin.getRow())
                yStep = -1;
            else if (end.getRow() == begin.getRow())
                yStep = 0;
            int indX = begin.getCol();
            int indY = begin.getRow();
            retIndex.Add(begin);
            while (indX < end.getCol())
            {
                indX++;
                error -= deltaY;
                if (error < 0)
                {
                    indY += yStep;
                    error += deltaX;
                }
                double d = Math.Sqrt(Math.Pow((begin.getRow() - indY), 2) + Math.Pow((begin.getCol() - indX), 2));
                double alt = (double)begin.getAlt() + d * Math.Tan(angle) * scaleFactor;
                retIndex.Add(new Index(indY, indX, alt));
            }
            return retIndex;
        }

        /// <summary>
        /// A rasterizer.
        /// </summary>
        /// <remarks>
        /// For more info see http://www.codeproject.com/KB/graphics/bresenham_revisited.aspx
        /// </remarks>
        /// <param name="begin">The index of the begin of the line.</param>
        /// <param name="end">The index of the begin of the line.</param>
        /// <param name="dem">A list of dems.</param>
        /// <returns>A list with the rastered indexs</returns>
        private static List<Index> rasterizeLineReverseOrig(Index begin, Index end, double scaleFactor)
        {
            List<Index> retIndex = new List<Index>();
            double totalDistance = Math.Sqrt(Math.Pow((begin.getRow() - end.getRow()), 2) + Math.Pow((begin.getCol() - end.getCol()), 2));
            double totalHeight = (double)(end.getAlt() - begin.getAlt()) / scaleFactor;
            double angle = Math.Atan2(totalHeight, totalDistance);
            int deltaX = (int)Math.Abs(end.getCol() - begin.getCol());
            int deltaY = (int)Math.Abs(end.getRow() - begin.getRow());
            int error = deltaX / 2;
            int yStep = 1;
            if (end.getRow() < begin.getRow())
                yStep = -1;
            else if (end.getRow() == begin.getRow())
                yStep = 0;
            int indX = begin.getCol();
            int indY = begin.getRow();
            retIndex.Add(begin);
            while (indX > end.getCol())
            {
                indX--;
                error -= deltaY;
                if (error < 0)
                {
                    indY += yStep;
                    error += deltaX;
                }
                double d = Math.Sqrt(Math.Pow((begin.getRow() - indY), 2) + Math.Pow((begin.getCol() - indX), 2));
                double alt = (double)begin.getAlt() + d * Math.Tan(angle) * scaleFactor;
                retIndex.Add(new Index(indY, indX, alt));
            }
            return retIndex;
        }

        /// <summary>
        /// A rasterizer.
        /// </summary>
        /// <remarks>
        /// For more info see http://www.codeproject.com/KB/graphics/bresenham_revisited.aspx
        /// </remarks>
        /// <param name="begin">The index of the begin of the line.</param>
        /// <param name="end">The index of the begin of the line.</param>
        /// <param name="dem">A list of dems.</param>
        /// <returns>A list with the rastered indexs</returns>
        private static List<Index> rasterizeLineSteep(Index begin, Index end, double scaleFactor)
        {
            List<Index> retIndex = new List<Index>();
            double totalDistance = Math.Sqrt(Math.Pow((begin.getRow() - end.getRow()), 2) + Math.Pow((begin.getCol() - end.getCol()), 2));
            double totalHeight = (double)(end.getAlt() - begin.getAlt()) / scaleFactor;
            double angle = Math.Atan2(totalHeight, totalDistance);
            int deltaX = Math.Abs(end.getCol() - begin.getCol());
            int deltaY = end.getRow() - begin.getRow();
            int error = Math.Abs((int)(deltaX / 2.0));
            int xStep = 1;
            if (end.getCol() < begin.getCol())
                xStep = -1;
            else if (end.getCol() == begin.getCol())
                xStep = 0;
            int indX = begin.getCol();
            int indY = begin.getRow();
            retIndex.Add(begin);
            while (indY < end.getRow())
            {
                indY++;
                error -= deltaX;
                if (error < 0)
                {
                    indX += xStep;
                    error += deltaY;
                }
                double d = Math.Sqrt(Math.Pow((begin.getRow() - indY), 2) + Math.Pow((begin.getCol() - indX), 2));
                double alt = (double)begin.getAlt() + d * Math.Tan(angle) * scaleFactor;
                retIndex.Add(new Index(indY, indX, alt));
            }
            return retIndex;
        }

        /// <summary>
        /// A rasterizer.
        /// </summary>
        /// <remarks>
        /// For more info see http://www.codeproject.com/KB/graphics/bresenham_revisited.aspx
        /// </remarks>
        /// <param name="begin">The index of the begin of the line.</param>
        /// <param name="end">The index of the begin of the line.</param>
        /// <param name="dem">A list of dems.</param>
        /// <returns>A list with the rastered indexs</returns>
        private static List<Index> rasterizeLineReverseSteep(Index begin, Index end, double scaleFactor)
        {
            List<Index> retIndex = new List<Index>();
            double totalDistance = Math.Sqrt(Math.Pow((begin.getRow() - end.getRow()), 2) + Math.Pow((begin.getCol() - end.getCol()), 2));
            double totalHeight = (double)(end.getAlt() - begin.getAlt()) / scaleFactor;
            double angle = Math.Atan2(totalHeight, totalDistance);
            int deltaX = end.getCol() - begin.getCol();
            int deltaY = end.getRow() - begin.getRow();
            int error = (int)(deltaX / 2.0);
            int xStep = 1;
            if (end.getCol() < begin.getCol())
                xStep = -1;
            else if (end.getCol() == begin.getCol())
                xStep = 0;
            int indX = begin.getCol();
            int indY = begin.getRow();
            retIndex.Add(begin);
            while (indY > end.getRow())
            {
                indY--;
                error += deltaX;
                if (error < 0)
                {
                    indX += xStep;
                    error -= deltaY;
                }
                double d = Math.Sqrt(Math.Pow((begin.getRow() - indY), 2) + Math.Pow((begin.getCol() - indX), 2));
                double alt = (double)begin.getAlt() + d * Math.Tan(angle) * scaleFactor;
                retIndex.Add(new Index(indY, indX, alt));
            }
            return retIndex;
        }
    }
}
