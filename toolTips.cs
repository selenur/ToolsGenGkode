// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ToolsGenGkode
{

        /// <summary>
        /// CustomizedToolTip to create tooltips with Image.
        /// </summary>
        internal class toolTips : ToolTip
        {
            #region Constants

            private const int TOOLTIP_WIDTH = 200;
            private const int TOOLTIP_HEIGHT = 60;
            private const int BORDER_THICKNESS = 1;
            private const int PADDING = 6;
            private const int DEFAULT_IMAGE_WIDTH = 15;

            #endregion

            #region Fields

            private static Color myBorderColor = Color.Red;
            private static Font myFont = new Font("Arial", 8);

            private StringFormat myTextFormat = new StringFormat();

            private Rectangle myImageRectangle = new Rectangle();
            private Rectangle myTextRectangle = new Rectangle();
            private Rectangle myToolTipRectangle = new Rectangle();

            private Brush myBackColorBrush = new SolidBrush(Color.LightYellow);
            private Brush myTextBrush = new SolidBrush(Color.Black);
            private Brush myBorderBrush = new SolidBrush(myBorderColor);

            private Size mySize = new Size(TOOLTIP_WIDTH, TOOLTIP_HEIGHT);

            private int myInternalImageWidth = DEFAULT_IMAGE_WIDTH;
            private bool myAutoSize = true;
            #endregion

            #region Properties

            /// <summary>
            /// Gets or sets a value indicating whether the ToolTip is drawn by the operating
            /// system or by code that you provide.
            /// If true, the properties 'ToolTipIcon' and 'ToolTipTitle' will set to their default values
            /// and the image will display in ToolTip otherwise only text will display.
            /// </summary>
            [CategoryAttribute("Custom Settings"), DescriptionAttribute(@"Gets or sets a value indicating whether the ToolTip is drawn by the operating system or by code that you provide. If true, the properties 'ToolTipIcon' and 'ToolTipTitle' will set to their default values and the image will display in ToolTip otherwise only text will display.")]
            public new bool OwnerDraw
            {
                get
                {
                    return base.OwnerDraw;
                }
                set
                {
                    if (value)
                    {
                        this.ToolTipIcon = ToolTipIcon.None;
                        this.ToolTipTitle = string.Empty;
                    }
                    base.OwnerDraw = value;
                }
            }

            /// <summary>
            /// Gets or sets a value that defines the type of icon to be displayed alongside
            /// the ToolTip text.
            /// Cannot set if the property 'OwnerDraw' is true.
            /// </summary>
            [CategoryAttribute("Custom Settings"), DescriptionAttribute(@"Gets or sets a value that defines the type of icon to be displayed alongside the ToolTip text. Cannot set if the property 'OwnerDraw' is true.")]
            public new ToolTipIcon ToolTipIcon
            {
                get
                {
                    return base.ToolTipIcon;
                }
                set
                {
                    if (!OwnerDraw)
                    {
                        base.ToolTipIcon = value;
                    }
                }
            }

            /// <summary>
            /// Gets or sets a title for the ToolTip window.
            /// Cannot set if the property 'OwnerDraw' is true.
            /// </summary>
            [CategoryAttribute("Custom Settings"), DescriptionAttribute(@"Gets or sets a title for the ToolTip window. Cannot set if the property 'OwnerDraw' is true.")]
            public new string ToolTipTitle
            {
                get
                {
                    return base.ToolTipTitle;
                }
                set
                {
                    if (!OwnerDraw)
                    {
                        base.ToolTipTitle = value;
                    }
                }
            }



            /// <summary>
            /// Gets or sets the background color for the ToolTip.
            /// </summary>
            [CategoryAttribute("Custom Settings"), DescriptionAttribute(@"Gets or sets the background color for the ToolTip.")]
            public new Color BackColor
            {
                get
                {
                    return base.BackColor;
                }
                set
                {
                    base.BackColor = value;
                    Brush temp = myBackColorBrush;
                    myBackColorBrush = new SolidBrush(value);
                    temp.Dispose();
                }
            }

            /// <summary>
            /// Gets or sets the foreground color for the ToolTip.
            /// </summary>
            [CategoryAttribute("Custom Settings"), DescriptionAttribute(@"Gets or sets the foreground color for the ToolTip.")]
            public new Color ForeColor
            {
                get
                {
                    return base.ForeColor;
                }
                set
                {
                    base.ForeColor = value;
                    Brush temp = myTextBrush;
                    myTextBrush = new SolidBrush(value);
                    temp.Dispose();
                }
            }

            /// <summary>
            /// Gets or sets a value that indicates whether the ToolTip resizes based on its text.
            /// true if the ToolTip automatically resizes based on its text; otherwise, false. The default is true.
            /// </summary>
            [CategoryAttribute("Custom Settings"), DescriptionAttribute(@"Gets or sets a value that indicates whether the ToolTip resizes based on its text. true if the ToolTip automatically resizes based on its text; otherwise, false. The default is true.")]
            public bool AutoSize
            {
                get { return myAutoSize; }
                set
                {
                    myAutoSize = value;
                    if (myAutoSize)
                    {
                        myTextFormat.Trimming = StringTrimming.None;
                    }
                    else
                    {
                        myTextFormat.Trimming = StringTrimming.EllipsisCharacter;
                    }
                }
            }

            /// <summary>
            /// Gets or sets the size of the ToolTip.
            /// Valid only if the Property 'AutoSize' is false.
            /// </summary>
            [CategoryAttribute("Custom Settings"), DescriptionAttribute(@"Gets or sets the size of the ToolTip. Valid only if the Property 'AutoSize' is false.")]
            public Size Size
            {
                get { return mySize; }
                set
                {
                    if (!myAutoSize)
                    {
                        mySize = value;
                        myToolTipRectangle.Size = mySize;
                    }
                }
            }

            /// <summary>
            /// Gets or sets the border color for the ToolTip.
            /// </summary>
            [CategoryAttribute("Custom Settings"), DescriptionAttribute(@"Gets or sets the border color for the ToolTip.")]
            public Color BorderColor
            {
                get
                {
                    return myBorderColor;
                }
                set
                {
                    myBorderColor = value;
                    Brush temp = myBorderBrush;
                    myBorderBrush = new SolidBrush(value);
                    temp.Dispose();
                }
            }

            #endregion

            #region Constructor

            /// <summary>
            /// Constructor to create instance of CustomizedToolTip class that can display Thumbnails/Images in it.
            /// </summary>
            public toolTips()
            {
                try
                {
                    this.OwnerDraw = true;

                    myTextFormat.FormatFlags = StringFormatFlags.LineLimit;
                    myTextFormat.Alignment = StringAlignment.Near;
                    myTextFormat.LineAlignment = StringAlignment.Center;
                    myTextFormat.Trimming = StringTrimming.None;

                    this.Popup += new PopupEventHandler(CustomizedToolTip_Popup);
                    this.Draw += new DrawToolTipEventHandler(CustomizedToolTip_Draw);
                }
                catch (Exception ex)
                {
                    string logMessage = "Exception in CustomizedToolTip.CustomizedToolTip () " + ex.ToString();
                    Trace.TraceError(logMessage);
                    throw;
                }
            }

            #endregion

            #region Methods

            /// <summary> 
            /// Clean up any resources being used.
            /// </summary>
            /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
            protected override void Dispose(bool disposing)
            {
                try
                {
                    //Dispose of the disposable objects.
                    try
                    {
                        if (disposing)
                        {
                            if (myFont != null)
                            {
                                myFont.Dispose();
                            }
                            if (myTextBrush != null)
                            {
                                myTextBrush.Dispose();
                            }
                            if (myBackColorBrush != null)
                            {
                                myBackColorBrush.Dispose();
                            }
                            if (myBorderBrush != null)
                            {
                                myBorderBrush.Dispose();
                            }
                            if (myTextFormat != null)
                            {
                                myTextFormat.Dispose();
                            }
                        }
                    }
                    finally
                    {
                        base.Dispose(disposing);
                    }
                }

                catch (Exception ex)
                {
                    string logMessage = "Exception in CustomizedToolTip.Dispose (bool) " + ex.ToString();
                    Trace.TraceError(logMessage);
                    throw;
                }
            }

            /// <summary>
            /// CustomizedToolTip_Draw raised when tooltip is drawn.
            /// </summary>
            /// <param name="sender">sender</param>
            /// <param name="e">e</param>
            void CustomizedToolTip_Draw(object sender, DrawToolTipEventArgs e)
            {
                try
                {

                    e.Graphics.CompositingQuality = CompositingQuality.HighQuality;

                    myToolTipRectangle.Size = e.Bounds.Size;
                    e.Graphics.FillRectangle(myBorderBrush, myToolTipRectangle);

                    myImageRectangle = Rectangle.Inflate(myToolTipRectangle, -BORDER_THICKNESS, -BORDER_THICKNESS);
                    e.Graphics.FillRectangle(myBackColorBrush, myImageRectangle);

                    Control parent = e.AssociatedControl;
                    Image toolTipImage = parent.Tag as Image;
                    if (toolTipImage != null)
                    {
                        myImageRectangle.Width = myInternalImageWidth;
                        myTextRectangle = new Rectangle(myImageRectangle.Right, myImageRectangle.Top,
                            (myToolTipRectangle.Width - myImageRectangle.Right - BORDER_THICKNESS), myImageRectangle.Height);
                        myTextRectangle.Location = new Point(myImageRectangle.Right, myImageRectangle.Top);

                        e.Graphics.FillRectangle(myBackColorBrush, myTextRectangle);
                        e.Graphics.DrawImage(toolTipImage, myImageRectangle);
                        e.Graphics.DrawString(e.ToolTipText, myFont, myTextBrush, myTextRectangle, myTextFormat);
                    }
                    else
                    {
                        e.Graphics.DrawString(e.ToolTipText, myFont, myTextBrush, myImageRectangle, myTextFormat);
                    }
                }

                catch (Exception ex)
                {
                    string logMessage = "Exception in CustomizedToolTip.BlindHeaderToolTip_Draw (object, DrawToolTipEventArgs) " + ex.ToString();
                    Trace.TraceError(logMessage);
                    throw;
                }
            }

            /// <summary>
            /// CustomizedToolTip_Popup raised when tooltip pops up.
            /// </summary>
            /// <param name="sender">sender</param>
            /// <param name="e">e</param>
            void CustomizedToolTip_Popup(object sender, PopupEventArgs e)
            {
                try
                {
                    if (OwnerDraw)
                    {
                        if (!myAutoSize)
                        {
                            e.ToolTipSize = mySize;
                            myInternalImageWidth = mySize.Height;
                        }
                        else
                        {
                            Size oldSize = e.ToolTipSize;
                            Control parent = e.AssociatedControl;
                            Image toolTipImage = parent.Tag as Image;
                            if (toolTipImage != null)
                            {
                                myInternalImageWidth = oldSize.Height;
                                oldSize.Width += myInternalImageWidth + PADDING;
                            }
                            else
                            {
                                oldSize.Width += PADDING;
                            }
                            e.ToolTipSize = oldSize;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string logMessage = "Exception in CustomizedToolTip.CustomizedToolTip_Popup (object, PopupEventArgs) " + ex.ToString();
                    Trace.TraceError(logMessage);
                    throw;
                }
            }

            #endregion
        }




}
