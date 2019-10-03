﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageFramework.Model;

namespace ImageConsole.Commands.Export
{
    public class TellFormatsCommand : Command
    {
        public TellFormatsCommand() : base("-tellformats", "\"file extension\"", "prints the available export formats for a specific file extension")
        {
        }

        public override void Execute(List<string> arguments, Models model)
        {
            var reader = new ParameterReader(arguments);
            var ext = reader.ReadString("file extension");
            reader.ExpectNoMoreArgs();

            foreach (var exportFormatModel in model.Export.Formats)
            {
                if (exportFormatModel.Extension == ext)
                {
                    foreach (var gliFormat in exportFormatModel.Formats)
                    {
                        Console.Out.WriteLine(gliFormat.ToString());
                    }

                    return;
                }
            }

            throw new Exception("could not find extension " + ext);
        }
    }
}
