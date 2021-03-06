﻿using System.Collections.Generic;
using SlideDotNet.Enums;
using SlideDotNet.Models.Settings;
using SlideDotNet.Models.SlideComponents;
using SlideDotNet.Models.SlideComponents.Chart;
using SlideDotNet.Models.TableComponents;
using P = DocumentFormat.OpenXml.Presentation;
using A = DocumentFormat.OpenXml.Drawing;

namespace SlideDotNet.Services.Builders
{
    /// <summary>
    /// Represents a shape builder.
    /// </summary>
    public interface IShapeBuilder
    {
        /// <summary>
        /// Builds a shape with OLE object content.
        /// </summary>
        ShapeEx WithOle(ILocation innerTransform, IShapeContext spContext, OleObject ole);

        /// <summary>
        /// Builds a shape with picture content.
        /// </summary>
        ShapeEx WithPicture(ILocation innerTransform, IShapeContext spContext, PictureEx picture, GeometryType geometry);

        /// <summary>
        /// Builds a AutoShape.
        /// </summary>
        ShapeEx WithAutoShape(ILocation innerTransform, IShapeContext spContext, GeometryType geometry);

        /// <summary>
        /// Builds a shape with table content.
        /// </summary>
        ShapeEx WithTable(ILocation innerTransform, IShapeContext spContext, TableEx table);

        /// <summary>
        /// Builds a shape with OLE object content.
        /// </summary>
        ShapeEx WithChart(ILocation innerTransform, IShapeContext spContext, ChartEx chart);

        /// <summary>
        /// Builds a group shape which has grouped shape items.
        /// </summary>
        ShapeEx WithGroup(ILocation innerTransform, IShapeContext spContext, IList<ShapeEx> groupedShapes);
    }
}
