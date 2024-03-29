﻿// -----------------------------------------------------------------------------------
//     <copyright file="AssemblyInfo.cs" company="CraterSpace">
//     Copyright (c) 2019 CraterSpace - All Rights Reserved 
//     </copyright>
//     <author>Zach Ayers</author>
//     <date>03/08/2019</date>
//     Description:    
//     Notes:  
//     References:          
// -----------------------------------------------------------------------------------

using System.Reflection;
using System.Runtime.InteropServices;
using Autodesk.AutoCAD.Runtime;
using RabCab.Commands.AnalysisSuite;
using RabCab.Commands.AnnotationSuite;
using RabCab.Commands.AssemblySuite;
using RabCab.Commands.AutomationSuite;
using RabCab.Commands.CarpentrySuite;
using RabCab.Commands.CNCSuite;
using RabCab.Commands.CombineSuite;
using RabCab.Commands.PaletteKit;
using RabCab.Commands.ReferenceSuite.BlockKit;
using RabCab.Commands.StructuralSuite;
using RabCab.Commands.TidySuite;
using RabCab.Initialization;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.

[assembly: AssemblyTitle("RabCab")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("CraterSpace")]
[assembly: AssemblyProduct("RabCab")]
[assembly: AssemblyCopyright("Copyright ©  2020")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]

[assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyFileVersion("1.0.0.0")]

// In order to sign your assembly you must specify a key to use. Refer to the 
// Microsoft .NET Framework documentation for more information on assembly signing.
//
// Use the attributes below to control which key is used for signing. 
//
// Notes: 
//   (*) If no key is specified, the assembly is not signed.
//   (*) KeyName refers to a key that has been installed in the Crypto Service
//       Provider (CSP) on your machine. KeyFile refers to a file which contains
//       a key.
//   (*) If the KeyFile and the KeyName values are both specified, the 
//       following processing occurs:
//       (1) If the KeyName can be found in the CSP, that key is used.
//       (2) If the KeyName does not exist and the KeyFile does exist, the key 
//           in the KeyFile is installed into the CSP and used.
//   (*) In order to create a KeyFile, you can use the sn.exe (Strong Name) utility.
//       When specifying the KeyFile, the location of the KeyFile should be
//       relative to the project output directory which is
//       %Project Directory%\obj\<configuration>. For example, if your KeyFile is
//       located in the project directory, you would specify the AssemblyKeyFile 
//       attribute as [assembly: AssemblyKeyFile("..\\..\\mykey.snk")]
//   (*) Delay Signing is an advanced option - see the Microsoft .NET Framework
//       documentation for more information on this.

