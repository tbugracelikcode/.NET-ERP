using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.Entities.Enums
{
    public enum CPRsManufacturingStepsEnum
    {
        [Display(Name = "AlphaVCheckEnum")]
        alphavcheck = 0,
        [Display(Name = "AnnealingEnum")]
        annealing = 1,
        [Display(Name = "AntiCorrosionEnum")]
        anticorrosion = 2,
        [Display(Name = "AOIEnum")]
        AOI = 3,
        [Display(Name = "ArcFormingEnum")]
        arcforming = 4,
        [Display(Name = "ArrayforSprayEnum")]
        arrayforspray = 5,
        [Display(Name = "ArrayonFixtureEnum")]
        arrayonfixture = 6,
        [Display(Name = "AssemblingEnum")]
        assembling = 7,



        [Display(Name = "BalancingEnum")]
        balancing = 8,
        [Display(Name = "BendingEnum")]
        bending = 9,
        [Display(Name = "BlankingEnum")]
        blanking = 10,
        [Display(Name = "BlendingEnum")]
        blending = 11,
        [Display(Name = "BondingEnum")]
        bonding = 12,

        [Display(Name = "BoringEnum")]
        boring = 13,
        [Display(Name = "BroachingEnum")]
        broaching = 14,
        [Display(Name = "BrushingEnum")]
        brushing = 15,
        [Display(Name = "CalibrationEnum")]
        calibration = 16,
        [Display(Name = "CarburizingEnum")]
        carburizing = 17,
        [Display(Name = "CastingEnum")]
        casting = 18,



        [Display(Name = "ChamferingEnum")]
        chamfering = 19,
        [Display(Name = "CleaningEnum")]
        cleaning = 20,
        [Display(Name = "CoatingEnum")]
        coating = 21,
        [Display(Name = "CoilingEnum")]
        coiling = 22,
        [Display(Name = "CompactingEnum")]
        compacting = 23,
        [Display(Name = "CondensationEnum")]
        condensation = 24,
        [Display(Name = "ConservationEnum")]
        conservation = 25,



        [Display(Name = "CoolingEnum")]
        cooling = 26,
        [Display(Name = "CoolingLubricantSystemEnum")]
        coolinglubricantsystem = 27,
        [Display(Name = "CrimpingEnum")]
        crimping = 28,
        [Display(Name = "CuringEnum")]
        curing = 29,
        [Display(Name = "CuttingEnum")]
        cutting = 30,
        [Display(Name = "DebindingEnum")]
        debinding = 31,


        [Display(Name = "DeburringEnum")]
        deburring = 32,
        [Display(Name = "DeepDrawingEnum")]
        deepdrawing = 33,
        [Display(Name = "DeflectionTestEnum")]
        deflectiontest = 34,
        [Display(Name = "DemagnetizeEnum")]
        demagnetize = 35,
        [Display(Name = "DephosphatingEnum")]
        dephosphating = 36,
        [Display(Name = "DetensioningEnum")]
        detensioning = 37,
        [Display(Name = "DisassemblingEnum")]
        disassembling = 38,



        [Display(Name = "DMCMarkingEnum")]
        DMCmarking = 39,
        [Display(Name = "DrillingEnum")]
        drilling = 40,
        [Display(Name = "DryingEnum")]
        drying = 41,
        [Display(Name = "DurabilityTestEnum")]
        durabilitytest = 42,
        [Display(Name = "ECoatingEnum")]
        ecoating = 43,
        [Display(Name = "EddyCurrentEnum")]
        eddycurrent = 44,
        [Display(Name = "EOLTestEnum")]
        EOLtest = 45,



        [Display(Name = "FineCuttingEnum")]
        finecutting = 46,
        [Display(Name = "FinishingEnum")]
        finishing = 47,
        [Display(Name = "ForgingEnum")]
        forging = 48,
        [Display(Name = "FormingEnum")]
        forming = 49,
        [Display(Name = "FrictionWeldingEnum")]
        frictionwelding = 50,
        [Display(Name = "FunctionalTestEnum")]
        functionaltest = 51,
        [Display(Name = "GatingRemovalEnum")]
        gatingremoval = 52,



        [Display(Name = "GBDEnum")]
        GBD = 53,
        [Display(Name = "GearingEnum")]
        gearing = 54,
        [Display(Name = "GlueEnum")]
        glue = 55,
        [Display(Name = "GreenMachiningEnum")]
        greenmachining = 56,
        [Display(Name = "GrindingIDEnum")]
        grindingID = 57,
        [Display(Name = "GrindingODEnum")]
        grindingOD = 58,
        [Display(Name = "HandlingEnum")]
        handling = 59,



        [Display(Name = "HAREnum")]
        HAR = 60,
        [Display(Name = "HardeningEnum")]
        hardening = 61,
        [Display(Name = "HeatForceSetTestgEnum")]
        heatforcesettest = 62,
        [Display(Name = "HeatTreatmentEnum")]
        heattreatment = 63,
        [Display(Name = "HighPressureWashingEnum")]
        highpreassurewashing = 64,
        [Display(Name = "HobbingEnum")]
        hobbing = 65,
        [Display(Name = "HoningEnum")]
        honing = 66,
        [Display(Name = "HotBarSolderingEnum")]
        hotbarsoldering = 67,



        [Display(Name = "HotRollingEnum")]
        hotrolling = 68,
        [Display(Name = "HotSettingEnum")]
        hotsetting = 69,
        [Display(Name = "HotStakingEnum")]
        hotstaking = 70,
        [Display(Name = "ICTestEnum")]
        ICtest = 71,
        [Display(Name = "InboundLogisticEnum")]
        inboundlogistic = 72,
        [Display(Name = "InduxtionHeatingEnum")]
        inductionheating = 73,
        [Display(Name = "InjectionEnum")]
        injection = 74,


        [Display(Name = "InspectionEnum")]
        inspection = 75,
        [Display(Name = "IntermediateTestEnum")]
        intermediatetest = 76,
        [Display(Name = "JetMillingEnum")]
        jetmilling = 77,
        [Display(Name = "LaserMarkingEnum")]
        lasermarking = 78,
        [Display(Name = "LeakageTestEnum")]
        leakagetest = 79,



        [Display(Name = "LiquidDispensingApplicationEnum")]
        liquiddispensingapplication = 80,
        [Display(Name = "LoadingFeedingEnum")]
        loadingfeeding = 81,
        [Display(Name = "MachiningEnum")]
        machining = 82,
        [Display(Name = "MagneticPropertyTestEnum")]
        magneticpropertytest = 83,
        [Display(Name = "MagnetizingEnum")]
        magnetizing = 84,
        [Display(Name = "MarkingEnum")]
        marking = 85,
        [Display(Name = "MaterialPreparationandCompoundingEnum")]
        materialpreparationandcompounding = 86,



        [Display(Name = "MeasuringEnum")]
        measuring = 87,
        [Display(Name = "MechanicalTreatmentEnum")]
        mechanicaltreatment = 88,
        [Display(Name = "MeltingEnum")]
        melting = 89,
        [Display(Name = "MicroPeeningEnum")]
        micropeening = 90,
        [Display(Name = "MillingEnum")]
        milling = 91,
        [Display(Name = "MixingEnum")]
        mixing = 92,
        [Display(Name = "MoldingEnum")]
        molding = 93,
        [Display(Name = "MPIEnum")]
        MPI = 94,


        [Display(Name = "NitridingEnum")]
        nitriding = 95,
        [Display(Name = "OilingEnum")]
        oiling = 96,
        [Display(Name = "OtherMiscellaneousEnum")]
        othersmiscellaneous = 97,
        [Display(Name = "OutboundLogisticEnum")]
        outboundlogistic = 98,
        [Display(Name = "OvermoldingEnum")]
        overmolding = 99,



        [Display(Name = "PackEnum")]
        pack = 100,
        [Display(Name = "PackagingEnum")]
        packaging = 101,
        [Display(Name = "PCBPanelingEnum")]
        PCBdepaneling = 102,
        [Display(Name = "PhospatingEnum")]
        phosphating = 103,
        [Display(Name = "PicklingEnum")]
        pickling = 104,
        [Display(Name = "PlatingEnum")]
        plating = 105,
        [Display(Name = "PolishingEnum")]
        polishing = 106,
        [Display(Name = "PostCuringEnum")]
        postcuring = 107,



        [Display(Name = "PottingEnum")]
        potting = 108,
        [Display(Name = "PowderCoatingEnum")]
        powdercoating = 109,
        [Display(Name = "PreformingEnum")]
        preforming = 110,
        [Display(Name = "PressFitAssemblyEnum")]
        pressfitassembly = 111,
        [Display(Name = "PressingEnum")]
        pressing = 112,
        [Display(Name = "PressureTestEnum")]
        pressuretest = 113,
        [Display(Name = "PretreatmentEnum")]
        pretreatment = 114,



        [Display(Name = "QuenchTemperingEnum")]
        quenchtempering = 115,
        [Display(Name = "RawMaterialInspectionEnum")]
        rawmaterialinspection = 116,
        [Display(Name = "ReamingEnum")]
        reaming = 117,
        [Display(Name = "ReflowOvenEnum")]
        reflowoven = 118,
        [Display(Name = "RollingEnum")]
        rolling = 119,
        [Display(Name = "SawingEnum")]
        sawing = 120,




        [Display(Name = "SelectiveSolderingEnum")]
        selectivesoldering = 121,
        [Display(Name = "SettingEnum")]
        setting = 122,
        [Display(Name = "ShearingEnum")]
        shearing = 123,
        [Display(Name = "ShotBlastingEnum")]
        shotblasting = 124,
        [Display(Name = "ShotPeeningEnum")]
        shotpeening = 125,
        [Display(Name = "SinteringEnum")]
        sintering = 126,
        [Display(Name = "SizingEnum")]
        sizing = 127,
        [Display(Name = "SkivingEnum")]
        skiving = 128,



        [Display(Name = "SMTEnum")]
        SMT = 129,
        [Display(Name = "SoftwareFlashingEnum")]
        softwareflashing = 130,
        [Display(Name = "SolderPastePrintingEnum")]
        solderpasteprinting = 131,
        [Display(Name = "SolderingEnum")]
        soldering = 132,
        [Display(Name = "SortingEnum")]
        sorting = 133,
        [Display(Name = "SpinningEnum")]
        spinning = 134,
        [Display(Name = "SpotWeldingEnum")]
        spotwelding = 135,
        [Display(Name = "SprayingEnum")]
        spraying = 136,



        [Display(Name = "SputteringPVDEnum")]
        sputteringPVD = 137,
        [Display(Name = "StampingEnum")]
        stamping = 138,
        [Display(Name = "StorageofPowderEnum")]
        storageofpowder = 139,
        [Display(Name = "StraighteningEnum")]
        straightening = 140,
        [Display(Name = "StripeCastingFlakesProductionEnum")]
        stripecastingflakesproduction = 141,
        [Display(Name = "SurfaceTreatmentEnum")]
        surfacetreatment = 142,
        [Display(Name = "TemperingEnum")]
        tempering = 143,



        [Display(Name = "TestingEnum")]
        testing = 144,
        [Display(Name = "TransformEnum")]
        transform = 145,
        [Display(Name = "TransportEnum")]
        transport = 146,
        [Display(Name = "TrimmingEnum")]
        trimming = 147,
        [Display(Name = "TumblingEnum")]
        tumbling = 148,
        [Display(Name = "TurningEnum")]
        turning = 149,
        [Display(Name = "UltrasonicCleaningEnum")]
        ultrasoniccleaning = 150,



        [Display(Name = "UltrasonicWeldingEnum")]
        ultrasonicwelding = 151,
        [Display(Name = "VarnishingEnum")]
        varnishing = 152,
        [Display(Name = "VulcanizationEnum")]
        vulcanization = 153,
        [Display(Name = "WashingEnum")]
        washing = 154,
        [Display(Name = "WeldingEnum")]
        welding = 155,
        [Display(Name = "WindingEnum")]
        winding = 156,


        [Display(Name = "XRayEnum")]
        xray = 157,
        [Display(Name = "OthersEnum")]
        others = 158,
    }
}
