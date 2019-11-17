﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageViewer.Controller;
using ImageViewer.Models.Display;

namespace ImageViewer.Models
{
    public class ModelsEx : ImageFramework.Model.Models
    {
        public WindowModel Window { get; }
        public DisplayModel Display { get; }

        public SettingsModel Settings { get; }

        public IReadOnlyList<StatisticModel> Statistics { get; }

        private readonly ResizeController resizeController;
        private readonly ComputeImageController computeImageController;
        private readonly ClientDropController clientDropController;
        private readonly PaintController paintController;
        private readonly CropController cropController;
        public ModelsEx(MainWindow window)
        : base(4)
        {
            // only enabled first pipeline
            for (int i = 1; i < NumPipelines; ++i)
                Pipelines[i].IsEnabled = false;

            Settings = new SettingsModel();
            Window = new WindowModel(window);
            Display = new DisplayModel(this);

            var stats = new List<StatisticModel>();
            for(int i = 0; i < NumPipelines; ++i)
                stats.Add(new StatisticModel(this, Display, i));
            Statistics = stats;

            resizeController = new ResizeController(this);
            computeImageController = new ComputeImageController(this);
            paintController = new PaintController(this);
            clientDropController = new ClientDropController(this);
            cropController = new CropController(this);
        }

        public override void Dispose()
        {
            Settings?.Save();
            paintController?.Dispose();
            base.Dispose();
        }
    }
}
