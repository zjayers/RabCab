﻿// -----------------------------------------------------------------------------------
//     <copyright file="RcMainPalette.cs" company="CraterSpace">
//     Copyright (c) 2019 CraterSpace - All Rights Reserved
//     </copyright>
//     <author>Zach Ayers</author>
//     <date>04/08/2019</date>
//     Description:
//     Notes:
//     References:
// -----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Internal;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Windows;
using RabCab.Agents;
using RabCab.Calculators;
using RabCab.Commands.AnalysisSuite;
using RabCab.Commands.AssemblySuite;
using RabCab.Engine.Enumerators;
using RabCab.Entities.Controls;
using RabCab.Entities.Controls.RabCab.Entities.Controls;
using RabCab.Extensions;
using RabCab.Settings;
using static Autodesk.AutoCAD.ApplicationServices.Core.Application;
using Exception = System.Exception;

namespace RabCab.Commands.PaletteKit
{
    internal class RcPaletteMetric
    {
        private const int CtrlHeight = 25;
        private const int LabColumn = 0;
        private const int InfoColumn = 1;
        internal static PaletteSet RcPal;
        private static bool _ignoreTextChange;

        private static TableLayoutPanel _tbLayout;

        private static Label _rcNameLab,
            _rcInfoLab,
            _rcLengthLab,
            _rcWidthLab,
            _rcThickLab,
            _rcVolLab,
            _rcAreaLab,
            _rcPerimLab,
            _rcAsymLab,
            _rcAsymStrLab,
            _rcQtyOfLab,
            _rcQtyTotalLab,
            _rcNumChangesLab,
            _rcParentLab,
            _rcChildLab,
            _rcTxDirLab,
            _prodTypLab;

        private static EntryBox
            _rcNameTxt,
            _rcInfoTxt;

        private static ReadOnlyTextBox
            _rcLengthTxt,
            _rcWidthTxt,
            _rcThickTxt,
            _rcVolTxt,
            _rcAreaTxt,
            _rcPerimTxt,
            _rcAsymTxt,
            _rcAsymStrTxt,
            _rcQtyOfTxt,
            _rcQtyTotalTxt,
            _rcNumChangesTxt,
            _rcParentTxt;


        private static ChildBox _rcChildList;

        private static CheckBox _rcIsSweepChk,
            _rcIsiMirChk,
            _rcHasHolesChk,
            _txDirUnknown,
            _txDirNone,
            _txDirHor,
            _txDirVer,
            _prodUnkown,
            _prodS4S,
            _prodMOne,
            _prodMMany;

        private static Button _travButton, _selParent, _selChildren, _updChildren;

        private static StatusStrip _stStrip;
        private static Panel _btPanel;

        private static ToolStripLabel _stText, _reqUpdate;
        private static UserControl _palPanel;
        private static bool _palDisabled;
        private readonly string _palName = "Metrics";

        /// <summary>
        /// </summary>
        [CommandMethod(SettingsInternal.CommandGroup, "_RCMETPAL",
            CommandFlags.Modal
            //| CommandFlags.Transparent
            //| CommandFlags.UsePickSet
            //| CommandFlags.Redraw
            //| CommandFlags.NoPerspective
            //| CommandFlags.NoMultiple
            //| CommandFlags.NoTileMode
            //| CommandFlags.NoPaperSpace
            //| CommandFlags.NoOem
            //| CommandFlags.Undefined
            //| CommandFlags.InProgress
            //| CommandFlags.Defun
            //| CommandFlags.NoNewStack
            //| CommandFlags.NoInternalLock
            //| CommandFlags.DocReadLock
            //| CommandFlags.DocExclusiveLock
            //| CommandFlags.Session
            //| CommandFlags.Interruptible
            //| CommandFlags.NoHistory
            //| CommandFlags.NoUndoMarker
            //| CommandFlags.NoBlockEditor
            //| CommandFlags.NoActionRecording
            //| CommandFlags.ActionMacro
            //| CommandFlags.NoInferConstraint
        )]
        public void Cmd_RcMetPal()
        {
            CreatePal();
        }

        /// <summary>
        ///     TODO
        /// </summary>
        /// <param name="objIds"></param>
        /// <param name="acCurDb"></param>
        internal static void ParseAndFill(ObjectId[] objIds, Database acCurDb)
        {
            _ignoreTextChange = true;

            var objCount = objIds.Length;

            switch (objCount)
            {
                case 0:

                    ClearInformation();
                    break;

                case 1:

                    ParseSingleObject(objIds, acCurDb);
                    break;

                default:

                    ParseManyObjects(objIds, acCurDb);
                    break;
            }

            _ignoreTextChange = false;
        }

        private static void ClearInformation(bool clearStatus = true)
        {
            if (clearStatus)
                _stText.Text = "No Objects Selected";

            ClearUpdateIcon();
            ClearText(_rcNameTxt);
            ClearText(_rcInfoTxt);
            ClearText(_rcQtyOfTxt);
            ClearText(_rcQtyTotalTxt);
            ClearText(_rcLengthTxt);
            ClearText(_rcWidthTxt);
            ClearText(_rcThickTxt);
            ClearText(_rcVolTxt);
            ClearText(_rcAreaTxt);
            ClearText(_rcPerimTxt);
            ClearText(_rcAsymTxt);
            ClearText(_rcAsymStrTxt);
            ClearText(_rcNumChangesTxt);
            ClearText(_rcParentTxt);

            _rcChildList.Items.Clear();

            _txDirUnknown.Checked = false;
            _txDirNone.Checked = false;
            _txDirHor.Checked = false;
            _txDirVer.Checked = false;

            _prodUnkown.Checked = false;
            _prodS4S.Checked = false;
            _prodMOne.Checked = false;
            _prodMMany.Checked = false;

            _rcIsSweepChk.Checked = false;
            _rcIsiMirChk.Checked = false;
            _rcHasHolesChk.Checked = false;
        }

        /// <summary>
        ///     TODO
        /// </summary>
        private static void VaryInformation()
        {
            ClearUpdateIcon();
            VaryText(_rcNameTxt);
            VaryText(_rcInfoTxt);
            VaryText(_rcQtyOfTxt);
            VaryText(_rcQtyTotalTxt);
            VaryText(_rcLengthTxt);
            VaryText(_rcWidthTxt);
            VaryText(_rcThickTxt);
            VaryText(_rcVolTxt);
            VaryText(_rcAreaTxt);
            VaryText(_rcPerimTxt);
            VaryText(_rcAsymTxt);
            VaryText(_rcAsymStrTxt);
            VaryText(_rcNumChangesTxt);
            VaryText(_rcParentTxt);

            _rcChildList.Items.Clear();
            var lItem = new ListBoxItem("*VARIES*", "VAR");
            _rcChildList.Items.Add(lItem);

            _txDirUnknown.Checked = false;
            _txDirNone.Checked = false;
            _txDirHor.Checked = false;
            _txDirVer.Checked = false;

            _prodUnkown.Checked = false;
            _prodS4S.Checked = false;
            _prodMOne.Checked = false;
            _prodMMany.Checked = false;

            _rcIsSweepChk.Checked = false;
            _rcIsiMirChk.Checked = false;
            _rcHasHolesChk.Checked = false;
        }

