Welcome to the Simple-Inpainting- wiki!

I have added some images to the repository to show how well it performs.

This is how the library can be used.

Bitmap bmp = (Bitmap)pictureBox1.Image; 
Bitmap result=Inpainting. DoInpaint(bmp, 100, Color.FromArgb(255, 0,0,0));
pictureBox1.Image = result;
result.Save("result.bmp");
