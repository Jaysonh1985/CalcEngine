using System.IO;
using System;
using Microsoft.Office.Interop;
using System.Web.Mvc;
using System.Collections.Generic;
using CalculationCSharp.Models.Calculation;
using System.ComponentModel.DataAnnotations;

namespace CalculationCSharp.Models.Calculation.Scheme.Fire2006.Calculation_Type.Deferred
{
    public class Calculation
    {
        //Mandatory Declarations DO NOT REMOVE!!!//
        public List<OutputList> List = new List<OutputList>();
        public OutputList OutputList = new OutputList();
        public Calculation InputForm;
        public LookupFunctions LookupFunctions = new LookupFunctions();
        public DateFunctions DateFunctions = new DateFunctions();

        public string CalcReference { get; set; }
        public DateTime DOL { get; set; }
        public double APP { get; set; }
        public double CPD { get; set; }
        public DateTime PIDateOverride { get; set; }
        public DateTime DOB { get; set; }
        public string MarStat { get; set; }
        public DateTime DJS { get; set; }
        public double AddedYrsService { get; set; }
        public double TransInService { get; set; }
        public double PartTimeService { get; set; }
        public double Breaks { get; set; }
        public string Grade { get; set; }
        public double CVofPensionDebit { get; set; }
        public decimal LSI { get; set; }
        public decimal SCPDPension { get; set; }
        public decimal SumAVCCont { get; set; }

        public double setPensionIncreaseFactor;
        public System.DateTime setHypotheticalPensionDate;
        private double setPotentialServicetoHRA;