        /// <summary>
        ///     TODO
        /// </summary>
        /// <param name="objIds"></param>
        /// <param name="acCurDb"></param>
        private static void ParseSingleObject(ObjectId[] objIds, Database acCurDb)
        {
            using (var acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                var acEnt = acTrans.GetObject(objIds[0], OpenMode.ForRead) as Entity;

                if (acEnt != null)
                {
                    var idStr = acEnt.Id.ToString();
                    idStr = idStr.Replace("(", string.Empty);
                    idStr = idStr.Replace(")", string.Empty);
                    _stText.Text = acEnt.Id.ObjectClass.DxfName + @" #" + idStr;

                    if (acEnt is BlockReference acBref)
                    {
                        AddText(_rcNameTxt, acBref.Name);
                    }
                    else if (acEnt.HasXData())
                    {
                        //Check for changes
                        var acSol = acEnt as Solid3d;
                        if (acSol != null)
                        {
                            if (acSol.NumChanges != acEnt.GetNumChanges())
                                ShowUpdateIcon();
                            else
                                ClearUpdateIcon();
                        }

                        AddText(_rcNameTxt, acEnt.GetPartName());
                        AddText(_rcInfoTxt, acEnt.GetPartInfo());
                        AddText(_rcQtyOfTxt, acEnt.GetQtyOf().ToString());
                        AddText(_rcQtyTotalTxt, acEnt.GetQtyTotal().ToString());
                        AddText(_rcLengthTxt, acCurDb.ConvertToDwgUnits(acEnt.GetPartLength()));
                        AddText(_rcWidthTxt, acCurDb.ConvertToDwgUnits(acEnt.GetPartWidth()));
                        AddText(_rcThickTxt, acCurDb.ConvertToDwgUnits(acEnt.GetPartThickness()));
                        AddText(_rcVolTxt, acEnt.GetPartVolume().ToString());
                        AddText(_rcAreaTxt, acEnt.GetPartArea().ToString());
                        AddText(_rcPerimTxt, acEnt.GetPartPerimeter().ToString());
                        AddText(_rcAsymTxt, acEnt.GetPartAsymmetry().ToString());
                        AddText(_rcAsymStrTxt, acEnt.GetAsymVector());
                        AddText(_rcNumChangesTxt, acEnt.GetNumChanges().ToString());
                        AddText(_rcParentTxt, acEnt.GetParent().ToString());
                        _rcChildList.Items.Clear();

                        //Fill Child List Pane
                        var cHandles = acEnt.GetChildren();

                        if (cHandles.Count > 0)
                            foreach (var cH in cHandles)
                                _rcChildList.Items.Add(new ListBoxItem(cH.ToString(), cH.ToString()));

                        var txDir = acEnt.GetTextureDirection();

                        _txDirUnknown.Checked = false;
                        _txDirNone.Checked = false;
                        _txDirHor.Checked = false;
                        _txDirVer.Checked = false;

                        switch (txDir)
                        {
                            case Enums.TextureDirection.Unknown:
                                _txDirUnknown.Checked = true;
                                break;
                            case Enums.TextureDirection.None:
                                _txDirNone.Checked = true;
                                break;
                            case Enums.TextureDirection.Along:
                                _txDirHor.Checked = true;
                                break;
                            case Enums.TextureDirection.Across:
                                _txDirVer.Checked = true;
                                break;
                            default:
                                _txDirUnknown.Checked = true;
                                break;
                        }

                        var prodType = acEnt.GetProductionType();

                        _prodUnkown.Checked = false;
                        _prodS4S.Checked = false;
                        _prodMOne.Checked = false;
                        _prodMMany.Checked = false;

                        switch (prodType)
                        {
                            case Enums.ProductionType.Unknown:
                                _prodUnkown.Checked = true;
                                break;
                            case Enums.ProductionType.Saw:
                                _prodS4S.Checked = true;
                                break;
                            case Enums.ProductionType.MillingOneSide:
                                _prodMOne.Checked = true;
                                break;
                            case Enums.ProductionType.MillingTwoSide:
                                _prodMMany.Checked = true;
                                break;
                            case Enums.ProductionType.Box:
                                _prodS4S.Checked = true;
                                break;
                            case Enums.ProductionType.Sweep:
                                _prodUnkown.Checked = true;
                                break;
                            default:
                                _prodUnkown.Checked = true;
                                break;
                        }

                        _rcIsSweepChk.Checked = acEnt.GetIsSweep();
                        _rcIsiMirChk.Checked = acEnt.GetIsMirror();
                        _rcHasHolesChk.Checked = acEnt.GetHasHoles();
                    }
                    else
                    {
                        ClearInformation(false);
                    }
                }

                acTrans.Abort();
            }
        }