[assembly: AssemblyDelaySign(false)]
[assembly: AssemblyKeyFile("")]
[assembly: AssemblyKeyName("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.

[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM

[assembly: Guid("12540b60-474b-4908-8781-f758acf78931")]

//Analysis Suite
[assembly: CommandClass(typeof(RcDump))]
[assembly: CommandClass(typeof(RcFilter))]
[assembly: CommandClass(typeof(RcShowBounds))]
[assembly: CommandClass(typeof(RcTraverse))]
//[assembly: CommandClass(typeof(RcWeight))]

//Annotation Suite
[assembly: CommandClass(typeof(RcAlignViews))]
[assembly: CommandClass(typeof(RcDimAlign))]
[assembly: CommandClass(typeof(RcDimArrow))]
[assembly: CommandClass(typeof(RcDimExtend))]
[assembly: CommandClass(typeof(RcDimSpace))]
[assembly: CommandClass(typeof(RcDimBreak))]
[assembly: CommandClass(typeof(RcDimToggle))]
[assembly: CommandClass(typeof(RcDimLt))]
[assembly: CommandClass(typeof(RcDimAdd))]
[assembly: CommandClass(typeof(RcDimMove))]
[assembly: CommandClass(typeof(RcDimPrec))]
[assembly: CommandClass(typeof(RcDimDelete))]
[assembly: CommandClass(typeof(RcGenDims))]
[assembly: CommandClass(typeof(RcDimSelect))]
[assembly: CommandClass(typeof(RcGenViews))]
[assembly: CommandClass(typeof(RcMatchViews))]
[assembly: CommandClass(typeof(RcUpdateLeaders))]
[assembly: CommandClass(typeof(RcSpaceViews))]
[assembly: CommandClass(typeof(RcVpConvert))]
[assembly: CommandClass(typeof(RcDimUpdate))]
[assembly: CommandClass(typeof(RcDimEdit))]
[assembly: CommandClass(typeof(RcShrinkWrap))]
[assembly: CommandClass(typeof(RcWeldMark))]
[assembly: CommandClass(typeof(RcPartLeader))]


//Assembly Suite
[assembly: CommandClass(typeof(RcBom))]
[assembly: CommandClass(typeof(RcBlockLegend))]
[assembly: CommandClass(typeof(RcDataExtract))]
[assembly: CommandClass(typeof(RcExplode))]
[assembly: CommandClass(typeof(RcLayParts))]
[assembly: CommandClass(typeof(RcNameParts))]
[assembly: CommandClass(typeof(RcPartMark))]
[assembly: CommandClass(typeof(RcUpdateChildren))]
[assembly: CommandClass(typeof(RcScatter))]
[assembly: CommandClass(typeof(RcSelection))]
[assembly: CommandClass(typeof(RcUpdate))]
[assembly: CommandClass(typeof(RcExportParts))]


//Automation Suite
[assembly: CommandClass(typeof(RcAutoLayer))]
[assembly: CommandClass(typeof(RcPageNumber))]
[assembly: CommandClass(typeof(RcTContents))]

//Carpentry Suite
[assembly: CommandClass(typeof(RcSolDivide))]
[assembly: CommandClass(typeof(RcDrill))]
[assembly: CommandClass(typeof(RcChop))]
[assembly: CommandClass(typeof(RcGap))]
[assembly: CommandClass(typeof(RcJoint))]
[assembly: CommandClass(typeof(RcOffset))]
[assembly: CommandClass(typeof(RcSlice))]
[assembly: CommandClass(typeof(RcFlip))]
[assembly: CommandClass(typeof(RcICut))]
[assembly: CommandClass(typeof(RcDogEar))]
[assembly: CommandClass(typeof(RcConnection))]
//[assembly: CommandClass(typeof(RcRoute))]
[assembly: CommandClass(typeof(RcCrossJoint))]
[assembly: CommandClass(typeof(RcEdgeBand))]
[assembly: CommandClass(typeof(RcLaminate))]


//CNC Suite
//[assembly: CommandClass(typeof(RcAutoPrep))]
//[assembly: CommandClass(typeof(RcBreakPoints))]
//[assembly: CommandClass(typeof(RcCheckWork))]
//[assembly: CommandClass(typeof(RcExportPaths))]
[assembly: CommandClass(typeof(RcFlatten))]
//[assembly: CommandClass(typeof(RcNest))]
//[assembly: CommandClass(typeof(RcLoopBit))]
//[assembly: CommandClass(typeof(RcLoopInner))]
//[assembly: CommandClass(typeof(RcLoopOuter))]


//Combine Suite
[assembly: CommandClass(typeof(RcConverge))]
[assembly: CommandClass(typeof(RcFuse))]
[assembly: CommandClass(typeof(RcSeparate))]
[assembly: CommandClass(typeof(RcSubtrahend))]


//Palette Kit
[assembly: CommandClass(typeof(RcPaletteLayer))]
[assembly: CommandClass(typeof(RcPaletteMetric))]
[assembly: CommandClass(typeof(RcPaletteNet))]
[assembly: CommandClass(typeof(RcPaletteNotebook))]

//Reference Suite
[assembly: CommandClass(typeof(RcAutoAtt))]
[assembly: CommandClass(typeof(RcAutoBlock))]
[assembly: CommandClass(typeof(RcQuickRename))]
[assembly: CommandClass(typeof(BlockToNamed))]

//Structural Suite
[assembly: CommandClass(typeof(RcAlign))]
[assembly: CommandClass(typeof(RcFloor))]
//[assembly: CommandClass(typeof(RcGenFastener))]
//[assembly: CommandClass(typeof(RcGenMember))]
//[assembly: CommandClass(typeof(RcWeldBead))]
//[assembly: CommandClass(typeof(RcGenFrame))]
//[assembly: CommandClass(typeof(RcUnfold))]

//Tidy Suite
[assembly: CommandClass(typeof(RcCleanDirectory))]
[assembly: CommandClass(typeof(RcCleanSols))]
[assembly: CommandClass(typeof(RcEmptyDwg))]