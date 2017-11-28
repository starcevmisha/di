﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Autofac;
using Autofac.Core;
using CommandLine;
using CommandLine.Text;

namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();
            if(!Parser.Default.ParseArguments(args, options))
                return;
            
            var cloudCenter = new Point(400, 400);
            
            var container = new ContainerBuilder();
            container.RegisterType<CircularCloudLayouter>()
                .As<ICloudLayouter>()
                .WithParameter("cloudCenter", cloudCenter);
            container.RegisterType<FileAnalyzer>()
                .WithParameters(new List<Parameter>(){
                    new NamedParameter("count", options.Count),
                    new NamedParameter("minLength",options.MinLength)})
                .As<IFileAnalyzer>();
            container.Register(b => new FontSizeMaker(options.MinFont, options.MaxFont))
                .As<IFontSizeMaker>().SingleInstance();
            container.RegisterType<TagHandler>().As<ITagHandler>()
                .WithParameter("fontFamily", options.Font);
            container.RegisterType<BoringWordsDeterminer>()
                .As<IBoringWordDeterminer>();
            container.RegisterType<CloudTagDrawer>().AsSelf()
                .WithParameter("width", options.Width)
                .WithParameter("height", options.Height);
            container.RegisterType<FileReader>()
                .WithParameter("filename", options.InputFile)
                .As<IReader>();
            
            var build = container.Build();
            
            var cloudtagDrawer = build.Resolve<CloudTagDrawer>();
            
//            cloudtagDrawer.DrawTags(File.ReadLines(options.InputFile)).Save(options.OutputFile);
            cloudtagDrawer.DrawTags().ToForm();

        }
    }


    class Options
    {
        [Option('i', "input", Required = true, HelpText = "Input file to read.")]
        public string InputFile { get; set; }
        
        [Option('o', "output", Required = true, HelpText = "Output path for image")]
        public string OutputFile { get; set; }
        
        [Option('w', "width", DefaultValue = 800, HelpText = "output image width")]
        public int Width { get; set; }

        [Option('h', "height", DefaultValue = 800, HelpText = "output image height")]
        public int Height { get; set; }

        [Option("maxFont", DefaultValue = 80, HelpText = "maximal font size")]
        public int MaxFont { get; set; }

        [Option("minFont", DefaultValue = 10, HelpText = "minimal font size")]
        public int MinFont { get; set; }


        [Option('l', "minLen", DefaultValue = 0, HelpText = "minimal word length")]
        public int MinLength { get; set; }
        
        [Option('c', "count", DefaultValue = 150, HelpText = "count of word in cloud")]
        public int Count { get; set; }


        [Option('f', "Font", DefaultValue = "Tahoma", HelpText = "Font Name")]
        public string Font { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this);
        }
         
    }

    internal static class BitmapExtensions
    {
        public static void ToForm(this Bitmap bitmap)
        {
            Form aForm = new Form();
            aForm.Width = bitmap.Width;
            aForm.Height = bitmap.Height;
            aForm.BackgroundImage = bitmap;
            aForm.ShowDialog();
        } 
    }
}
