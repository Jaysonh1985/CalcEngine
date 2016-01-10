using System;
using System.Collections.Generic;
using CalculationCSharp.Models.Calculation;

namespace CalculationCSharp.Areas.Fire2006.Models
{
    [Serializable]
    public class Deferred
    {
        public string CalcReference { get; set; }
        public DateTime CalcDate { get; set; }
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
        public double LSI { get; set; }
        public double SCPDPension { get; set; }
        public double SumAVCCont { get; set; }
    }

    public class DeferredFunctions
    {
        public Deferred InputForm;
        public LookupFunctions LookupFunctions = new LookupFunctions();
        public DateFunctions DateFunctions = new DateFunctions();
        public List<OutputList> List = new List<OutputList>();

        public double setPensionIncreaseFactor;
        public System.DateTime setHypotheticalPensionDate;
        private double setPotentialServicetoHRA;

        public object Setup(object Input)
        {
            InputForm = Input as Deferred;
            getFactors();

            List.Add(new OutputList { ID = "EGC1.0", Field = "Calculation Reference", Value = Convert.ToString(InputForm.CalcReference), Group = "Deferred Calculation"});
            List.Add(new OutputList { ID = "EGC1.1", Field = "Hypothetical Pension", Value = Convert.ToString(getHypotheticalPension()), Group = "Deferred Calculation"});
            List.Add(new OutputList { ID = "EGC1.2", Field = "Base Pension", Value = Convert.ToString(getBasePension()), Group = "Deferred Calculation"});
            List.Add(new OutputList { ID = "EGC1.3", Field = "LSI Pension", Value = Convert.ToString(InputForm.LSI), Group = "Deferred Calculation"});
            List.Add(new OutputList { ID = "EGC1.4", Field = "Total CPD Pension", Value = Convert.ToString(getTotalCPDPension()), Group = "Deferred Calculation"});
            List.Add(new OutputList { ID = "EGC1.5", Field = "Added Years Pension", Value = Convert.ToString(getAddedYearsPension()), Group = "Deferred Calculation"});
            List.Add(new OutputList { ID = "EGC1.6", Field = "Total Gross Pension", Value = Convert.ToString(getTotalGrossPension()), Group = "Deferred Calculation"});
            List.Add(new OutputList { ID = "EGC1.7", Field = "PI on Pension", Value = Convert.ToString(getPIonPension()), Group = "Deferred Calculation"});
            List.Add(new OutputList { ID = "EGC1.8", Field = "Current Value of Pension Debit", Value = Convert.ToString(InputForm.CVofPensionDebit), Group = "Deferred Calculation"});
            List.Add(new OutputList { ID = "EGC1.9", Field = "Total Gross Pension at Exit", Value = Convert.ToString(getTotalGrossPensionatExit()), Group = "Deferred Calculation"});
            List.Add(new OutputList { ID = "EGC1.10", Field = "Total Pension at Exit", Value = Convert.ToString(getTotalPensionatExit()), Group = "Deferred Calculation" });
            List.Add(new OutputList { ID = "EGI1.1", Field = "Date of Leaving", Value = Convert.ToString(InputForm.DOL.ToShortDateString()), Group = "Deferred Calculation" });
            List.Add(new OutputList { ID = "EGI1.2", Field = "Average Pensionable Pay", Value = Convert.ToString(InputForm.APP), Group = "Deferred Calculation" });
            List.Add(new OutputList { ID = "EGI1.3", Field = "CPD Pension", Value = Convert.ToString(InputForm.CPD), Group = "Deferred Calculation" });
            List.Add(new OutputList { ID = "EGI1.4", Field = "Pension Increase Date Overide", Value = Convert.ToString(InputForm.PIDateOverride.ToShortDateString()), Group = "Deferred Calculation" });
            List.Add(new OutputList { ID = "EGI2.1", Field = "Date of Birth", Value = Convert.ToString(InputForm.DOB.Date), Group = "Member Details" });
            List.Add(new OutputList { ID = "EGI2.2", Field = "Marital Status", Value = Convert.ToString(InputForm.MarStat), Group = "Member Details" });
            List.Add(new OutputList { ID = "EGI2.3", Field = "Date Joined Scheme", Value = Convert.ToString(InputForm.DJS.Date), Group = "Member Details" });
            List.Add(new OutputList { ID = "EGI2.4", Field = "Added Years Service", Value = Convert.ToString(InputForm.AddedYrsService), Group = "Member Details" });
            List.Add(new OutputList { ID = "EGI2.5", Field = "Transferred in Service", Value = Convert.ToString(InputForm.TransInService), Group = "Member Details" });
            List.Add(new OutputList { ID = "EGI2.6", Field = "Part Time Service", Value = Convert.ToString(InputForm.PartTimeService), Group = "Member Details" });
            List.Add(new OutputList { ID = "EGI2.7", Field = "Breaks", Value = Convert.ToString(InputForm.Breaks), Group = "Member Details" });
            List.Add(new OutputList { ID = "EGI2.8", Field = "Grade", Value = Convert.ToString(InputForm.Grade), Group = "Member Details" });
            List.Add(new OutputList { ID = "EGI2.9", Field = "Hypothetical Pension Date", Value = Convert.ToString(getHypotheticalPensionDate()), Group = "Member Details" });
            List.Add(new OutputList { ID = "EGI2.10", Field = "LSI Pension", Value = Convert.ToString(InputForm.LSI), Group = "Member Details" });
            List.Add(new OutputList { ID = "EGI2.11", Field = "CPD Pension", Value = Convert.ToString(InputForm.SCPDPension), Group = "Member Details" });
            List.Add(new OutputList { ID = "EGI3.1", Field = "Age at Date of leaving", Value = Convert.ToString(getAgeatDOL()), Group = "Dates Ages & Factors" });
            List.Add(new OutputList { ID = "EGI3.2", Field = "Calculation Date", Value = Convert.ToString(InputForm.CalcDate.ToShortDateString()), Group = "Dates Ages & Factors" });
            List.Add(new OutputList { ID = "EGI3.3", Field = "Eligible Payment Date", Value = Convert.ToString(getEligiblePaymentDate().ToShortDateString()), Group = "Dates Ages & Factors" });
            List.Add(new OutputList { ID = "EGI3.4", Field = "Pension Increase Factor", Value = Convert.ToString(setPensionIncreaseFactor), Group = "Dates Ages & Factors" });
            List.Add(new OutputList { ID = "EGI3.5", Field = "CPD Pension Increase Factor", Value = Convert.ToString(getCPDPensionIncreaseFactor()), Group = "Dates Ages & Factors" });
            List.Add(new OutputList { ID = "EGI4.1", Field = "Total Pension Service", Value = Convert.ToString(getTotalPensionService()), Group = "Service" });
            List.Add(new OutputList { ID = "EGI4.2", Field = "Potential Pensionable Service to HRA", Value = Convert.ToString(getPotentialPensionableServicetoHRA()), Group = "Service" });
            List.Add(new OutputList { ID = "EGI4.3", Field = "Total Entitlement Service", Value = Convert.ToString(getTotalEntitlementService()), Group = "Service" });
            List.Add(new OutputList { ID = "EGI4.4", Field = "Part Time Adjustment", Value = Convert.ToString(getPartTimeAdjustment()), Group = "Service" });
            List.Add(new OutputList { ID = "EGI4.5", Field = "Total Post 88 Pension Service", Value = Convert.ToString(getTotalPost88PensionService()), Group = "Service" });
            List.Add(new OutputList { ID = "EGI4.6", Field = "Total Service for Survivor Requisite Benefit Pension", Value = Convert.ToString(getTotalServiceSurvivorRequisiteBenefitPension()), Group = "Service" });
            List.Add(new OutputList { ID = "EGR1.1", Field = "Date Effective", Value = Convert.ToString(getAADateEffective()), Group = "Annual Allowance" });
            List.Add(new OutputList { ID = "EGR1.2", Field = "AVC Contributions", Value = Convert.ToString(InputForm.SumAVCCont), Group = "Annual Allowance" });
            List.Add(new OutputList { ID = "EGR1.3", Field = "Annual Allowance Pension", Value = Convert.ToString(getAnnualAllowancePension()), Group = "Annual Allowance" });
            List.Add(new OutputList { ID = "EGR2.1", Field = "Survivor Contingent Pension", Value = Convert.ToString(getSurvivorContingentPension()), Group = "Survivor Benefits" });
            List.Add(new OutputList { ID = "EGR2.2", Field = "PI on Contingent Survivor Pension", Value = Convert.ToString(getPIonSurvivorContingentPension()), Group = "Survivor Benefits" });
            List.Add(new OutputList { ID = "EGR2.3", Field = "Survivor LSI Pension", Value = Convert.ToString(getSurvivorLSIPension()), Group = "Survivor Benefits" });
            List.Add(new OutputList { ID = "EGR2.4", Field = "Survivor CPD Pension", Value = Convert.ToString(getSurvivorCPDPension()), Group = "Survivor Benefits" });
            List.Add(new OutputList { ID = "EGR2.5", Field = "Total Contingent Survivor Pension", Value = Convert.ToString(getTotalContingentSurvivorPension()), Group = "Survivor Benefits" });
            List.Add(new OutputList { ID = "EGR2.6", Field = "Survivor's Requisite Benefit Pension", Value = Convert.ToString(getSurvivorRequisiteBenefitPension()), Group = "Survivor Benefits" });

            return List;

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
        public double getSurvivorLSIPension()
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
       

