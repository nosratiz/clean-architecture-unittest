using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Hastnama.Solico.Common.FileProcessor
{
    public interface IImageProcessor
    {
        Image ResizeByWidthAndHeight(IFormFile upload, int newWidth, int newHeight, int horizontalResolution = 0, int verticalResolution = 0, string fileName = null);

        Image ResizeByPercentage(IFormFile upload, int percent, int horizontalResolution = 0, int verticalResolution = 0);

        Image ResizeByWidth(IFormFile upload, int newWidth, int horizontalResolution = 0, int verticalResolution = 0);

        Image ResizeByHeight(IFormFile upload, int newHeight, int horizontalResolution = 0, int verticalResolution = 0);

        Image ResizeByWiderSide(IFormFile upload, int widerSideSize, int horizontalResolution = 0, int verticalResolution = 0);

        Image Crop(IFormFile upload, int width, int height, AnchorPosition anchor = AnchorPosition.Center, int horizontalResolution = 0, int verticalResolution = 0);

        Image Crop(Image upload, int width, int height, AnchorPosition anchor = AnchorPosition.Center,
            int horizontalResolution = 0, int verticalResolution = 0);

        Image GrayScale(IFormFile upload);

        Image Merge(IFormFile upload, string rootPath, string directory);

        Image Merge(IFormFile upload, string rootPath, string directory, MergePosition mergePosition, float xSpace, float ySpace);

        Image WriteTextOnImage(IFormFile upload, string text, string fontFamily, float fontSize, Brush color, float xSpace, float ySpace);

        Image WriteTextOnImage(Image image, string text, string fontFamily, float fontSize, Brush color, float xSpace, float ySpace);

        Image WriteTextOnImage(string imageFilePath, string text, string fontFamily, float fontSize, Brush color, float xSpace, float ySpace);

        Image WriteTextOnImageWithBackGround(string imageFilePath, string text, string fontFamily, float fontSize, Brush color, float xSpace, float ySpace);

        Image MergeAndWriteTextOnImage(IFormFile upload, string rootPath, string watermarkPhotoPath, string text, string fontFamily, float fontSize, Brush color, MergePosition mergePosition, float xSpace, float ySpace, float xTextAdjustment = 0, float yTextAdjustment = 0);

        Image MergeAndWriteTextOnImage(Image image, string rootPath, string watermarkPhotoPath, string text, string fontFamily, float fontSize, Brush color, MergePosition mergePosition, float xSpace, float ySpace, float xTextAdjustment, float yTextAdjustment);

        SavedImageInfo Save(Image image, string filePath, string fileName = null, int quality = 100);

        SavedImageInfo Save(IFormFile upload, string filePath, string fileName = null, int quality = 100);

        ImageCodecInfo GetEncoderInfo(string mimeType);

        Stream GetStreamFromUrl(string url);

        Stream GetWebClientStreamFromPath(string path);

        Stream GetWebClientStreamFromUrl(string url);
    }
}