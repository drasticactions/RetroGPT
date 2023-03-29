// <copyright file="ImageTranscodeOptions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace RetroGPT.Core;

/// <summary>
    /// Image Transcode Options.
    /// </summary>
    public class ImageTranscodeOptions
    {
        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        public ImageFormat Format { get; set; } = ImageFormat.gif;

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        public int Width { get; set; } = 0;

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        public int Height { get; set; } = 0;

        /// <summary>
        /// Gets or sets the value to scale the image down by.
        /// Ignores width and height parameter.
        /// </summary>
        public int ScaleDownBy { get; set; } = 0;

        /// <summary>
        /// Gets or sets the frames per second for an animation.
        /// </summary>
        public int FramesPerSecond { get; set; } = 0;

        /// <summary>
        /// Gets or sets a value indicating whether to use the file cache.
        /// Defaults to true.
        /// </summary>
        public bool UseCache { get; set; } = true;

        /// <summary>
        /// Gets or sets the original, full, query string.
        /// </summary>
        public string FullQueryString { get; set; } = string.Empty;

        /// <inheritdoc/>
        public override string ToString()
        {
            if (this.Format is ImageFormat.Unknown)
            {
                throw new ArgumentException("ImageFormat must not be Unknown");
            }

            return $"format={this.Format}&width={this.Width}&height={this.Height}&scaledownby={this.ScaleDownBy}&framespersecond={this.FramesPerSecond}&usecache={this.UseCache}";
        }

        public static ImageTranscodeOptions FromQueryString(IQueryCollection queryCollection)
        {
            var parameters = queryCollection.Keys.Cast<string>().ToDictionary(k => k, v => queryCollection[v].ToString());
            return ImageTranscodeOptions.FromQueryString(parameters);
        }

        public static ImageTranscodeOptions FromQueryString(Dictionary<string, string> query)
        {
            var imageTranscodeOptions = new ImageTranscodeOptions();

            Enum.TryParse(typeof(ImageFormat), query.GetValueOrDefault("format"), out var imageFormat);
            if (imageFormat is ImageFormat format)
            {
                imageTranscodeOptions.Format = format;
            }

            int.TryParse(query.GetValueOrDefault("width"), out var width);
            int.TryParse(query.GetValueOrDefault("height"), out var height);
            int.TryParse(query.GetValueOrDefault("scaledownby"), out var scaledownby);
            int.TryParse(query.GetValueOrDefault("framespersecond"), out var framespersecond);
            bool.TryParse(query.GetValueOrDefault("usecache"), out var usecache);

            imageTranscodeOptions.UseCache = usecache;
            imageTranscodeOptions.Width = width;
            imageTranscodeOptions.Height = height;
            imageTranscodeOptions.ScaleDownBy = scaledownby;
            imageTranscodeOptions.FramesPerSecond = framespersecond;
            imageTranscodeOptions.FullQueryString = query.ToQueryString();
            return imageTranscodeOptions;
        }
    }