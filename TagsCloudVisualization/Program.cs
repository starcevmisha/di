using System.Collections.Generic;
using System.Drawing;
using Autofac;
using Autofac.Core;
using CommandLine;
using CommandLine.Text;
using TagsCloudVisualization.CloudTagDrawer;
using TagsCloudVisualization.Viewer;
using TagsCloudVisualization.WordsAnalyze;

namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            //Запускать можно с этими праметрами: -i books/book1.txt -o 123.bmp -l 4
            var options = new Options();
            if (!Parser.Default.ParseArguments(args, options))
                return;

            var cloudCenter = new Point(options.Width / 2, options.Height / 2);

            var container = new ContainerBuilder();
            container.RegisterType<CircularCloudLayouter>()
                .As<ICloudLayouter>()
                .WithParameter("cloudCenter", cloudCenter);
            container.RegisterType<Exiter>().As<IExiter>();
            container.RegisterType<WordsAnalyzer>()
                .WithParameters(new List<Parameter>()
                {
                    new NamedParameter("count", options.Count),
                    new NamedParameter("minLength", options.MinLength)
                })
                .As<IWordsAnalyzer>();
            container.Register(b => new FontSizeMaker(options.MinFont, options.MaxFont))
                .As<IFontSizeMaker>().SingleInstance();
            container.RegisterType<TagMaker>().As<ITagMaker>()
                .WithParameter("fontName", options.Font);
            container.RegisterType<BoringWordsDeterminer>()
                .As<IBoringWordDeterminer>();
            container.RegisterType<BitmapViewerToForm>().As<IBitmapViewer>();

            container.RegisterType<CloudTagDrawer.CloudTagDrawer>().AsSelf()
                .WithParameter("width", options.Width)
                .WithParameter("height", options.Height)
                .WithParameter("outputFilename", options.OutputFile);
            container.RegisterType<FileReader>()
                .WithParameter("filename", options.InputFile)
                .As<IReader>();

            var build = container.Build();

            var cloudtagDrawer = build.Resolve<CloudTagDrawer.CloudTagDrawer>();

            cloudtagDrawer.DrawTags();
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
}