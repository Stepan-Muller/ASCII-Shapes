using System;

class Program
{
    static void Main()
    {
        Console.WindowWidth = 120;
        Console.WindowHeight = 40;

        // pocatecni uhel (rad)
        double angleX = Math.PI / 8;
        double angleY = 0.0;

        int scaleX = enterInt("Zadejte velikost X (1 - 13): ", 1, 13);
        int scaleY = enterInt("Zadejte velikost Y (1 - 13): ", 1, 13);
        int scaleZ = enterInt("Zadejte velikost Z (1 - 13): ", 1, 13);

        // souradnice bodu jehlanu
        double[,] pyramidVertices = new double[,]
        {
            {-scaleX,  scaleY, -scaleZ},
            {-scaleX,  scaleY,  scaleZ},
            { 0     , -scaleY,  0},
            { 0     , -scaleY,  0},
            { scaleX,  scaleY, -scaleZ},
            { scaleX,  scaleY,  scaleZ},
            { 0     , -scaleY, -0},
            { 0     , -scaleY,  0}
        };
        // propojeni bodu jehlanu do hran
        int[,] pyramidEdges = new int[,]
        {
            {0, 1}, {1, 3}, {2, 0},
            {4, 5}, {5, 7}, {6, 4},
            {0, 4}, {1, 5}
        };

        // souradnice bodu krychle
        double[,] cubeVertices = new double[,]
        {
            {-scaleX, -scaleY, -scaleZ},
            {-scaleX, -scaleY,  scaleZ},
            {-scaleX,  scaleY, -scaleZ},
            {-scaleX,  scaleY,  scaleZ},
            { scaleX, -scaleY, -scaleZ},
            { scaleX, -scaleY,  scaleZ},
            { scaleX,  scaleY, -scaleZ},
            { scaleX,  scaleY,  scaleZ}
        };
        // propojeni bodu krychle do hran
        int[,] cubeEdges = new int[,]
        {
            {0, 1}, {1, 3}, {3, 2}, {2, 0},
            {4, 5}, {5, 7}, {7, 6}, {6, 4},
            {0, 4}, {1, 5}, {2, 6}, {3, 7}
        };

        while (true)
        {
            Console.Clear();
            DrawRotatingShape(angleX, angleY, pyramidVertices, pyramidEdges, Console.WindowWidth / 4, Console.WindowHeight / 2);
            DrawRotatingShape(angleX, angleY, cubeVertices, cubeEdges, Console.WindowWidth / 4 * 3, Console.WindowHeight / 2);

            // otaceni krychle (rad)
            angleX += 0.0;
            angleY += 0.03;

            System.Threading.Thread.Sleep(50);
        }
    }

    static int enterInt(string prompt, int min = int.MinValue, int max = int.MaxValue)
    {
        Console.Write(prompt);

        int i;
        bool success = int.TryParse(Console.ReadLine(), out i);

        if (success && i >= min && i <= max)
            return i;
        
        Console.WriteLine("Nesprávná hodnota! Prosím zkuste to znovu.");
        return enterInt(prompt, min, max);
    }

    static void DrawRotatingShape(double angleX, double angleY, double[,] vertices, int[,] edges, int cx, int cy)
    {
        double cosX = Math.Cos(angleX);
        double sinX = Math.Sin(angleX);
        double cosY = Math.Cos(angleY);
        double sinY = Math.Sin(angleY);

        for (int i = 0; i < edges.GetLength(0); i++)
        {
            int vertex1 = edges[i, 0];
            int vertex2 = edges[i, 1];

            // otaceni podle y
            double x1 = vertices[vertex1, 0] * cosY - vertices[vertex1, 2] * sinY;
            double y1 = vertices[vertex1, 1];
            double z1 = vertices[vertex1, 0] * sinY + vertices[vertex1, 2] * cosY;

            double x2 = vertices[vertex2, 0] * cosY - vertices[vertex2, 2] * sinY;
            double y2 = vertices[vertex2, 1];
            double z2 = vertices[vertex2, 0] * sinY + vertices[vertex2, 2] * cosY;

            // otaceni podle x
            double tempY1 = y1 * cosX - z1 * sinX;
            double tempY2 = y2 * cosX - z2 * sinX;

            y1 = tempY1;
            y2 = tempY2;

            int screenX1 = (int)(cx + x1);
            int screenY1 = (int)(cy + y1);

            int screenX2 = (int)(cx + x2);
            int screenY2 = (int)(cy + y2);

            DrawLine(screenX1, screenY1, screenX2, screenY2, '*');
        }
    }

    // nakresli caru na obrazovku mezi souradnicemi z daneho znaku
    static void DrawLine(int x1, int y1, int x2, int y2, char symbol)
    {
        // delta souradnic
        int dx = Math.Abs(x2 - x1);
        int dy = Math.Abs(y2 - y1);
        // smer souradnic
        int sx = (x1 < x2) ? 1 : -1;
        int sy = (y1 < y2) ? 1 : -1;
        
        int err = dx - dy;

        while (true)
        {
            // kresleni znaku
            Console.SetCursorPosition(x1, y1);
            Console.Write(symbol);

            // ukonci funkci kdyz dosel ke konci cary
            if (x1 == x2 && y1 == y2)
                break;

            // zjisti ktera souradnice se ma posunout a posune ji
            int e2 = 2 * err;

            if (e2 > -dy)
            {
                err -= dy;
                x1 += sx;
            }

            if (e2 < dx)
            {
                err += dx;
                y1 += sy;
            }
        }
    }
}
