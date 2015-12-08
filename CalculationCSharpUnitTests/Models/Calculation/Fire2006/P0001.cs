using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CalculationCSharp;
using System.IO;
using Microsoft.Office.Interop;
using System.Web.Mvc;
using CalculationCSharp.Models.Calculation;
using System.ComponentModel.DataAnnotations;

namespace CalculationCSharpUnitTests
    
{
    
    /// <summary>
    /// Summary description for Deferred
    /// </summary>
    [TestClass]
    public class Fire2006P001
    {
        CalculationCSharp.Models.Calculation.Fire2006.Deferred.Deferred Calculation = new CalculationCSharp.Models.Calculation.Fire2006.Deferred.Deferred();
        public LookupFunctions LookupFunctions = new LookupFunctions();
        public DateFunctions DateFunctions = new DateFunctions();

        DateTime DOL = Convert.ToDateTime("17/07/2015");
        DateTime PIDateOverride = Convert.ToDateTime("30/04/2013");
        DateTime DOB = Convert.ToDateTime("30/09/1973");
        DateTime DJS = Convert.ToDateTime("03/04/1991");
        public double setPensionIncreaseFactor;
        public System.DateTime setHypotheticalPensionDate;

        [TestMethod]
        public void DeferredCalc()
        {
            Double Result = Calculation.getTotalPensionatExit();
            Assert.AreEqual(Result, 24.29);
        }

        
    }
}
