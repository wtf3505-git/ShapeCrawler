﻿using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using ObjectEx.Utilities;
using PptxXML.Enums;
using PptxXML.Models.Elements;
using PptxXML.Services;
using PptxXML.Services.Placeholder;
using P = DocumentFormat.OpenXml.Presentation;
using A = DocumentFormat.OpenXml.Drawing;

namespace PptxXML.Models
{
    /// <summary>
    /// Represents a slide.
    /// </summary>
    public class SlideEx
    {
        #region Fields

        private readonly SlidePart _xmlSldPart;

        private List<Element> _elements; //TODO: use capacity
        private ImageEx _backgroundImage;

        #region Dependencies

        private readonly IElementFactory _elFactory;
        private readonly IGroupShapeTypeParser _shapeTreeParser;
        private readonly IGroupBuilder _groupBuilder;
        private readonly ISlideLayoutPartParser _sldLayoutPartParser;
        private readonly IBackgroundImageFactory _bgImgFactory;

        #endregion Dependencies

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets elements.
        /// </summary>
        public IList<Element> Elements
        {
            get
            {
                if (_elements == null)
                {
                    InitElements();
                }

                return _elements;
            }
        }

        /// <summary>
        /// Returns a slide number in presentation.
        /// </summary>
        public int Number { get; set; } //TODO: Remove public setter somehow

        /// <summary>
        /// Returns a background image of slide.
        /// </summary>
        public ImageEx BackgroundImage
        {
            get
            {
                return _backgroundImage ??= _bgImgFactory.CreateBackgroundSlide(_xmlSldPart);
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initialize a new instance of the <see cref="SlideEx"/> class.
        /// </summary>
        public SlideEx(SlidePart xmlSldPart, 
                       int sldNumber, 
                       IElementFactory elFactory, 
                       IGroupShapeTypeParser shapeTreeParser,
                       IGroupBuilder groupBuilder,
                       ISlideLayoutPartParser sldLayoutPartParser,
                       IBackgroundImageFactory bgImgFactory)
        {
            Check.NotNull(xmlSldPart, nameof(xmlSldPart));
            Check.IsPositive(sldNumber, nameof(sldNumber));
            Check.NotNull(elFactory, nameof(elFactory));
            Check.NotNull(shapeTreeParser, nameof(shapeTreeParser));
            Check.NotNull(groupBuilder, nameof(groupBuilder));
            Check.NotNull(sldLayoutPartParser, nameof(sldLayoutPartParser));
            Check.NotNull(bgImgFactory, nameof(bgImgFactory));
            _xmlSldPart = xmlSldPart;
            Number = sldNumber;
            _elFactory = elFactory;
            _shapeTreeParser = shapeTreeParser;
            _groupBuilder = groupBuilder;
            _sldLayoutPartParser = sldLayoutPartParser;
            _bgImgFactory = bgImgFactory;
        }

        #endregion Constructors

        #region Private Methods

        private void InitElements()
        {
            _elements = new List<Element>();

            // Get candidates
            var shapeTreeCandidates = _shapeTreeParser.CreateCandidates(_xmlSldPart.Slide.CommonSlideData.ShapeTree);

            // Get placeholder dictionary
            var phDic = _sldLayoutPartParser.GetPlaceholderDic(_xmlSldPart.SlideLayoutPart);

            foreach (var ec in shapeTreeCandidates)
            {
                var newEl = ec.ElementType.Equals(ElementType.Group)
                    ? _groupBuilder.Build((P.GroupShape)ec.CompositeElement, _xmlSldPart)
                    : _elFactory.CreateRootElement(ec, _xmlSldPart, phDic);
                _elements.Add(newEl);
            }
        }

        #endregion Private Methods
    }
}