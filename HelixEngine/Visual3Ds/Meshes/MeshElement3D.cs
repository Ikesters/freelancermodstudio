﻿using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace HelixEngine
{
    /// <summary>
    /// Represents a base class for elements that contain one <see cref="GeometryModel3D"/> and front and back <see cref="Material"/>s.
    /// </summary>
    /// <remarks>
    /// Derived classes should override the Tesselate() method to generate the geometry.
    /// </remarks>
    public abstract class MeshElement3D : ModelVisual3D, IEditableObject
    {
        #region Constants and Fields

        /// <summary>
        ///   The back material property.
        /// </summary>
        public static readonly DependencyProperty BackMaterialProperty = DependencyProperty.Register(
            "BackMaterial", typeof(Material), typeof(MeshElement3D), new UIPropertyMetadata(null, MaterialChanged));

        /// <summary>
        ///   The fill property.
        /// </summary>
        public static readonly DependencyProperty FillProperty = DependencyProperty.Register(
            "Fill", typeof(Brush), typeof(MeshElement3D), new UIPropertyMetadata(null, FillChanged));

        /// <summary>
        ///   The material property.
        /// </summary>
        public static readonly DependencyProperty MaterialProperty = DependencyProperty.Register(
            "Material",
            typeof(Material),
            typeof(MeshElement3D),
            new UIPropertyMetadata(null, MaterialChanged));

        /// <summary>
        /// The is editing.
        /// </summary>
        private bool isEditing;

        /// <summary>
        /// The is geometry changed.
        /// </summary>
        private bool isGeometryChanged;

        /// <summary>
        /// The is material changed.
        /// </summary>
        private bool isMaterialChanged;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "MeshElement3D" /> class.
        /// </summary>
        protected MeshElement3D()
        {
            this.Content = new GeometryModel3D();
            this.UpdateModel();
        }
        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the back material.
        /// </summary>
        /// <value>The back material.</value>
        public Material BackMaterial
        {
            get
            {
                return (Material)this.GetValue(BackMaterialProperty);
            }

            set
            {
                this.SetValue(BackMaterialProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the fill brush. This brush will be used for both the Material and BackMaterial.
        /// </summary>
        /// <value>The fill brush.</value>
        public Brush Fill
        {
            get
            {
                return (Brush)this.GetValue(FillProperty);
            }

            set
            {
                this.SetValue(FillProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the material.
        /// </summary>
        /// <value>The material.</value>
        public Material Material
        {
            get
            {
                return (Material)this.GetValue(MaterialProperty);
            }

            set
            {
                this.SetValue(MaterialProperty, value);
            }
        }

        /// <summary>
        ///   Gets the geometry model.
        /// </summary>
        /// <value>The geometry model.</value>
        public GeometryModel3D Model
        {
            get
            {
                return this.Content as GeometryModel3D;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Begins an edit on an object.
        /// </summary>
        public void BeginEdit()
        {
            this.isEditing = true;
            this.isGeometryChanged = false;
            this.isMaterialChanged = false;
        }

        /// <summary>
        /// Discards changes since the last <see cref="M:System.ComponentModel.IEditableObject.BeginEdit"/> call.
        /// </summary>
        public void CancelEdit()
        {
            this.isEditing = false;
        }

        /// <summary>
        /// Pushes changes since the last <see cref="M:System.ComponentModel.IEditableObject.BeginEdit"/> or <see cref="M:System.ComponentModel.IBindingList.AddNew"/> call into the underlying object.
        /// </summary>
        public void EndEdit()
        {
            this.isEditing = false;
            if (this.isGeometryChanged)
            {
                this.OnGeometryChanged();
            }

            if (this.isMaterialChanged)
            {
                this.OnMaterialChanged();
            }
        }

        /// <summary>
        /// Forces an update to the geometry model and materials
        /// </summary>
        public void UpdateModel()
        {
            this.OnGeometryChanged();
            this.OnMaterialChanged();
        }

        #endregion

        #region Methods

        /// <summary>
        /// The geometry changed.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        protected static void GeometryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MeshElement3D)d).OnGeometryChanged();
        }

        /// <summary>
        /// The material changed.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        protected static void MaterialChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MeshElement3D)d).OnMaterialChanged();
        }

        /// <summary>
        /// The on fill changed.
        /// </summary>
        protected virtual void OnFillChanged()
        {
            this.Material = MaterialHelper.CreateMaterial(this.Fill);
            this.BackMaterial = this.Material;
        }

        /// <summary>
        /// The geometry changed.
        /// </summary>
        protected virtual void OnGeometryChanged()
        {
            if (!this.isEditing)
            {
                // Debug.WriteLine("{0} geometry changed. Tesselating.", GetType());
                this.Model.Geometry = this.Tessellate();
            }
            else
            {
                this.isGeometryChanged = true;
            }
        }

        /// <summary>
        /// The material changed.
        /// </summary>
        protected virtual void OnMaterialChanged()
        {
            if (!this.isEditing)
            {
                this.Model.Material = this.Material;
                this.Model.BackMaterial = this.BackMaterial;
            }
            else
            {
                this.isMaterialChanged = true;
            }
        }

        /// <summary>
        /// Do the tesselation and return the <see cref="MeshGeometry3D"/>.
        /// </summary>
        /// <returns>
        /// A triangular mesh geometry.
        /// </returns>
        protected abstract MeshGeometry3D Tessellate();

        /// <summary>
        /// Called when Fill is changed.
        /// </summary>
        /// <param name="d">
        /// The mesh element.
        /// </param>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        private static void FillChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MeshElement3D)d).OnFillChanged();
        }

        #endregion
    }
}