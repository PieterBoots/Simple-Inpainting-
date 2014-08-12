using System;
using System.Drawing.Imaging;
using System.Drawing;


  class Inpainting
  {
    private class AColor
    {
    public   double R;
    public   double G;
   public   double B;
    }

    static private AColor GetAColor(AColor[,] inp, int x, int y, int width, int height)
    {
      if (x > width - 1) { x = width - 1; }
      if (x < 0) { x = 0; }
      if (y > height - 1) { y = height - 1; }
      if (y < 0) { y = 0; }
      return inp[x,y];
    }

    private static AColor[,] prepare(Bitmap bmp)
    {
      AColor[,] matrix2d = new AColor[bmp.Width, bmp.Height];
     
      for (int y = 0; y < bmp.Height; y++)
      {
        for (int x = 0; x < bmp.Width; x++)
        {
          Color middle = bmp.GetPixel(x, y);
          AColor AColor1 = new AColor();         
          AColor1.R = middle.R;
          AColor1.G = middle.G;
          AColor1.B = middle.B;
          matrix2d[x, y] = AColor1;
        }
      }
      return matrix2d;
    }


    private static Bitmap GetBitmap(AColor[,] inp, int width, int heigt)
    {
      Bitmap bmp = new Bitmap(width, heigt, PixelFormat.Format32bppArgb);
      for (int y = 0; y < heigt; y++)
      {
        for (int x = 0; x < width; x++)
        {
          int r = (int)inp[x, y].R;
          if (r < 0) { r = 0; }; if (r > 255) { r = 255; }
          int g = (int)inp[x, y].G;
          if (g < 0) { g = 0; }; if (g > 255) { g = 255; }
          int b = (int)inp[x, y].B;
          if (b < 0) { b = 0; }; if (b > 255) { b = 255; }
          bmp.SetPixel(x, y, Color.FromArgb(r, g, b));
        }
      }
      return bmp;
    }


    public static Bitmap DoInpaint(Bitmap bmp, int iterations,Color PaintColor)
    {
      AColor[,] inp = prepare(bmp);
      AColor[,] rf = prepare(bmp);
      AColor[,] precopy = prepare(bmp);
      for (int n = 1; n < iterations; n++)
      {
        HeatIteration(rf, inp,precopy, PaintColor, bmp.Width, bmp.Height);
      }
      return GetBitmap(inp, bmp.Width, bmp.Height);
    }

    private static void HeatIteration(AColor[,] rf, AColor[,] inp,AColor[,] precopy , Color paintcolor, int width, int height)
    {
      int y_minus_1 = 0;
      int y_plus_1=0;
      int x_minus_1=0;
      int x_plus_1 = 0;
      
      for (int y = 0; y < height; y++)
      {
         y_minus_1 = y - 1;
         y_plus_1 = y + 1;
        if (y_minus_1 < 0) 
        {
          y_minus_1 = 0;
        }      
        if (y_plus_1 > height - 1)
        { 
          y_plus_1 = height - 1;
        }

        for (int x = 0; x < width; x++)
        {
         
          if (rf[x,y].R == paintcolor.R)
          {

            x_minus_1 = x - 1;
            x_plus_1 = x + 1;
            if (x_minus_1 < 0) 
            { 
              x_minus_1 = 0;
            }            
            if (x_plus_1 > width - 1) 
            {
              x_plus_1 = width - 1; 
            }
            int cnt = 0;
            AColor c1 = inp[ x, y];
            AColor c2 = inp[ x_plus_1, y];
            AColor c3 = inp[ x_minus_1, y];
            AColor c4 = inp[ x, y_plus_1];
            AColor c5 = inp[ x, y_minus_1];
            AColor pre = new AColor();

            if (c1.R != paintcolor.R) {
              pre.R =pre.R +c1.R;
              pre.G = pre.G + c1.G;
              pre.B = pre.B + c1.B;
              cnt = cnt + 1;
            }
            if (c2.R != paintcolor.R)
            {
              pre.R = pre.R + c2.R;
              pre.G = pre.G + c2.G;
              pre.B = pre.B + c2.B;
              cnt = cnt + 1;
            }
            if (c3.R != paintcolor.R)
            {
              pre.R = pre.R + c3.R;
              pre.G = pre.G + c3.G;
              pre.B = pre.B + c3.B;
              cnt = cnt + 1;
            }
            if (c4.R != paintcolor.R)
            {
              pre.R = pre.R + c4.R;
              pre.G = pre.G + c4.G;
              pre.B = pre.B + c4.B;
              cnt = cnt + 1;
            }
            if (c5.R != paintcolor.R)
            {
              pre.R = pre.R + c5.R;
              pre.G = pre.G + c5.G;
              pre.B = pre.B + c5.B;
              cnt = cnt + 1;
            } 
            

            if (cnt > 0)
            {

              pre.R = pre.R / cnt;
              pre.G = pre.G / cnt;
              pre.B = pre.B / cnt;
              precopy[x, y] = pre;
            }
          }          
        }
      }
      for (int y = 0; y < height; y++)
      {
        for (int x = 0; x < width; x++)
        {
          if (rf[x, y].R == paintcolor.R)
          {
            inp[x, y] = precopy[x, y];
          }
        }
      }
    }

  }

