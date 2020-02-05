﻿using SlideXML.Validation;

namespace SlideXML.Models.TextBody
{
    /// <summary>
    /// Represents a paragraph text portion.
    /// </summary>
    public class Portion
    {
        #region Properties

        /// <summary>
        /// Returns font height in EMUs.
        /// </summary>
        public int FontHeight { get; }

        public string Text { get; }
        

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Portion"/> class.
        /// </summary>
        public Portion(int fontHeight, string text)
        {
            Check.IsPositive(fontHeight, nameof(fontHeight));
            Check.NotNull(text, nameof(text));
            FontHeight = fontHeight;
            Text = text;
        }

        #endregion Constructors
    }
}