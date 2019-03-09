﻿using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using RabCab.Utilities.Calculators;

namespace RabCab.Utilities.Extensions
{
    internal static class EditorExtensions
    {

        #region Prompt Angle Options

        /// <summary>
        /// Method to get an angle from the user in radians
        /// </summary>
        /// <param name="acCurEd">The current working editor</param>
        /// <param name="prompt">The string to be prompted to the user</param>
        /// <param name="defaultValue">The default value to use in prompt -> pressing enter will automatically use the default distance. Default value is input in Degrees</param>
        /// <returns>Returns the value input by the user as a radian</returns>
        public static double GetRadian(this Editor acCurEd, string prompt, double defaultValue = 0)
        {//If default value, convert it from degrees to radians

            if (defaultValue != 0)
                defaultValue = UnitConverter.ConvertToRadians(defaultValue);

            //Prompt user to enter an angle in autoCAD
            var prAngOpts = new PromptAngleOptions(string.Empty)
            {
                Message = prompt,
                AllowNone = false,
                AllowZero = false,
                DefaultValue = defaultValue,
                UseDefaultValue = defaultValue != 0,
            };

            //Prompt the editor to receive the angle from the user
            var prAngRes = acCurEd.GetAngle(prAngOpts);

            //If bad input -> return 0
            if (prAngRes.Status != PromptStatus.OK) return 0;

            //Return the angle entered into the editor
            var doubleResult = prAngRes.Value;
            return doubleResult;
        }

        /// <summary>
        /// Method to get an angle from the user and return it in degrees
        /// </summary>
        /// <param name="acCurEd">The current working editor</param>
        /// <param name="prompt">The prompt to present to the user</param>
        /// <param name="defaultValue">The default value to use in prompt -> pressing enter will automatically use the default distance. Default value is input in degrees</param>
        /// <returns>Returns the value input by the user as a degree</returns>
        public static double GetDegree(this Editor acCurEd, string prompt, double defaultValue = 0)
        {
            //If default value, convert it from degrees to radians
            if (defaultValue != 0)
                defaultValue = UnitConverter.ConvertToRadians(defaultValue);

            //Prompt user to enter an angle in autoCAD
            var prAngOpts = new PromptAngleOptions(string.Empty)
            {
                Message = prompt,
                AllowNone = false,
                AllowZero = false,
                DefaultValue = defaultValue,
                UseDefaultValue = defaultValue != 0,
            };

            //Prompt the editor to receive the angle from the user
            var prAngRes = acCurEd.GetAngle(prAngOpts);

            //If bad input -> return 0
            if (prAngRes.Status != PromptStatus.OK) return 0;

            //Return the angle entered into the editor
            var doubleResult = prAngRes.Value;
            return UnitConverter.ConvertToDegrees(doubleResult);
        }

        #endregion

        #region Prompt Distance Options

        /// <summary>
        /// Gets any 3D distance in CAD, allows positive and negative values.
        /// </summary>
        /// <param name="acCurEd">The current working Editor.</param>
        /// <param name="prompt">The prompt to present to the user.</param>
        /// <param name="defaultValue">The default value to use in prompt -> pressing enter will automatically use the default distance.</param>
        /// <returns>Returns a distance from the editor in decimal format.</returns>
        public static double GetAnyDistance(this Editor acCurEd, string prompt, double defaultValue = 0)
        {
            //Prompt user to enter a distance in autoCAD
            var prDistOpts = new PromptDistanceOptions(string.Empty)
            {
                Message = prompt,
                AllowNone = false,
                AllowZero = true,
                DefaultValue = defaultValue,
                UseDefaultValue = defaultValue != 0,
            };

            //Prompt the editor to receive the distance from the user
            var prDistRes = acCurEd.GetDistance(prDistOpts);

            //If bad input -> return 0
            if (prDistRes.Status != PromptStatus.OK) return 0;

            //Return the distance entered into the editor
            var distResult = prDistRes.Value;
            return distResult;
        }

