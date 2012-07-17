using System.IO;
using System;
class ArrayPrinter
{
    #region Declarations

    static bool isLeftAligned = false;
    const string cellLeftTop = "┌";
    const string cellRightTop = "┐";
    const string cellLeftBottom = "└";
    const string cellRightBottom = "┘";
    const string cellHorizontalJointTop = "┬";
    const string cellHorizontalJointbottom = "┴";
    const string cellVerticalJointLeft = "├";
    const string cellTJoint = "┼";
    const string cellVerticalJointRight = "┤";
    const string cellHorizontalLine = "─";
    const string cellVerticalLine = "│";

    #endregion

    #region Private Methods

    private static int GetMaxCellWidth(string[,] arrValues)
    {
        int maxWidth = 1;

        for (int i = 0; i < arrValues.GetLength(0); i++)
        {
            for (int j = 0; j < arrValues.GetLength(1); j++)
            {
                int length = arrValues[i, j].Length;
                if (length > maxWidth)
                {
                    maxWidth = length;
                }
            }
        }

        return maxWidth;
    }

    private static string GetDataInTableFormat(string[,] arrValues)
    {
        string formattedString = string.Empty;

        if (arrValues == null)
            return formattedString;

        int dimension1Length = arrValues.GetLength(0);
        int dimension2Length = arrValues.GetLength(1);

        int maxCellWidth = GetMaxCellWidth(arrValues);
        int indentLength = (dimension2Length * maxCellWidth) + (dimension2Length - 1);
        //printing top line;
        formattedString = string.Format("{0}{1}{2}{3}", cellLeftTop, Indent(indentLength), cellRightTop, System.Environment.NewLine);

        for (int i = 0; i < dimension1Length; i++)
        {
            string lineWithValues = cellVerticalLine;
            string line = cellVerticalJointLeft;
            for (int j = 0; j < dimension2Length; j++)
            {
                string value = (isLeftAligned) ? arrValues[i, j].PadRight(maxCellWidth, ' ') : arrValues[i, j].PadLeft(maxCellWidth, ' ');
                lineWithValues += string.Format("{0}{1}", value, cellVerticalLine);
                line += Indent(maxCellWidth);
                if (j < (dimension2Length - 1))
                {
                    line += cellTJoint;
                }
            }
            line += cellVerticalJointRight;
            formattedString += string.Format("{0}{1}", lineWithValues, System.Environment.NewLine);
            if (i < (dimension1Length - 1))
            {
                formattedString += string.Format("{0}{1}", line, System.Environment.NewLine);
            }
        }

        //printing bottom line
        formattedString += string.Format("{0}{1}{2}{3}", cellLeftBottom, Indent(indentLength), cellRightBottom, System.Environment.NewLine);
        return formattedString;
    }

    private static string Indent(int count)
    {
        return string.Empty.PadLeft(count, '─');
    }

    #endregion

    #region Public Methods

    public static void PrintToStream(string[,] arrValues, StreamWriter writer)
    {
        if (arrValues == null)
            return;

        if (writer == null)
            return;

        writer.Write(GetDataInTableFormat(arrValues));
    }

    public static void PrintToConsole(string[,] arrValues)
    {
        if (arrValues == null)
            return;

        Console.WriteLine(GetDataInTableFormat(arrValues));
    }

    #endregion

    //static void Main(string[] args)
    //{
    //    int value = 997;
    //    string[,] arrValues = new string[5, 5];
    //    for (int i = 0; i < arrValues.GetLength(0); i++)
    //    {
    //        for (int j = 0; j < arrValues.GetLength(1); j++)
    //        {
    //            value++;
    //            arrValues[i, j] = value.ToString();
    //        }
    //    }
    //    ArrayPrinter.PrintToConsole(arrValues);
    //    Console.ReadLine();
    //}
}