        public void Setup(Calculation Input)
        {
            InputForm = Input;
            
            getFactors();
            OutputList.ListBuild(List,"EGC1.1", "Hypothetical Pension", getHypotheticalPension(), "Deferred Calculation");
            OutputList.ListBuild(List, "EGC1.2", "Base Pension", getBasePension(), "Deferred Calculation");
            OutputList.ListBuild(List, "EGC1.3", "LSI Pension", InputForm.LSI, "Deferred Calculation");
            OutputList.ListBuild(List, "EGC1.4", "Total CPD Pension", getTotalCPDPension(), "Deferred Calculation");
            OutputList.ListBuild(List, "EGC1.5", "Added Years Pension", getAddedYearsPension(), "Deferred Calculation");
            OutputList.ListBuild(List, "EGC1.6", "Total Gross Pension", getTotalGrossPension(), "Deferred Calculation");
            OutputList.ListBuild(List, "EGC1.7", "PI on Pension", getPIonPension(), "Deferred Calculation");
            OutputList.ListBuild(List, "EGC1.8", "Current Value of Pension Debit", InputForm.CVofPensionDebit, "Deferred Calculation");
            OutputList.ListBuild(List, "EGC1.9", "Total Gross Pension at Exit", getTotalGrossPensionatExit(), "Deferred Calculation");
            OutputList.ListBuild(List, "EGC1.10", "Total Pension at Exit", getTotalPensionatExit(), "Deferred Calculation");
            OutputList.ListBuild(List, "EGI1.1", "Date of Leaving", InputForm.DOL.ToShortDateString(), "Deferred Calculation");
            OutputList.ListBuild(List, "EGI1.2", "Average Pensionable Pay", InputForm.APP, "Deferred Calculation");
            OutputList.ListBuild(List, "EGI1.3", "CPD Pension", InputForm.CPD, "Deferred Calculation");
            OutputList.ListBuild(List, "EGI1.4", "Pension Increase Date Overide", InputForm.PIDateOverride.ToShortDateString(), "Deferred Calculation");

            OutputList.ListBuild(List, "EGI2.1", "Date of Birth", InputForm.DOB.Date, "Member Details");
            OutputList.ListBuild(List, "EGI2.2", "Marital Status", InputForm.MarStat, "Member Details");
            OutputList.ListBuild(List, "EGI2.3", "Date Joined Scheme", InputForm.DJS.Date, "Member Details");
            OutputList.ListBuild(List, "EGI2.4", "Added Years Service", InputForm.AddedYrsService, "Member Details");
            OutputList.ListBuild(List, "EGI2.5", "Transferred in Service", InputForm.TransInService, "Member Details");
            OutputList.ListBuild(List, "EGI2.6", "Part Time Service", InputForm.PartTimeService, "Member Details");
            OutputList.ListBuild(List, "EGI2.7", "Breaks", InputForm.Breaks, "Member Details");
            OutputList.ListBuild(List, "EGI2.8", "Grade", InputForm.Grade, "Member Details");
            OutputList.ListBuild(List, "EGI2.9", "Hypothetical Pension Date", getHypotheticalPensionDate(), "Member Details");
            OutputList.ListBuild(List, "EGI2.10", "LSI Pension", InputForm.LSI, "Member Details");
            OutputList.ListBuild(List, "EGI2.11", "CPD Pension", InputForm.SCPDPension, "Member Details");

            OutputList.ListBuild(List, "EGI3.1", "Age at Date of leaving", getAgeatDOL(), "Dates Ages & Factors");
            OutputList.ListBuild(List, "EGI3.2", "Calculation Date", System.DateTime.Now.ToShortDateString(), "Dates Ages & Factors");
            OutputList.ListBuild(List, "EGI3.3", "Eligible Payment Date", getEligiblePaymentDate().ToShortDateString(), "Dates Ages & Factors");
            OutputList.ListBuild(List, "EGI3.4", "Pension Increase Factor", setPensionIncreaseFactor, "Dates Ages & Factors");
            OutputList.ListBuild(List, "EGI3.5", "CPD Pension Increase Factor", getCPDPensionIncreaseFactor(), "Dates Ages & Factors");

            OutputList.ListBuild(List, "EGI4.1", "Total Pension Service", getTotalPensionService(), "Service");
            OutputList.ListBuild(List, "EGI4.2", "Potential Pensionable Service to HRA", getPotentialPensionableServicetoHRA(), "Service");
            OutputList.ListBuild(List, "EGI4.3", "Total Entitlement Service", getTotalEntitlementService(), "Service");
            OutputList.ListBuild(List, "EGI4.4", "Part Time Adjustment", getPartTimeAdjustment(), "Service");
            OutputList.ListBuild(List, "EGI4.5", "Total Post 88 Pension Service", getTotalPost88PensionService(), "Service");
            OutputList.ListBuild(List, "EGI4.6", "Total Service for Survivor Requisite Benefit Pension", getTotalServiceSurvivorRequisiteBenefitPension(), "Service");

            OutputList.ListBuild(List, "EGR1.1", "Date Effective", getAADateEffective(), "Annual Allowance");
            OutputList.ListBuild(List, "EGR1.2", "AVC Contributions", InputForm.SumAVCCont, "Annual Allowance");
            OutputList.ListBuild(List, "EGR1.3", "Annual Allowance Pension", getAnnualAllowancePension(), "Annual Allowance");

            OutputList.ListBuild(List, "EGR2.1", "Survivor Contingent Pension", getSurvivorContingentPension(), "Survivor Benefits");
            OutputList.ListBuild(List, "EGR2.2", "PI on Contingent Survivor Pension", getPIonSurvivorContingentPension(), "Survivor Benefits");
            OutputList.ListBuild(List, "EGR2.3", "Survivor LSI Pension", getSurvivorLSIPension(), "Survivor Benefits");
            OutputList.ListBuild(List, "EGR2.4", "Survivor CPD Pension", getSurvivorCPDPension(), "Survivor Benefits");
            OutputList.ListBuild(List, "EGR2.5", "Total Contingent Survivor Pension", getTotalContingentSurvivorPension(), "Survivor Benefits");
            OutputList.ListBuild(List, "EGR2.6", "Survivor's Requisite Benefit Pension", getSurvivorRequisiteBenefitPension(), "Survivor Benefits");
        }

        public void getFactors()
        {
            setPensionIncreaseFactor = LookupFunctions.CSVLookup("PIFactor", Convert.ToString(InputForm.PIDateOverride.Date), 1, 2);
            setHypotheticalPensionDate = Convert.ToDateTime(InputForm.DOL.Date).AddDays(LookupFunctions.CSVLookup("Grade", InputForm.Grade, 2, 3));
            setPotentialServicetoHRA = getPotentialPensionableServicetoHRA();
        }