        /// <summary>
        /// Gets any 3D distance in CAD, uses a base point and allows positive and negative values.
        /// </summary>
        /// <param name="acCurEd">The current working Editor.</param>
        /// <param name="basePt">The point to start the distance from.</param>
        /// <param name="prompt">The prompt to present to the user.</param>
        /// <param name="defaultValue">The default value to use in prompt -> pressing enter will automatically use the default distance.</param>
        /// <returns>Returns a distance from the editor in decimal format.</returns>
        public static double GetAnyDistance(this Editor acCurEd, Point3d basePt, string prompt, double defaultValue = 0)
        {
            //Prompt user to enter a distance in autoCAD
            var prDistOpts = new PromptDistanceOptions(string.Empty)
            {
                Message = prompt,
                AllowNone = false,
                AllowZero = true,
                BasePoint = basePt,
                UseBasePoint = true,
                DefaultValue = defaultValue,
                UseDefaultValue = defaultValue != 0,
            };

            //Prompt the editor to receive the distance from the user
            var prDistRes = acCurEd.GetDistance(prDistOpts);

            //If bad input -> return 0
            if (prDistRes.Status != PromptStatus.OK) return 0;

            //Return the distance entered into the editor
            var distResult = prDistRes.Value;
            return distResult;
        }

        /// <summary>
        /// Gets any 3D distance in CAD, allows positive values.
        /// </summary>
        /// <param name="acCurEd">The current working Editor.</param>
        /// <param name="prompt">The prompt to present to the user.</param>
        /// <param name="defaultValue">The default value to use in prompt -> pressing enter will automatically use the default distance.</param>
        /// <returns>Returns a distance from the editor in decimal format.</returns>
        public static double GetPositiveDistance(this Editor acCurEd, string prompt, double defaultValue = 0)
        {
            //Prompt user to enter a distance in autoCAD
            var prDistOpts = new PromptDistanceOptions(string.Empty)
            {
                Message = prompt,
                AllowNone = false,
                AllowZero = true,
                AllowNegative = false,
                DefaultValue = defaultValue,
                UseDefaultValue = defaultValue != 0,
            };

            //Prompt the editor to receive the distance from the user
            var prDistRes = acCurEd.GetDistance(prDistOpts);

            //If bad input -> return 0
            if (prDistRes.Status != PromptStatus.OK) return 0;

            //Return the distance entered into the editor
            var distResult = prDistRes.Value;
            return distResult;
        }

        /// <summary>
        /// Gets any 3D distance in CAD, uses a base point and allows positive values.
        /// </summary>
        /// <param name="acCurEd">The current working Editor.</param>
        /// <param name="basePt">The point to start the distance from.</param>
        /// <param name="prompt">The prompt to present to the user.</param>
        /// <param name="defaultValue">The default value to use in prompt -> pressing enter will automatically use the default distance.</param>
        /// <returns>Returns a distance from the editor in decimal format.</returns>
        public static double GetPositiveDistance(this Editor acCurEd, Point3d basePt, string prompt, double defaultValue = 0)
        {
            //Prompt user to enter a distance in autoCAD
            var prDistOpts = new PromptDistanceOptions(string.Empty)
            {
                Message = prompt,
                AllowNone = false,
                AllowZero = true,
                AllowNegative = false,
                BasePoint = basePt,
                UseBasePoint = true,
                DefaultValue = defaultValue,
                UseDefaultValue = defaultValue != 0,
            };

            //Prompt the editor to receive the distance from the user
            var prDistRes = acCurEd.GetDistance(prDistOpts);

            //If bad input -> return 0
            if (prDistRes.Status != PromptStatus.OK) return 0;

            //Return the distance entered into the editor
            var distResult = prDistRes.Value;
            return distResult;
        }

        /// <summary>
        /// Gets any 2D distance in CAD, allows positive and negative values.
        /// </summary>
        /// <param name="acCurEd">The current working Editor.</param>
        /// <param name="prompt">The prompt to present to the user.</param>
        /// /// <param name="defaultValue">The default value to use in prompt -> pressing enter will automatically use the default distance.</param>
        /// <returns>SReturns a distance from the editor in decimal format.</returns>
        public static double GetAny2DDistance(this Editor acCurEd, string prompt, double defaultValue = 0)
        {
            //Prompt user to enter a distance in autoCAD
            var prDistOpts = new PromptDistanceOptions(string.Empty)
            {
                Message = prompt,
                AllowNone = false,
                AllowZero = true,
                Only2d = true,
                DefaultValue = defaultValue,
                UseDefaultValue = defaultValue != 0,
            };

            //Prompt the editor to receive the distance from the user
            var prDistRes = acCurEd.GetDistance(prDistOpts);

            //If bad input -> return 0
            if (prDistRes.Status != PromptStatus.OK) return 0;

            //Return the distance entered into the editor
            var distResult = prDistRes.Value;
            return distResult;
        }

