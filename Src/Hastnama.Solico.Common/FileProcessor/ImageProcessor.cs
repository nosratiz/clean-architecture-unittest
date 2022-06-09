using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace Hastnama.Solico.Common.FileProcessor
{
    public class ImageProcessor : IImageProcessor
    {
        #region Size Proccessing

        public Image ResizeByWidthAndHeight(IFormFile upload, int newWidth, int newHeight, int horizontalResolution = 0,
            int verticalResolution = 0, string fileName = null)
        {
            using var image = Image.FromStream(upload.OpenReadStream());
            var sourceWidth = image.Width;
            var sourceHeight = image.Height;

            //Consider vertical pics
            if (sourceWidth < sourceHeight)
            {
                var temp = newWidth;
                newWidth = newHeight;
                newHeight = temp;
            }

            var sourceX = 0;
            var sourceY = 0;
            var destX = 0;
            var destY = 0;
            float nPercent;

            var nPercentW = (newWidth / (float) sourceWidth);
            var nPercentH = (newHeight / (float) sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = Convert.ToInt16((newWidth -
                                         (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = Convert.ToInt16((newHeight -
                                         (sourceHeight * nPercent)) / 2);
            }

            var destWidth = (int) (sourceWidth * nPercent);
            var destHeight = (int) (sourceHeight * nPercent);

            var bmPhoto = new Bitmap(newWidth, newHeight,
                PixelFormat.Format32bppArgb);

            if (horizontalResolution > 0 && verticalResolution > 0)
            {
                bmPhoto.SetResolution(Convert.ToInt32(horizontalResolution),
                    Convert.ToInt32(verticalResolution));
            }
            else
            {
                bmPhoto.SetResolution(image.HorizontalResolution,
                    image.VerticalResolution);
            }

            var grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.FromArgb(0, 255, 255, 255));
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
            grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;

            grPhoto.DrawImage(image,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        public Image ResizeByPercentage(IFormFile upload, int percent, int horizontalResolution = 0,
            int verticalResolution = 0)
        {
            using var image = Image.FromStream(upload.OpenReadStream());
            float nPercent = (percent / 100);

            var sourceWidth = image.Width;
            var sourceHeight = image.Height;
            var sourceX = 0;
            var sourceY = 0;
            var destX = 0;
            var destY = 0;
            var destWidth = (int) (sourceWidth * nPercent);
            var destHeight = (int) (sourceHeight * nPercent);

            var bmPhoto = new Bitmap(destWidth, destHeight,
                PixelFormat.Format32bppArgb);

            if (horizontalResolution > 0 && verticalResolution > 0)
            {
                bmPhoto.SetResolution(Convert.ToInt32(horizontalResolution),
                    Convert.ToInt32(verticalResolution));
            }
            else
            {
                bmPhoto.SetResolution(image.HorizontalResolution,
                    image.VerticalResolution);
            }

            var grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.FromArgb(0, 255, 255, 255));
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
            grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;

            grPhoto.DrawImage(image,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        public Image ResizeByWidth(IFormFile upload, int newWidth, int horizontalResolution = 0,
            int verticalResolution = 0)
        {
            using var image = Image.FromStream(upload.OpenReadStream());
            var sourceWidth = image.Width;
            var sourceHeight = image.Height;
            var sourceX = 0;
            var sourceY = 0;
            var destX = 0;
            var destY = 0;
            var destWidth = newWidth;
            var destHeight = (sourceHeight * newWidth) / sourceWidth;

            var bmPhoto = new Bitmap(destWidth, destHeight,
                PixelFormat.Format32bppArgb);

            if (horizontalResolution > 0 && verticalResolution > 0)
            {
                bmPhoto.SetResolution(Convert.ToInt32(horizontalResolution),
                    Convert.ToInt32(verticalResolution));
            }
            else
            {
                bmPhoto.SetResolution(image.HorizontalResolution,
                    image.VerticalResolution);
            }

            var grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.FromArgb(0, 255, 255, 255));
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
            grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;

            grPhoto.DrawImage(image,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        public Image ResizeByHeight(IFormFile upload, int newHeight, int horizontalResolution = 0,
            int verticalResolution = 0)
        {
            using var image = Image.FromStream(upload.OpenReadStream());
            var sourceWidth = image.Width;
            var sourceHeight = image.Height;
            var sourceX = 0;
            var sourceY = 0;
            var destX = 0;
            var destY = 0;
            var destWidth = (sourceWidth * newHeight) / sourceHeight;
            var destHeight = newHeight;

            var bmPhoto = new Bitmap(destWidth, destHeight,
                PixelFormat.Format32bppArgb);

            if (horizontalResolution > 0 && verticalResolution > 0)
            {
                bmPhoto.SetResolution(Convert.ToInt32(horizontalResolution),
                    Convert.ToInt32(verticalResolution));
            }
            else
            {
                bmPhoto.SetResolution(image.HorizontalResolution,
                    image.VerticalResolution);
            }

            var grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.FromArgb(0, 255, 255, 255));
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
            grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;

            grPhoto.DrawImage(image,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        public Image ResizeByWiderSide(IFormFile upload, int widerSideSize, int horizontalResolution = 0,
            int verticalResolution = 0)
        {
            using var image = Image.FromStream(upload.OpenReadStream());
            var sourceWidth = image.Width;
            var sourceHeight = image.Height;
            var sourceX = 0;
            var sourceY = 0;
            var destX = 0;
            var destY = 0;
            int destWidth;
            int destHeight;

            // landscape photo
            if (sourceWidth > sourceHeight)
            {
                destWidth = widerSideSize;
                destHeight = (sourceHeight * widerSideSize) / sourceWidth;
            }
            //portrait photo
            else
            {
                destWidth = (sourceWidth * widerSideSize) / sourceHeight;
                destHeight = widerSideSize;
            }

            var bmPhoto = new Bitmap(destWidth, destHeight, PixelFormat.Format32bppArgb);

            if (horizontalResolution > 0 && verticalResolution > 0)
            {
                bmPhoto.SetResolution(Convert.ToInt32(horizontalResolution), Convert.ToInt32(verticalResolution));
            }
            else
            {
                bmPhoto.SetResolution(image.HorizontalResolution, image.VerticalResolution);
            }

            var grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.FromArgb(0, 255, 255, 255));
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
            grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;

            grPhoto.DrawImage(image,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        public Image Crop(IFormFile upload, int width, int height, AnchorPosition anchor = AnchorPosition.Center,
            int horizontalResolution = 0, int verticalResolution = 0)
        {
            using var image = Image.FromStream(upload.OpenReadStream());
            // -1 for remove 1 px transparent black boarder remains from converting to percent
            var sourceWidth = image.Width;
            var sourceHeight = image.Height;
            var sourceX = 0;
            var sourceY = 0;
            var destX = 0;
            var destY = 0;
            float nPercent;

            var nPercentW = (width / (float) sourceWidth);
            var nPercentH = (height / (float) sourceHeight);

            if (nPercentH < nPercentW)
            {
                nPercent = nPercentW;
                destY = anchor switch
                {
                    AnchorPosition.Top => 0,
                    AnchorPosition.Bottom => (int) (height - (sourceHeight * nPercent)),
                    _ => (int) ((height - (sourceHeight * nPercent)) / 2)
                };
            }
            else
            {
                nPercent = nPercentH;
                destX = anchor switch
                {
                    AnchorPosition.Left => 0,
                    AnchorPosition.Right => (int) (width - (sourceWidth * nPercent)),
                    _ => (int) ((width - (sourceWidth * nPercent)) / 2)
                };
            }

            var destWidth = (int) (sourceWidth * nPercent);
            var destHeight = (int) (sourceHeight * nPercent);

            var bmPhoto = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            if (horizontalResolution > 0 && verticalResolution > 0)
            {
                bmPhoto.SetResolution(Convert.ToInt32(horizontalResolution),
                    Convert.ToInt32(verticalResolution));
            }
            else
            {
                bmPhoto.SetResolution(image.HorizontalResolution,
                    image.VerticalResolution);
            }

            var grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.FromArgb(0, 255, 255, 255));
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
            grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;

            grPhoto.DrawImage(image,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        public Image Crop(Image image, int width, int height, AnchorPosition anchor = AnchorPosition.Center,
            int horizontalResolution = 0, int verticalResolution = 0)
        {
            var sourceWidth = image.Width;
            var sourceHeight = image.Height;
            var sourceX = 0;
            var sourceY = 0;
            var destX = 0;
            var destY = 0;
            float nPercent;

            var nPercentW = (width / (float) sourceWidth);
            var nPercentH = (height / (float) sourceHeight);

            if (nPercentH < nPercentW)
            {
                nPercent = nPercentW;
                destY = anchor switch
                {
                    AnchorPosition.Top => 0,
                    AnchorPosition.Bottom => (int) (height - (sourceHeight * nPercent)),
                    _ => (int) ((height - (sourceHeight * nPercent)) / 2)
                };
            }
            else
            {
                nPercent = nPercentH;
                destX = anchor switch
                {
                    AnchorPosition.Left => 0,
                    AnchorPosition.Right => (int) (width - (sourceWidth * nPercent)),
                    _ => (int) ((width - (sourceWidth * nPercent)) / 2)
                };
            }

            var destWidth = (int) (sourceWidth * nPercent);
            var destHeight = (int) (sourceHeight * nPercent);

            var bmPhoto = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            if (horizontalResolution > 0 && verticalResolution > 0)
            {
                bmPhoto.SetResolution(Convert.ToInt32(horizontalResolution),
                    Convert.ToInt32(verticalResolution));
            }
            else
            {
                bmPhoto.SetResolution(image.HorizontalResolution,
                    image.VerticalResolution);
            }

            var grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.FromArgb(0, 255, 255, 255));
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
            grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;

            grPhoto.DrawImage(image,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        #endregion Size Proccessing

        #region Manipulations

        public Image GrayScale(IFormFile upload)
        {
            using var image = Image.FromStream(upload.OpenReadStream());
            var newBitmap = new Bitmap(image.Width, image.Height);
            using var graphic = Graphics.FromImage(newBitmap);
            var colorMatrix = new ColorMatrix(
                new[]
                {
                    new[] {.3f, .3f, .3f, 0, 0},
                    new[] {.59f, .59f, .59f, 0, 0},
                    new[] {.11f, .11f, .11f, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 1}
                });
            var attributes = new ImageAttributes();
            attributes.SetColorMatrix(colorMatrix);
            graphic.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height),
                0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
            graphic.Dispose();
            return newBitmap;
        }

        public Image Merge(IFormFile upload, string rootPath, string directory)
        {
            using var image = Image.FromStream(upload.OpenReadStream());
            var topImagePath = Path.Combine(rootPath, directory).Replace('/', '\\');
            using var topImage = Image.FromFile(topImagePath);
            var newBitmap = new Bitmap(image.Width, image.Height);
            
            using var graphic = Graphics.FromImage(newBitmap);
            
            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphic.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height),
                new Rectangle(0, 0, image.Width, image.Height),
                GraphicsUnit.Pixel);
            graphic.DrawImage(topImage, 0, 0);

            return newBitmap;
        }

        public Image Merge(IFormFile upload, string rootPath, string directory, MergePosition mergePosition,
            float xSpace, float ySpace)
        {
            using var image = Image.FromStream(upload.OpenReadStream());
            using var newBitmap = new Bitmap(image.Width, image.Height);
            using var graphic = Graphics.FromImage(newBitmap);
            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphic.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height),
                new Rectangle(0, 0, image.Width, image.Height),
                GraphicsUnit.Pixel);

            var topImagePath = Path.Combine(rootPath, directory).Replace('/', '\\');

            using var topImage = Image.FromFile(topImagePath);
            switch (mergePosition)
            {
                case MergePosition.TopLeft:
                    graphic.DrawImage(topImage, xSpace, ySpace);
                    break;
                case MergePosition.BottomLeft:
                {
                    var yPoint = image.Height - ySpace;
                    graphic.DrawImage(topImage, xSpace, yPoint);
                    break;
                }
                case MergePosition.TopRight:
                {
                    var xPoint = image.Width - topImage.Width - xSpace;
                    graphic.DrawImage(topImage, xPoint, ySpace);
                    break;
                }
                case MergePosition.BottomRight:
                {
                    var yPoint = image.Height - topImage.Height - ySpace;
                    var xPoint = image.Width - topImage.Width - xSpace;
                    graphic.DrawImage(topImage, xPoint, yPoint);
                    break;
                }
                case MergePosition.Center:
                {
                    var yPoint = image.Height / 2 - topImage.Height / 2 - ySpace / 2;
                    var xPoint = image.Width / 2 - topImage.Width / 2 - xSpace / 2;
                    graphic.DrawImage(topImage, xPoint, yPoint);
                    break;
                }
            }

            return newBitmap;
        }

        public Image WriteTextOnImage(IFormFile upload, string text, string fontFamily, float fontSize, Brush color,
            float xSpace, float ySpace)
        {
            using var image = Image.FromStream(upload.OpenReadStream());
            using Graphics graphics = Graphics.FromImage(image);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using var font = new Font(fontFamily, fontSize);
            graphics.DrawString(text, font, color, xSpace, ySpace);

            var result = new Bitmap(image);

            return result;
        }

        public Image WriteTextOnImage(Image image, string text, string fontFamily, float fontSize, Brush color,
            float xSpace, float ySpace)
        {
            using var image1 = image;
            using Graphics graphics = Graphics.FromImage(image);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using var font = new Font(fontFamily, fontSize);
            graphics.DrawString(text, font, color, xSpace, ySpace);

            var result = new Bitmap(image);

            return result;
        }

        public Image WriteTextOnImage(string imageFilePath, string text, string fontFamily, float fontSize, Brush color,
            float xSpace, float ySpace)
        {
            using var image = (Bitmap) Image.FromFile(imageFilePath);
            using Graphics graphics = Graphics.FromImage(image);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using var font = new Font(fontFamily, fontSize);
            graphics.DrawString(text, font, color, xSpace, ySpace);
            var result = new Bitmap(image);

            return result;
        }

        public Image WriteTextOnImageWithBackGround(string imageFilePath, string text, string fontFamily,
            float fontSize, Brush color, float xSpace, float ySpace)
        {
            using var image = (Bitmap) Image.FromFile(imageFilePath);
            using Graphics graphics = Graphics.FromImage(image);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using var font = new Font(fontFamily, fontSize);
            // Measure string.
            var stringSize = graphics.MeasureString(text, font);
            Color customColor = Color.FromArgb(180, Color.White);
            SolidBrush shadowBrush = new SolidBrush(customColor);

            graphics.FillRectangle(shadowBrush, 15, image.Height - 40, stringSize.Width + 15,
                stringSize.Height + 10);
            graphics.DrawString(text, font, color, new PointF(20f, image.Height - 35));

            var result = new Bitmap(image);

            return result;
        }

        public Image MergeAndWriteTextOnImage(IFormFile upload, string rootPath, string watermarkPhotoPath,
            string text, string fontFamily, float fontSize, Brush color, MergePosition mergePosition, float xSpace,
            float ySpace, float xTextAdjustment, float yTextAdjustment)
        {
            using var image = Image.FromStream(upload.OpenReadStream());
            var topImagePath = Path.Combine(rootPath, watermarkPhotoPath).Replace('/', '\\');
            using var topImage = Image.FromFile(topImagePath);
            var newBitmap = new Bitmap(image.Width, image.Height);
            var graphic = Graphics.FromImage(newBitmap);

            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphic.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height),
                new Rectangle(0, 0, image.Width, image.Height),
                GraphicsUnit.Pixel);

            switch (mergePosition)
            {
                case MergePosition.TopLeft:
                {
                    graphic.DrawImage(topImage, xSpace, ySpace);
                    using var font = new Font(fontFamily, fontSize);
                    graphic.DrawString(text, font, color, xSpace + xTextAdjustment, ySpace + yTextAdjustment);
                    break;
                }
                case MergePosition.BottomLeft:
                {
                    var yPoint = image.Height - ySpace;
                    graphic.DrawImage(topImage, xSpace, yPoint);
                    using var font = new Font(fontFamily, fontSize);
                    graphic.DrawString(text, font, color, xSpace + xTextAdjustment, yPoint + yTextAdjustment);
                    break;
                }
                case MergePosition.TopRight:
                {
                    var xPoint = image.Width - topImage.Width - xSpace;
                    graphic.DrawImage(topImage, xPoint, ySpace);
                    using var font = new Font(fontFamily, fontSize);
                    graphic.DrawString(text, font, color, xPoint + xTextAdjustment, ySpace + yTextAdjustment);
                    break;
                }
                case MergePosition.BottomRight:
                {
                    var yPoint = image.Height - topImage.Height - ySpace;
                    var xPoint = image.Width - topImage.Width - xSpace;
                    graphic.DrawImage(topImage, xPoint, yPoint);
                    using var font = new Font(fontFamily, fontSize);
                    graphic.DrawString(text, font, color, xPoint + xTextAdjustment, yPoint + yTextAdjustment);
                    break;
                }
                case MergePosition.Center:
                {
                    var yPoint = image.Height / 2 - topImage.Height / 2 - ySpace / 2;
                    var xPoint = image.Width / 2 - topImage.Width / 2 - xSpace / 2;
                    graphic.DrawImage(topImage, xPoint, yPoint);
                    using var font = new Font(fontFamily, fontSize);
                    graphic.DrawString(text, font, color, xPoint + xTextAdjustment, yPoint + yTextAdjustment);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(mergePosition), mergePosition, null);
            }

            return newBitmap;
        }

        public Image MergeAndWriteTextOnImage(Image image, string rootPath, string watermarkPhotoPath, string text,
            string fontFamily, float fontSize, Brush color, MergePosition mergePosition, float xSpace, float ySpace,
            float xTextAdjustment, float yTextAdjustment)
        {
            using var image1 = image;
            var topImagePath = Path.Combine(rootPath, watermarkPhotoPath).Replace('/', '\\');
            using var topImage = Image.FromFile(topImagePath);
            var newBitmap = new Bitmap(image.Width, image.Height);
            var graphic = Graphics.FromImage(newBitmap);

            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphic.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height),
                new Rectangle(0, 0, image.Width, image.Height),
                GraphicsUnit.Pixel);

            switch (mergePosition)
            {
                case MergePosition.TopLeft:
                {
                    graphic.DrawImage(topImage, xSpace, ySpace);
                    using var font = new Font(fontFamily, fontSize);
                    graphic.DrawString(text, font, color, xSpace + xTextAdjustment, ySpace + yTextAdjustment);
                    break;
                }
                case MergePosition.BottomLeft:
                {
                    var yPoint = image.Height - ySpace;
                    graphic.DrawImage(topImage, xSpace, yPoint);
                    using var font = new Font(fontFamily, fontSize);
                    graphic.DrawString(text, font, color, xSpace + xTextAdjustment, yPoint + yTextAdjustment);
                    break;
                }
                case MergePosition.TopRight:
                {
                    var xPoint = image.Width - topImage.Width - xSpace;
                    graphic.DrawImage(topImage, xPoint, ySpace);
                    using var font = new Font(fontFamily, fontSize);
                    graphic.DrawString(text, font, color, xPoint + xTextAdjustment, ySpace + yTextAdjustment);
                    break;
                }
                case MergePosition.BottomRight:
                {
                    var yPoint = image.Height - topImage.Height - ySpace;
                    var xPoint = image.Width - topImage.Width - xSpace;
                    graphic.DrawImage(topImage, xPoint, yPoint);
                    using var font = new Font(fontFamily, fontSize);
                    graphic.DrawString(text, font, color, xPoint + xTextAdjustment, yPoint + yTextAdjustment);
                    break;
                }
                case MergePosition.Center:
                {
                    var yPoint = image.Height / 2 - topImage.Height / 2 - ySpace / 2;
                    var xPoint = image.Width / 2 - topImage.Width / 2 - xSpace / 2;
                    graphic.DrawImage(topImage, xPoint, yPoint);
                    using var font = new Font(fontFamily, fontSize);
                    graphic.DrawString(text, font, color, xPoint + xTextAdjustment, yPoint + yTextAdjustment);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(mergePosition), mergePosition, null);
            }

            return newBitmap;
        }

        #endregion Manipulations

        #region Re-Format and Save

        public SavedImageInfo Save(Image image, string filePath, string fileName = null, int quality = 100)
        {
            // encode parameter for image quality
            string extension = Path.GetExtension(filePath).Replace(".", "").ToLower();
            var qualityParam = new EncoderParameter(Encoder.Quality, quality);

            var codec = (extension == "jpg" || extension == "jpeg")
                ? GetEncoderInfo($"image/jpeg")
                : GetEncoderInfo("image/png");
            var encoderParams = new EncoderParameters(1) {Param = {[0] = qualityParam}};

            image.Save(filePath, codec, encoderParams);

            // Result
            return new SavedImageInfo
            {
                FileName = fileName,
                DiskPath = filePath,
                Height = image.Height,
                Width = image.Width
            };
        }

        public SavedImageInfo Save(IFormFile upload, string filePath, string fileName = null, int quality = 100)
        {
            using var image = Image.FromStream(upload.OpenReadStream());
            // encode parameter for image quality
            var qualityParam = new EncoderParameter(Encoder.Quality, quality);
            var codec = GetEncoderInfo($"image/{Path.GetExtension(filePath.ToLower())}");
            var encoderParams = new EncoderParameters(1) {Param = {[0] = qualityParam}};

            image.Save(filePath, codec, encoderParams);

            // Result
            return new SavedImageInfo
            {
                FileName = fileName,
                DiskPath = filePath,
                Height = image.Height,
                Width = image.Width
            };
        }

        #endregion Re-Format and Save

        #region Other

        public ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats
            var codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec
            return codecs.FirstOrDefault(t => t.MimeType == mimeType);
        }

        public Stream GetStreamFromUrl(string url)
        {
            var request = (HttpWebRequest) WebRequest.Create(url);
            request.Credentials = CredentialCache.DefaultCredentials;

            var response = (HttpWebResponse) request.GetResponse();
            return response.GetResponseStream();
        }

        public Stream GetWebClientStreamFromPath(string path)
        {
            using var webClient = new WebClient();
            var uri = new Uri(path, UriKind.Absolute);
            return new MemoryStream(webClient.DownloadData(uri));
        }

        public Stream GetWebClientStreamFromUrl(string url)
        {
            using var webClient = new WebClient();
            return new MemoryStream(webClient.DownloadData(url));
        }

        #endregion Other
    }

    #region Models and Enums

    public class SavedImageInfo
    {
        public string FileName { get; init; }
    
        public string DiskPath { get; init; }
        public int Width { get; init; }
        public int Height { get; init; }
    }

   

    public enum AnchorPosition
    {
        Top,
        Center,
        Bottom,
        Left,
        Right
    }

    public enum MergePosition
    {
        TopLeft,
        BottomLeft,
        TopRight,
        BottomRight,
        Center
    }

    #endregion Models and Enums
}