﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

public static class SecretImageSharing
{
    public static Bitmap[] SplitImageIntoShadows(Bitmap secretImage, int k, int n)
    {
        int width = secretImage.Width;
        int height = secretImage.Height;

        // Generate n random matrices (shadow images)
        Bitmap[] shadowImages = new Bitmap[n];
        Random random = new Random();

        for (int i = 0; i < k; i++)
        {
            shadowImages[i] = new Bitmap(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Color pixelColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
                    shadowImages[i].SetPixel(x, y, pixelColor);
                }
            }
        }

        // Compute the shadow images as XOR of secret image and random shadows
        for (int i = 0; i < k; i++)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Color pixelColor = secretImage.GetPixel(x, y);

                    for (int j = 0; j < k; j++)
                    {
                        if (j != i)
                        {
                            pixelColor = Color.FromArgb(
                                pixelColor.R ^ shadowImages[j].GetPixel(x, y).R,
                                pixelColor.G ^ shadowImages[j].GetPixel(x, y).G,
                                pixelColor.B ^ shadowImages[j].GetPixel(x, y).B);
                        }
                    }

                    shadowImages[i].SetPixel(x, y, pixelColor);
                }
            }
        }
        for (int i = k; i < n; i++)
        {
            shadowImages[i] = new Bitmap(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Color pixelColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
                    shadowImages[i].SetPixel(x, y, pixelColor);
                }
            }
        }

        for (int i = 0; i < shadowImages.Length; i++)
        {
            shadowImages[i].Save($"C:\\Users\\G3-3500\\Pictures\\Saved Pictures\\share_{i}.png");
        }

        return shadowImages;
    }

    public static Bitmap RevealSecretImage(Bitmap[] shadowImages, int k)
    {

        int width = shadowImages[0].Width;
        int height = shadowImages[0].Height;

        Bitmap revealedImage = new Bitmap(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color pixelColor = shadowImages[0].GetPixel(x, y);

                for (int i = 1; i < k; i++)
                {
                    pixelColor = Color.FromArgb(
                        pixelColor.R ^ shadowImages[i].GetPixel(x, y).R,
                        pixelColor.G ^ shadowImages[i].GetPixel(x, y).G,
                        pixelColor.B ^ shadowImages[i].GetPixel(x, y).B);
                }

                revealedImage.SetPixel(x, y, pixelColor);
            }
        }

        return revealedImage;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        string imagePath = "C:\\Users\\G3-3500\\Pictures\\Saved Pictures\\clr.png";
        Bitmap secretImage = new Bitmap(imagePath);

        int n = 5;
        int k = 3;
        // Total number of shadow images

        Bitmap[] shadowImages = SecretImageSharing.SplitImageIntoShadows(secretImage, k, n);
        Bitmap revealedImage = SecretImageSharing.RevealSecretImage(shadowImages, k);

        string outputImagePath = "C:\\Users\\G3-3500\\Pictures\\Saved Pictures\\revealed_image1.png";
        revealedImage.Save(outputImagePath, ImageFormat.Png);

        Console.WriteLine("Revealed image saved at: " + outputImagePath);
    }
}