        /// <summary>
        /// Gets any 2D distance in CAD, uses a base point and allows positive and negative values.
        /// </summary>
        /// <param name="acCurEd">The current working Editor.</param>
        /// <param name="basePt">The point to start the distance from.</param>
        /// <param name="prompt">The prompt to present to the user.</param>
        /// <param name="defaultValue">The default value to use in prompt -> pressing enter will automatically use the default distance.</param>
        /// <returns>Returns a distance from the editor in decimal format.</returns>
        public static double GetAny2DDistance(this Editor acCurEd, Point3d basePt, string prompt, double defaultValue = 0)
        {
            //Prompt user to enter a distance in autoCAD
            var prDistOpts = new PromptDistanceOptions(string.Empty)
            {
                Message = prompt,
                AllowNone = false,
                AllowZero = true,
                Only2d = true,
                BasePoint = basePt,
                UseBasePoint = true,
                DefaultValue = defaultValue,
                UseDefaultValue = defaultValue != 0,
            };

            //Prompt the editor to receive the distance from the user
            var prDistRes = acCurEd.GetDistance(prDistOpts);

            //If bad input -> return 0
            if (prDistRes.Status != PromptStatus.OK) return 0;

            //Return the distance entered into the editor
            var distResult = prDistRes.Value;
            return distResult;
        }

        /// <summary>
        /// Gets any 2D distance in CAD, allows positive values.
        /// </summary>
        /// <param name="acCurEd">The current working Editor.</param>
        /// <param name="prompt">The prompt to present to the user.</param>
        /// <param name="defaultValue">The default value to use in prompt -> pressing enter will automatically use the default distance.</param>
        /// <returns>Returns a distance from the editor in decimal format.</returns>
        public static double GetPositive2DDistance(this Editor acCurEd, string prompt, double defaultValue = 0)
        {
            //Prompt user to enter a distance in autoCAD
            var prDistOpts = new PromptDistanceOptions(string.Empty)
            {
                Message = prompt,
                AllowNone = false,
                AllowZero = true,
                Only2d = true,
                AllowNegative = false,
                DefaultValue = defaultValue,
                UseDefaultValue = defaultValue != 0,
            };

            //Prompt the editor to receive the distance from the user
            var prDistRes = acCurEd.GetDistance(prDistOpts);

            //If bad input -> return 0
            if (prDistRes.Status != PromptStatus.OK) return 0;

            //Return the distance entered into the editor
            var distResult = prDistRes.Value;
            return distResult;
        }

        /// <summary>
        /// Gets any 2D distance in CAD, allows positive values.
        /// </summary>
        /// <param name="acCurEd">The current working Editor.</param>
        /// <param name="basePt">The point to start the distance from.</param>
        /// <param name="prompt">The prompt to present to the user.</param>
        /// <param name="defaultValue">The default value to use in prompt -> pressing enter will automatically use the default distance.</param>
        /// <returns>Returns a distance from the editor in decimal format.</returns>
        public static double GetPositive2DDistance(this Editor acCurEd, Point3d basePt, string prompt, double defaultValue = 0)
        {
            //Prompt user to enter a distance in autoCAD
            var prDistOpts = new PromptDistanceOptions(string.Empty)
            {
                Message = prompt,
                AllowNone = false,
                AllowZero = true,
                Only2d = true,
                AllowNegative = false,
                BasePoint = basePt,
                UseBasePoint = true,
                DefaultValue = defaultValue,
                UseDefaultValue = defaultValue != 0
            };

            //Prompt the editor to receive the distance from the user
            var prDistRes = acCurEd.GetDistance(prDistOpts);

            //If bad input -> return 0
            if (prDistRes.Status != PromptStatus.OK) return 0;

            //Return the distance entered into the editor
            var distResult = prDistRes.Value;
            return distResult;
        }

        #endregion

    }
}