        public double getHypotheticalPension()
        {
            if (setPotentialServicetoHRA > 20)
            {
                double HypotheticalPension = (setPotentialServicetoHRA * InputForm.APP * 0.0166666666666667) + (((setPotentialServicetoHRA - 20) * 2) * InputForm.APP * 0.0166666666666667);

                return Math.Round(HypotheticalPension, 2);
            }
            else
            {
                return Math.Round(setPotentialServicetoHRA * InputForm.APP * (1 / 6), 2);
            }
        }
        public double getBasePension()
        {
            if (InputForm.PartTimeService > 0)
            {
                return Math.Round(getHypotheticalPension() * getPartTimeAdjustment(), 2);
            }
            else
            {
                return getHypotheticalPension();
            }
        }
        public double getTotalLSIPension()
        {
            return Convert.ToDouble(InputForm.LSI) * getCPDPensionIncreaseFactor();
        }
        public double getTotalCPDPension()
        {
            return InputForm.CPD * getCPDPensionIncreaseFactor();
        }
        public double getAddedYearsPension()
        {
            return getBasePension() * InputForm.AddedYrsService * (1 / 60);
        }
        public double getTotalGrossPension()
        {
            return getBasePension() + getTotalLSIPension() + getTotalCPDPension() + getAddedYearsPension();
        }
        public double getPIonPension()
        {
            return Math.Round(((getTotalGrossPension() - getTotalLSIPension() - getTotalCPDPension()) * setPensionIncreaseFactor) - (getTotalGrossPension() - getTotalLSIPension() - getTotalCPDPension()), 2);
        }
        public double getTotalGrossPensionatExit()
        {
            return getTotalGrossPension() + getPIonPension();
        }
        public double getTotalPensionatExit()
        {
            return getTotalGrossPensionatExit() - InputForm.CVofPensionDebit;
        }
        public System.DateTime getHypotheticalPensionDate()
        {
            return setHypotheticalPensionDate;
        }
        public double getAgeatDOL()
        {
            return DateFunctions.YearsDaysBetween(Convert.ToDateTime(InputForm.DOB), Convert.ToDateTime(InputForm.DOL), false, 365);
        }
        public System.DateTime getEligiblePaymentDate()
        {
            return Convert.ToDateTime(InputForm.DOB.Date).AddYears(60);
        }

        public double getCPDPensionIncreaseFactor()
        {
            return 0;
        }
        public double getTotalPensionService()
        {
            return DateFunctions.YearsDaysBetween(InputForm.DJS.Date, InputForm.DOL.Date, false, 365) + InputForm.AddedYrsService + InputForm.TransInService - InputForm.PartTimeService - InputForm.Breaks;
        }
        public double getPotentialPensionableServicetoHRA()
        {
            int YrsDJS;
            double Daysbetween;
            int YrsDOL;
            YrsDJS = Convert.ToInt32(DateFunctions.YearsDaysBetween(InputForm.DJS, InputForm.DOL, false, 365));
            YrsDOL = Convert.ToInt32(DateFunctions.YearsDaysBetween(InputForm.DOL, getHypotheticalPensionDate(), true, 365));

            double daycalc = (InputForm.DOL - InputForm.DJS).TotalDays;
            double daycalc2 = (getHypotheticalPensionDate() - InputForm.DOL).TotalDays;

            Daysbetween = ((daycalc / 365)) - YrsDJS + ((daycalc2 / 365) - YrsDOL);
            return YrsDJS + Daysbetween;
        }
        public double getTotalEntitlementService()
        {
            return getTotalPensionService() + InputForm.TransInService;
        }
        public double getPartTimeAdjustment()
        {
            return InputForm.PartTimeService / getTotalEntitlementService();
        }
        public double getTotalPost88PensionService()
        {
            return 0;
        }
        public double getTotalServiceSurvivorRequisiteBenefitPension()
        {
            if (InputForm.MarStat == "Civil Partnership")
            {
                return getTotalPost88PensionService();
            }
            else
            {
                return getTotalPensionService();
            }
        }
        public System.DateTime getAADateEffective()
        {
            return Convert.ToDateTime("31/01/2016");
        }
        public double getAnnualAllowancePension()
        {
            return getTotalPensionatExit();
        }
        public double getSurvivorContingentPension()
        {
            if (getHypotheticalPension() > 0)
            {
                return getHypotheticalPension() / 2;
            }
            else
            {
                return 0;
            }
        }
        public double getPIonSurvivorContingentPension()
        {
            return Math.Round((getSurvivorContingentPension() * setPensionIncreaseFactor) - getSurvivorContingentPension(), 2);
        }
        public decimal getSurvivorLSIPension()
        {
            if (InputForm.MarStat == "Married" | InputForm.MarStat == "Civil Partnership")
            {
                return InputForm.LSI / 2;
            }
            else
            {
                return 0;
            }
        }
        public double getSurvivorCPDPension()
        {
            if (InputForm.MarStat == "Married" | InputForm.MarStat == "Civil Partnership")
            {
                return InputForm.CPD / 2;
            }
            else
            {
                return 0;
            }
        }
        public double getTotalContingentSurvivorPension()
        {
            return getSurvivorContingentPension() + getPIonSurvivorContingentPension() + getSurvivorCPDPension();
        }
        public double getSurvivorRequisiteBenefitPension()
        {
            return getTotalServiceSurvivorRequisiteBenefitPension() * InputForm.APP;
        }


    }
}