        /// <summary>
        ///     TODO
        /// </summary>
        /// <param name="objIds"></param>
        /// <param name="acCurDb"></param>
        private static void ParseManyObjects(ObjectId[] objIds, Database acCurDb)
        {
            if (objIds.Length > 1)
            {
                _stText.Text = objIds.Length + " Objects Selected";

                using (var acTrans = acCurDb.TransactionManager.StartTransaction())
                {
                    var nameList = new List<string>();
                    var infoList = new List<string>();
                    var qtyOfList = new List<string>();
                    var qtyTotList = new List<string>();
                    var lengthList = new List<string>();
                    var widthList = new List<string>();
                    var thickList = new List<string>();
                    var volList = new List<string>();
                    var areaList = new List<string>();
                    var perimList = new List<string>();
                    var asymList = new List<string>();
                    var asymStrList = new List<string>();
                    var numChangesList = new List<string>();
                    var parentList = new List<string>();
                    _ = new List<List<Handle>>();
                    var txDirList = new List<Enums.TextureDirection>();
                    var prodTypeList = new List<Enums.ProductionType>();
                    var isSweepList = new List<bool>();
                    var isMirList = new List<bool>();
                    var hasHolesList = new List<bool>();

                    foreach (var objId in objIds)
                    {
                        var acEnt = acTrans.GetObject(objId, OpenMode.ForRead) as Entity;
                        if (acEnt == null)
                        {
                            VaryInformation();
                            break;
                        }

                        nameList.Add(acEnt.GetPartName());
                        infoList.Add(acEnt.GetPartInfo());
                        qtyOfList.Add(acEnt.GetQtyOf().ToString());
                        qtyTotList.Add(acEnt.GetQtyTotal().ToString());
                        lengthList.Add(acEnt.GetPartLength().ToString());
                        widthList.Add(acEnt.GetPartWidth().ToString());
                        thickList.Add(acEnt.GetPartThickness().ToString());
                        volList.Add(acEnt.GetPartVolume().ToString());
                        areaList.Add(acEnt.GetPartArea().ToString());
                        perimList.Add(acEnt.GetPartPerimeter().ToString());
                        asymList.Add(acEnt.GetPartAsymmetry().ToString());
                        asymStrList.Add(acEnt.GetAsymVector());
                        numChangesList.Add(acEnt.GetNumChanges().ToString());
                        parentList.Add(acEnt.GetParent().ToString());
                        //childList.Add(acEnt.GetChildren());
                        txDirList.Add(acEnt.GetTextureDirection());
                        prodTypeList.Add(acEnt.GetProductionType());
                        isSweepList.Add(acEnt.GetIsSweep());
                        isMirList.Add(acEnt.GetIsMirror());
                        hasHolesList.Add(acEnt.GetHasHoles());
                    }

                    var varyText = "*VARIES*";

                    //Check list sizes
                    _rcNameTxt.Text = nameList.Distinct().Count() == 1 ? nameList.First() : varyText;
                    _rcInfoTxt.Text = infoList.Distinct().Count() == 1 ? infoList.First() : varyText;
                    _rcQtyOfTxt.Text = qtyOfList.Distinct().Count() == 1 ? qtyOfList.First() : varyText;
                    _rcQtyTotalTxt.Text = qtyTotList.Distinct().Count() == 1 ? qtyTotList.First() : varyText;
                    _rcLengthTxt.Text = lengthList.Distinct().Count() == 1 ? lengthList.First() : varyText;
                    _rcWidthTxt.Text = widthList.Distinct().Count() == 1 ? widthList.First() : varyText;
                    _rcThickTxt.Text = thickList.Distinct().Count() == 1 ? thickList.First() : varyText;
                    _rcVolTxt.Text = volList.Distinct().Count() == 1 ? volList.First() : varyText;
                    _rcAreaTxt.Text = areaList.Distinct().Count() == 1 ? areaList.First() : varyText;
                    _rcPerimTxt.Text = perimList.Distinct().Count() == 1 ? perimList.First() : varyText;
                    _rcAsymTxt.Text = asymList.Distinct().Count() == 1 ? asymList.First() : varyText;
                    _rcAsymStrTxt.Text = asymStrList.Distinct().Count() == 1 ? asymStrList.First() : varyText;
                    _rcNumChangesTxt.Text =
                        numChangesList.Distinct().Count() == 1 ? numChangesList.First() : varyText;
                    _rcParentTxt.Text = parentList.Distinct().Count() == 1 ? parentList.First() : varyText;

                    _rcChildList.Items.Clear();

                    //if (childList.AreListsSame())
                    //{
                    //    var cHandles = childList.First();

                    //    if (cHandles.Count > 0)
                    //        foreach (var cH in cHandles)
                    //            _rcChildList.Items.Add(new ListBoxItem(cH.ToString(), cH.ToString()));
                    //}
                    //else
                    //{
                    //    _rcChildList.Items.Add(new ListBoxItem("*VARIES*", "VAR"));
                    //}

                    _txDirUnknown.Checked = false;
                    _txDirNone.Checked = false;
                    _txDirHor.Checked = false;
                    _txDirVer.Checked = false;

                    if (txDirList.Distinct().Count() == 1)
                        switch (txDirList.First())
                        {
                            case Enums.TextureDirection.Unknown:
                                _txDirUnknown.Checked = true;
                                break;
                            case Enums.TextureDirection.None:
                                _txDirNone.Checked = true;
                                break;
                            case Enums.TextureDirection.Along:
                                _txDirHor.Checked = true;
                                break;
                            case Enums.TextureDirection.Across:
                                _txDirVer.Checked = true;
                                break;
                            default:
                                _txDirUnknown.Checked = true;
                                break;
                        }

                    _prodUnkown.Checked = false;
                    _prodS4S.Checked = false;
                    _prodMOne.Checked = false;
                    _prodMMany.Checked = false;

                    if (prodTypeList.Distinct().Count() == 1)
                        switch (prodTypeList.First())
                        {
                            case Enums.ProductionType.Unknown:
                                _prodUnkown.Checked = true;
                                break;
                            case Enums.ProductionType.Saw:
                                _prodS4S.Checked = true;
                                break;
                            case Enums.ProductionType.MillingOneSide:
                                _prodMOne.Checked = true;
                                break;
                            case Enums.ProductionType.MillingTwoSide:
                                _prodMMany.Checked = true;
                                break;
                            case Enums.ProductionType.Box:
                                _prodS4S.Checked = true;
                                break;
                            case Enums.ProductionType.Sweep:
                                _prodUnkown.Checked = true;
                                break;
                            default:
                                _prodUnkown.Checked = true;
                                break;
                        }

                    if (isSweepList.Distinct().Count() == 1)
                        _rcIsSweepChk.Checked = isSweepList.First();
                    else
                        _rcIsSweepChk.CheckState = CheckState.Indeterminate;

                    if (isMirList.Distinct().Count() == 1)
                        _rcIsiMirChk.Checked = isMirList.First();
                    else
                        _rcIsiMirChk.CheckState = CheckState.Indeterminate;

                    if (hasHolesList.Distinct().Count() == 1)
                        _rcHasHolesChk.Checked = hasHolesList.First();
                    else
                        _rcHasHolesChk.CheckState = CheckState.Indeterminate;


                    acTrans.Abort();
                }
            }
        }


        private static void ShowUpdateIcon()
        {
            _reqUpdate.Text = "!!!";
        }

        private static void ClearUpdateIcon()
        {
            _reqUpdate.Text = string.Empty;
        }

        #region Pal Initialization

        /// <summary>
        ///     TODO
        /// </summary>
        private void CreatePal()
        {
            if (RcPal == null)
            {
                RcPal = new PaletteSet(_palName, new Guid("EB7074AA-0181-4871-9803-994477136E69"))
                {
                    Style = PaletteSetStyles.ShowPropertiesMenu
                            | PaletteSetStyles.ShowAutoHideButton
                            | PaletteSetStyles.ShowCloseButton
                };

                _palPanel = new UserControl();

                PopulatePal();
                //_palPanel.UpdateTheme();
                RcPal.Add(_palName, _palPanel);
            }

            RcPal.Visible = true;
        }

