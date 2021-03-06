﻿using System.Drawing;

namespace TagsCloudVisualization.Viewer
{
    public class BitmapViewerToFile:IBitmapViewer
    {
        private readonly string filename;

        public BitmapViewerToFile(string filename)
        {
            this.filename = filename;
        }
        public void View(Bitmap bitmap)
        {
            bitmap.Save(filename);
        }
    }
}