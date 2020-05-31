﻿using System;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using SlideDotNet.Models.Settings;
using SlideDotNet.Services;
using SlideDotNet.Services.Drawing;

// ReSharper disable PossibleMultipleEnumeration

namespace SlideDotNet.Models
{
    /// <summary>
    /// Represents a slide.
    /// </summary>
    public class Slide
    {
        #region Fields

        private readonly Lazy<ImageEx> _backgroundImage;
        private readonly Lazy<ShapeCollection> _shapes;
        private readonly IPreSettings _preSettings;
        private readonly ISlideSchemeService _schemeService;
        private readonly SlidePart _sdkSldPart;
        private readonly SlideNumber _sldNumEntity;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Returns a slide shapes.
        /// </summary>
        public ShapeCollection Shapes => _shapes.Value;

        /// <summary>
        /// Returns a slide number in presentation.
        /// </summary>
        public int Number => _sldNumEntity.Number;

        /// <summary>
        /// Returns a background image of the slide. Returns <c>null</c>if slide does not have background image.
        /// </summary>
        public ImageEx BackgroundImage => _backgroundImage.Value;

        #endregion Properties

        #region Constructors

        public Slide(SlidePart sdkSldPart, SlideNumber sldNum, IPreSettings preSettings) :
            this(sdkSldPart, sldNum, preSettings, new SlideSchemeService())
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Slide"/> class.
        /// </summary>
        public Slide(SlidePart sdkSldPart, SlideNumber sldNum, IPreSettings preSettings, ISlideSchemeService schemeService)
        {
            _sdkSldPart = sdkSldPart ?? throw new ArgumentNullException(nameof(sdkSldPart));
            _sldNumEntity = sldNum ?? throw new ArgumentNullException(nameof(sldNum));
            _preSettings = preSettings ?? throw new ArgumentNullException(nameof(preSettings));
            _schemeService = schemeService ?? throw new ArgumentNullException(nameof(schemeService));
            _shapes = new Lazy<ShapeCollection>(GetShapeCollection);
            _backgroundImage = new Lazy<ImageEx>(TryGetBackground);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Saves slide scheme in PNG file.
        /// </summary>
        /// <param name="filePath"></param>
        public void SaveScheme(string filePath)
        {
            var sldSize = _preSettings.SlideSize.Value;
            _schemeService.SaveScheme(_shapes.Value, sldSize.Width, sldSize.Height, filePath);
        }

        /// <summary>
        /// Saves slide scheme in stream.
        /// </summary>
        /// <param name="stream"></param>
        public void SaveScheme(Stream stream)
        {
            var sldSize = _preSettings.SlideSize.Value;
            _schemeService.SaveScheme(_shapes.Value, sldSize.Width, sldSize.Height, stream);
        }

        #endregion Public Methods

        #region Private Methods

        private ShapeCollection GetShapeCollection()
        {
            var shapeCollection = new ShapeCollection(_sdkSldPart, _preSettings);
            return shapeCollection;
        }

        private ImageEx TryGetBackground()
        {
            var backgroundImageFactory = new ImageExFactory();
            return backgroundImageFactory.TryFromSdkSlide(_sdkSldPart);
        }

        #endregion Private Methods
    }
}