        /// <summary>
        ///     TODO
        /// </summary>
        private void PopulatePal()
        {
            _palPanel.Controls.Clear();

            var rowCount = 0;

            _ = Colors.GetCadBackColor();
            var foreColor = Colors.GetCadForeColor();
            var entryColor = Colors.GetCadEntryColor();
            var textColor = Colors.GetCadTextColor();

            _palPanel.BackColor = foreColor;
            _palPanel.ForeColor = foreColor;

            _tbLayout = new TableLayoutPanel
            {
                AutoScroll = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                BackColor = foreColor,
                ForeColor = foreColor,
                ColumnCount = 3,
                Dock = DockStyle.Fill
            };

            _tbLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70F));
            _tbLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _tbLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 5F));

            _rcNameLab = new Label
            {
                Text = "Name:",
                TextAlign = ContentAlignment.MiddleLeft,
                Anchor = AnchorStyles.None,
                BackColor = foreColor,
                ForeColor = textColor
            };
            _rcInfoLab = new Label
            {
                Text = "Info:",
                TextAlign = ContentAlignment.MiddleLeft,
                Anchor = AnchorStyles.None,
                BackColor = foreColor,
                ForeColor = textColor
            };
            _rcLengthLab = new Label
            {
                Text = "Length:",
                TextAlign = ContentAlignment.MiddleLeft,
                Anchor = AnchorStyles.None,
                BackColor = foreColor,
                ForeColor = textColor
            };
            _rcWidthLab = new Label
            {
                Text = "Width:",
                TextAlign = ContentAlignment.MiddleLeft,
                Anchor = AnchorStyles.None,
                BackColor = foreColor,
                ForeColor = textColor
            };
            _rcThickLab = new Label
            {
                Text = "Thickness:",
                TextAlign = ContentAlignment.MiddleLeft,
                Anchor = AnchorStyles.None,
                BackColor = foreColor,
                ForeColor = textColor
            };
            _rcVolLab = new Label
            {
                Text = "Volume:",
                TextAlign = ContentAlignment.MiddleLeft,
                Anchor = AnchorStyles.None,
                BackColor = foreColor,
                ForeColor = textColor
            };
            _rcAreaLab = new Label
            {
                Text = "Area:",
                TextAlign = ContentAlignment.MiddleLeft,
                Anchor = AnchorStyles.None,
                BackColor = foreColor,
                ForeColor = textColor
            };
            _rcPerimLab = new Label
            {
                Text = "Perimeter:",
                TextAlign = ContentAlignment.MiddleLeft,
                Anchor = AnchorStyles.None,
                BackColor = foreColor,
                ForeColor = textColor
            };
            _rcAsymLab = new Label
            {
                Text = "Asym:",
                TextAlign = ContentAlignment.MiddleLeft,
                Anchor = AnchorStyles.None,
                BackColor = foreColor,
                ForeColor = textColor
            };
            _rcAsymStrLab = new Label
            {
                Text = "Asym V:",
                TextAlign = ContentAlignment.MiddleLeft,
                Anchor = AnchorStyles.None,
                BackColor = foreColor,
                ForeColor = textColor
            };
            _rcQtyOfLab = new Label
            {
                Text = "Qty Of:",
                TextAlign = ContentAlignment.MiddleLeft,
                Anchor = AnchorStyles.None,
                BackColor = foreColor,
                ForeColor = textColor
            };
            _rcQtyTotalLab = new Label
            {
                Text = "Qty Total:",
                TextAlign = ContentAlignment.MiddleLeft,
                Anchor = AnchorStyles.None,
                BackColor = foreColor,
                ForeColor = textColor
            };
            _rcNumChangesLab = new Label
            {
                Text = "Changes:",
                TextAlign = ContentAlignment.MiddleLeft,
                Anchor = AnchorStyles.None,
                BackColor = foreColor,
                ForeColor = textColor
            };
            _rcParentLab = new Label
            {
                Text = "Parent: ",
                TextAlign = ContentAlignment.MiddleLeft,
                Anchor = AnchorStyles.None,
                BackColor = foreColor,
                ForeColor = textColor
            };
            _rcChildLab = new Label
            {
                Text = "Children: ",
                TextAlign = ContentAlignment.TopLeft,
                Anchor = AnchorStyles.None,
                BackColor = foreColor,
                ForeColor = textColor
            };
            _rcTxDirLab = new Label
            {
                Text = "Texture: ",
                TextAlign = ContentAlignment.TopLeft,
                Anchor = AnchorStyles.None,
                BackColor = foreColor,
                ForeColor = textColor
            };
            _prodTypLab = new Label
            {
                Text = "Production: ",
                TextAlign = ContentAlignment.TopLeft,
                Anchor = AnchorStyles.None,
                BackColor = foreColor,
                ForeColor = textColor
            };

            _stText = new ToolStripLabel {Text = "No Objects Selected", BackColor = foreColor, ForeColor = textColor};
            _reqUpdate = new ToolStripLabel {Text = string.Empty, BackColor = entryColor, ForeColor = textColor};

            _rcNameTxt = new EntryBox {Dock = DockStyle.Fill, BackColor = entryColor, ForeColor = textColor};
            _rcNameTxt.TextChanged += name_TextChanged;

            _rcInfoTxt = new EntryBox
            {
                Dock = DockStyle.Fill,
                WordWrap = true,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                BackColor = entryColor,
                ForeColor = textColor
            };
            _rcInfoTxt.TextChanged += info_TextChanged;

            _rcLengthTxt = new ReadOnlyTextBox
                {Dock = DockStyle.Fill, ReadOnly = true, BackColor = entryColor, ForeColor = textColor};
            _rcWidthTxt = new ReadOnlyTextBox
                {Dock = DockStyle.Fill, ReadOnly = true, BackColor = entryColor, ForeColor = textColor};
            _rcThickTxt = new ReadOnlyTextBox
                {Dock = DockStyle.Fill, ReadOnly = true, BackColor = entryColor, ForeColor = textColor};
            _rcVolTxt = new ReadOnlyTextBox
                {Dock = DockStyle.Fill, ReadOnly = true, BackColor = entryColor, ForeColor = textColor};
            _rcAreaTxt = new ReadOnlyTextBox
                {Dock = DockStyle.Fill, ReadOnly = true, BackColor = entryColor, ForeColor = textColor};
            _rcPerimTxt = new ReadOnlyTextBox
                {Dock = DockStyle.Fill, ReadOnly = true, BackColor = entryColor, ForeColor = textColor};
            _rcAsymTxt = new ReadOnlyTextBox
                {Dock = DockStyle.Fill, ReadOnly = true, BackColor = entryColor, ForeColor = textColor};
            _rcAsymStrTxt = new ReadOnlyTextBox
                {Dock = DockStyle.Fill, ReadOnly = true, BackColor = entryColor, ForeColor = textColor};
            _rcQtyOfTxt = new ReadOnlyTextBox
                {Dock = DockStyle.Fill, ReadOnly = true, BackColor = entryColor, ForeColor = textColor};
            _rcQtyTotalTxt = new ReadOnlyTextBox
                {Dock = DockStyle.Fill, ReadOnly = true, BackColor = entryColor, ForeColor = textColor};
            _rcNumChangesTxt = new ReadOnlyTextBox
                {Dock = DockStyle.Fill, ReadOnly = true, BackColor = entryColor, ForeColor = textColor};
            _rcParentTxt = new ReadOnlyTextBox
                {Dock = DockStyle.Fill, ReadOnly = true, BackColor = entryColor, ForeColor = textColor};

            _rcChildList = new ChildBox {Dock = DockStyle.Fill, BackColor = entryColor, ForeColor = textColor};
            _rcChildList.Sorted = true;
            _rcChildList.DisplayMember = "Text";
            _rcChildList.MouseDoubleClick += childList_MouseDoubleClick;

            _rcIsSweepChk = new CheckBox
            {
                Text = "Is Sweep",
                Dock = DockStyle.Fill,
                AutoSize = false,
                BackColor = foreColor,
                ForeColor = textColor
            };

            _rcIsSweepChk.Click += sweep_CheckClick;

            _rcIsiMirChk = new CheckBox
            {
                Text = "Is Mirror",
                Dock = DockStyle.Fill,
                AutoSize = false,
                BackColor = foreColor,
                ForeColor = textColor
            };

            _rcIsiMirChk.Click += mirror_CheckClick;

            _rcHasHolesChk = new CheckBox
            {
                Text = "Has Holes",
                Dock = DockStyle.Fill,
                AutoSize = false,
                BackColor = foreColor,
                ForeColor = textColor
            };

            _rcHasHolesChk.Click += holes_CheckClick;

            _txDirUnknown = new CheckBox
            {
                Appearance = Appearance.Button,
                Dock = DockStyle.Fill,
                BackColor = entryColor,
                ForeColor = textColor,
                Image = Properties.Resources.TxUnknoen16X16__I_,
                ImageAlign = ContentAlignment.MiddleCenter
            };

            _txDirUnknown.Click += texture_CheckClick;

            _txDirNone = new CheckBox
            {
                Appearance = Appearance.Button,
                Dock = DockStyle.Fill,
                BackColor = entryColor,
                ForeColor = textColor,
                Image = Properties.Resources.None16X16__I_,
                ImageAlign = ContentAlignment.MiddleCenter
            };

            _txDirNone.Click += texture_CheckClick;

            _txDirHor = new CheckBox
            {
                Appearance = Appearance.Button,
                Dock = DockStyle.Fill,
                BackColor = entryColor,
                ForeColor = textColor,
                Image = Properties.Resources.TxAlong16X16__I_,
                ImageAlign = ContentAlignment.MiddleCenter
            };

            _txDirHor.Click += texture_CheckClick;

            _txDirVer = new CheckBox
            {
                Appearance = Appearance.Button,
                Dock = DockStyle.Fill,
                BackColor = entryColor,
                ForeColor = textColor,
                Image = Properties.Resources.TxAcross16X16__I_,
                ImageAlign = ContentAlignment.MiddleCenter
            };

            _txDirVer.Click += texture_CheckClick;

            var txLayout = new TableLayoutPanel
            {
                AutoScroll = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                BackColor = foreColor,
                ForeColor = foreColor,
                ColumnCount = 4,
                RowCount = 1,
                Dock = DockStyle.Fill
            };

            txLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            txLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            txLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            txLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            txLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            txLayout.Controls.Add(_txDirUnknown, 0, 0);
            txLayout.Controls.Add(_txDirNone, 1, 0);
            txLayout.Controls.Add(_txDirHor, 2, 0);
            txLayout.Controls.Add(_txDirVer, 3, 0);

            _txDirUnknown.FlatStyle = FlatStyle.Flat;
            _txDirUnknown.FlatAppearance.BorderColor = Colors.GetCadBorderColor();
            _txDirUnknown.FlatAppearance.BorderSize = 1;

            _txDirNone.FlatStyle = FlatStyle.Flat;
            _txDirNone.FlatAppearance.BorderColor = Colors.GetCadBorderColor();
            _txDirNone.FlatAppearance.BorderSize = 1;

            _txDirHor.FlatStyle = FlatStyle.Flat;
            _txDirHor.FlatAppearance.BorderColor = Colors.GetCadBorderColor();
            _txDirHor.FlatAppearance.BorderSize = 1;

            _txDirVer.FlatStyle = FlatStyle.Flat;
            _txDirVer.FlatAppearance.BorderColor = Colors.GetCadBorderColor();
            _txDirVer.FlatAppearance.BorderSize = 1;

            _prodUnkown = new CheckBox
            {
                Appearance = Appearance.Button,
                Dock = DockStyle.Fill,
                BackColor = entryColor,
                ForeColor = textColor,
                Image = Properties.Resources.TxUnknoen16X16__I_,
                ImageAlign = ContentAlignment.MiddleCenter
            };

            _prodUnkown.Click += prod_CheckClick;

            _prodS4S = new CheckBox
            {
                Dock = DockStyle.Fill,
                Appearance = Appearance.Button,
                BackColor = entryColor,
                ForeColor = textColor,
                Image = Properties.Resources.S4S16X16__I_,
                ImageAlign = ContentAlignment.MiddleCenter
            };

            _prodS4S.Click += prod_CheckClick;

            _prodMOne = new CheckBox
            {
                Appearance = Appearance.Button,
                Dock = DockStyle.Fill,
                BackColor = entryColor,
                ForeColor = textColor,
                Image = Properties.Resources.RcDogEar16X16__I_,
                ImageAlign = ContentAlignment.MiddleCenter
            };

            _prodMOne.Click += prod_CheckClick;

            _prodMMany = new CheckBox
            {
                Appearance = Appearance.Button,
                Dock = DockStyle.Fill,
                BackColor = entryColor,
                ForeColor = textColor,
                Image = Properties.Resources.ProdMultSide16X16__I_,
                ImageAlign = ContentAlignment.MiddleCenter
            };

            _prodMMany.Click += prod_CheckClick;

            var prodLayout = new TableLayoutPanel
            {
                AutoScroll = false,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                BackColor = foreColor,
                ForeColor = foreColor,
                ColumnCount = 4,
                RowCount = 1,
                Dock = DockStyle.Fill
            };

            prodLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            prodLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            prodLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            prodLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            prodLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            prodLayout.Controls.Add(_prodUnkown, 0, 0);
            prodLayout.Controls.Add(_prodS4S, 1, 0);
            prodLayout.Controls.Add(_prodMOne, 2, 0);
            prodLayout.Controls.Add(_prodMMany, 3, 0);

            _prodUnkown.FlatStyle = FlatStyle.Flat;
            _prodUnkown.FlatAppearance.BorderColor = Colors.GetCadBorderColor();
            _prodUnkown.FlatAppearance.BorderSize = 1;

            _prodS4S.FlatStyle = FlatStyle.Flat;
            _prodS4S.FlatAppearance.BorderColor = Colors.GetCadBorderColor();
            _prodS4S.FlatAppearance.BorderSize = 1;

            _prodMOne.FlatStyle = FlatStyle.Flat;
            _prodMOne.FlatAppearance.BorderColor = Colors.GetCadBorderColor();
            _prodMOne.FlatAppearance.BorderSize = 1;

            _prodMMany.FlatStyle = FlatStyle.Flat;
            _prodMMany.FlatAppearance.BorderColor = Colors.GetCadBorderColor();
            _prodMMany.FlatAppearance.BorderSize = 1;

            _travButton = new Button
                {Text = "TR", Dock = DockStyle.Fill, BackColor = entryColor, ForeColor = textColor};
            _travButton.Click += traverse_Click;

            _selParent = new Button
                {Text = "SP", Dock = DockStyle.Fill, BackColor = entryColor, ForeColor = textColor};
            _selParent.Click += selParent_Click;

            _selChildren = new Button
                {Text = "SC", Dock = DockStyle.Fill, BackColor = entryColor, ForeColor = textColor};
            _selChildren.Click += selChildren_Click;

            _updChildren = new Button
                {Text = "UC", Dock = DockStyle.Fill, BackColor = entryColor, ForeColor = textColor};
            _updChildren.Click += updChildren_Click;

            _btPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = CtrlHeight,
                AutoSize = false,
                BackColor = foreColor,
                ForeColor = textColor
            };
            var btLayout = new TableLayoutPanel
            {
                AutoScroll = false,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                BackColor = foreColor,
                ForeColor = foreColor,
                ColumnCount = 4,
                RowCount = 1,
                Dock = DockStyle.Fill
            };

            btLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            btLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            btLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            btLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            btLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, CtrlHeight + 5));

            btLayout.Controls.Add(_travButton, 0, 0);
            btLayout.Controls.Add(_selParent, 1, 0);
            btLayout.Controls.Add(_selChildren, 2, 0);
            btLayout.Controls.Add(_updChildren, 3, 0);

            _travButton.FlatStyle = FlatStyle.Flat;
            _travButton.FlatAppearance.BorderColor = Colors.GetCadBorderColor();
            _travButton.FlatAppearance.BorderSize = 1;
            _travButton.Text = "";
            _travButton.ImageAlign = ContentAlignment.MiddleCenter;
            _travButton.Image = Properties.Resources.RcTraverse16X16__I_;

            _selParent.FlatStyle = FlatStyle.Flat;
            _selParent.FlatAppearance.BorderColor = Colors.GetCadBorderColor();
            _selParent.FlatAppearance.BorderSize = 1;
            _selParent.Text = "";
            _selParent.ImageAlign = ContentAlignment.MiddleCenter;
            _selParent.Image = Properties.Resources.UpParent16X16__I_;

            _selChildren.FlatStyle = FlatStyle.Flat;
            _selChildren.FlatAppearance.BorderColor = Colors.GetCadBorderColor();
            _selChildren.FlatAppearance.BorderSize = 1;
            _selChildren.Text = "";
            _selChildren.ImageAlign = ContentAlignment.MiddleCenter;
            _selChildren.Image = Properties.Resources.RcSelection16X16__I_;

            _updChildren.FlatStyle = FlatStyle.Flat;
            _updChildren.FlatAppearance.BorderColor = Colors.GetCadBorderColor();
            _updChildren.FlatAppearance.BorderSize = 1;
            _updChildren.Text = "";
            _updChildren.ImageAlign = ContentAlignment.MiddleCenter;
            _updChildren.Image = Properties.Resources.RcUpdateChildren16X16__I_;

            _btPanel.Controls.Add(btLayout);

            _stStrip = new StatusStrip {Dock = DockStyle.Bottom, BackColor = foreColor, ForeColor = textColor};
            _stStrip.Items.Add(_reqUpdate);
            _stStrip.Items.Add(_stText);

            #region AddInfoToTable

            AddToTable(_rcNameLab, _rcNameTxt, CtrlHeight, ref rowCount);
            AddToTable(_rcInfoLab, _rcInfoTxt, CtrlHeight * 2, ref rowCount);
            AddToTable(_rcQtyOfLab, _rcQtyOfTxt, CtrlHeight, ref rowCount);
            AddToTable(_rcQtyTotalLab, _rcQtyTotalTxt, CtrlHeight, ref rowCount);
            AddToTable(_rcLengthLab, _rcLengthTxt, CtrlHeight, ref rowCount);
            AddToTable(_rcWidthLab, _rcWidthTxt, CtrlHeight, ref rowCount);
            AddToTable(_rcThickLab, _rcThickTxt, CtrlHeight, ref rowCount);
            AddToTable(_rcVolLab, _rcVolTxt, CtrlHeight, ref rowCount);
            AddToTable(_rcAreaLab, _rcAreaTxt, CtrlHeight, ref rowCount);
            AddToTable(_rcPerimLab, _rcPerimTxt, CtrlHeight, ref rowCount);
            AddToTable(_rcAsymLab, _rcAsymTxt, CtrlHeight, ref rowCount);
            AddToTable(_rcAsymStrLab, _rcAsymStrTxt, CtrlHeight, ref rowCount);
            AddToTable(_rcNumChangesLab, _rcNumChangesTxt, CtrlHeight, ref rowCount);

            AddToTable(_rcParentLab, _rcParentTxt, CtrlHeight, ref rowCount);
            AddToTable(_rcChildLab, _rcChildList, CtrlHeight * 5, ref rowCount);

            AddToTable(_rcTxDirLab, txLayout, CtrlHeight + 10, ref rowCount);
            AddToTable(_prodTypLab, prodLayout, CtrlHeight + 10, ref rowCount);

            AddToTable(new Label(), _rcIsSweepChk, CtrlHeight, ref rowCount);
            AddToTable(new Label(), _rcIsiMirChk, CtrlHeight, ref rowCount);
            AddToTable(new Label(), _rcHasHolesChk, CtrlHeight, ref rowCount);

            AddToTable(new Label(), new Label(), CtrlHeight, ref rowCount);

            #endregion

            _palPanel.Controls.Add(_tbLayout);
            _palPanel.Controls.Add(_stStrip);
            _palPanel.Controls.Add(_btPanel);
        }

        internal static void DisablePal()
        {
            _stText.Text = "PANEL HAS BEEN DISABLED";
            _palDisabled = true;

            foreach (Control ctr in _palPanel.Controls) ctr.Enabled = false;
        }

        internal static void EnablePal()
        {
            if (_palDisabled != true) return;

            _palDisabled = false;
            _stText.Text = "PANEL HAS BEEN ENABLED";

            foreach (Control ctr in _palPanel.Controls) ctr.Enabled = true;
        }

        /// <summary>
        ///     TODO
        /// </summary>
        /// <param name="ctrl1"></param>
        /// <param name="ctr2"></param>
        /// <param name="ctHeight"></param>
        /// <param name="rowCount"></param>
        private void AddToTable(Control ctrl1, Control ctr2, float ctHeight, ref int rowCount)
        {
            if (ctrl1 == null) throw new ArgumentNullException(nameof(ctrl1));
            if (ctr2 == null) throw new ArgumentNullException(nameof(ctr2));

            _tbLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, ctHeight));
            _tbLayout.Controls.Add(ctrl1, LabColumn, rowCount);
            _tbLayout.Controls.Add(ctr2, InfoColumn, rowCount);
            rowCount++;
        }

        #endregion

        #region CheckHandling

        /// <summary>
        ///     TODO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void texture_CheckClick(object sender, EventArgs e)
        {
            var nonChecked = false;

            if (!(sender is CheckBox chk)) return;
            if (!chk.Checked) nonChecked = true;

            var acCurDoc = DocumentManager.MdiActiveDocument;
            if (acCurDoc == null) return;

            using (acCurDoc.LockDocument())
            {
                var acCurDb = acCurDoc.Database;
                var acCurEd = acCurDoc.Editor;

                var selRes = acCurEd.SelectImplied();

                if (selRes.Status == PromptStatus.OK)
                    using (var acTrans = acCurDb.TransactionManager.StartTransaction())
                    {
                        foreach (var id in selRes.Value.GetObjectIds())
                        {
                            var ent = acTrans.GetObject(id, OpenMode.ForWrite) as Entity;
                            if (ent != null)
                            {
                                if (chk == _txDirUnknown || nonChecked)
                                    ent.UpdateXData(
                                        Enums.TextureDirection.Unknown,
                                        Enums.XDataCode.TextureDirection,
                                        acCurDb,
                                        acTrans);
                                else if (chk == _txDirNone)
                                    ent.UpdateXData(
                                        Enums.TextureDirection.None,
                                        Enums.XDataCode.TextureDirection,
                                        acCurDb,
                                        acTrans);
                                else if (chk == _txDirHor)
                                    ent.UpdateXData(
                                        Enums.TextureDirection.Along,
                                        Enums.XDataCode.TextureDirection,
                                        acCurDb,
                                        acTrans);
                                else if (chk == _txDirVer)
                                    ent.UpdateXData(
                                        Enums.TextureDirection.Across,
                                        Enums.XDataCode.TextureDirection,
                                        acCurDb,
                                        acTrans);
                            }
                        }

                        acTrans.Commit();
                    }

                if (chk == _txDirUnknown || nonChecked)
                {
                    _txDirNone.Checked = false;
                    _txDirHor.Checked = false;
                    _txDirVer.Checked = false;

                    if (nonChecked)
                        _txDirUnknown.Checked = true;
                }
                else if (chk == _txDirNone)
                {
                    _txDirUnknown.Checked = false;
                    _txDirHor.Checked = false;
                    _txDirVer.Checked = false;
                }
                else if (chk == _txDirHor)
                {
                    _txDirNone.Checked = false;
                    _txDirUnknown.Checked = false;
                    _txDirVer.Checked = false;
                }
                else if (chk == _txDirVer)
                {
                    _txDirNone.Checked = false;
                    _txDirHor.Checked = false;
                    _txDirUnknown.Checked = false;
                }
            }
        }

        /// <summary>
        ///     TODO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void prod_CheckClick(object sender, EventArgs e)
        {
            var nonChecked = false;

            if (!(sender is CheckBox chk)) return;
            if (!chk.Checked) nonChecked = true;

            var acCurDoc = DocumentManager.MdiActiveDocument;
            if (acCurDoc == null) return;

            using (acCurDoc.LockDocument())
            {
                var acCurDb = acCurDoc.Database;
                var acCurEd = acCurDoc.Editor;

                var selRes = acCurEd.SelectImplied();

                if (selRes.Status == PromptStatus.OK)
                    using (var acTrans = acCurDb.TransactionManager.StartTransaction())
                    {
                        foreach (var id in selRes.Value.GetObjectIds())
                        {
                            var ent = acTrans.GetObject(id, OpenMode.ForWrite) as Entity;
                            if (ent != null)
                            {
                                if (chk == _prodUnkown || nonChecked)
                                    ent.UpdateXData(
                                        Enums.ProductionType.Unknown,
                                        Enums.XDataCode.ProductionType,
                                        acCurDb,
                                        acTrans);
                                else if (chk == _prodS4S)
                                    ent.UpdateXData(
                                        Enums.ProductionType.Saw,
                                        Enums.XDataCode.ProductionType,
                                        acCurDb,
                                        acTrans);
                                else if (chk == _prodMOne)
                                    ent.UpdateXData(
                                        Enums.ProductionType.MillingOneSide,
                                        Enums.XDataCode.ProductionType,
                                        acCurDb,
                                        acTrans);
                                else if (chk == _prodMMany)
                                    ent.UpdateXData(
                                        Enums.ProductionType.MillingTwoSide,
                                        Enums.XDataCode.ProductionType,
                                        acCurDb,
                                        acTrans);
                            }
                        }

                        acTrans.Commit();
                    }

                if (chk == _prodUnkown || nonChecked)
                {
                    _prodS4S.Checked = false;
                    _prodMOne.Checked = false;
                    _prodMMany.Checked = false;

                    if (nonChecked)
                        _prodUnkown.Checked = true;
                }
                else if (chk == _prodS4S)
                {
                    _prodUnkown.Checked = false;
                    _prodMOne.Checked = false;
                    _prodMMany.Checked = false;
                }
                else if (chk == _prodMOne)
                {
                    _prodS4S.Checked = false;
                    _prodUnkown.Checked = false;
                    _prodMMany.Checked = false;
                }
                else if (chk == _prodMMany)
                {
                    _prodS4S.Checked = false;
                    _prodMOne.Checked = false;
                    _prodUnkown.Checked = false;
                }
            }
        }

        /// <summary>
        ///     TODO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sweep_CheckClick(object sender, EventArgs e)
        {
            if (!(sender is CheckBox chk)) return;

            var acCurDoc = DocumentManager.MdiActiveDocument;
            if (acCurDoc == null) return;

            using (acCurDoc.LockDocument())
            {
                var acCurDb = acCurDoc.Database;
                var acCurEd = acCurDoc.Editor;

                var selRes = acCurEd.SelectImplied();

                if (selRes.Status != PromptStatus.OK) return;

                using (var acTrans = acCurDb.TransactionManager.StartTransaction())
                {
                    foreach (var id in selRes.Value.GetObjectIds())
                    {
                        var ent = acTrans.GetObject(id, OpenMode.ForWrite) as Entity;
                        if (ent != null)
                            ent.UpdateXData(
                                chk.Checked,
                                Enums.XDataCode.IsSweep,
                                acCurDb,
                                acTrans);
                    }

                    acTrans.Commit();
                }
            }
        }

        /// <summary>
        ///     TODO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mirror_CheckClick(object sender, EventArgs e)
        {
            if (!(sender is CheckBox chk)) return;

            var acCurDoc = DocumentManager.MdiActiveDocument;
            if (acCurDoc == null) return;

            using (acCurDoc.LockDocument())
            {
                var acCurDb = acCurDoc.Database;
                var acCurEd = acCurDoc.Editor;

                var selRes = acCurEd.SelectImplied();

                if (selRes.Status != PromptStatus.OK) return;

                using (var acTrans = acCurDb.TransactionManager.StartTransaction())
                {
                    foreach (var id in selRes.Value.GetObjectIds())
                    {
                        var ent = acTrans.GetObject(id, OpenMode.ForWrite) as Entity;
                        if (ent != null)
                            ent.UpdateXData(
                                chk.Checked,
                                Enums.XDataCode.IsMirror,
                                acCurDb,
                                acTrans);
                    }

                    acTrans.Commit();
                }
            }
        }

        /// <summary>
        ///     TODO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void holes_CheckClick(object sender, EventArgs e)
        {
            if (!(sender is CheckBox chk)) return;

            var acCurDoc = DocumentManager.MdiActiveDocument;
            if (acCurDoc == null) return;

            using (acCurDoc.LockDocument())
            {
                var acCurDb = acCurDoc.Database;
                var acCurEd = acCurDoc.Editor;

                var selRes = acCurEd.SelectImplied();

                if (selRes.Status != PromptStatus.OK) return;

                using (var acTrans = acCurDb.TransactionManager.StartTransaction())
                {
                    foreach (var id in selRes.Value.GetObjectIds())
                    {
                        var ent = acTrans.GetObject(id, OpenMode.ForWrite) as Entity;
                        if (ent != null)
                            ent.UpdateXData(
                                chk.Checked,
                                Enums.XDataCode.HasHoles,
                                acCurDb,
                                acTrans);
                    }

                    acTrans.Commit();
                }
            }
        }

        #endregion

        #region Text Handling

        /// <summary>
        ///     TODO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void name_TextChanged(object sender, EventArgs e)
        {
            if (_ignoreTextChange) return;

            if (_rcNameTxt.Text == SettingsInternal.VariesTxt) return;

            var acCurDoc = DocumentManager.MdiActiveDocument;
            if (acCurDoc == null) return;

            using (acCurDoc.LockDocument())
            {
                var acCurDb = acCurDoc.Database;
                var acCurEd = acCurDoc.Editor;

                var selRes = acCurEd.SelectImplied();

                if (selRes.Status != PromptStatus.OK) return;

                using (var acTrans = acCurDb.TransactionManager.StartTransaction())
                {
                    foreach (var id in selRes.Value.GetObjectIds())
                    {
                        var ent = acTrans.GetObject(id, OpenMode.ForWrite) as Entity;
                        if (ent != null) ent.UpdateXData(_rcNameTxt.Text, Enums.XDataCode.Name, acCurDb, acTrans);
                    }

                    acTrans.Commit();
                }
            }
        }

        /// <summary>
        ///     TODO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void info_TextChanged(object sender, EventArgs e)
        {
            if (_ignoreTextChange) return;

            if (_rcInfoTxt.Text == SettingsInternal.VariesTxt) return;

            var acCurDoc = DocumentManager.MdiActiveDocument;
            if (acCurDoc == null) return;

            using (acCurDoc.LockDocument())
            {
                var acCurDb = acCurDoc.Database;
                var acCurEd = acCurDoc.Editor;

                var selRes = acCurEd.SelectImplied();

                if (selRes.Status != PromptStatus.OK) return;

                using (var acTrans = acCurDb.TransactionManager.StartTransaction())
                {
                    foreach (var id in selRes.Value.GetObjectIds())
                    {
                        var ent = acTrans.GetObject(id, OpenMode.ForWrite) as Entity;
                        if (ent != null) ent.UpdateXData(_rcInfoTxt.Text, Enums.XDataCode.Info, acCurDb, acTrans);
                    }

                    acTrans.Commit();
                }
            }
        }

        /// <summary>
        ///     TODO
        /// </summary>
        /// <param name="txtBox"></param>
        private static void ClearText(TextBox txtBox)
        {
            txtBox.Text = string.Empty;
        }

        /// <summary>
        ///     TODO
        /// </summary>
        /// <param name="txtBox"></param>
        private static void VaryText(TextBox txtBox)
        {
            txtBox.Text = "*VARIES*";
        }

        /// <summary>
        ///     TODO
        /// </summary>
        /// <param name="txtBox"></param>
        /// <param name="text"></param>
        private static void AddText(TextBox txtBox, string text)
        {
            try
            {
                txtBox.Text = text;
            }
            catch (Exception)
            {
                ClearText(txtBox);
            }
        }

        #endregion

        #region ButtonHandling

        /// <summary>
        ///     TODO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void traverse_Click(object sender, EventArgs e)
        {
            var acCurDoc = DocumentManager.MdiActiveDocument;

            using (acCurDoc.LockDocument())
            {
                Utils.SetFocusToDwgView();
                RcTraverse.Traverse(true);
            }
        }

        /// <summary>
        ///     TODO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selParent_Click(object sender, EventArgs e)
        {
            var acCurDoc = DocumentManager.MdiActiveDocument;

            using (acCurDoc.LockDocument())
            {
                Utils.SetFocusToDwgView();
                var acCurDb = acCurDoc.Database;
                var acCurEd = acCurDoc.Editor;

                var hdlString = _rcParentTxt.Text;

                if (string.IsNullOrEmpty(hdlString) || hdlString == "0" || hdlString == string.Empty ||
                    hdlString == "*VARIES*") return;

                using (var acTrans = acCurDb.TransactionManager.StartTransaction())
                {
                    var obj = acCurEd.SelectByHandle(hdlString, acCurDb, acTrans);

                    if (obj != ObjectId.Null && !obj.IsErased)
                        acCurEd.SetImpliedSelection(new[] {obj});

                    acTrans.Commit();
                }
            }
        }

        /// <summary>
        ///     TODO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selChildren_Click(object sender, EventArgs e)
        {
            var acCurDoc = DocumentManager.MdiActiveDocument;

            using (acCurDoc.LockDocument())
            {
                Utils.SetFocusToDwgView();
                var acCurDb = acCurDoc.Database;
                var acCurEd = acCurDoc.Editor;
                var cList = new List<string>();

                foreach (var lBox in _rcChildList.Items)
                {
                    var lBItem = (ListBoxItem) lBox;
                    if (lBItem != null)
                    {
                        var hdlString = lBItem.Text;

                        if (string.IsNullOrEmpty(hdlString) || hdlString == "0" || hdlString == string.Empty ||
                            hdlString == "*VARIES*") return;

                        cList.Add(hdlString);
                    }
                }

                if (cList.Count <= 0) return;
                using (var acTrans = acCurDb.TransactionManager.StartTransaction())
                {
                    acCurEd.SelectByHandle(cList, acCurDb, acTrans);
                    acTrans.Commit();
                }
            }
        }

        /// <summary>
        ///     TODO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updChildren_Click(object sender, EventArgs e)
        {
            var acCurDoc = DocumentManager.MdiActiveDocument;
            if (acCurDoc == null) return;

            using (acCurDoc.LockDocument())
            {
                Utils.SetFocusToDwgView();
                var acCurDb = acCurDoc.Database;
                var acCurEd = acCurDoc.Editor;

                var prSelRes = acCurEd.SelectImplied();
                if (prSelRes.Status != PromptStatus.OK) return;

                var objIds = prSelRes.Value.GetObjectIds();

                RcUpdateChildren.UpdateChildren(objIds, acCurEd, acCurDb);
            }
        }

        private void childList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var index = _rcChildList.IndexFromPoint(e.Location);
            if (index == ListBox.NoMatches) return;
            try
            {
                var acCurDoc = DocumentManager.MdiActiveDocument;

                if (acCurDoc == null) return;

                using (acCurDoc.LockDocument())
                {
                    var acCurDb = acCurDoc.Database;
                    var acCurEd = acCurDoc.Editor;

                    using (var acTrans = acCurDb.TransactionManager.StartTransaction())
                    {
                        var hdlString = ((ListBoxItem) _rcChildList.Items[index]).Text;

                        var objId = acCurEd.SelectByHandle(hdlString, acCurDb, acTrans);

                        if (objId != ObjectId.Null && !objId.IsErased) acCurEd.SetImpliedSelection(new[] {objId});

                        acTrans.Commit();
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        #endregion
    }

    internal class ListBoxItem
    {
        public ListBoxItem(string text, string tag)
        {
            Text = text;
            Tag = tag;
        }

        public string Text { get; }
        private string Tag { get; }
    }
}
